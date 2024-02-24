using System.Collections.Concurrent;
using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using WireMock;
using WireMock.Admin.Mappings;
using WireMock.Matchers.Request;
using WireMock.Models;
using WireMock.Owin.Mappers.Providers;
using WireMock.ResponseProviders;
using WireMock.Serialization;
using WireMock.Settings;

namespace Wiremock.Net.MappingProviders.Cosmos;

internal class CosmosMappingProvider : IMappingProvider
{
    // Wiremock uses its mapping dictionary to store internal configuration as well.
    // This can contain complex function objects that won't serialise.
    // As this config is hardcoded we use a concurrent dictionary instead of Cosmos.
    private readonly ConcurrentDictionary<Guid, IMapping> _configStore = new();

    private readonly MappingConverter _mappingConverter;
    private readonly Container _container;
    private WireMockServerSettings? _serverSettings;

    private const string Database = "wiremock";
    private const string Mappings = "mappings";

    public CosmosMappingProvider(CosmosMappingProviderOptions options,
        MappingConverter mappingConverter)
    {
        _mappingConverter = mappingConverter;

        var client = new CosmosClient(connectionString: options.ConnectionString);

        var dbCreation = client.CreateDatabaseIfNotExistsAsync(id: Database).Result;
        if (dbCreation.StatusCode != HttpStatusCode.Created && dbCreation.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception("Cosmos Database could not be created");
        }

        var database = client.GetDatabase(Database);
        var containerCreation =
            database.CreateContainerIfNotExistsAsync(new ContainerProperties(Mappings, "/id")).Result;
        if (containerCreation.StatusCode != HttpStatusCode.Created && containerCreation.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception("Cosmos Container could not be created");
        }

        _container = database.GetContainer(Mappings);
    }

    public KeyValuePair<Guid, IMapping>[] ToArray()
    {
        return ReturnValues().Select(mapping =>
            new KeyValuePair<Guid, IMapping>(mapping.Guid, mapping)).ToArray();
    }

    public bool TryAdd(Guid key, IMapping mapping)
    {
        if (mapping.IsAdminInterface)
        {
            // Capture the server settings if we haven't already
            _serverSettings ??= mapping.Settings;

            // This is an internal mapping, use ConfigStore
            return _configStore.TryAdd(key, mapping);
        }

        var mappingToStore = (CosmosMappingModel)_mappingConverter.ToMappingModel(mapping);
        mappingToStore.Id = mappingToStore.Guid.ToString()!;

        var responseMessage = _container.CreateItemAsync(item: mappingToStore,
                partitionKey: new PartitionKey(key.ToString()))
            .Result;
        return responseMessage.StatusCode switch
        {
            HttpStatusCode.Accepted => true,
            _ => false
        };
    }

    public bool TryRemove(Guid key, out bool result)
    {
        result = _configStore.TryRemove(key, out _);
        if (result)
        {
            return result;
        }

        var responseMessage = _container.DeleteItemAsync<CosmosMappingModel>(
            partitionKey: new PartitionKey(key.ToString()),
            id: key.ToString()).Result;
        return responseMessage.StatusCode switch
        {
            _ => true
        };
    }

    public bool ContainsKey(Guid key)
    {
        // Check the internal config store first
        var result = _configStore.ContainsKey(key);
        if (result)
        {
            return result;
        }

        // Now check the cosmos container
        using var responseMessage = _container.ReadItemStreamAsync(
            partitionKey: new PartitionKey(key.ToString()),
            id: key.ToString()).Result;
        return responseMessage.StatusCode switch
        {
            HttpStatusCode.NotFound => false,
            _ => true
        };
    }

    public void Update(Guid key, IMapping mapping)
    {
        if (mapping.IsAdminInterface)
        {
            // This is an internal mapping, use ConfigStore
            _configStore[key] = mapping;
            return;
        }

        var mappingToStore = (CosmosMappingModel)_mappingConverter.ToMappingModel(mapping);
        mappingToStore.Id = mappingToStore.Guid.ToString()!;

        var responseMessage = _container.UpsertItemAsync(item: mappingToStore,
                partitionKey: new PartitionKey(key.ToString()))
            .Result;

        if (responseMessage.StatusCode != HttpStatusCode.Accepted)
        {
            throw new Exception("Could not update Mapping");
        }
    }

    public int Count => ReturnValues().Count;

    ICollection<IMapping> IMappingProvider.Values => ReturnValues();

    private List<IMapping> ReturnValues()
    {
        // Initialise the collection with internal config mappings
        var collection = _configStore.Select(mapping => mapping.Value)
            .ToList();

        // Now pull user mappings from cosmos
        var query = _container.GetItemLinqQueryable<CosmosMappingModel>();
        var iterator = query.ToFeedIterator();
        collection.AddRange(iterator.ReadNextAsync().Result
            .Select(userMapping => ToMapping(userMapping, _serverSettings!)).Cast<IMapping>());

        return collection;
    }

    private static Mapping ToMapping(MappingModel mapping, WireMockServerSettings settings)
    {
        return new Mapping(guid: mapping.Guid.GetValueOrDefault(),
            updatedAt: mapping.UpdatedAt.GetValueOrDefault(),
            title: mapping.Title,
            description: mapping.Description,
            path: mapping.Request.Path!.ToString(),
            settings: settings,
            requestMatcher: (IRequestMatcher)mapping.Request,
            provider: (IResponseProvider)mapping.Response,
            priority: mapping.Priority.GetValueOrDefault(),
            scenario: mapping.Scenario,
            executionConditionState: mapping.WhenStateIs,
            nextState: mapping.SetStateTo,
            stateTimes: null,
            webhooks: (IWebhook[])mapping.Webhooks,
            useWebhooksFireAndForget: mapping.UseWebhooksFireAndForget,
            timeSettings: (ITimeSettings)mapping.TimeSettings,
            data: mapping.Data);
    }
}
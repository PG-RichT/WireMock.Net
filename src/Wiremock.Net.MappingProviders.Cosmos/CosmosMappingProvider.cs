using System.Net;
using Microsoft.Azure.Cosmos;
using WireMock;
using WireMock.Owin.Mappers.Providers;

namespace Wiremock.Net.MappingProviders.Cosmos;

internal class CosmosMappingProvider : IMappingProvider
{
    private ICollection<IMapping> _values;

    private CosmosMappingProviderOptions? _options;

    private readonly Container _container;

    private const string Database = "wiremock";
    private const string Mappings = "mappings";

    public CosmosMappingProvider(CosmosMappingProviderOptions options)
    {
        _options = options;

        var client = new CosmosClient(connectionString: options.ConnectionString);

        var dbCreation = client.CreateDatabaseIfNotExistsAsync(id: Database).Result;
        if (dbCreation.StatusCode != HttpStatusCode.Created && dbCreation.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception("Cosmos Database could not be created");
        }

        var database = client.GetDatabase(Database);
        var containerCreation = database.CreateContainerIfNotExistsAsync(new ContainerProperties(Mappings, "/id")).Result;
        if (containerCreation.StatusCode != HttpStatusCode.Created && containerCreation.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception("Cosmos Container could not be created");
        }

        _container = database.GetContainer(Mappings);
    }

    public KeyValuePair<Guid, IMapping>[] ToArray()
    {
        throw new NotImplementedException();
    }

    public bool TryAdd(Guid key, IMapping mapping)
    {
        var newMapping = Map(mapping);

        var responseMessage = _container.CreateItemAsync(item: newMapping,
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
        throw new NotImplementedException();
    }

    public bool ContainsKey(Guid key)
    {
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
        throw new NotImplementedException();
    }

    public int Count { get; }

    ICollection<IMapping> IMappingProvider.Values => ReturnValues();

    private ICollection<IMapping> ReturnValues()
    {
        return new List<IMapping>();
    }

    private static CosmosMapping Map(IMapping mapping)
    {
        var mapped = new CosmosMapping(mapping.Guid,
            mapping.UpdatedAt,
            mapping.Title,
            mapping.Description,
            mapping.Path,
            mapping.Settings,
            mapping.RequestMatcher,
            mapping.Provider,
            mapping.Priority,
            mapping.Scenario,
            mapping.ExecutionConditionState,
            mapping.NextState,
            mapping.StateTimes,
            mapping.Webhooks,
            mapping.UseWebhooksFireAndForget,
            mapping.TimeSettings,
            mapping.Data);

        return mapped;
    }
}
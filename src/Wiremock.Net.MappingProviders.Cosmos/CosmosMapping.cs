using Newtonsoft.Json;
using WireMock;
using WireMock.Matchers.Request;
using WireMock.Models;
using WireMock.ResponseProviders;
using WireMock.Settings;

namespace Wiremock.Net.MappingProviders.Cosmos;

public class CosmosMapping : Mapping
{
    [JsonProperty("id")]
    public string Id { get; }

    public CosmosMapping(Guid guid,
        DateTime? updatedAt,
        string? title,
        string? description,
        string? path,
        WireMockServerSettings settings,
        IRequestMatcher requestMatcher,
        IResponseProvider provider,
        int priority,
        string? scenario,
        string? executionConditionState,
        string? nextState,
        int? stateTimes,
        IWebhook[]? webhooks,
        bool? useWebhooksFireAndForget,
        ITimeSettings? timeSettings,
        object? data) : base(guid,
        updatedAt,
        title,
        description,
        path,
        settings,
        requestMatcher,
        provider,
        priority,
        scenario,
        executionConditionState,
        nextState,
        stateTimes,
        webhooks,
        useWebhooksFireAndForget,
        timeSettings,
        data)
    {
        Id = guid.ToString();
    }
}
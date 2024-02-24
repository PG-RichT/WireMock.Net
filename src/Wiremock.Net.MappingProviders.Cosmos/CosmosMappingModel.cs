using Microsoft.AspNetCore.Server.Kestrel.Transport.Quic;
using Newtonsoft.Json;
using WireMock.Admin.Mappings;

namespace Wiremock.Net.MappingProviders.Cosmos;

public class CosmosMappingModel : MappingModel
{
    [JsonProperty("id")]
    public string Id { get; set; }
}
using WireMock.Owin.Mappers.Providers;

namespace Wiremock.Net.MappingProviders.Cosmos;

/// <summary>
/// Options for the Cosmos Mapping Provider
/// </summary>
public class CosmosMappingProviderOptions : IMappingProviderOptions
{
    /// <summary>
    /// Connection string to the Cosmos Database
    /// </summary>
    public string? ConnectionString { get; set; }
}
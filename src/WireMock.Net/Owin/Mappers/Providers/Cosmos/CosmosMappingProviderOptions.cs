namespace WireMock.Owin.Mappers.Providers.Cosmos;

/// <summary>
/// Options for the Cosmos Mapping Provider
/// </summary>
internal class CosmosMappingProviderOptions
{
    /// <summary>
    /// Connection string to the Cosmos Database
    /// </summary>
    public string? ConnectionString { get; set; }
}
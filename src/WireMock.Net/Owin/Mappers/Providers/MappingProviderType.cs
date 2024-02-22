namespace WireMock.Owin.Mappers.Providers;

/// <summary>
/// Type of Mapping Provider
/// </summary>
public enum MappingProviderType
{
    /// <summary>
    /// Legacy Mapping Provider, uses original concurrent dictionary implementation
    /// </summary>
    Legacy,
    /// <summary>
    /// Use Cosmos database as mapping provider
    /// </summary>
    Cosmos
}
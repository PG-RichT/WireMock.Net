﻿namespace WireMock.Owin.Mappers.Providers;

/// <summary>
/// Type of Mapping Provider
/// </summary>
public enum MappingProviderType
{
    /// <summary>
    /// Default Mapping Provider, uses original concurrent dictionary implementation
    /// </summary>
    Default,
    /// <summary>
    /// Use Cosmos database as mapping provider
    /// </summary>
    Cosmos
}
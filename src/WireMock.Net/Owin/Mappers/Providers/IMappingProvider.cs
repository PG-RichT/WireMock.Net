using System;
using System.Collections.Generic;

namespace WireMock.Owin.Mappers.Providers;

/// <summary>
/// Interface for Mapping Providers
/// </summary>
public interface IMappingProvider
{
    /// <summary>
    /// Mappings stored in the provider
    /// </summary>
    ICollection<IMapping> Values { get; }

    /// <summary>
    /// Return stored mappings as an array
    /// </summary>
    KeyValuePair<Guid,IMapping>[] ToArray();

    /// <summary>
    /// Try and add mapping with provided key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="mapping"></param>
    bool TryAdd(Guid key, IMapping mapping);

    /// <summary>
    /// Try and remove mapping with provided key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="result"></param>
    bool TryRemove(Guid key, out bool result);

    /// <summary>
    /// Does the provider have a mapping matching the provided key?
    /// </summary>
    /// <param name="key"></param>
    bool ContainsKey(Guid key);

    /// <summary>
    /// Update the mapping matching the provided key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="mapping"></param>
    void Update(Guid key, IMapping mapping);

    /// <summary>
    /// Returns the number of mappings stored
    /// </summary>
    /// <returns></returns>
    int Count { get; }
}
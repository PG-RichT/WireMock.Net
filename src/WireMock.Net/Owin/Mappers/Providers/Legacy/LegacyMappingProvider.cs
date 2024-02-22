using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WireMock.Owin.Mappers.Providers.Legacy;

internal class LegacyMappingProvider : IMappingProvider
{
    private ConcurrentDictionary<Guid, IMapping> ValueStore { get; } = new();
    public ICollection<IMapping> Values => ValueStore.Values;
    public KeyValuePair<Guid, IMapping>[] ToArray()
    {
        return ValueStore.ToArray();
    }

    public bool TryAdd(Guid key, IMapping mapping)
    {
        return ValueStore.TryAdd(key, mapping);
    }

    public bool TryRemove(Guid key, out bool result)
    {
        return result = ValueStore.TryRemove(key, out _);
    }

    public bool ContainsKey(Guid key)
    {
        return ValueStore.ContainsKey(key);
    }

    public void Update(Guid key, IMapping mapping)
    {
        ValueStore[key] = mapping;
    }
}
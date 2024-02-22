using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WireMock.Owin.Mappers.Providers.Cosmos;

internal class CosmosMappingProvider : IMappingProvider
{
    private ICollection<IMapping> _values;
    public ConcurrentDictionary<Guid, IMapping> Values { get; }
    public KeyValuePair<Guid, IMapping>[] ToArray()
    {
        throw new NotImplementedException();
    }

    public bool TryRemove(Guid key, out bool result)
    {
        throw new NotImplementedException();
    }

    public bool ContainsKey(Guid key)
    {
        throw new NotImplementedException();
    }

    ICollection<IMapping> IMappingProvider.Values => _values;
}
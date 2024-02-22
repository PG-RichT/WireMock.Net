using System;
using System.Collections.Generic;

namespace WireMock.Owin.Mappers.Providers;

internal interface IMappingProvider
{
    ICollection<IMapping> Values { get; }
    KeyValuePair<Guid,IMapping>[] ToArray();
    bool TryAdd(Guid key, IMapping mapping);
    bool TryRemove(Guid key, out bool result);
    bool ContainsKey(Guid key);
    void Update(Guid key, IMapping mapping);
}
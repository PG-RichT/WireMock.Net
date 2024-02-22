using System;
using System.Collections.Generic;

namespace WireMock.Owin.Mappers.Providers;

internal interface IMappingProvider
{
    ICollection<IMapping> Values { get; }
    KeyValuePair<Guid,IMapping>[] ToArray();
    bool TryRemove(Guid key, out bool result);
    bool ContainsKey(Guid key);
}
using WireMock;
using WireMock.Owin.Mappers.Providers;

namespace Wiremock.Net.MappingProviders.Cosmos;

internal class CosmosMappingProvider : IMappingProvider
{
    private ICollection<IMapping> _values;

    private CosmosMappingProviderOptions? _options;

    public CosmosMappingProvider(CosmosMappingProviderOptions options)
    {
        _options = options;
    }

    public KeyValuePair<Guid, IMapping>[] ToArray()
    {
        throw new NotImplementedException();
    }

    public bool TryAdd(Guid key, IMapping mapping)
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

    public void Update(Guid key, IMapping mapping)
    {
        throw new NotImplementedException();
    }

    public int Count { get; }

    ICollection<IMapping> IMappingProvider.Values => _values;
}
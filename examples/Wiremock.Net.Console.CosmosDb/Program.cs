using Wiremock.Net.MappingProviders.Cosmos;
using WireMock.Owin.Mappers.Providers;
using WireMock.Server;
using WireMock.Settings;

namespace Wiremock.Net.Console.CosmosDb;

internal class Program
{
    private static void Main(string[] args)
    {
        var settings = new WireMockServerSettings
        {
            Port = 5000,
            StartAdminInterface = true,
            MaxRequestLogCount = 20,
            MappingProviderType = MappingProviderType.Cosmos,
            MappingProviderOptions = new CosmosMappingProviderOptions
            {
                ConnectionString =
                    "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
            }
        };

        var server = WireMockServer.Start(settings);
    }
}
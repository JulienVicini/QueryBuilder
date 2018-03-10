using System.IO;
using Microsoft.Extensions.Configuration;

namespace QueryBuilder.Samples.EFCore.Helpers
{
    public static class ConfigurationManager
    {
        private static readonly IConfiguration _configuration;

        static ConfigurationManager()
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json");

            _configuration = builder.Build();
        }

        public static string SampleDbContextConnectionString => _configuration.GetConnectionString("SampleDbContext");

    }
}

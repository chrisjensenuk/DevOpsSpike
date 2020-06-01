using DevOpsSpike;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace DevOpsSpike
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var env = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");

            var configBuilder = new ConfigurationBuilder()
                .AddEnvironmentVariables();

            if (env == "Development")
            {
                configBuilder.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: false);
            }

            var config = configBuilder
               .Build();

            builder.Services.AddSingleton<IConfiguration>(config);
        }
    }
}

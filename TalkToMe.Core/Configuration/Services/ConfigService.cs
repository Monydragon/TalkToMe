using Microsoft.Extensions.Configuration;

namespace TalkToMe.Core.Configuration.Services;

public class ConfigService
{
    public IConfigurationRoot LoadConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        return builder.Build();
    }
}
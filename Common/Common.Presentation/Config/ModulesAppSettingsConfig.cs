using Microsoft.Extensions.Configuration;

namespace Common.Presentation.Config;

public static class ModulesAppSettingsConfig
{
    /// <summary>
    /// Adds configuration for specified modules to the provided <see cref="IConfigurationBuilder"/>.
    /// By default, this method loads JSON configuration files for each module, including environment-specific configuration files.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="IConfigurationBuilder"/> to which the module configurations will be added.
    /// </param>
    public static void AddModulesConfiguration(this IConfigurationBuilder builder)
    {
            builder.AddJsonFile("appsettings.json", true, true);
            builder.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);
        
    }
}
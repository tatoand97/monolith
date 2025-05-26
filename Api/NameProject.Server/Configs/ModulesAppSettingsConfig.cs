namespace NameProject.Server.Configs;

internal static class ModulesAppSettingsConfig
{
    /// <summary>
    /// Adds configuration for specified modules to the provided <see cref="IConfigurationBuilder"/>.
    /// By default, this method loads JSON configuration files for each module, including environment-specific configuration files.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="IConfigurationBuilder"/> to which the module configurations will be added.
    /// </param>
    /// <param name="modules">
    /// An array of module names for which the configuration files should be loaded.
    /// Each module's configuration is expected in files named "appsettings.{module}.json" and
    /// "appsettings.{module}.{Environment}.json", where {Environment} is derived from the current ASP.NET Core environment.
    /// </param>
    internal static void AddModulesConfiguration(this IConfigurationBuilder builder, string[] modules)
    {
        foreach (var module in modules)
        {
            builder.AddJsonFile($"appsettings.{module}.json", true, true);
            builder.AddJsonFile($"appsettings.{module}.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);
        }
    }
}
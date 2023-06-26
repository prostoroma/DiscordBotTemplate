using System.Reflection;
using DiscordBot.Permissions.Features.Configuration;
using DiscordBot.Permissions.Features.Functions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DiscordBot.Permissions.Extensions;

public static class ServiceCollectionExtensions
{
    private static readonly string PermissionsModulesConfigPath =
        Path.Combine(Directory.GetCurrentDirectory(), "interaction_modules.json");

    private static IServiceCollection AddPermissionConfig(this IServiceCollection serviceCollection)
    {
        const string ConfigurationPath = "permission_groups.json";
        try
        {
            var config = PermissionsConfig.GetPermissionsConfig();

            serviceCollection.AddSingleton(config);
            
            return serviceCollection;
        }
        catch (FileNotFoundException)
        {
            PermissionsConfig.CreateDefaultPermissionsConfig();

            var config = PermissionsConfig.GetPermissionsConfig();

            serviceCollection.AddSingleton(config);
            
            return serviceCollection;
        }
    }

    private static IServiceCollection AddInteractionModulesConfig(this IServiceCollection serviceCollection)
    {
        var availableModules = Helpers.GetAvailableModules(Assembly.GetEntryAssembly()!);

        if (!File.Exists(PermissionsModulesConfigPath))
        {
            var modulesInstances = Helpers.RegisterModules(availableModules);

            var interactionModulesConfig = new InteractionModulesConfig(modulesInstances);
            var json = JsonConvert.SerializeObject(interactionModulesConfig, Formatting.Indented);

            File.WriteAllText(PermissionsModulesConfigPath, json);

            return serviceCollection.AddSingleton(interactionModulesConfig);
        }

        var fileContent = File.ReadAllText(PermissionsModulesConfigPath);
        var config = JsonConvert.DeserializeObject<InteractionModulesConfig>(fileContent);

        if (config == null)
            throw new NullReferenceException("Не удалось получить список доступных модулей");

        var configuredModules = config.InteractionModules.ToList();

        if (availableModules.ToList().Count() != configuredModules.Count)
        {
            Helpers.ConfigureModules(availableModules, configuredModules);

            var orderedModules = configuredModules
                .OrderByDescending(x => x.RequiredPermissions);

            var interactionModulesConfig = new InteractionModulesConfig(orderedModules);

            var json = JsonConvert.SerializeObject(interactionModulesConfig, Formatting.Indented);

            File.WriteAllText(PermissionsModulesConfigPath, json);

            return serviceCollection.AddSingleton(interactionModulesConfig);
        }

        return serviceCollection.AddSingleton(config);
    }

    public static IServiceCollection UsePermissions(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddPermissionConfig();
        serviceCollection.AddInteractionModulesConfig();

        return serviceCollection;
    }
}
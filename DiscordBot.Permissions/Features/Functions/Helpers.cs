using System.Reflection;
using DiscordBot.Permissions.Attributes;
using DiscordBot.Permissions.Features.Configuration;
using DiscordBot.Permissions.Features.Entities;

namespace DiscordBot.Permissions.Features.Functions;

public static class Helpers
{
    internal static IEnumerable<Type> GetAvailableModules(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(type => type.GetCustomAttribute<ProtectedModule>() != null)
            .ToList();
    }

    internal static IEnumerable<InteractionModule> RegisterModules(IEnumerable<Type> availableModules)
    {
        return availableModules
            .Select(module => new InteractionModule(true, module.Name, 255, null))
            .ToList();
    }

    public static IEnumerable<Type> GetEnabledModules(Assembly assembly, InteractionModulesConfig config)
    {
        return GetAvailableModules(assembly)
            .Where(type => type.GetCustomAttribute<ProtectedModule>()!
                .IsModuleEnabled(config))
            .ToList();
    }

    internal static IEnumerable<InteractionModule> ConfigureModules(IEnumerable<Type> availableModules,
        List<InteractionModule> configuredModules)
    {
        var missingModules = new List<InteractionModule>();
        foreach (var module in availableModules)
        {
            if (configuredModules.Any(x => x.Name == module.Name))
                continue;

            missingModules.Add(new InteractionModule(true, module.Name, 255, null));
        }

        foreach (var module in new List<InteractionModule>(configuredModules))
        {
            if (availableModules.Any(x => x.Name == module.Name))
                continue;

            configuredModules.Remove(module);
        }

        configuredModules.AddRange(missingModules);

        return configuredModules;
    }
}
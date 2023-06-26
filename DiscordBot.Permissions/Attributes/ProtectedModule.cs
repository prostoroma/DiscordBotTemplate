using Discord;
using Discord.Interactions;
using DiscordBot.Permissions.Extensions;
using DiscordBot.Permissions.Features.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.Permissions.Attributes;

public class ProtectedModule : PreconditionAttribute
{
    private readonly string _moduleName;

    public ProtectedModule(string moduleName)
    {
        _moduleName = moduleName;
    }

    public bool IsModuleEnabled(InteractionModulesConfig interactionModulesConfig)
    {
        var slashCommand = interactionModulesConfig.InteractionModules
            .FirstOrDefault(x => x.Name == _moduleName);

        return slashCommand is { IsEnabled: true };
    }

    public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context,
        ICommandInfo commandInfo, IServiceProvider services)
    {
        var interactionModulesConfig = services.GetRequiredService<InteractionModulesConfig>()!;
        var moduleName = commandInfo.Module.Name;

        var requiredPermissions = interactionModulesConfig.InteractionModules
            .FirstOrDefault(x => x.Name == moduleName);

        if (requiredPermissions == null)
            throw new NullReferenceException("Не найдена установка прав для команды");

        var permissionConfig = PermissionsConfig.GetPermissionsConfigAsync();

        if (context.User is not IGuildUser guildUser)
            throw new NullReferenceException("Пользователь не найден");

        var hasPermission = guildUser.HasRequiredPermission(requiredPermissions, permissionConfig);

        return Task.FromResult(hasPermission
            ? PreconditionResult.FromSuccess()
            : PreconditionResult.FromError("У Вас недостаточно прав!"));
    }
}
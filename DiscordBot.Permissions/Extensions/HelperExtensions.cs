using Discord;
using DiscordBot.Permissions.Features;
using DiscordBot.Permissions.Features.Configuration;
using DiscordBot.Permissions.Features.Entities;

namespace DiscordBot.Permissions.Extensions;

internal static class HelperExtensions
{
    internal static bool HasRequiredPermission(this IGuildUser guildUser, InteractionModule interactionModule,
        PermissionsConfig permissionsConfig)
    {
        return guildUser.RoleIds.Any(roleId =>
            interactionModule.RolesIdOverride.Contains(roleId) ||
            permissionsConfig.HasPermission(roleId, interactionModule.RequiredPermissions));
    }
}
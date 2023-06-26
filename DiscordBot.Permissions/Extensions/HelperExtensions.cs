using Discord;
using DiscordBot.Permissions.Features;
using DiscordBot.Permissions.Features.Configuration;
using DiscordBot.Permissions.Features.Entities;

namespace DiscordBot.Permissions.Extensions;

internal static class HelperExtensions
{
    internal static bool HasRequiredPermission(this IGuildUser guildUser, InteractionModule requiredPermissions,
        PermissionsConfig permissionsConfig)
    {
        return guildUser.RoleIds.Any(roleId =>
            requiredPermissions.RolesIdOverride.Contains(roleId) ||
            permissionsConfig.HasPermission(roleId, requiredPermissions.RequiredPermissions));
    }
}
using DiscordBot.Permissions.Features.Configuration;

namespace DiscordBot.Permissions.Features;

public static class PermissionManager
{
    public static bool HasPermission(this PermissionsConfig config, ulong roleId, byte requiredPermissions)
    {
        var roleRecord = config.PermissionRecords.FirstOrDefault(x => x.RoleId == roleId);

        if (roleRecord != null) return roleRecord.PermissionLevel >= requiredPermissions;

        return false;
    }
}
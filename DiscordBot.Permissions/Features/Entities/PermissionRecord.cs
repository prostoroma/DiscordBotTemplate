namespace DiscordBot.Permissions.Features.Entities;

public class PermissionRecord
{
    public PermissionRecord()
    {
    }

    public PermissionRecord(ulong roleId, byte permissionLevel)
    {
        RoleId = roleId;
        PermissionLevel = permissionLevel;
    }

    public ulong RoleId { get; init; }

    public byte PermissionLevel { get; init; }
}
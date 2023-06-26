namespace DiscordBot.Permissions.Features.Entities;

public class InteractionModule
{
    public InteractionModule()
    {
    }

    public InteractionModule(bool isEnabled, string name, byte requiredPermissions, ulong[]? rolesIdOverride)
    {
        IsEnabled = isEnabled;
        Name = name;
        RequiredPermissions = requiredPermissions;
        RolesIdOverride = rolesIdOverride ?? Array.Empty<ulong>();
    }

    public bool IsEnabled { get; init; }

    public string Name { get; init; }

    public byte RequiredPermissions { get; init; }

    public ulong[] RolesIdOverride { get; init; }
}
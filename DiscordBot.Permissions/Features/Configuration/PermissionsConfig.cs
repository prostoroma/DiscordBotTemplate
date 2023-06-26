using DiscordBot.Permissions.Features.Entities;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DiscordBot.Permissions.Features.Configuration;

public class PermissionsConfig
{
    private static readonly string PermissionConfigPath = "permission_groups.yml";

    private static readonly IDeserializer Deserializer;

    private static readonly ISerializer Serializer;

    static PermissionsConfig()
    {
        Deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        Serializer = new SerializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
    }

    public PermissionsConfig()
    {
    }

    private PermissionsConfig(List<PermissionRecord> permissionRecords)
    {
        PermissionRecords = permissionRecords;
    }

    public List<PermissionRecord> PermissionRecords { get; private init; }

    public static PermissionsConfig GetPermissionsConfigAsync()
    {
        if (!File.Exists(PermissionConfigPath))
            throw new FileNotFoundException("Файл конфигурации не найден!");

        var fileContent = File.ReadAllText(PermissionConfigPath);

        var permissionsConfig = Deserializer.Deserialize<PermissionsConfig>(fileContent);

        return permissionsConfig;
    }

    public static void CreateDefaultPermissionsConfigAsync()
    {
        var defaultPermissionsConfig = new PermissionsConfig(
            new List<PermissionRecord>
            {
                new(548784278306160640, 255),
                new(541231234674567568, 44),
                new(548786867861678456, 23),
                new(5487842783867844323, 12)
            });

        File.WriteAllText(PermissionConfigPath, Serializer.Serialize(defaultPermissionsConfig));
    }
}
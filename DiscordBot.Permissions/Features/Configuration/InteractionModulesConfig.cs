using DiscordBot.Permissions.Features.Entities;

namespace DiscordBot.Permissions.Features.Configuration;

public class InteractionModulesConfig
{
    public InteractionModulesConfig()
    {
    }

    public InteractionModulesConfig(IEnumerable<InteractionModule> interactionModules)
    {
        InteractionModules = interactionModules;
    }

    public IEnumerable<InteractionModule> InteractionModules { get; init; }
}
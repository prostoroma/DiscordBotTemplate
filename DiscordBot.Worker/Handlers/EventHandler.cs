using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.WebSocket;

namespace DiscordBotTemplate.Handlers;

public class EventHandler : DiscordClientService
{
    public EventHandler(DiscordSocketClient client, ILogger<DiscordClientService> logger) 
        : base(client, logger)
    {
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Client.WaitForReadyAsync(stoppingToken);

        // Client.Event...
    }
}
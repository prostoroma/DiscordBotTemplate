using System.Diagnostics;
using System.Net.NetworkInformation;
using Discord.Interactions;
using DiscordBot.Permissions.Attributes;

namespace DiscordBotTemplate.Modules.General;

[ProtectedModule(nameof(PingCommand))]
public class PingCommand : InteractionBase
{
    [SlashCommand("ping", "pong")]
    public async Task ExecuteCommandAsync()
    {
        var stopwatch = Stopwatch.StartNew();

        await RespondAsync($"Pong! Websocket latency: **{Context.Client.Latency}ms**", ephemeral: true);
        
        await ModifyOriginalResponseAsync(x =>
        {
            var oldContent = x.Content.GetValueOrDefault($"Pong! Latency: **{Context.Client.Latency}ms**");
            x.Content = oldContent + $"\nMessage sent in **{stopwatch.ElapsedMilliseconds}ms**";
        });

        stopwatch.Stop();
    }
}
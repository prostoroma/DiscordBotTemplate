using System.Reflection;
using Discord;
using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordBot.Permissions.Features.Configuration;

namespace DiscordBotTemplate.Handlers;

internal class InteractionHandler : DiscordClientService
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;
    private readonly InteractionService _interactionService;
    private readonly IServiceProvider _provider;

    public InteractionHandler(DiscordSocketClient client, ILogger<DiscordClientService> logger,
        IServiceProvider provider, InteractionService interactionService, IHostEnvironment environment,
        IConfiguration configuration) : base(client, logger)
    {
        _provider = provider;
        _interactionService = interactionService;
        _environment = environment;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Client.InteractionCreated += OnInteractionCreated;

        _interactionService.SlashCommandExecuted += OnSlashCommandExecuted;
        _interactionService.ContextCommandExecuted += OnContextCommandExecuted;
        _interactionService.ComponentCommandExecuted += OnComponentCommandExecuted;

        var interactionConfig = _provider.GetService<InteractionModulesConfig>();
        
        if (interactionConfig != null)
        {
            Logger.LogInformation("Будет использована система защиты команд с помощью настроенных прав");
            
            var enabledModules = DiscordBot.Permissions.Features.Functions.Helpers.GetEnabledModules(Assembly.GetEntryAssembly()!, interactionConfig);

            foreach (var module in enabledModules)
            {
                await _interactionService.AddModuleAsync(module, _provider);
                Logger.LogInformation("Включен модуль {0}", module.Name);
            }
        }
        else
        {
            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

        await Client.WaitForReadyAsync(stoppingToken);

        if (_environment.IsDevelopment())
            await _interactionService.RegisterCommandsToGuildAsync(_configuration.GetValue<ulong>("Discord:Guild"));
        // else
        //     await _interactionService.RegisterCommandsGloballyAsync();
    }

    private Task OnComponentCommandExecuted(ComponentCommandInfo commandInfo, IInteractionContext context, IResult result)
    {
        if (result.IsSuccess) return Task.CompletedTask;
        
        if (!context.Interaction.HasResponded)
        {
            context.Interaction.RespondAsync($"Произошла ошибка: `{result.ErrorReason}`");
        }
        
        switch (result.Error)
        {
            case InteractionCommandError.UnmetPrecondition:
                // implement
                break;
            case InteractionCommandError.UnknownCommand:
                // implement
                break;
            case InteractionCommandError.BadArgs:
                // implement
                break;
            case InteractionCommandError.Exception:
                // implement
                break;
            case InteractionCommandError.Unsuccessful:
                // implement
                break;
        }

        return Task.CompletedTask;
    }

    private Task OnContextCommandExecuted(ContextCommandInfo commandInfo, IInteractionContext context, IResult result)
    {
        if (result.IsSuccess) return Task.CompletedTask;
        
        if (!context.Interaction.HasResponded)
        {
            context.Interaction.RespondAsync($"Произошла ошибка: `{result.ErrorReason}`");
        }
        
        switch (result.Error)
        {
            case InteractionCommandError.UnmetPrecondition:
                // implement
                break;
            case InteractionCommandError.UnknownCommand:
                // implement
                break;
            case InteractionCommandError.BadArgs:
                // implement
                break;
            case InteractionCommandError.Exception:
                // implement
                break;
            case InteractionCommandError.Unsuccessful:
                // implement
                break;
        }

        return Task.CompletedTask;
    }

    private Task OnSlashCommandExecuted(SlashCommandInfo commandInfo, IInteractionContext context, IResult result)
    {
        if (result.IsSuccess) return Task.CompletedTask;
        
        if (!context.Interaction.HasResponded)
        {
            context.Interaction.RespondAsync($"Произошла ошибка: `{result.ErrorReason}`");
        }
        
        switch (result.Error)
        {
            case InteractionCommandError.UnmetPrecondition:
                // implement
                break;
            case InteractionCommandError.UnknownCommand:
                // implement
                break;
            case InteractionCommandError.BadArgs:
                // implement
                break;
            case InteractionCommandError.Exception:
                // implement
                break;
            case InteractionCommandError.Unsuccessful:
                // implement
                break;
        }

        return Task.CompletedTask;
    }


    private async Task OnInteractionCreated(SocketInteraction arg)
    {
        try
        {
            var ctx = new SocketInteractionContext(Client, arg);
            await _interactionService.ExecuteCommandAsync(ctx, _provider);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Произошла внутреняя ошибка при выполнении команды");

            if (arg.Type == InteractionType.ApplicationCommand)
            {
                var msg = await arg.GetOriginalResponseAsync();
                await msg.DeleteAsync();
            }
        }
    }
}
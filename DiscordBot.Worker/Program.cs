using Discord;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using DiscordBot.Database.Extensions;
using DiscordBot.Permissions.Extensions;
using DiscordBotTemplate.Handlers;
using Serilog;
using Serilog.Events;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Запуск бота...");

    var host = Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureDiscordHost((context, config) =>
        {
            config.SocketConfig = new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                GatewayIntents = GatewayIntents.All,
                LogGatewayIntentWarnings = false,
                AlwaysDownloadUsers = true,
                MessageCacheSize = 200,
            };
            
            config.Token = context.Configuration.GetValue<string>("Discord:Token")!;
            
            config.LogFormat = (message, exception) => 
                exception is not null 
                    ? $"{message.Source}: {message.Exception} | {message.Message}" 
                    : $"{message.Source}: {message.Message}";
        })
        .UseInteractionService((context, config) =>
        {
            config.LogLevel = LogSeverity.Info;
            config.UseCompiledLambda = true;
        })
        .ConfigureServices((context, services) =>
        {
            //services.AddDatabaseLayer(context.Configuration);
            services.UsePermissions();
            services.AddHostedService<InteractionHandler>();
        }).Build();

    await host.RunAsync();
    
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Произошла внутреняя ошибка. Бот уничтожен.");

    Console.ReadKey();
    
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
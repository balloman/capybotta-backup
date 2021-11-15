using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapybottaBot.Commands;
using CapybottaBot.Handlers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CapybottaBot;

public class Driver
{
    private DiscordSocketClient Client { get; }
    public static void Main(string[] args) => new Driver().MainAsync().GetAwaiter().GetResult();
    private CommandService Commands { get; }
    private CommandHandler CommandHandler { get; }
        
    private Driver()
    {
        Client = new DiscordSocketClient();
        Commands = new CommandService();
        CommandHandler = new CommandHandler(Client, Commands);
    }
        
    private async Task MainAsync()
    {
        Client.Log += Utils.Log;
        Client.Ready += OnReady;
        await Client.LoginAsync(TokenType.Bot, Utils.GetToken().Result);
        await Client.StartAsync();
        await CommandHandler.InstallCommandsAsync();
        await Task.Delay(-1);
    }

    private async Task OnReady()
    {
        IReadOnlyCollection<SocketGuild>? guilds = Client.Guilds;
        foreach (var guild in guilds)
        {
            //await guild.SystemChannel.SendMessageAsync("OKAY I PULL UP!!!!");
        }
    }
}
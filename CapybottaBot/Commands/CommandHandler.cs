using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace CapybottaBot.Commands
{
    public class CommandHandler
    {
        public const string COMMAND_PREFIX = "!capy ";
        private DiscordSocketClient Client { get; }
        private CommandService Cs { get; }

        public CommandHandler(DiscordSocketClient client, CommandService cs)
        {
            Client = client;
            Cs = cs;
        }

        public async Task InstallCommandsAsync()
        {
            Client.MessageReceived += HandleCommandAsync;
            await Cs.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            if (arg is not SocketUserMessage message) return;
            var argPos = 0;
            if (!message.HasStringPrefix(COMMAND_PREFIX, ref argPos)) return;
            var context = new SocketCommandContext(Client, message);
            await Cs.ExecuteAsync(context, argPos, null);
        }
    }
}

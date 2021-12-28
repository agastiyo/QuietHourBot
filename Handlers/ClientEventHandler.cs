using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using QuietHourBot.Databases;

namespace QuietHourBot.Handlers
{
    public class ClientEventHandler
    {

        public void SetUpEventHandler(DiscordClient Client)
        {
            Client.Heartbeated += HandleHeartbeat;
            Client.GetCommandsNext().CommandExecuted += LogCommandExecution;
        }

        private Task HandleHeartbeat(object sender, HeartbeatEventArgs e)
        {
            Console.WriteLine($"{e.Timestamp}: {e.Ping}");
            return Task.CompletedTask;
        }

        private Task LogCommandExecution(object sender, CommandExecutionEventArgs e)
        {
            ConsoleExt.ColorWriteLine($"{DateTime.Now}:\nSuccessfully executed command: {e.Command.Name}", ConsoleColor.Green);
            return Task.CompletedTask;
        }
    }
}

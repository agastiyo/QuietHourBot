using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QuietHourBot.Commands;
using QuietHourBot.Handlers;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace QuietHourBot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public ErrorHandler EHandler { get; private set; }
        public ClientEventHandler CEHandler { get; private set; }

        public async Task RunAsync()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = sr.ReadToEnd();

            var configJson = JsonConvert.DeserializeObject<Configjson>(json);

            var config = new DiscordConfiguration()
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };

            EHandler = new ErrorHandler();

            CEHandler = new ClientEventHandler();

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromSeconds(45)
            });

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = true,
                EnableMentionPrefix = true,
                EnableDefaultHelp = true //change this to false later to build your own help page
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<QuietHourCommands>();
            Commands.RegisterCommands<FunCommands>();
            Commands.RegisterCommands<TextCommands>();
            Commands.RegisterCommands<NoteCommands>();
            Commands.RegisterCommands<RPGCommands>();

            await Client.ConnectAsync();

            EHandler.SetUpCommandErrorHandler(Client);
            CEHandler.SetUpEventHandler(Client);

            await Task.Delay(-1);
        }

        private Task OnClientReady(object sender, ReadyEventArgs e)
        {
            Console.WriteLine("Ready");
            return Task.CompletedTask;
        }
    }
}

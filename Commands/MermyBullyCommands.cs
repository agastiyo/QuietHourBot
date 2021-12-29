using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using QuietHourBot.Databases;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;

namespace QuietHourBot.Commands {
    class MermyBullyCommands : BaseCommandModule {
        [Command("MermyMonitor")]
        [Description("Monitors for Mermy (Owner ONLY)")]
        [RequireOwner]
        public async Task MermyMonitor(CommandContext ctx) {
            ctx.Client.MessageCreated += ; //function here//

            Random random = new Random();
            string[] startup = { "Anti-Mermy radar active", "Searching for Mermy...", "Anti-Mermy radar online" };
            await ctx.Channel.SendMessageAsync(person.Mention + startup[random.Next(0, startup.Length)]).ConfigureAwait(false);

            //do something to every message sent by mermy
       }
    }
}
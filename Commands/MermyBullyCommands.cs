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
        [Description("Monitors for Mermy (Owner ONLY). To stop monitoring, type 'leave mermy alone'")]
        [RequireOwner]
        public async Task MermyMonitor(CommandContext ctx, [Description("Mermy's handle")] [RemainingText] DiscordMember mermy) {
            ctx.Client.MessageCreated += Reply; //Sub to function

            string[] startup = { "Anti-Mermy radar active", "Searching for Mermy...", "Anti-Mermy radar online" };
            await ctx.Channel.SendMessageAsync(startup.PickRandomly()).ConfigureAwait(false);

            async Task Reply(object sender, MessageCreateEventArgs e) {
                if (e.Author == mermy) {
                    string[] replies = { "Ok, Mermy whatEVER", "ok but did I ask tho", "Mermy, this is SERIOUS",
                                        "You talking mad for a bot that itsn't open source", }
                    await ctx.Channel.SendMessageAsync(replies.PickRandomly()).ConfigureAwait(false);
                }
                else if (e.Message.Content == "leave mermy alone" && e.Author == ctx.Member) {
                    ctx.Client.MessageCreated -= Reply; //unsub from function and shut down
                    string[] shutdown = { "Mermy is safe..... for now", "I'll get Mermy next time", 
                                        "I need to be lucky only once, Mermy needs to be lucky every time"};
                    await ctx.Channel.SendMessageAsync(shutdown.PickRandomly()).ConfigureAwait(false);
                }
                else {
                    //ignore message
                }
            }
       }
    }
}
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using QuietHourBot.Databases;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuietHourBot
{
    public class QuietHourCommands : BaseCommandModule
    {
        [Command("quiet")]
        [RequireOwner]
        [Hidden]
        public async Task Monitor(CommandContext ctx)
        {
            //gets the database for quiet hour
            var db = new QuietHourDB();
            db.violationLog.Clear();

            ctx.Client.MessageCreated += Delete; //sub to message created

            await ctx.Channel.SendMessageAsync("monitoring").ConfigureAwait(false);//confirm            

            async Task Delete(object sender, MessageCreateEventArgs e)
            {
                if (e.Message.Content == "loud" && e.Author == ctx.Member) //end task
                {
                    //cancel monitoring
                    ctx.Client.MessageCreated -= Delete;
                    await ctx.Channel.SendMessageAsync("stopped monitoring").ConfigureAwait(false);

                    //report violations
                    var embedBuilder = new DiscordEmbedBuilder
                    {
                        Title = "Violations:",
                        Color = DiscordColor.DarkRed,
                    };

                    foreach (KeyValuePair<DiscordMember, int> violation in db.violationLog)
                    {
                        embedBuilder.AddField($"@{violation.Key.DisplayName}:", $"{violation.Value} violations");
                    }

                    await e.Channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);
                }

                else if (e.Author == ctx.Client.CurrentUser || e.Guild != ctx.Message.Channel.Guild) /*ignore message*/ {/*do nothing*/}

                else //log violation
                {
                    //Notice the message, get the violator
                    Console.WriteLine("see one");
                    DiscordMessage message = e.Message;
                    DiscordMember violator = (DiscordMember)message.Author;

                    //Delete the message
                    await message.DeleteAsync("Quiet Hour Violation").ConfigureAwait(false);

                    //storing their violation data in the dictionary
                    if (db.violationLog.ContainsKey(violator)) { db.violationLog[violator]++; }
                    else { db.violationLog.Add(violator, 1); }
                }
            }
        }
    }
}
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using QuietHourBot.Databases;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuietHourBot.Commands
{
    class TextCommands : BaseCommandModule
    {
        [Command("lithp")]
        [Description("lispifies your sentence")]
        public async Task Lisp(CommandContext ctx, [Description("Your sentence")][RemainingText] string sentence)
        {
            await ctx.Channel.SendMessageAsync(sentence.ToLisp()).ConfigureAwait(false);
        }

        [Command("expand")]
        [Description("expand (max 10 chars)")]
        public async Task Expand(CommandContext ctx, [Description("thing to expand")][RemainingText] string phrase)
        {
            if (phrase.Length > 10) { await ctx.Channel.SendMessageAsync("message is too long!"); }
            else
            {
                string message = "";
                for (int i = 1; i <= phrase.Length; i++)
                {
                    message += "\n" + phrase.Truncate(i);
                }
                for (int i = phrase.Length - 1; i >= 0; i--)
                {
                    message += "\n" + phrase.Truncate(i);
                }

                await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
            }
        }

        [Command("emojify")]
        [Description("Completely change your message to emojis")]
        public async Task Emojify(CommandContext ctx, [Description("the message you want to emojigy")][RemainingText] string message)
        {
            message = message.Trim().ToLower();
            string editedMessage = "";

            foreach (char character in message)
            {
                editedMessage += character.ToEmoji();
            }

            await ctx.Channel.SendMessageAsync(editedMessage).ConfigureAwait(false);
        }
    }
}


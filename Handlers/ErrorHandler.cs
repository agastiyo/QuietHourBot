using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using QuietHourBot.Databases;
using QuietHourBot.Exceptions;
using System;
using System.Threading.Tasks;
using DSharpPlus.Exceptions;

namespace QuietHourBot.Handlers
{
    public class ErrorHandler
    {
        public void SetUpCommandErrorHandler(DiscordClient Client)
        {
            Client.GetCommandsNext().CommandErrored += CommandErrorHandler;
        }

        private static async Task CommandErrorHandler(object sender, CommandErrorEventArgs e)
        {
            switch (e.Exception)
            {
                case AggregateException a:
                    await e.Context.Channel.SendMessageAsync($"A whole lot of things went wrong. You should recheck your input.").ConfigureAwait(false);
                    break;

                case ChecksFailedException a:
                    foreach (var failedCheck in a.FailedChecks)
                    {
                        switch (failedCheck)
                        {
                            case CooldownAttribute b:
                                await e.Context.Channel.SendMessageAsync($"Slow down! Wait for **{b.GetRemainingCooldown(a.Context).Seconds}** more seconds.").ConfigureAwait(false);
                                break;

                            case RequireBotPermissionsAttribute b:
                                await e.Context.Channel.SendMessageAsync($"You need {b.Permissions.ToPermissionString()} perms to use this command.").ConfigureAwait(false);
                                break;

                            case RequireOwnerAttribute b:
                                await e.Context.Channel.SendMessageAsync("Only mega chads can use this command. AKA my creator.").ConfigureAwait(false);
                                break;

                            default:
                                await e.Context.Channel.SendMessageAsync($"PATCH THIS IMMEDIATELY: {failedCheck}").ConfigureAwait(false);
                                break;
                        }
                    }
                    break;

                case CommandNotFoundException a:
                    await e.Context.Channel.SendMessageAsync($"The command {a.CommandName} does not exist. Please try again.").ConfigureAwait(false);
                    break;

                case ArgumentException a:
                    await e.Context.Channel.SendMessageAsync($"{a.Message} How about you read the help docs.").ConfigureAwait(false);
                    break;

                case NullReferenceException a:
                    await e.Context.Channel.SendMessageAsync($"{a.Message} How about you read the help docs.").ConfigureAwait(false);
                    break;

                case InvalidOverloadException a:
                    await e.Context.Channel.SendMessageAsync($"How about you read the help docs. The parameter {a.Parameter} was wrong.").ConfigureAwait(false);
                    break;

                case InvalidOperationException a:
                    await e.Context.Channel.SendMessageAsync($"{a.Message} I think you need to read the help docs.").ConfigureAwait(false);
                    break;

                case NotFoundException a:
                    await e.Context.Channel.SendMessageAsync($"Something you were looking for could not be found.").ConfigureAwait(false);
                    break;

                default:
                    await e.Context.Channel.SendMessageAsync($"PATCH THIS IMMEDIATELY: {e.Exception}").ConfigureAwait(false);
                    break;
            }

            ConsoleExt.ColorWriteLine($"{DateTime.Now}: {e.Exception}", ConsoleColor.Yellow);
        }

        /// <summary>
        /// Handles custom Exceptions
        /// </summary>
        /// <param name="ctx">Context fo the exception</param>
        /// <param name="e">The exception to handle</param>
        public void Handle(CommandContext ctx, Exception e)
        {
            switch (e)
            {
                //Note Exceptions
                case MoreThanOneNoteFoundException a:
                    ctx.Channel.SendMessageAsync(a.Message).ConfigureAwait(false);
                    break;

                case NoteNotFoundException a:
                    ctx.Channel.SendMessageAsync(a.Message).ConfigureAwait(false);
                    break;

                case NoteNotValidException a:
                    ctx.Channel.SendMessageAsync(a.Message).ConfigureAwait(false);
                    break;

                case TooManyNotesException a:
                    ctx.Channel.SendMessageAsync(a.Message).ConfigureAwait(false);
                    break;

                //RPG Exceptions
                case NotEnoughGoldException a:
                    ctx.Channel.SendMessageAsync(a.Message).ConfigureAwait(false);
                    break;

                case PlayerExistsException a:
                    ctx.Channel.SendMessageAsync(a.Message).ConfigureAwait(false);
                    break;

                case PlayerNotFoundException a:
                    ctx.Channel.SendMessageAsync(a.Message).ConfigureAwait(false);
                    break;

                //Default to
                default:
                    ctx.Channel.SendMessageAsync($"The code threw something and I have no clue what it means: {e.Message}").ConfigureAwait(false);
                    break;
            }
        }
    }
}

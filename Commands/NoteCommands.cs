using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using QuietHourBot.Databases;
using QuietHourBot.Handlers;

namespace QuietHourBot.Commands
{
    [Group("note")]
    [Description("If you want to save a note!")]
    class NoteCommands : BaseCommandModule
    {
        readonly NoteDB db = new NoteDB();
        readonly ErrorHandler handler = new ErrorHandler();

        [Command("add")]
        [Description("Saves a note")]
        public async Task Add(CommandContext ctx, [Description("Your note")] [RemainingText] string noteContent)
        {
            Note newNote = new Note(noteContent, ctx.Member, ctx.Message.Timestamp);
            
            db.Load();

            try
            {
                db.AddNote(newNote, ctx.Member);
                await ctx.Channel.SendMessageAsync("Done").ConfigureAwait(false);
            }
            catch (Exception e) { handler.Handle(ctx, e); }

            db.Save();
        }

        [Command("delete")]
        [Description("Deletes a note")]
        public async Task Delete(CommandContext ctx, [Description("key word/phrase from the note you want to delete")][RemainingText] string notePart)
        {
            db.Load();

            try { db.DeleteNote(notePart, ctx.Member); await ctx.Channel.SendMessageAsync("Deleted!").ConfigureAwait(false); }
            catch (Exception e) { handler.Handle(ctx, e); }

            db.Save();
        }

        [Command("print")]
        [Description("Prints all your notes")]
        public async Task Print(CommandContext ctx)
        {
            db.Load();
            await ctx.Channel.SendMessageAsync(embed: db.PrintNotes(ctx.Member)).ConfigureAwait(false);
        }
    }
}

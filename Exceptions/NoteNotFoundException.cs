using System;
using System.Collections.Generic;
using System.Text;
using QuietHourBot.Databases;

namespace QuietHourBot.Exceptions
{
    class NoteNotFoundException : Exception
    {
        public NoteNotFoundException(string notePart) : base($"No note found with keyword '{notePart}'! Please try a different keyword!")
        {
            ConsoleExt.ColorWriteLine($"{DateTime.Now} : {Message}", ConsoleColor.Yellow);
        }
    }
}

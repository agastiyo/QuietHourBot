using System;
using System.Collections.Generic;
using System.Text;
using QuietHourBot.Databases;

namespace QuietHourBot.Exceptions
{
    class MoreThanOneNoteFoundException : Exception
    {
        public MoreThanOneNoteFoundException(string notePart) : base($"You have mulitple notes with the keyword '{notePart}'. Try a more specific keyword")
        {
            ConsoleExt.ColorWriteLine($"{DateTime.Now} : {Message}", ConsoleColor.Yellow);
        }
    }
}

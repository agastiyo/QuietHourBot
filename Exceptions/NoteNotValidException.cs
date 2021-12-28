using System;
using System.Collections.Generic;
using System.Text;
using QuietHourBot.Databases;

namespace QuietHourBot.Exceptions
{
    class NoteNotValidException : Exception
    {
        public NoteNotValidException(string text) : base($"This note with text '{text}' is not valid")
        {
            ConsoleExt.ColorWriteLine($"{DateTime.Now} : {Message}", ConsoleColor.Yellow);
        }
    }
}

using QuietHourBot.Databases;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuietHourBot.Exceptions
{
    class TooManyNotesException : Exception
    {
        public TooManyNotesException() : base("You have too many notes! Please delete one before adding another")
        {
            ConsoleExt.ColorWriteLine($"{DateTime.Now} : {Message}", ConsoleColor.Yellow);
        }
    }
}

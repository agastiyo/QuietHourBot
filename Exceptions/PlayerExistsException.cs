using QuietHourBot.Databases;
using QuietHourBot.RPG.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuietHourBot.Exceptions
{
    public class PlayerExistsException : Exception
    {
        public PlayerExistsException(PlayerProfile profile) : base($"WARNING: The user {profile.ID} already exists.")
        {
            ConsoleExt.ColorWriteLine($"{DateTime.Now} : {Message}", ConsoleColor.Yellow);
        }
    }
}

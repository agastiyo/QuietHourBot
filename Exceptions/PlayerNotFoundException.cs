using QuietHourBot.RPG.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using QuietHourBot.Databases;

namespace QuietHourBot.Exceptions
{
    public class PlayerNotFoundException : Exception
    {
        public PlayerNotFoundException(string name) : base($"WARNING: The user {name} does not exist.")
        {
            ConsoleExt.ColorWriteLine($"{DateTime.Now} : {Message}", ConsoleColor.Yellow);
        }

        public PlayerNotFoundException(PlayerProfile profile) : base($"WARNING: The user {profile.ID} does not exist.")
        {
            ConsoleExt.ColorWriteLine($"{DateTime.Now} : {Message}", ConsoleColor.Yellow);
        }
    }
}

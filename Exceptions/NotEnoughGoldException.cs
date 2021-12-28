using QuietHourBot.RPG.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using QuietHourBot.Databases;

namespace QuietHourBot.Exceptions
{
    class NotEnoughGoldException : Exception
    {
        /// <summary>
        /// Thrown when a player does not have enough gold to cover an expense
        /// </summary>
        /// <param name="target">The person being targeted</param>
        /// <param name="goldAmount">The amount of gold that needs to be withdrawn</param>
        public NotEnoughGoldException(ulong targetID, uint goldAmount) : 
            base($"WARNING: User {targetID} does not have enough money to cover the expense of {goldAmount} gold")
        {
            ConsoleExt.ColorWriteLine($"{DateTime.Now} : {Message}", ConsoleColor.Yellow);
        }
    }
}

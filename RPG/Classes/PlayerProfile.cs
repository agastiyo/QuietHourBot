using DSharpPlus.Entities;
using QuietHourBot.Databases;
using QuietHourBot.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuietHourBot.RPG.Classes
{
    [Serializable]
    public class PlayerProfile
    {
        public ulong ID { get; }

        public uint Gold { get; private set; } = 0;

        private readonly List<Item> inventory = new List<Item>();

        public PlayerProfile(ulong ID)
        {
            this.ID = ID;
        }

        /// <summary>
        /// Getting the profile of the user who triggered the command
        /// </summary>
        /// <param name="guild">The user's guild</param>
        /// <returns>DiscordEmbedBuilder</returns>
        public DiscordEmbedBuilder DisplayProfile(DiscordGuild guild)
        {
            string inventoryList = "_ _";
            foreach (Item item in inventory)
            {
                inventoryList += $"`{item.ItemName} x{item.ItemAmount}`,";
            }

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"{ID.FindUser(guild).DisplayName}'s profile:",
                Color = DiscordColor.Green
            };

            embedBuilder.AddField("Gold:", Gold.ToString());
            embedBuilder.AddField("Inventory:", inventoryList);

            return embedBuilder;
        }

        /// <summary>
        /// Gets the DiscordMember of the given profile
        /// </summary>
        /// <param name="guild">The user's guild</param>
        /// <returns>DiscordMember</returns>
        public DiscordMember User(DiscordGuild guild)
        {
            return ID.FindUser(guild);
        }

        /// <summary>
        /// Gives the player a certain amount of gold
        /// </summary>
        /// <param name="gold">The amount of gold to give to the player</param>
        public void Pay(uint gold)
        {
            this.Gold += gold;
        }

        /// <summary>
        /// Takes a certain amount of gold from the player
        /// </summary>
        /// <param name="gold">Amount of gold to take from the player</param>
        public void Withdraw(uint gold)
        {
            if (this.Gold >= gold) { this.Gold -= gold; }
            else { throw new NotEnoughGoldException(this.ID, gold); }
        }
    }
}

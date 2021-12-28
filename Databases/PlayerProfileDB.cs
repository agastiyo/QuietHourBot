using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using QuietHourBot.Exceptions;
using QuietHourBot.RPG.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace QuietHourBot.Databases
{
    [Serializable]
    public class PlayerProfileDB
    {
        private readonly BinaryFormatter formatter;
        private const string DATA_FILENAME = "playerlist.dat";

        List<PlayerProfile> players = new List<PlayerProfile>();

        public PlayerProfileDB()
        {
            this.formatter = new BinaryFormatter();
        }

        public void AddPlayer(PlayerProfile player)
        {
            foreach(PlayerProfile profile in players)
            {
                if(profile.ID == player.ID)
                {
                    throw new PlayerExistsException(player);
                }
            }

            players.Add(player);
        }

        public void DeletePlayer(PlayerProfile player)
        {
            foreach(PlayerProfile profile in players)
            {
                if(profile.ID == player.ID)
                {
                    players.Remove(profile);
                    return;
                }
            }

            throw new PlayerNotFoundException(player);
        }

        public string ListPlayers(DiscordGuild guild)
        {
            string list = "_ _";

            foreach(PlayerProfile player in players)
            {
                try { list += $"`{player.ID.FindUser(guild).DisplayName}`,"; }
                catch(Exception e) 
                {
                    ConsoleExt.ColorWriteLine(e.Message, ConsoleColor.Yellow);
                }
            }

            return list;
        }

        /// <summary>
        /// Returns the PlayerProfile of the given ID if it exists.
        /// Throws a PlayerNotFound exception if profile does not exist.
        /// </summary>
        /// <param name="id">The id of the profile</param>
        /// <returns>PlayerProfile</returns>
        public PlayerProfile GetPlayer(ulong id)
        {
            foreach (PlayerProfile player in players)
            {
                if (player.ID == id)
                {
                    return player;
                }
            }

            throw new PlayerNotFoundException(id.ToString());
        }

        /// <summary>
        /// Returns true if the player is registered in the database
        /// </summary>
        /// <param name="id">The ID of the player to check</param>
        /// <param name="ctx">The context of the command</param>
        /// <returns>bool</returns>
        public bool ConfirmPlayer(ulong id, CommandContext ctx)
        {
            try { GetPlayer(id); }
            catch (Exception e) { ctx.Channel.SendMessageAsync(e.Message).ConfigureAwait(false); return false; }
            return true;
        }

        public void Save()
        {
            try
            {
                FileStream writerFileStream = new FileStream(DATA_FILENAME, FileMode.Create, FileAccess.Write);
                this.formatter.Serialize(writerFileStream, players);
                writerFileStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to save! - {e}");
            }
        }

        public void Load()
        {
            if (File.Exists(DATA_FILENAME))
            {
                try
                {
                    FileStream readerFileStream = new FileStream(DATA_FILENAME, FileMode.Open, FileAccess.Read);
                    players = (List<PlayerProfile>)
                        this.formatter.Deserialize(readerFileStream);
                    readerFileStream.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Data file FOUND\nUnexpected reading problem\nException - - - {e}");
                }
            }
        }
    }
}

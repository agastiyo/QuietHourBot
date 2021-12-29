using DSharpPlus.Entities;
using Emzi0767;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QuietHourBot.Databases
{
    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        /// <summary>
        /// Turns a character into an emoji represented by a string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToEmoji(this char value)
        {
            switch (value)
            {
                case '?':
                    return ":question:";
                case '!':
                    return ":exclamation:";
                case '#':
                    return ":hash:";
                case '$':
                    return ":dollar:";
                case '*':
                    return ":asterisk:";
                case '0':
                    return ":zero:";
                case '1':
                    return ":one:";
                case '2':
                    return ":two:";
                case '3':
                    return ":three:";
                case '4':
                    return ":four:";
                case '5':
                    return ":five:";
                case '6':
                    return ":six:";
                case '7':
                    return ":seven:";
                case '8':
                    return ":eight:";
                case '9':
                    return ":nine:";
            }
            if (value.IsBasicLetter()) { return $":regional_indicator_{value}:"; }
            return value.ToString();
        }

        public static string ToLisp(this string value)
        {
            value = value.Trim().ToLower().Replace("sh", "th").Replace("ss", "thh").Replace("s", "th").Replace("x", "cth");
            return value;
        }

        public static string Emphasize(this string value)
        {
            char[] chars = value.ToCharArray();
            string result = "";

            foreach (char character in chars)
            {
                result += $"***___{character}___***    ";
            }

            return result;
        }
    }

    public static class ConsoleExt
    {
        /// <summary>
        /// Writes in a specified color
        /// </summary>
        /// <param name="value">The string to write</param>
        /// <param name="color">The color to write in</param>
        /// <param name="defaultColor">The color to revert back to (default gray)</param>
        public static void ColorWriteLine(string value, ConsoleColor color, ConsoleColor defaultColor = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = defaultColor;
        }
    }

    public static class ArrayExt {
        /// <summary>
        /// Chooses a random option from an array
        /// </summary>
        /// <param name="options">Array to choose from</param>
        public string PickRandomly(this string[] options) {
            Random rnd = new Random();
            return options[rnd.Next(0, options.Length)];
        }
    }

    public static class IDInfoGrabber
    {
        /// <summary>
        /// Takes in a User ID and returns the associated DiscordMember
        /// </summary>
        /// <param name="value"></param>
        /// <param name="guild"></param>
        /// <returns>DiscordMember</returns>
        public static DiscordMember FindUser(this ulong value, DiscordGuild guild)
        {
            return guild.GetMemberAsync(value).Result;
        }
    }
}

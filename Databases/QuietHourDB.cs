using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuietHourBot.Databases
{
    public class QuietHourDB
    {
        public Dictionary<DiscordMember, int> violationLog = new Dictionary<DiscordMember, int>();
    }
}

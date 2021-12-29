using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using QuietHourBot.Databases;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;

namespace QuietHourBot.Commands
{
    class FunCommands : BaseCommandModule
    {
        [Command("kill")]
        [Description("kills the tagged user")]
        public async Task Kill(CommandContext ctx, [Description("The person you want to kill")] [RemainingText] DiscordMember person)
        {
            Random random = new Random();
            string[] waysToDie = { " contracted Cholera after drinking water from their dog's water bowl.", " died.", " tried to shoot a killer bee hive.",
                                    " drank the magic potion under the sink.", " is breaking the world record for holding their breath. They've been in the pool for" +
                                    " 11 minutes now.", " told their dad that he didn't need to get milk because they already had some.", " put the special sugar" +
                                    " in their cereal.", " fell between the subway and the platform.", " simped for Belle Delphine.", " donated to a Twitch thot.",
                                    " got very lucky while playing Russian Roulette.", " is the person who made array.count inclusive but not the for loop int.",
                                    " talked during quiet hours.", "got away!", " got stick bugged lol.", " got Rick Rolled.", " looked at the :ok_hand:",
                                    " is the person who got an index out of bounds exception for not declaring an array properly"};
            await ctx.Channel.SendMessageAsync(person.Mention + waysToDie[random.Next(0, waysToDie.Length)]).ConfigureAwait(false);
        }

        [Command("boba")]
        [Description("Gives boba to the tagged user")]
        public async Task Boba(CommandContext ctx, DiscordMember person)
        {
            await ctx.Channel.SendMessageAsync($"{ctx.Member.DisplayName} gave {person.Mention} boba! I hope they like it.");
        }

        [Command("rr")]
        [Description("Russian Roulette")]
        public async Task RussianRoulette(CommandContext ctx)
        {
            Random random = new Random();
            string[] actions = { "survived! :angel:", "survived! :angel:", "survived! :angel:", "survived! :angel:", "survived! :angel:", "died! :skull:" };

            await ctx.Channel.SendMessageAsync("Spinning cylinder...").ConfigureAwait(false);
            Thread.Sleep(1000);
            await ctx.Channel.SendMessageAsync("Aiming gun...").ConfigureAwait(false);
            Thread.Sleep(1000);
            await ctx.Channel.SendMessageAsync("**Pulling trigger!**").ConfigureAwait(false);
            Thread.Sleep(1000);
            string outcome = actions[random.Next(0, actions.Length)];

            var EmbedBuilder = new DiscordEmbedBuilder
            {
                Title = $"{ctx.Member.DisplayName} {outcome}",
                Color = ctx.Member.Color
            };

            await ctx.Channel.SendMessageAsync(embed: EmbedBuilder).ConfigureAwait(false);
        }

        [Command("duel")]
        [Description("Starts a duel with another user")]
        public async Task Duel(CommandContext ctx, [Description("The person you would like to duel")] [RemainingText] DiscordMember opponent)
        {
            Random random = new Random();
            int userHealth = 50;
            int opponentHealth = 50;
            string tempAction;
            DiscordMember winner;

            string[] actions = { "The gun misfired!", "The gun had a reload mishap!", "Headshot!", "Hit!" };
            Dictionary<string, int> actDamage = new Dictionary<string, int>
            {
                { "The gun misfired!", 10 },
                { "The gun had a reload mishap!", 0 },
                { "Headshot!", 50 },
                { "Hit!", 25 }
            };

            await ctx.Channel.SendMessageAsync($"The duel between {ctx.Member.Mention} and {opponent.Mention} begins...").ConfigureAwait(false);
            Thread.Sleep(2000);

            while (true)
            {
                tempAction = actions[random.Next(0, actions.Length)];
                actDamage.TryGetValue(tempAction, out int tempDamage);
                await ctx.Channel.SendMessageAsync($"{ctx.Member.DisplayName} begins their turn with {userHealth} health.").ConfigureAwait(false);
                Thread.Sleep(1000);
                await ctx.Channel.SendMessageAsync($"{tempAction} {ctx.Member.DisplayName} dealt {tempDamage} damage to {opponent.DisplayName}.");
                opponentHealth -= tempDamage;
                Thread.Sleep(500);
                if (opponentHealth <= 0) { break; }
                await ctx.Channel.SendMessageAsync($"{opponent.DisplayName} has {opponentHealth} health left.").ConfigureAwait(false);

                Thread.Sleep(1500);

                tempAction = actions[random.Next(0, actions.Length)];
                actDamage.TryGetValue(tempAction, out tempDamage);
                await ctx.Channel.SendMessageAsync($"{opponent.DisplayName} begins their turn with {opponentHealth} health.").ConfigureAwait(false);
                Thread.Sleep(1000);
                await ctx.Channel.SendMessageAsync($"{tempAction} {opponent.DisplayName} dealt {tempDamage} damage to {ctx.Member.DisplayName}.");
                userHealth -= tempDamage;
                Thread.Sleep(500);
                if (userHealth <= 0) { break; }
                await ctx.Channel.SendMessageAsync($"{ctx.Member.DisplayName} has {userHealth} health left.").ConfigureAwait(false);

                Thread.Sleep(1500);
            }

            if (userHealth > 0) { winner = ctx.Member; }
            else { winner = opponent; }

            var winnerEmbed = new DiscordEmbedBuilder
            {
                Title = $"{winner.DisplayName} wins the duel!",
                Description = "woohoo",
                ImageUrl = winner.AvatarUrl,
                Color = winner.Color
            };

            await ctx.Channel.SendMessageAsync(embed: winnerEmbed).ConfigureAwait(false);
        }

        [Command("rate")]
        [Description("Rates a thing of your choice")]
        public async Task Rate(CommandContext ctx, [Description("The thing to rate")] [RemainingText] string thing)
        {
            thing = thing.ToLower().Trim();

            if (thing.Contains("yourself") || thing.Contains("quiet bot"))
            {
                await ctx.Channel.SendMessageAsync("ofc im a 3000/10").ConfigureAwait(false);
            }
            else if (thing.Contains("agastya"))
            {
                await ctx.Channel.SendMessageAsync("ofc Agastya is a 5000000000000/10").ConfigureAwait(false);
            }
            else
            {
                Random random = new Random(thing.GetHashCode());
                await ctx.Channel.SendMessageAsync($"Lmao I think {thing} is a {random.Next(0, 10)}/10").ConfigureAwait(false);
            }
        }

        [Command("m8b")]
        [Description("Is a magic 8 ball")]
        public async Task Magic(CommandContext ctx, [Description("your question")][RemainingText] string question)
        {
            question = question.ToLower().Trim();
            string[] answers = { "yes", "no", "not sure", "try again later", "thats a dumb question", "shut up", "ofc", "no way", "it is certain",
                                 "there is no chance", "i think yes", "i think no", "according to fox news, yes", "according to cnn, no"};

            Random random = new Random(question.GetHashCode());
            int answer = random.Next(0, answers.Length);
            await ctx.Channel.SendMessageAsync(answers[answer]).ConfigureAwait(false);
        }

        [Command("pp")]
        [Description("pp")]
        public async Task PP(CommandContext ctx, DiscordMember person)
        {
            Random rnd = new Random(person.Username.GetHashCode());
            string pp = "8";
            int length = rnd.Next(0, 15);

            for (int i = 0; i <= length; i++) 
            {
                pp += "=";
            }

            pp += "D";

            await ctx.Channel.SendMessageAsync(pp).ConfigureAwait(false);
        }

        [Command("spam")]
        [Cooldown(1, 15, (CooldownBucketType)1)]
        [Description("spams the given phrase. Only the creator of the bot may use this command")]
        [RequireOwner]
        public async Task Spam(CommandContext ctx, [Description("How many times to spam the phrase. Maximum 30 times")] int spamTimes,
            [Description("Buffer between each message in milliseconds. Minimum 500 ms")] int bufferMs,
            [Description("phrase to spam")][RemainingText] string phrase)
        {
            if (bufferMs < 500) { bufferMs = 500; }
            if (spamTimes > 30) { spamTimes = 30; }
            for (int i = spamTimes; i > 0; i--)
            {
                await ctx.Channel.SendMessageAsync(phrase).ConfigureAwait(false);
                Thread.Sleep(bufferMs);
            }
        }

        [Command("getuser")]
        [Description("Gets info on the given user")]
        public async Task GetUser(CommandContext ctx, [Description("The ID of the user you want to find")] ulong ID)
        {
            DiscordMember member = ID.FindUser(ctx.Guild);

            var footer = new DiscordEmbedBuilder.EmbedFooter
            {
                IconUrl = member.AvatarUrl,
                Text = $"This user has been on Discord since {member.CreationTimestamp}" //how long the user has been on discord
            };

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"Information on User {ID}",
                Color = member.Color,
                Footer = footer
            };

            embedBuilder.AddField(member.Username, $"{member.DisplayName}'s username/display name"); //Display name
            embedBuilder.AddField(member.Discriminator, $"{member.DisplayName}'s 4 digit discriminator"); //4-digit discriminator
            embedBuilder.AddField($"{member.JoinedAt}", $" when {member.DisplayName} joined {ctx.Guild.Name}"); //When the user joined the server
            embedBuilder.AddField($"{ListString(member.Roles)}", $"These are {member.DisplayName}'s roles"); //The user's roles

            await ctx.Channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);
        }

        [Command("getuser")]
        [Description("Gets the given user")]
        public async Task GetUser(CommandContext ctx, [Description("The member you want to get")] DiscordMember member)
        {
            var footer = new DiscordEmbedBuilder.EmbedFooter
            {
                IconUrl = member.AvatarUrl,
                Text = $"This user has been on Discord since {member.CreationTimestamp}"
            };

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"Information on User {member.DisplayName}",
                Color = member.Color,
                Footer = footer
            };

            embedBuilder.AddField(member.Username, $"{member.DisplayName}'s username/display name");
            embedBuilder.AddField(member.Discriminator, $"{member.DisplayName}'s 4 digit discriminator");
            embedBuilder.AddField($"{member.JoinedAt}", $" when {member.DisplayName} joined {ctx.Guild.Name}");
            embedBuilder.AddField($"{ListString(member.Roles)}", $"These are {member.DisplayName}'s roles");

            await ctx.Channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);
        }

        [Command("ping")]
        [Description("Tests the bot's ping")]
        public async Task Ping(CommandContext ctx)
        {
            string emoji;

            if(ctx.Client.Ping > 100) { emoji = ":turtle:"; }
            else { emoji = ":rabbit2:"; }

            var embedBuilder = new DiscordEmbedBuilder()
            {
                Title = $"{ctx.Client.Ping} ms {emoji}"
            };

            await ctx.Channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);
        }

        [Command("gaytest")]
        [Description("ツ")]
        public async Task GayTest(CommandContext ctx, [Description("The person to test")] [RemainingText] DiscordMember person)
        {
            Random rnd = new Random(person.DisplayName.GetHashCode());
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"{person.DisplayName} is {rnd.Next(0,101)}% gay",
                Color = person.Color
            };

            await ctx.Channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);
        }

        [Command("getpfp")]
        [Description("Gets your pfp")]
        public async Task GetPfp(CommandContext ctx)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"{ctx.Member.DisplayName}'s pfp",
                ImageUrl = ctx.Member.AvatarUrl
            };

            await ctx.Channel.SendMessageAsync(embed : embedBuilder).ConfigureAwait(false);
        }

        [Command("getpfp")]
        [Description("Gets the profile picture of the specified user")]
        public async Task GetPfp(CommandContext ctx, [Description("The person to the the pfp of")][RemainingText] DiscordMember person)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"{person.DisplayName}'s pfp",
                ImageUrl = person.AvatarUrl
            };

            await ctx.Channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);
        }

        [Command("random")]
        [Description("Chooses a random element from a provided list")]
        public async Task Random(CommandContext ctx, [Description("List of choices. Separate each choice with ';'")] [RemainingText] string choicesStr)
        {
            Random random = new Random();
            string[] choices = choicesStr.Split(';', StringSplitOptions.RemoveEmptyEntries);

            await ctx.Channel.SendMessageAsync($"I choose '{choices[random.Next(0, choices.Length)]}'").ConfigureAwait(false);
        }

        [Command("serverpfp")]
        [Description("Gets the pfp of the server")]
        public async Task ServerPfp(CommandContext ctx)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"{ctx.Guild.Name}",
                ImageUrl = ctx.Guild.IconUrl
            };

            await ctx.Channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);
        }

        //-------------------------------------------------------------------------------

        private string ListString(IEnumerable<DiscordRole> list)
        {
            string result = "";
            foreach(var item in list)
            {
                result += $"{item.Name}, ";
            }
            return result;
        }
    }
}
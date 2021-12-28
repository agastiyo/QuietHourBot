using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using QuietHourBot.Databases;
using QuietHourBot.Handlers;
using QuietHourBot.RPG.Classes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace QuietHourBot.Commands
{
    [Group("RPG")]
    [Description("RPG commands")]
    public class RPGCommands : BaseCommandModule
    {
        readonly PlayerProfileDB db = new PlayerProfileDB();
        readonly ErrorHandler handler = new ErrorHandler();

        [Command("join")]
        [Description("Creates your account in the RPG")]
        public async Task Join(CommandContext ctx)
        {
            db.Load();
            PlayerProfile profile = new PlayerProfile(ctx.Member.Id);

            try
            {
                db.AddPlayer(profile);
                db.Save();
                db.GetPlayer(ctx.Member.Id).Pay(500);
                await ctx.Channel.SendMessageAsync("Player created!!!").ConfigureAwait(false);
                await ctx.Channel.SendMessageAsync(embed: profile.DisplayProfile(ctx.Guild)).ConfigureAwait(false);
            }
            catch (Exception e) { handler.Handle(ctx, e); }

            db.Save();
        }

        [Command("delete")]
        [Description("Deletes your account in the RPG")]
        public async Task Delete(CommandContext ctx)
        {
            db.Load();
            PlayerProfile profile = new PlayerProfile(ctx.Member.Id);

            try
            {
                db.DeletePlayer(profile);
                await ctx.Channel.SendMessageAsync("Player deleted!!!").ConfigureAwait(false);
            }
            catch (Exception e) { handler.Handle(ctx, e); }

            db.Save();
        }

        [Command("listplayers")]
        [Description("Lists all registered players in this guild")]
        public async Task ListPlayers(CommandContext ctx)
        {
            db.Load();
            await ctx.Channel.SendMessageAsync(db.ListPlayers(ctx.Guild)).ConfigureAwait(false);
        }

        [Command("profile")]
        [Description("Shows your profile")]
        public async Task Profile(CommandContext ctx)
        {
            db.Load();
            try { await ctx.Channel.SendMessageAsync(embed: db.GetPlayer(ctx.Member.Id).DisplayProfile(ctx.Guild)).ConfigureAwait(false); }
            catch (Exception e) { handler.Handle(ctx, e); }
        }

        [Command("adminprofile")]
        [RequireOwner]
        [Hidden]
        public async Task AdminProfile(CommandContext ctx, DiscordMember member)
        {
            db.Load();
            await ctx.Channel.SendMessageAsync(embed: db.GetPlayer(member.Id).DisplayProfile(ctx.Guild)).ConfigureAwait(false);
        }

        [Command("brawl")]
        [Description("Starts a fight with another player.")]
        [Cooldown(1, 15, (CooldownBucketType)1)]
        public async Task Brawl(CommandContext ctx, [Description("The player you want to brawl")] [RemainingText] DiscordMember target)
        {
            db.Load();

            if (!db.ConfirmPlayer(ctx.Member.Id, ctx)) { return; }
            if (!db.ConfirmPlayer(target.Id, ctx)) { return; }

            Random rand = new Random();
            bool win = rand.NextDouble() >= 0.5;

            if(win)
            {
                uint winnings = (uint)rand.Next(0, (int)db.GetPlayer(target.Id).Gold);
                db.GetPlayer(ctx.Member.Id).Pay(winnings);
                db.GetPlayer(target.Id).Withdraw(winnings);
                await ctx.Channel.SendMessageAsync($"{db.GetPlayer(ctx.Member.Id).User(ctx.Guild).Mention} defeated {db.GetPlayer(target.Id).User(ctx.Guild).Mention} and won {winnings} gold!")
                    .ConfigureAwait(false);
            }
            else
            {
                uint winnings = (uint)rand.Next(0, (int)db.GetPlayer(ctx.Member.Id).Gold);
                db.GetPlayer(ctx.Member.Id).Withdraw(winnings);
                db.GetPlayer(target.Id).Pay(winnings);
                await ctx.Channel.SendMessageAsync($"{db.GetPlayer(target.Id).User(ctx.Guild).Mention} defeated {db.GetPlayer(ctx.Member.Id).User(ctx.Guild).Mention} and won {winnings} gold!")
                    .ConfigureAwait(false);
            }

            db.Save();
        }

        [Command("donate")]
        [Description("Donates a certain amount of gold to another player")]
        [Cooldown(1, 7, (CooldownBucketType)1)]
        public async Task Donate(CommandContext ctx, [Description("Amount of gold to donate")] uint gold, 
            [Description("The player to donate to")] [RemainingText] DiscordMember target)
        {
            db.Load();

            if(!db.ConfirmPlayer(ctx.Member.Id, ctx)) { return; }
            if(!db.ConfirmPlayer(target.Id, ctx)) { return; }

            try { db.GetPlayer(ctx.Member.Id).Withdraw(gold); }
            catch (Exception e) { handler.Handle(ctx, e); return; }

            db.GetPlayer(target.Id).Pay(gold);

            await ctx.Channel.SendMessageAsync($"{ctx.Member.DisplayName} donated {gold} gold to {target.Mention}").ConfigureAwait(false);

            db.Save();
        }

        [Command("gamble")]
        [Description("Gamble a set amount of money")]
        [Cooldown(1, 7, (CooldownBucketType)1)]
        public async Task Gamble(CommandContext ctx, [Description("Amount of gold to gamble")] uint gold)
        {
            Random rnd = new Random();

            int[] actions = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 5 };
            //2 = lose all
            //3 = 100%
            //4 = 200%
            //5 = 500%

            db.Load();
            
            try { db.GetPlayer(ctx.Member.Id).Withdraw(gold); }
            catch (Exception e) { handler.Handle(ctx, e); return; }

            int outcome = actions[rnd.Next(0, actions.Length)];

            switch(outcome)
            {
                case (2):
                    await ctx.Channel.SendMessageAsync($"You bet {gold} gold and lost everything!").ConfigureAwait(false);
                    break;
                case (3):
                    await ctx.Channel.SendMessageAsync($"You bet {gold} gold and got {gold} back!").ConfigureAwait(false);
                    db.GetPlayer(ctx.Member.Id).Pay(gold);
                    break;
                case (4):
                    await ctx.Channel.SendMessageAsync($"You bet {gold} gold and got {gold*2} back!").ConfigureAwait(false);
                    db.GetPlayer(ctx.Member.Id).Pay(gold * 2);
                    break;
                case (5):
                    await ctx.Channel.SendMessageAsync($"You bet {gold} gold and got {gold*5} back!").ConfigureAwait(false);
                    db.GetPlayer(ctx.Member.Id).Pay(gold * 5);
                    break;
            }

            db.Save();
        }
    }
}

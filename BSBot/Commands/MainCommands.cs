using System;
using System.Threading.Tasks;

using Microsoft.Extensions.PlatformAbstractions;

using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace BSBot.Commands
{
	public class MainCommands : BaseCommandModule
	{
		[Command("Ping")]
		[Description("Pongs!")]
		public async Task Ping(CommandContext ctx)
		{
			await ctx.RespondAsync("Pong! " + ctx.Client.Ping);
		}

		[Command("info")]
		[Aliases("about")]
		[Description("Display some info about the bot")]
		public async Task Info(CommandContext ctx)
		{
			DiscordEmbedBuilder discordEmbed = new DiscordEmbedBuilder
			{
				Color = DiscordColor.MidnightBlue,
				Title = "BSBot#1974",
				Description = $"BSBot is a custom bot for the server BSIT20, prefix is / or {ctx.Client.CurrentUser.Mention}\n" +
					"For more info use /help",
				Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = ctx.Client.CurrentUser.AvatarUrl }
			};
			discordEmbed.AddField("Language", "C# using Visual Studio 2019");
			discordEmbed.AddField("Library", "DSharpPlus, Version: " + ctx.Client.VersionString);
			discordEmbed.AddField(".NET Core Version: ", PlatformServices.Default.Application.RuntimeFramework.Version.ToString(2));
			discordEmbed.AddField("Repository", "[GitHub](https://github.com/JulianusIV/BSBot)");
			TimeSpan uptime = DateTime.Now - Bot.Instance.StartTime;
			discordEmbed.AddField("Uptime", $"Bot has been running since {uptime}");
			await ctx.Channel.SendMessageAsync(discordEmbed.Build());
		}
	}
}

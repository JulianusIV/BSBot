using BSBot.Objects;
using BSBot.Repositories;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace BSBot.Commands
{
	[Group("exam")]
	public class ExamCommands : BaseCommandModule
	{
		[Command("new")]
		public async Task New(CommandContext ctx, DateTime date, string subject, [RemainingText]string text)
		{
			if (date < DateTime.Now)
			{
				await ctx.RespondAsync("Date has to be in the future!");
				return;
			}

			DiscordEmbedBuilder builder = new DiscordEmbedBuilder
			{
				Title = $"Klassenarbeit in {subject}:",
				Description = text +
					Environment.NewLine +
					date.ToString("yyyy-MM-dd"),
				Color = DiscordColor.Red
			};

			DiscordMessage message = await ctx.Guild.GetChannel(747431696755851355).SendMessageAsync(builder);

			Exam exam = new Exam
			{
				DueDate = date,
				Subject = subject,
				Text = text,
				MessageId = message.Id
			};

			ExamRepository repo = new ExamRepository(Bot.Instance.ConfigJson.ConnectionString);
			if (!repo.Create(ref exam))
			{
				await ctx.RespondAsync("There was a problem creating a database entry!");
				await message.DeleteAsync();
				return;
			}
			await ctx.RespondAsync("Done!");
		}

		[Command("delete")]
		public async Task Delete(CommandContext ctx, DiscordMessage messageLink)
		{
			ExamRepository repo = new ExamRepository(Bot.Instance.ConfigJson.ConnectionString);
			if (!repo.GetByMessageId(messageLink.Id, out Exam entity))
			{
				await ctx.RespondAsync("There was a problem reading from the database!");
				return;
			}
			if (!repo.Delete(entity.Id))
			{
				await ctx.RespondAsync("There was a problem deleting the entry from the database!");
				return;
			}
			await messageLink.DeleteAsync();
			await ctx.RespondAsync("Done!");
		}
	}
}

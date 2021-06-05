using System;
using System.Threading.Tasks;

using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using BSBot.Objects;
using BSBot.Repositories;

namespace BSBot.Commands
{
	[Group("exam")]
	[Description("Command group for handling exams")]
	public class ExamCommands : BaseCommandModule
	{
		[Command("new")]
		[Description("Create a new exam")]
		public async Task New(CommandContext ctx,
			[Description("The day of the exam, Format: yyyy.MM.dd or dd.MM.yyyy")]Date date,
			[Description("Subject of the exam (max 10 chars)")]string subject,
			[Description("Free text, for example topics.")][RemainingText]string text)
		{
			if (ctx.Channel.Id != 850501999882928158)
				return;
			if (date < Date.FromDateTime(DateTime.Now))
			{
				await ctx.RespondAsync("Date has to be in the future!");
				return;
			}
			if (subject.Length > 10)
			{
				await ctx.RespondAsync("Max length of the subject is 10 chars!");
				return;
			}

			DiscordEmbedBuilder builder = new DiscordEmbedBuilder
			{
				Title = $"Klassenarbeit in {subject}:",
				Description = text +
					Environment.NewLine +
					date.ToString(),
				Color = DiscordColor.Orange
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
		[Description("Delete an existing exam")]
		public async Task Delete(CommandContext ctx, [Description("Link to the message")]DiscordMessage messageLink)
		{
			if (ctx.Channel.Id != 850501999882928158)
				return;
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

		[Command("edit")]
		[Description("Edit an existing exams parameters")]
		public async Task Edit(CommandContext ctx,
			[Description("Messagelink of the exam you want to edit")] DiscordMessage messageLink,
			[Description("The day of the exam, Format: yyyy.MM.dd or dd.MM.yyyy")] Date date,
			[Description("Subject of the exam (max 10 chars)")] string subject,
			[Description("Free text, for example topics.")][RemainingText] string text)
		{
			if (ctx.Channel.Id != 850501999882928158)
				return;
			ExamRepository repo = new ExamRepository(Bot.Instance.ConfigJson.ConnectionString);
			if (!repo.GetByMessageId(messageLink.Id, out Exam entity))
			{
				await ctx.RespondAsync("There was a problem reading from the database!");
				return;
			}
			entity.DueDate = date;
			entity.Subject = subject;
			entity.Text = text;
			if (!repo.Update(entity))
			{
				await ctx.RespondAsync("There was a problem updating the database entry!");
				return;
			}
			DiscordEmbedBuilder builder = new DiscordEmbedBuilder
			{
				Title = $"Klassenarbeit in {subject}:",
				Description = text +
					Environment.NewLine +
					date.ToString(),
				Color = DiscordColor.Orange
			};
			await messageLink.ModifyAsync(builder.Build());
			await ctx.RespondAsync("Done!");
		}
	}
}

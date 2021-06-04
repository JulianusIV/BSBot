using BSBot.Objects;
using BSBot.Repositories;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;

namespace BSBot
{
	public class TimerMethods
	{
		internal static void Tick(object sender, ElapsedEventArgs e)
		{
			((Timer)sender).Stop();
			_ = Task.Run(async () =>
			{
				ExamRepository repo = new ExamRepository(Bot.Instance.ConfigJson.ConnectionString);
				if (!repo.GetAll(out List<Exam> list))
				{
					//TODO: add logging
					Debug.WriteLine("There was a problem reading all exams from the database!");
					return;
				}
				foreach (var item in list)
				{
					if (item.DueDate <= Date.FromDateTime(DateTime.Now))
					{
						if (!repo.Delete(item.Id))
						{
							//TODO: add logging
							Debug.WriteLine("There was a problem deleting an entry from the database!");
							continue;
						}
						DiscordGuild guild = await Bot.Instance.Client.GetGuildAsync(747429821830922302);
						DiscordMessage message = await guild.GetChannel(747431696755851355).GetMessageAsync(item.MessageId);
						DiscordEmbedBuilder builder = new DiscordEmbedBuilder
						{
							Title = $"Klassenarbeit in {item.Subject}:",
							Description = item.Text +
								Environment.NewLine +
								item.DueDate.ToString(),
							Color = DiscordColor.Green
						};
						await message.ModifyAsync(builder.Build());
					}
					else if (item.DueDate <= Date.FromDateTime(DateTime.Now + TimeSpan.FromDays(1)))
					{
						DiscordGuild guild = await Bot.Instance.Client.GetGuildAsync(747429821830922302);
						DiscordMessage message = await guild.GetChannel(747431696755851355).GetMessageAsync(item.MessageId);
						DiscordEmbedBuilder builder = new DiscordEmbedBuilder
						{
							Title = $"Klassenarbeit in {item.Subject}:",
							Description = item.Text +
								Environment.NewLine +
								item.DueDate.ToString(),
							Color = DiscordColor.Red
						};
						await message.ModifyAsync(builder.Build());
					}
				}
			});
		}
	}
}

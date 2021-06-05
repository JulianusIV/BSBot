using System;
using System.IO;
using System.Timers;
using System.Text.Json;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;

using BSBot.Objects;
using BSBot.Repositories;

namespace BSBot
{
	public class Bot
	{
		#region Singleton
		private static Bot _instance;
		private static readonly object padlock = new object();
		public static Bot Instance
		{
			get
			{
				lock (padlock)
				{
					if (_instance == null)
						_instance = new Bot();
					return _instance;
				}
			}
		}
		#endregion

		public DiscordClient Client { get; private set; }

		public CommandsNextExtension Commands { get; private set; }

		public ConfigJson ConfigJson { get; set; }

		public Timer Timer = new Timer(86400000); //one day

		public DateTime StartTime { get; private set; }

		public async Task RunAsync()
		{
			StartTime = DateTime.Now;

			string json = File.ReadAllText("config.json");
			ConfigJson = JsonSerializer.Deserialize<ConfigJson>(json);

			DiscordConfiguration config = new DiscordConfiguration
			{
				Token = ConfigJson.Token,
				TokenType = TokenType.Bot,
				AutoReconnect = true,
				MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug
			};

			Client = new DiscordClient(config);

			//Register client events
			Client.Ready += Client_Ready;
			Client.MessageDeleted += Client_MessageDeleted;

			CommandsNextConfiguration commandsConfig = new CommandsNextConfiguration
			{
				StringPrefixes = new string[] { ConfigJson.Prefix },
				EnableMentionPrefix = true
			};
			Commands = Client.UseCommandsNext(commandsConfig);

			//Register command modules
			Commands.RegisterConverter(new DateArgumentConverter());
			Commands.RegisterCommands(typeof(Bot).GetTypeInfo().Assembly);

			//Register command events

			await Client.ConnectAsync();

			//Timer events and start
			Timer.Elapsed += TimerMethods.Tick;
			Timer.Start();

			//Delay Task forever (-1) to keep program running while waiting for events
			await Task.Delay(-1);
		}

		private Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
		{
			DiscordActivity activity = new DiscordActivity
			{
				ActivityType = ActivityType.Streaming,
				Name = "your IP adress.",
				StreamUrl = "https://twitch.tv/test"
			};
			Client.UpdateStatusAsync(activity);
			return Task.CompletedTask;
		}

		private Task Client_MessageDeleted(DiscordClient sender, MessageDeleteEventArgs e)
		{
			_ = Task.Run(() =>
			{
				if (e.Message.Channel.Id == 747431696755851355)
				{
					ExamRepository repo = new ExamRepository(ConfigJson.ConnectionString);
					if (!repo.GetByMessageId(e.Message.Id, out Exam entity))
					{
						//TODO: add logging
						Debug.WriteLine("There was a problem reading a database entry! " + e.Message.Id);
						return;
					}
					if (!repo.Delete(entity.Id))
					{
						//TODO: add logging
						Debug.WriteLine("There was a problem deleting a database entry! " + e.Message.Id);
						return;
					}
				}
			});
			return Task.CompletedTask;
		}
	}
}

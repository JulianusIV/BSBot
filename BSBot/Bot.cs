using DSharpPlus;
using DSharpPlus.CommandsNext;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

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

		public async Task RunAsync()
		{
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

			CommandsNextConfiguration commandsConfig = new CommandsNextConfiguration
			{
				StringPrefixes = new string[] { ConfigJson.Prefix },
				EnableMentionPrefix = true
			};
			Commands = Client.UseCommandsNext(commandsConfig);

			//Register command modules
			Commands.RegisterCommands(typeof(Bot).GetTypeInfo().Assembly);

			//Register command events

			await Client.ConnectAsync();

			//Delay Task forever (-1) to keep program running while waiting for events
			await Task.Delay(-1);
		}
	}
}

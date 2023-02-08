using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBot
{
	public class Program
	{
		private DiscordSocketClient _client;
		private CommandService _commands;

		public static Task Main(string[] args) => new Program().MainAsync();

		public async Task MainAsync()
		{
			_client = new DiscordSocketClient();

			_commands = new CommandService();

			var commandHandler = new CommandHandler(_client, _commands);
			await commandHandler.InstallCommandsAsync();

			_client.Log += Log;

			//  You can assign your bot token to a string, and pass that in to connect.
			//  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.

			string token = null;

			try
			{
                token = File.ReadAllText("token.txt");
            }
			catch (FileNotFoundException e)
			{
				throw new FileNotFoundException("You need token.txt file with token for this bot");
			}

			if (token is null)
			{
                throw new ArgumentNullException("You need token.txt file with token for this bot");
            }

			// Some alternative options would be to keep your token in an Environment Variable or a standalone file.
			// var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
			// var token = File.ReadAllText("token.txt");
			// var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();

			// Block this task until the program is closed.
			await Task.Delay(-1);
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}

using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Discord_ParkourFPS_Bot
{
	public class Program
	{
		public string Prefix = "/";

		private DiscordSocketClient _client;
		static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			_client = new DiscordSocketClient(new DiscordSocketConfig
			{
				LogLevel = LogSeverity.Info
			});

			_client.Log += Log;
			_client.MessageReceived += MessageReceived;

			Environment.SetEnvironmentVariable("DiscordToken", "NzYxMTg2MjM1MjYyOTU5NjQ5.X3W77A.0Lhchw_1BFU0DRsRzz8X_KCQ62E");

			await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken"));
			await _client.StartAsync();

			await Task.Delay(-1);
		}

		private Task Log(LogMessage message)
		{
			Console.WriteLine(message.ToString());
			return Task.CompletedTask;
		}

		private async Task MessageReceived(SocketMessage message)
		{
			if (message.Content == Prefix+"hi")
			{
				await message.Channel.SendMessageAsync("Hello!");
			}
		}
	}
}

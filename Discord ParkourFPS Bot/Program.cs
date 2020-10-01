using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Discord_ParkourFPS_Bot
{
    public class Program
    {
        //Prefix variable
        public string Prefix = "~";

        //Bot startup variables
        private DiscordSocketClient _client;
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        //Bot initialisation
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

        //Log when bot startup is complete
        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        //On message received
        private async Task MessageReceived(SocketMessage message)
        {
            //Checks if the channel of the message == bot-games
            if (message.Channel.Id == 761286954955309057)
            {
                //Checks if the message starts with the prefix
                if (message.Content.StartsWith(Prefix))
                {
                    //Sets message to lowercase and stores it to message_lowercase
                    string message_lowercase = message.Content.ToLower();

                    //Help command
                    if (message_lowercase == Prefix + "help")
                    {
                        await message.Channel.SendMessageAsync("Hello @"+message.Author+", soon you'll be able to play a game with me!");
                    }
                }
            }
        }
    }
}
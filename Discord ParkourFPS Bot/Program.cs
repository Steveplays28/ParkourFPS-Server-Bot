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
        private DiscordSocketClient client;
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        //Bot initialisation
        public async Task MainAsync()
        {
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });

            client.Log += Log;
            client.MessageReceived += MessageReceived;
            client.UserJoined += UserJoined;
            client.Ready += ClientReady;

            Environment.SetEnvironmentVariable("DiscordToken", "NzYxMTg2MjM1MjYyOTU5NjQ5.X3W77A.0Lhchw_1BFU0DRsRzz8X_KCQ62E");

            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken"));
            await client.StartAsync();

            await Task.Delay(-1);
        }

        //Logs the bot's startup process
        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        private async Task ClientReady()
        {
            //Set bot status
            await client.SetActivityAsync(new Game("Sneakily playing games..."/*,type: ActivityType.CustomStatus*/));
            Console.WriteLine("I should've set my status now...");
        }

        //On message received
        private async Task MessageReceived(SocketMessage message)
        {
            if(message_lowercase.Contains("you are bad"))
            {
                await message.Channel.SendMessageAsync(message_author + "test");
            }
            
            //Checks if the message author is not a bot or a webhook
            if (message.Author.IsBot == false && message.Author.IsWebhook == false)
            {
                //Checks if the channel of the message == bot-games
                if (message.Channel.Id == 761286954955309057 || message.Channel.Id == 761284474339721236)
                {
                    //Checks if the message contains the prefix
                    if (message.Content.Contains(Prefix))
                    {
                        //Sets message to lowercase and stores it to message_lowercase
                        string message_lowercase = message.Content.ToLower();
                        //Gets the message author in a mentionable way
                        string message_author = message.Author.Mention;

                        //Help command
                        if (message_lowercase.Contains(Prefix + "help"))
                        {
                            await message.Channel.SendMessageAsync("Hello " + message_author + ", soon you'll be able to play a game with me!");
                        }
                        
                    }
                }
            }
        }

        private async Task UserJoined(SocketGuildUser user)
        {
            //Checks if the message author is not a bot or a webhook
            if (user.IsBot == false && user.IsWebhook == false)
            {
                await Task.Delay(new TimeSpan(0, 10, 0));
                await user.SendMessageAsync("Some PM, soon it'll say something!");
            }
        }
    }
}

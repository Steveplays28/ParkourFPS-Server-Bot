using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Discord_ParkourFPS_Bot
{
    public class Program
    {
        //Global variables
        private DiscordSocketClient client;
        private readonly string Prefix = "~";
        private SocketGuild parkourfps_server;
        private SocketGuildUser steveplays;
        private SocketTextChannel rules_and_info;

        //Main static void
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        //Bot initialisation
        public async Task MainAsync()
        {
            //Get the client and store it in the "client" variable
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });

            //Tasks
            client.Log += Log;
            client.Ready += ClientReady;
            client.MessageReceived += MessageReceived;
            client.UserJoined += UserJoined;
            client.ReactionAdded += ReactionAdded;
            client.ReactionRemoved += ReactionRemoved;

            //Store bot token
            Environment.SetEnvironmentVariable("DiscordToken", "NzYxMTg2MjM1MjYyOTU5NjQ5.X3W77A.0Lhchw_1BFU0DRsRzz8X_KCQ62E");

            //Start initialisation with bot token
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

        //On bot ready
        private async Task ClientReady()
        {
            //Set bot status
            await client.SetGameAsync("user commands!", streamUrl: "", type: ActivityType.Listening);
            Console.WriteLine("I should've set my status now...");

            //Set global variables
            parkourfps_server = client.GetGuild(746681304111906867);
            steveplays = parkourfps_server.GetUser(746088882461737111);
            rules_and_info = parkourfps_server.GetTextChannel(746697885248258078);
        }

        //On message received
        private async Task MessageReceived(SocketMessage message)
        {
            //Checks if the message author is not a bot or a webhook
            if (message.Author.IsBot == false && message.Author.IsWebhook == false)
            {
                //Checks if the channel of the message is the bot-games channel or the bot-testing channel
                if (message.Channel.Id == 761286954955309057 || message.Channel.Id == 761284474339721236)
                {
                    //Sets message to lowercase and stores it to message_lowercase
                    string message_lowercase = message.Content.ToLower();

                    //Gets the message author in a mentionable way
                    string message_author_mention = message.Author.Mention;

                    //Help command
                    if (message_lowercase.Contains(Prefix + "help"))
                    {
                        await message.Channel.SendMessageAsync("Hello " + message_author_mention + ", I'm managing role reactions and soon you'll be able to play games with me! \nMore functionality is in development.");
                    }

                    //Bot mention reply
                    if (message.Content.Contains(client.CurrentUser.Mention))
                    {
                        await message.Channel.SendMessageAsync("Hello " + message_author_mention + ", what can I do for you?");
                    }

                    //Play snake command
                    if (message_lowercase.Contains(Prefix + "play snake"))
                    {
                        //EmbedBuilder snake_game_embed = new EmbedBuilder();
                        //int x = 5;
                        //int y = 5;

                        //snake_game_embed.WithTitle("Snake, WIP");
                        //snake_game_embed.AddField("Snake, WIP", x);

                        //await message.Channel.SendMessageAsync(embed: snake_game_embed.Build());

                        Snake snake_game = new Snake(5, 5, "🟦");
                        Snake player_character = (3,3, "🟩");
                        await message.Channel.SendMessageAsync(snake_game.ToString() + player_character.ToString());
                        
                        
                    }
                }
            }
        }

        //On user joined server
        private async Task UserJoined(SocketGuildUser user)
        {
            //Checks if the message author is not a bot or a webhook
            if (user.IsBot == false && user.IsWebhook == false)
            {
                //Gets the user in a mentionable way
                string user_mention = user.Mention;

                //Sends the user a welcome message
                await user.SendMessageAsync("Hi there " + user_mention + ", welcome to the official ParkourFPS Discord server! \nHere you can talk with others about ParkourFPS, talk about game development and play some games with me! \n \nPlease check out the channel " + rules_and_info.Mention + " for all the rules and info you'll need to know. \nYou will have to accept the TOS of Discord and this server by clicking on the check to gain access to the server.");

                //Gets the "New Member" role and give it to the user
                SocketRole new_member_role = parkourfps_server.GetRole(761633924220190732);
                await user.AddRoleAsync(new_member_role);
            }
        }

        //On reaction added to message
        private async Task ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel messageChannel, SocketReaction reaction)
        {
            //Checks if the message is the rules-and-info message
            if (reaction.MessageId == 746698103356260403)
            {
                //Checks if the reaction emote is a :white_check_mark:
                if (reaction.Emote.Name == "✅")
                {
                    //Gets the user that reacted
                    ulong user_id = reaction.UserId;
                    SocketGuildUser user = parkourfps_server.GetUser(user_id);

                    //Removes the "New Member" role from the user
                    SocketRole new_member_role = parkourfps_server.GetRole(761633924220190732);
                    await user.RemoveRoleAsync(new_member_role);
                }
            }

            //Checks if the message is the bot-games role message
            if (reaction.MessageId == 761701551126478849)
            {
                //Checks if the reaction emote is a :robot:
                if (reaction.Emote.Name == "🤖")
                {
                    //Gets the user that reacted
                    ulong user_id = reaction.UserId;
                    SocketGuildUser user = parkourfps_server.GetUser(user_id);

                    //Adds the "Bot games" role to the user
                    SocketRole bot_games = parkourfps_server.GetRole(761700403078889472);
                    await user.AddRoleAsync(bot_games);
                }
            }
        }

        //On reaction removed from message
        private async Task ReactionRemoved(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel messageChannel, SocketReaction reaction)
        {
            //Checks if the message is the rules-and-info message
            if (reaction.MessageId == 746698103356260403)
            {
                //Checks if the removed reaction emote is a :white_check_mark:
                if (reaction.Emote.Name == "✅")
                {
                    //Gets the user that removed the reaction
                    ulong user_id = reaction.UserId;
                    SocketGuildUser user = parkourfps_server.GetUser(user_id);

                    //Adds the "New Member" role to the user
                    SocketRole new_member_role = parkourfps_server.GetRole(761633924220190732);
                    await user.AddRoleAsync(new_member_role);
                }
            }

            //Checks if the message is the bot-games role message
            if (reaction.MessageId == 761701551126478849)
            {
                //Checks if the reaction emote is a :robot:
                if (reaction.Emote.Name == "🤖")
                {
                    //Gets the user that reacted
                    ulong user_id = reaction.UserId;
                    SocketGuildUser user = parkourfps_server.GetUser(user_id);

                    //Removes the "Bot games" role from the user
                    SocketRole bot_games = parkourfps_server.GetRole(761700403078889472);
                    await user.RemoveRoleAsync(bot_games);
                }
            }
        }
    }
}

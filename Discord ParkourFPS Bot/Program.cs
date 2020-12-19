using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Discord_ParkourFPS_Bot
{
    public class Program
    {
        //Global variables
        Random r = new Random();
        private DiscordSocketClient client;
        private readonly string prefix = "~";
        private SocketGuild parkourfpsServer;
        private SocketTextChannel rulesAndInfoChannel;
        private bool is_playing_snake;
        private int snake_line;
        private int snake_column;
        private Snake snake_game;

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
            Environment.SetEnvironmentVariable("DiscordToken", "NzYxMTg2MjM1MjYyOTU5NjQ5.X3W77A.VPEBmiA4yv0q1D6mxMyUv7xXfOk");

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
            parkourfpsServer = client.GetGuild(746681304111906867);
            rulesAndInfoChannel = parkourfpsServer.GetTextChannel(746697885248258078);
        }

        //On message received
        private async Task MessageReceived(SocketMessage message)
        {
            //Checks if the message author is not a bot or a webhook
            if (message.Author.IsBot == false && message.Author.IsWebhook == false)
            {
                //Sets message to lowercase and stores it to messageLowercase
                string messageLowercase = message.Content.ToLower();

                //Gets the message author in a mentionable way
                string messageAuthorMention = message.Author.Mention;

                //Help command
                if (messageLowercase == prefix + "help")
                {
                    await message.Channel.SendMessageAsync("Hello " + messageAuthorMention + ", I'm managing role reactions and soon you'll be able to play games with me! \nMore functionality is in development.");
                }

                //Bot mention reply
                if (messageLowercase == client.CurrentUser.Mention)
                {
                    await message.Channel.SendMessageAsync("Hello " + messageAuthorMention + ", what can I do for you?");
                }

                //Move command
                if (messageLowercase.StartsWith(prefix) && messageLowercase.Contains("move") && messageLowercase.Contains("#"))
                {
                    //Extract ID's
                    string linkedGuildIdString = messageLowercase.Split('/', '/')[4];
                    string linkedChannelIdString = messageLowercase.Split('/', '/')[5];
                    string linkedMessageIdString = messageLowercase.Split('/', '<')[6];
                    linkedMessageIdString = linkedMessageIdString.Remove(linkedMessageIdString.Length - 1);
                    string otherChannelIdString = messageLowercase.Split('#', '>')[1];

                    //Convert ID's to ulong
                    ulong linkedGuildId = Convert.ToUInt64(linkedGuildIdString);
                    ulong linkedChannelId = Convert.ToUInt64(linkedChannelIdString);
                    ulong linkedMessageId = Convert.ToUInt64(linkedMessageIdString);
                    ulong otherChannelId = Convert.ToUInt64(otherChannelIdString);

                    //Get linked message
                    IMessage linkedMessage = await client.GetGuild(linkedGuildId).GetTextChannel(linkedChannelId).GetMessageAsync(linkedMessageId);
                    string linkedMessageString = linkedMessage.ToString();

                    //Get other channel
                    SocketTextChannel otherChannel = client.GetGuild(linkedGuildId).GetTextChannel(otherChannelId);

                    //Send new message in other channel
                    IMessage v = await otherChannel.SendMessageAsync(linkedMessage.Author.Username + " said: \n> " + linkedMessageString);

                    //Delete linked message
                    await client.GetGuild(linkedGuildId).GetTextChannel(linkedChannelId).DeleteMessageAsync(linkedMessageId);

                    //Remove command invocation
                    await message.DeleteAsync();
                    //Success message
                    IMessage successMessage = await message.Channel.SendMessageAsync("Message moved successfully!");

                    //Remove success message
                    await Task.Delay(5000);
                    await successMessage.DeleteAsync();
                }

                //Checks if the channel of the message is the bot-games channel or the bot-testing channel
                if (message.Channel.Id == 761286954955309057 || message.Channel.Id == 761284474339721236)
                {
                    //Play snake command
                    if (messageLowercase == prefix + "play snake")
                    {
                        //Random snake position
                        snake_line = r.Next(5);
                        snake_column = r.Next(5);

                        //Start new game of snake
                        is_playing_snake = true;
                        snake_game = new Snake(10, 10);
                        snake_game.canvas_array[snake_line, snake_column] = snake_game.snake_emoji;
                        await message.Channel.SendMessageAsync(snake_game.ToString());

                    }

                    // if the person is playing snake, then magic and stuff.
                    if (is_playing_snake == true)
                    {
                        //Move snake up
                        if (messageLowercase == prefix + "w")
                        {
                            snake_line -= 1;
                            snake_game.canvas_array[snake_line + 1, snake_column] = snake_game.canvas_emoji;
                            snake_game.canvas_array[snake_line, snake_column] = "🟩";
                            await message.Channel.SendMessageAsync(snake_game.ToString());
                        }

                        //Move snake left
                        if (messageLowercase == prefix + "a")
                        {
                            snake_column -= 1;
                            snake_game.canvas_array[snake_line, snake_column + 1] = snake_game.canvas_emoji;
                            snake_game.canvas_array[snake_line, snake_column] = "🟩";
                            await message.Channel.SendMessageAsync(snake_game.ToString());
                        }

                        //Move snake down
                        if (messageLowercase == prefix + "s")
                        {
                            snake_line += 1;
                            snake_game.canvas_array[snake_line - 1, snake_column] = snake_game.canvas_emoji;
                            snake_game.canvas_array[snake_line, snake_column] = "🟩";
                            await message.Channel.SendMessageAsync(snake_game.ToString());
                        }

                        //Move snake right
                        if (messageLowercase == prefix + "d")
                        {
                            snake_column += 1;
                            snake_game.canvas_array[snake_line, snake_column - 1] = snake_game.canvas_emoji;
                            snake_game.canvas_array[snake_line, snake_column] = "🟩";
                            await message.Channel.SendMessageAsync(snake_game.ToString());
                        }
                    }
                }

                //if (message.Channel.Id == 789271453362552883)
                //{
                //    var x = message.Channel.GetMessagesAsync(2, CacheMode.AllowDownload);
                    

                //    if (GetDamerauLevenshteinDistance(s, message) > 20)
                //    {
                //        await message.Channel.SendMessageAsync("Heh, a chain breaker, pathetic. \nThe ```" + message.Content + "``` chain was {x} messages long. \n \nGG.");
                //    }
                //}
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
                await user.SendMessageAsync("Hi there " + user_mention + ", welcome to the official ParkourFPS Discord server! \nHere you can talk with others about ParkourFPS, talk about game development and play some games with me! \n \nPlease check out the channel " + rulesAndInfoChannel.Mention + " for all the rules and info you'll need to know. \nYou will have to accept the TOS of Discord and this server by clicking on the check to gain access to the server.");

                //Gets the "New Member" role and give it to the user
                SocketRole new_member_role = parkourfpsServer.GetRole(777823510072131615);
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
                    SocketGuildUser user = parkourfpsServer.GetUser(user_id);

                    //Removes the "New Member" role from the user
                    SocketRole new_member_role = parkourfpsServer.GetRole(761633924220190732);
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
                    SocketGuildUser user = parkourfpsServer.GetUser(user_id);

                    //Adds the "Bot games" role to the user
                    SocketRole bot_games = parkourfpsServer.GetRole(761700403078889472);
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
                    SocketGuildUser user = parkourfpsServer.GetUser(user_id);

                    //Adds the "New Member" role to the user
                    SocketRole new_member_role = parkourfpsServer.GetRole(761633924220190732);
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
                    SocketGuildUser user = parkourfpsServer.GetUser(user_id);

                    //Removes the "Bot games" role from the user
                    SocketRole bot_games = parkourfpsServer.GetRole(761700403078889472);
                    await user.RemoveRoleAsync(bot_games);
                }
            }
        }

        public static int GetDamerauLevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException(s, "String Cannot Be Null Or Empty");
            }

            if (string.IsNullOrEmpty(t))
            {
                throw new ArgumentNullException(t, "String Cannot Be Null Or Empty");
            }

            int n = s.Length; // length of s
            int m = t.Length; // length of t

            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            int[] p = new int[n + 1]; //'previous' cost array, horizontally
            int[] d = new int[n + 1]; // cost array, horizontally

            // indexes into strings s and t
            int i; // iterates through s
            int j; // iterates through t

            for (i = 0; i <= n; i++)
            {
                p[i] = i;
            }

            for (j = 1; j <= m; j++)
            {
                char tJ = t[j - 1]; // jth character of t
                d[0] = j;

                for (i = 1; i <= n; i++)
                {
                    int cost = s[i - 1] == tJ ? 0 : 1; // cost
                                                       // minimum of cell to the left+1, to the top+1, diagonally left and up +cost                
                    d[i] = Math.Min(Math.Min(d[i - 1] + 1, p[i] + 1), p[i - 1] + cost);
                }

                // copy current distance counts to 'previous row' distance counts
                int[] dPlaceholder = p; //placeholder to assist in swapping p and d
                p = d;
                d = dPlaceholder;
            }

            // our last action in the above loop was to switch d and p, so p now 
            // actually has the most recent cost counts
            return p[n];
        }
    }
}
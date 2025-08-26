using DiscSteam.config;
using DiscSteam.SteamActions.AppDetails;
using DiscSteam.SteamActions.AppDetails.Object;
using DiscSteam.SteamActions.GetGamesFromFamily;
using DiscSteam.SteamActions.GetGamesFromFamily.Object;
using DiscSteam.SteamActions.GetUserInfo;
using DiscSteam.SteamActions.GetUserInfo.Object;
using DSharpPlus;
using DSharpPlus.Entities;

namespace DiscSteam
{
    public class DiscSteam
    {
        private static DiscordClient Client { get; set; }

        private static readonly DiscSteam discSteam = new DiscSteam();

        static async Task Main(string[] args)
        {
            // Initial configurations
            await discSteam.VerifyAndSetConfigs();

            // Connect to discord client
            await discSteam.ConnectToDiscordClient();

            // Repeat the routine every 5 minutes, to update the games
            while (true)
            {
                await discSteam.Routine();
                Console.WriteLine("\nEverything is OK!.");
                Thread.Sleep(300000);
            }
        }

        private async Task Routine()
        {
            var configs = new ReaderJSON();
            await configs.ReadConfigs();

            // Get the discord channel
            var channel = await GetChannelAsync(configs);

            // Get the apps shared in the family
            var sharedLibrary = await GetSharedLibrary(configs);
            var apps = sharedLibrary.Response.Apps;

            // Filter only games
            var games = apps.Where(x => x.AppType == AppType.Game).ToList();

            var lastGameTimeAdquired = configs.LastGameAdquiredTime;
            var teste = games.OrderByDescending(x => x.RtTimeAcquired);
            var newGames = games.OrderBy(x => x.RtTimeAcquired).Where(x => x.RtTimeAcquired >= lastGameTimeAdquired).ToList();
            var imageHeaderOfGames = await GetHeaderImageAppdetails(newGames.Select(x => x.AppId).ToList());

            if (newGames.Any())
            {
                // Set the current time +1 to not repeat the game
                await configs.SetLastGameAdquiredTime(newGames.Select(x => x.RtTimeAcquired + 1).LastOrDefault());
                foreach (var newGame in newGames)
                {
                    // Transform UNIX in real time
                    var acquisitionDateNewGame = DateTimeOffset.FromUnixTimeSeconds(newGame.RtTimeAcquired).ToLocalTime().DateTime;

                    // Get the user profiles of the owners
                    var ownersProfilesNewGames = await discSteam.GetSteamUserInfo(configs, newGame.OwnerSteamIds);
                    var ownersNamesNewgGames = string.Join(" - ", ownersProfilesNewGames.Select(x => x.PersonaName));
                    var headerNewGame = imageHeaderOfGames
                        .Select(x => x.TryGetValue(newGame.AppId, out var gameInfo) ? gameInfo.Data.HeaderImage : null)
                        .FirstOrDefault(header => header != null);

                    // Build the message
                    var messageNewGames =
                    $"```Opa, parece que temos novos jogos!\n" +
                    $"O jogo é: {newGame.Name}\n" +
                    $"Ele foi adquirido em: {acquisitionDateNewGame:dd/MM/yyyy HH:mm:ss}.\n" +
                    $"Quem possui esse jogo: {ownersNamesNewgGames}.```";

                    await channel.SendMessageAsync(messageNewGames);
                    await channel.SendMessageAsync(headerNewGame);
                }
            }
        }

        private async Task VerifyAndSetConfigs()
        {
            var configs = new ReaderJSON();
            await configs.ReadConfigs();

            await SetNewGamesOfFamily(configs);
            await SetTokenBot(configs);
            await SetSteamKey(configs);
            await SetAcessToken(configs);
        }

        private async Task SetNewGamesOfFamily(ReaderJSON configs, bool clearChannel = false)
        {
            if (clearChannel)
            {
                await configs.SetNewGamesOfFamilyChannelId(0);
                await configs.ReadConfigs();
            }

            while (configs.Channels.NewGamesOfFamily == 0)
            {
                //Console.WriteLine("\nPlease insert the discord channel to send the notifications!");

                var newGamesOfFamilyEnviromentVariable = Environment.GetEnvironmentVariable("NEW_GAMES_OF_FAMILY");
                if (ulong.TryParse(newGamesOfFamilyEnviromentVariable, out var id))
                {
                    await configs.SetNewGamesOfFamilyChannelId(id);
                    await configs.ReadConfigs();
                }
                else
                {
                    //Console.WriteLine("Please set a valid channel!");
                }
            }
        }

        private async Task<DiscordChannel> GetChannelAsync(ReaderJSON configs)
        {
            DiscordChannel discordChannel = null;

            while (discordChannel == null)
            {
                try
                {
                    discordChannel = await Client.GetChannelAsync(configs.Channels.NewGamesOfFamily);
                }
                catch
                {
                    //Console.WriteLine("\nThe channel does not exist.");
                    await SetNewGamesOfFamily(configs, clearChannel: true);
                }
            }

            return discordChannel;
        }

        private async Task SetTokenBot(ReaderJSON configs, bool clearToken = false)
        {
            if (clearToken)
            {
                await configs.SetTokenBot(string.Empty);
                await configs.ReadConfigs();
            }

            while (string.IsNullOrEmpty(configs.TokenBot))
            {
                //Console.WriteLine("\nPlease insert your token bot to send the notifications!");

                var tokenBotEnviromentVariable = Environment.GetEnvironmentVariable("TOKEN_BOT");
                await configs.SetTokenBot(tokenBotEnviromentVariable);
                await configs.ReadConfigs();
            }
        }

        private async Task ConnectToDiscordClient()
        {
            var valid = false;
            var configs = new ReaderJSON();
            await configs.ReadConfigs();

            var discordConfigs = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = configs.TokenBot,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            };


            Client = new DiscordClient(discordConfigs);

            while (!valid)
            {
                try
                {
                    await configs.ReadConfigs();

                    discordConfigs.Token = configs.TokenBot;

                    Client = new DiscordClient(discordConfigs);

                    await Client.ConnectAsync();

                    // If the code reaches this, it mean that the connection was successful
                    valid = true;
                }
                catch
                {
                    //Console.WriteLine("\nFailed to authenticate the bot.");
                    await SetTokenBot(configs, clearToken: true);
                }
            }
        }

        private async Task SetAcessToken(ReaderJSON configs, bool clearToken = false)
        {
            if (clearToken)
            {
                await configs.SetAcessToken(string.Empty);
                await configs.ReadConfigs();
            }

            while (string.IsNullOrEmpty(configs.AcessToken))
            {
                //Console.WriteLine("\nPlease insert your token access to connect to the SteamAPI");

                var acessTokenEnviromentVariable = Environment.GetEnvironmentVariable("ACESS_TOKEN");
                await configs.SetAcessToken(acessTokenEnviromentVariable);
                await configs.ReadConfigs();
            }
        }

        private async Task<SteamGamesFamilyResponse> GetSharedLibrary(ReaderJSON configs)
        {
            SteamGamesFamilyResponse sharedLibrary = null;

            while (sharedLibrary == null)
            {
                await configs.ReadConfigs();

                sharedLibrary = await discSteam.GetSharedLibraryApps();
                if (sharedLibrary == null)
                {
                    //Console.WriteLine("\nThe acess token is invalid or expired.");
                    await SetAcessToken(configs, clearToken: true);
                }
            }

            return sharedLibrary;
        }

        private async Task SetSteamKey(ReaderJSON configs, bool clearToken = false)
        {
            if (clearToken)
            {
                await configs.SetSteamKey(string.Empty);
                await configs.ReadConfigs();
            }

            while (string.IsNullOrEmpty(configs.SteamKey))
            {
                //Console.WriteLine("\nPlease insert your Steam key to connect to the Steam API");

                var steamKeyEnviromentVariable = Environment.GetEnvironmentVariable("STEAM_KEY");
                await configs.SetSteamKey(steamKeyEnviromentVariable);
                await configs.ReadConfigs();
            }
        }

        private async Task<List<SteamProfileInfo>> GetSteamUserInfo(ReaderJSON configs, List<string> ownersIds)
        {
            List<SteamProfileInfo> ownersProfilesNewGames = null;

            while (ownersProfilesNewGames == null)
            {
                await configs.ReadConfigs();

                ownersProfilesNewGames = await discSteam.GetSteamUserInfo(ownersIds);
                if (ownersProfilesNewGames == null)
                {
                    //Console.WriteLine("\nThe steam key is invalid or expired.");
                    await SetSteamKey(configs, clearToken: true);
                }
            }

            return ownersProfilesNewGames;
        }

        private async Task<List<Dictionary<int, AppDetailsResponse>>> GetHeaderImageAppdetails(List<int> ids)
        {
            return await new AppDetails().GetHeaderImageAppdetails(ids);
        }

        private async Task<List<SteamProfileInfo>> GetSteamUserInfo(List<string> ids)
        {
            var convertedIds = ids.Select(x => ulong.Parse(x)).ToList();

            var owners = await new GetUserInfo().GetSteamUserInfo(convertedIds);
            if (owners == null)
            {
                return null;
            }

            var ownersProfilesNewGamesOrdenated = new List<SteamProfileInfo>();
            foreach (var id in convertedIds)
            {
                ownersProfilesNewGamesOrdenated.Add(owners.Response.Players.FirstOrDefault(x => ulong.Parse(x.SteamId) == id));
            }

            return ownersProfilesNewGamesOrdenated;
        }

        private async Task<SteamGamesFamilyResponse> GetSharedLibraryApps()
        {
            return await new GetGamesFromFamily().GetSharedLibraryApps();
        }
    }
}

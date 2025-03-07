using DiscSteam.config;
using DiscSteam.SteamActions.GetGamesFromFamily.Object;
using Newtonsoft.Json;
using System.Net;
namespace DiscSteam.SteamActions.GetGamesFromFamily
{
    public class GetGamesFromFamily : SteamActionsBase
    {
        public async Task<SteamGamesFamilyResponse?> GetSharedLibraryApps()
        {
            var configs = new ReaderJSON();
            await configs.ReadConfigs();

            using var client = new HttpClient();
            var uriText = await CreateURITextWithAcessToken(SteamActionsConstantsInterfaces.IFamilyGroupsService, SteamActionsConstantsEndpoints.GetSharedLibraryApps, SteamActionsConstantsVersions.v1);
            var endpoint = new Uri(uriText + $"&family_groupid={configs.FamilyGroupId}&include_own=true&include_excluded=true&include_free=true&include_non_games=true");
            var result = await client.GetAsync(endpoint);

            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                return null;
            }

            var json = await result.Content.ReadAsStringAsync();
            var apps = JsonConvert.DeserializeObject<SteamGamesFamilyResponse>(json);

            if (result.IsSuccessStatusCode)
            {
                return apps;
            }

            return null;
        }
    }
}

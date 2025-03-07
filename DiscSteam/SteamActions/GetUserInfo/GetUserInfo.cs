using DiscSteam.config;
using DiscSteam.SteamActions.GetUserInfo.Object;
using Newtonsoft.Json;
using System.Net;
namespace DiscSteam.SteamActions.GetUserInfo
{
    public class GetUserInfo : SteamActionsBase
    {
        public async Task<SteamProfilesInfoResponse?> GetSteamUserInfo(List<ulong> steamIds)
        {
            var configs = new ReaderJSON();
            await configs.ReadConfigs();

            var idsFormatados = string.Join(";", steamIds);
            using var client = new HttpClient();
            var uriText = await CreateURITextWithSteamKey(SteamActionsConstantsInterfaces.ISteamUser, SteamActionsConstantsEndpoints.GetPlayerSummaries, SteamActionsConstantsVersions.v0002);
            var endpoint = new Uri(uriText + $"steamids={idsFormatados}");
            var result = await client.GetAsync(endpoint);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var json = await result.Content.ReadAsStringAsync();
            var perfil = JsonConvert.DeserializeObject<SteamProfilesInfoResponse>(json);

            if (result.IsSuccessStatusCode)
            {
                return perfil;
            }

            return null;
        }
    }
}

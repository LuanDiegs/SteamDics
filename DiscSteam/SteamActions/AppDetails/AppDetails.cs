using DiscSteam.config;
using DiscSteam.SteamActions.AppDetails.Object;
using Newtonsoft.Json;
namespace DiscSteam.SteamActions.AppDetails
{
    public class AppDetails : SteamActionsBase
    {
        public async Task<List<Dictionary<int, AppDetailsResponse>>> GetHeaderImageAppdetails(List<int> ids)
        {
            var configs = new ReaderJSON();
            await configs.ReadConfigs();

            var headersImages = new List<Dictionary<int, AppDetailsResponse>>();
            using var client = new HttpClient();
            foreach (var id in ids)
            {
                var uriText = CreateStoreURIBase(SteamActionsConstantsEndpoints.Appdetails);
                var endpoint = new Uri(uriText + $"?appids={id}&filters=basic");
                var result = await client.GetAsync(endpoint);
                var json = await result.Content.ReadAsStringAsync();
                var apps = JsonConvert.DeserializeObject<Dictionary<int, AppDetailsResponse>>(json);

                if (result.IsSuccessStatusCode)
                {
                    headersImages.Add(apps);
                }
            }

            return headersImages;
        }
    }
}

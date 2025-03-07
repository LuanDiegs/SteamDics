using Newtonsoft.Json;

namespace DiscSteam.SteamActions.AppDetails.Object
{
    public class AppDetailsResponse
    {
        public AppDetailsResponseInfo Data { get; set; }
    }

    public class AppDetailsResponseInfo
    {
        [JsonProperty("header_image")]
        public string HeaderImage { get; set; }

    }
}

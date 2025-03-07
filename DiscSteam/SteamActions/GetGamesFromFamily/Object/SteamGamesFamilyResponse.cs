using Newtonsoft.Json;

namespace DiscSteam.SteamActions.GetGamesFromFamily.Object
{
    public class SteamGamesFamilyResponse
    {
        public SteamGamesFamilyInfo Response { get; set; }
    }

    public class SteamGamesFamilyInfo
    {
        public List<SteamGameFamilyInfo> Apps { get; set; }
    }

    public class SteamGameFamilyInfo
    {
        public int AppId { get; set; }

        [JsonProperty("owner_steamids")]
        public List<string> OwnerSteamIds { get; set; }

        public string Name { get; set; }

        public string CapsuleFilename { get; set; }

        [JsonProperty("img_icon_hash")]
        public string ImgIconHash { get; set; }

        public int ExcludeReason { get; set; }

        [JsonProperty("rt_time_acquired")]
        public long RtTimeAcquired { get; set; }

        public long RtLastPlayed { get; set; }

        public long RtPlaytime { get; set; }

        [JsonProperty("app_type")]
        public AppType AppType { get; set; }

        public List<int> ContentDescriptors { get; set; }
    }

    public enum AppType
    {
        Game = 1,
        Software = 2,
        IDK = 3,
        Tools = 4
    }
}

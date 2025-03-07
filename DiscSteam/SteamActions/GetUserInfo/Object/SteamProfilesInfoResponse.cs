namespace DiscSteam.SteamActions.GetUserInfo.Object
{
    public class SteamProfilesInfoResponse
    {
        public SteamProfilesInfo Response { get; set; }
    }

    public class SteamProfilesInfo
    {
        public List<SteamProfileInfo> Players { get; set; }
    }

    public class SteamProfileInfo
    {
        public string SteamId { get; set; }

        public int CommunityVisibilityState { get; set; }

        public int ProfileState { get; set; }

        public string PersonaName { get; set; }

        public int CommentPermission { get; set; }

        public string ProfileUrl { get; set; }

        public string Avatar { get; set; }

        public string AvatarMedium { get; set; }

        public string AvatarFull { get; set; }

        public string AvatarHash { get; set; }

        public long LastLogoff { get; set; }

        public int PersonaState { get; set; }

        public string RealName { get; set; }

        public string PrimaryClanId { get; set; }

        public long TimeCreated { get; set; }

        public int PersonaStateFlags { get; set; }

        public string LocCountryCode { get; set; }

        public string LocStateCode { get; set; }

        public int LocCityId { get; set; }
    }
}

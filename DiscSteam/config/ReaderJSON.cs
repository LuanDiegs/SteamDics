using Newtonsoft.Json;

namespace DiscSteam.config
{
    internal class ReaderJSON
    {
        public ReaderJSON()
        {
            VerifyIfPathExistsAndSetPath();
        }

        private string Path {  get; set; }

        public string TokenBot { get; set; }

        public Channels Channels { get; set; }

        public string SteamKey { get; set; }

        public string AcessToken { get; set; }

        public long FamilyGroupId { get; set; }

        public long LastGameAdquiredTime { get; set; }

        public async Task ReadConfigs()
        {
            JSONStructure? data;

            using (StreamReader str = new(Path))
            {
                string json = await str.ReadToEndAsync();

                data = JsonConvert.DeserializeObject<JSONStructure>(json);
                if (data == null)
                {
                    throw new Exception("No data to process.");
                }

                this.TokenBot = data.TokenBot;
                this.Channels = data.Channels;
                this.SteamKey = data.SteamKey;
                this.FamilyGroupId = data.FamilyGroupId;
                this.AcessToken = data.AcessToken;
                this.LastGameAdquiredTime = data.LastGameAdquiredTime;
            }

            using (StreamWriter str = new(Path))
            {
                string updatedJson = JsonConvert.SerializeObject(data, Formatting.Indented);
                await str.WriteAsync(updatedJson);
            }
        }

        private void VerifyIfPathExistsAndSetPath()
        {
            string path;
            if (File.Exists("../../../config/config.json"))
            {
                path = "../../../config/config.json";
            }
            else if(File.Exists("../../app/config/config.json"))
            {
                path = "../../app/config/config.json";
            }
            else
            {
                throw new Exception("The config file was not found.");
            }

            Path = path;
        }

        public async Task SetLastGameAdquiredTime(long lastGameAdquiredTime)
        {
            JSONStructure data = await ReadJSON();

            using StreamWriter str = new(Path);
            data.LastGameAdquiredTime = lastGameAdquiredTime;
            string updatedJson = JsonConvert.SerializeObject(data, Formatting.Indented);
            await str.WriteAsync(updatedJson);
        }

        public async Task SetNewGamesOfFamilyChannelId(ulong newGamesOfFamilyChannelId)
        {
            JSONStructure data = await ReadJSON();

            using StreamWriter str = new(Path);
            data.Channels.NewGamesOfFamily = newGamesOfFamilyChannelId;
            string updatedJson = JsonConvert.SerializeObject(data, Formatting.Indented);
            await str.WriteAsync(updatedJson);
        }

        public async Task SetTokenBot(string tokenBot)
        {
            JSONStructure data = await ReadJSON();

            using StreamWriter str = new(Path);
            data.TokenBot = tokenBot;
            string updatedJson = JsonConvert.SerializeObject(data, Formatting.Indented);
            await str.WriteAsync(updatedJson);
        }

        public async Task SetSteamKey(string steamKey)
        {
            JSONStructure data = await ReadJSON();

            using StreamWriter str = new(Path);
            data.SteamKey = steamKey;
            string updatedJson = JsonConvert.SerializeObject(data, Formatting.Indented);
            await str.WriteAsync(updatedJson);
        }

        public async Task SetAcessToken(string acessToken)
        {
            JSONStructure data = await ReadJSON();

            using StreamWriter str = new(Path);
            data.AcessToken = acessToken;
            string updatedJson = JsonConvert.SerializeObject(data, Formatting.Indented);
            await str.WriteAsync(updatedJson);
        }

        private async Task<JSONStructure> ReadJSON()
        {
            JSONStructure? data;
            using (StreamReader str = new(Path))
            {
                string json = await str.ReadToEndAsync();

                data = JsonConvert.DeserializeObject<JSONStructure>(json);
                if (data == null)
                {
                    throw new Exception("No data to process.");
                }
            }

            return data;
        }
    }

    internal sealed class JSONStructure
    {
        public string TokenBot { get; set; }


        public Channels Channels { get; set; }

        public string SteamKey { get; set; }

        public string AcessToken { get; set; }

        public long FamilyGroupId { get; set; }

        public long LastGameAdquiredTime { get; set; }
    }

    internal sealed class Channels
    {
        public ulong NewGamesOfFamily { get; set; }
    }
}

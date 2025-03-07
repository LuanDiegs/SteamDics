using DiscSteam.config;
using System.Threading.Tasks;

namespace DiscSteam.SteamActions
{
    public class SteamActionsBase
    {
        protected async Task<string> CreateURITextWithSteamKey(string interfa, string endpoint, string version)
        {
            var configs = new ReaderJSON();
            await configs.ReadConfigs();

            var URIBase = CreateURIBase(interfa, endpoint, version);
            return URIBase + $"?key={configs.SteamKey}&";
        }

        protected async Task<string> CreateURITextWithAcessToken(string interfa, string endpoint, string version)
        {
            var configs = new ReaderJSON();
            await configs.ReadConfigs();

            var URIBase = CreateURIBase(interfa, endpoint, version);
            return URIBase + $"?access_token={configs.AcessToken}";
        }

        public string CreateStoreURIBase(string endpoint)
        {
            return $"{SteamActionsConstants.STORE_URI_DEFAULT}/{endpoint}";
        }

        private string CreateURIBase(string interfa, string endpoint, string version)
        {
            return $"{SteamActionsConstants.URI_DEFAULT}/{interfa}/{endpoint}/{version}/";
        }

    }
}

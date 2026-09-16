using LeagueOfSmurfs.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace LeagueOfSmurfs.Utils
{
    /// <summary>
    /// Valorant player data via HenrikDev (official Riot API has no personal RR endpoint).
    /// Key from https://api.henrikdev.xyz/dashboard/
    /// </summary>
    public static class ValorantUtils
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private const string BaseUrl = "https://api.henrikdev.xyz";

        public static string getAffinity(RegionEnum region)
        {
            switch (region)
            {
                case RegionEnum.NA:
                    return "na";
                case RegionEnum.BR:
                    return "br";
                case RegionEnum.LAN:
                case RegionEnum.LAS:
                    return "latam";
                case RegionEnum.KR:
                    return "kr";
                case RegionEnum.JP:
                case RegionEnum.OCE:
                case RegionEnum.SG:
                case RegionEnum.TW:
                case RegionEnum.TH:
                case RegionEnum.VN:
                    return "ap";
                case RegionEnum.EUW:
                case RegionEnum.EUNE:
                case RegionEnum.TR:
                case RegionEnum.RU:
                default:
                    return "eu";
            }
        }

        public static async Task<bool> checkApiKeyAsync(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                return false;

            try
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + "/valorant/v1/version"))
                {
                    AddAuth(request, apiKey);
                    using (HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        // 200 = ok; 429 = key accepted but rate-limited
                        if (response.IsSuccessStatusCode || (int)response.StatusCode == 429)
                            return true;
                        Debug.WriteLine("Valorant API key check failed: " + (int)response.StatusCode);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Valorant API key check error: " + ex.Message);
                return false;
            }
        }

        public static async Task<ValorantAccountInfo> GetAccountByNameAsync(string apiKey, string name, string tag)
        {
            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(tag))
                return null;

            string url = BaseUrl + "/valorant/v2/account/"
                + Uri.EscapeDataString(name.Trim()) + "/"
                + Uri.EscapeDataString(tag.Trim());

            JObject root = await GetJsonAsync(apiKey, url).ConfigureAwait(false);
            JToken data = root?["data"];
            if (data == null)
                return null;

            return new ValorantAccountInfo
            {
                Puuid = data.Value<string>("puuid"),
                Name = data.Value<string>("name"),
                Tag = data.Value<string>("tag"),
                Region = data.Value<string>("region"),
                Level = data.Value<int?>("account_level") ?? 0
            };
        }

        public static async Task<ValorantMmrInfo> GetMmrByNameAsync(string apiKey, RegionEnum region, string name, string tag)
        {
            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(tag))
                return null;

            string affinity = getAffinity(region);
            string url = BaseUrl + "/valorant/v3/mmr/" + affinity + "/pc/"
                + Uri.EscapeDataString(name.Trim()) + "/"
                + Uri.EscapeDataString(tag.Trim());

            return ParseMmr(await GetJsonAsync(apiKey, url).ConfigureAwait(false));
        }

        public static async Task<ValorantMmrInfo> GetMmrByPuuidAsync(string apiKey, RegionEnum region, string puuid)
        {
            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(puuid))
                return null;

            string affinity = getAffinity(region);
            string url = BaseUrl + "/valorant/v3/by-puuid/mmr/" + affinity + "/pc/"
                + Uri.EscapeDataString(puuid.Trim());

            return ParseMmr(await GetJsonAsync(apiKey, url).ConfigureAwait(false));
        }

        private static ValorantMmrInfo ParseMmr(JObject root)
        {
            JToken data = root?["data"];
            if (data == null)
                return null;

            JToken account = data["account"];
            JToken current = data["current"];
            JToken peak = data["peak"];

            int currentTier = current?["tier"]?.Value<int?>("id") ?? 0;
            int peakTier = peak?["tier"]?.Value<int?>("id") ?? 0;

            return new ValorantMmrInfo
            {
                Puuid = account?.Value<string>("puuid"),
                Name = account?.Value<string>("name"),
                Tag = account?.Value<string>("tag"),
                CurrentTierId = currentTier,
                CurrentTierName = current?["tier"]?.Value<string>("name"),
                RR = current?.Value<int?>("rr") ?? 0,
                PeakTierId = peakTier,
                PeakTierName = peak?["tier"]?.Value<string>("name")
            };
        }

        private static async Task<JObject> GetJsonAsync(string apiKey, string url)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                AddAuth(request, apiKey);
                using (HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false))
                {
                    string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Valorant API failed (" + (int)response.StatusCode + "): " + body);
                        response.EnsureSuccessStatusCode();
                    }
                    return JObject.Parse(body);
                }
            }
        }

        private static void AddAuth(HttpRequestMessage request, string apiKey)
        {
            string key = (apiKey ?? string.Empty).Trim();
            request.Headers.TryAddWithoutValidation("Authorization", key);
        }
    }

    public class ValorantAccountInfo
    {
        public string Puuid { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Region { get; set; }
        public int Level { get; set; }
    }

    public class ValorantMmrInfo
    {
        public string Puuid { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public int CurrentTierId { get; set; }
        public string CurrentTierName { get; set; }
        public int RR { get; set; }
        public int PeakTierId { get; set; }
        public string PeakTierName { get; set; }
    }
}

using LeagueOfSmurfs.Configuration;
using Microsoft.Win32;
using Newtonsoft.Json;
using RiotSharp;
using RiotSharp.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace LeagueOfSmurfs.Utils
{
    public class RiotUtils
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static string getRiotPath()
        {
            string path = null;

            // Check basic path
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                path = Path.Combine(drive.Name, "Riot Games\\Riot Client\\RiotClientServices.exe");
                if (File.Exists(drive.Name))
                    return path;
                path = Path.Combine(drive.Name, "Programs\\Riot Games\\Riot Client\\RiotClientServices.exe");
                if (File.Exists(drive.Name))
                    return path;
            }

            // Get registry uninstall path
            path = null;
            RegistryKey uninstall = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            if (uninstall == null)
            {
                Debug.WriteLine("Uninstall not found!");
                return (null);
            }
            foreach (string name  in uninstall.GetSubKeyNames())
            {
                if (name.ToLower().Contains("riot_client"))
                {
                    path = uninstall.OpenSubKey(name).GetValue("InstallLocation").ToString();
                }
            }
            if (path != null)
                return (Path.Combine(path, "RiotClientServices.exe"));


            // Get registry uninstall path
            path = null;
            RegistryKey switched = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FeatureUsage\\AppSwitched");
            if (switched == null)
            {
                Debug.WriteLine("AppSwitched not found!");
                return (null);
            }
            foreach (string name in switched.GetValueNames())
            {
                if (name.ToLower().Contains("riotclientux"))
                {
                    path = Directory.GetParent(name).Parent.FullName;
                }
            }
            if (path != null)
                return (Path.Combine(path, "RiotClientServices.exe"));
            return (path);
        }

        public static bool launchClient()
        {
            string ritoPath = getRiotPath();

            if (ritoPath == null)
            {
                Debug.WriteLine("Can't find riot path");
                return (false);
            }
            Process applicationProcess = Process.Start(ritoPath);
            applicationProcess.WaitForInputIdle();
            return (true);
        }
        public static bool launchLeague()
        {
            string ritoPath = getRiotPath();

            if (ritoPath == null)
            {
                Debug.WriteLine("Can't find riot path");
                return (false);
            }
            Process applicationProcess = Process.Start(ritoPath, "--launch-product=league_of_legends --launch-patchline=live");
            applicationProcess.WaitForInputIdle();
            return (true);
        }

        public static void closeRiot()
        {
            Process[] proc = Process.GetProcesses();
            foreach (Process process in proc)
            {
                string name = process.ProcessName;
                // Modern client is "Riot Client" (with space); older builds used RiotClientUx / RiotClientServices
                if (name.IndexOf("Riot", StringComparison.OrdinalIgnoreCase) >= 0
                    && name.IndexOf("Client", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    try { process.Kill(); } catch { /* ignore */ }
                }
                if (name.IndexOf("LeagueClient", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    try { process.Kill(); } catch { /* ignore */ }
                }
            }
        }
        public static RegionEnum getRegionByName(string name)
        {
            RegionEnum regionEnum = RegionEnum.EUW;
            foreach (RegionEnum region in (RegionEnum[])Enum.GetValues(typeof(RegionEnum)))
            {
                if (region.ToString().Equals(name))
                    return region;
            }
            Debug.WriteLine("Can't find rank " + regionEnum);
            return RegionEnum.EUW;
        }

        public static Region getRegion(RegionEnum regionEnum)
        {
            switch (regionEnum)
            {
                case RegionEnum.BR:
                    return Region.Br;
                case RegionEnum.EUW:
                    return Region.Euw;
                case RegionEnum.EUNE:
                    return Region.Eune;
                case RegionEnum.LAN:
                    return Region.Lan;
                case RegionEnum.LAS:
                    return Region.Las;
                case RegionEnum.NA:
                    return Region.Na;
                case RegionEnum.OCE:
                    return Region.Oce;
                case RegionEnum.RU:
                    return Region.Ru;
                case RegionEnum.TR:
                    return Region.Tr;
                case RegionEnum.JP:
                    return Region.Jp;
                case RegionEnum.KR:
                    return Region.Kr;
                // SEA platforms unsupported by this RiotSharp fork's Region enum.
                // Platform calls use getPlatformHost() instead.
                case RegionEnum.SG:
                case RegionEnum.TW:
                case RegionEnum.TH:
                case RegionEnum.VN:
                    return Region.Oce;
                case RegionEnum.PBE:
                    return Region.NoRegion;
            }
            return Region.Euw;
        }

        /// <summary>
        /// Platform routing for summoner/league/spectator (e.g. euw1, sg2).
        /// </summary>
        public static string getPlatformHost(RegionEnum regionEnum)
        {
            switch (regionEnum)
            {
                case RegionEnum.BR:
                    return "br1";
                case RegionEnum.EUNE:
                    return "eun1";
                case RegionEnum.EUW:
                    return "euw1";
                case RegionEnum.JP:
                    return "jp1";
                case RegionEnum.KR:
                    return "kr";
                case RegionEnum.LAN:
                    return "la1";
                case RegionEnum.LAS:
                    return "la2";
                case RegionEnum.NA:
                    return "na1";
                case RegionEnum.OCE:
                    return "oc1";
                case RegionEnum.TR:
                    return "tr1";
                case RegionEnum.RU:
                    return "ru";
                case RegionEnum.SG:
                    return "sg2";
                case RegionEnum.TW:
                    return "tw2";
                case RegionEnum.TH:
                    return "th2";
                case RegionEnum.VN:
                    return "vn2";
                default:
                    return "euw1";
            }
        }

        /// <summary>
        /// account-v1 routing: americas | asia | europe only.
        /// </summary>
        public static Region getAccountRegion(RegionEnum regionEnum)
        {
            switch (regionEnum)
            {
                case RegionEnum.TR:
                case RegionEnum.RU:
                case RegionEnum.EUW:
                case RegionEnum.EUNE:
                    return Region.Europe;
                case RegionEnum.JP:
                case RegionEnum.KR:
                case RegionEnum.OCE:
                case RegionEnum.SG:
                case RegionEnum.TW:
                case RegionEnum.TH:
                case RegionEnum.VN:
                    return Region.Asia;
                case RegionEnum.BR:
                case RegionEnum.LAN:
                case RegionEnum.LAS:
                case RegionEnum.NA:
                    return Region.Americas;
                case RegionEnum.PBE:
                    return Region.NoRegion;
            }
            return Region.Europe;
        }

        /// <summary>
        /// GET /lol/league/v4/entries/by-puuid/{puuid}
        /// Replaces by-summoner: summoner IDs were removed from SummonerDTO (July 2025).
        /// </summary>
        public static async Task<List<LeagueEntryInfo>> GetLeagueEntriesByPuuidAsync(string apiKey, RegionEnum region, string puuid)
        {
            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrEmpty(puuid))
                return new List<LeagueEntryInfo>();

            string host = getPlatformHost(region);
            string url = $"https://{host}.api.riotgames.com/lol/league/v4/entries/by-puuid/{Uri.EscapeDataString(puuid)}";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.Headers.Add("X-Riot-Token", apiKey);
                using (HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false))
                {
                    string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine($"League by-puuid failed ({(int)response.StatusCode}): {body}");
                        response.EnsureSuccessStatusCode();
                    }
                    return JsonConvert.DeserializeObject<List<LeagueEntryInfo>>(body) ?? new List<LeagueEntryInfo>();
                }
            }
        }

        /// <summary>
        /// GET /lol/summoner/v4/summoners/by-puuid/{puuid} via platform host
        /// (needed for SEA platforms not covered by RiotSharp's Region enum).
        /// </summary>
        public static async Task<SummonerLevelInfo> GetSummonerByPuuidAsync(string apiKey, RegionEnum region, string puuid)
        {
            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrEmpty(puuid))
                return null;

            string host = getPlatformHost(region);
            string url = $"https://{host}.api.riotgames.com/lol/summoner/v4/summoners/by-puuid/{Uri.EscapeDataString(puuid)}";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.Headers.Add("X-Riot-Token", apiKey);
                using (HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false))
                {
                    string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine($"Summoner by-puuid failed ({(int)response.StatusCode}): {body}");
                        response.EnsureSuccessStatusCode();
                    }
                    return JsonConvert.DeserializeObject<SummonerLevelInfo>(body);
                }
            }
        }

        public static RiotApi checkAPI(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                return null;

            RiotApi api = RiotApi.GetInstance(apiKey.Trim(), 10, 5000);
            try
            {
                var test = api.Champion.GetChampionRotationAsync(Region.Euw);
                if (test == null || test.Result == null)
                {
                    Debug.WriteLine("Api Invalid Result");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Api Key Invalid: " + apiKey + " " + e.Message);
                return null;
            }
            return api;
        }

    }

    /// <summary>
    /// Minimal SummonerDTO after Riot removed id / accountId / name fields.
    /// </summary>
    public class SummonerLevelInfo
    {
        [JsonProperty("puuid")]
        public string Puuid { get; set; }

        [JsonProperty("profileIconId")]
        public int ProfileIconId { get; set; }

        [JsonProperty("revisionDate")]
        public long RevisionDate { get; set; }

        [JsonProperty("summonerLevel")]
        public long Level { get; set; }
    }

    /// <summary>
    /// LeagueEntryDTO fields used by the app.
    /// </summary>
    public class LeagueEntryInfo
    {
        [JsonProperty("queueType")]
        public string QueueType { get; set; }

        [JsonProperty("tier")]
        public string Tier { get; set; }

        [JsonProperty("rank")]
        public string Rank { get; set; }

        [JsonProperty("leaguePoints")]
        public int LeaguePoints { get; set; }
    }
}

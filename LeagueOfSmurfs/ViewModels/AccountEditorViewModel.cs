using LeagueOfSmurfs.Configuration;
using LeagueOfSmurfs.Configurations;
using LeagueOfSmurfs.Mvvm;
using LeagueOfSmurfs.Utils;
using RiotSharp;
using RiotSharp.Endpoints.AccountEndpoint;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LeagueOfSmurfs.ViewModels
{
    public sealed class AccountEditorViewModel : ViewModelBase
    {
        private readonly ConfigurationManager confManager;
        private readonly RiotApi api;
        private readonly Action onSaved;
        private readonly string originalPuuid;
        private readonly SmurfsConfiguration draft;
        private bool suppressReset;

        private string gameName;
        private string tag;
        private string username;
        private string password;
        private string errorText;
        private bool errorVisible;
        private string previewName;
        private string levelText;
        private string leftTitle;
        private string leftRank;
        private string leftScore;
        private string rightTitle;
        private string rightRank;
        private string rightScore;
        private string identityTitle;
        private RegionEnum selectedRegion;
        private bool passwordVisible;

        public AccountEditorViewModel(
            ConfigurationManager confManager,
            RiotApi api,
            SmurfsConfiguration existing,
            Action onSaved)
        {
            this.confManager = confManager;
            this.api = api;
            this.onSaved = onSaved;
            this.draft = existing != null ? Clone(existing) : new SmurfsConfiguration();
            this.originalPuuid = existing?.puuid;

            Regions = Enum.GetValues(typeof(RegionEnum)).Cast<RegionEnum>().ToList();
            selectedRegion = draft.region;

            suppressReset = true;
            if (!string.IsNullOrWhiteSpace(draft.summonerName))
            {
                string name = draft.summonerName;
                string t = string.Empty;
                int hash = name.IndexOf('#');
                if (hash >= 0)
                {
                    t = name.Substring(hash + 1);
                    name = name.Substring(0, hash);
                }
                gameName = name;
                tag = t;
            }
            username = draft.username;
            password = draft.password;
            suppressReset = false;

            CheckCommand = new AsyncRelayCommand(_ => CheckAsync());
            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
            TogglePasswordCommand = new RelayCommand(_ => PasswordVisible = !PasswordVisible);
            RefreshPreview();
            ApplyModeTitles();
        }

        public bool IsEdit { get { return !string.IsNullOrWhiteSpace(originalPuuid); } }
        public List<RegionEnum> Regions { get; }

        public string GameName
        {
            get { return gameName; }
            set
            {
                if (Set(ref gameName, value) && !suppressReset)
                    ResetIdentity();
            }
        }

        public string Tag
        {
            get { return tag; }
            set
            {
                if (Set(ref tag, value) && !suppressReset)
                    ResetIdentity();
            }
        }

        public string Username
        {
            get { return username; }
            set { Set(ref username, value); }
        }

        public string Password
        {
            get { return password; }
            set { Set(ref password, value); }
        }

        public bool PasswordVisible
        {
            get { return passwordVisible; }
            set { Set(ref passwordVisible, value); }
        }

        public RegionEnum SelectedRegion
        {
            get { return selectedRegion; }
            set
            {
                if (Set(ref selectedRegion, value))
                    draft.region = value;
            }
        }

        public string ErrorText
        {
            get { return errorText; }
            set { Set(ref errorText, value); }
        }

        public bool ErrorVisible
        {
            get { return errorVisible; }
            set { Set(ref errorVisible, value); }
        }

        public string IdentityTitle { get { return identityTitle; } private set { Set(ref identityTitle, value); } }
        public string PreviewName { get { return previewName; } private set { Set(ref previewName, value); } }
        public string LevelText { get { return levelText; } private set { Set(ref levelText, value); } }
        public string LeftTitle { get { return leftTitle; } private set { Set(ref leftTitle, value); } }
        public string LeftRank { get { return leftRank; } private set { Set(ref leftRank, value); } }
        public string LeftScore { get { return leftScore; } private set { Set(ref leftScore, value); } }
        public string RightTitle { get { return rightTitle; } private set { Set(ref rightTitle, value); } }
        public string RightRank { get { return rightRank; } private set { Set(ref rightRank, value); } }
        public string RightScore { get { return rightScore; } private set { Set(ref rightScore, value); } }

        public ICommand CheckCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand TogglePasswordCommand { get; }

        private void ApplyModeTitles()
        {
            if (AppTheme.IsValorant)
            {
                IdentityTitle = "Agent";
                LeftTitle = "Current";
                RightTitle = "Peak";
            }
            else
            {
                IdentityTitle = "Summoner";
                LeftTitle = "Flex";
                RightTitle = "Solo/Duo";
            }
        }

        private void RefreshPreview()
        {
            PreviewName = draft.summonerName;
            LevelText = "Level: " + draft.level;
            if (AppTheme.IsValorant)
            {
                LeftRank = "Rank: " + RankedUtils.valorantRankToString(draft.valRank);
                LeftScore = "RR: " + draft.valRR;
                RightRank = "Rank: " + RankedUtils.valorantRankToString(draft.valPeakRank);
                RightScore = string.Empty;
            }
            else
            {
                LeftRank = "Ladder: " + RankedUtils.rankToString(draft.flexRank);
                LeftScore = "LP: " + draft.flexLP;
                RightRank = "Ladder: " + RankedUtils.rankToString(draft.soloRank);
                RightScore = "LP: " + draft.soloLP;
            }
        }

        private void ResetIdentity()
        {
            draft.puuid = string.Empty;
            draft.encryptedId = string.Empty;
            draft.summonerName = string.Empty;
            ErrorVisible = false;
            RefreshPreview();
        }

        private async Task CheckAsync()
        {
            ApplyModeTitles();
            if (string.IsNullOrWhiteSpace(GameName) || GameName.Trim().Length < 3)
            {
                ShowError("Name is too short");
                return;
            }
            if (string.IsNullOrWhiteSpace(Tag))
            {
                ShowError("Tag required");
                return;
            }

            try
            {
                if (AppTheme.IsValorant)
                    await CheckValorantAsync().ConfigureAwait(true);
                else
                    await CheckLeagueAsync().ConfigureAwait(true);
                ErrorVisible = false;
                RefreshPreview();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Check failed: " + ex.Message);
                ShowError("Invalid Name");
            }
        }

        private async Task CheckLeagueAsync()
        {
            if (api == null || string.IsNullOrWhiteSpace(confManager.apiKey))
            {
                ShowError("No API key");
                return;
            }

            Account account = await api.Account.GetAccountByRiotIdAsync(
                RiotUtils.getAccountRegion(SelectedRegion), GameName.Trim(), Tag.Trim()).ConfigureAwait(true);
            if (account == null || string.IsNullOrWhiteSpace(account.Puuid))
                throw new Exception("Empty account");

            string riotId = account.GameName + "#" + account.TagLine;
            SummonerLevelInfo summoner = await RiotUtils.GetSummonerByPuuidAsync(
                confManager.apiKey, SelectedRegion, account.Puuid).ConfigureAwait(true);
            if (summoner == null || string.IsNullOrWhiteSpace(summoner.Puuid))
                throw new Exception("Empty summoner");

            draft.puuid = summoner.Puuid;
            draft.encryptedId = string.Empty;
            draft.summonerName = riotId;
            draft.level = summoner.Level;
            draft.region = SelectedRegion;

            List<LeagueEntryInfo> entries = await RiotUtils.GetLeagueEntriesByPuuidAsync(
                confManager.apiKey, SelectedRegion, summoner.Puuid).ConfigureAwait(true);
            draft.soloRank = RankEnum.UNRANKED;
            draft.flexRank = RankEnum.UNRANKED;
            foreach (LeagueEntryInfo entry in entries)
            {
                if (entry.QueueType == "RANKED_FLEX_SR")
                {
                    draft.flexRank = RankedUtils.getRankByEntry(entry);
                    draft.flexLP = entry.LeaguePoints;
                }
                if (entry.QueueType == "RANKED_SOLO_5x5")
                {
                    draft.soloRank = RankedUtils.getRankByEntry(entry);
                    draft.soloLP = entry.LeaguePoints;
                }
            }
        }

        private async Task CheckValorantAsync()
        {
            if (string.IsNullOrWhiteSpace(confManager.valorantApiKey))
            {
                ShowError("No Valorant API key");
                return;
            }

            ValorantAccountInfo account = await ValorantUtils.GetAccountByNameAsync(
                confManager.valorantApiKey, GameName.Trim(), Tag.Trim()).ConfigureAwait(true);
            if (account == null || string.IsNullOrWhiteSpace(account.Puuid))
                throw new Exception("Empty account");

            ValorantMmrInfo mmr = await ValorantUtils.GetMmrByNameAsync(
                confManager.valorantApiKey, SelectedRegion, GameName.Trim(), Tag.Trim()).ConfigureAwait(true);
            if (mmr == null)
                throw new Exception("Empty MMR");

            draft.puuid = account.Puuid;
            draft.encryptedId = string.Empty;
            draft.summonerName = account.Name + "#" + account.Tag;
            draft.level = account.Level;
            draft.region = SelectedRegion;
            draft.valRank = RankedUtils.getValorantRankByTierId(mmr.CurrentTierId);
            draft.valRR = mmr.RR;
            draft.valPeakRank = RankedUtils.getValorantRankByTierId(mmr.PeakTierId);
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Username)
                && !string.IsNullOrWhiteSpace(Password)
                && Username.Trim().Length >= 3
                && Password.Trim().Length >= 3;
        }

        private void Save()
        {
            draft.username = Username?.Trim();
            draft.password = Password;
            draft.region = SelectedRegion;

            if (string.IsNullOrWhiteSpace(draft.puuid))
            {
                if (!string.IsNullOrWhiteSpace(originalPuuid))
                    draft.puuid = originalPuuid;
                else
                    draft.puuid = "local-" + Guid.NewGuid().ToString("N");
            }

            if (string.IsNullOrWhiteSpace(draft.summonerName))
            {
                string name = (GameName ?? string.Empty).Trim();
                string t = (Tag ?? string.Empty).Trim();
                draft.summonerName = string.IsNullOrWhiteSpace(t) ? name : name + "#" + t;
            }

            if (!confManager.Add(draft))
            {
                ShowError("Could not save account");
                return;
            }

            onSaved?.Invoke();
        }

        private void ShowError(string text)
        {
            ErrorText = text;
            ErrorVisible = true;
        }

        private static SmurfsConfiguration Clone(SmurfsConfiguration conf)
        {
            return new SmurfsConfiguration
            {
                puuid = conf.puuid,
                encryptedId = conf.encryptedId,
                username = conf.username,
                password = conf.password,
                region = conf.region,
                summonerName = conf.summonerName,
                level = conf.level,
                flexRank = conf.flexRank,
                flexLP = conf.flexLP,
                soloRank = conf.soloRank,
                soloLP = conf.soloLP,
                valRank = conf.valRank,
                valRR = conf.valRR,
                valPeakRank = conf.valPeakRank
            };
        }
    }
}

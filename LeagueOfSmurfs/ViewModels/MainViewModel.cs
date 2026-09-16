using LeagueOfSmurfs.Configuration;
using LeagueOfSmurfs.Configurations;
using LeagueOfSmurfs.Mvvm;
using LeagueOfSmurfs.Services;
using LeagueOfSmurfs.Utils;
using LeagueOfSmurfs.Views;
using RiotSharp;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DrawingImage = System.Drawing.Image;

namespace LeagueOfSmurfs.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        private readonly ConfigurationManager confManager;
        private RiotApi api;
        private bool valorantApiValid;
        private string validatedRiotKey;
        private string validatedValorantKey;
        private string apiKeyText;
        private bool apiKeyVisible;
        private bool apiValid;
        private BitmapSource apiStatusImage;
        private BitmapSource logoImage;
        private BitmapSource titleImage;
        private BitmapSource backgroundImage;
        private BitmapSource minimizeImage;
        private BitmapSource closeImage;
        private BitmapSource addImage;
        private BitmapSource refreshImage;
        private BitmapSource editImage;
        private BitmapSource deleteImage;
        private BitmapSource playImage;
        private DrawingImage backgroundGif;
        private bool backgroundAnimating;
        private int backgroundFrameBusy;

        public MainViewModel()
        {
            confManager = new ConfigurationManager();
            Accounts = new ObservableCollection<AccountItemViewModel>();

            SwitchLeagueCommand = new RelayCommand(_ => SetGameMode(GameMode.League));
            SwitchValorantCommand = new RelayCommand(_ => SetGameMode(GameMode.Valorant));
            ValidateApiCommand = new AsyncRelayCommand(_ => ValidateApiAsync());
            ToggleRevealCommand = new RelayCommand(_ =>
            {
                ApiKeyVisible = !ApiKeyVisible;
            });
            AddAccountCommand = new RelayCommand(_ => OpenEditor(null));
            RefreshAccountsCommand = new AsyncRelayCommand(_ => RefreshAllAsync(), _ => ApiValid);

            ThemeService.Apply(AppTheme.Current);
            LoadThemedImages();
            StartBackgroundAnimation();
            ApiKeyText = confManager.apiKey ?? string.Empty;
            ReloadAccounts();
            _ = ValidateApiAsync();
        }

        public ObservableCollection<AccountItemViewModel> Accounts { get; }

        public string ApiKeyText
        {
            get { return apiKeyText; }
            set { Set(ref apiKeyText, value); }
        }

        public bool ApiKeyVisible
        {
            get { return apiKeyVisible; }
            set { Set(ref apiKeyVisible, value); }
        }

        public bool ApiValid
        {
            get { return apiValid; }
            private set
            {
                if (Set(ref apiValid, value))
                    UpdateApiStatusImage();
            }
        }

        public BitmapSource ApiStatusImage
        {
            get { return apiStatusImage; }
            private set { Set(ref apiStatusImage, value); }
        }

        public BitmapSource LogoImage
        {
            get { return logoImage; }
            private set { Set(ref logoImage, value); }
        }

        public BitmapSource TitleImage
        {
            get { return titleImage; }
            private set { Set(ref titleImage, value); }
        }

        public BitmapSource BackgroundImage
        {
            get { return backgroundImage; }
            private set { Set(ref backgroundImage, value); }
        }

        public BitmapSource MinimizeImage
        {
            get { return minimizeImage; }
            private set { Set(ref minimizeImage, value); }
        }

        public BitmapSource CloseImage
        {
            get { return closeImage; }
            private set { Set(ref closeImage, value); }
        }

        public BitmapSource AddImage
        {
            get { return addImage; }
            private set { Set(ref addImage, value); }
        }

        public BitmapSource RefreshImage
        {
            get { return refreshImage; }
            private set { Set(ref refreshImage, value); }
        }

        public BitmapSource EditImage
        {
            get { return editImage; }
            private set { Set(ref editImage, value); }
        }

        public BitmapSource DeleteImage
        {
            get { return deleteImage; }
            private set { Set(ref deleteImage, value); }
        }

        public BitmapSource PlayImage
        {
            get { return playImage; }
            private set { Set(ref playImage, value); }
        }

        public bool IsLeague { get { return !AppTheme.IsValorant; } }
        public bool IsValorant { get { return AppTheme.IsValorant; } }

        public string TitleText
        {
            get { return AppTheme.IsValorant ? "Agents Of Smurfs" : "League Of Smurfs"; }
        }

        public ICommand SwitchLeagueCommand { get; }
        public ICommand SwitchValorantCommand { get; }
        public ICommand ValidateApiCommand { get; }
        public ICommand ToggleRevealCommand { get; }
        public ICommand AddAccountCommand { get; }
        public ICommand RefreshAccountsCommand { get; }
        public ICommand MinimizeCommand { get; } = new RelayCommand(_ =>
        {
            if (Application.Current?.MainWindow != null)
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
        });
        public ICommand CloseCommand { get; } = new RelayCommand(_ => Application.Current?.Shutdown());

        public void EditAccount(SmurfsConfiguration conf)
        {
            OpenEditor(conf);
        }

        public void DeleteAccount(SmurfsConfiguration conf)
        {
            confManager.Remove(conf);
            ReloadAccounts();
        }

        private void SetGameMode(GameMode mode)
        {
            if (AppTheme.Current == mode)
                return;

            PersistCurrentApiKey();
            ThemeService.Apply(mode);
            LoadThemedImages();
            LoadApiKeyForMode();
            Raise(nameof(IsLeague));
            Raise(nameof(IsValorant));
            Raise(nameof(TitleText));

            foreach (AccountItemViewModel item in Accounts)
                item.RefreshLabels();

            if (!IsCurrentModeApiAlreadyValidated())
                _ = ValidateApiAsync();
            else
                ApiValid = true;
        }

        private void PersistCurrentApiKey()
        {
            string candidate = (ApiKeyText ?? string.Empty).Trim();
            if (AppTheme.IsValorant)
            {
                confManager.valorantApiKey = candidate;
                if (string.IsNullOrWhiteSpace(candidate))
                    confManager.ClearValorantApi();
                else
                    confManager.SaveValorantApi();
            }
            else
            {
                confManager.apiKey = candidate;
                if (string.IsNullOrWhiteSpace(candidate))
                    confManager.ClearApi();
                else
                    confManager.SaveApi();
            }
        }

        private void LoadApiKeyForMode()
        {
            ApiKeyText = AppTheme.IsValorant
                ? (confManager.valorantApiKey ?? string.Empty)
                : (confManager.apiKey ?? string.Empty);
        }

        private bool IsCurrentModeApiAlreadyValidated()
        {
            string candidate = (ApiKeyText ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(candidate))
                return false;

            if (AppTheme.IsValorant)
            {
                return valorantApiValid
                    && string.Equals(candidate, validatedValorantKey, StringComparison.Ordinal);
            }

            return api != null
                && string.Equals(candidate, validatedRiotKey, StringComparison.Ordinal);
        }

        private async Task ValidateApiAsync()
        {
            string candidate = (ApiKeyText ?? string.Empty).Trim();

            if (AppTheme.IsValorant)
            {
                valorantApiValid = false;
                validatedValorantKey = null;
                if (string.IsNullOrWhiteSpace(candidate))
                {
                    confManager.ClearValorantApi();
                    ApiKeyText = string.Empty;
                    ApiValid = false;
                    return;
                }

                confManager.valorantApiKey = candidate;
                confManager.SaveValorantApi();
                ApiKeyText = candidate;
                valorantApiValid = await ValorantUtils.checkApiKeyAsync(candidate).ConfigureAwait(true);
                if (valorantApiValid)
                    validatedValorantKey = candidate;
                ApiValid = valorantApiValid;
                return;
            }

            api = null;
            validatedRiotKey = null;
            if (string.IsNullOrWhiteSpace(candidate))
            {
                confManager.ClearApi();
                ApiKeyText = string.Empty;
                ApiValid = false;
                return;
            }

            confManager.apiKey = candidate;
            confManager.SaveApi();
            ApiKeyText = candidate;
            api = RiotUtils.checkAPI(candidate);
            if (api != null)
                validatedRiotKey = candidate;
            ApiValid = api != null;
        }

        private async Task RefreshAllAsync()
        {
            if (!ApiValid)
                return;

            foreach (AccountItemViewModel item in Accounts.ToList())
            {
                await RefreshOneAsync(item).ConfigureAwait(true);
                await Task.Delay(200).ConfigureAwait(true);
            }
            ReloadAccounts();
        }

        private async Task RefreshOneAsync(AccountItemViewModel item)
        {
            SmurfsConfiguration conf = item.Config;
            if (string.IsNullOrWhiteSpace(conf.puuid))
                return;

            try
            {
                if (AppTheme.IsValorant)
                {
                    if (string.IsNullOrWhiteSpace(confManager.valorantApiKey))
                        return;
                    ValorantMmrInfo mmr = await ValorantUtils.GetMmrByPuuidAsync(
                        confManager.valorantApiKey, conf.region, conf.puuid).ConfigureAwait(true);
                    if (mmr == null)
                        return;
                    if (!string.IsNullOrWhiteSpace(mmr.Name) && !string.IsNullOrWhiteSpace(mmr.Tag))
                        conf.summonerName = mmr.Name + "#" + mmr.Tag;
                    if (!string.IsNullOrWhiteSpace(mmr.Puuid))
                        conf.puuid = mmr.Puuid;
                    conf.valRank = RankedUtils.getValorantRankByTierId(mmr.CurrentTierId);
                    conf.valRR = mmr.RR;
                    conf.valPeakRank = RankedUtils.getValorantRankByTierId(mmr.PeakTierId);
                }
                else
                {
                    if (api == null || string.IsNullOrWhiteSpace(confManager.apiKey))
                        return;
                    var account = await api.Account.GetAccountByPuuidAsync(
                        RiotUtils.getAccountRegion(conf.region), conf.puuid).ConfigureAwait(true);
                    string riotId = account.GameName + "#" + account.TagLine;
                    SummonerLevelInfo summoner = await RiotUtils.GetSummonerByPuuidAsync(
                        confManager.apiKey, conf.region, conf.puuid).ConfigureAwait(true);
                    if (summoner == null)
                        return;
                    conf.puuid = summoner.Puuid;
                    conf.summonerName = riotId;
                    conf.level = summoner.Level;
                    conf.encryptedId = string.Empty;
                    var entries = await RiotUtils.GetLeagueEntriesByPuuidAsync(
                        confManager.apiKey, conf.region, summoner.Puuid).ConfigureAwait(true);
                    conf.soloRank = RankEnum.UNRANKED;
                    conf.flexRank = RankEnum.UNRANKED;
                    foreach (var entry in entries)
                    {
                        if (entry.QueueType == "RANKED_FLEX_SR")
                        {
                            conf.flexRank = RankedUtils.getRankByEntry(entry);
                            conf.flexLP = entry.LeaguePoints;
                        }
                        if (entry.QueueType == "RANKED_SOLO_5x5")
                        {
                            conf.soloRank = RankedUtils.getRankByEntry(entry);
                            conf.soloLP = entry.LeaguePoints;
                        }
                    }
                }

                confManager.UpdateAccount(conf);
                item.ReplaceConfig(conf);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Refresh failed: " + ex.Message);
            }
        }

        private void OpenEditor(SmurfsConfiguration conf)
        {
            var editor = new AccountEditorWindow(confManager, api, conf, ReloadAccounts)
            {
                Owner = Application.Current?.MainWindow
            };
            editor.ShowDialog();
        }

        private void ReloadAccounts()
        {
            Accounts.Clear();
            foreach (SmurfsConfiguration conf in confManager.Get())
                Accounts.Add(new AccountItemViewModel(this, confManager, conf));
        }

        private void LoadThemedImages()
        {
            LogoImage = ImageService.FromThemed("main.logo", Properties.Resources.LolSmurflogo);
            TitleImage = ImageService.FromThemed("main.title", Properties.Resources.title);
            MinimizeImage = ImageService.FromThemed("main.minimize", Properties.Resources.minimize);
            CloseImage = ImageService.FromThemed("main.close", Properties.Resources.close);
            AddImage = ImageService.FromThemed("main.add", Properties.Resources.add);
            RefreshImage = ImageService.FromThemed("main.refresh", Properties.Resources.refresh);
            EditImage = ImageService.FromThemed("account.edit", Properties.Resources.settings);
            DeleteImage = ImageService.FromThemed("account.delete", Properties.Resources.delete);
            PlayImage = ImageService.FromThemed("account.launch", Properties.Resources.play);
            UpdateApiStatusImage();
        }

        public void StartBackgroundAnimation()
        {
            if (backgroundAnimating)
                return;

            backgroundGif = (DrawingImage)Properties.Resources.galaxybggif.Clone();
            backgroundAnimating = true;
            ImageAnimator.Animate(backgroundGif, OnBackgroundFrameChanged);
        }

        public void StopBackgroundAnimation()
        {
            if (!backgroundAnimating || backgroundGif == null)
                return;

            ImageAnimator.StopAnimate(backgroundGif, OnBackgroundFrameChanged);
            backgroundGif.Dispose();
            backgroundGif = null;
            backgroundAnimating = false;
        }

        private void OnBackgroundFrameChanged(object sender, EventArgs e)
        {
            if (backgroundGif == null)
                return;

            // Drop frames if UI is still converting the previous one.
            if (Interlocked.Exchange(ref backgroundFrameBusy, 1) == 1)
                return;

            try
            {
                ImageAnimator.UpdateFrames(backgroundGif);
                BitmapSource frame = ImageService.FromAnimatedFrame(backgroundGif, AppTheme.IsValorant);
                var dispatcher = Application.Current?.Dispatcher;
                if (dispatcher == null)
                {
                    Interlocked.Exchange(ref backgroundFrameBusy, 0);
                    return;
                }

                dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        BackgroundImage = frame;
                    }
                    finally
                    {
                        Interlocked.Exchange(ref backgroundFrameBusy, 0);
                    }
                }));
            }
            catch
            {
                Interlocked.Exchange(ref backgroundFrameBusy, 0);
            }
        }

        private void UpdateApiStatusImage()
        {
            var src = ApiValid ? Properties.Resources.greendot : Properties.Resources.reddot;
            ApiStatusImage = ImageService.FromDrawing(src);
        }
    }
}

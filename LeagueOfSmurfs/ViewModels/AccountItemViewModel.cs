using LeagueOfSmurfs.Configuration;
using LeagueOfSmurfs.Configurations;
using LeagueOfSmurfs.Mvvm;
using LeagueOfSmurfs.Services;
using LeagueOfSmurfs.Utils;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MediaColor = System.Windows.Media.Color;

namespace LeagueOfSmurfs.ViewModels
{
    public sealed class AccountItemViewModel : ViewModelBase
    {
        private readonly MainViewModel owner;
        private readonly ConfigurationManager confManager;
        private SmurfsConfiguration conf;
        private double progress;
        private bool launchRunning;

        public AccountItemViewModel(MainViewModel owner, ConfigurationManager confManager, SmurfsConfiguration conf)
        {
            this.owner = owner;
            this.confManager = confManager;
            this.conf = conf;
            LaunchCommand = new AsyncRelayCommand(_ => LaunchAsync(), _ => !launchRunning);
            EditCommand = new RelayCommand(_ => owner.EditAccount(this.conf));
            DeleteCommand = new RelayCommand(_ => owner.DeleteAccount(this.conf));
            RefreshLabels();
        }

        public SmurfsConfiguration Config { get { return conf; } }

        public string IdentityTitle { get; private set; }
        public string DisplayName { get; private set; }
        public string RegionText { get; private set; }
        public string LevelText { get; private set; }
        public string LeftTitle { get; private set; }
        public string LeftRank { get; private set; }
        public string LeftScore { get; private set; }
        public string RightTitle { get; private set; }
        public string RightRank { get; private set; }
        public string RightScore { get; private set; }
        public Brush LeftRankBrush { get; private set; }
        public Brush RightRankBrush { get; private set; }
        public Brush RankBarBrush { get; private set; }

        public double Progress
        {
            get { return progress; }
            set { Set(ref progress, value); }
        }

        public ICommand LaunchCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public void RefreshLabels()
        {
            DisplayName = conf.summonerName;
            RegionText = conf.region.ToString();
            LevelText = "Level: " + conf.level;

            if (AppTheme.IsValorant)
            {
                IdentityTitle = "Agent";
                LeftTitle = "Current";
                RightTitle = "Peak";
                LeftRank = "Rank: " + RankedUtils.valorantRankToString(conf.valRank);
                LeftScore = "RR: " + conf.valRR;
                RightRank = "Rank: " + RankedUtils.valorantRankToString(conf.valPeakRank);
                RightScore = string.Empty;
                LeftRankBrush = ToBrush(RankedUtils.getValorantRankPen(conf.valRank));
                RightRankBrush = ToBrush(RankedUtils.getValorantRankPen(conf.valPeakRank));
                RankBarBrush = BuildRankBar(
                    AppTheme.Hover,
                    RankedUtils.getValorantRankPen(conf.valRank),
                    RankedUtils.getValorantRankPen(conf.valPeakRank));
            }
            else
            {
                IdentityTitle = "Summoner";
                LeftTitle = "Flex";
                RightTitle = "Solo/Duo";
                LeftRank = "Ladder: " + RankedUtils.rankToString(conf.flexRank);
                LeftScore = "LP: " + conf.flexLP;
                RightRank = "Ladder: " + RankedUtils.rankToString(conf.soloRank);
                RightScore = "LP: " + conf.soloLP;
                LeftRankBrush = ToBrush(RankedUtils.getRankPen(conf.flexRank));
                RightRankBrush = ToBrush(RankedUtils.getRankPen(conf.soloRank));
                RankBarBrush = BuildRankBar(
                    AppTheme.Hover,
                    RankedUtils.getRankPen(conf.flexRank),
                    RankedUtils.getRankPen(conf.soloRank));
            }

            Raise(nameof(IdentityTitle));
            Raise(nameof(DisplayName));
            Raise(nameof(RegionText));
            Raise(nameof(LevelText));
            Raise(nameof(LeftTitle));
            Raise(nameof(LeftRank));
            Raise(nameof(LeftScore));
            Raise(nameof(RightTitle));
            Raise(nameof(RightRank));
            Raise(nameof(RightScore));
            Raise(nameof(LeftRankBrush));
            Raise(nameof(RightRankBrush));
            Raise(nameof(RankBarBrush));
        }

        public void ReplaceConfig(SmurfsConfiguration updated)
        {
            conf = updated;
            RefreshLabels();
        }

        private async Task LaunchAsync()
        {
            if (launchRunning)
                return;
            launchRunning = true;
            try
            {
                var progress = new Progress<int>(v => Progress = v);
                await AccountLaunchService.LaunchAsync(
                    conf,
                    progress,
                    () =>
                    {
                        if (Application.Current?.MainWindow != null)
                            Application.Current.MainWindow.WindowState = WindowState.Minimized;
                    }).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                DebugWrite(ex.Message);
                Progress = 0;
            }
            finally
            {
                launchRunning = false;
            }
        }

        private static Brush ToBrush(System.Drawing.Color color)
        {
            var media = MediaColor.FromArgb(color.A, color.R, color.G, color.B);
            var brush = new SolidColorBrush(media);
            brush.Freeze();
            return brush;
        }

        private static Brush BuildRankBar(
            System.Drawing.Color start,
            System.Drawing.Color mid,
            System.Drawing.Color end)
        {
            var brush = new LinearGradientBrush
            {
                StartPoint = new System.Windows.Point(0, 0.5),
                EndPoint = new System.Windows.Point(1, 0.5)
            };
            brush.GradientStops.Add(new GradientStop(ToMedia(start), 0));
            brush.GradientStops.Add(new GradientStop(ToMedia(mid), 0.35));
            brush.GradientStops.Add(new GradientStop(ToMedia(end), 1));
            brush.Freeze();
            return brush;
        }

        private static MediaColor ToMedia(System.Drawing.Color color)
        {
            return MediaColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        private static void DebugWrite(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}

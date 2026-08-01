using LeagueOfSmurfs.Configuration;
using LeagueOfSmurfs.Configurations;
using LeagueOfSmurfs.Utils;
using RiotSharp;
using RiotSharp.Endpoints.AccountEndpoint;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueOfSmurfs
{
    public partial class AccountDisplay : Form
    {
        // Don't care
        private Form parent;

        // Rito api
        private RiotApi api;

        // Config
        private ConfigurationManager confManager;
        private SmurfsConfiguration conf;

        // Auto minimized
        private DateTime lastCheck;
        private bool inGame;
        private bool launchRunning;

        public AccountDisplay(Form parent, ConfigurationManager confManager, SmurfsConfiguration conf, RiotApi api)
        {
            InitializeComponent();
            this.Owner = parent;
            this.parent = parent;
            this.confManager = confManager;
            this.conf = conf;
            this.api = api;
            this.launchRunning = false;

            // Rounded window
            CustomForms.Utils.SetRoundedRegion(this, 50, 50);
            this.TopLevel = false;
            this.Show();

            // Instant update placeholder
            this.Update(false);

            // Remove progress bar
            operationProcessBar.SetValue(0);
            Debug.WriteLine("Created display for account " + conf.summonerName);

            // Auto minimize
            this.lastCheck = DateTime.Now;
            this.inGame = false;

            // Remove button background (flemme de mettre les flat)
            this.launchButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 40);
            this.editButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 40);
            this.deleteButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 40);
        }

        /*
         * ====================================================================================
         *                              LOCATION ON SCREEN
         * ====================================================================================
        */

        public void Relocate(int n)
        {
            // Set location
            Point newLoc = new Point();

            // Left/Right column
            newLoc.X += 10;
            if (n % 2 != 0)
                newLoc.X += 470;

            // Height
            newLoc.Y += 20;
            newLoc.Y += (this.Size.Height + 20) * (n / 2);

            // Set new location
            this.Location = newLoc;
        }

        /*
         * ====================================================================================
         *                              UPDATE INFORMATIONS
         * ====================================================================================
        */

        public async void Update(bool request)
        {
            operationProcessBar.SetValue(0);

            // Riot requests only with a validated API session + stored key
            if (request && this.api != null && !string.IsNullOrWhiteSpace(this.confManager.apiKey)
                && !string.IsNullOrWhiteSpace(this.conf.puuid))
            {
                try
                {
                    // Never Remove() before refresh — a failed API call used to delete the yaml permanently
                    Task<Account> account = this.api.Account.GetAccountByPuuidAsync(RiotUtils.getAccountRegion(this.conf.region), this.conf.puuid);
                    await account;
                    operationProcessBar.SetValue(10);
                    string riotId = account.Result.GameName + "#" + account.Result.TagLine;
                    Debug.WriteLine("Success query account: " + riotId);

                    SummonerLevelInfo summoner = await RiotUtils.GetSummonerByPuuidAsync(this.confManager.apiKey, this.conf.region, this.conf.puuid);
                    if (summoner == null || string.IsNullOrWhiteSpace(summoner.Puuid))
                        throw new Exception("Empty summoner response");

                    operationProcessBar.SetValue(40);
                    Debug.WriteLine("Success query name: " + riotId);

                    this.conf.puuid = summoner.Puuid;
                    this.conf.encryptedId = string.Empty;
                    this.conf.summonerName = riotId;
                    this.conf.level = summoner.Level;
                    this.summonerError.Text = "";

                    List<LeagueEntryInfo> entries = await RiotUtils.GetLeagueEntriesByPuuidAsync(this.confManager.apiKey, this.conf.region, summoner.Puuid);
                    operationProcessBar.SetValue(80);

                    this.conf.soloRank = RankEnum.UNRANKED;
                    this.conf.flexRank = RankEnum.UNRANKED;
                    foreach (LeagueEntryInfo entry in entries)
                    {
                        if (entry.QueueType.Equals("RANKED_FLEX_SR"))
                        {
                            this.conf.flexRank = RankedUtils.getRankByEntry(entry);
                            this.conf.flexLP = entry.LeaguePoints;
                        }
                        if (entry.QueueType.Equals("RANKED_SOLO_5x5"))
                        {
                            this.conf.soloRank = RankedUtils.getRankByEntry(entry);
                            this.conf.soloLP = entry.LeaguePoints;
                        }
                    }

                    if (!this.confManager.UpdateAccount(this.conf))
                        Debug.WriteLine("Failed to persist refreshed account");

                    operationProcessBar.SetValue(100);
                    await Task.Delay(1000);
                    operationProcessBar.SetValue(0);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Fail re-query name " + ex.Message);
                    this.summonerError.Text = "Invalid Name";
                    // Keep existing yaml / in-memory data intact
                }
            }

            // Always refresh UI from cached config (works without API key)
            this.summonerNameLabel.Text = this.conf.summonerName;
            this.regionLabel.Text = this.conf.region.ToString();
            this.levelLabel.Text = "Level: " + this.conf.level;
            this.flexLadderLabel.Text = "Ladder: " + RankedUtils.rankToString(this.conf.flexRank);
            this.flexLpLabel.Text = "LP: " + this.conf.flexLP.ToString();
            this.soloLadderLabel.Text = "Ladder: " + RankedUtils.rankToString(this.conf.soloRank);
            this.soloLpLabel.Text = "LP: " + this.conf.soloLP.ToString();
        }

        /*
         * ====================================================================================
         *                              FUNCTIONNALITIES
         * ====================================================================================
        */

        private void deleteButton_Click(object sender, EventArgs e)
        {
            this.confManager.Remove(this.conf);
            ((MainMenu)this.parent).updateDisplay();
        }
        private void editButton_Click(object sender, EventArgs e)
        {
            new AccountAdder(this.conf, this.parent, this.confManager, this.api);
        }

        private async void launchButton_Click(object sender, EventArgs e)
        {
            if (this.launchRunning)
                return;

            this.launchRunning = true;
            DateTime deadline = DateTime.UtcNow.AddSeconds(30);
            bool completed = false;
            CancellationTokenSource focusCts = null;
            Task focusLoop = null;

            try
            {
                operationProcessBar.SetValue(0);

                // Close client if open
                RiotUtils.closeRiot();

                // Launch client and wait for ui
                RiotUtils.launchClient();
                Debug.WriteLine("Launched client!");
                operationProcessBar.SetValue(20);

                Process riotProcess = null;
                while (riotProcess == null)
                {
                    ThrowIfLaunchTimedOut(deadline);
                    riotProcess = WindowUtils.FindRiotClientProcess();
                    if (riotProcess == null)
                        await Task.Delay(100);
                }

                try { riotProcess.WaitForInputIdle(5000); } catch { /* ignore */ }
                Debug.WriteLine("Found client process: " + riotProcess.ProcessName + " pid=" + riotProcess.Id
                    + " title='" + riotProcess.MainWindowTitle + "' hwnd=" + riotProcess.MainWindowHandle);
                operationProcessBar.SetValue(40);

                // Focus watchdog: check + refocus once per second for up to 30s
                focusCts = new CancellationTokenSource();
                focusLoop = WindowUtils.KeepFocusedLoopAsync(deadline, focusCts.Token);

                // Wait for window + input readiness (probe clipboard roundtrip)
                await Task.Delay(1500);
                Clipboard.Clear();
                while (!Clipboard.GetText().Equals("longtestforinput"))
                {
                    ThrowIfLaunchTimedOut(deadline);
                    if (!await WindowUtils.SendKeysWhenFocusedAsync("longtestforinput", deadline))
                        throw new TimeoutException("Could not focus Riot client for input probe");
                    await Task.Delay(100);
                    if (!await WindowUtils.SendKeysWhenFocusedAsync("^{a}^{x}", deadline))
                        throw new TimeoutException("Could not focus Riot client for input probe clear");
                }

                operationProcessBar.SetValue(50);

                // Username
                Clipboard.SetText(this.conf.username ?? string.Empty);
                if (!await WindowUtils.SendKeysWhenFocusedAsync("^{v}", deadline))
                    throw new TimeoutException("Could not paste username");
                operationProcessBar.SetValue(65);

                if (!await WindowUtils.SendKeysWhenFocusedAsync("{TAB}", deadline))
                    throw new TimeoutException("Could not tab to password");
                await Task.Delay(200);

                // Password
                Clipboard.SetText(this.conf.password ?? string.Empty);
                if (!await WindowUtils.SendKeysWhenFocusedAsync("^{v}", deadline))
                    throw new TimeoutException("Could not paste password");
                await Task.Delay(50);
                Clipboard.Clear();
                operationProcessBar.SetValue(80);
                Debug.WriteLine("Entered logins!");

                // Tab to play / sign-in button
                for (int i = 0; i < 7; i++)
                {
                    ThrowIfLaunchTimedOut(deadline);
                    if (!await WindowUtils.SendKeysWhenFocusedAsync("{TAB}", deadline))
                        throw new TimeoutException("Could not tab through login form");
                    await Task.Delay(50);
                }
                if (!await WindowUtils.SendKeysWhenFocusedAsync("{ENTER}", deadline))
                    throw new TimeoutException("Could not confirm login");

                ThrowIfLaunchTimedOut(deadline);
                int waitMs = Math.Min(3000, Math.Max(0, (int)(deadline - DateTime.UtcNow).TotalMilliseconds));
                if (waitMs > 0)
                    await Task.Delay(waitMs);
                RiotUtils.launchLeague();
                operationProcessBar.SetValue(100);
                completed = true;

                // Stop focus loop once login sequence reached 100%
                focusCts.Cancel();

                // Wait for league to launch (bounded)
                DateTime leagueDeadline = DateTime.UtcNow.AddSeconds(60);
                Process[] league = null;
                while (league == null || league.Length == 0)
                {
                    if (DateTime.UtcNow >= leagueDeadline)
                    {
                        Debug.WriteLine("League client wait timed out");
                        break;
                    }
                    league = Process.GetProcessesByName("LeagueClient");
                    await Task.Delay(100);
                }
                if (league != null && league.Length > 0)
                {
                    this.parent.WindowState = FormWindowState.Minimized;
                    this.inGame = true;
                }

                await Task.Delay(1000);
                operationProcessBar.SetValue(0);
            }
            catch (TimeoutException tex)
            {
                Debug.WriteLine("Launch aborted (timeout/focus): " + tex.Message);
                operationProcessBar.SetValue(0);
                try { Clipboard.Clear(); } catch { /* ignore */ }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("error" + ex.Message);
                operationProcessBar.SetValue(0);
                try { Clipboard.Clear(); } catch { /* ignore */ }
            }
            finally
            {
                if (focusCts != null)
                {
                    focusCts.Cancel();
                    if (focusLoop != null)
                    {
                        try { await focusLoop; } catch { /* ignore */ }
                    }
                    focusCts.Dispose();
                }
                if (!completed)
                    operationProcessBar.SetValue(0);
                this.launchRunning = false;
            }
        }

        private static void ThrowIfLaunchTimedOut(DateTime deadlineUtc)
        {
            if (DateTime.UtcNow >= deadlineUtc)
                throw new TimeoutException("Login sequence exceeded 30 seconds");
        }

        /*
         * ====================================================================================
         *                              PANEL CALL
         * ====================================================================================
        */

        private void AccountDisplay_Paint(object sender, PaintEventArgs e)
        {
            Pen pen;
            Color left, right;
            Point pointTop, pointBot;


            // Solo
            right = RankedUtils.getRankPen(this.conf.soloRank);
            left = right;

            pointTop = new Point(224, 70);
            pointBot = new Point(440, 70);

            pen = new Pen(new LinearGradientBrush(pointTop, pointBot, left, right), 3);
            e.Graphics.DrawLine(pen, pointTop, pointBot);

            // Flex to solo
            left = RankedUtils.getRankPen(this.conf.flexRank);

            pointTop = new Point(74, 70);
            pointBot = new Point(255, 70);

            pen = new Pen(new LinearGradientBrush(pointTop, pointBot, left, right), 3);
            e.Graphics.DrawLine(pen, pointTop, pointBot);

            // Account to flex
            right = left;
            left = Color.FromArgb(40, 40, 40);

            pointTop = new Point(10, 70);
            pointBot = new Point(75, 70);

            pen = new Pen(new LinearGradientBrush(pointTop, pointBot, left, right), 3);
            e.Graphics.DrawLine(pen, pointTop, pointBot);
        }

        private void progressTimer_Tick(object sender, EventArgs e)
        {
            this.operationProcessBar.update();
            if (this.inGame && (DateTime.Now.Subtract(this.lastCheck).TotalSeconds > 5))
            {
                this.lastCheck = DateTime.Now;
                Process[] league = Process.GetProcessesByName("LeagueClient");
                if (league == null || league.Length == 0)
                {
                    this.parent.WindowState = FormWindowState.Normal;
                    this.inGame = false;
                }
            }
        }

        /*
         * ====================================================================================
         *                              FUCK BUFF
         * ====================================================================================
        */

        private void buffPopupCloser_Tick(object sender, EventArgs e)
        {
            this.CloseBuffPopUp();
        }

        private async void CloseBuffPopUp()
        {
            await Task.Delay(1);
            /*try
            {
                DateTime firstWindow;
                Process[] processes;

                processes = Process.GetProcessesByName("BUFF");
                if (processes.Length > 1)
                {
                    firstWindow = processes[0].StartTime;
                    Debug.WriteLine("Check process (" + processes.Length + "):");
                    // Get first window opened
                    foreach (Process process in processes)
                    {
                        Debug.WriteLine("Process: " + process.ProcessName + ", " + process.StartTime + " " + process.PrivateMemorySize64 + " " + process.StartTime.CompareTo(firstWindow));
                        if (process.StartTime.CompareTo(firstWindow) < 0)
                        {
                            firstWindow = process.StartTime;
                        }
                    }
                    // Close all other after this time
                    foreach (Process process in processes)
                    {
                        if (process.StartTime.CompareTo(firstWindow) > 0)
                        {
                            //WindowUtils.ShowWindow(process.MainWindowHandle, 6);
                            //WindowUtils.SetWindowPos(process.Handle, 0, 0, 1, 1, 0, 0);
                            Debug.WriteLine("Process: " + process.ProcessName + " " + process.Id + " found");
                            if (WindowUtils.EnumerateProcessWindowHandles(process).Any())
                            {
                                Debug.WriteLine("Handle " + WindowUtils.EnumerateProcessWindowHandles(process).First());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("error" + ex.Message);
            }*/
        }
    }
}

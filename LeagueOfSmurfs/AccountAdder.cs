using LeagueOfSmurfs.Configuration;
using LeagueOfSmurfs.Configurations;
using LeagueOfSmurfs.CustomForms;
using LeagueOfSmurfs.Utils;
using RiotSharp;
using RiotSharp.Endpoints.AccountEndpoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LeagueOfSmurfs
{
    public partial class AccountAdder : Form
    {
        // Don't care
        private Form parent;
        private TitleBar titleBar;

        // Configuration 
        private ConfigurationManager confManager;
        private SmurfsConfiguration newAccount;
        private string originalPuuid;
        private bool suppressCredentialReset;

        // Riot session
        private RiotApi api;

        public AccountAdder(Form parent, ConfigurationManager confManager, RiotApi api)
        {
            InitializeComponent();
            this.parent = parent;
            this.confManager = confManager;
            this.newAccount = new SmurfsConfiguration();
            this.originalPuuid = null;
            this.api = api;

            // Rounded window
            CustomForms.Utils.SetRoundedRegion(this, 50, 50);

            // Title bar
            this.titleBar = new TitleBar(this);
            this.titleBar.OnPaint(new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));

            // Set in the middle and show
            Point point = parent.Location;
            point.X += 275;
            point.Y += 150;
            this.Location = point;
            this.TopMost = true;
            this.Show();

            // Add regions
            this.regionBox.Region = new Region(new Rectangle(3, 3, this.regionBox.Width - 25, this.regionBox.Height - 7));
            foreach (RegionEnum region in (RegionEnum[])Enum.GetValues(typeof(RegionEnum)))
            {
                this.regionBox.Items.Add(region.ToString());
            }
            this.regionBox.SelectedItem = this.regionBox.Items[2];

            // Informations separator
            this.accountCheckPanel.Paint += Account_Paint;
        }
        public AccountAdder(SmurfsConfiguration conf, Form parent, ConfigurationManager confManager, RiotApi api) : this(parent, confManager, api)
        {
            this.originalPuuid = conf.puuid;
            this.newAccount.puuid = conf.puuid;
            this.newAccount.encryptedId = conf.encryptedId;
            this.newAccount.summonerName = conf.summonerName;
            this.newAccount.level = conf.level;
            this.newAccount.soloRank = conf.soloRank;
            this.newAccount.soloLP = conf.soloLP;
            this.newAccount.flexRank = conf.flexRank;
            this.newAccount.flexLP = conf.flexLP;
            this.newAccount.region = conf.region;
            this.newAccount.username = conf.username;
            this.newAccount.password = conf.password;

            this.suppressCredentialReset = true;
            string gameName = conf.summonerName ?? string.Empty;
            string tag = string.Empty;
            int hashIndex = gameName.IndexOf('#');
            if (hashIndex >= 0)
            {
                tag = gameName.Substring(hashIndex + 1);
                gameName = gameName.Substring(0, hashIndex);
            }
            this.summonerNameBox.Text = gameName;
            this.tagBox.Text = tag;
            this.usernameBox.Text = conf.username;
            this.passwordBox.Text = conf.password;
            this.suppressCredentialReset = false;

            for (int i = 0; i < this.regionBox.Items.Count; i++)
            {
                if (this.regionBox.Items[i].Equals(conf.region.ToString()))
                {
                    this.regionBox.SelectedItem = this.regionBox.Items[i];
                    break;
                }
            }

            // Prefill UI from cache; optional re-check if API available
            this.summonerNameLabel.Text = conf.summonerName;
            this.levelLabel.Text = "Level: " + conf.level;
            this.soloLadderLabel.Text = "Ladder: " + RankedUtils.rankToString(conf.soloRank);
            this.soloLpLabel.Text = "LP: " + conf.soloLP.ToString();
            this.flexLadderLabel.Text = "Ladder: " + RankedUtils.rankToString(conf.flexRank);
            this.flexLpLabel.Text = "LP: " + conf.flexLP.ToString();

            if (this.api != null && !string.IsNullOrWhiteSpace(this.confManager.apiKey))
                checkAccountButton_Click(null, null);
        }

        /*
         * ====================================================================================
         *                              RIOT ID CHECK
         * ====================================================================================
        */

        private void summonerNameBox_TextChanged(object sender, EventArgs e)
        {
            this.summonerError.Visible = false;

            if (this.summonerNameBox.Text.Length < 3)
            {
                this.summonerError.Text = "Name is too short";
                this.summonerError.Visible = true;
            }

            if (this.suppressCredentialReset)
                return;

            // Name changed: require a fresh Riot check before save
            this.newAccount.puuid = string.Empty;
            this.newAccount.encryptedId = string.Empty;
            this.newAccount.summonerName = string.Empty;

            this.summonerNameLabel.Text = "";
            this.levelLabel.Text = "Level: 0";
            this.flexLadderLabel.Text = "Ladder: Iron 4";
            this.flexLpLabel.Text = "LP: 0";
            this.soloLadderLabel.Text = "Ladder: Iron 4";
            this.soloLpLabel.Text = "LP: 0";
        }

        private void tagBox_TextChanged(object sender, EventArgs e)
        {
            if (this.suppressCredentialReset)
                return;

            this.newAccount.puuid = string.Empty;
            this.newAccount.encryptedId = string.Empty;
            this.newAccount.summonerName = string.Empty;
        }

        private void regionBox_SelectedValueChanged(object sender, EventArgs e)
        {
            this.newAccount.region = RiotUtils.getRegionByName(this.regionBox.Text);
        }

        private async void checkAccountButton_Click(object sender, EventArgs e)
        {
            if (this.api == null || string.IsNullOrWhiteSpace(this.confManager.apiKey))
            {
                this.summonerError.Text = "No API key";
                this.summonerError.Visible = true;
                return;
            }

            if (this.summonerNameBox.Text != null && this.summonerNameBox.Text.Length >= 3)
            {
                if (string.IsNullOrWhiteSpace(this.tagBox.Text))
                {
                    this.summonerError.Text = "Tag required";
                    this.summonerError.Visible = true;
                    return;
                }

                this.newAccount.summonerName = summonerNameBox.Text;
                try
                {
                    Task<Account> account = this.api.Account.GetAccountByRiotIdAsync(RiotUtils.getAccountRegion(this.newAccount.region), this.summonerNameBox.Text.Trim(), this.tagBox.Text.Trim());
                    await account;
                    if (account.Result == null || string.IsNullOrWhiteSpace(account.Result.Puuid))
                        throw new Exception("Empty account response");

                    string riotId = account.Result.GameName + "#" + account.Result.TagLine;
                    Debug.WriteLine("Success query account: " + riotId);

                    SummonerLevelInfo summoner = await RiotUtils.GetSummonerByPuuidAsync(this.confManager.apiKey, this.newAccount.region, account.Result.Puuid);
                    if (summoner == null || string.IsNullOrWhiteSpace(summoner.Puuid))
                        throw new Exception("Empty summoner response");

                    Debug.WriteLine("Success query summoner: " + riotId + ", level: " + summoner.Level);

                    // Set config info (summoner IDs removed from API — PUUID is the identifier)
                    this.newAccount.puuid = summoner.Puuid;
                    this.newAccount.encryptedId = string.Empty;
                    this.newAccount.summonerName = riotId;
                    this.newAccount.level = summoner.Level;

                    // Set basic information
                    this.summonerError.Text = "";
                    this.summonerError.Visible = false;
                    this.summonerNameLabel.Text = riotId;
                    this.levelLabel.Text = "Level: " + summoner.Level;

                    // Set ranked information (league-v4 by-puuid — by-summoner is obsolete)
                    List<LeagueEntryInfo> entries = await RiotUtils.GetLeagueEntriesByPuuidAsync(this.confManager.apiKey, this.newAccount.region, summoner.Puuid);

                    // Set config info
                    this.newAccount.soloRank = RankEnum.UNRANKED;
                    this.newAccount.flexRank = RankEnum.UNRANKED;
                    foreach (LeagueEntryInfo entry in entries)
                    {
                        if (entry.QueueType.Equals("RANKED_FLEX_SR"))
                        {
                            this.newAccount.flexRank = RankedUtils.getRankByEntry(entry);
                            this.newAccount.flexLP = entry.LeaguePoints;
                        }
                        if (entry.QueueType.Equals("RANKED_SOLO_5x5"))
                        {
                            this.newAccount.soloRank = RankedUtils.getRankByEntry(entry);
                            this.newAccount.soloLP = entry.LeaguePoints;
                        }
                    }

                    // Set basic information
                    this.soloLadderLabel.Text = "Ladder: " + RankedUtils.rankToString(this.newAccount.soloRank);
                    this.soloLpLabel.Text = "LP: " + this.newAccount.soloLP.ToString();
                    this.flexLadderLabel.Text = "Ladder: " + RankedUtils.rankToString(this.newAccount.flexRank);
                    this.flexLpLabel.Text = "LP: " + this.newAccount.flexLP.ToString();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Fail query name " + ex.Message);
                    this.summonerError.Text = "Invalid Name";
                    this.summonerError.Visible = true;
                }
            }
        }

        /*
         * ====================================================================================
         *                              ACCOUNT ADD
         * ====================================================================================
        */

        private void passwordBox_TextChanged(object sender, EventArgs e)
        {
            this.passwordBox.ForeColor = Color.White;
            if (this.passwordBox.Text.Length < 3)
                this.passwordBox.ForeColor = Color.Red;
            else
                this.newAccount.password = this.passwordBox.Text;
        }

        private void usernameBox_TextChanged(object sender, EventArgs e)
        {
            this.usernameBox.ForeColor = Color.White;
            if (this.usernameBox.Text.Length < 3)
                this.usernameBox.ForeColor = Color.Red;
            else
                this.newAccount.username = this.usernameBox.Text;
        }

        private void confirmAdd_Click(object sender, EventArgs e)
        {
            this.newAccount.username = this.usernameBox.Text;
            this.newAccount.password = this.passwordBox.Text;
            this.newAccount.region = RiotUtils.getRegionByName(this.regionBox.Text);

            string gameName = (this.summonerNameBox.Text ?? string.Empty).Trim();
            string tag = (this.tagBox.Text ?? string.Empty).Trim();

            if (gameName.Length < 3)
            {
                this.summonerError.Text = "Name is too short";
                this.summonerError.Visible = true;
                return;
            }
            if (this.passwordBox.Text == null || this.passwordBox.Text.Length < 3)
            {
                this.passwordBox.ForeColor = Color.Red;
                return;
            }
            if (this.usernameBox.Text == null || this.usernameBox.Text.Length < 3)
            {
                this.usernameBox.ForeColor = Color.Red;
                return;
            }

            // Manual save allowed without API: keep existing id, or create a local one
            if (string.IsNullOrWhiteSpace(this.newAccount.puuid))
            {
                if (!string.IsNullOrWhiteSpace(this.originalPuuid))
                    this.newAccount.puuid = this.originalPuuid;
                else
                    this.newAccount.puuid = "local-" + Guid.NewGuid().ToString("N");
            }

            this.newAccount.summonerName = string.IsNullOrEmpty(tag) ? gameName : (gameName + "#" + tag);

            if (!this.confManager.IsValidAccount(this.newAccount))
            {
                this.summonerError.Text = "Incomplete account";
                this.summonerError.Visible = true;
                return;
            }

            // If Riot ID / puuid changed while editing, drop the old yaml
            if (!string.IsNullOrWhiteSpace(this.originalPuuid)
                && this.originalPuuid != this.newAccount.puuid)
            {
                SmurfsConfiguration old = this.confManager.Get()
                    .FirstOrDefault(c => c.puuid == this.originalPuuid);
                if (old != null)
                    this.confManager.Remove(old);
            }

            if (!this.confManager.Add(this.newAccount))
            {
                this.summonerError.Text = "Save failed";
                this.summonerError.Visible = true;
                return;
            }

            ((MainMenu)this.parent).updateDisplay();
            this.Close();
        }

        /*
         * ====================================================================================
         *                              TITLE BAR BUTTONS
         * ====================================================================================
        */

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }


        /*
         * ====================================================================================
         *                              TITLE BAR CALLS
         * ====================================================================================
        */

        private void Form_Paint(object sender, PaintEventArgs e)
        {
            this.titleBar.OnPaint(e);
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            this.titleBar.MouseDown(sender, e);
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            this.titleBar.MouseUp(sender, e);
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            this.titleBar.MouseMove(sender, e);
        }

        /*
         * ====================================================================================
         *                              ACCOUNT PANEL CALL
         * ====================================================================================
        */

        private void Account_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(30, 30, 30), 3), new PointF(120F, 0F), new PointF(120F, 550F));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(30, 30, 30), 3), new PointF(255F, 0F), new PointF(255F, 550F));
        }

        private void passwordRepeat_Click(object sender, EventArgs e)
        {

        }
    }
}

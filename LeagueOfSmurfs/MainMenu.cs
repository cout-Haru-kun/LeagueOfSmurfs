using LeagueOfSmurfs.Configuration;
using LeagueOfSmurfs.Configurations;
using LeagueOfSmurfs.CustomForms;
using LeagueOfSmurfs.Properties;
using LeagueOfSmurfs.Utils;
using RiotSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueOfSmurfs
{
    public partial class MainMenu : Form
    {
        // Don't care
        private TitleBar titleBar;

        // Manager
        private ConfigurationManager confManager;
        private List<AccountDisplay> accounts;

        // Riot / Valorant sessions
        private RiotApi api;
        private bool valorantApiValid;
        private bool apiKeyInitialized;
        private bool apiKeyVisible;
        private string validatedRiotKey;
        private string validatedValorantKey;


        public MainMenu()
        {
            InitializeComponent();

            // Rounded window
            CustomForms.Utils.SetRoundedRegion(this, 50, 50);

            // Configuration Manager
            this.confManager = new ConfigurationManager();
            this.accounts = new List<AccountDisplay>();
            this.apiKeyInitialized = false;
            this.valorantApiValid = false;
            this.apiKeyVisible = false;
            this.validatedRiotKey = null;
            this.validatedValorantKey = null;
            this.ApplyApiKeyVisibility();

            // Title bar
            this.titleBar = new TitleBar(this);
            this.titleBar.OnPaint(new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));

            // Update
            this.LoadApiKeyIntoBox();
            this.checkAPI();
            this.updateDisplay();
            this.ApplyTheme();
        }



        /*
         * ====================================================================================
         *                              GAME MODE TOGGLE
         * ====================================================================================
        */

        private void gameLeagueButton_Click(object sender, EventArgs e)
        {
            SetGameMode(GameMode.League);
        }

        private void gameValorantButton_Click(object sender, EventArgs e)
        {
            SetGameMode(GameMode.Valorant);
        }

        private void SetGameMode(GameMode mode)
        {
            if (AppTheme.Current == mode)
                return;

            PersistCurrentApiKeyFromBox();
            AppTheme.SetMode(mode);
            LoadApiKeyIntoBox();
            ApplyTheme();

            // Don't re-hit Riot/HenrikDev on every L/V toggle if that mode's key was already validated
            if (!IsCurrentModeApiAlreadyValidated())
                checkAPI();
            else
                apiColor();

            foreach (AccountDisplay display in accounts)
                display.ApplyModeLabels();
            Invalidate(true);
        }

        private bool IsCurrentModeApiAlreadyValidated()
        {
            string candidate = (this.apiKeyBox.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(candidate))
                return false;

            if (AppTheme.IsValorant)
            {
                return this.valorantApiValid
                    && string.Equals(candidate, this.validatedValorantKey, StringComparison.Ordinal);
            }

            return this.api != null
                && string.Equals(candidate, this.validatedRiotKey, StringComparison.Ordinal);
        }

        public void ApplyTheme()
        {
            this.BackColor = AppTheme.Window;
            this.pictureBox2.BackColor = AppTheme.TitleBar;
            this.icon.BackColor = AppTheme.TitleBar;
            this.MinimizeButton.BackColor = AppTheme.TitleBar;
            this.CloseButton.BackColor = AppTheme.TitleBar;

            this.ButtonsPanel.BackColor = AppTheme.Panel;
            this.AccountList.BackColor = AppTheme.PanelAlt;
            this.innerAccountList.BackColor = AppTheme.PanelAlt;

            this.apiKeyBox.BackColor = AppTheme.Input;
            this.apiRefresh.BackColor = AppTheme.Panel;
            this.apiKeyReveal.BackColor = AppTheme.Panel;
            this.apiKeyReveal.ForeColor = AppTheme.Accent;
            this.AddAccount.BackColor = AppTheme.Panel;
            this.refreshAccount.BackColor = AppTheme.Panel;
            this.apiStatus.BackColor = AppTheme.Window;

            // L / V keep brand colors regardless of active mode
            this.gameLeagueButton.BackColor = AppTheme.LeagueAccent;
            this.gameLeagueButton.ForeColor = Color.Black;
            this.gameValorantButton.BackColor = AppTheme.ValorantAccent;
            this.gameValorantButton.ForeColor = Color.Black;

            ApplyThemedImages();

            foreach (AccountDisplay display in accounts)
                display.ApplyTheme();

            this.Invalidate();
        }

        private void ApplyThemedImages()
        {
            this.pictureBox1.Image = ThemeImages.Resolve("main.bg", this.pictureBox1.Image);
            this.pictureBox2.Image = ThemeImages.Resolve("main.title", Resources.title);
            this.icon.BackgroundImage = ThemeImages.Resolve("main.logo", Resources.LolSmurflogo);
            this.apiRefresh.BackgroundImage = ThemeImages.Resolve("main.apiRefresh", Resources.refresh);
            this.AddAccount.BackgroundImage = ThemeImages.Resolve("main.add", Resources.add);
            this.refreshAccount.BackgroundImage = ThemeImages.Resolve("main.refresh", Resources.refresh);
            this.MinimizeButton.BackgroundImage = ThemeImages.Resolve("main.minimize", Resources.minimize);
            this.CloseButton.BackgroundImage = ThemeImages.Resolve("main.close", Resources.close);
        }

        /*
         * ====================================================================================
         *                              API CHECK
         * ====================================================================================
        */

        public bool HasValidApi()
        {
            if (AppTheme.IsValorant)
                return this.valorantApiValid && !string.IsNullOrWhiteSpace(this.confManager.valorantApiKey);
            return this.api != null && !string.IsNullOrWhiteSpace(this.confManager.apiKey);
        }

        public ConfigurationManager GetConfManager()
        {
            return this.confManager;
        }

        private void PersistCurrentApiKeyFromBox()
        {
            string candidate = (this.apiKeyBox.Text ?? string.Empty).Trim();
            if (AppTheme.IsValorant)
            {
                this.confManager.valorantApiKey = candidate;
                if (string.IsNullOrWhiteSpace(candidate))
                    this.confManager.ClearValorantApi();
                else
                    this.confManager.SaveValorantApi();
            }
            else
            {
                this.confManager.apiKey = candidate;
                if (string.IsNullOrWhiteSpace(candidate))
                    this.confManager.ClearApi();
                else
                    this.confManager.SaveApi();
            }
        }

        private void LoadApiKeyIntoBox()
        {
            if (AppTheme.IsValorant)
                this.apiKeyBox.Text = this.confManager.valorantApiKey ?? string.Empty;
            else
                this.apiKeyBox.Text = this.confManager.apiKey ?? string.Empty;
        }

        private async void checkAPI()
        {
            string candidate = (this.apiKeyBox.Text ?? string.Empty).Trim();

            if (!this.apiKeyInitialized)
                this.apiKeyInitialized = true;

            if (AppTheme.IsValorant)
            {
                this.valorantApiValid = false;
                this.validatedValorantKey = null;
                if (string.IsNullOrWhiteSpace(candidate))
                {
                    this.confManager.ClearValorantApi();
                    this.apiKeyBox.Text = string.Empty;
                    this.apiColor();
                    this.updateDisplay();
                    return;
                }

                this.confManager.valorantApiKey = candidate;
                this.confManager.SaveValorantApi();
                this.apiKeyBox.Text = candidate;

                Debug.WriteLine("Checking Valorant API Key");
                this.valorantApiValid = await ValorantUtils.checkApiKeyAsync(candidate);
                if (this.valorantApiValid)
                    this.validatedValorantKey = candidate;
                this.apiColor();
                this.updateDisplay();
                return;
            }

            this.api = null;
            this.validatedRiotKey = null;

            if (string.IsNullOrWhiteSpace(candidate))
            {
                this.confManager.ClearApi();
                this.apiKeyBox.Text = string.Empty;
                this.apiColor();
                this.updateDisplay();
                return;
            }

            this.confManager.apiKey = candidate;
            this.confManager.SaveApi();
            this.apiKeyBox.Text = candidate;

            Debug.WriteLine("Checking Riot API Key: " + candidate);
            this.api = RiotUtils.checkAPI(candidate);
            if (this.api != null)
                this.validatedRiotKey = candidate;

            this.apiColor();
            this.updateDisplay();
        }

        private void apiColor()
        {
            if (!HasValidApi())
                this.apiStatus.BackgroundImage = Resources.reddot;
            else
                this.apiStatus.BackgroundImage = Resources.greendot;
        }

        private void apiRefresh_Click(object sender, EventArgs e)
        {
            this.checkAPI();
        }

        private void apiKeyReveal_Click(object sender, EventArgs e)
        {
            this.apiKeyVisible = !this.apiKeyVisible;
            this.ApplyApiKeyVisibility();
        }

        private void ApplyApiKeyVisibility()
        {
            this.apiKeyBox.PasswordChar = this.apiKeyVisible ? '\0' : '*';
            // Segoe MDL2 Assets: EyeReveal / Hide
            this.apiKeyReveal.Text = this.apiKeyVisible ? "\uED1A" : "\uE7B3";
        }

        /*
         * ====================================================================================
         *                              DISPLAY ACCOUNT
         * ====================================================================================
        */

        private async void refreshAccount_Click(object sender, EventArgs e)
        {
            if (!HasValidApi())
                return;

            foreach (AccountDisplay display in accounts)
            {
                display.Update(true);
                await Task.Delay(200);
            }
        }

        private void RelocateDisplays()
        {
            for (int i = 0; i < this.accounts.Count; i++)
                this.accounts[i].Relocate(i);
        }

        public void updateDisplay()
        {
            // Close all and clear list
            foreach (AccountDisplay display in accounts)
                display.Close();
            this.accounts.Clear();
            // Create all new
            foreach (SmurfsConfiguration conf in this.confManager.Get())
            {
                AccountDisplay displayForm = new AccountDisplay(this, this.confManager, conf, this.api);
                this.innerAccountList.Controls.Add(displayForm);
                this.accounts.Add(displayForm);
            }
            // Refresh and reloc
            this.RelocateDisplays();
            foreach (AccountDisplay display in accounts)
            {
                display.ApplyTheme();
                display.ApplyModeLabels();
            }
        }


        private void MainMenu_Move(object sender, EventArgs e)
        {
            this.RelocateDisplays();
        }

        /*
         * ====================================================================================
         *                              ADD ACCOUNT
         * ====================================================================================
        */

        private void AddAccount_Click(object sender, EventArgs e)
        {
            new AccountAdder(this, this.confManager, this.api);
        }


        /*
         * ====================================================================================
         *                              TITLE BAR BUTTONS
         * ====================================================================================
        */

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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
            int scrollValue = this.innerAccountList.VerticalScroll.Value;
            int maxValue = this.innerAccountList.VerticalScroll.Maximum;
            this.titleBar.MouseMove(sender, e);
            this.innerAccountList.VerticalScroll.Value = scrollValue;
            this.innerAccountList.VerticalScroll.Maximum = maxValue;
        }
    }
}

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

        // Riot session
        private RiotApi api;
        private bool apiKeyInitialized;


        public MainMenu()
        {
            InitializeComponent();

            // Rounded window
            CustomForms.Utils.SetRoundedRegion(this, 50, 50);

            // Configuration Manager
            this.confManager = new ConfigurationManager();
            this.accounts = new List<AccountDisplay>();
            this.apiKeyInitialized = false;

            // Title bar
            this.titleBar = new TitleBar(this);
            this.titleBar.OnPaint(new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));

            // Update
            this.checkAPI();
            this.updateDisplay();
        }



        /*
         * ====================================================================================
         *                              API CHECK
         * ====================================================================================
        */

        private bool HasValidApi()
        {
            return this.api != null && !string.IsNullOrWhiteSpace(this.confManager.apiKey);
        }

        private void checkAPI()
        {
            this.api = null;

            string candidate = this.apiKeyBox.Text;

            // Only on first startup: restore the last saved key into the box
            if (!this.apiKeyInitialized
                && string.IsNullOrWhiteSpace(candidate)
                && !string.IsNullOrWhiteSpace(this.confManager.apiKey))
            {
                candidate = this.confManager.apiKey;
                this.apiKeyBox.Text = candidate;
            }
            this.apiKeyInitialized = true;

            // Empty field = intentionally no API key (do not reload from disk)
            if (string.IsNullOrWhiteSpace(candidate))
            {
                this.confManager.ClearApi();
                this.apiKeyBox.Text = string.Empty;
                this.apiColor();
                this.updateDisplay();
                return;
            }

            candidate = candidate.Trim();
            Debug.WriteLine("Checking API Key: " + candidate);
            this.api = RiotUtils.checkAPI(candidate);

            if (this.api != null)
            {
                this.confManager.apiKey = candidate;
                this.confManager.SaveApi();
                this.apiKeyBox.Text = candidate;
            }
            else
            {
                this.confManager.ClearApi();
                this.apiKeyBox.Text = string.Empty;
            }

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

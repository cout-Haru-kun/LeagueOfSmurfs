using LeagueOfSmurfs.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Drawing;
using System.Windows.Forms;

namespace LeagueOfSmurfs
{
    partial class MainMenu
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.icon = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.AccountList = new LeagueOfSmurfs.CustomForms.LightPanel();
            this.innerAccountList = new LeagueOfSmurfs.CustomForms.LightPanel();
            this.ButtonsPanel = new LeagueOfSmurfs.CustomForms.LightPanel();
            this.apiRefresh = new LeagueOfSmurfs.CustomForms.FlatButton();
            this.apiKeyBox = new LeagueOfSmurfs.CustomForms.LightTextBox();
            this.apiStatus = new LeagueOfSmurfs.CustomForms.FlatButton();
            this.AddAccount = new LeagueOfSmurfs.CustomForms.FlatButton();
            this.refreshAccount = new LeagueOfSmurfs.CustomForms.FlatButton();
            this.MinimizeButton = new LeagueOfSmurfs.CustomForms.FlatButton();
            this.CloseButton = new LeagueOfSmurfs.CustomForms.FlatButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.AccountList.SuspendLayout();
            this.ButtonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.pictureBox2.Image = global::LeagueOfSmurfs.Properties.Resources.title;
            this.pictureBox2.Location = new System.Drawing.Point(420, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(150, 30);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // icon
            // 
            this.icon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.icon.BackgroundImage = global::LeagueOfSmurfs.Properties.Resources.LolSmurflogo;
            this.icon.Location = new System.Drawing.Point(15, 0);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(30, 30);
            this.icon.TabIndex = 5;
            this.icon.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(998, 669);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // AccountList
            // 
            this.AccountList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.AccountList.Controls.Add(this.innerAccountList);
            this.AccountList.Location = new System.Drawing.Point(31, 120);
            this.AccountList.Name = "AccountList";
            this.AccountList.Size = new System.Drawing.Size(940, 550);
            this.AccountList.TabIndex = 3;
            // 
            // innerAccountList
            // 
            this.innerAccountList.AutoScroll = true;
            this.innerAccountList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.innerAccountList.Location = new System.Drawing.Point(0, 0);
            this.innerAccountList.Name = "innerAccountList";
            this.innerAccountList.Size = new System.Drawing.Size(960, 550);
            this.innerAccountList.TabIndex = 7;
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ButtonsPanel.Controls.Add(this.apiRefresh);
            this.ButtonsPanel.Controls.Add(this.apiKeyBox);
            this.ButtonsPanel.Controls.Add(this.apiStatus);
            this.ButtonsPanel.Controls.Add(this.AddAccount);
            this.ButtonsPanel.Controls.Add(this.refreshAccount);
            this.ButtonsPanel.Location = new System.Drawing.Point(31, 50);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Size = new System.Drawing.Size(940, 50);
            this.ButtonsPanel.TabIndex = 2;
            // 
            // apiRefresh
            // 
            this.apiRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.apiRefresh.BackgroundImage = global::LeagueOfSmurfs.Properties.Resources.refresh;
            this.apiRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.apiRefresh.FlatAppearance.BorderSize = 0;
            this.apiRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.apiRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.apiRefresh.Location = new System.Drawing.Point(637, 14);
            this.apiRefresh.Name = "apiRefresh";
            this.apiRefresh.Size = new System.Drawing.Size(20, 20);
            this.apiRefresh.TabIndex = 6;
            this.apiRefresh.UseVisualStyleBackColor = false;
            this.apiRefresh.Click += new System.EventHandler(this.apiRefresh_Click);
            // 
            // apiKeyBox
            // 
            this.apiKeyBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.apiKeyBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.apiKeyBox.ForeColor = System.Drawing.Color.White;
            this.apiKeyBox.Location = new System.Drawing.Point(324, 19);
            this.apiKeyBox.MaxLength = 50;
            this.apiKeyBox.Name = "apiKeyBox";
            this.apiKeyBox.Size = new System.Drawing.Size(307, 13);
            this.apiKeyBox.TabIndex = 5;
            // 
            // apiStatus
            // 
            this.apiStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.apiStatus.BackgroundImage = global::LeagueOfSmurfs.Properties.Resources.reddot;
            this.apiStatus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.apiStatus.FlatAppearance.BorderSize = 0;
            this.apiStatus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.apiStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.apiStatus.Location = new System.Drawing.Point(308, 20);
            this.apiStatus.Name = "apiStatus";
            this.apiStatus.Size = new System.Drawing.Size(10, 10);
            this.apiStatus.TabIndex = 4;
            this.apiStatus.UseVisualStyleBackColor = false;
            // 
            // AddAccount
            // 
            this.AddAccount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.AddAccount.BackgroundImage = global::LeagueOfSmurfs.Properties.Resources.add;
            this.AddAccount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddAccount.FlatAppearance.BorderSize = 0;
            this.AddAccount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.AddAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddAccount.Location = new System.Drawing.Point(830, 5);
            this.AddAccount.Name = "AddAccount";
            this.AddAccount.Size = new System.Drawing.Size(40, 40);
            this.AddAccount.TabIndex = 2;
            this.AddAccount.UseVisualStyleBackColor = false;
            this.AddAccount.Click += new System.EventHandler(this.AddAccount_Click);
            // 
            // refreshAccount
            // 
            this.refreshAccount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.refreshAccount.BackgroundImage = global::LeagueOfSmurfs.Properties.Resources.refresh;
            this.refreshAccount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.refreshAccount.FlatAppearance.BorderSize = 0;
            this.refreshAccount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.refreshAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshAccount.Location = new System.Drawing.Point(875, 5);
            this.refreshAccount.Name = "refreshAccount";
            this.refreshAccount.Size = new System.Drawing.Size(40, 40);
            this.refreshAccount.TabIndex = 3;
            this.refreshAccount.UseVisualStyleBackColor = false;
            this.refreshAccount.Click += new System.EventHandler(this.refreshAccount_Click);
            // 
            // MinimizeButton
            // 
            this.MinimizeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.MinimizeButton.BackgroundImage = global::LeagueOfSmurfs.Properties.Resources.minimize;
            this.MinimizeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MinimizeButton.FlatAppearance.BorderSize = 0;
            this.MinimizeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.MinimizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MinimizeButton.Location = new System.Drawing.Point(932, 3);
            this.MinimizeButton.Name = "MinimizeButton";
            this.MinimizeButton.Size = new System.Drawing.Size(20, 20);
            this.MinimizeButton.TabIndex = 1;
            this.MinimizeButton.UseVisualStyleBackColor = false;
            this.MinimizeButton.Click += new System.EventHandler(this.MinimizeButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.CloseButton.BackgroundImage = global::LeagueOfSmurfs.Properties.Resources.close;
            this.CloseButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CloseButton.FlatAppearance.BorderSize = 0;
            this.CloseButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseButton.Location = new System.Drawing.Point(958, 3);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(20, 20);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.UseVisualStyleBackColor = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.icon);
            this.Controls.Add(this.AccountList);
            this.Controls.Add(this.ButtonsPanel);
            this.Controls.Add(this.MinimizeButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainMenu";
            this.Text = "League of Smurfs";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form_MouseUp);
            this.Move += new System.EventHandler(this.MainMenu_Move);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.AccountList.ResumeLayout(false);
            this.ButtonsPanel.ResumeLayout(false);
            this.ButtonsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomForms.FlatButton CloseButton;
        private CustomForms.FlatButton MinimizeButton;
        private CustomForms.LightPanel ButtonsPanel;
        private CustomForms.LightPanel AccountList;
        private PictureBox pictureBox1;
        private PictureBox icon;
        private PictureBox pictureBox2;
        private CustomForms.LightPanel innerAccountList;
        private CustomForms.FlatButton AddAccount;
        private CustomForms.FlatButton refreshAccount;
        private CustomForms.FlatButton apiStatus;
        private CustomForms.LightTextBox apiKeyBox;
        private CustomForms.FlatButton apiRefresh;
    }
}


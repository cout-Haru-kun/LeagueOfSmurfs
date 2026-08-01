namespace LeagueOfSmurfs
{
    partial class AccountDisplay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountDisplay));
            this.progressTimer = new System.Windows.Forms.Timer(this.components);
            this.buffPopupCloser = new System.Windows.Forms.Timer(this.components);
            this.editButton = new LeagueOfSmurfs.CustomForms.FlatButtonRaw();
            this.regionLabel = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.operationProcessBar = new LeagueOfSmurfs.CustomForms.FlatProgressBar();
            this.summonerError = new LeagueOfSmurfs.CustomForms.ErrorLabel();
            this.summonerNameLabel = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.soloLpLabel = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.soloLadderLabel = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.soloLabel = new LeagueOfSmurfs.CustomForms.PrimaryLabel();
            this.flexLpLabel = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.flexLadderLabel = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.levelLabel = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.flexLabel = new LeagueOfSmurfs.CustomForms.PrimaryLabel();
            this.summonerLabel = new LeagueOfSmurfs.CustomForms.PrimaryLabel();
            this.deleteButton = new LeagueOfSmurfs.CustomForms.FlatButtonRaw();
            this.launchButton = new LeagueOfSmurfs.CustomForms.FlatButtonRaw();
            this.SuspendLayout();
            // 
            // progressTimer
            // 
            this.progressTimer.Enabled = true;
            this.progressTimer.Interval = 10;
            this.progressTimer.Tick += new System.EventHandler(this.progressTimer_Tick);
            // 
            // buffPopupCloser
            // 
            this.buffPopupCloser.Enabled = true;
            this.buffPopupCloser.Interval = 10000;
            this.buffPopupCloser.Tick += new System.EventHandler(this.buffPopupCloser_Tick);
            // 
            // editButton
            // 
            this.editButton.BackColor = System.Drawing.Color.Transparent;
            this.editButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("editButton.BackgroundImage")));
            this.editButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.editButton.FlatAppearance.BorderSize = 0;
            this.editButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.editButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editButton.ForeColor = System.Drawing.Color.Orange;
            this.editButton.Location = new System.Drawing.Point(413, 6);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(25, 25);
            this.editButton.TabIndex = 21;
            this.editButton.UseVisualStyleBackColor = false;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // regionLabel
            // 
            this.regionLabel.AutoSize = true;
            this.regionLabel.BackColor = System.Drawing.Color.Transparent;
            this.regionLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.regionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.regionLabel.Location = new System.Drawing.Point(20, 26);
            this.regionLabel.Name = "regionLabel";
            this.regionLabel.Size = new System.Drawing.Size(42, 19);
            this.regionLabel.TabIndex = 20;
            this.regionLabel.Text = "PTDR";
            this.regionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // operationProcessBar
            // 
            this.operationProcessBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(134)))), ((int)(((byte)(252)))));
            this.operationProcessBar.Location = new System.Drawing.Point(0, 140);
            this.operationProcessBar.Name = "operationProcessBar";
            this.operationProcessBar.Size = new System.Drawing.Size(450, 5);
            this.operationProcessBar.TabIndex = 19;
            this.operationProcessBar.Value = 100;
            // 
            // summonerError
            // 
            this.summonerError.AutoSize = true;
            this.summonerError.Enabled = false;
            this.summonerError.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.summonerError.ForeColor = System.Drawing.Color.Red;
            this.summonerError.Location = new System.Drawing.Point(7, 76);
            this.summonerError.Name = "summonerError";
            this.summonerError.Size = new System.Drawing.Size(0, 13);
            this.summonerError.TabIndex = 18;
            this.summonerError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.summonerError.Visible = false;
            // 
            // summonerNameLabel
            // 
            this.summonerNameLabel.AutoSize = true;
            this.summonerNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.summonerNameLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.summonerNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.summonerNameLabel.Location = new System.Drawing.Point(65, 26);
            this.summonerNameLabel.Name = "summonerNameLabel";
            this.summonerNameLabel.Size = new System.Drawing.Size(102, 19);
            this.summonerNameLabel.TabIndex = 17;
            this.summonerNameLabel.Text = "Ton gros daron";
            this.summonerNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // soloLpLabel
            // 
            this.soloLpLabel.AutoSize = true;
            this.soloLpLabel.BackColor = System.Drawing.Color.Transparent;
            this.soloLpLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.soloLpLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.soloLpLabel.Location = new System.Drawing.Point(230, 116);
            this.soloLpLabel.Name = "soloLpLabel";
            this.soloLpLabel.Size = new System.Drawing.Size(39, 19);
            this.soloLpLabel.TabIndex = 16;
            this.soloLpLabel.Text = "LP: 0";
            this.soloLpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // soloLadderLabel
            // 
            this.soloLadderLabel.AutoSize = true;
            this.soloLadderLabel.BackColor = System.Drawing.Color.Transparent;
            this.soloLadderLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.soloLadderLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.soloLadderLabel.Location = new System.Drawing.Point(230, 97);
            this.soloLadderLabel.Name = "soloLadderLabel";
            this.soloLadderLabel.Size = new System.Drawing.Size(118, 19);
            this.soloLadderLabel.TabIndex = 15;
            this.soloLadderLabel.Text = "Ladder: Unranked";
            this.soloLadderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // soloLabel
            // 
            this.soloLabel.AutoSize = true;
            this.soloLabel.BackColor = System.Drawing.Color.Transparent;
            this.soloLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.soloLabel.ForeColor = System.Drawing.Color.White;
            this.soloLabel.Location = new System.Drawing.Point(230, 76);
            this.soloLabel.Name = "soloLabel";
            this.soloLabel.Size = new System.Drawing.Size(83, 21);
            this.soloLabel.TabIndex = 14;
            this.soloLabel.Text = "Solo/Duo";
            this.soloLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flexLpLabel
            // 
            this.flexLpLabel.AutoSize = true;
            this.flexLpLabel.BackColor = System.Drawing.Color.Transparent;
            this.flexLpLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.flexLpLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.flexLpLabel.Location = new System.Drawing.Point(20, 115);
            this.flexLpLabel.Name = "flexLpLabel";
            this.flexLpLabel.Size = new System.Drawing.Size(39, 19);
            this.flexLpLabel.TabIndex = 13;
            this.flexLpLabel.Text = "LP: 0";
            this.flexLpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flexLadderLabel
            // 
            this.flexLadderLabel.AutoSize = true;
            this.flexLadderLabel.BackColor = System.Drawing.Color.Transparent;
            this.flexLadderLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.flexLadderLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.flexLadderLabel.Location = new System.Drawing.Point(20, 95);
            this.flexLadderLabel.Name = "flexLadderLabel";
            this.flexLadderLabel.Size = new System.Drawing.Size(118, 19);
            this.flexLadderLabel.TabIndex = 12;
            this.flexLadderLabel.Text = "Ladder: Unranked";
            this.flexLadderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // levelLabel
            // 
            this.levelLabel.AutoSize = true;
            this.levelLabel.BackColor = System.Drawing.Color.Transparent;
            this.levelLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.levelLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.levelLabel.Location = new System.Drawing.Point(20, 46);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(55, 19);
            this.levelLabel.TabIndex = 11;
            this.levelLabel.Text = "Level: 0";
            this.levelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flexLabel
            // 
            this.flexLabel.AutoSize = true;
            this.flexLabel.BackColor = System.Drawing.Color.Transparent;
            this.flexLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flexLabel.ForeColor = System.Drawing.Color.White;
            this.flexLabel.Location = new System.Drawing.Point(15, 75);
            this.flexLabel.Name = "flexLabel";
            this.flexLabel.Size = new System.Drawing.Size(41, 21);
            this.flexLabel.TabIndex = 10;
            this.flexLabel.Text = "Flex";
            this.flexLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // summonerLabel
            // 
            this.summonerLabel.AutoSize = true;
            this.summonerLabel.BackColor = System.Drawing.Color.Transparent;
            this.summonerLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.summonerLabel.ForeColor = System.Drawing.Color.White;
            this.summonerLabel.Location = new System.Drawing.Point(15, 6);
            this.summonerLabel.Name = "summonerLabel";
            this.summonerLabel.Size = new System.Drawing.Size(94, 21);
            this.summonerLabel.TabIndex = 9;
            this.summonerLabel.Text = "Summoner";
            this.summonerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // deleteButton
            // 
            this.deleteButton.BackColor = System.Drawing.Color.Transparent;
            this.deleteButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("deleteButton.BackgroundImage")));
            this.deleteButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.deleteButton.FlatAppearance.BorderSize = 0;
            this.deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteButton.ForeColor = System.Drawing.Color.Red;
            this.deleteButton.Location = new System.Drawing.Point(413, 37);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(25, 25);
            this.deleteButton.TabIndex = 1;
            this.deleteButton.UseVisualStyleBackColor = false;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // launchButton
            // 
            this.launchButton.BackColor = System.Drawing.Color.Transparent;
            this.launchButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("launchButton.BackgroundImage")));
            this.launchButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.launchButton.FlatAppearance.BorderSize = 0;
            this.launchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.launchButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.launchButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(134)))), ((int)(((byte)(252)))));
            this.launchButton.Location = new System.Drawing.Point(390, 85);
            this.launchButton.Name = "launchButton";
            this.launchButton.Size = new System.Drawing.Size(50, 50);
            this.launchButton.TabIndex = 0;
            this.launchButton.UseVisualStyleBackColor = false;
            this.launchButton.Click += new System.EventHandler(this.launchButton_Click);
            // 
            // AccountDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(450, 145);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.regionLabel);
            this.Controls.Add(this.operationProcessBar);
            this.Controls.Add(this.summonerError);
            this.Controls.Add(this.summonerNameLabel);
            this.Controls.Add(this.soloLpLabel);
            this.Controls.Add(this.soloLadderLabel);
            this.Controls.Add(this.soloLabel);
            this.Controls.Add(this.flexLpLabel);
            this.Controls.Add(this.flexLadderLabel);
            this.Controls.Add(this.levelLabel);
            this.Controls.Add(this.flexLabel);
            this.Controls.Add(this.summonerLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.launchButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AccountDisplay";
            this.Text = "AccountDisplay";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.AccountDisplay_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomForms.FlatButtonRaw launchButton;
        private CustomForms.FlatButtonRaw deleteButton;
        private CustomForms.SecondaryLabel summonerNameLabel;
        private CustomForms.SecondaryLabel soloLpLabel;
        private CustomForms.SecondaryLabel soloLadderLabel;
        private CustomForms.PrimaryLabel soloLabel;
        private CustomForms.SecondaryLabel flexLpLabel;
        private CustomForms.SecondaryLabel flexLadderLabel;
        private CustomForms.SecondaryLabel levelLabel;
        private CustomForms.PrimaryLabel flexLabel;
        private CustomForms.PrimaryLabel summonerLabel;
        private CustomForms.ErrorLabel summonerError;
        private CustomForms.FlatProgressBar operationProcessBar;
        private System.Windows.Forms.Timer progressTimer;
        private CustomForms.SecondaryLabel regionLabel;
        private CustomForms.FlatButtonRaw editButton;
        private System.Windows.Forms.Timer buffPopupCloser;
    }
}
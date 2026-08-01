namespace LeagueOfSmurfs
{
    partial class AccountAdder
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

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountAdder));
            this.summonerError = new LeagueOfSmurfs.CustomForms.ErrorLabel();
            this.checkAccountButton = new LeagueOfSmurfs.CustomForms.FlatButtonRaw();
            this.confirmAdd = new LeagueOfSmurfs.CustomForms.FlatButtonRaw();
            this.lightPanel2 = new LeagueOfSmurfs.CustomForms.LightPanel();
            this.usernameBox = new LeagueOfSmurfs.CustomForms.LightTextBox();
            this.username = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.lightPanel1 = new LeagueOfSmurfs.CustomForms.LightPanel();
            this.passwordBox = new LeagueOfSmurfs.CustomForms.LightTextBox();
            this.password = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.accountCheckPanel = new LeagueOfSmurfs.CustomForms.LightPanel();
            this.summonerNameLabel = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.soloLpLabel = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.soloLadderLabel = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.soloLabel = new LeagueOfSmurfs.CustomForms.PrimaryLabel();
            this.flexLpLabel = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.flexLadderLabel = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.levelLabel = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.flexLabel = new LeagueOfSmurfs.CustomForms.PrimaryLabel();
            this.summonerLabel = new LeagueOfSmurfs.CustomForms.PrimaryLabel();
            this.riotNamePanel = new LeagueOfSmurfs.CustomForms.LightPanel();
            this.tagLabel = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.tagBox = new LeagueOfSmurfs.CustomForms.LightTextBox();
            this.regionBox = new System.Windows.Forms.ComboBox();
            this.riotName = new LeagueOfSmurfs.CustomForms.SecondaryLabel();
            this.summonerNameBox = new LeagueOfSmurfs.CustomForms.LightTextBox();
            this.CloseButton = new LeagueOfSmurfs.CustomForms.FlatButton();
            this.lightPanel2.SuspendLayout();
            this.lightPanel1.SuspendLayout();
            this.accountCheckPanel.SuspendLayout();
            this.riotNamePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // summonerError
            // 
            this.summonerError.AutoSize = true;
            this.summonerError.Enabled = false;
            this.summonerError.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.summonerError.ForeColor = System.Drawing.Color.Red;
            this.summonerError.Location = new System.Drawing.Point(210, 35);
            this.summonerError.Name = "summonerError";
            this.summonerError.Size = new System.Drawing.Size(0, 13);
            this.summonerError.TabIndex = 7;
            this.summonerError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.summonerError.Visible = false;
            // 
            // checkAccountButton
            // 
            this.checkAccountButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.checkAccountButton.FlatAppearance.BorderSize = 0;
            this.checkAccountButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkAccountButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkAccountButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(134)))), ((int)(((byte)(252)))));
            this.checkAccountButton.Location = new System.Drawing.Point(385, 50);
            this.checkAccountButton.Name = "checkAccountButton";
            this.checkAccountButton.Size = new System.Drawing.Size(80, 30);
            this.checkAccountButton.TabIndex = 3;
            this.checkAccountButton.Text = "Check";
            this.checkAccountButton.UseVisualStyleBackColor = false;
            this.checkAccountButton.Click += new System.EventHandler(this.checkAccountButton_Click);
            // 
            // confirmAdd
            // 
            this.confirmAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(134)))), ((int)(((byte)(252)))));
            this.confirmAdd.FlatAppearance.BorderSize = 0;
            this.confirmAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.confirmAdd.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.confirmAdd.ForeColor = System.Drawing.Color.Black;
            this.confirmAdd.Location = new System.Drawing.Point(210, 310);
            this.confirmAdd.Name = "confirmAdd";
            this.confirmAdd.Size = new System.Drawing.Size(75, 30);
            this.confirmAdd.TabIndex = 6;
            this.confirmAdd.Text = "Confirm";
            this.confirmAdd.UseVisualStyleBackColor = false;
            this.confirmAdd.Click += new System.EventHandler(this.confirmAdd_Click);
            // 
            // lightPanel2
            // 
            this.lightPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lightPanel2.Controls.Add(this.usernameBox);
            this.lightPanel2.Controls.Add(this.username);
            this.lightPanel2.Location = new System.Drawing.Point(130, 210);
            this.lightPanel2.Name = "lightPanel2";
            this.lightPanel2.Size = new System.Drawing.Size(240, 30);
            this.lightPanel2.TabIndex = 4;
            // 
            // usernameBox
            // 
            this.usernameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.usernameBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.usernameBox.ForeColor = System.Drawing.Color.White;
            this.usernameBox.Location = new System.Drawing.Point(79, 9);
            this.usernameBox.MaxLength = 16;
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(141, 13);
            this.usernameBox.TabIndex = 4;
            this.usernameBox.TextChanged += new System.EventHandler(this.usernameBox_TextChanged);
            // 
            // username
            // 
            this.username.AutoSize = true;
            this.username.BackColor = System.Drawing.Color.Transparent;
            this.username.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.username.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.username.Location = new System.Drawing.Point(3, 4);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(74, 19);
            this.username.TabIndex = 0;
            this.username.Text = "Username:";
            this.username.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lightPanel1
            // 
            this.lightPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lightPanel1.Controls.Add(this.passwordBox);
            this.lightPanel1.Controls.Add(this.password);
            this.lightPanel1.Location = new System.Drawing.Point(130, 250);
            this.lightPanel1.Name = "lightPanel1";
            this.lightPanel1.Size = new System.Drawing.Size(240, 30);
            this.lightPanel1.TabIndex = 3;
            // 
            // passwordBox
            // 
            this.passwordBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.passwordBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.passwordBox.ForeColor = System.Drawing.Color.White;
            this.passwordBox.Location = new System.Drawing.Point(79, 8);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.PasswordChar = '*';
            this.passwordBox.Size = new System.Drawing.Size(141, 13);
            this.passwordBox.TabIndex = 5;
            this.passwordBox.TextChanged += new System.EventHandler(this.passwordBox_TextChanged);
            // 
            // password
            // 
            this.password.AutoSize = true;
            this.password.BackColor = System.Drawing.Color.Transparent;
            this.password.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.password.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.password.Location = new System.Drawing.Point(3, 4);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(70, 19);
            this.password.TabIndex = 0;
            this.password.Text = "Password:";
            this.password.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // accountCheckPanel
            // 
            this.accountCheckPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.accountCheckPanel.Controls.Add(this.summonerNameLabel);
            this.accountCheckPanel.Controls.Add(this.soloLpLabel);
            this.accountCheckPanel.Controls.Add(this.soloLadderLabel);
            this.accountCheckPanel.Controls.Add(this.soloLabel);
            this.accountCheckPanel.Controls.Add(this.flexLpLabel);
            this.accountCheckPanel.Controls.Add(this.flexLadderLabel);
            this.accountCheckPanel.Controls.Add(this.levelLabel);
            this.accountCheckPanel.Controls.Add(this.flexLabel);
            this.accountCheckPanel.Controls.Add(this.summonerLabel);
            this.accountCheckPanel.Location = new System.Drawing.Point(50, 100);
            this.accountCheckPanel.Name = "accountCheckPanel";
            this.accountCheckPanel.Size = new System.Drawing.Size(400, 90);
            this.accountCheckPanel.TabIndex = 4;
            // 
            // summonerNameLabel
            // 
            this.summonerNameLabel.AutoSize = true;
            this.summonerNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.summonerNameLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.summonerNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.summonerNameLabel.Location = new System.Drawing.Point(9, 40);
            this.summonerNameLabel.Name = "summonerNameLabel";
            this.summonerNameLabel.Size = new System.Drawing.Size(0, 19);
            this.summonerNameLabel.TabIndex = 8;
            this.summonerNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // soloLpLabel
            // 
            this.soloLpLabel.AutoSize = true;
            this.soloLpLabel.BackColor = System.Drawing.Color.Transparent;
            this.soloLpLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.soloLpLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.soloLpLabel.Location = new System.Drawing.Point(260, 59);
            this.soloLpLabel.Name = "soloLpLabel";
            this.soloLpLabel.Size = new System.Drawing.Size(39, 19);
            this.soloLpLabel.TabIndex = 7;
            this.soloLpLabel.Text = "LP: 0";
            this.soloLpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // soloLadderLabel
            // 
            this.soloLadderLabel.AutoSize = true;
            this.soloLadderLabel.BackColor = System.Drawing.Color.Transparent;
            this.soloLadderLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.soloLadderLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.soloLadderLabel.Location = new System.Drawing.Point(260, 40);
            this.soloLadderLabel.Name = "soloLadderLabel";
            this.soloLadderLabel.Size = new System.Drawing.Size(118, 19);
            this.soloLadderLabel.TabIndex = 6;
            this.soloLadderLabel.Text = "Ladder: Unranked";
            this.soloLadderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // soloLabel
            // 
            this.soloLabel.AutoSize = true;
            this.soloLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.soloLabel.ForeColor = System.Drawing.Color.White;
            this.soloLabel.Location = new System.Drawing.Point(260, 9);
            this.soloLabel.Name = "soloLabel";
            this.soloLabel.Size = new System.Drawing.Size(83, 21);
            this.soloLabel.TabIndex = 5;
            this.soloLabel.Text = "Solo/Duo";
            this.soloLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flexLpLabel
            // 
            this.flexLpLabel.AutoSize = true;
            this.flexLpLabel.BackColor = System.Drawing.Color.Transparent;
            this.flexLpLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.flexLpLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.flexLpLabel.Location = new System.Drawing.Point(128, 59);
            this.flexLpLabel.Name = "flexLpLabel";
            this.flexLpLabel.Size = new System.Drawing.Size(39, 19);
            this.flexLpLabel.TabIndex = 4;
            this.flexLpLabel.Text = "LP: 0";
            this.flexLpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flexLadderLabel
            // 
            this.flexLadderLabel.AutoSize = true;
            this.flexLadderLabel.BackColor = System.Drawing.Color.Transparent;
            this.flexLadderLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.flexLadderLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.flexLadderLabel.Location = new System.Drawing.Point(128, 40);
            this.flexLadderLabel.Name = "flexLadderLabel";
            this.flexLadderLabel.Size = new System.Drawing.Size(118, 19);
            this.flexLadderLabel.TabIndex = 3;
            this.flexLadderLabel.Text = "Ladder: Unranked";
            this.flexLadderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // levelLabel
            // 
            this.levelLabel.AutoSize = true;
            this.levelLabel.BackColor = System.Drawing.Color.Transparent;
            this.levelLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.levelLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.levelLabel.Location = new System.Drawing.Point(9, 59);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(55, 19);
            this.levelLabel.TabIndex = 2;
            this.levelLabel.Text = "Level: 0";
            this.levelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flexLabel
            // 
            this.flexLabel.AutoSize = true;
            this.flexLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flexLabel.ForeColor = System.Drawing.Color.White;
            this.flexLabel.Location = new System.Drawing.Point(128, 9);
            this.flexLabel.Name = "flexLabel";
            this.flexLabel.Size = new System.Drawing.Size(41, 21);
            this.flexLabel.TabIndex = 1;
            this.flexLabel.Text = "Flex";
            this.flexLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // summonerLabel
            // 
            this.summonerLabel.AutoSize = true;
            this.summonerLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.summonerLabel.ForeColor = System.Drawing.Color.White;
            this.summonerLabel.Location = new System.Drawing.Point(9, 9);
            this.summonerLabel.Name = "summonerLabel";
            this.summonerLabel.Size = new System.Drawing.Size(94, 21);
            this.summonerLabel.TabIndex = 0;
            this.summonerLabel.Text = "Summoner";
            this.summonerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // riotNamePanel
            // 
            this.riotNamePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.riotNamePanel.Controls.Add(this.tagLabel);
            this.riotNamePanel.Controls.Add(this.tagBox);
            this.riotNamePanel.Controls.Add(this.regionBox);
            this.riotNamePanel.Controls.Add(this.riotName);
            this.riotNamePanel.Controls.Add(this.summonerNameBox);
            this.riotNamePanel.Location = new System.Drawing.Point(35, 50);
            this.riotNamePanel.Name = "riotNamePanel";
            this.riotNamePanel.Size = new System.Drawing.Size(344, 30);
            this.riotNamePanel.TabIndex = 2;
            // 
            // tagLabel
            // 
            this.tagLabel.AutoSize = true;
            this.tagLabel.BackColor = System.Drawing.Color.Transparent;
            this.tagLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tagLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.tagLabel.Location = new System.Drawing.Point(179, 4);
            this.tagLabel.Name = "tagLabel";
            this.tagLabel.Size = new System.Drawing.Size(32, 19);
            this.tagLabel.TabIndex = 12;
            this.tagLabel.Text = "Tag:";
            this.tagLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tagBox
            // 
            this.tagBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tagBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tagBox.ForeColor = System.Drawing.Color.White;
            this.tagBox.Location = new System.Drawing.Point(218, 8);
            this.tagBox.MaxLength = 5;
            this.tagBox.Name = "tagBox";
            this.tagBox.Size = new System.Drawing.Size(50, 13);
            this.tagBox.TabIndex = 1;
            this.tagBox.TextChanged += new System.EventHandler(this.tagBox_TextChanged);
            // 
            // regionBox
            // 
            this.regionBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.regionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.regionBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.regionBox.ForeColor = System.Drawing.Color.White;
            this.regionBox.FormattingEnabled = true;
            this.regionBox.Location = new System.Drawing.Point(283, 4);
            this.regionBox.MaxDropDownItems = 16;
            this.regionBox.Name = "regionBox";
            this.regionBox.Size = new System.Drawing.Size(52, 21);
            this.regionBox.TabIndex = 2;
            this.regionBox.SelectedValueChanged += new System.EventHandler(this.regionBox_SelectedValueChanged);
            // 
            // riotName
            // 
            this.riotName.AutoSize = true;
            this.riotName.BackColor = System.Drawing.Color.Transparent;
            this.riotName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.riotName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.riotName.Location = new System.Drawing.Point(5, 4);
            this.riotName.Name = "riotName";
            this.riotName.Size = new System.Drawing.Size(48, 19);
            this.riotName.TabIndex = 1;
            this.riotName.Text = "Name:";
            this.riotName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // summonerNameBox
            // 
            this.summonerNameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.summonerNameBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.summonerNameBox.ForeColor = System.Drawing.Color.White;
            this.summonerNameBox.Location = new System.Drawing.Point(55, 8);
            this.summonerNameBox.MaxLength = 16;
            this.summonerNameBox.Name = "summonerNameBox";
            this.summonerNameBox.Size = new System.Drawing.Size(122, 13);
            this.summonerNameBox.TabIndex = 0;
            this.summonerNameBox.TextChanged += new System.EventHandler(this.summonerNameBox_TextChanged);
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.CloseButton.BackgroundImage = global::LeagueOfSmurfs.Properties.Resources.close;
            this.CloseButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CloseButton.FlatAppearance.BorderSize = 0;
            this.CloseButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseButton.Location = new System.Drawing.Point(468, 3);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(20, 20);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.UseVisualStyleBackColor = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // AccountAdder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(500, 350);
            this.Controls.Add(this.summonerError);
            this.Controls.Add(this.checkAccountButton);
            this.Controls.Add(this.confirmAdd);
            this.Controls.Add(this.lightPanel2);
            this.Controls.Add(this.lightPanel1);
            this.Controls.Add(this.accountCheckPanel);
            this.Controls.Add(this.riotNamePanel);
            this.Controls.Add(this.CloseButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AccountAdder";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form_MouseUp);
            this.lightPanel2.ResumeLayout(false);
            this.lightPanel2.PerformLayout();
            this.lightPanel1.ResumeLayout(false);
            this.lightPanel1.PerformLayout();
            this.accountCheckPanel.ResumeLayout(false);
            this.accountCheckPanel.PerformLayout();
            this.riotNamePanel.ResumeLayout(false);
            this.riotNamePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomForms.FlatButton CloseButton;
        private CustomForms.LightPanel riotNamePanel;
        private CustomForms.LightPanel accountCheckPanel;
        private CustomForms.LightPanel lightPanel1;
        private CustomForms.LightPanel lightPanel2;
        private CustomForms.SecondaryLabel riotName;
        private CustomForms.LightTextBox summonerNameBox;
        private CustomForms.LightTextBox passwordBox;
        private CustomForms.SecondaryLabel password;
        private CustomForms.SecondaryLabel username;
        private CustomForms.LightTextBox usernameBox;
        private CustomForms.FlatButtonRaw confirmAdd;
        private CustomForms.FlatButtonRaw checkAccountButton;
        private CustomForms.PrimaryLabel summonerLabel;
        private CustomForms.ErrorLabel summonerError;
        private CustomForms.PrimaryLabel flexLabel;
        private CustomForms.SecondaryLabel levelLabel;
        private CustomForms.SecondaryLabel flexLadderLabel;
        private CustomForms.SecondaryLabel flexLpLabel;
        private CustomForms.SecondaryLabel soloLpLabel;
        private CustomForms.SecondaryLabel soloLadderLabel;
        private CustomForms.PrimaryLabel soloLabel;
        private CustomForms.SecondaryLabel summonerNameLabel;
        private System.Windows.Forms.ComboBox regionBox;
        private CustomForms.SecondaryLabel tagLabel;
        private CustomForms.LightTextBox tagBox;
    }
}

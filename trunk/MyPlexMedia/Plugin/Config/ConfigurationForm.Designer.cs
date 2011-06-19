namespace MyPlexMedia.Plugin.Config {
    partial class ConfigurationForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.label16 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBoxDeleteOnExit = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxCheezRootFolder = new System.Windows.Forms.TextBox();
            this.labelCheezRootFolder = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonRefreshBonjourServers = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.FriendlyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.isOnlineDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.uriPlexSectionsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uriPlexBaseDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serverCapabilitiesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.hostNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hostAdressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userPassDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isBonjourDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.plexServerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plexServerBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Trebuchet MS", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Tomato;
            this.label16.Location = new System.Drawing.Point(12, 9);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(224, 43);
            this.label16.TabIndex = 6;
            this.label16.Text = "MyPlexMedia";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 66);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(688, 214);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBoxDeleteOnExit);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.textBoxCheezRootFolder);
            this.tabPage1.Controls.Add(this.labelCheezRootFolder);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(680, 188);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBoxDeleteOnExit
            // 
            this.checkBoxDeleteOnExit.AutoSize = true;
            this.checkBoxDeleteOnExit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxDeleteOnExit.Location = new System.Drawing.Point(9, 56);
            this.checkBoxDeleteOnExit.Name = "checkBoxDeleteOnExit";
            this.checkBoxDeleteOnExit.Size = new System.Drawing.Size(119, 17);
            this.checkBoxDeleteOnExit.TabIndex = 11;
            this.checkBoxDeleteOnExit.Text = "Clear Cache on Exit";
            this.checkBoxDeleteOnExit.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(593, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBoxCheezRootFolder
            // 
            this.textBoxCheezRootFolder.Location = new System.Drawing.Point(139, 26);
            this.textBoxCheezRootFolder.Name = "textBoxCheezRootFolder";
            this.textBoxCheezRootFolder.Size = new System.Drawing.Size(448, 20);
            this.textBoxCheezRootFolder.TabIndex = 7;
            // 
            // labelCheezRootFolder
            // 
            this.labelCheezRootFolder.AutoSize = true;
            this.labelCheezRootFolder.Location = new System.Drawing.Point(6, 29);
            this.labelCheezRootFolder.Name = "labelCheezRootFolder";
            this.labelCheezRootFolder.Size = new System.Drawing.Size(125, 13);
            this.labelCheezRootFolder.TabIndex = 6;
            this.labelCheezRootFolder.Text = "Thumb && Artwork Cache:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonRefreshBonjourServers);
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(680, 188);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Plex Servers";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage2.Enter += new System.EventHandler(this.tabPage2_Enter);
            // 
            // buttonRefreshBonjourServers
            // 
            this.buttonRefreshBonjourServers.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonRefreshBonjourServers.Location = new System.Drawing.Point(3, 162);
            this.buttonRefreshBonjourServers.Name = "buttonRefreshBonjourServers";
            this.buttonRefreshBonjourServers.Size = new System.Drawing.Size(674, 23);
            this.buttonRefreshBonjourServers.TabIndex = 1;
            this.buttonRefreshBonjourServers.Text = "Update Online Status && Discover Plex Servers (Bonjour Discovery)";
            this.buttonRefreshBonjourServers.UseVisualStyleBackColor = true;
            this.buttonRefreshBonjourServers.Click += new System.EventHandler(this.buttonRefreshBonjourServers_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.hostNameDataGridViewTextBoxColumn,
            this.hostAdressDataGridViewTextBoxColumn,
            this.userNameDataGridViewTextBoxColumn,
            this.userPassDataGridViewTextBoxColumn,
            this.FriendlyName,
            this.isBonjourDataGridViewCheckBoxColumn});
            this.dataGridView1.DataSource = this.plexServerBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 25;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.Size = new System.Drawing.Size(674, 153);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView1_RowPrePaint);
            // 
            // FriendlyName
            // 
            this.FriendlyName.DataPropertyName = "FriendlyName";
            this.FriendlyName.FillWeight = 75.85722F;
            this.FriendlyName.HeaderText = "Friendly Name";
            this.FriendlyName.Name = "FriendlyName";
            this.FriendlyName.ReadOnly = true;
            this.FriendlyName.Width = 99;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(680, 188);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Connection Speed && Quality";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::MyPlexMedia.Properties.Resources.icon_default;
            this.pictureBox1.Image = global::MyPlexMedia.Properties.Resources.icon_default;
            this.pictureBox1.Location = new System.Drawing.Point(630, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(70, 70);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // isOnlineDataGridViewCheckBoxColumn
            // 
            this.isOnlineDataGridViewCheckBoxColumn.DataPropertyName = "IsOnline";
            this.isOnlineDataGridViewCheckBoxColumn.HeaderText = "IsOnline";
            this.isOnlineDataGridViewCheckBoxColumn.Name = "isOnlineDataGridViewCheckBoxColumn";
            this.isOnlineDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isOnlineDataGridViewCheckBoxColumn.Width = 59;
            // 
            // uriPlexSectionsDataGridViewTextBoxColumn
            // 
            this.uriPlexSectionsDataGridViewTextBoxColumn.DataPropertyName = "UriPlexSections";
            this.uriPlexSectionsDataGridViewTextBoxColumn.HeaderText = "UriPlexSections";
            this.uriPlexSectionsDataGridViewTextBoxColumn.Name = "uriPlexSectionsDataGridViewTextBoxColumn";
            this.uriPlexSectionsDataGridViewTextBoxColumn.ReadOnly = true;
            this.uriPlexSectionsDataGridViewTextBoxColumn.Width = 58;
            // 
            // uriPlexBaseDataGridViewTextBoxColumn
            // 
            this.uriPlexBaseDataGridViewTextBoxColumn.DataPropertyName = "UriPlexBase";
            this.uriPlexBaseDataGridViewTextBoxColumn.HeaderText = "UriPlexBase";
            this.uriPlexBaseDataGridViewTextBoxColumn.Name = "uriPlexBaseDataGridViewTextBoxColumn";
            this.uriPlexBaseDataGridViewTextBoxColumn.ReadOnly = true;
            this.uriPlexBaseDataGridViewTextBoxColumn.Width = 59;
            // 
            // serverCapabilitiesDataGridViewTextBoxColumn
            // 
            this.serverCapabilitiesDataGridViewTextBoxColumn.DataPropertyName = "ServerCapabilities";
            this.serverCapabilitiesDataGridViewTextBoxColumn.HeaderText = "ServerCapabilities";
            this.serverCapabilitiesDataGridViewTextBoxColumn.Name = "serverCapabilitiesDataGridViewTextBoxColumn";
            this.serverCapabilitiesDataGridViewTextBoxColumn.Width = 58;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(306, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "coming soon...";
            // 
            // hostNameDataGridViewTextBoxColumn
            // 
            this.hostNameDataGridViewTextBoxColumn.DataPropertyName = "HostName";
            this.hostNameDataGridViewTextBoxColumn.FillWeight = 75.85722F;
            this.hostNameDataGridViewTextBoxColumn.HeaderText = "Host Name";
            this.hostNameDataGridViewTextBoxColumn.Name = "hostNameDataGridViewTextBoxColumn";
            this.hostNameDataGridViewTextBoxColumn.Width = 85;
            // 
            // hostAdressDataGridViewTextBoxColumn
            // 
            this.hostAdressDataGridViewTextBoxColumn.DataPropertyName = "HostAdress";
            this.hostAdressDataGridViewTextBoxColumn.FillWeight = 75.85722F;
            this.hostAdressDataGridViewTextBoxColumn.HeaderText = "Host Adress";
            this.hostAdressDataGridViewTextBoxColumn.Name = "hostAdressDataGridViewTextBoxColumn";
            this.hostAdressDataGridViewTextBoxColumn.Width = 89;
            // 
            // userNameDataGridViewTextBoxColumn
            // 
            this.userNameDataGridViewTextBoxColumn.DataPropertyName = "UserName";
            this.userNameDataGridViewTextBoxColumn.FillWeight = 75.85722F;
            this.userNameDataGridViewTextBoxColumn.HeaderText = "User Name";
            this.userNameDataGridViewTextBoxColumn.Name = "userNameDataGridViewTextBoxColumn";
            this.userNameDataGridViewTextBoxColumn.Width = 85;
            // 
            // userPassDataGridViewTextBoxColumn
            // 
            this.userPassDataGridViewTextBoxColumn.DataPropertyName = "UserPass";
            this.userPassDataGridViewTextBoxColumn.FillWeight = 75.85722F;
            this.userPassDataGridViewTextBoxColumn.HeaderText = "User Pass";
            this.userPassDataGridViewTextBoxColumn.Name = "userPassDataGridViewTextBoxColumn";
            this.userPassDataGridViewTextBoxColumn.Width = 80;
            // 
            // isBonjourDataGridViewCheckBoxColumn
            // 
            this.isBonjourDataGridViewCheckBoxColumn.DataPropertyName = "IsBonjour";
            this.isBonjourDataGridViewCheckBoxColumn.FillWeight = 68.13522F;
            this.isBonjourDataGridViewCheckBoxColumn.HeaderText = "Bonjour";
            this.isBonjourDataGridViewCheckBoxColumn.MinimumWidth = 30;
            this.isBonjourDataGridViewCheckBoxColumn.Name = "isBonjourDataGridViewCheckBoxColumn";
            this.isBonjourDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isBonjourDataGridViewCheckBoxColumn.Width = 49;
            // 
            // plexServerBindingSource
            // 
            this.plexServerBindingSource.DataSource = typeof(PlexMediaCenter.Plex.Connection.PlexServer);
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MyPlexMedia.Properties.Resources.config_ctripes;
            this.ClientSize = new System.Drawing.Size(712, 292);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label16);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ConfigurationForm";
            this.Text = "Configuration";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.plexServerBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox checkBoxDeleteOnExit;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxCheezRootFolder;
        private System.Windows.Forms.Label labelCheezRootFolder;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button buttonRefreshBonjourServers;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource plexServerBindingSource;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn serverCapabilitiesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isOnlineDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uriPlexSectionsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uriPlexBaseDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hostNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hostAdressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userPassDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FriendlyName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isBonjourDataGridViewCheckBoxColumn;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label1;


    }
}
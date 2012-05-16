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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
            this.label16 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.buttonMyPlexLogin = new System.Windows.Forms.Button();
            this.textBoxMyPlexPass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxMyPlexUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonManageManualConnections = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.friendlyNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uriPlexBaseDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plexVersionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isMyPlexDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isBonjourDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isManualDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.machineIdentifierDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plexServerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.buttonRefreshBonjourServers = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.checkBoxSelectQualityPriorToPlayback = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelQualityLAN = new System.Windows.Forms.Label();
            this.comboBoxQualityWAN = new System.Windows.Forms.ComboBox();
            this.comboBoxQualityLAN = new System.Windows.Forms.ComboBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBoxDeleteOnExit = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxCacheFolder = new System.Windows.Forms.TextBox();
            this.labelCheezRootFolder = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.isOnlineDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.uriPlexSectionsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uriPlexBaseDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serverCapabilitiesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.baseConnectionInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.checkBoxDownloadArtwork = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plexServerBindingSource)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseConnectionInfoBindingSource)).BeginInit();
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
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 66);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(688, 214);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.linkLabel1);
            this.tabPage4.Controls.Add(this.pictureBox2);
            this.tabPage4.Controls.Add(this.buttonMyPlexLogin);
            this.tabPage4.Controls.Add(this.textBoxMyPlexPass);
            this.tabPage4.Controls.Add(this.label3);
            this.tabPage4.Controls.Add(this.textBoxMyPlexUser);
            this.tabPage4.Controls.Add(this.label2);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(680, 188);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "MyPlex";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(194, 119);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(124, 13);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://my.plexapp.com/";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::MyPlexMedia.Properties.Resources.config_ctripes;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(6, 6);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(148, 176);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // buttonMyPlexLogin
            // 
            this.buttonMyPlexLogin.Location = new System.Drawing.Point(556, 109);
            this.buttonMyPlexLogin.Name = "buttonMyPlexLogin";
            this.buttonMyPlexLogin.Size = new System.Drawing.Size(75, 23);
            this.buttonMyPlexLogin.TabIndex = 4;
            this.buttonMyPlexLogin.Text = "Login";
            this.buttonMyPlexLogin.UseVisualStyleBackColor = true;
            this.buttonMyPlexLogin.Click += new System.EventHandler(this.buttonMyPlexLogin_Click);
            // 
            // textBoxMyPlexPass
            // 
            this.textBoxMyPlexPass.Location = new System.Drawing.Point(403, 83);
            this.textBoxMyPlexPass.Name = "textBoxMyPlexPass";
            this.textBoxMyPlexPass.Size = new System.Drawing.Size(228, 20);
            this.textBoxMyPlexPass.TabIndex = 3;
            this.textBoxMyPlexPass.UseSystemPasswordChar = true;
            this.textBoxMyPlexPass.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxMyPlexPass_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(341, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password:";
            // 
            // textBoxMyPlexUser
            // 
            this.textBoxMyPlexUser.Location = new System.Drawing.Point(403, 57);
            this.textBoxMyPlexUser.Name = "textBoxMyPlexUser";
            this.textBoxMyPlexUser.Size = new System.Drawing.Size(228, 20);
            this.textBoxMyPlexUser.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Email, username, or Plex forum username:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonManageManualConnections);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.buttonRefreshBonjourServers);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(680, 188);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Plex Servers";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage2.Enter += new System.EventHandler(this.tabPage2_Enter);
            // 
            // buttonManageManualConnections
            // 
            this.buttonManageManualConnections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonManageManualConnections.Location = new System.Drawing.Point(6, 159);
            this.buttonManageManualConnections.Name = "buttonManageManualConnections";
            this.buttonManageManualConnections.Size = new System.Drawing.Size(181, 23);
            this.buttonManageManualConnections.TabIndex = 3;
            this.buttonManageManualConnections.Text = "Manage Manual Connections";
            this.buttonManageManualConnections.UseVisualStyleBackColor = true;
            this.buttonManageManualConnections.Click += new System.EventHandler(this.buttonManageManualConnections_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(668, 147);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Plex Servers";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.friendlyNameDataGridViewTextBoxColumn,
            this.uriPlexBaseDataGridViewTextBoxColumn1,
            this.plexVersionDataGridViewTextBoxColumn,
            this.isMyPlexDataGridViewCheckBoxColumn,
            this.isBonjourDataGridViewCheckBoxColumn,
            this.isManualDataGridViewCheckBoxColumn,
            this.machineIdentifierDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.plexServerBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 16);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 25;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.Size = new System.Drawing.Size(662, 128);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView1_RowPrePaint);
            // 
            // friendlyNameDataGridViewTextBoxColumn
            // 
            this.friendlyNameDataGridViewTextBoxColumn.DataPropertyName = "FriendlyName";
            this.friendlyNameDataGridViewTextBoxColumn.FillWeight = 75.26157F;
            this.friendlyNameDataGridViewTextBoxColumn.HeaderText = "FriendlyName";
            this.friendlyNameDataGridViewTextBoxColumn.Name = "friendlyNameDataGridViewTextBoxColumn";
            this.friendlyNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // uriPlexBaseDataGridViewTextBoxColumn1
            // 
            this.uriPlexBaseDataGridViewTextBoxColumn1.DataPropertyName = "UriPlexBase";
            this.uriPlexBaseDataGridViewTextBoxColumn1.FillWeight = 75.26157F;
            this.uriPlexBaseDataGridViewTextBoxColumn1.HeaderText = "Current Url";
            this.uriPlexBaseDataGridViewTextBoxColumn1.Name = "uriPlexBaseDataGridViewTextBoxColumn1";
            this.uriPlexBaseDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // plexVersionDataGridViewTextBoxColumn
            // 
            this.plexVersionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.plexVersionDataGridViewTextBoxColumn.DataPropertyName = "PlexVersion";
            this.plexVersionDataGridViewTextBoxColumn.FillWeight = 75.26157F;
            this.plexVersionDataGridViewTextBoxColumn.HeaderText = "PlexVersion";
            this.plexVersionDataGridViewTextBoxColumn.Name = "plexVersionDataGridViewTextBoxColumn";
            this.plexVersionDataGridViewTextBoxColumn.ReadOnly = true;
            this.plexVersionDataGridViewTextBoxColumn.Width = 87;
            // 
            // isMyPlexDataGridViewCheckBoxColumn
            // 
            this.isMyPlexDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.isMyPlexDataGridViewCheckBoxColumn.DataPropertyName = "IsMyPlex";
            this.isMyPlexDataGridViewCheckBoxColumn.FillWeight = 1F;
            this.isMyPlexDataGridViewCheckBoxColumn.HeaderText = "IsMyPlex";
            this.isMyPlexDataGridViewCheckBoxColumn.MinimumWidth = 20;
            this.isMyPlexDataGridViewCheckBoxColumn.Name = "isMyPlexDataGridViewCheckBoxColumn";
            this.isMyPlexDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isMyPlexDataGridViewCheckBoxColumn.Width = 20;
            // 
            // isBonjourDataGridViewCheckBoxColumn
            // 
            this.isBonjourDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.isBonjourDataGridViewCheckBoxColumn.DataPropertyName = "IsBonjour";
            this.isBonjourDataGridViewCheckBoxColumn.FillWeight = 38.93023F;
            this.isBonjourDataGridViewCheckBoxColumn.HeaderText = "IsBonjour";
            this.isBonjourDataGridViewCheckBoxColumn.MinimumWidth = 20;
            this.isBonjourDataGridViewCheckBoxColumn.Name = "isBonjourDataGridViewCheckBoxColumn";
            this.isBonjourDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isBonjourDataGridViewCheckBoxColumn.Width = 20;
            // 
            // isManualDataGridViewCheckBoxColumn
            // 
            this.isManualDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.isManualDataGridViewCheckBoxColumn.DataPropertyName = "IsManual";
            this.isManualDataGridViewCheckBoxColumn.FillWeight = 42.45407F;
            this.isManualDataGridViewCheckBoxColumn.HeaderText = "IsManual";
            this.isManualDataGridViewCheckBoxColumn.MinimumWidth = 20;
            this.isManualDataGridViewCheckBoxColumn.Name = "isManualDataGridViewCheckBoxColumn";
            this.isManualDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isManualDataGridViewCheckBoxColumn.Width = 20;
            // 
            // machineIdentifierDataGridViewTextBoxColumn
            // 
            this.machineIdentifierDataGridViewTextBoxColumn.DataPropertyName = "MachineIdentifier";
            this.machineIdentifierDataGridViewTextBoxColumn.FillWeight = 75.26157F;
            this.machineIdentifierDataGridViewTextBoxColumn.HeaderText = "MachineIdentifier";
            this.machineIdentifierDataGridViewTextBoxColumn.Name = "machineIdentifierDataGridViewTextBoxColumn";
            // 
            // plexServerBindingSource
            // 
            this.plexServerBindingSource.DataSource = typeof(PlexMediaCenter.Plex.Connection.PlexServer);
            // 
            // buttonRefreshBonjourServers
            // 
            this.buttonRefreshBonjourServers.Location = new System.Drawing.Point(193, 159);
            this.buttonRefreshBonjourServers.Name = "buttonRefreshBonjourServers";
            this.buttonRefreshBonjourServers.Size = new System.Drawing.Size(481, 23);
            this.buttonRefreshBonjourServers.TabIndex = 1;
            this.buttonRefreshBonjourServers.Text = "Update Online Status && Discover Plex Servers (Bonjour Discovery)";
            this.buttonRefreshBonjourServers.UseVisualStyleBackColor = true;
            this.buttonRefreshBonjourServers.Click += new System.EventHandler(this.buttonRefreshBonjourServers_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.checkBoxSelectQualityPriorToPlayback);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.labelQualityLAN);
            this.tabPage3.Controls.Add(this.comboBoxQualityWAN);
            this.tabPage3.Controls.Add(this.comboBoxQualityLAN);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(680, 188);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Connection Speed && Quality";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // checkBoxSelectQualityPriorToPlayback
            // 
            this.checkBoxSelectQualityPriorToPlayback.AutoSize = true;
            this.checkBoxSelectQualityPriorToPlayback.Location = new System.Drawing.Point(344, 119);
            this.checkBoxSelectQualityPriorToPlayback.Name = "checkBoxSelectQualityPriorToPlayback";
            this.checkBoxSelectQualityPriorToPlayback.Size = new System.Drawing.Size(134, 17);
            this.checkBoxSelectQualityPriorToPlayback.TabIndex = 5;
            this.checkBoxSelectQualityPriorToPlayback.Text = "select before Playback";
            this.checkBoxSelectQualityPriorToPlayback.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(163, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Transcoding Quality (WAN/Internet):";
            // 
            // labelQualityLAN
            // 
            this.labelQualityLAN.AutoSize = true;
            this.labelQualityLAN.Location = new System.Drawing.Point(163, 55);
            this.labelQualityLAN.Name = "labelQualityLAN";
            this.labelQualityLAN.Size = new System.Drawing.Size(175, 13);
            this.labelQualityLAN.TabIndex = 3;
            this.labelQualityLAN.Text = "Transcoding Quality (LAN/Bonjour):";
            // 
            // comboBoxQualityWAN
            // 
            this.comboBoxQualityWAN.FormattingEnabled = true;
            this.comboBoxQualityWAN.Location = new System.Drawing.Point(344, 91);
            this.comboBoxQualityWAN.Name = "comboBoxQualityWAN";
            this.comboBoxQualityWAN.Size = new System.Drawing.Size(136, 21);
            this.comboBoxQualityWAN.TabIndex = 2;
            // 
            // comboBoxQualityLAN
            // 
            this.comboBoxQualityLAN.FormattingEnabled = true;
            this.comboBoxQualityLAN.Location = new System.Drawing.Point(344, 52);
            this.comboBoxQualityLAN.Name = "comboBoxQualityLAN";
            this.comboBoxQualityLAN.Size = new System.Drawing.Size(136, 21);
            this.comboBoxQualityLAN.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBoxDownloadArtwork);
            this.tabPage1.Controls.Add(this.checkBoxDeleteOnExit);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.textBoxCacheFolder);
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
            this.checkBoxDeleteOnExit.Location = new System.Drawing.Point(101, 65);
            this.checkBoxDeleteOnExit.Name = "checkBoxDeleteOnExit";
            this.checkBoxDeleteOnExit.Size = new System.Drawing.Size(119, 17);
            this.checkBoxDeleteOnExit.TabIndex = 11;
            this.checkBoxDeleteOnExit.Text = "Clear Cache on Exit";
            this.checkBoxDeleteOnExit.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(554, 105);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBoxCacheFolder
            // 
            this.textBoxCacheFolder.Location = new System.Drawing.Point(100, 107);
            this.textBoxCacheFolder.Name = "textBoxCacheFolder";
            this.textBoxCacheFolder.Size = new System.Drawing.Size(448, 20);
            this.textBoxCacheFolder.TabIndex = 7;
            // 
            // labelCheezRootFolder
            // 
            this.labelCheezRootFolder.AutoSize = true;
            this.labelCheezRootFolder.Location = new System.Drawing.Point(97, 91);
            this.labelCheezRootFolder.Name = "labelCheezRootFolder";
            this.labelCheezRootFolder.Size = new System.Drawing.Size(125, 13);
            this.labelCheezRootFolder.TabIndex = 6;
            this.labelCheezRootFolder.Text = "Thumb && Artwork Cache:";
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
            // baseConnectionInfoBindingSource
            // 
            this.baseConnectionInfoBindingSource.DataSource = typeof(PlexMediaCenter.Plex.Connection.BaseConnectionInfo);
            // 
            // checkBoxDownloadArtwork
            // 
            this.checkBoxDownloadArtwork.AutoSize = true;
            this.checkBoxDownloadArtwork.Checked = true;
            this.checkBoxDownloadArtwork.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDownloadArtwork.Location = new System.Drawing.Point(100, 42);
            this.checkBoxDownloadArtwork.Name = "checkBoxDownloadArtwork";
            this.checkBoxDownloadArtwork.Size = new System.Drawing.Size(328, 17);
            this.checkBoxDownloadArtwork.TabIndex = 12;
            this.checkBoxDownloadArtwork.Text = "Download Artwork (uncheck to save precious bandwidth/traffic)";
            this.checkBoxDownloadArtwork.UseVisualStyleBackColor = true;
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
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.plexServerBindingSource)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseConnectionInfoBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox checkBoxDeleteOnExit;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxCacheFolder;
        private System.Windows.Forms.Label labelCheezRootFolder;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button buttonRefreshBonjourServers;
        private System.Windows.Forms.BindingSource plexServerBindingSource;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn serverCapabilitiesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isOnlineDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uriPlexSectionsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uriPlexBaseDataGridViewTextBoxColumn;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ComboBox comboBoxQualityWAN;
        private System.Windows.Forms.ComboBox comboBoxQualityLAN;
        private System.Windows.Forms.CheckBox checkBoxSelectQualityPriorToPlayback;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelQualityLAN;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button buttonMyPlexLogin;
        private System.Windows.Forms.TextBox textBoxMyPlexPass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxMyPlexUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn userNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userPassDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.BindingSource baseConnectionInfoBindingSource;
        private System.Windows.Forms.Button buttonManageManualConnections;
        private System.Windows.Forms.DataGridViewTextBoxColumn friendlyNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uriPlexBaseDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn plexVersionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isMyPlexDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isBonjourDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isManualDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn machineIdentifierDataGridViewTextBoxColumn;
        private System.Windows.Forms.CheckBox checkBoxDownloadArtwork;        


    }
}
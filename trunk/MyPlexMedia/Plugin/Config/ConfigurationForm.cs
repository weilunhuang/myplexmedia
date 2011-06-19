#region Copyright (C) 2005-2008 Team MediaPortal

/* 
 *      Copyright (C) 2005-2008 Team MediaPortal
 *      http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *   
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *   
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using MediaPortal.UserInterface.Controls;
using PlexMediaCenter.Plex.Connection;
using PlexMediaCenter.Util;
using MediaPortal.GUI.Library;
using System.Windows.Forms;
using PlexMediaCenter.Plex;
using System.Drawing;
using System.Net;
using System.Reflection;


namespace MyPlexMedia.Plugin.Config {
    public partial class ConfigurationForm : MPConfigForm {

        private List<PlexServer> PlexServers { get; set; }
        private WebClient _webClient = new WebClient();

        public ConfigurationForm() {
            try {
                InitializeComponent();
                Load += new EventHandler(ConfigurationForm_Load);
                FormClosing += new System.Windows.Forms.FormClosingEventHandler(ConfigurationForm_FormClosing);
                Settings.Load();
                PlexInterface.Init(Settings.PLEX_SERVER_LIST_XML, Settings.PLEX_ARTWORK_CACHE_ROOT_PATH, Settings.PLEX_ICON_DEFAULT);
                PlexInterface.ServerManager.OnPlexServersChanged += new ServerManager.OnPlexServersChangedEventHandler(ServerManager_OnPlexServersChanged);
                PlexInterface.ServerManager.OnServerManangerError += new ServerManager.OnServerManangerErrorEventHandler(ServerManager_OnServerManangerError);
                BonjourDiscovery.OnBonjourServer += new BonjourDiscovery.OnBonjourServerEventHandler(BonjourDiscovery_OnBonjourServer);
                PlexServers = PlexInterface.ServerManager.PlexServers;
            } catch (Exception ex) {
                Log.Error(ex);
            }
        }

        void ServerManager_OnPlexServersChanged(List<PlexServer> updatedServerList) {

        }

        void BonjourDiscovery_OnBonjourServer(PlexServer bojourDiscoveredServer) {
            if (PlexServers.Contains<PlexServer>(bojourDiscoveredServer)) {
                PlexServers.Find(x => x.Equals(bojourDiscoveredServer)).IsBonjour = true;
            } else {
                PlexServers.Add(bojourDiscoveredServer);
            }
            plexServerBindingSource.ResetBindings(true);
        }

        void ServerManager_OnServerManangerError(PlexException e) {
            MessageBox.Show(e.ToString());
        }

        void ConfigurationForm_Load(object sender, EventArgs e) {
            this.Text = String.Format("{0} - {1} - Configuration", Settings.PLUGIN_NAME, Settings.PLUGIN_VERSION);
            textBoxCheezRootFolder.Text = Settings.CacheFolder;
            checkBoxDeleteOnExit.Checked = Settings.DeleteCacheOnExit;
        }

        void ConfigurationForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e) {
            PlexInterface.ServerManager.SavePlexServers(PlexServers);
            Settings.CacheFolder = textBoxCheezRootFolder.Text;
            Settings.DeleteCacheOnExit = checkBoxDeleteOnExit.Checked;
            Settings.Save();
        }

        private void button1_Click(object sender, EventArgs e) {
            FolderBrowserDialog dlgFolderBrowser = new FolderBrowserDialog();
            dlgFolderBrowser.Description = "Local download path for cheezy pictures...";
            dlgFolderBrowser.SelectedPath = Settings.CacheFolder;
            if (dlgFolderBrowser.ShowDialog(this) == DialogResult.OK) {
                Settings.CacheFolder = dlgFolderBrowser.SelectedPath;
            } else {
                Log.Debug("No folder selected");
            }
        }

        private void buttonRefreshBonjourServers_Click(object sender, EventArgs e) {
            BonjourDiscovery.RefreshBonjourDiscovery();
            RefreshOnlineStatus();
        }

        private void RefreshOnlineStatus() {
            this.Invoke(new MethodInvoker(() => { this.UseWaitCursor = true; }));
            PlexServers.ForEach(svr => PlexInterface.Login(svr));
            this.Invoke(new MethodInvoker(() => { this.UseWaitCursor = false; }));
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (e.ColumnIndex == userPassDataGridViewTextBoxColumn.Index && e.Value != null) {
                e.Value = "[password set]";
            }
        }

        private void tabPage2_Enter(object sender, EventArgs e) {
            plexServerBindingSource.DataSource = PlexServers;
            RefreshOnlineStatus();
            BonjourDiscovery.StartBonjourDiscovery();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            try {
                if (e.ColumnIndex == userPassDataGridViewTextBoxColumn.Index) {
                    PlexServers[e.RowIndex].EncryptPassword(PlexServers[e.RowIndex].UserName, dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString());
                }

                if (e.ColumnIndex == hostAdressDataGridViewTextBoxColumn.Index || !PlexServers[e.RowIndex].IsBonjour && (e.ColumnIndex == userPassDataGridViewTextBoxColumn.Index || e.ColumnIndex == userNameDataGridViewTextBoxColumn.Index)) {
                    PlexInterface.Login(PlexServers[e.RowIndex]);
                }
            } catch { }
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) {
            if (e.RowIndex < PlexServers.Count)
                if (PlexServers[e.RowIndex].IsOnline) {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                } else {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Tomato;
                }
        }
    }
}
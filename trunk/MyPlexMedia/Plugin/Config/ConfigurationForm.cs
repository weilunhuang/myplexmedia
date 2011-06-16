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
                BonjourDiscovery.OnBonjourServer += new BonjourDiscovery.OnBonjourServerEventHandler(BonjourDiscovery_OnBonjourServer);
                PlexServers = PlexInterface.ServerManager.PlexServers;

            } catch (Exception ex) {
                Log.Error(ex);
            }
        }

        void BonjourDiscovery_OnBonjourServer(PlexServer bojourDiscoveredServer) {
            if (PlexServers.Contains<PlexServer>(bojourDiscoveredServer)) {
                PlexServers.Find(x => x.Equals(bojourDiscoveredServer)).IsBonjour = true;
            } else {
                PlexServers.Add(bojourDiscoveredServer);
            }
        }

        void ServerManager_OnServerManangerError(PlexException e) {
            MessageBox.Show(e.ToString());
        }

        void ConfigurationForm_Load(object sender, EventArgs e) {
            textBoxCheezRootFolder.Text = Settings.CheezRootFolder;
            numericUpDownFetchCount.Value = Settings.FetchCount;
            checkBoxDeleteOnExit.Checked = Settings.DeleteLocalCheezOnExit;
        }

        void ConfigurationForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e) {
            PlexInterface.ServerManager.SavePlexServers(PlexServers);

            Settings.CheezRootFolder = textBoxCheezRootFolder.Text;
            Settings.FetchCount = (int)numericUpDownFetchCount.Value;
            Settings.DeleteLocalCheezOnExit = checkBoxDeleteOnExit.Checked;
            Settings.Save();
        }

        private void button1_Click(object sender, EventArgs e) {
            FolderBrowserDialog dlgFolderBrowser = new FolderBrowserDialog();
            dlgFolderBrowser.Description = "Local download path for cheezy pictures...";
            dlgFolderBrowser.SelectedPath = Settings.CheezRootFolder;
            if (dlgFolderBrowser.ShowDialog(this) == DialogResult.OK) {
                Settings.CheezRootFolder = dlgFolderBrowser.SelectedPath;
            } else {
                Log.Debug("No folder selected");
            }
        }



        private void buttonRefreshBonjourServers_Click(object sender, EventArgs e) {
            System.Threading.Thread t1 = new System.Threading.Thread(delegate() {
                BonjourDiscovery.RefreshBonjourDiscovery();
            });
            t1.Start();

            RefreshOnlineStatus();
        }

        private void RefreshOnlineStatus() {
            this.Invoke(new MethodInvoker(() => { Application.UseWaitCursor = true; }));

            for (int i = 0; i < PlexServers.Count; i++) {
                if (PlexServers[i].Authenticate(ref _webClient)) {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                } else {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Tomato;
                }
            }
            this.Invoke(new MethodInvoker(() => { Application.UseWaitCursor = false; }));
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (e.ColumnIndex == userPassDataGridViewTextBoxColumn.Index && e.Value != null) {
                e.Value = "[password set]";
            }
        }

        private void tabPage2_Enter(object sender, EventArgs e) {
            plexServerBindingSource.DataSource = PlexServers;
        }
    

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex == userPassDataGridViewTextBoxColumn.Index) {
                PlexServers[e.RowIndex].EncryptPassword(PlexServers[e.RowIndex].UserName, dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString());
            }
            if (PlexServers[e.RowIndex].Authenticate(ref _webClient)) {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
            } else {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Tomato;
            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
            if (PlexServers[e.RowIndex].Authenticate(ref _webClient)) {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
            } else {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Tomato;
            }
        }        
    }
}
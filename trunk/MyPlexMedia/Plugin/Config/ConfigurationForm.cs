#region #region Copyright (C) 2005-2011 Team MediaPortal

// 
// Copyright (C) 2005-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.
// 

#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MediaPortal.GUI.Library;
using MediaPortal.UserInterface.Controls;
using PlexMediaCenter.Plex;
using PlexMediaCenter.Plex.Connection;
using PlexMediaCenter.Util;
using MyPlexMedia.Plugin.Window.Playback;
using MyPlexMedia.Plugin.Window.Dialogs;

namespace MyPlexMedia.Plugin.Config {
    public partial class ConfigurationForm : MPConfigForm {
        public ConfigurationForm() {
            try {
                InitializeComponent();
                Settings.Load();
                PlexInterface.OnPlexError += PlexInterface_OnPlexError;
                PlexInterface.Init(Settings.PLEX_SERVER_LIST_XML, Settings.PLEX_ARTWORK_CACHE_ROOT_PATH);
            } catch (Exception ex) {
                Log.Error(ex);
            }
        }

        void PlexInterface_OnPlexError(PlexException e) {
            MessageBox.Show(e.Message);
        }

        private void ServerManager_OnPlexServersChanged(List<PlexServer> updatedServerList) {
            Invoke(new MethodInvoker(() => {
                plexServerBindingSource.DataSource = updatedServerList;
                plexServerBindingSource.ResetBindings(true);
            }));
        }

        protected override void OnLoad(EventArgs e) {
            Text = String.Format("{0} - {1} - Configuration", Settings.PLUGIN_NAME, Settings.PLUGIN_VERSION);
            textBoxCacheFolder.Text = Settings.CacheFolder;
            checkBoxDeleteOnExit.Checked = Settings.DeleteCacheOnExit;
            comboBoxQualityLAN.DataSource = Enum.GetValues(typeof(PlexQualities));
            comboBoxQualityLAN.SelectedItem = Settings.DefaultQualityLAN;
            comboBoxQualityWAN.DataSource = Enum.GetValues(typeof(PlexQualities));
            comboBoxQualityWAN.SelectedItem = Settings.DefaultQualityWAN;
            checkBoxSelectQualityPriorToPlayback.Checked = Settings.SelectQualityPriorToPlayback;
            ServerManager.OnPlexServersChanged += ServerManager_OnPlexServersChanged;
            textBoxMyPlexPass.Text = Settings.MyPlexPass;
            textBoxMyPlexUser.Text = Settings.MyPlexUser;
            checkBoxDownloadArtwork.Checked = Settings.DownloadArtwork;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            Settings.CacheFolder = textBoxCacheFolder.Text;
            Settings.DeleteCacheOnExit = checkBoxDeleteOnExit.Checked;
            Settings.DefaultQualityLAN = (PlexQualities)comboBoxQualityLAN.SelectedValue;
            Settings.DefaultQualityWAN = (PlexQualities)comboBoxQualityWAN.SelectedValue;
            Settings.MyPlexPass = textBoxMyPlexPass.Text;
            Settings.MyPlexUser = textBoxMyPlexUser.Text;
            Settings.DownloadArtwork = checkBoxDownloadArtwork.Checked;
            Settings.Save();
        }

        protected override void OnShown(EventArgs e) {
            if (!String.IsNullOrEmpty(Settings.MyPlexUser) && !String.IsNullOrEmpty(Settings.MyPlexPass)) {
                buttonMyPlexLogin_Click(null, null);
            }
        }

        private void buttonRefreshBonjourServers_Click(object sender, EventArgs e) {
            PlexInterface.ServerManager.RefreshBonjourServers();
            RefreshOnlineStatus();
        }

        private void RefreshOnlineStatus() {
            //Invoke(new MethodInvoker(() => { UseWaitCursor = true; }));
            //PlexServers.ForEach(svr => PlexInterface.Login(svr));
            //Invoke(new MethodInvoker(() => { UseWaitCursor = false; }));
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            //try {
            //    if (e.ColumnIndex == userPassDataGridViewTextBoxColumn.Index) {
            //        if (string.IsNullOrEmpty(PlexServers[e.RowIndex].UserPass)) {
            //            e.Value = "[password set]";
            //        }
            //        if (PlexServers[e.RowIndex].IsMyPlex) {
            //            e.Value = "[token set]";
            //        }
            //    }
            //} catch {
            //}
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            //try {
            //    if (e.ColumnIndex == userPassDataGridViewTextBoxColumn.Index) {
            //        PlexServers[e.RowIndex].UserPass = PlexServer.EncryptPassword(PlexServers[e.RowIndex].UserName,
            //                                                dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString());
            //    }

            //    if (e.ColumnIndex == hostAdressDataGridViewTextBoxColumn.Index ||
            //        !PlexServers[e.RowIndex].IsBonjour &&
            //        (e.ColumnIndex == userPassDataGridViewTextBoxColumn.Index ||
            //         e.ColumnIndex == userNameDataGridViewTextBoxColumn.Index)) {
            //        PlexInterface.Login(PlexServers[e.RowIndex]);
            //    }
            //} catch (Exception) {
            //}
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) {
            try {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = ((List<PlexServer>)plexServerBindingSource.DataSource)[e.RowIndex].IsOnline
                                                                                    ? Color.LightGreen
                                                                                    : Color.Tomato;
            } catch { }
        }

        private void buttonMyPlexLogin_Click(object sender, EventArgs e) {
            buttonMyPlexLogin.BackColor =
                PlexInterface.MyPlexLogin(textBoxMyPlexUser.Text, textBoxMyPlexPass.Text)
                                 ? Color.LightGreen
                                 : Color.Tomato;
        }

        private void textBoxMyPlexPass_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)13) {
                buttonMyPlexLogin_Click(sender, e);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {

        }

        private void tabPage2_Enter(object sender, EventArgs e) {
            PlexInterface.ServerManager.RefreshBonjourServers();
        }

        private void buttonManageManualConnections_Click(object sender, EventArgs e) {
            List<ManualConnectionInfo> currentManualConnections;
            try {
                currentManualConnections = PlexInterface.ServerManager.PlexServers.Select(svr => svr.KnownConnections[typeof(ManualConnectionInfo)] as ManualConnectionInfo).ToList();
            } catch {
                currentManualConnections = new List<ManualConnectionInfo>();
            }
            if (DialogResult.OK == new DialogManageManualConnections(ref currentManualConnections).ShowDialog()) {
                currentManualConnections.ForEach(mc => PlexInterface.ServerManager.TryAddManualServerConnection(mc));
            }
        }
    }
}
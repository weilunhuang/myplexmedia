using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlexMediaCenter.Plex.Connection;
using System.Net;

namespace MyPlexMedia.Plugin.Config {
    public partial class DialogManageManualConnections : Form {

        private List<ManualConnectionInfo> ManualConnections { get;  set; }

        public DialogManageManualConnections(ref List<ManualConnectionInfo> knownConnections) {
            InitializeComponent();
            manualConnectionInfoBindingSource.DataSource = ManualConnections = knownConnections;
        }

        private void toolStripButtonCheck_Click(object sender, EventArgs e) {
            WebProxy myProxy = new WebProxy();
            myProxy.Address = new Uri("http://194.114.63.23:8080");
            myProxy.Credentials = new NetworkCredential("v7708m3", "mandela");
            WebClient wc = new WebClient();
            wc.Proxy = myProxy;
            ManualConnections.ForEach(mc => mc.TryConnect(ref wc));
            wc.Dispose();
        }

        private void toolStripButtonAccept_Click(object sender, EventArgs e) {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void manualConnectionInfoDataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) {
            try {
                manualConnectionInfoDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = ManualConnections[e.RowIndex].IsOnline
                                                                                    ? Color.LightGreen
                                                                                    : Color.Tomato;
            } catch { }
        }

        private void manualConnectionInfoDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            try {
                if (e.ColumnIndex == columnPassword.Index) {
                    ManualConnections[e.RowIndex].UserPass = ManualConnectionInfo.EncryptPassword(ManualConnections[e.RowIndex].UserName,
                                                            manualConnectionInfoDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString());

                }
            } catch { }
        }
    }
}

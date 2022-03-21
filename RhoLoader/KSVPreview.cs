using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KartRider.Record;

namespace RhoLoader
{
    public partial class KSVPreview : Form
    {
        public KSVPreview(byte[] Data)
        {
            InitializeComponent();
            ReadKSV(Data);
        }

        public void ReadKSV(byte[] data)
        {
            KSVInfo ksvinfo = KartRecord.ReadKSVFromBytes(data);
            this.contestName.Text = ksvinfo.RecordTitle;
            infoBox.Items.Add(new ListViewItem(new string[] { "Contest Type", ksvinfo.ContestType.ToString() }));
            infoBox.Items.Add(new ListViewItem(new string[] { "Map Name", ksvinfo.MapName.ToString() }));
            infoBox.Items.Add(new ListViewItem(new string[] { "Description", ksvinfo.Description }));
            infoBox.Items.Add(new ListViewItem(new string[] { "Record Time", ksvinfo.RecordTime.ToString("yyyy/MM/dd HH:mm") }));
            foreach (PlayerInfo pi in ksvinfo.Players)
            {
                ListViewItem lvi = new ListViewItem(pi.PlayerName);
                int team = pi.Equipment.Equ5 >> 8;
                if (team == 1)
                    lvi.BackColor = Color.OrangeRed;
                else if(team == 2)
                    lvi.BackColor = Color.SkyBlue;
                players.Items.Add(lvi);
            }
        }

        private void players_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                e.Item.Selected = false;
                e.Item.Focused = false;
            }
        }

        private void players_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void KSVPreview_Load(object sender, EventArgs e)
        {

        }
    }
}

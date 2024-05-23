using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KartLibrary;
using KartLibrary.Record;
using KartLibrary.Xml;
using RhoLoader.Setting;

namespace RhoLoader.PreviewWindow
{
    public partial class KSVPreview : Form
    {
        BinaryXmlTag LocaleData;
        public KSVPreview(byte[] Data, BinaryXmlTag LocaleData = null)
        {
            InitializeComponent();
            this.LocaleData = LocaleData;
            ReadKSV(Data);
        }

        public void ReadKSV(byte[] data)
        {
            KSVInfo ksvinfo = KartRecord.ReadKSVFromBytes(data);
            this.contestName.Text = ksvinfo.RecordTitle;
            infoBox.Items.Add(new ListViewItem(new string[] { "ksv_Contesttype".GetStringBag(),
                ksvinfo.ContestType switch
                {
                    ContestType.InvalidGameType => "Invalid",
                    ContestType.SpeedIndividual => "ksv_SpeedIndividual".GetStringBag(),
                    ContestType.SpeedTeam => "ksv_SpeedTeam".GetStringBag(),
                    ContestType.ItemIndividual => "ksv_ItemIndividual".GetStringBag(),
                    ContestType.ItemTeam => "ksv_ItemTeam".GetStringBag(),
                    _ => ksvinfo.ContestType.ToString()
                }}));
            infoBox.Items.Add(new ListViewItem(new string[] { "ksv_RecordingDate".GetStringBag(), $"{ksvinfo.RecordingDate:yyyy/MM/dd HH:mm:ss}"}));
            infoBox.Items.Add(new ListViewItem(new string[] { "ksv_Region".GetStringBag(), ksvinfo.RegionCode.ToString()}));
            infoBox.Items.Add(new ListViewItem(new string[] { "ksv_TrackName".GetStringBag(), ReadTrackName(ksvinfo.TrackName) }));
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

        private string ReadTrackName(string track_name)
        {
            if (LocaleData is null)
                return track_name;
            BinaryXmlTag? found_tag = LocaleData.Children.Where(x => x.GetAttribute("id") == track_name).First();
            if(found_tag is not null)
                return found_tag.GetAttribute("name");
            else
                return track_name;
        }
    }
}

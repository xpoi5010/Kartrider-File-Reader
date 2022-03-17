using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace RhoLoader
{
    public partial class AboutMe : Form
    {
        CheckResult result = CheckResult.Checking;

        string UpdateFile = "";
        string LastVersion = "";
        private CheckResult Result
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
                ChangeUpdateStatus();
            }
        }

        public AboutMe()
        {
            InitializeComponent();
            Version ver = Assembly.GetExecutingAssembly().GetName().Version;
            this.version.Text = $"Version : {(ver.Revision == 0?"":ver.Revision == 1?"Beta ":ver.Revision == 2?"Dev ":"Custom ")}{ver.Major}.{ver.Minor}.{ver.Build}";
            StartCheckUpdate();
        }


        private enum CheckResult
        {
            UpToDate,NewVersionReleased,NoNetworkConnection,UnknownError,Checking
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If this program is running incorrectly,\r\nYou can report to RhoReader github page.\r\nThank you for reporting!");
        }

        public void StartCheckUpdate()
        {
            Task task = new Task(CheckUpdate_BGFunc);
            task.Start();
        }

        private void CheckUpdate_BGFunc()
        {
            try
            {
                Result = CheckResult.Checking;
                WebClient wc = new WebClient();
                string updateInfo = wc.DownloadString(@"https://raw.githubusercontent.com/xpoi5010/Kartrider-File-Reader/main/VersionInfo.json");
                dynamic info = JsonConvert.DeserializeObject(updateInfo);
                LastVersion = info.LatestVersion;
                Version ver = Assembly.GetExecutingAssembly().GetName().Version;
                if (LastVersion != $"{ver.Major}.{ver.Minor}.{ver.Build}")
                {
                    Result = CheckResult.NewVersionReleased;
                    UpdateFile = info.DownloadLink;
                }
                else
                    Result = CheckResult.UpToDate;

            }
            catch(Exception ex)
            {
                Debug.Print($"Function:{nameof(CheckUpdate_BGFunc)} throws a excetion: {ex.Message}");
            }
        }

        private void ChangeUpdateStatus()
        {
            if (this.InvokeRequired)
            {
                Action dele = new Action(ChangeUpdateStatus);
                this.Invoke(dele);
            }
            else
            {
                switch (result)
                {
                    case CheckResult.UpToDate:
                        checkUpdateStatus.Text = "Up to date";
                        checkUpdateStatus.ForeColor = Color.BlueViolet;
                        break;
                    case CheckResult.NewVersionReleased:
                        checkUpdateStatus.Text = "New version released, Clicking for downloading.";
                        checkUpdateStatus.ForeColor = Color.DodgerBlue;
                        break;
                    case CheckResult.NoNetworkConnection:
                        checkUpdateStatus.Text = "No network connection. Please check if your computer has connected to the internet.";
                        checkUpdateStatus.ForeColor = Color.Red;
                        break;
                    case CheckResult.UnknownError:
                        checkUpdateStatus.Text = "Unknown Error! Please try again.";
                        checkUpdateStatus.ForeColor = Color.Red;
                        break;
                    case CheckResult.Checking:
                        checkUpdateStatus.Text = "Checking the update";
                        checkUpdateStatus.ForeColor = Color.Gray;
                        break;
                }
            }
        }

        private void checkUpdateStatus_Click(object sender, EventArgs e)
        {
            if(Result == CheckResult.NewVersionReleased)
            {
                if(MessageBox.Show(string.Format("msg_newVersionReleased".GetStringBag(),LastVersion), "Rho Loader",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Process.Start(UpdateFile);
                }
            }
            else if(Result != CheckResult.Checking)
            {
                StartCheckUpdate();
            }
        }
    }
}

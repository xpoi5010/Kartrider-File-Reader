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
using RhoLoader.Update;

namespace RhoLoader
{
    public partial class AboutMe : Form
    {
        private string UpdateFile = "";
        private string LastVersion = "";
        private Task bg_work;
        private UpdateInfo updateInfo;
        private CheckResult Result = CheckResult.Checking;

        public AboutMe()
        {
            InitializeComponent();
            Version ver = Assembly.GetExecutingAssembly().GetName().Version;
            this.version.Text = $"Version : {(ver.Revision == 0 ? "" : ver.Revision == 1 ? "Beta " : ver.Revision == 2 ? "Dev " : ver.Revision == 3 ? "Unstable " : "Custom ")}{ver.Major}.{ver.Minor}.{ver.Build}";
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

        private void StartCheckUpdate()
        {
            bg_work = new Task(CheckUpdate_BGFunc);
            bg_work.Start();
        }

        private void CheckUpdate_BGFunc()
        {
            try
            {
                ChangeUpdateStatus(CheckResult.Checking);
                UpdateInfo updateInfo;
                try
                {
                    updateInfo = UpdateManager.GetUpdateInfo();
                }
                catch (Exception ex)
                {
                    switch (ex)
                    {
                        case WebException webException:
                            ChangeUpdateStatus(CheckResult.NoNetworkConnection);
                            break;
                        default:
                            ChangeUpdateStatus(CheckResult.UnknownError);
                            break;
                    }
                    return;
                }
                if (updateInfo.Version != UpdateManager.GetCurrentVersion())
                {
                    ChangeUpdateStatus(CheckResult.NewVersionReleased);
                    UpdateFile = updateInfo.DownloadLink;
                }
                else
                    ChangeUpdateStatus(CheckResult.UpToDate);

            }
            catch(Exception ex)
            {

                Debug.Print($"Function:{nameof(CheckUpdate_BGFunc)} throws a excetion: {ex.Message}\nStack: {ex.StackTrace}");
            }
        }

        private void ChangeUpdateStatus(CheckResult status, int _invoke_recursion_counter = 0)
        {
            if (this.InvokeRequired)
            {
                Action<CheckResult, int> dele = new Action<CheckResult, int>(ChangeUpdateStatus);
                this.Invoke(dele, status, _invoke_recursion_counter + 1);
            }
            else
            {
                try
                {
                    Result = status;
                    switch (Result)
                    {
                        case CheckResult.UpToDate:
                            checkUpdateStatus.Text = "about_UpToDate".GetStringBag();
                            checkUpdateStatus.ForeColor = Color.BlueViolet;
                            break;
                        case CheckResult.NewVersionReleased:
                            checkUpdateStatus.Text = "about_NewVersion".GetStringBag();
                            checkUpdateStatus.ForeColor = Color.DodgerBlue;
                            break;
                        case CheckResult.NoNetworkConnection:
                            checkUpdateStatus.Text = "about_NoInternet".GetStringBag();
                            checkUpdateStatus.ForeColor = Color.Red;
                            break;
                        case CheckResult.UnknownError:
                            checkUpdateStatus.Text = "about_UnknowError".GetStringBag();
                            checkUpdateStatus.ForeColor = Color.Red;
                            break;
                        case CheckResult.Checking:
                            checkUpdateStatus.Text = "about_Checking".GetStringBag();
                            checkUpdateStatus.ForeColor = Color.Gray;
                            break;
                    }
                }
                catch(Exception ex)
                {
                    // Ensure this function will running at the thread that process window events.
                    if (_invoke_recursion_counter >= 5)
                        throw ex;
                    Action<CheckResult, int> dele = new Action<CheckResult, int>(ChangeUpdateStatus);
                    this.Invoke(dele, status, _invoke_recursion_counter + 1);
                }
                
            }
        }

        private void checkUpdateStatus_Click(object sender, EventArgs e)
        {
            if(Result == CheckResult.NewVersionReleased)
            {
                if(MessageBox.Show(string.Format("msg_newVersionReleased".GetStringBag(),LastVersion), "Rho Loader",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (updateInfo is null)
                    {
                        updateInfo = UpdateManager.GetUpdateInfo();
                    }
                    Process proc = new Process();
                    proc.StartInfo.FileName = "explorer.exe";
                    proc.StartInfo.Arguments = updateInfo.DownloadLink;
                    proc.Start();
                }
            }
            else if(Result != CheckResult.Checking)
            {
                StartCheckUpdate();
            }
        }
    }
}

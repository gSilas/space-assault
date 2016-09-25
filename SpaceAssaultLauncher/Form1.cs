using System.Diagnostics;
using System.Net;

namespace SpaceAssaultLauncher
{
    public partial class FormLauncher : System.Windows.Forms.Form
    {
        public FormLauncher()
        {
            InitializeComponent();

            using (var client = new WebClient())
            {
                client.Headers.Add("user-agent", "Anything");
                client.DownloadFile(
                    "https://raw.githubusercontent.com/gSilas/space-assault-build/master/version",
                    "version");
            }
        }

        private void Run_Click(object sender, System.EventArgs e)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.Arguments = string.Empty;
            start.FileName = "data/SpaceAssault.exe";
            start.WindowStyle = ProcessWindowStyle.Normal;
            start.CreateNoWindow = false;
       
            Process.Start(start);

            this.Close();
        }

        private void Update_Click(object sender, System.EventArgs e)
        {

        }

        private void Update_Available(object sender, System.EventArgs e)
        {
            
        }

    }
}

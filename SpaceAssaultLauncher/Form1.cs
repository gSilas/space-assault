using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace SpaceAssaultLauncher
{
    public partial class FormLauncher : System.Windows.Forms.Form
    {
        bool updateReady;
        public FormLauncher()
        {
            InitializeComponent();
            this.label1.ForeColor = System.Drawing.Color.Green;

            string old_contents = File.ReadAllText("version");

            using (var client = new WebClient())
            {
                client.Headers.Add("user-agent", "Anything");
                client.DownloadFile(
                    "https://raw.githubusercontent.com/gSilas/space-assault-build/master/version",
                    "remoteversion");
            }

            string new_contents = File.ReadAllText("remoteversion");

            updateReady = !string.Equals(old_contents, new_contents);
            if (updateReady)
            {
                this.label1.Text = "Update available";
                this.label1.ForeColor = System.Drawing.Color.Red;
                this.runButton.Visible = false;
            }

        }

        private void Run_Click(object sender, System.EventArgs e)
        {
            Directory.SetCurrentDirectory("data");

            ProcessStartInfo start = new ProcessStartInfo();
            start.Arguments = string.Empty;
            start.FileName = "SpaceAssault.exe";
            start.WindowStyle = ProcessWindowStyle.Normal;
            start.CreateNoWindow = false;
       
            Process.Start(start);

            this.Close();
        }

        private void Update_Click(object sender, System.EventArgs e)
        {
            if (updateReady)
            {
                this.label1.Text = "Downloading!";
                this.label1.ForeColor = System.Drawing.Color.Red;
                updateReady = false;

                try { Directory.Delete("data_old", true); }
                catch (DirectoryNotFoundException ex) { }

                try
                {
                    Directory.Move(@"data", @"data_old");
                }
                catch (DirectoryNotFoundException ex) { }

                using (var client = new WebClient())
                {
                    client.Headers.Add("user-agent", "Anything");
                    client.DownloadFile(
                        "https://api.github.com/repos/gSilas/space-assault-build/zipball",
                        "tmp.zip");
                }

                //Decompression
                System.IO.Compression.ZipFile.ExtractToDirectory("tmp.zip", "temp");

                string path = string.Empty;

                var gitDir = Directory.GetDirectories("temp");
                var p = Directory.GetDirectories(gitDir[0]);
                path = p[0];

                Directory.Move(path, "data");   

                //Removing archive + versiontext
                Directory.Delete("temp",true);
                File.Delete("tmp.zip");
                File.Delete("oldversion");

                //adding current version
                File.Replace("remoteversion", "version", "oldversion");
                File.Delete("remoteversion");

                //text update + start button
                this.label1.Text = "Done! Game updated!";
                this.label1.ForeColor = System.Drawing.Color.Green;
                this.runButton.Visible = true;
            }
        }
    }
}

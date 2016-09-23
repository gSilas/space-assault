using System.Diagnostics;

namespace SpaceAssaultLauncher
{
    public partial class FormLauncher : System.Windows.Forms.Form
    {
        public FormLauncher()
        {
            InitializeComponent();
        }

        private void Run_Click(object sender, System.EventArgs e)
        {
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

        }

    }
}

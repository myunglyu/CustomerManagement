using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace WooriOptical.Desktop
{
    public partial class MainForm : Form
    {
        private Process backendProcess;
        private WebView2 webView;

        public MainForm()
        {
            InitializeComponent();

            // Set form size to 80% of screen and center
            var screen = Screen.PrimaryScreen.WorkingArea;
            this.Width = (int)(screen.Width * 0.6);
            this.Height = (int)(screen.Height * 0.8);
            this.StartPosition = FormStartPosition.CenterScreen;

            StartBackend();
            webView = new WebView2
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(webView);
            webView.Source = new Uri("http://localhost:5000");
        }

        private void StartBackend()
        {
            backendProcess = new Process();
            backendProcess.StartInfo.FileName = "WooriOptical.exe"; // Path to your published backend
            backendProcess.StartInfo.CreateNoWindow = true;
            backendProcess.StartInfo.UseShellExecute = false;
            backendProcess.Start();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            backendProcess?.Kill();
            base.OnFormClosed(e);
        }
    }
}

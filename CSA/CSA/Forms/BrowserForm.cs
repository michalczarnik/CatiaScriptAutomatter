using System;
using System.IO;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using static CSA.Constants;

namespace CSA.Forms
{
    public partial class BrowserForm : Form
    {
        private ChromiumWebBrowser _browser;

        public BrowserForm()
        {
            InitializeComponent();
            InitializeBrowser();
            _browser.RegisterJsObject("exposedClass", new ExposedClass(_browser, this));
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(this.FormClicked);

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.ChangeSize(ScreenSize.M);
            this.Text = "Catia Script Automater";
            
        }

        private delegate void ChangeSizeDelegate(ScreenSize size);

        public void ChangeSize(ScreenSize size)
        {
            if (this.InvokeRequired)
            {
                // This is a worker thread so delegate the task.
                this.Invoke(new ChangeSizeDelegate(this.ChangeSize), size);
            }
            else
            {
                // This is the UI thread so perform the task.
                var height = 0;
                var width = 0;
                switch (size)
                {
                    case ScreenSize.L:
                        height = Screen.PrimaryScreen.WorkingArea.Height;
                        width = Screen.PrimaryScreen.WorkingArea.Width;
                        break;
                    case ScreenSize.M:
                        height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * 0.75);
                        width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.75);
                        break;
                    case ScreenSize.S:
                        height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * 0.5);
                        width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.5);
                        break;
                }
                this.Height = height;
                this.Width = width;
            }
        }

        private void FormClicked(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }


        private void InitializeBrowser()
        {
            Cef.EnableHighDPISupport();
            var settings = new CefSettings()
            {
                BrowserSubprocessPath = @"x86\CefSharp.BrowserSubprocess.exe"
                
            };

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            var path = string.Format(@"{0}\html-resources\html\index.html", Application.StartupPath);

            if (!File.Exists(path))
            {
                MessageBox.Show(string.Format(@"Error! The layout file does not exist...\nFile path provided: {0}", path));
            }
            
            _browser = new ChromiumWebBrowser(path);
            this.Controls.Add(_browser);
            _browser.Dock = DockStyle.Fill;

            var browserSettings = new BrowserSettings();
            browserSettings.FileAccessFromFileUrls = CefState.Enabled;
            browserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            _browser.BrowserSettings = browserSettings;

            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
        }
    }
}

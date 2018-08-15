using CSA.Forms;
using System.Threading;
using System.Windows.Forms;
using static CSA.Constants;

namespace CSA.Services.Static
{
    static class WinFormsService
    {
        private static string _pathToDirectory;
        public static string PathToDirectory
        {
            get
            {
                return _pathToDirectory;
            }
            private set
            {
                _pathToDirectory = value;
            }
        }

        public static void OpenFolderDialog()
        {
            using(var fbd = new FolderBrowserDialog())
            {
                var thread = new Thread((ThreadStart)(() =>
                {
                    var result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        _pathToDirectory = fbd.SelectedPath;
                    }
                }));

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }
            
        }

        public static string OpenFileDialog()
        {
            var path = string.Empty;
            using(var ofd = new OpenFileDialog())
            {
                var thread = new Thread((ThreadStart)(() =>
                {
                    ofd.Filter = "All Files (*.*)|*.*|Catia Part (*.CatPart)|*.CATPart";
                    ofd.FilterIndex = 2;
                    ofd.Multiselect = false;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        path = ofd.FileName;
                    }
                }));

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }
            return path;
        }

        public static void ChangeSize(ScreenSize size)
        {
            var form = Application.OpenForms[0];
            var browserForm = (BrowserForm) form;
            browserForm.ChangeSize(size);
        }

    }
}

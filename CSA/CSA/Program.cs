using System;
using System.Windows.Forms;
using CSA.Configuration;
using CSA.Forms;

namespace CSA
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            AutoMapperProfile.InitializeAutomapper();
            Application.Run(new BrowserForm());
        }
    }
}
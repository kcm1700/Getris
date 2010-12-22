using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;


namespace getris.Core
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static MainDlg myDlg;
        static Keyboard keyboard;

        static Program()
        {
        }

        [STAThread]
        static void Main()
        {
            keyboard = KeyboardSingleton.Instance;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            myDlg = new MainDlg();
            Application.Run(myDlg);
        }

    }
}

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

        static Program()
        {
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            myDlg = new MainDlg();
            // add idlegameloop
            Application.Idle += new EventHandler(myDlg.IdleGameLoop);
            Application.Run(myDlg);
        }

    }
}

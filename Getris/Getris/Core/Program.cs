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
            /*
            int size = GameState.BlockList.Instance.Size;
            for (int i = 0; i < size; i++)
            {
                int a = GameState.BlockList.Instance.Get(i);
                GameState.BlockList.Instance.Set(i,a);
                int b = GameState.BlockList.Instance.Get(i);
                if (a == b)
                    Console.WriteLine(0);
                else
                    Console.WriteLine(1);
            }
            */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            myDlg = new MainDlg();
            Application.Run(myDlg);
        }

    }
}

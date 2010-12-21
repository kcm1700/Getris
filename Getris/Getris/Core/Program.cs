using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;


namespace getris
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private enum GameMode
        {
            CreatingGL,
            Game
        }

        static GameMode gameMode;
        static MainDlg myDlg;
        static DateTime lastTime;
        const int MaxFrameRate = 30;

        static Program()
        {
            gameMode = GameMode.CreatingGL;
            lastTime = DateTime.Now;
        }



        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Idle += new EventHandler(IdleGameLoop);
            myDlg = new MainDlg();
            Application.Run(myDlg);
        }

        // an efficient game loop
        static void IdleGameLoop(object sender, EventArgs e)
        {
            while (IsIdle)
            {
                /* calculate elapsed time in seconds */
                DateTime now = DateTime.Now;
                double timeDelta = now.Subtract(lastTime).TotalSeconds;
                if (timeDelta >= 1.0 / MaxFrameRate)
                {
                    lastTime = now;
                    /* Update & Render */
                    TransitGameMode();
                    Update(timeDelta);
                    Render();
                }
                Thread.Sleep(1);
            }
        }

        static void TransitGameMode()
        {
            if (gameMode == GameMode.CreatingGL)
            {
                if (myDlg.isGLReady)// when it's ready to use GL
                {   //Enter Game mode
                    gameMode = GameMode.Game;
                }
            }
        }

        static void Update(double timeDelta)
        {
            switch (gameMode)
            {
                case GameMode.CreatingGL:
                    //do nothing here
                    break;
                case GameMode.Game:
                    //TODO: update game state based on input queue
                    break;
            }
        }

        static void Render()
        {
            switch (gameMode)
            {
                case GameMode.CreatingGL:
                    //do nothing here
                    break;
                case GameMode.Game:
                    //TODO: render game state
                    break;
            }
        }

        static bool IsIdle
        {
            get
            {
                NativeMessage msg;
                return ! PeekMessage(out msg, myDlg.Handle, 0, 0, 0);
            }
        }

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PeekMessage(out NativeMessage lpMsg, IntPtr hWnd, uint wMsgFilterMin,
           uint wMsgFilterMax, uint wRemoveMsg);

        [StructLayout(LayoutKind.Sequential)]
        private struct NativeMessage
        {
            public IntPtr handle;
            public uint msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public System.Drawing.Point p;
        }

    }
}

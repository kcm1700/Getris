using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

using getris.GameState;

namespace getris
{
    public partial class MainDlg : Form
    {
        private enum GameMode
        {
            CreatingGL = 0,
            GameMenu = 1,
            Game = 2
        }

        private GameMode gameMode;
        private bool glLoad;
        private const int MaxFrameRate = 60;
        private Stopwatch sw;

        private const int LeftGameWidth = 200;
        private const int LeftGameHeight = 420;
        private const int LeftGameLeft = 20;
        private const int LeftGameBottom = 80;
        private static readonly Rectangle LeftGameNext1 = new Rectangle(225, 380, 20 * 3, 20 * 3);
        private static readonly Rectangle LeftGameNext2 = new Rectangle(225, 280, 20 * 3, 20 * 3);

        private const int RightGameWidth = 200;
        private const int RightGameHeight = 420;
        private const int RightGameLeft = 20+200+20+100+20;
        private const int RightGameBottom = 80;
        private static readonly Rectangle RightGameNext1 = new Rectangle(295, 380, 20 * 3, 20 * 3);
        private static readonly Rectangle RightGameNext2 = new Rectangle(295, 280, 20 * 3, 20 * 3);

        private const string blockimagefilename = "block1.bmp";
        private int TN_BLOCK;

        private GameState.Battle battle;

        public MainDlg()
        {
            battle = new Battle();
            sw = new Stopwatch();
            sw.Start();
            glLoad = false;
            gameMode = GameMode.CreatingGL;
            InitializeComponent();
        }

        public bool isGLReady
        {
            get
            {
                return glLoad;
            }
        }


        private void glMain_Load(object sender, EventArgs e)
        {
            glLoad = true;
            Application.Idle += new EventHandler(IdleGameLoop);
            Core.Keyboard.Start();

            //Load Textures
            try
            {
                TN_BLOCK = Core.GraphicsUtil.LoadTexture(blockimagefilename);
            }
            catch
            {
            }
        }

        // an efficient game loop
        public void IdleGameLoop(object sender, EventArgs e)
        {
            while (glMain.IsIdle)
            {
                /* calculate elapsed time in seconds */
                double timeDelta = sw.Elapsed.TotalSeconds;
                if (timeDelta >= 1.0 / MaxFrameRate)
                {
                    sw.Restart();
                    Accumulate(timeDelta);
                    /* Update & Render */
                    TransitGameMode();
                    Update(timeDelta);
                    Render(timeDelta);
                }
                Thread.Sleep(1);
            }
        }

        double elapsedTime = 0;
        int frameCounter = 0;
        private void Accumulate(double timeDelta)
        {
            frameCounter++;
            elapsedTime += timeDelta;
            if (elapsedTime > 1)
            {
                lblFPS.Text = String.Format("{0:N} FPS", frameCounter / elapsedTime);
                elapsedTime -= 1;
                frameCounter = 0;
            }
        }

        void TransitGameMode()
        {
            if (gameMode == GameMode.CreatingGL)
            {
                if (isGLReady)// when it's ready to use GL
                {   //Enter Game mode
                    gameMode = GameMode.GameMenu;
                }
            }
            else if(gameMode == GameMode.GameMenu){
            }
            else if (gameMode == GameMode.Game)
            {
                //Nothing to do
            }
        }

        void Update(double timeDelta)
        {
            //TODO: is there something to do with it?
        }

        private void glMain_Enter(object sender, EventArgs e)
        {
            Core.Keyboard.IsGame = true;
        }

        private void glMain_Leave(object sender, EventArgs e)
        {
            Core.Keyboard.IsGame = false;
        }

        private void glMain_KeyDown(object sender, KeyEventArgs e)
        {
            Core.Keyboard.KeyDown(e.KeyCode);
        }

        private void glMain_KeyUp(object sender, KeyEventArgs e)
        {
            Core.Keyboard.KeyUp(e.KeyCode);
        }

        private void MainDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Core.Keyboard.keyboardThread.Abort();
            battle.LeftThread.Abort();
            battle.RightThread.Abort();
        }

        private void MainDlg_Load(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                control.PreviewKeyDown += new PreviewKeyDownEventHandler(control_PreviewKeyDown);
            }
        }

        private void control_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }

        private void MainDlg_Deactivate(object sender, EventArgs e)
        {
//            Core.Keyboard.KeyReset();
        }
    }
}

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
            GameSelect = 2,
            Game = 3,
            GameOver = 4,
            Exit = 99
        }

        private GameMode gameMode;
        private GameMode nextGameMode;
        private bool glLoad;
        public static int MaxFrameRate = 60;
        private Stopwatch sw;

        private const string blockimagefilename = "block1.bmp";
        private int TN_BLOCK;
        private const string numberimagefilename = "numbers.bmp";
        private int TN_NUMBERS;
        private static readonly string[] menuImageFileName = { "menu1.bmp", "menu2.bmp", "menu3.bmp", "menu4.bmp", "menu5.bmp"};
        private int[] TN_MENU;
        private const string chainfilename = "chain.bmp";
        private int TN_CHAIN;
        private const int menuCnt = 5;


        private GameState.Battle battle;

        public MainDlg()
        {
            sw = new Stopwatch();
            sw.Start();
            glLoad = false;
            gameMode = GameMode.CreatingGL;
            nextGameMode = GameMode.CreatingGL;

            menuPositionAngle = new double[menuCnt];
            TN_MENU = new int[menuCnt];

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
            try
            {
                TN_NUMBERS = Core.GraphicsUtil.LoadTexture(numberimagefilename);
            }
            catch
            {
            }
            for (int i = 0; i < menuPositionAngle.Length; i++)
            {
                menuPositionAngle[i] = OpenTK.MathHelper.TwoPi * (-i) / menuPositionAngle.Length;
                try
                {
                    TN_MENU[i] = Core.GraphicsUtil.LoadTexture(menuImageFileName[i]);
                }
                catch
                {
                }
            }
            try
            {
                TN_CHAIN = Core.GraphicsUtil.LoadTexture(chainfilename);
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
                if (gameMode == GameMode.GameOver)
                {
                    try
                    {
                        battle.LeftThread.Abort();
                    }
                    catch
                    {
                    }
                    try
                    {
                        battle.RightThread.Abort();
                    }
                    catch
                    {
                    }
                }
                if (gameMode == GameMode.Exit)
                {
                    this.Close();
                    return;
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
                Core.Keyboard.InputMode = Core.Keyboard.InputModes.Menu;
                gameMode = nextGameMode;
            }
            else if (gameMode == GameMode.Game)
            {
                Core.Keyboard.InputMode = Core.Keyboard.InputModes.Game;
                gameMode = nextGameMode;
                //Nothing to do
            }
            else if (gameMode == GameMode.GameOver)
            {
                gameMode = nextGameMode;
            }
        }

        void Update(double timeDelta)
        {
            if (gameMode == GameMode.GameMenu)
            {
                UpdateMenu(timeDelta);
            }
            if (gameMode == GameMode.Game)
            {
                nextGameMode = gameMode;
                if (battle.Finished)
                {
                    nextGameMode = GameMode.GameOver;
                    ResetAccum(true);
                    ResetAccum(false);
                }
            }
            if (gameMode == GameMode.GameOver)
            {
                nextGameMode = GameMode.GameMenu;
            }
        }

        private void Render(double timeDelta)
        {
            if (gameMode == GameMode.CreatingGL)
            {
                return;
            }
            else if (gameMode == GameMode.GameMenu)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.AccumBufferBit | ClearBufferMask.StencilBufferBit);
                MenuRender(timeDelta);
                glMain.SwapBuffers();
            }
            else if (gameMode == GameMode.Game)
            {
                GL.Disable(EnableCap.DepthTest);
                GL.Disable(EnableCap.AlphaTest);
                GL.Disable(EnableCap.Blend);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.AccumBufferBit | ClearBufferMask.StencilBufferBit);
                RenderBackground(timeDelta);

                //lock for the left
                bool [] users = {true, false};
                foreach (bool user in users)
                {
                    battle.MonEnter(user);
                    if (battle.isAnimationMode(user))
                    {
                        RenderUserAnimation(user, timeDelta);
                    }
                    else
                    {
                        RenderUserGame(user, timeDelta);
                        ResetAccum(user);// reset time accumulator
                    }
                    //draw next blocks
                    SetupNextRender(user, 1);
                    RenderNextBlock(user, 0);
                    SetupNextRender(user, 2);
                    RenderNextBlock(user, 1);
                    ScoreRender(user);

                    battle.MonExit(user);

                }
                glMain.SwapBuffers();
            }
            else if (gameMode == GameMode.GameOver)
            {
                GL.Disable(EnableCap.DepthTest);
                GL.Disable(EnableCap.AlphaTest);
                GL.Disable(EnableCap.Blend);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.AccumBufferBit | ClearBufferMask.StencilBufferBit);
                RenderBackground(timeDelta);

                //lock for the left
                bool []users = { true, false };
                foreach (bool user in users)
                {
                    battle.MonEnter(user);
                    RenderGameOver(user, timeDelta);
                    //draw next blocks
                    SetupNextRender(user, 1);
                    SetupNextRender(user, 2);

                    battle.MonExit(user);
                }
                glMain.SwapBuffers();
                if (accumLeft > 2)
                {
                    Core.Keyboard.Instance.ClearBuffer();
                    nextGameMode = GameMode.GameMenu;
                }
                else
                {
                    nextGameMode = gameMode;
                }
            }
        }


        private void glMain_Enter(object sender, EventArgs e)
        {
            Core.Keyboard.Enabled = true;
        }

        private void glMain_Leave(object sender, EventArgs e)
        {
            Core.Keyboard.Enabled = false;
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
            if (battle != null)
            {
                battle.LeftThread.Abort();
                battle.RightThread.Abort();
            }
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

        private void btnColor_Click(object sender, EventArgs e)
        {
            UI.PreferencesDlg a = new UI.PreferencesDlg();
            a.Show();
        }
    }
}

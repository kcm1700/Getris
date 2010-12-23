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
using getris.Animation;

namespace getris
{
    public partial class MainDlg : Form
    {
        private enum GameMode
        {
            CreatingGL = 0,
            Game = 1
        }

        private GameMode gameMode;
        private bool glLoad;
        private const int MaxFrameRate = 60;
        private Stopwatch sw;

        private const int LeftGameWidth = 200;
        private const int LeftGameHeight = 420;
        private const int LeftGameLeft = 20;
        private const int LeftGameBottom = 20;
        private const int RightGameWidth = 200;
        private const int RightGameHeight = 420;
        private const int RightGameLeft = 20+200+20+100+20;
        private const int RightGameBottom = 20;

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
                    gameMode = GameMode.Game;
                }
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

        private void Render(double timeDelta)
        {
            if (gameMode == GameMode.CreatingGL)
            {
                return;
            }
            if ((gameMode & GameMode.Game) != 0)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                RenderBackground(timeDelta);

                //lock for the left
                battle.MonEnter(true);
                if (battle.isAnimationMode(true))
                {
                    RenderLeftAnimation(timeDelta);
                }
                else
                {
                    RenderLeftGame();
                    accumLeft = 0; // reset time accumulator
                }
                battle.MonExit(true);

                battle.MonEnter(false);
                if (battle.isAnimationMode(false))
                {
                    RenderRightAnimation(timeDelta);
                }
                else
                {
                    RenderRightGame();
                    accumRight = 0; // reset time accumulator
                }
                battle.MonExit(false);
                glMain.SwapBuffers();
            }
        }


        private void RenderAnimation(bool isLeft, double timeElapsed)
        {
            //TODO:
/*            ChainResult cResult = battle.chainResult(isLeft);
            cResult.SetElapsedTime(timeElapsed);*/
        }


        double accumLeft = 0;
        double accumRight = 0;
        private void RenderLeftAnimation(double timeDelta)
        {
            SetupLeftGameRender();
            RenderAnimation(true, accumLeft);

            accumRight += timeDelta;
        }

        private void RenderRightAnimation(double timeDelta)
        {
            SetupRightGameRender();
            RenderAnimation(false, accumRight);

            accumLeft += timeDelta;
        }


        private void RenderPile(bool isLeft)
        {
            //Use Texture
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, TN_BLOCK);
            //Draw begin
            GL.Begin(BeginMode.Quads);
            for (int i = 0; i < Pile.ROW_SIZE; i++)
            {
                for (int j = 0; j < Pile.COL_SIZE; j++)
                {
                    Color color = Core.GraphicsUtil.CellColor2Color(battle.GetPileCellColor(isLeft, i, j));
                    GL.Color4(color);
                    GL.TexCoord2(0, 1);
                    GL.Vertex2(20 * j + 1, 20 * i + 1);
                    GL.TexCoord2(1, 1);
                    GL.Vertex2(20 * (j + 1) - 1, 20 * i + 1);
                    GL.TexCoord2(1, 0);
                    GL.Vertex2(20 * (j + 1) - 1, 20 * (i + 1) - 1);
                    GL.TexCoord2(0, 0);
                    GL.Vertex2(20 * j + 1, 20 * (i + 1) - 1);
                }
            }
            GL.End();
            //Disable Texture
            GL.Disable(EnableCap.Texture2D);
        }
        private void RenderBlock(bool isLeft)
        {
            //Use Texture
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, TN_BLOCK);
            //Draw begin
            GL.Begin(BeginMode.Quads);
            for (int i = 0; i < Block.ROW_SIZE; i++)
            {
                for (int j = 0; j < Block.COL_SIZE; j++)
                {
                    int row = i + battle.GetRow(isLeft), col = j + battle.GetCol(isLeft);

                    Color color = Core.GraphicsUtil.CellColor2Color(battle.GetBlockCellColor(isLeft, i, j));
                    GL.Color4(color);
                    GL.TexCoord2(0, 1);
                    GL.Vertex2(20 * col + 1, 20 * row + 1);
                    GL.TexCoord2(1, 1);
                    GL.Vertex2(20 * (col + 1) - 1, 20 * row + 1);
                    GL.TexCoord2(1, 0);
                    GL.Vertex2(20 * (col + 1) - 1, 20 * (row + 1) - 1);
                    GL.TexCoord2(0, 0);
                    GL.Vertex2(20 * col + 1, 20 * (row + 1) - 1);
                }
            }
            GL.End();
            //Disable Texture
            GL.Disable(EnableCap.Texture2D);
        }

        private void RenderGhost(bool isLeft)
        {
            if (battle.UseGhost == false) return;
            //Use Texture
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, TN_BLOCK);
            //Draw begin
            GL.Begin(BeginMode.Quads);
            for (int i = 0; i < Block.ROW_SIZE; i++)
            {
                for (int j = 0; j < Block.COL_SIZE; j++)
                {
                    int row = battle.GetGhostRow(isLeft, i, j), col = battle.GetGhostCol(isLeft,i,j);

                    Color colorOriginal = Core.GraphicsUtil.CellColor2Color(battle.GetBlockCellColor(isLeft, i, j));
                    Color color = System.Drawing.Color.FromArgb(colorOriginal.A / 4,colorOriginal.R, colorOriginal.G, colorOriginal.B);
                    GL.Color4(color);
                    GL.TexCoord2(0, 1);
                    GL.Vertex2(20 * col + 1, 20 * row + 1);
                    GL.TexCoord2(1, 1);
                    GL.Vertex2(20 * (col + 1) - 1, 20 * row + 1);
                    GL.TexCoord2(1, 0);
                    GL.Vertex2(20 * (col + 1) - 1, 20 * (row + 1) - 1);
                    GL.TexCoord2(0, 0);
                    GL.Vertex2(20 * col + 1, 20 * (row + 1) - 1);
                }
            }
            GL.End();
            //Disable Texture
            GL.Disable(EnableCap.Texture2D);
        }


        private void RenderGame(bool isLeft)
        {
            RenderPile(isLeft);
            RenderBlock(isLeft);
            RenderGhost(isLeft);
        }

        private void SetupLeftGameRender()
        {
            //Setup Viewport
            int w = LeftGameWidth;
            int h = LeftGameHeight;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1);
            GL.Viewport(LeftGameLeft, LeftGameBottom, w, h);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            //Set alpha blending
            GL.ColorMask(true, true, true, true);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            //Draw Background
            GL.Begin(BeginMode.Quads);
            GL.Color4(Color.Black);
            GL.Vertex2(0, 0);
            GL.Vertex2(w, 0);
            GL.Vertex2(w, h);
            GL.Vertex2(0, h);
            GL.End();
        }

        private void RenderLeftGame()
        {
            SetupLeftGameRender();
            RenderGame(true);
        }
        private void SetupRightGameRender()
        {
            //Setup Viewport
            int w = RightGameWidth;
            int h = RightGameHeight;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1);
            GL.Viewport(RightGameLeft, RightGameBottom, w, h);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            //Set alpha blending
            GL.ColorMask(true, true, true, true);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            //Draw Background
            GL.Begin(BeginMode.Quads);
            GL.Color4(Color.Black);
            GL.Vertex2(0, 0);
            GL.Vertex2(w, 0);
            GL.Vertex2(w, h);
            GL.Vertex2(0, h);
            GL.End();
        }
        private void RenderRightGame()
        {
            SetupRightGameRender();
            RenderGame(false);
        }

        private void SetupViewport()
        {
            int w = glMain.Width;
            int h = glMain.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
        }

        private void RenderBackground(double timeDelta)
        {
            //set render pipeline matrix
            SetupViewport();

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            
            // draw both front and back, polygon fill mode
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            // set polygon front face
            GL.FrontFace(FrontFaceDirection.Ccw);

            //TODO: modify below code
            GL.Begin(BeginMode.Polygon);
            GL.Color3((double)0.3, (double)0.5, (double)Math.Abs(System.DateTime.Now.Millisecond-500)/1000.0);
            GL.Vertex2(0, 0);
            GL.Vertex2(glMain.Width, 0);
            GL.Vertex2(glMain.Width, glMain.Height);
            GL.Vertex2(0, glMain.Height);
            GL.End();


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

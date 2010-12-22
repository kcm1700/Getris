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

namespace getris
{
    public partial class MainDlg : Form
    {
        private enum GameMode
        {
            CreatingGL,
            GameControl,
            GameAnimation
        }

        private GameMode gameMode;
        private bool glLoad;
        private const int MaxFrameRate = 30;
        private Stopwatch sw;

        public MainDlg()
        {
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
                    gameMode = GameMode.GameControl;
                }
            }
            if (gameMode == GameMode.GameControl)
            {
                //TODO: check if animation should occur.
            }
            if (gameMode == GameMode.GameAnimation)
            {
                //TODO: check if animation ended
            }
        }

        void Update(double timeDelta)
        {
            switch (gameMode)
            {
                case GameMode.CreatingGL:
                    //do nothing here
                    break;
                case GameMode.GameControl:
                    //TODO: update game state based on input queue
                    break;
                case GameMode.GameAnimation:
                    //TODO: 
                    break;
            }
        }

        private void Render(double timeDelta)
        {
            switch (gameMode)
            {
                case GameMode.CreatingGL:
                    //do nothing
                    break;
                case GameMode.GameControl:
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    RenderBackground(timeDelta);
                    RenderLeftGame();
                    RenderRightGame();
                    glMain.SwapBuffers();
                    break;
                case GameMode.GameAnimation:
                    //TODO:
                    break;
            }
        }

        private void RenderLeftGame()
        {
            //TODO:
        }
        private void RenderRightGame()
        {
            //TODO:
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
            GL.Color3((double)0.3, (double)0.5, (double)System.DateTime.Now.Millisecond/1000.0);
            GL.Vertex2(0, timeDelta*1000);
            GL.Vertex2(glMain.Width, 0);
            GL.Vertex2(glMain.Width, glMain.Height);
            GL.Vertex2(0, glMain.Height);
            GL.End();
        }
    }
}

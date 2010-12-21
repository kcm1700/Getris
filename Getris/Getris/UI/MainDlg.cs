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

namespace getris
{
    public partial class MainDlg : Form
    {
        private enum GameMode
        {
            CreatingGL,
            Game
        }

        private GameMode gameMode;
        private bool glLoad;
        private DateTime lastTime;
        private const int MaxFrameRate = 30;

        public MainDlg()
        {
            glLoad = false;
            gameMode = GameMode.CreatingGL;
            lastTime = DateTime.Now;
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
        }

        // an efficient game loop
        public void IdleGameLoop(object sender, EventArgs e)
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

        void TransitGameMode()
        {
            if (gameMode == GameMode.CreatingGL)
            {
                if (isGLReady)// when it's ready to use GL
                {   //Enter Game mode
                    gameMode = GameMode.Game;
                }
            }
        }

        void Update(double timeDelta)
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

        private void Render()
        {
            switch (gameMode)
            {
                case GameMode.CreatingGL:
                    //do nothing
                    break;
                case GameMode.Game:
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    RenderBackground();
                    RenderLeftGame();
                    RenderRightGame();
                    glMain.SwapBuffers();
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

        private void RenderBackground()
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
            GL.Vertex2(0, 0);
            GL.Vertex2(glMain.Width, 0);
            GL.Vertex2(glMain.Width, glMain.Height);
            GL.Vertex2(0, glMain.Height);
            GL.End();
        }

        bool IsIdle
        {
            get
            {
                NativeMessage msg;
                return !PeekMessage(out msg, this.Handle, 0, 0, 0);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
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

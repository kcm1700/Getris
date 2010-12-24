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

    public partial class MainDlg
    {

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
                    RenderLeftGame(timeDelta);
                    accumLeft = 0; // reset time accumulator
                }
                //draw next blocks
                SetupLeftNext1Render();
                RenderNextBlock(true, 0);
                SetupLeftNext2Render();
                RenderNextBlock(true, 1);

                battle.MonExit(true);

                battle.MonEnter(false);
                if (battle.isAnimationMode(false))
                {
                    RenderRightAnimation(timeDelta);
                }
                else
                {
                    RenderRightGame(timeDelta);
                    accumRight = 0; // reset time accumulator
                }
                //draw next blocks
                SetupRightNext1Render();
                RenderNextBlock(false, 0);
                SetupRightNext2Render();
                RenderNextBlock(false, 1);
                battle.MonExit(false);

                glMain.SwapBuffers();
            }
        }


        void ConnectHorizontally(double row, double col, Color a, Color b)
        {
            if (a.Equals(b))
            {
                GL.Disable(EnableCap.Texture2D);
                GL.Color4(a);
                GL.Vertex2(20 * (col + 1) - 1, 20 * (row) + 1);
                GL.Vertex2(20 * (col + 1) + 1, 20 * (row) + 1);
                GL.Vertex2(20 * (col + 1) + 1, 20 * (row + 1) - 1);
                GL.Vertex2(20 * (col + 1) - 1, 20 * (row + 1) - 1);
                GL.Enable(EnableCap.Texture2D);
            }
        }
        void ConnectVertically(double row, double col, Color a, Color b)
        {
            if (a.Equals(b))
            {
                GL.Disable(EnableCap.Texture2D);
                GL.Color4(a);
                GL.Vertex2(20 * (col) + 1, 20 * (row + 1) - 1);
                GL.Vertex2(20 * (col + 1) - 1, 20 * (row + 1) - 1);
                GL.Vertex2(20 * (col + 1) - 1, 20 * (row + 1) + 1);
                GL.Vertex2(20 * (col) + 1, 20 * (row + 1) + 1);
                GL.Enable(EnableCap.Texture2D);
            }
        }


        private void RenderAnimation(bool isLeft, double timeElapsed)
        {
            //TODO:
            Animation.Animator aResult = battle.GetAnimator(isLeft);
            if (aResult.SetElapsedTime(timeElapsed))
            {
                battle.finishedAnimationMode(isLeft);
            }
            //Use Texture
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, TN_BLOCK);
            //Draw begin
            GL.Begin(BeginMode.Quads);
            for (int i = 0; i < Pile.ROW_SIZE; i++)
            {
                for (int j = 0; j < Pile.COL_SIZE; j++)
                {
                    Color color = Core.GraphicsUtil.CellColor2Color(aResult.getAniCellColor(i, j));
                    double row = aResult.getAniRow(i, j), col = aResult.getAniCol(i, j);
                    if (j + 1 < Pile.COL_SIZE)
                    {
                        if (Math.Abs(row - aResult.getAniRow(i, j + 1)) < 1e-9)
                        {
                            ConnectHorizontally(row, col, color, Core.GraphicsUtil.CellColor2Color(aResult.getAniCellColor(i, j + 1)));
                        }
                    }
                    if (i + 1 < Pile.ROW_SIZE)
                    {
                        if (Math.Abs(col - aResult.getAniCol(i + 1, j)) < 1e-9)
                        {
                            ConnectVertically(row, col, color, Core.GraphicsUtil.CellColor2Color(aResult.getAniCellColor(i + 1, j)));
                        }
                    }
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


        double accumLeft = 0;
        double accumRight = 0;
        private void RenderLeftAnimation(double timeDelta)
        {
            SetupLeftGameRender();
            RenderAnimation(true, accumLeft);

            accumLeft += timeDelta;
        }

        private void RenderRightAnimation(double timeDelta)
        {
            SetupRightGameRender();
            RenderAnimation(false, accumRight);

            accumRight += timeDelta;
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

                    if (j + 1 < Pile.COL_SIZE)
                    {
                        ConnectHorizontally(i, j, color, Core.GraphicsUtil.CellColor2Color(battle.GetPileCellColor(isLeft, i, j + 1)));
                    }
                    if (i + 1 < Pile.ROW_SIZE)
                    {
                        ConnectVertically(i, j, color, Core.GraphicsUtil.CellColor2Color(battle.GetPileCellColor(isLeft, i + 1, j)));
                    }
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
        private void RenderBlock(bool isLeft, double timeElapsed)
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

                    //set center block brighter
                    if (i == 1 && j == 1)
                    {
                        if ((int)(timeElapsed / 0.2) % 2 == 0)
                        {
                            color = Color.FromArgb(color.A, color.R / 2 + 128, color.G / 2 + 128, color.B / 2 + 128);
                        }
                    }
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
                    int row = battle.GetGhostRow(isLeft, i, j), col = battle.GetGhostCol(isLeft, i, j);

                    Color colorOriginal = Core.GraphicsUtil.CellColor2Color(battle.GetBlockCellColor(isLeft, i, j));
                    Color color = System.Drawing.Color.FromArgb(colorOriginal.A / 3, colorOriginal.R, colorOriginal.G, colorOriginal.B);
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


        private void RenderGame(bool isLeft, double timeElapsed)
        {
            RenderPile(isLeft);
            RenderBlock(isLeft, timeElapsed);
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


        private void RenderNextBlock(bool isLeft, int howmany)
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
                    int row = i, col = j;
                    Color color = Core.GraphicsUtil.CellColor2Color(battle.GetNextBlockCellColor(isLeft, howmany, i, j));
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
        private void SetupLeftNext1Render()
        {
            //Setup Viewport
            int w = LeftGameNext1.Width;
            int h = LeftGameNext1.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1);
            GL.Viewport(LeftGameNext1);
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
        private void SetupLeftNext2Render()
        {
            //Setup Viewport
            int w = LeftGameNext2.Width;
            int h = LeftGameNext2.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1);
            GL.Viewport(LeftGameNext2);
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
        private void SetupRightNext1Render()
        {
            //Setup Viewport
            int w = RightGameNext1.Width;
            int h = RightGameNext1.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1);
            GL.Viewport(RightGameNext1);
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
        private void SetupRightNext2Render()
        {
            //Setup Viewport
            int w = RightGameNext2.Width;
            int h = RightGameNext2.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1);
            GL.Viewport(RightGameNext2);
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

        double timeElapsedLeft = 0;
        double timeElapsedRight = 0;
        private void RenderLeftGame(double timeDelta)
        {
            SetupLeftGameRender();
            RenderGame(true, timeElapsedLeft);
            timeElapsedLeft += timeDelta;
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
        private void RenderRightGame(double timeDelta)
        {
            SetupRightGameRender();
            RenderGame(false, timeElapsedRight);
            timeElapsedRight += timeDelta;
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
            GL.Color3((double)0.3, (double)0.5, (double)Math.Abs(System.DateTime.Now.Millisecond - 500) / 1000.0);
            GL.Vertex2(0, 0);
            GL.Vertex2(glMain.Width, 0);
            GL.Vertex2(glMain.Width, glMain.Height);
            GL.Vertex2(0, glMain.Height);
            GL.End();


        }

    }
}

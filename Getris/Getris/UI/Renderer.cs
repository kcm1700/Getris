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

        private const int LeftGameWidth = 200;
        private const int LeftGameHeight = 420;
        private const int LeftGameLeft = 20;
        private const int LeftGameBottom = 80;
        private static readonly Rectangle LeftGameNext1 = new Rectangle(225, 380, 20 * 3, 20 * 3);
        private static readonly Rectangle LeftGameNext2 = new Rectangle(225, 280, 20 * 3, 20 * 3);
        private static readonly Rectangle LeftScore = new Rectangle(20, 20, 200, 40);

        private const int RightGameWidth = 200;
        private const int RightGameHeight = 420;
        private const int RightGameLeft = 20 + 200 + 20 + 100 + 20;
        private const int RightGameBottom = 80;
        private static readonly Rectangle RightGameNext1 = new Rectangle(295, 380, 20 * 3, 20 * 3);
        private static readonly Rectangle RightGameNext2 = new Rectangle(295, 280, 20 * 3, 20 * 3);
        private static readonly Rectangle RightScore = new Rectangle(360, 20, 200, 40);


        void ConnectHorizontally(double row, double col, CellColor a, CellColor b, Color color)
        {
            if (a==b)
            {
                GL.Disable(EnableCap.Texture2D);
                GL.Color4(color);
                GL.Vertex2(20 * (col + 1) - 1, 20 * (row) + 1);
                GL.Vertex2(20 * (col + 1) + 1, 20 * (row) + 1);
                GL.Vertex2(20 * (col + 1) + 1, 20 * (row + 1) - 1);
                GL.Vertex2(20 * (col + 1) - 1, 20 * (row + 1) - 1);
                GL.Enable(EnableCap.Texture2D);
            }
        }
        void ConnectVertically(double row, double col, CellColor a, CellColor b, Color color)
        {
            if (a==b)
            {
                GL.Disable(EnableCap.Texture2D);
                GL.Color4(color);
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
                            ConnectHorizontally(row, col,aResult.getAniCellColor(i, j), aResult.getAniCellColor(i, j + 1), color);
                        }
                    }
                    if (i + 1 < Pile.ROW_SIZE)
                    {
                        if (Math.Abs(col - aResult.getAniCol(i + 1, j)) < 1e-9)
                        {
                            ConnectVertically(row, col, aResult.getAniCellColor(i, j), aResult.getAniCellColor(i + 1, j), color);
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
            SetupGameRender(true);
            RenderAnimation(true, accumLeft);

            accumLeft += timeDelta;
        }

        private void RenderRightAnimation(double timeDelta)
        {
            SetupGameRender(false);
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
                    int row = i;
                    int col = j;
                    Color color = Core.GraphicsUtil.CellColor2Color(battle.GetPileCellColor(isLeft, row, col));
                    if (col + 1 < Pile.COL_SIZE)
                    {
                        ConnectHorizontally(row, col, battle.GetPileCellColor(isLeft, row, col),  battle.GetPileCellColor(isLeft, row, col + 1), color);
                    }
                    if (row + 1 < Pile.ROW_SIZE)
                    {
                        ConnectVertically(row, col, battle.GetPileCellColor(isLeft, row, col), battle.GetPileCellColor(isLeft, row + 1, col), color);
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

                    if (i + 1 < Block.ROW_SIZE)
                    {
                        ConnectVertically(row, col, battle.GetBlockCellColor(isLeft, i, j), battle.GetBlockCellColor(isLeft, i + 1, j), color);
                    }
                    if (j + 1 < Block.COL_SIZE)
                    {
                        ConnectHorizontally(row, col, battle.GetBlockCellColor(isLeft, i, j), battle.GetBlockCellColor(isLeft, i, j + 1), color);
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

        private void SetupGameRender(bool isLeft)
        {
            //Setup Viewport
            int w= isLeft?LeftGameWidth:RightGameWidth;
            int h = isLeft ? LeftGameHeight : RightGameHeight;
            int gameLeft = isLeft ? LeftGameLeft : RightGameLeft;
            int gameBottom = isLeft ? LeftGameBottom : RightGameBottom;
            
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1);
            GL.Viewport(gameLeft, gameBottom, w, h);
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
                    
                    if (row + 1 < Block.ROW_SIZE)
                    {
                        ConnectVertically(row, col, battle.GetNextBlockCellColor(isLeft, howmany, row, col), battle.GetNextBlockCellColor(isLeft, howmany, row + 1, col), color);
                    }
                    if (col + 1 < Block.COL_SIZE)
                    {
                        ConnectHorizontally(row, col, battle.GetNextBlockCellColor(isLeft, howmany, row, col), battle.GetNextBlockCellColor(isLeft, howmany, row, col + 1), color);
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
        private void SetupNextRender(bool isLeft,int howmany)
        {
            Rectangle next;
            if(howmany==1)
                next= isLeft? LeftGameNext1:RightGameNext1;
            else
                next = isLeft ? LeftGameNext2 : RightGameNext2;
            
            //Setup Viewport
            int w = next.Width;
            int h = next.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1);
            GL.Viewport(next);
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
            SetupGameRender(true);
            RenderGame(true, timeElapsedLeft);
            timeElapsedLeft += timeDelta;
        }
        private void RenderRightGame(double timeDelta)
        {
            SetupGameRender(false);
            RenderGame(false, timeElapsedRight);
            timeElapsedRight += timeDelta;
        }

        private void SetupViewport()
        {
            int w = glMain.Width;
            int h = glMain.Height;
            GL.ClearColor(Color.White);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
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










        // Score Renderer
        // Score Renderer
        // Score Renderer
        // Score Renderer
        // Score Renderer
        // Score Renderer
        // Score Renderer
        // Score Renderer
        // Score Renderer
        // Score Renderer
        // Score Renderer
        // Score Renderer
        // Score Renderer




        private void SetupLeftScoreRender()
        {
            //Setup Viewport
            int w = LeftScore.Width;
            int h = LeftScore.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1);
            GL.Viewport(LeftScore.X, LeftScore.Y, LeftScore.Width, LeftScore.Height);
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
        private void SetupRightScoreRender()
        {
            //Setup Viewport
            int w = RightScore.Width;
            int h = RightScore.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1);
            GL.Viewport(RightScore.X, RightScore.Y, RightScore.Width, RightScore.Height);
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

        private void LeftScoreRender()
        {
            SetupLeftScoreRender();
            PrintScore(battle.GetScore(true), LeftScore.Width, LeftScore.Height);
        }

        private void RightScoreRender()
        {
            SetupRightScoreRender();
            PrintScore(battle.GetScore(false), LeftScore.Width, LeftScore.Height);
        }


        private void PrintScore(Decimal value,double w,double h)
        {
            double pos = w - h * 0.5;
            double digitWidth = h * 0.5;
            double digitHeight = h;
            if (value == 0)
            {
                GL.MatrixMode(MatrixMode.Projection);
                GL.PushMatrix();
                GL.Translate(pos, 0, 0);
                PrintDigit((int)((value % 10 + 10) % 10), digitWidth, digitHeight);
                value /= 10;
                pos -= digitWidth;
                GL.PopMatrix();
            }
            while (value >= 1)
            {
                GL.MatrixMode(MatrixMode.Projection);
                GL.PushMatrix();
                GL.Translate(pos,0,0);
                PrintDigit((int)((value % 10+10)%10),digitWidth,digitHeight);
                value /= 10;
                pos -= digitWidth;
                GL.MatrixMode(MatrixMode.Projection);
                GL.PopMatrix();
            }
        }

        private void PrintDigit(int digitval, double digitWidth, double digitHeight)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, TN_NUMBERS);
            GL.Begin(BeginMode.Quads);{
                GL.Color4(Color.White);
                GL.TexCoord2((double)digitval / 10.0, 1);
                GL.Vertex2(0, 0);
                GL.TexCoord2((double)digitval / 10.0, 0);
                GL.Vertex2(0, digitHeight);
                GL.TexCoord2((double)(digitval+1) / 10.0, 0);
                GL.Vertex2(digitWidth, digitHeight);
                GL.TexCoord2((double)(digitval+1) / 10.0, 1);
                GL.Vertex2(digitWidth, 0);
            } GL.End();
            GL.Disable(EnableCap.Texture2D);
        }



        // BELOW ARE
        // GAME OVER RENDER
        //
        //
        //
        private void SetupGameOverRender(bool isLeft, double timeElapsed)
          {
            //Setup Viewport
            int w = isLeft ? LeftGameWidth : RightGameWidth;
            int h = isLeft ? LeftGameHeight : RightGameHeight;
            int gameLeft = isLeft ? LeftGameLeft : RightGameLeft;
            int gameBottom = isLeft ? LeftGameBottom : RightGameBottom;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1);
            GL.Viewport(gameLeft, gameBottom, w, h);
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

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0 - timeElapsed * w, w + timeElapsed * w, 0 - timeElapsed * h, h + timeElapsed * h, -1, 1);
            GL.Translate(w / 2, h / 2, 0);
            GL.Rotate(timeElapsed * 360, 0, 0, 1);
            GL.Translate(-w / 2, -h / 2, 0);
        }

        private void RenderGameOver(bool isLeft, double timeDelta)
        {
            double accum= isLeft? accumLeft:accumRight;
            if (battle.isOver(isLeft))
            {
                SetupGameOverRender(isLeft,accum);
            }
            else
            {
                SetupGameRender(isLeft);
            }
            RenderGame(false, accum);
            if(isLeft)
                accumLeft += timeDelta;
            else
                accumRight += timeDelta;
        }
    }
}

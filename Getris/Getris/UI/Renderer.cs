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

        double accumLeft = 0;
        double accumRight = 0;

        double timeElapsedLeft = 0;
        double timeElapsedRight = 0;

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
                    DrawCell(row, col, color);
                }
            }
            GL.End();
            //Disable Texture
            GL.Disable(EnableCap.Texture2D);
        }
        private void RenderUserAnimation(bool isLeft, double timeDelta)
        {
            SetupGameRender(isLeft);
            RenderAnimation(isLeft, isLeft ? accumLeft : accumRight);
            IncreaseAccum(isLeft, timeDelta);
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
                    DrawCell(row, col, color);
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

                    DrawCell(row, col, color);
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

                    DrawCell(row, col, color);
                }
            }
            GL.End();
            //Disable Texture
            GL.Disable(EnableCap.Texture2D);
        }

        private void DrawCell(double row, double col, Color color)
        {
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
        private void ConnectHorizontally(double row, double col, CellColor a, CellColor b, Color color)
        {
            if (a == b)
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
        private void ConnectVertically(double row, double col, CellColor a, CellColor b, Color color)
        {
            if (a == b)
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

        private void DrawBackground(int width, int height, Color color)
        {
            GL.Begin(BeginMode.Quads);
            GL.Color4(color);
            GL.Vertex2(0, 0);
            GL.Vertex2(width, 0);
            GL.Vertex2(width, height);
            GL.Vertex2(0, height);
            GL.End();
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
            DrawBackground(w, h, Color.Black);
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

                    DrawCell(row, col, color);
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
            DrawBackground(w, h, Color.Black);
        }


        private void RenderUserGame(bool isLeft, double timeDelta)
        {
            SetupGameRender(isLeft);
            RenderGame(isLeft, isLeft?timeElapsedLeft:timeElapsedRight);
            if(isLeft)
                timeElapsedLeft += timeDelta;
            else
                timeElapsedRight+=timeDelta;
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
        //
        //
        private void SetupScoreRender(bool isLeft)
        {
            Rectangle score = isLeft ? LeftScore : RightScore;
            //Setup Viewport
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, score.Width, 0, score.Height, -1, 1);
            GL.Viewport(score.X, score.Y, score.Width, score.Height);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //Set alpha blending
            GL.ColorMask(true, true, true, true);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            DrawBackground(score.Width, score.Height, Color.Black);
        }
        private void ScoreRender(bool isLeft)
        {
            int w = isLeft? LeftScore.Width:RightScore.Width;
            int h = isLeft? LeftScore.Height:RightScore.Height;
            SetupScoreRender(isLeft);
            PrintScore(battle.GetScore(isLeft), w, h);
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
            GL.Begin(BeginMode.Quads);
            {
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

            DrawBackground(w, h, Color.Black);

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
            IncreaseAccum(isLeft, timeDelta);
        }
        private void IncreaseAccum(bool isLeft, double inc)
        {
            if (isLeft)
                accumLeft += inc;
            else
                accumRight += inc;
        }
        private void ResetAccum(bool isLeft)
        {
            if (isLeft)
                accumLeft = 0;
            else
                accumRight = 0;
        }
    }
}

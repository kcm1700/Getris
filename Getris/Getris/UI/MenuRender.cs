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
using Microsoft.Win32;
using System.Runtime.InteropServices;

using getris.GameState;

namespace getris
{
    public partial class MainDlg
    {
        private double timeElapsedMenu = 0;
        private void MenuRender(double timeDelta)
        {
            SetupViewportMenu();

            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Texture2D);
            // draw both front and back, polygon fill mode
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            // set polygon front face
            GL.FrontFace(FrontFaceDirection.Ccw);

            //enable alpha 
            GL.ColorMask(true, true, true, true);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            RenderBackgroundMenu();

            RenderMenuBoard(timeDelta);
            timeElapsedMenu += timeDelta;
        }

        private double xwMin = -0.5;
        private double xwMax = 0.5;
        private double ywMin = -0.5;
        private double ywMax = 0.5;
        private double near = 1;
        private double far = 100;

        private double x0 = 0,y0 = 20,z0 = -50,xref = 0,yref = 0,zref = 0,vx=0,vy=1,vz=0;

        void LookAt(double eyeX,double eyeY,double eyeZ,double centerX,double centerY,double centerZ,double upX,double upY,double upZ)
        {
            OpenTK.Vector3d F = new OpenTK.Vector3d(centerX - eyeX, centerX - eyeY, centerZ - eyeZ);
            OpenTK.Vector3d UP = new OpenTK.Vector3d(upX, upY, upZ);
            F.Normalize();
            UP.Normalize();
            OpenTK.Vector3d s = new OpenTK.Vector3d(F.Y * UP.Z - F.Z * UP.Y,
                                                    F.Z * UP.X - F.X * UP.Z,
                                                    F.X * UP.Y - F.Y * UP.X);
            OpenTK.Vector3d u = new OpenTK.Vector3d(s.Y * F.Z - s.Z * F.Y,
                                                    s.Z * F.X - s.X * F.Z,
                                                    s.X * F.Y - s.Y * F.X);
            OpenTK.Matrix4d m = new OpenTK.Matrix4d(s.X, s.Y, s.Z, 0, u.X, u.Y, u.Z, 0, -F.X, -F.Y, -F.Z, 0, 0, 0, 0, 1);
            GL.MultMatrix(ref m);
            GL.Translate(-eyeX, -eyeY, -eyeZ);

        }

        private void SetupViewportMenu()
        {
            GL.ClearColor(Color.Black);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            LookAt(x0, y0, z0, xref, yref, zref, vx, vy, vz);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Frustum(xwMin, xwMax, ywMin, ywMax, near, far);

            GL.Viewport(0, 0, glMain.Width, glMain.Height);
        }
        private void RenderBackgroundMenu()
        {
            //TODO:
        }


        private double[] menuPositionAngle;
        private double menuCurAngle;
        private double menuOriginAngle;
        private int menuSelection;

        private void RenderMenuBoard(double timeDelta)
        {
            for (int i = 0; i < menuPositionAngle.Length; i++)
            {
                double boardAngle = -menuCurAngle + menuPositionAngle[i];
                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                GL.Translate(Math.Sin(boardAngle) * 18, 0, -Math.Cos(boardAngle) * Math.Abs(Math.Cos(boardAngle)) * 8);

                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D,TN_MENU[i]);
                GL.Begin(BeginMode.Polygon);
                {
                    if (i == menuSelection)
                    {
                        GL.Color4(Color.White);
                    }
                    else
                    {
                        GL.Color4(1.0, 1.0, 1.0, Math.Pow((1 + Math.Cos(boardAngle)), 1) * 0.6);
                    }
                    GL.TexCoord2(1, 1);
                    GL.Vertex3(-10, -10, 0);
                    GL.TexCoord2(1, 0);
                    GL.Vertex3(-10, 10, 0);
                    GL.TexCoord2(0, 0);
                    GL.Vertex3(10, 10, 0);
                    GL.TexCoord2(0, 1);
                    GL.Vertex3(10, -10, 0);
                } GL.End();
                GL.Disable(EnableCap.Texture2D);

                GL.PopMatrix();
            }
        }

        private void UpdateMenuPosition(double timeDelta)
        {
            double ratio = Math.Pow(Math.Exp(timeElapsedMenu)-1,1);
            double destAngle = menuPositionAngle[menuSelection];
            if (Math.Abs(menuOriginAngle - destAngle) > Math.Abs(menuOriginAngle - OpenTK.MathHelper.TwoPi - destAngle))
            {
                menuOriginAngle -= OpenTK.MathHelper.TwoPi;
            }
            if (Math.Abs(menuOriginAngle - destAngle) > Math.Abs(menuOriginAngle + OpenTK.MathHelper.TwoPi - destAngle))
            {
                menuOriginAngle += OpenTK.MathHelper.TwoPi;
            }
            menuCurAngle = (destAngle*ratio + menuOriginAngle) / (1 + ratio);
        }

        private void UpdateMenu(double timeDelta)
        {
            nextGameMode = GameMode.GameMenu;
            if (!Core.Keyboard.Instance.IsEmpty())
            {
                switch (Core.Keyboard.Instance.Peek().data)
                {
                    case "up":
                    case "left":
                        menuOriginAngle = menuCurAngle;
                        menuSelection = (menuSelection + menuPositionAngle.Length - 1) % menuPositionAngle.Length;
                        timeElapsedMenu = 0;
                        Core.Keyboard.Instance.Pop();
                        break;
                    case "down":
                    case "right":
                        menuOriginAngle = menuCurAngle;
                        menuSelection = (menuSelection + 1) % menuPositionAngle.Length;
                        timeElapsedMenu = 0;
                        Core.Keyboard.Instance.Pop();
                        break;
                    case "":
                        break;
                    default:
                        Core.Keyboard.Instance.Pop();
                        MenuWork();
                        break;
                }
            }
            UpdateMenuPosition(timeDelta);
            timeElapsedMenu += timeDelta;
        }

        private void MenuWork()
        {
            switch (menuSelection)
            {
                case 0: // 혼자하기
                    nextGameMode = GameMode.Game;
                    battle = new Battle();
                    Core.Keyboard.InputMode = Core.Keyboard.InputModes.Game;
                    break;
                case 1: // 둘이하기
                    /*
                    nextGameMode = GameMode.Game;
                    battle = new Battle(true,true);
                    Core.Keyboard.InputMode = Core.Keyboard.InputModes.Game;
                     */
                    break;
                case 2: // 네트워크플레이
                    nextGameMode = GameMode.Game;
                    battle = new Battle();
                    Core.Keyboard.InputMode = Core.Keyboard.InputModes.Game;
                    Core.Network.Instance.Open();
                    break;
                case 3: // 끝내기
                    nextGameMode = GameMode.Exit;
                    break;
                case 4: // 환경설정
                    btnPreference.PerformClick();
                    Core.Keyboard.KeyReset();
                    break;
            }
        }
        
    }
}



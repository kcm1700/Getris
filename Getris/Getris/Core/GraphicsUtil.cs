﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

using getris.GameState;
using System.IO;

namespace getris.Core
{
    static class GraphicsUtil
    {
        static Dictionary<CellColor, Color> colorDictionary;

        static GraphicsUtil()
        {
            colorDictionary = new Dictionary<CellColor,Color>();
            colorDictionary[CellColor.dropped] = Color.White;
            colorDictionary[CellColor.transparent] = Color.Transparent;
            colorDictionary[CellColor.color1] = Color.Red;
            colorDictionary[CellColor.color2] = Color.DeepSkyBlue;
            colorDictionary[CellColor.color3] = Color.LimeGreen;
            colorDictionary[CellColor.color4] = Color.Yellow;
            colorDictionary[CellColor.color5] = Color.BlueViolet;
        }
        static public Color CellColor2Color(CellColor color)
        {
            try
            {
                return colorDictionary[color];
            }
            catch
            {
                return Color.Transparent;
            }
        }
        static public void SetNewColor(CellColor cellColor, Color color)
        {
            colorDictionary[cellColor] = color;
        }

        static public int LoadTexture(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bmp = new Bitmap(filename);
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);

            // We haven't uploaded mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // On newer video cards, we can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return id;
        }

    }
}

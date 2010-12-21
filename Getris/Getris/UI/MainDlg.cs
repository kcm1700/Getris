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

namespace getris
{
    public partial class MainDlg : Form
    {
        private bool glLoad = false;

        public bool isGLReady
        {
            get
            {
                return glLoad;
            }
        }

        public MainDlg()
        {
            InitializeComponent();
        }

        private void glMain_Load(object sender, EventArgs e)
        {
            glLoad = true;
        }
    }
}

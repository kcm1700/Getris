using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace getris.UI
{
    public partial class PreferencesDlg : Form
    {
        public PreferencesDlg()
        {
            InitializeComponent();
        }

        private void setBtnColor()
        {
            colorButton1.BackColor = Core.GraphicsUtil.CellColor2Color(GameState.CellColor.color1);
            colorButton2.BackColor = Core.GraphicsUtil.CellColor2Color(GameState.CellColor.color2);
            colorButton3.BackColor = Core.GraphicsUtil.CellColor2Color(GameState.CellColor.color3);
            colorButton4.BackColor = Core.GraphicsUtil.CellColor2Color(GameState.CellColor.color4);
            colorButton5.BackColor = Core.GraphicsUtil.CellColor2Color(GameState.CellColor.color5);
        }

        private void setRotation()
        {
            this.rdoCw.Checked = false;
            this.rdoCcw.Checked = false;
            if (Core.KeySettings.KeyRotateCcw2 == Keys.Up)
            {
                this.rdoCcw.Checked = true;
            }
            if (Core.KeySettings.KeyRotateCw2 == Keys.Up)
            {
                this.rdoCw.Checked = true;
            }
        }
        private void setNetworkMode()
        {
            this.rdoGuest.Checked = !Core.Network.Instance.IsHost;
            this.rdoHost.Checked = Core.Network.Instance.IsHost;
            this.ipText.Text = Core.Network.Instance.IP;
            this.portText.Text = Core.Network.Instance.Port;
        }
        private void ColorPicker_Load(object sender, EventArgs e)
        {
            setBtnColor();
            setRotation();
            setNetworkMode();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Reset();
            this.colorDialog1.Color = Core.GraphicsUtil.CellColor2Color(GameState.CellColor.color1);
            this.colorDialog1.ShowDialog();
            Core.GraphicsUtil.SetNewColor(GameState.CellColor.color1, colorDialog1.Color);
            setBtnColor();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Reset();
            this.colorDialog1.Color = Core.GraphicsUtil.CellColor2Color(GameState.CellColor.color2);
            this.colorDialog1.ShowDialog();
            Core.GraphicsUtil.SetNewColor(GameState.CellColor.color2, colorDialog1.Color);
            setBtnColor();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Reset();
            this.colorDialog1.Color = Core.GraphicsUtil.CellColor2Color(GameState.CellColor.color3);
            this.colorDialog1.ShowDialog();
            Core.GraphicsUtil.SetNewColor(GameState.CellColor.color3, colorDialog1.Color);
            setBtnColor();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Reset();
            this.colorDialog1.Color = Core.GraphicsUtil.CellColor2Color(GameState.CellColor.color4);
            this.colorDialog1.ShowDialog();
            Core.GraphicsUtil.SetNewColor(GameState.CellColor.color4, colorDialog1.Color);
            setBtnColor();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Reset();
            this.colorDialog1.Color = Core.GraphicsUtil.CellColor2Color(GameState.CellColor.color5);
            this.colorDialog1.ShowDialog();
            Core.GraphicsUtil.SetNewColor(GameState.CellColor.color5, colorDialog1.Color);
            setBtnColor();

        }

        private void rdoCw_Clicked(object sender, EventArgs e)
        {
            Core.KeySettings.KeyRotateCw2 = Keys.Up;
            Core.KeySettings.KeyRotateCcw2 = Keys.Enter;
        }

        private void rdoCcw_Clicked(object sender, EventArgs e)
        {
            Core.KeySettings.KeyRotateCcw2 = Keys.Up;
            Core.KeySettings.KeyRotateCw2 = Keys.Enter;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            MainDlg.MaxFrameRate = fpsBar.Value;
        }

        private void save_Click(object sender, EventArgs e)
        {
            Core.Network.Instance.IP = this.ipText.Text;
            Core.Network.Instance.Port = this.portText.Text;
            Core.Network.Instance.IsHost = this.rdoHost.Checked;
        }
        private void reset_Click(object sender, EventArgs e)
        {
            setNetworkMode();
        }
    }
}

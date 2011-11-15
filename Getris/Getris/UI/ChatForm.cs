using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using getris.Core;

namespace getris.UI
{
    public partial class ChatForm : Form
    {

        public ChatForm()
        {
            InitializeComponent();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Network.Instance.IsOnline == false)
            {
                return;
            }
            while (!Network.Instance.ChatIsEmpty())
            {
                textBox1.Text = textBox1.Text + "\r\n" + Network.Instance.PopChat().data;
                Network.Instance.PopChat();
            }
        }

        private void textBox2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (Network.Instance.IsOnline == false)
            {
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                string data = ("<" + textBox3.Text + ">" + (sender as TextBox).Text);
                if (data.Length > 100) data.Remove(100);
                Chat a = new Chat(data);
                Network.Instance.Send(a);
                textBox1.Text = textBox1.Text + "\r\n" + data;
                textBox2.Text = "";
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

    }
}

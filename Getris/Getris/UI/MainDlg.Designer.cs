namespace getris
{
    partial class MainDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.glMain = new OpenTK.GLControl();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lblFPS = new System.Windows.Forms.Label();
            this.btnPreference = new System.Windows.Forms.Button();
            this.btnRegame = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // glMain
            // 
            this.glMain.BackColor = System.Drawing.Color.Black;
            this.glMain.Location = new System.Drawing.Point(0, 30);
            this.glMain.Name = "glMain";
            this.glMain.Size = new System.Drawing.Size(700, 480);
            this.glMain.TabIndex = 0;
            this.glMain.VSync = false;
            this.glMain.Load += new System.EventHandler(this.glMain_Load);
            this.glMain.Enter += new System.EventHandler(this.glMain_Enter);
            this.glMain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glMain_KeyDown);
            this.glMain.KeyUp += new System.Windows.Forms.KeyEventHandler(this.glMain_KeyUp);
            this.glMain.Leave += new System.EventHandler(this.glMain_Leave);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(716, 11);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(240, 430);
            this.textBox1.TabIndex = 1;
            this.textBox1.TabStop = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(716, 447);
            this.textBox2.MaxLength = 511;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(240, 21);
            this.textBox2.TabIndex = 2;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // lblFPS
            // 
            this.lblFPS.AutoSize = true;
            this.lblFPS.Location = new System.Drawing.Point(-2, 9);
            this.lblFPS.Name = "lblFPS";
            this.lblFPS.Size = new System.Drawing.Size(28, 12);
            this.lblFPS.TabIndex = 4;
            this.lblFPS.Text = "FPS";
            // 
            // btnPreference
            // 
            this.btnPreference.Location = new System.Drawing.Point(716, 474);
            this.btnPreference.Name = "btnPreference";
            this.btnPreference.Size = new System.Drawing.Size(126, 36);
            this.btnPreference.TabIndex = 5;
            this.btnPreference.Text = "OptionBox";
            this.btnPreference.UseVisualStyleBackColor = true;
            this.btnPreference.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // btnRegame
            // 
            this.btnRegame.Location = new System.Drawing.Point(848, 474);
            this.btnRegame.Name = "btnRegame";
            this.btnRegame.Size = new System.Drawing.Size(108, 36);
            this.btnRegame.TabIndex = 6;
            this.btnRegame.Text = "restart";
            this.btnRegame.Click += new System.EventHandler(this.btnRegame_Click);
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(971, 524);
            this.Controls.Add(this.btnPreference);
            this.Controls.Add(this.btnRegame);
            this.Controls.Add(this.lblFPS);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.glMain);
            this.Name = "MainDlg";
            this.Text = "Getris";
            this.Deactivate += new System.EventHandler(this.MainDlg_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDlg_FormClosing);
            this.Load += new System.EventHandler(this.MainDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glMain;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label lblFPS;
        private System.Windows.Forms.Button btnPreference;
        private System.Windows.Forms.Button btnRegame;
    }
}


namespace getris.UI
{
    partial class PreferencesDlg
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
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.colorBox = new System.Windows.Forms.GroupBox();
            this.colorButton5 = new System.Windows.Forms.Button();
            this.colorButton4 = new System.Windows.Forms.Button();
            this.colorButton3 = new System.Windows.Forms.Button();
            this.colorButton2 = new System.Windows.Forms.Button();
            this.colorButton1 = new System.Windows.Forms.Button();
            this.rotateBox = new System.Windows.Forms.GroupBox();
            this.rdoCcw = new System.Windows.Forms.RadioButton();
            this.rdoCw = new System.Windows.Forms.RadioButton();
            this.qualityBox = new System.Windows.Forms.GroupBox();
            this.fpsBar = new System.Windows.Forms.TrackBar();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.networkBox = new System.Windows.Forms.GroupBox();
            this.rdoHost = new System.Windows.Forms.RadioButton();
            this.rdoGuest = new System.Windows.Forms.RadioButton();
            this.ipLabel = new System.Windows.Forms.Label();
            this.ipText = new System.Windows.Forms.TextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.portText = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.colorBox.SuspendLayout();
            this.rotateBox.SuspendLayout();
            this.qualityBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsBar)).BeginInit();
            this.networkBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // colorBox
            // 
            this.colorBox.Controls.Add(this.colorButton5);
            this.colorBox.Controls.Add(this.colorButton4);
            this.colorBox.Controls.Add(this.colorButton3);
            this.colorBox.Controls.Add(this.colorButton2);
            this.colorBox.Controls.Add(this.colorButton1);
            this.colorBox.Location = new System.Drawing.Point(14, 11);
            this.colorBox.Name = "colorBox";
            this.colorBox.Size = new System.Drawing.Size(115, 187);
            this.colorBox.TabIndex = 1;
            this.colorBox.TabStop = false;
            this.colorBox.Text = "Block Color";
            // 
            // colorButton5
            // 
            this.colorButton5.Location = new System.Drawing.Point(7, 150);
            this.colorButton5.Name = "colorButton5";
            this.colorButton5.Size = new System.Drawing.Size(98, 28);
            this.colorButton5.TabIndex = 5;
            this.colorButton5.Text = "Color5";
            this.colorButton5.UseVisualStyleBackColor = true;
            this.colorButton5.Click += new System.EventHandler(this.button5_Click);
            // 
            // colorButton4
            // 
            this.colorButton4.Location = new System.Drawing.Point(7, 117);
            this.colorButton4.Name = "colorButton4";
            this.colorButton4.Size = new System.Drawing.Size(98, 28);
            this.colorButton4.TabIndex = 4;
            this.colorButton4.Text = "Color4";
            this.colorButton4.UseVisualStyleBackColor = true;
            this.colorButton4.Click += new System.EventHandler(this.button4_Click);
            // 
            // colorButton3
            // 
            this.colorButton3.Location = new System.Drawing.Point(7, 84);
            this.colorButton3.Name = "colorButton3";
            this.colorButton3.Size = new System.Drawing.Size(98, 28);
            this.colorButton3.TabIndex = 3;
            this.colorButton3.Text = "Color3";
            this.colorButton3.UseVisualStyleBackColor = true;
            this.colorButton3.Click += new System.EventHandler(this.button3_Click);
            // 
            // colorButton2
            // 
            this.colorButton2.Location = new System.Drawing.Point(7, 51);
            this.colorButton2.Name = "colorButton2";
            this.colorButton2.Size = new System.Drawing.Size(98, 28);
            this.colorButton2.TabIndex = 2;
            this.colorButton2.Text = "Color2";
            this.colorButton2.UseVisualStyleBackColor = true;
            this.colorButton2.Click += new System.EventHandler(this.button2_Click);
            // 
            // colorButton1
            // 
            this.colorButton1.Location = new System.Drawing.Point(7, 18);
            this.colorButton1.Name = "colorButton1";
            this.colorButton1.Size = new System.Drawing.Size(98, 28);
            this.colorButton1.TabIndex = 1;
            this.colorButton1.Text = "Color1";
            this.colorButton1.UseVisualStyleBackColor = true;
            this.colorButton1.Click += new System.EventHandler(this.button1_Click);
            // 
            // rotateBox
            // 
            this.rotateBox.Controls.Add(this.rdoCcw);
            this.rotateBox.Controls.Add(this.rdoCw);
            this.rotateBox.Location = new System.Drawing.Point(136, 11);
            this.rotateBox.Name = "rotateBox";
            this.rotateBox.Size = new System.Drawing.Size(152, 66);
            this.rotateBox.TabIndex = 2;
            this.rotateBox.TabStop = false;
            this.rotateBox.Text = "Rotation";
            // 
            // rdoCcw
            // 
            this.rdoCcw.AutoSize = true;
            this.rdoCcw.Location = new System.Drawing.Point(7, 39);
            this.rdoCcw.Name = "rdoCcw";
            this.rdoCcw.Size = new System.Drawing.Size(130, 16);
            this.rdoCcw.TabIndex = 1;
            this.rdoCcw.TabStop = true;
            this.rdoCcw.Text = "Counter Clockwise";
            this.rdoCcw.UseVisualStyleBackColor = true;
            this.rdoCcw.Click += new System.EventHandler(this.rdoCcw_Clicked);
            // 
            // rdoCw
            // 
            this.rdoCw.AutoSize = true;
            this.rdoCw.Location = new System.Drawing.Point(7, 18);
            this.rdoCw.Name = "rdoCw";
            this.rdoCw.Size = new System.Drawing.Size(82, 16);
            this.rdoCw.TabIndex = 0;
            this.rdoCw.TabStop = true;
            this.rdoCw.Text = "Clockwise";
            this.rdoCw.UseVisualStyleBackColor = true;
            this.rdoCw.Click += new System.EventHandler(this.rdoCw_Clicked);
            // 
            // qualityBox
            // 
            this.qualityBox.Controls.Add(this.fpsBar);
            this.qualityBox.Controls.Add(this.fpsLabel);
            this.qualityBox.Controls.Add(this.checkBox1);
            this.qualityBox.Location = new System.Drawing.Point(141, 86);
            this.qualityBox.Name = "qualityBox";
            this.qualityBox.Size = new System.Drawing.Size(147, 113);
            this.qualityBox.TabIndex = 3;
            this.qualityBox.TabStop = false;
            this.qualityBox.Text = "Quality";
            // 
            // fpsBar
            // 
            this.fpsBar.Location = new System.Drawing.Point(7, 50);
            this.fpsBar.Maximum = 100;
            this.fpsBar.Minimum = 10;
            this.fpsBar.Name = "fpsBar";
            this.fpsBar.Size = new System.Drawing.Size(133, 45);
            this.fpsBar.TabIndex = 3;
            this.fpsBar.TickFrequency = 5;
            this.fpsBar.Value = 60;
            this.fpsBar.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // fpsLabel
            // 
            this.fpsLabel.AutoSize = true;
            this.fpsLabel.Location = new System.Drawing.Point(7, 35);
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(59, 12);
            this.fpsLabel.TabIndex = 2;
            this.fpsLabel.Text = "FPS Limit";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(10, 17);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(113, 16);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Animate Gravity";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // networkBox
            // 
            this.networkBox.Controls.Add(this.rdoHost);
            this.networkBox.Controls.Add(this.rdoGuest);
            this.networkBox.Controls.Add(this.ipLabel);
            this.networkBox.Controls.Add(this.ipText);
            this.networkBox.Controls.Add(this.portLabel);
            this.networkBox.Controls.Add(this.portText);
            this.networkBox.Controls.Add(this.saveButton);
            this.networkBox.Controls.Add(this.resetButton);
            this.networkBox.Location = new System.Drawing.Point(294, 12);
            this.networkBox.Name = "networkBox";
            this.networkBox.Size = new System.Drawing.Size(115, 187);
            this.networkBox.TabIndex = 2;
            this.networkBox.TabStop = false;
            this.networkBox.Text = "Network";
            // 
            // rdoHost
            // 
            this.rdoHost.AutoSize = true;
            this.rdoHost.Location = new System.Drawing.Point(6, 20);
            this.rdoHost.Name = "rdoHost";
            this.rdoHost.Size = new System.Drawing.Size(48, 16);
            this.rdoHost.TabIndex = 1;
            this.rdoHost.TabStop = true;
            this.rdoHost.Text = "Host";
            this.rdoHost.UseVisualStyleBackColor = true;
            // 
            // rdoGuest
            // 
            this.rdoGuest.AutoSize = true;
            this.rdoGuest.Location = new System.Drawing.Point(5, 41);
            this.rdoGuest.Name = "rdoGuest";
            this.rdoGuest.Size = new System.Drawing.Size(56, 16);
            this.rdoGuest.TabIndex = 1;
            this.rdoGuest.TabStop = true;
            this.rdoGuest.Text = "Guest";
            this.rdoGuest.UseVisualStyleBackColor = true;
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Location = new System.Drawing.Point(6, 74);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(16, 12);
            this.ipLabel.TabIndex = 2;
            this.ipLabel.Text = "IP";
            // 
            // ipText
            // 
            this.ipText.Location = new System.Drawing.Point(33, 113);
            this.ipText.Name = "ipText";
            this.ipText.Size = new System.Drawing.Size(73, 21);
            this.ipText.TabIndex = 3;
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(4, 116);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(26, 12);
            this.portLabel.TabIndex = 4;
            this.portLabel.Text = "port";
            // 
            // portText
            // 
            this.portText.Location = new System.Drawing.Point(6, 88);
            this.portText.Name = "portText";
            this.portText.Size = new System.Drawing.Size(100, 21);
            this.portText.TabIndex = 5;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(6, 140);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(50, 23);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler(this.save_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(56, 140);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(50, 23);
            this.resetButton.TabIndex = 7;
            this.resetButton.Text = "Reset";
            this.resetButton.Click += new System.EventHandler(this.reset_Click);
            // 
            // PreferencesDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 210);
            this.Controls.Add(this.networkBox);
            this.Controls.Add(this.qualityBox);
            this.Controls.Add(this.rotateBox);
            this.Controls.Add(this.colorBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreferencesDlg";
            this.Text = "OptionDialog";
            this.Load += new System.EventHandler(this.ColorPicker_Load);
            this.colorBox.ResumeLayout(false);
            this.rotateBox.ResumeLayout(false);
            this.rotateBox.PerformLayout();
            this.qualityBox.ResumeLayout(false);
            this.qualityBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsBar)).EndInit();
            this.networkBox.ResumeLayout(false);
            this.networkBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.GroupBox colorBox;
        private System.Windows.Forms.Button colorButton5;
        private System.Windows.Forms.Button colorButton4;
        private System.Windows.Forms.Button colorButton3;
        private System.Windows.Forms.Button colorButton2;
        private System.Windows.Forms.Button colorButton1;
        private System.Windows.Forms.GroupBox rotateBox;
        private System.Windows.Forms.RadioButton rdoCcw;
        private System.Windows.Forms.RadioButton rdoCw;
        private System.Windows.Forms.GroupBox qualityBox;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label fpsLabel;
        private System.Windows.Forms.TrackBar fpsBar;
        private System.Windows.Forms.GroupBox networkBox;
        private System.Windows.Forms.RadioButton rdoHost;
        private System.Windows.Forms.RadioButton rdoGuest;
        private System.Windows.Forms.Label ipLabel;
        private System.Windows.Forms.TextBox ipText;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.TextBox portText;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button resetButton;
    }
}
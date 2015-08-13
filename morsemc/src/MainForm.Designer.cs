namespace Vurdalakov.MorseMc
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelAvailability = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelCurrent = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelFriendlyName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSendOnce = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSendForever = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAbout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonOptions = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonConvert = new System.Windows.Forms.ToolStripDropDownButton();
            this.textToMorseCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxText = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelAvailability,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabelCurrent,
            this.toolStripStatusLabelFriendlyName});
            this.statusStrip1.Location = new System.Drawing.Point(0, 297);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(642, 24);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelAvailability
            // 
            this.toolStripStatusLabelAvailability.Name = "toolStripStatusLabelAvailability";
            this.toolStripStatusLabelAvailability.Size = new System.Drawing.Size(58, 19);
            this.toolStripStatusLabelAvailability.Text = "Unknown";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(487, 19);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // toolStripStatusLabelCurrent
            // 
            this.toolStripStatusLabelCurrent.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelCurrent.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabelCurrent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabelCurrent.Name = "toolStripStatusLabelCurrent";
            this.toolStripStatusLabelCurrent.Size = new System.Drawing.Size(26, 19);
            this.toolStripStatusLabelCurrent.Text = "---";
            this.toolStripStatusLabelCurrent.Visible = false;
            // 
            // toolStripStatusLabelFriendlyName
            // 
            this.toolStripStatusLabelFriendlyName.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelFriendlyName.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabelFriendlyName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabelFriendlyName.Name = "toolStripStatusLabelFriendlyName";
            this.toolStripStatusLabelFriendlyName.Size = new System.Drawing.Size(82, 19);
            this.toolStripStatusLabelFriendlyName.Text = "Not signed in";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSendOnce,
            this.toolStripButtonSendForever,
            this.toolStripSeparator1,
            this.toolStripButtonCancel,
            this.toolStripButtonAbout,
            this.toolStripSeparator2,
            this.toolStripButtonOptions,
            this.toolStripDropDownButtonConvert});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(642, 38);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonSendOnce
            // 
            this.toolStripButtonSendOnce.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSendOnce.Image")));
            this.toolStripButtonSendOnce.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSendOnce.Name = "toolStripButtonSendOnce";
            this.toolStripButtonSendOnce.Size = new System.Drawing.Size(68, 35);
            this.toolStripButtonSendOnce.Text = "Send Once";
            this.toolStripButtonSendOnce.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonSendOnce.Click += new System.EventHandler(this.toolStripButtonSendOnce_Click);
            // 
            // toolStripButtonSendForever
            // 
            this.toolStripButtonSendForever.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSendForever.Image")));
            this.toolStripButtonSendForever.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSendForever.Name = "toolStripButtonSendForever";
            this.toolStripButtonSendForever.Size = new System.Drawing.Size(79, 35);
            this.toolStripButtonSendForever.Text = "Send Forever";
            this.toolStripButtonSendForever.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonSendForever.Click += new System.EventHandler(this.toolStripButtonSendForever_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 38);
            // 
            // toolStripButtonCancel
            // 
            this.toolStripButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCancel.Image")));
            this.toolStripButtonCancel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonCancel.ImageTransparentColor = System.Drawing.Color.LightGray;
            this.toolStripButtonCancel.Name = "toolStripButtonCancel";
            this.toolStripButtonCancel.Size = new System.Drawing.Size(47, 35);
            this.toolStripButtonCancel.Text = "Cancel";
            this.toolStripButtonCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonCancel.Click += new System.EventHandler(this.toolStripButtonCancel_Click);
            // 
            // toolStripButtonAbout
            // 
            this.toolStripButtonAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonAbout.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAbout.Image")));
            this.toolStripButtonAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAbout.Name = "toolStripButtonAbout";
            this.toolStripButtonAbout.Size = new System.Drawing.Size(44, 35);
            this.toolStripButtonAbout.Text = "About";
            this.toolStripButtonAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonAbout.Click += new System.EventHandler(this.toolStripButtonAbout_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 38);
            // 
            // toolStripButtonOptions
            // 
            this.toolStripButtonOptions.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonOptions.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOptions.Image")));
            this.toolStripButtonOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOptions.Name = "toolStripButtonOptions";
            this.toolStripButtonOptions.Size = new System.Drawing.Size(53, 35);
            this.toolStripButtonOptions.Text = "Options";
            this.toolStripButtonOptions.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonOptions.Click += new System.EventHandler(this.toolStripButtonOptions_Click);
            // 
            // toolStripDropDownButtonConvert
            // 
            this.toolStripDropDownButtonConvert.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripDropDownButtonConvert.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textToMorseCodeToolStripMenuItem,
            this.toolStripMenuItem1,
            this.undoToolStripMenuItem});
            this.toolStripDropDownButtonConvert.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonConvert.Image")));
            this.toolStripDropDownButtonConvert.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonConvert.Name = "toolStripDropDownButtonConvert";
            this.toolStripDropDownButtonConvert.Size = new System.Drawing.Size(62, 35);
            this.toolStripDropDownButtonConvert.Text = "Convert";
            this.toolStripDropDownButtonConvert.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // textToMorseCodeToolStripMenuItem
            // 
            this.textToMorseCodeToolStripMenuItem.Name = "textToMorseCodeToolStripMenuItem";
            this.textToMorseCodeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.textToMorseCodeToolStripMenuItem.Text = "Text To Morse Code";
            this.textToMorseCodeToolStripMenuItem.Click += new System.EventHandler(this.textToMorseCodeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // textBoxText
            // 
            this.textBoxText.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxText.Location = new System.Drawing.Point(0, 38);
            this.textBoxText.Multiline = true;
            this.textBoxText.Name = "textBoxText";
            this.textBoxText.Size = new System.Drawing.Size(642, 259);
            this.textBoxText.TabIndex = 3;
            this.textBoxText.TextChanged += new System.EventHandler(this.textBoxText_TextChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 321);
            this.Controls.Add(this.textBoxText);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Morse MC";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelFriendlyName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelAvailability;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSendOnce;
        private System.Windows.Forms.ToolStripButton toolStripButtonSendForever;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonCancel;
        private System.Windows.Forms.ToolStripButton toolStripButtonAbout;
        private System.Windows.Forms.ToolStripButton toolStripButtonOptions;
        private System.Windows.Forms.TextBox textBoxText;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonConvert;
        private System.Windows.Forms.ToolStripMenuItem textToMorseCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCurrent;
    }
}


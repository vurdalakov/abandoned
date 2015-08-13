namespace Vurdalakov.MorseMc
{
    partial class OptionsForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDownDotDuration = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownUnitDuration = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownDashDuration = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownGapDuration = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLetterSpacing = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWordSpacing = new System.Windows.Forms.NumericUpDown();
            this.buttonReset = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxFont = new System.Windows.Forms.TextBox();
            this.buttonFont = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDotDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUnitDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDashDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGapDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLetterSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWordSpacing)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "The duration of one unit (in ms):";
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(350, 30);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(92, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(350, 70);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(92, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "The duration of a dot (in units):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(159, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "The duration of a dash (in units):";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(18, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(159, 26);
            this.label5.TabIndex = 6;
            this.label5.Text = "The duration of a gap between dots and dashes (in units):";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 162);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(198, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "The space between two letters (in units):";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 189);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(198, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "The space between two words (in units):";
            // 
            // numericUpDownDotDuration
            // 
            this.numericUpDownDotDuration.Location = new System.Drawing.Point(241, 66);
            this.numericUpDownDotDuration.Name = "numericUpDownDotDuration";
            this.numericUpDownDotDuration.Size = new System.Drawing.Size(63, 20);
            this.numericUpDownDotDuration.TabIndex = 3;
            // 
            // numericUpDownUnitDuration
            // 
            this.numericUpDownUnitDuration.Location = new System.Drawing.Point(241, 28);
            this.numericUpDownUnitDuration.Name = "numericUpDownUnitDuration";
            this.numericUpDownUnitDuration.Size = new System.Drawing.Size(63, 20);
            this.numericUpDownUnitDuration.TabIndex = 1;
            // 
            // numericUpDownDashDuration
            // 
            this.numericUpDownDashDuration.Location = new System.Drawing.Point(241, 91);
            this.numericUpDownDashDuration.Name = "numericUpDownDashDuration";
            this.numericUpDownDashDuration.Size = new System.Drawing.Size(63, 20);
            this.numericUpDownDashDuration.TabIndex = 5;
            // 
            // numericUpDownGapDuration
            // 
            this.numericUpDownGapDuration.Location = new System.Drawing.Point(241, 117);
            this.numericUpDownGapDuration.Name = "numericUpDownGapDuration";
            this.numericUpDownGapDuration.Size = new System.Drawing.Size(63, 20);
            this.numericUpDownGapDuration.TabIndex = 7;
            // 
            // numericUpDownLetterSpacing
            // 
            this.numericUpDownLetterSpacing.Location = new System.Drawing.Point(241, 162);
            this.numericUpDownLetterSpacing.Name = "numericUpDownLetterSpacing";
            this.numericUpDownLetterSpacing.Size = new System.Drawing.Size(63, 20);
            this.numericUpDownLetterSpacing.TabIndex = 9;
            // 
            // numericUpDownWordSpacing
            // 
            this.numericUpDownWordSpacing.Location = new System.Drawing.Point(241, 189);
            this.numericUpDownWordSpacing.Name = "numericUpDownWordSpacing";
            this.numericUpDownWordSpacing.Size = new System.Drawing.Size(63, 20);
            this.numericUpDownWordSpacing.TabIndex = 11;
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(352, 149);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(92, 23);
            this.buttonReset.TabIndex = 4;
            this.buttonReset.Text = "Reset to Default";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.numericUpDownWordSpacing);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.numericUpDownLetterSpacing);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.numericUpDownGapDuration);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numericUpDownDashDuration);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.numericUpDownDotDuration);
            this.groupBox1.Controls.Add(this.numericUpDownUnitDuration);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 225);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Generation";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonFont);
            this.groupBox2.Controls.Add(this.textBoxFont);
            this.groupBox2.Location = new System.Drawing.Point(12, 253);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(320, 53);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Font";
            // 
            // textBoxFont
            // 
            this.textBoxFont.Location = new System.Drawing.Point(21, 19);
            this.textBoxFont.Name = "textBoxFont";
            this.textBoxFont.ReadOnly = true;
            this.textBoxFont.Size = new System.Drawing.Size(199, 20);
            this.textBoxFont.TabIndex = 0;
            // 
            // buttonFont
            // 
            this.buttonFont.Location = new System.Drawing.Point(229, 19);
            this.buttonFont.Name = "buttonFont";
            this.buttonFont.Size = new System.Drawing.Size(75, 23);
            this.buttonFont.TabIndex = 1;
            this.buttonFont.Text = "Change...";
            this.buttonFont.UseVisualStyleBackColor = true;
            this.buttonFont.Click += new System.EventHandler(this.buttonFont_Click);
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(456, 323);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDotDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUnitDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDashDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGapDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLetterSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWordSpacing)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericUpDownUnitDuration;
        private System.Windows.Forms.NumericUpDown numericUpDownDotDuration;
        private System.Windows.Forms.NumericUpDown numericUpDownDashDuration;
        private System.Windows.Forms.NumericUpDown numericUpDownGapDuration;
        private System.Windows.Forms.NumericUpDown numericUpDownLetterSpacing;
        private System.Windows.Forms.NumericUpDown numericUpDownWordSpacing;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonFont;
        private System.Windows.Forms.TextBox textBoxFont;
    }
}
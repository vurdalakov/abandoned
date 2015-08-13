using System;
using System.Drawing;
using System.Windows.Forms;

using Vurdalakov.Configuration;

namespace Vurdalakov.MorseMc
{
    public partial class OptionsForm : Form
    {
        private ConfigurationFile configurationFile;

        public OptionsForm(ConfigurationFile configurationFile)
        {
            InitializeComponent();

            this.configurationFile = configurationFile;
        }

        private Font font;
        public Font Font { get { return font; } }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            this.numericUpDownUnitDuration.Minimum = 1;
            this.numericUpDownUnitDuration.Maximum = 10000;
            this.numericUpDownUnitDuration.Value = configurationFile.GetSetting("unitDuration", 250);

            this.numericUpDownDotDuration.Value = configurationFile.GetSetting("dotDuration", 1);
            this.numericUpDownDashDuration.Value = configurationFile.GetSetting("dashDuration", 3);
            this.numericUpDownGapDuration.Value = configurationFile.GetSetting("gapDuration", 1);

            this.numericUpDownLetterSpacing.Value = configurationFile.GetSetting("letterSpacing", 3);
            this.numericUpDownWordSpacing.Value = configurationFile.GetSetting("wordSpacing", 7);

            SetFont(configurationFile.GetSetting("font", "Courier New", 10));
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            configurationFile.SetSetting("unitDuration", this.numericUpDownUnitDuration.Value);

            configurationFile.SetSetting("dotDuration", this.numericUpDownDotDuration.Value);
            configurationFile.SetSetting("dashDuration", this.numericUpDownDashDuration.Value);
            configurationFile.SetSetting("gapDuration", this.numericUpDownGapDuration.Value);

            configurationFile.SetSetting("letterSpacing", this.numericUpDownLetterSpacing.Value);
            configurationFile.SetSetting("wordSpacing", this.numericUpDownWordSpacing.Value);

            configurationFile.SetSetting("font", font);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            this.numericUpDownUnitDuration.Value = 250;

            this.numericUpDownDotDuration.Value = 1;
            this.numericUpDownDashDuration.Value = 3;
            this.numericUpDownGapDuration.Value = 1;

            this.numericUpDownLetterSpacing.Value = 3;
            this.numericUpDownWordSpacing.Value = 7;

            SetFont(new Font("Courier New", 10));
        }

        private void buttonFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            
            fontDialog.Font = font;

            if (DialogResult.OK == fontDialog.ShowDialog(this))
            {
                SetFont(fontDialog.Font);
            }
        }

        private void SetFont(Font font)
        {
            this.font = font;

            this.textBoxFont.Text = String.Format("{0}, {1} pt", font.Name, Math.Round(font.Size));
        }
    }
}

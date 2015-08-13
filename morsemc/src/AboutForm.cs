using System;
using System.Windows.Forms;

namespace Vurdalakov.MorseMc
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            this.labelTitle.Text = Application.ProductName;
            this.labelVersion.Text = "Version " + Application.ProductVersion;
        }

        private void label_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;

            if (label != null)
            {
                System.Diagnostics.Process.Start(label.Text);
            }
        }
    }
}

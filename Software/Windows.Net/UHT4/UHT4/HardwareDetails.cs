using System;
using System.Windows.Forms;

namespace UHT4
{
    public partial class HardwareDetails : Form
    {
        public string l1, l2, l3;
        public HardwareDetails()
        {
            InitializeComponent();
        }

        private void HardwareDetails_Shown(object sender, EventArgs e)
        {
            label1.Text = l1;
            label2.Text = l2;
            label3.Text = l3;
        }
    }
}

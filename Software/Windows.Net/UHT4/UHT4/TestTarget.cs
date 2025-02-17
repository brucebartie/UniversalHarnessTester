using System;
using System.Windows.Forms;

namespace UHT4
{
    public partial class TestTarget : Form
    {
        public int limit;

        public TestTarget()
        {
            InitializeComponent();
        }

        private void TestTarget_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (textBox1.Text == "") textBox1.Text = "-1";
            int i = Convert.ToInt16(textBox1.Text);
            limit = i;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '0' && e.KeyChar != '1' && e.KeyChar != '2' &&
                e.KeyChar != '3' && e.KeyChar != '4' && e.KeyChar != '5' &&
                e.KeyChar != '6' && e.KeyChar != '7' && e.KeyChar != '8' &&
                e.KeyChar != '9' && e.KeyChar != '0') return;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int i = Convert.ToInt16(textBox1.Text);
            }
            catch (Exception)
            {
                textBox1.Text = "";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

using System;
using System.Windows.Forms;

namespace UHT4
{
    public partial class Resistance : Form
    {
        public double r;
        public double rt;

        public Resistance()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void tbResistanceTol_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double t = Double.Parse(tbResistance.Text);
            }
            catch (Exception)
            {
                tbResistance.Text = "100000";
            }
        }

        private void tbResistance_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double t = Double.Parse(tbResistanceTol.Text);
            }
            catch (Exception)
            {
                tbResistanceTol.Text = "10";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            r = Double.Parse(tbResistance.Text);
            rt = Double.Parse(tbResistanceTol.Text);
            this.Close();
        }

        private void Resistance_Shown(object sender, EventArgs e)
        {
            tbResistance.Text = r.ToString();
            tbResistanceTol.Text = rt.ToString();
        }
    }
}

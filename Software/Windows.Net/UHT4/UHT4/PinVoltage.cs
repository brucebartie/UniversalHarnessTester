using System;
using System.Windows.Forms;

namespace UHT4
{
    public partial class PinVoltage : Form
    {
        public double psuVoltage;
        public double dPinVoltage;
        public Boolean bCheckResistance;
        public double iMinResistance;

        public Boolean SetPins = false;
        public UHTForm mainForm;

        public PinVoltage()
        {
            InitializeComponent();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            dPinVoltage = ((double)trackBar1.Value) / 60;
            bCheckResistance = checkBox1.Checked;
            iMinResistance = (int)numericUpDown1.Value;

            label1.Text = "Pin Voltage = " + dPinVoltage.ToString("0.000");
            label2.Text = "Circuit Equivalent Resistance = " + mainForm.GetPinResistance(dPinVoltage).ToString("0.0");
        }

        private void PinVoltage_Shown(object sender, EventArgs e)
        {
            trackBar1.Value = (int)(dPinVoltage)*60;
            checkBox1.Checked = bCheckResistance = checkBox1.Checked;
            numericUpDown1.Value = (int)iMinResistance;

            label1.Text = "Pin Voltage = " + (((double)dPinVoltage) ).ToString("0.000");
            label2.Text = "Circuit Equivalent Resistance = " + mainForm.GetPinResistance( ((double)dPinVoltage)).ToString("0.0");
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            btnSet.Enabled = false;
            SetPins = true;
            bCheckResistance = checkBox1.Checked;
            iMinResistance = (int)numericUpDown1.Value;
            this.Close();
        }
    }
}

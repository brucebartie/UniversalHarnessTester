using System;
using System.Windows.Forms;

namespace UHT4
{
    public partial class HarnessDetails : Form
    {
        public String ClientName;
        public String HarnessName;
        public String HarnessVersion;

        public HarnessDetails()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClientName = tbClientName.Text;
            }
            catch
            {
                ClientName = "";
                tbClientName.Text = "";
            }
            try
            {
                HarnessName = tbHarnessName.Text;
            }
            catch
            {
                HarnessName = "";
                tbHarnessName.Text = "";
            }
            try
            {
                HarnessVersion = tbHarnessVersion.Text;
            }
            catch
            {
                HarnessVersion = "";
                tbHarnessVersion.Text = "";
            }
            this.Refresh();
            if (ClientName != "" && HarnessName != "" && HarnessVersion != "")
                this.Close();
        }

        private void HarnessDetails_Load(object sender, EventArgs e)
        {
            tbClientName.Text = ClientName;
            tbHarnessName.Text = HarnessName;
            tbHarnessVersion.Text = HarnessVersion;
        }
    }
}

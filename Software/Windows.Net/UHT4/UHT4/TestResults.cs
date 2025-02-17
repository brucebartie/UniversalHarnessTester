using System;
using System.Windows.Forms;

namespace UHT4
{
    public partial class TestResults : Form
    {
        public ListView lv;
        public TestResults()
        {
            InitializeComponent();
        }

        private void TestResults_Shown(object sender, EventArgs e)
        {
            for (int t = 0; t < lv.Items.Count; t++)
            {
                String s = lv.Items[t].Text;
                listView1.Items.Add(s);
            }
        }
    }
}

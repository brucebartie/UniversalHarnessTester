using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace UHT4
{
    public partial class TestReport : Form
    {
        //TestData td;

        public TestReport()
        {
            InitializeComponent();
        }

        private void TestReport_Load(object sender, EventArgs e)
        {
            //String result="";
            XmlSerializer s;
            System.IO.FileInfo[] fiA = (new System.IO.DirectoryInfo(Application.StartupPath).GetFiles("TestData*"));
            if (fiA.Length == 0)
            {
            }
            else
            {
                SortFileList(fiA);
                foreach (FileInfo fi in fiA)
                {
                    TreeNode parent = treeView1.Nodes.Add(fi.Name, fi.Name, 0, 0);
                    FileStream fs = File.Open(fi.FullName, FileMode.Open, FileAccess.Read);
                    // Deserialization
                    s = new XmlSerializer(typeof(SerializationTestData));
                    SerializationTestData std = (UHT4.SerializationTestData)s.Deserialize(fs);
                    fs.Close();

                    List<String> ids;
                    ids = new List<String>(0);

                    foreach (TestData td in std.testdata)
                    {
                        if (!ids.Contains(td.id)) ids.Add(td.id);
                    }
                    int i = ids.Count;
                    foreach (String id in ids)
                    {
                        int count_test_per_id = 0;
                        List<String> harness = new List<String>(0);
                        List<Int16> harnesscountpass = new List<Int16>(0);
                        List<Int16> harnesscountfail = new List<Int16>(0);
                        TreeNode child = parent.Nodes.Add("Operator", "Operator " + id.ToString() + " ", 1, 1);
                        foreach (TestData td in std.testdata)
                        {
                            if (td.id == id)
                            {
                                count_test_per_id++;
                                if (harness.Contains(td.HarnessName))
                                {
                                    int j = harness.IndexOf(td.HarnessName);
                                    if (td.passfail)
                                        harnesscountpass[j]++;
                                    else
                                        harnesscountfail[j]++;
                                }
                                else
                                {
                                    harness.Add(td.HarnessName);
                                    harnesscountpass.Add(0);
                                    harnesscountfail.Add(0);
                                    int m = harnesscountpass.Count - 1;

                                    if (td.passfail)
                                        harnesscountpass[m]++;
                                    else
                                        harnesscountfail[m]++;

                                }
                            }
                        }
                        if (harness.Count > 0)
                        {
                            foreach (String h in harness)
                            {
                                child.Nodes.Add("Harness", "Harness ( " + h + " )" + ", Count ( Passed=" + harnesscountpass[harness.IndexOf(h)].ToString() + " Failed=" + harnesscountfail[harness.IndexOf(h)].ToString() + " )", 2, 2);
                            }
                        }
                        int total_passed = 0;
                        int total_failed = 0;
                        foreach (int m in harnesscountpass) total_passed += m;
                        foreach (int m in harnesscountfail) total_failed += m;
                        string n = child.Text;
                        n = n + "( Passed=" + total_passed.ToString() + " Failed=" + total_failed.ToString() + " )";
                        //child.Name =n;
                        child.Text = n;
                    }
                }
            }
            ResizeMe();
        }

        private void SortFileList(System.IO.FileInfo[] fiA)
        {
            Boolean movement = false;
            FileInfo fiA1;
            FileInfo fiA2;
            string a, b;
            string[] datestring1;
            string[] datestring2;
            DateTime DateTime1;
            DateTime DateTime2;
            char[] c = new char[2];
            c[0] = '_';
            c[1] = '.';
            a = "";
            b = "";
            try
            {
                for (int i = 0; i < fiA.Length - 1; i++)
                {
                    a = fiA[i].Name;
                    b = fiA[i + 1].Name;
                    datestring1 = a.Split(c);
                    datestring2 = b.Split(c);
                    DateTime1 = new DateTime(Convert.ToInt16(datestring1[1].Substring(4, 4)), Convert.ToInt16(datestring1[1].Substring(2, 2)), Convert.ToInt16(datestring1[1].Substring(0, 2)));
                    DateTime2 = new DateTime(Convert.ToInt16(datestring2[1].Substring(4, 4)), Convert.ToInt16(datestring2[1].Substring(2, 2)), Convert.ToInt16(datestring2[1].Substring(0, 2)));
                    int uu = DateTime2.CompareTo(DateTime1);
                    if (DateTime2.CompareTo(DateTime1) < 0)
                    {
                        fiA1 = fiA[i];
                        fiA2 = fiA[i + 1];
                        fiA[i] = fiA2;
                        fiA[i + 1] = fiA1;
                        movement = true;
                    }
                }
                if (movement) SortFileList(fiA);
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error sorting the test results. Check that files have the correct naming format. It should be 'TestData_ddmmyyyy.xml' Check the file names and try again. Possible problem files are " + a + " and " + b, "Sorting Error", MessageBoxButtons.OK);
            }
        }

        private void TestReport_Resize(object sender, EventArgs e)
        {
            ResizeMe();
        }
        private void ResizeMe()
        {
            groupBox1.Height = this.Height - 6;
            groupBox1.Width = this.Width - 6;
            treeView1.Width = groupBox1.Width - System.Windows.Forms.SystemInformation.HorizontalScrollBarThumbWidth - 6;
            treeView1.Height = groupBox1.Height - groupBox1.Top - 55;

        }

        private void treeView1_Resize(object sender, EventArgs e)
        {
            ResizeMe();
        }

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
        }

        private void expandAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }

    }
}

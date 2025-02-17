using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UHT4
{
    public partial class OperatorsForm : Form
    {
        public List<Operators> operators = new List<Operators>();
        int selectednode;

        public OperatorsForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Operators o = new Operators();
            o.name = tbOpName.Text;
            o.surname = tbOpSurname.Text;
            o.id = tbOpID.Text;
            if (o.name == "" || o.surname == "" || o.id == "")
            {
                MessageBox.Show("You must complete all 3 fields", "Error", MessageBoxButtons.OK);
                return;
            }
            if (operators == null) operators = new List<Operators>();
            for (int i = 0; i < operators.Count; i++)
            {
                if (operators[i].id == o.id)
                {
                    MessageBox.Show("The ID field must be unique", "Error", MessageBoxButtons.OK);
                    return;
                }
            }
            operators.Add(o);
            button1.Enabled = false;
            button2.Enabled = false;
            RefreshList();
        }

        private void RefreshList()
        {
            treeView1.Nodes.Clear();
            if (operators == null) return;
            foreach (UHT4.Operators oo in operators)
            {
                treeView1.Nodes.Add(oo.name + " " + oo.surname + " ( " + oo.id + " )");
            }
        }

        private void OperatorsForm_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            selectednode = treeView1.GetNodeAt(e.Location).Index;
            if (selectednode != -1)
            {
                tbOpName.Text = operators[selectednode].name;
                tbOpSurname.Text = operators[selectednode].surname;
                tbOpID.Text = operators[selectednode].id;
                button1.Enabled = true;
                button2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selectednode != -1)
            {
                operators[selectednode].name = tbOpName.Text;
                operators[selectednode].surname = tbOpSurname.Text;
                operators[selectednode].id = tbOpID.Text;
                RefreshList();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectednode != -1)
            {
                operators.RemoveAt(selectednode);
                button1.Enabled = false;
                button2.Enabled = false;
                RefreshList();

            }
        }

    }
}

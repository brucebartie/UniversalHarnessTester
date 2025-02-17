using System;
using System.Drawing;
using System.Windows.Forms;
using static UHT4.UHTSerializationInfo;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Drawing.Printing;
using System.Runtime.ConstrainedExecution;
using System.CodeDom;
using BarcodeStandard;

namespace UHT4
{
    public partial class LabelSettings : Form
    {
        public class LabelItemDisplay
        {
            public TextBox text;
            public ComboBox type;
            public TextBox format;
            public CheckBox show;
            public CheckBox bold;
            public NumericUpDown font_size;
            public NumericUpDown line_position;
            public TextBox friendly_name;
        }

        public UHTForm form;
        public UHTSerializationInfo s;
        public List<LabelItemDisplay> item_display;
        public BarcodeStandard.Type type;

    public void PopulateLabelItems()
        {
            form.ser.label_items = new List<LabelItem>();
            
            LabelItem li1 = new LabelItem();
            li1.text = ""; li1.show = false; li1.format = ""; li1.font_size = 9; li1.line_position = 0; li1.friendly_name = "Text1";

            LabelItem li2 = new LabelItem();
            li2.text = ""; li2.show = false; li2.format = ""; li2.font_size = 9; li2.line_position = 1; li2.friendly_name = "Text2";

            LabelItem li3 = new LabelItem();
            li3.text = ""; li3.show = false; li3.format = ""; li3.font_size = 9; li3.line_position = 2; li3.friendly_name = "HarnessName";

            LabelItem li4 = new LabelItem();
            li4.text = ""; li4.show = false; li4.format = ""; li4.font_size = 9; li4.line_position = 3; li4.friendly_name = "HarnessVersion";

            LabelItem li5 = new LabelItem();
            li5.text = ""; li5.show = false; li5.format = ""; li5.font_size = 9; li5.line_position = 4; li5.friendly_name = "ClientName";

            LabelItem li6 = new LabelItem();
            li6.text = ""; li6.show = false; li6.format = "DD/MM/YYY"; li6.font_size = 9; li6.line_position = 5; li6.friendly_name = "Date";

            LabelItem li7 = new LabelItem();
            li7.text = ""; li7.show = false; li7.format = "HH:MM"; li7.font_size = 9; li7.line_position = 6; li7.friendly_name = "Time";

            LabelItem li8 = new LabelItem();
            li8.text = ""; li8.show = false; li8.format = ""; li8.font_size = 9; li8.line_position = 7; li8.friendly_name = "Operator";

            LabelItem li9 = new LabelItem();
            li9.text = ""; li9.show = false; li9.format = ""; li9.font_size = 9; li9.line_position = 8; li9.friendly_name = "";
            
            LabelItem li10 = new LabelItem();
            li10.text = ""; li10.show = false; li10.format = ""; li10.font_size = 9; li10.line_position = 9; li10.friendly_name = "";

            LabelItem li11 = new LabelItem();
            li11.text = ""; li11.show = false; li11.format = ""; li11.font_size = 9; li11.line_position = 10; li11.friendly_name = "";

            form.ser.label_items.Add(li1);
            form.ser.label_items.Add(li2);
            form.ser.label_items.Add(li3);
            form.ser.label_items.Add(li4);
            form.ser.label_items.Add(li5);
            form.ser.label_items.Add(li6);
            form.ser.label_items.Add(li7);
            form.ser.label_items.Add(li8);
            form.ser.label_items.Add(li9);
            form.ser.label_items.Add(li10);
            form.ser.label_items.Add(li11);
        }
        public LabelSettings(UHTForm f )
        {
            InitializeComponent();
            form = f;
            Init();
        }
        public void Init()
        {
            int top = 25;
            int left = 15;
            int index = 0;

            if (form.ser.label_items == null)
                PopulateLabelItems();
            else
            {
                if (form.ser.label_items.Count==0)
                    PopulateLabelItems();
            }


            item_display = new List<LabelItemDisplay>();

            foreach (UHTSerializationInfo.LabelItem li in form.ser.label_items)
            {
                top += 30;

                item_display.Add(new LabelItemDisplay());
                index = item_display.Count - 1;

                item_display[index].text = new TextBox();
                item_display[index].text.Left = left;
                item_display[index].text.Top = top;
                item_display[index].text.Width = 110;
                item_display[index].text.Text = li.text;
                this.groupBox4.Controls.Add(item_display[index].text);

                item_display[index].type = new ComboBox();
                item_display[index].type.Width = 80;
                item_display[index].type.Left = item_display[index].text.Left + item_display[index].text.Width + 10;
                item_display[index].type.Top = top;
                item_display[index].type.Items.Add(Enum.GetName(typeof(LabelItemType), UHTSerializationInfo.LabelItemType.TEXT));
                item_display[index].type.Items.Add(Enum.GetName(typeof(LabelItemType), UHTSerializationInfo.LabelItemType.DATE));
                item_display[index].type.Items.Add(Enum.GetName(typeof(LabelItemType), UHTSerializationInfo.LabelItemType.TIME));
                item_display[index].type.Items.Add(Enum.GetName(typeof(LabelItemType), UHTSerializationInfo.LabelItemType.OPERATOR));
                item_display[index].type.Items.Add(Enum.GetName(typeof(LabelItemType), UHTSerializationInfo.LabelItemType.WEEKCODE));
                item_display[index].type.Items.Add(Enum.GetName(typeof(LabelItemType), UHTSerializationInfo.LabelItemType.BARCODE));
                item_display[index].type.Items.Add(Enum.GetName(typeof(LabelItemType), UHTSerializationInfo.LabelItemType.COUNTER));
                item_display[index].type.SelectedIndex = (int)li.type;
                item_display[index].type.SelectedIndexChanged += Type_SelectedIndexChanged;
                this.groupBox4.Controls.Add(item_display[index].type);

                item_display[index].format = new TextBox();
                item_display[index].format.Width = 100;
                item_display[index].format.Left = item_display[index].type.Left + item_display[index].type.Width + 10;
                item_display[index].format.Top = top;
                item_display[index].format.Text = li.format;
                this.groupBox4.Controls.Add(item_display[index].format);

                item_display[index].show = new CheckBox();
                item_display[index].show.Width = 20;
                item_display[index].show.Left = item_display[index].format.Left + item_display[index].format.Width + 10;
                item_display[index].show.Top = top;
                item_display[index].show.Checked = li.show;
                this.groupBox4.Controls.Add(item_display[index].show);

                item_display[index].font_size = new NumericUpDown();
                item_display[index].font_size.Width = 40;
                item_display[index].font_size.Left = item_display[index].show.Left + item_display[index].show.Width + 10;
                item_display[index].font_size.Top = top;
                item_display[index].font_size.Value = li.font_size;
                this.groupBox4.Controls.Add(item_display[index].font_size);

                item_display[index].line_position = new NumericUpDown();
                item_display[index].line_position.Width = 30;
                item_display[index].line_position.Left = item_display[index].font_size.Left + item_display[index].font_size.Width + 10;
                item_display[index].line_position.Top = top;
                item_display[index].line_position.Value = li.line_position;
                this.groupBox4.Controls.Add(item_display[index].line_position);
            }

            top = 25;

            Label l1 = new Label(); l1.Text = "Text";   l1.Width = 30; l1.Left = left;                                 l1.Top = top; this.groupBox4.Controls.Add(l1);
            Label l2 = new Label(); l2.Text = "Type";   l2.Width = 35; l2.Left = item_display[0].type.Left;            l2.Top = top; this.groupBox4.Controls.Add(l2);
            Label l3 = new Label(); l3.Text = "Format"; l3.Width = 40; l3.Left = item_display[0].format.Left;          l3.Top = top; this.groupBox4.Controls.Add(l3);
            Label l4 = new Label(); l4.Text = "Show";   l4.Width = 35; l4.Left = item_display[0].show.Left-5;          l4.Top = top; this.groupBox4.Controls.Add(l4);
            Label l5 = new Label(); l5.Text = "Font";   l5.Width = 35; l5.Left = item_display[0].font_size.Left;       l5.Top = top; this.groupBox4.Controls.Add(l5);
            Label l6 = new Label(); l6.Text = "Line";   l6.Width = 35; l6.Left = item_display[0].line_position.Left;   l6.Top = top; this.groupBox4.Controls.Add(l6);
        }

        private void Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbx = (ComboBox)sender;
        }

        private void LabelSettings_Load(object sender, EventArgs e)
        {
            nmBarcodeMarginLeft.Value = form.ser.psBarcodeMarginLeft;
            
            for (int i=0;i<comboBox1.Items.Count;i++)
            { 
                if ( (int)GetDropDownType( comboBox1.Items[i].ToString().Trim() ) == form.ser.BarcodeType )
                {
                    comboBox1.SelectedIndex = i;
                }
            }

            numericUpDown2.Value = (decimal)form.ser.psPaperSizeX;
            numericUpDown3.Value = (decimal)form.ser.psPaperSizey;

            numericUpDown4.Value = form.ser.psMarginTop;
            numericUpDown5.Value = form.ser.psMarginLeft;
            numericUpDown6.Value = form.ser.psMarginBottom;
            numericUpDown9.Value = form.ser.psMarginRight;

            nmBarcodeMarginLeft.Value = form.ser.psBarcodeMarginLeft;

            textBox1.Text = form.ser.font_name;
            textBox2.Text = form.ser.font_type;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawNewBarCode();
        }
        private void DrawNewBarCode()
        {
            if (comboBox1.SelectedItem == null) return;
            form.tpBarcode = GetDropDownType(comboBox1.SelectedItem.ToString().Trim());
        }
        public BarcodeStandard.Type GetDropDownType(string s)
        {
            switch (s)
            {
                case "UPC-A": return BarcodeStandard.Type.UpcA; 
                case "UPC-A (Numbered)": return BarcodeStandard.Type.UpcA;
                case "UPC-E": return BarcodeStandard.Type.UpcE;
                case "UPC 2 Digit Ext.": return BarcodeStandard.Type.UpcSupplemental2Digit;
                case "UPC 5 Digit Ext.": return BarcodeStandard.Type.UpcSupplemental5Digit;
                case "EAN-13": return BarcodeStandard.Type.Ean13;
                case "JAN-13": return BarcodeStandard.Type.Jan13;
                case "EAN-8": return BarcodeStandard.Type.Ean8;
                case "ITF-14": return BarcodeStandard.Type.Itf14;
                case "Codabar": return BarcodeStandard.Type.Codabar;
                case "PostNet": return BarcodeStandard.Type.PostNet;
                case "Bookland/ISBN": return BarcodeStandard.Type.Bookland;
                case "Code 11": return BarcodeStandard.Type.Code11;
                case "Code 39": return BarcodeStandard.Type.Code39;
                case "Code 39 Extended": return BarcodeStandard.Type.Code39Extended;
                case "Code 93": return BarcodeStandard.Type.Code93;
                case "LOGMARS": return BarcodeStandard.Type.Logmars;
                case "MSI": return BarcodeStandard.Type.MsiMod10;
                case "Interleaved 2 of 5": return BarcodeStandard.Type.Industrial2Of5;
                case "Standard 2 of 5": return BarcodeStandard.Type.Standard2Of5;
                case "Code 128": return BarcodeStandard.Type.Code128;
                case "Code 128-A": return BarcodeStandard.Type.Code128A;
                case "Code 128-B": return BarcodeStandard.Type.Code128B;
                case "Code 128-C": return BarcodeStandard.Type.Code128C;
                default: return BarcodeStandard.Type.Unspecified;
            }//switch
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            DrawNewBarCode();
        }
        private void LabelSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            PostBackVariables();
        }

        public void PostBackVariables()
        {
            int index = 0;

            if (form.ser.label_items != null)
            {
                foreach (UHTSerializationInfo.LabelItem li in form.ser.label_items)
                {
                    li.text = item_display[index].text.Text;
                    li.type = (UHTSerializationInfo.LabelItemType)item_display[index].type.SelectedIndex;
                    li.format = item_display[index].format.Text;
                    li.show = item_display[index].show.Checked;
                    li.font_size = (int)item_display[index].font_size.Value;
                    li.line_position = (int)item_display[index].line_position.Value;

                    index++;
                }
            }

            try
            {
                form.ser.BarcodeType = (int)GetDropDownType(comboBox1.SelectedItem.ToString().Trim());
            }
            catch (Exception ex) { form.ser.BarcodeType = (int)BarcodeStandard.Type.Code128; }
            form.ser.psBarcodeMarginLeft = (int)nmBarcodeMarginLeft.Value;

            form.ser.psPaperSizeX = (int)numericUpDown2.Value;
            form.ser.psPaperSizey = (int)numericUpDown3.Value;

            form.ser.psMarginTop = (int)numericUpDown4.Value;
            form.ser.psMarginLeft = (int)numericUpDown5.Value;
            form.ser.psMarginBottom = (int)numericUpDown6.Value;
            form.ser.psMarginRight = (int)numericUpDown9.Value;

            form.ser.font_name = textBox1.Text;
            form.ser.font_type = textBox2.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PostBackVariables();
            panel1.Refresh();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            form.printPreviewBody(PrintDocType.PRINT_PANEL, e);
        }

        private void LabelSettings_Resize(object sender, EventArgs e)
        {
            ResizeBody();
        }

        public void ResizeBody()
        {
            groupBox4.Left = 5;
            groupBox4.Top = 10;
            panel1.Left = groupBox4.Left + groupBox4.Width + 10;
            groupBox1.Left = panel1.Left + panel1.Width + 10;
            groupBox3.Left = groupBox1.Left;
            this.Width = groupBox4.Width + panel1.Width + groupBox3.Width + 60;
            button1.Left = panel1.Left;
            button1.Top = panel1.Top + panel1.Height + 10;
            int tempheight = button1.Top + button1.Height +50;
            int tempheight2 = groupBox4.Height+60;
            if (tempheight<tempheight2) 
                this.Height = tempheight2;
            else
                this.Height = tempheight;
                
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            form.ser.psPaperSizeX = (int)numericUpDown2.Value;
            panel1.Width = form.ser.psPaperSizeX;
            ResizeBody();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            form.ser.psPaperSizey = (int)numericUpDown3.Value;
            panel1.Height = form.ser.psPaperSizey;
            ResizeBody();
        }
    }
}

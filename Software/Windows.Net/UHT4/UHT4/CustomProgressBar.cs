using System;
using System.Drawing;
using System.Windows.Forms;

namespace UHT4
{
    public partial class CustomProgressBar : UserControl
    {
        public CustomProgressBar()
        {
            InitializeComponent();
            iValue = 0;
            iMinimum = 0;
            iMaximum = 100;
            cColorOff = Color.LightGreen;
            cColorOn = Color.DarkGreen;

        }

        private int iValue;
        private int iMinimum;
        private int iMaximum;
        private Color cColorOff;
        private Color cColorOn;
        private String sText;
        private Boolean bDrawValueAsLabel;
        private Boolean bMinMax;

        Font fLabel = new Font("Arial", 8, FontStyle.Bold);

        public Boolean DrawValueAsLabel
        {
            set { bDrawValueAsLabel = value; this.Refresh(); }
            get { return bDrawValueAsLabel; }
        }
        public Boolean ShowMinimumAndMaximum
        {
            set { bMinMax = value; this.Refresh(); }
            get { return bMinMax; }
        }
        public String LabelText
        {
            set { sText = value; }
            get { return sText; }
        }

        public Font LabelFont
        {
            set { fLabel = value; this.Refresh(); }
            get { return fLabel; }
        }

        public Color ColorOff
        {
            get { return cColorOff; }
            set { cColorOff = value; this.Refresh(); }
        }

        public Color ColorOn
        {
            get { return cColorOn; }
            set { cColorOn = value; this.Refresh(); }
        }

        public int Minimum
        {
            get { return iMinimum; }
            set { iMinimum = value; this.Refresh(); }
        }
        public int Maximum
        {
            get { return iMaximum; }
            set { iMaximum = value; this.Refresh(); }
        }
        public int Value
        {
            get { return iValue; }
            set
            {
                if (value > iMaximum) value = iMaximum;
                iValue = value;
                this.Refresh();
            }
        }

        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            int width;
            Rectangle inside;
            SizeF s1 = e.Graphics.MeasureString(iMinimum.ToString(), fLabel);
            SizeF s2 = e.Graphics.MeasureString(iMaximum.ToString(), fLabel);
            if (ShowMinimumAndMaximum)
            {
                width = (int)((float)(this.Width - 2 - (int)s1.Width - (int)s2.Width) / ((float)iMaximum - (float)iMinimum) * (float)(iValue));
                inside = new Rectangle(1 + (int)s1.Width, 1, width, this.Height - 2);
                e.Graphics.DrawString(iMinimum.ToString(), fLabel, SystemBrushes.ControlText, 0, this.Height / 2 - s1.Height / 2);
                e.Graphics.DrawString(iMaximum.ToString(), fLabel, SystemBrushes.ControlText, this.Width - s2.Width, this.Height / 2 - s2.Height / 2);
                e.Graphics.DrawRectangle(SystemPens.ControlDarkDark, new Rectangle((int)s1.Width, 0, this.Width - (int)s1.Width - (int)s2.Width, this.Height - 1));
            }
            else
            {
                width = (int)((float)(this.Width - 2) / ((float)iMaximum - (float)iMinimum) * (float)(iValue));
                inside = new Rectangle(1, 1, width, this.Height - 2);
                e.Graphics.DrawRectangle(SystemPens.ControlDarkDark, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            }


            if (width > 0)
            {
                System.Drawing.Drawing2D.LinearGradientBrush lgb = new System.Drawing.Drawing2D.LinearGradientBrush(inside, cColorOff, cColorOn, 0.0);
                e.Graphics.FillRectangle(lgb, inside);
                SizeF s = e.Graphics.MeasureString(sText, fLabel);
                if (bDrawValueAsLabel) sText = iValue.ToString();
                e.Graphics.DrawString(sText, fLabel, SystemBrushes.ControlText, this.Width / 2, this.Height / 2 - s.Height / 2);
            }
            //this.Refresh();
        }
    }
}



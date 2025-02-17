using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UHT4
{
    public partial class HorizontalSplitter : UserControl
    {
        Pen p;
        public HorizontalSplitter()
        {
            InitializeComponent();
            p = new Pen(new SolidBrush(SystemColors.ControlText), 2);
        }

        private void HorizontalSplitter_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.DrawLine(p, new Point(5, this.Height/2-1), new Point(this.Width-5,this.Height/2-1));
        }
    }
}

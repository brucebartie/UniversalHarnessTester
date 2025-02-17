using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace UHT4
{
    public partial class BackGroundImages : Form
    {
        public Boolean bDataFileOpen;
        public UHTForm form1;
        public Panel panelMain;

        public BackGroundImages()
        {
            InitializeComponent();
        }

        public void ReloadBackGroundImages()
        {
            comboBox1.Items.Clear();
            foreach (ImageSerialization imgser in form1.ser.images)
            {
                comboBox1.Items.Add(form1.GetFileNameFromFullPathFileName(imgser.filename));
                comboBox1.SelectedIndex = 0;
            }
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            if (!bDataFileOpen)
            {
                MessageBox.Show("You must first load a harness configuration", "Info");
                return;
            }
            OpenFileDialog fd = new OpenFileDialog();
            fd.ShowDialog();
            try
            {
                if (fd.FileName == "") return;
                FileInfo fi = new FileInfo(fd.FileName);
                PictureBox pb = new PictureBox();
                pb.Image = new Bitmap(fd.FileName);
                if (form1.ser.images == null)
                {
                    form1.ser.images = new List<ImageSerialization>();
                }
                ImageSerialization imgser = new ImageSerialization();
                imgser.filename = fd.FileName;
                imgser.posx = panelMain.Width / 2;
                imgser.posy = panelMain.Height / 2;
                form1.ser.images.Add(imgser);
                ReloadBackGroundImages();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString());  }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = ((ComboBox)sender).SelectedIndex;

            if ( i !=-1)
            {
                pictureBox1.Image = new Bitmap(form1.ser.images[i].filename);
                form1.iSelectedeBackGroundImage = i;
            }
        }

        private void BackGroundImages_Shown(object sender, EventArgs e)
        {
            ReloadBackGroundImages();
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            int i = comboBox1.SelectedIndex;

            if (i != -1)
            {
                form1.ser.images.RemoveAt(i);
                ReloadBackGroundImages();
                panelMain.Invalidate();
            }
        }
    }
}

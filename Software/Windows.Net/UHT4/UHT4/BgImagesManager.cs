using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace UHT4
{
    public class BgImagesManager
    {
        public UHTSerializationAppInfo serApp;
        public List<ImageSerialization> imgser;
        public int image_no = -1;
        public int x_pos;
        public int y_pos;
        public double scale;
        public System.Drawing.Bitmap image;

        public int ImageNo
        {
            set {
                if (imgser != null)
                    {
                    if ( value < (imgser.Count()) )
                        {
                        image_no = value;
                        image = new System.Drawing.Bitmap(imgser[value].filename);
                        x_pos = imgser[value].posx;
                        y_pos = imgser[value].posy;
                        }
                    }
                }
            get { return image_no; }
        }

        public System.Drawing.Bitmap Image
        {
            get
            {
                if (image == null) image = new System.Drawing.Bitmap(serApp.sDataDirectory + "\\default.png");
                return image;
            }
        }

    }
}

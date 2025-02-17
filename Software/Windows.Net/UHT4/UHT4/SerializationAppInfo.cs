using System;
using System.Collections.Generic;

namespace UHT4
{
    public class UHTSerializationAppInfo
    {
        public String sDataDirectory = "c:\\";
        public Boolean ShowWires;
        public Boolean bAppDebug = false;
        public Boolean bAppDebug2 = false;
        public Boolean bSaveTestData = false;
        public List<Operators> operators;
        public int iSplitterPosition = -1;
        public double vscale = 0.0;
        public int width = 1024;
        public int height = 768;
        public int locationx = 0;
        public int locationy = 0;
        public double output_resistor = 1500.0;
        public double input_resistor_top = 4700.0;
        public double input_resistor_bottom = 1500.0 + 45.0;
        public double vdiode = 0.7;
        public double vloss = 1.2;
        public int general_sleep = 3;
        public int data_read_speed =80;
    }
}

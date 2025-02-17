using System;
using System.Collections.Generic;

namespace UHT4
{
    public class UHTSerializationInfo
    {
        public enum LabelItemType { TEXT, DATE, TIME, OPERATOR, WEEKCODE, BARCODE, COUNTER }
        public enum PrintDocType { PRINT_DIALOG, PRINT_PANEL }
        public class LabelItem
        { 
            public string text="";
            public string text2="";
            public string concatenated_text="";
            public LabelItemType type;
            public string format;
            public Boolean show;
            public Boolean bold;
            public int font_size;
            public int line_position;
            public string friendly_name;

            public String GetText() { return text + text2; }
        }

        public int FileVersion = 13;
        public int psPaperSizeX = 250;
        public int psPaperSizey = 400;
        public int psMarginTop = 0;
        public int psMarginBottom = 10;
        public int psMarginLeft = 0;
        public int psMarginRight = 10;
        public int psLabelSpacing = 10;
        public int psBarcodeMarginLeft = 5;
        public int BarcodeType = 22;
        public int iTestTarget = 250;
        public int iResistanceBlockFontSize = 9;
        public double dPinTriggerVoltage = 3.0;
        public double dMinResitanceForAutoPickup = 250.0;

        public int horizontal_bar_top=700;
        public Boolean bCheckResistance = false;
        public Boolean bSaveTestData = false;
        public Boolean bPrintLabelAfterTest = false;
        public Boolean bStopTestOnFirstError = false;
        public Boolean bShowAllErrors = false;

        public string font_name;
        public int font_size;
        public string font_type;

        public List<PinButtonSerialization> pins;
        public List<ImageSerialization> images;
        public List<LabelItem> label_items;

    }
}

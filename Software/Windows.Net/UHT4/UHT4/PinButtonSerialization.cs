using System;
using System.Collections.Generic;

namespace UHT4
{
    public class PinButtonSerialization
    {
        public int sizeX;
        public int SizeY;
        public int SizeMin;
        public int SizeMax;
        public int positionLeft;
        public int positionTop;
        public int pin_no;
        public int rimwidth = 10;
        public int wire_width;
        public List<int> parentIDs;
        public List<int> childIDs;
        public int backgroundtype;
        public int pintype;
        public Int32 colorBackGround;
        public Int32 colorOn;
        public Int32 colorOff;
        public Int32 colorStripe;
        public Int32 colorInput;
        public Int32 colorOutput;
        public Int32 colorPinNo;
        public Int32 colorPinText;
        public Boolean valueOnOff;
        public Boolean showID;
        public Boolean active;
        public Boolean hasChidren;
        public Boolean hasChildren;
        public Boolean bGradients = true;
        public String pinText;
        public String ClientName;
        public String HarnessName;
        public String HarnessVersion;
        public double dPinTriggerVoltage;
        public double dInputVoltageLast;
        public double dResistance;
        public double dResistanceTolerance;
        public Boolean CheckResistance;
    }
}

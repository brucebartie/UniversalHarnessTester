using System;
using System.Collections.Generic;
using System.Drawing;

namespace UHT4
{
    class ButtonPropertyGrid
    {
        String sPinText;
        Color cColorOn;
        Color cColorOff;
        Color cColorBorder;
        Color cColorBar;
        Color cColorBlock;
        Color cColorInput;
        Color cColorOutput;
        Color cColorIDString;
        Color cColorTextString;
        int szFontSize;
        Boolean bGradient;
        PinButton.BackGroundTypes bgType;
        Size szButton;

        PinButton _pb;
        List<PinButton> _pinbuttons;
        UHTSerializationInfo _ser;

        public ButtonPropertyGrid(UHT4.PinButton pb, List<PinButton> pinbuttons, UHTSerializationInfo ser)
        {
            _pb = pb;
            _pinbuttons = pinbuttons;
            _ser = ser;

            sPinText = pb.PinText;
            cColorOn = pb.ColorOn;
            cColorOff = pb.ColorOff;
            cColorBorder = pb.ColorBorder;
            cColorBlock = pb.ColorBlock;
            cColorInput = pb.ColorInputButton;
            cColorOutput = pb.ColorOutputButton;
            cColorIDString = pb.ColorIDString;
            cColorTextString = pb.ColorTextString;
            szFontSize = _ser.iResistanceBlockFontSize;
            bgType = pb.BackGroundType;
            szButton = pb.Size;
        }

        public Size ButtonSize
        {
            get { return szButton; }
            set 
            { 
                szButton = value;
                try
                {
                    foreach (UHT4.PinButton p in _pinbuttons) { p.Size = value; }
                }
                catch (Exception) { }
            }
        }
        public String PinText
        {
            get { return sPinText; }
            set
            {
                sPinText = value;
                _pb.PinText = value;
            }
        }

        public Color ColorOn
        {
            get { return cColorOn; }
            set
            {
                cColorOn = value;
                foreach (PinButton p in _pinbuttons) { p.ColorOn = value; }
            }
        }

        public Color ColorOff
        {
            get { return cColorOff; }
            set
            {
                cColorOff = value;
                foreach (PinButton p in _pinbuttons) { p.ColorOff = value; }
            }
        }

        public Color ColorBorder
        {
            get { return cColorBorder; }
            set
            {
                cColorBorder = value;
                foreach (PinButton p in _pinbuttons) { p.ColorBorder = value; }
            }
        }

        public Color ColorBar
        {
            get { return cColorBar; }
            set
            {
                cColorBar = value;
                foreach (PinButton p in _pinbuttons) { p.ColorBar = value; }
            }
        }

        public Color ColorBlock
        {
            get { return cColorBlock; }
            set
            {
                cColorBlock = value;
                _pb.ColorBlock = value; 
            }
        }

        public Color ColorInput
        {
            get { return cColorInput; }
            set
            {
                cColorInput = value;
                foreach (PinButton p in _pinbuttons) { p.ColorInputButton = value; }
            }
        }

        public Color ColorOutput
        {
            get { return cColorOutput; }
            set
            {
                cColorOutput = value;
                foreach (PinButton p in _pinbuttons) { p.ColorOutputButton = value; }
            }
        }

        public Color ColorIDString
        {
            get { return cColorIDString; }
            set
            {
                cColorIDString = value;
                _pb.ColorIDString = value;
            }
        }

        public Color ColorTextString
        {
            get { return cColorTextString; }
            set
            {
                cColorTextString = value;
                _pb.ColorTextString = value;
            }
        }

        public int ResBlockFontSize
        {
            get { return _ser.iResistanceBlockFontSize; }
            set { _ser.iResistanceBlockFontSize = value; }
        }

        public PinButton.BackGroundTypes BackGroundType
        {
            get { return bgType; }
            set
            {
                bgType = value;
                _pb.BackGroundType = value;
            }
        }

        public Boolean Gradient
        {
            get { return bGradient; }
            set
            {
                bGradient = !value;
                _pb.LinearBrushes = !value;
            }
        }

    }
}

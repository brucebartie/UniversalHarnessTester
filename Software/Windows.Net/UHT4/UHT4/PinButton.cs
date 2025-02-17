using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UHT4
{
    public partial class PinButton : UserControl
    {
        public delegate void ParentIDChangedEventHandler(object sender, EventArgs e);
        public event ParentIDChangedEventHandler ParentIDChanged;
        protected virtual void RaiseParentIDChangedEvent()
        {
            if (ParentIDChanged != null) ParentIDChanged(this, null);
        }

        public delegate void ButtonGotFocusEventHandler(object sender, EventArgs e);
        public event ButtonGotFocusEventHandler ButtonGotFocusChanged;
        protected virtual void RaiseButtonGotFocusChangedEvent()
        {
            if (ButtonGotFocusChanged != null) ButtonGotFocusChanged(this, null);
        }

        public delegate void ButtonLostFocusEventHandler(object sender, EventArgs e);
        public event ButtonLostFocusEventHandler ButtonLostFocusChanged;
        protected virtual void RaiseButtonLostFocusChangedEvent()
        {
            if (ButtonLostFocusChanged != null) ButtonLostFocusChanged(this, null);
        }

        public enum BackGroundTypes { Solid, ForwardLines, BackwardLines, VerticalLines, HorizontalLines };
        public enum IDLabelPosition { Centre, Left, Top, Right, Bottom };
        public enum PinTypes { Output, Input };
        public enum PinTextOrientationType { Top, Left, Right, Bottom };

        private Point pDrawingOffset;
        //private int left;
        //private int top;
        //private int cl_x;
        //private int cl_y;
        private int old_cl_x;
        private int old_cl_y;
        private int sx;
        private int sy;
        private int iControlRimWidth = 10;
        private int iBorderWidth = 1;
        private int iBarWidth = 10;
        private int iID = -1;
        private int iWireWidth = 5;
        private int iNextChildOrParentID;
        private List<int> iParentIDs;
        private List<int> iChildIDs;
        private BackGroundTypes iBackGroundType = BackGroundTypes.Solid;
        private PinTextOrientationType iPinTextOrientation = PinTextOrientationType.Top;
        private PinTypes iPinType;
        //private int ixOrg;
        //private int iyOrg;
        private double inputVoltage;
        private double inputVoltageLast;
        private double inputVoltageTriggerLevel;
        private double dResistance = 1000.0;
        private double dResistanceTolerance = 10;
        private String sPinText;

        Font fLabel = new Font("Arial", 9, FontStyle.Bold);

        private Boolean bValueOnOff = false;
        private Boolean bSelected = false;
        private Boolean bClickSelected = false;
        private Boolean bShowID = true;
        private Boolean bActive = true;
        private Boolean bFail = false;
        private Boolean bParentHasChanged = false;
        //private Boolean bChildHasChanged = false;
        private Boolean bHasChildren = false;
        private Boolean bAutoActivateOnInput = true;
        private Boolean bShowText = false;
        private Boolean bAutoRefresh = true;
        private Boolean bShowLabelBox = true;
        private Boolean bCheckResistance = false;
        private Boolean bLinearBrushes = false;

        private Color cColorOn = Color.LawnGreen;
        private Color cColorOff = Color.Red;
        private Color cColorBorder = Color.Black;
        private Color cColorBar = Color.Yellow;
        private Color cColorBlock = Color.LightGray;
        private Color cColorInputType = Color.Purple;
        private Color cColorOutputType = Color.DarkBlue;
        private Color cColorIDString = Color.Black;
        private Color cColorTextString = Color.White;

        public int GetFirstParentOrChildID()
        {
            iNextChildOrParentID = 0;
            if (ExistsChildIDs())
            {
                return iChildIDs[iNextChildOrParentID];
            }
            if (ExistsParentIDs())
            {
                return iParentIDs[iNextChildOrParentID];
            }
            return -1;
        }
        public int GetNextParentOrChildID()
        {
            if (ExistsChildIDs())
            {
                if (iNextChildOrParentID < iChildIDs.Count - 1)
                {
                    iNextChildOrParentID++;
                    return iChildIDs[iNextChildOrParentID];
                }
                else
                {
                    iNextChildOrParentID = 0;
                    return iChildIDs[iNextChildOrParentID];
                }
            }
            if (ExistsParentIDs())
            {
                if (iNextChildOrParentID < iParentIDs.Count - 1)
                {
                    iNextChildOrParentID++;
                    return iParentIDs[iNextChildOrParentID];
                }
                else
                {
                    iNextChildOrParentID = 0;
                    return iParentIDs[iNextChildOrParentID];
                }
            }
            return -1;
        }
        public Boolean ExistsChildIDs()
        {
            if (iChildIDs == null) return false;
            if (iChildIDs.Count == 0)
                return false;
            else
                return true;
        }
        public Boolean ExistsParentIDs()
        {
            if (iParentIDs == null) return false;
            if (iParentIDs.Count == 0)
                return false;
            else
                return true;
        }
        public Boolean LinearBrushes
        {
            get { return bLinearBrushes; }
            set { bLinearBrushes = value; if (bAutoRefresh) this.Refresh(); }
        }
        public int WireWidth
        {
            get { return iWireWidth; }
            set { iWireWidth = value; }
        }
        public int CentreLocation_X
        {
            set
            {
                this.Left = value - sx / 2;
            }
            get
            {
                return CentreLocation.X;
            }
        }
        public int CentreLocation_Y
        {
            set
            {
                this.Top = this.Top = value - sy / 2 - iControlRimWidth;
            }
            get
            {
                return CentreLocation.Y;
            }
        }
        public Point OldCentreLocation
        {
            get
            {
                return new Point(old_cl_x, old_cl_y);
            }
            set
            {
                old_cl_x = value.X;
                old_cl_y = value.Y;
            }
        }
        public Point CentreLocation
        {
            get
            {
                return new Point(this.Left + sx / 2, this.Top + sy / 2 + iControlRimWidth);
            }
            set
            {
                this.Left = value.X - sx / 2;
                this.Top = value.Y - sy / 2 - iControlRimWidth;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public Boolean AutoRefresh
        {
            get { return bAutoRefresh; }
            set
            {
                bAutoRefresh = value;
                //this.Refresh();
            }
        }
        public Color ColorHalf(Color c)
        {
            return Color.FromArgb(50, c);
        }
        public Boolean Fail
        {
            get { return bFail; }
            set
            {
                bFail = value;
                //if (bAutoRefresh) this.Refresh();
            }
        }
        public Boolean Value
        {
            get { return bValueOnOff; }
            set
            {
                if (value == bValueOnOff) return;
                bValueOnOff = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public Boolean Active
        {
            get { return bActive; }
            set
            {
                if (value == bActive) return;
                bActive = value;
                this.Visible = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public Boolean IsSelected
        {
            get { return bSelected; }
            set { bSelected = value; }
        }
        public Boolean ClickSelect
        {
            get { return bClickSelected; }
            set
            {
                if (value == bClickSelected) return;
                bClickSelected = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public Boolean AutoActivateOnInput
        {
            get { return bAutoActivateOnInput; }
            set
            {
                bAutoActivateOnInput = value;
                if (value) CheckInputValue();
            }
        }
        public Boolean ShowPinText
        {
            get { return bShowText; }
            set { bShowText = value; this.Refresh(); }
        }
        public Boolean ShowLabelBox
        {
            get { return bShowLabelBox; }
            set { bShowLabelBox = value; this.Refresh(); }
        }
        public Boolean CheckResistance
        {
            get { return bCheckResistance; }
            set { bCheckResistance = value; }
        }
        public int ID
        {
            get { return iID; }
            set { iID = value; }
        }
        private void CheckInputValue()
        {
            if (AutoActivateOnInput)
            {
                if (InputVoltage > InputVoltageTriggerLevel)
                {
                    if (Value != true)
                    {
                        Value = true;
                        this.Refresh();
                    }
                    else
                    {
                        Value = true;
                    }
                }
                else
                {
                    if (Value != false)
                    {
                        Value = false;
                        this.Refresh();
                    }
                    else
                    {
                        Value = false;
                    }

                }
            }
        }
        public double InputVoltageTriggerLevel
        {
            get { return inputVoltageTriggerLevel; }
            set
            {
                if (inputVoltageTriggerLevel < 0) inputVoltageTriggerLevel = 0;
                if (inputVoltageTriggerLevel > 3.3) inputVoltageTriggerLevel = 3.3;
                inputVoltageTriggerLevel = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public double InputVoltage
        {
            get { return inputVoltage; }
            set
            {
                if (value == inputVoltage) return;
                inputVoltage = value;
                CheckInputValue();
                //if (bAutoRefresh) this.Refresh();
            }
        }
        public double InputVoltageLast
        {
            get { return inputVoltageLast; }
            set
            {
                inputVoltageLast = value;
            }
        }
        public double Resistance
        {
            get { return dResistance; }
            set
            {
                dResistance = value;
            }
        }
        public double ResistanceTolerance
        {
            get { return dResistanceTolerance; }
            set
            {
                dResistanceTolerance = value;
            }
        }
        public void ClearChildIDs()
        {
            iChildIDs = new List<int>(0);
        }
        public void ClearParentIDs()
        {
            iParentIDs = new List<int>(0);
        }
        public void AddParentID(int value)
        {
            if (iParentIDs == null) iParentIDs = new List<int>(0);
            iParentIDs.Add(value);
            bParentHasChanged = true;
        }
        public void AddChildID(int value)
        {
            if (iChildIDs == null) iChildIDs = new List<int>(0);
            iChildIDs.Add(value);
            //bChildHasChanged = true;
            bHasChildren = true;
        }
        public Boolean Has_Children
        {
            set { bHasChildren = value; }
            get { return bHasChildren; }
        }
        public List<int> ChildIDs
        {
            get { return iChildIDs; }
            set { iChildIDs = value; }
        }
        public List<int> ParentIDs
        {
            get { return iParentIDs; }
            set { iParentIDs = value; }
        }
        public String PinText
        {
            get { return sPinText; }
            set
            {
                sPinText = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public Boolean ShowID
        {
            get { return bShowID; }
            set
            {
                bShowID = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public Color ColorOn
        {
            get { return cColorOn; }
            set
            {
                cColorOn = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public Color ColorOff
        {
            get { return cColorOff; }
            set
            {
                cColorOff = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public Color ColorBorder
        {
            get { return cColorBorder; }
            set
            {
                cColorBorder = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public Color ColorBar
        {
            get { return cColorBar; }
            set
            {
                cColorBar = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public Color ColorBlock
        {
            get { return cColorBlock; }
            set
            {
                cColorBlock = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public Color ColorInputButton
        {
            get { return cColorInputType; }
            set
            {
                cColorInputType = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public Color ColorOutputButton
        {
            get { return cColorOutputType; }
            set
            {
                cColorOutputType = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public Color ColorIDString
        {
            get { return cColorIDString; }
            set
            {
                cColorIDString = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public Color ColorTextString
        {
            get { return cColorTextString; }
            set
            {
                cColorTextString = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public PinTextOrientationType PinTextOrientation
        {
            get { return iPinTextOrientation; }
            set
            {
                iPinTextOrientation = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public BackGroundTypes BackGroundType
        {
            get { return iBackGroundType; }
            set
            {
                iBackGroundType = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public PinTypes PinType
        {
            get { return iPinType; }
            set
            {
                iPinType = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public int BorderWidth
        {
            get { return iBorderWidth; }
            set
            {
                if (value < 1) value = 1;
                if (value > 20) value = 20;
                iBorderWidth = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public int RimWidth
        {
            get { return iControlRimWidth; }
            set
            {
                if (value < 1) value = 1;
                if (value > (this.Size.Width / 2)) value = this.Size.Width / 2;
                iControlRimWidth = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public int BarWidth
        {
            get { return iBarWidth; }
            set
            {
                if (value < 2) value = 2;
                if (value > 50) value = 50;
                iBarWidth = value;
                if (bAutoRefresh) this.Refresh();
            }
        }
        public PinButton()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.GotFocus += new EventHandler(PinButton_GotFocus);
            this.LostFocus += new EventHandler(PinButton_LostFocus);
            //ixOrg = 0;
            //iyOrg = 0;
            //iControlWidth = 200;
            //iControlHeight = 200;
            iControlRimWidth = 20;
            //iControlEdgeWidth =5;
            iPinTextOrientation = PinTextOrientationType.Top;
            sPinText = "None";
            bShowText = true;
            inputVoltageTriggerLevel = 2.5;
            inputVoltageLast = 3.3;
            this.BackColor = Color.FromArgb(10, Color.White);
            //this.BackColor = Color.Gray;
            pDrawingOffset = new Point(0, 0);
        }
        public Boolean Unlink
        {
            set
            {
                if (value == true)
                {
                    ClearParentIDs();
                    ColorBlock = Color.Gray;
                    if (bAutoRefresh) this.Refresh();
                }
            }
        }
        void PinButton_LostFocus(object sender, EventArgs e)
        {
            //if (bAutoRefresh) this.Refresh();
        }
        void PinButton_GotFocus(object sender, EventArgs e)
        {
            //if (bAutoRefresh) this.Refresh();
        }
        private void PinButton_Paint(object sender, PaintEventArgs e)
        {
            if (!bAutoRefresh) return;
            if (!bActive) return;
            if (bParentHasChanged)
            {
                RaiseParentIDChangedEvent();
                bParentHasChanged = false;
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            sx = this.Size.Width;
            sy = this.Size.Height;

            SizeF sz = e.Graphics.MeasureString(PinText, fLabel); // measure the stringt

            // If the label box is show shift the origin
            // If the label bix is not shown reset the origin
            int xorg = 0;
            int yorg = 0;
            if (bShowLabelBox)
            {
                xorg = (int)(sx * 0.05);
                yorg = (int)(sy * 0.05 + sz.Height);
            }

            sx = sx - xorg;
            sy = sy - yorg;
            int x_block = sx * iControlRimWidth / 100;
            int y_block = sy * iControlRimWidth / 100;
            int x_end = sx - 2 * x_block;
            int y_end = sy - 2 * y_block;
            int stripe_width = (int)(sx * BarWidth / 100);

            // Create the pen to draw the inside bars if needed
            Pen colorbarPen = new Pen(ColorBar, stripe_width);
            Pen borderPen = new Pen(ColorBorder, BorderWidth);
            Pen selectedPen = new Pen(Color.Yellow, 2);

            //Create brushes
            SolidBrush stringBrush = new SolidBrush(ColorIDString);
            SolidBrush pinTextBrush = new SolidBrush(Color.Black); // new SolidBrush(ColorTextString);
            SolidBrush pintypeBrush = null;
            LinearGradientBrush BlockBrush;
            LinearGradientBrush OnBrush;
            LinearGradientBrush OffBrush;
            LinearGradientBrush FailBrush;

            if (LinearBrushes)
            {
                BlockBrush = new LinearGradientBrush(new Point(xorg + x_block, yorg + y_block), new Point(xorg + x_block + x_end, yorg + y_block + y_end), ColorBlock, ColorHalf(ColorBlock));
                OnBrush = new LinearGradientBrush(new Point(xorg, yorg), new Point(sx + 20, sy + 20), ColorOn, ColorHalf(ColorOn));
                OffBrush = new LinearGradientBrush(new Point(xorg, yorg), new Point(sx + 20, sy + 20), ColorOff, ColorHalf(ColorOff));
                FailBrush = new LinearGradientBrush(new Point(xorg, yorg), new Point(sx + 20, sy + 20), Color.Yellow, ColorHalf(Color.Yellow));
            }
            else
            {
                BlockBrush = new LinearGradientBrush(new Point(xorg + x_block, yorg + y_block), new Point(xorg + x_block + x_end, yorg + y_block + y_end), ColorBlock, ColorBlock);
                OnBrush = new LinearGradientBrush(new Point(xorg, yorg), new Point(sx + 20, sy + 20), ColorOn, ColorOn);
                OffBrush = new LinearGradientBrush(new Point(xorg, yorg), new Point(sx + 20, sy + 20), ColorOff, ColorOff);
                FailBrush = new LinearGradientBrush(new Point(xorg, yorg), new Point(sx + 20, sy + 20), Color.Yellow, Color.Yellow);
            }

            String ttt = "K";
            if (ExistsChildIDs())
            {
                ttt = "C=";
                for (int i = 0; i < iChildIDs.Count; i++) { ttt = ttt + iChildIDs[i].ToString() + ","; }
            }
            if (ExistsParentIDs())
            {
                ttt = "P=";
                for (int i = 0; i < iParentIDs.Count; i++) { ttt = ttt + iParentIDs[i].ToString() + ","; }
            }

            // Draw the initial bachground box
            if (PinType == PinTypes.Output) { pintypeBrush = new SolidBrush(ColorOutputButton); }
            if (PinType == PinTypes.Input) { pintypeBrush = new SolidBrush(ColorInputButton); }

            if (ShowLabelBox)
            {
                e.Graphics.FillRectangle(pintypeBrush, new Rectangle(0, 0, sx, yorg));
                e.Graphics.FillRectangle(pintypeBrush, new Rectangle(0, 0, xorg, sy));
                if (bShowText)
                {
                    sz = e.Graphics.MeasureString(PinText, fLabel); // measure the stringt
                    //e.Graphics.DrawString(PinText, fLabel, pinTextBrush, new Point(xorg + 5, (int)((yorg - sz.Height) / 2)));
                    e.Graphics.DrawString(PinText, fLabel, pinTextBrush, new Point(0, 0));
                }
            }

            // Draw the color indicating pass or fail
            if (Fail)
                e.Graphics.FillRectangle(FailBrush, new Rectangle(xorg, yorg, sx, sy));
            else
            {
                if (Value) // if button is on or off draw appropriate value
                    e.Graphics.FillRectangle(OnBrush, new Rectangle(xorg, yorg, sx, sy));
                else
                    e.Graphics.FillRectangle(OffBrush, new Rectangle(xorg, yorg, sx, sy));
            }

            // Draw the inside block
            //e.Graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(xorg + x_block, yorg + y_block, x_end, y_end));
            e.Graphics.FillRectangle(BlockBrush, new Rectangle(xorg + x_block, yorg + y_block, x_end, y_end));

            // Begin of type -------------------------------------------------------------------------------
            // If the inside block is forward lines
            if (iBackGroundType == BackGroundTypes.ForwardLines)
            {
                for (int i = x_block + stripe_width; i < (sx + xorg - x_block); i = i + (stripe_width * 2))
                {
                    e.Graphics.DrawLine(colorbarPen, i, sy + yorg - y_block - 5, i + stripe_width, yorg + y_block + 5);
                }
            }
            // If the inside block is backward lines
            if (iBackGroundType == BackGroundTypes.BackwardLines)
            {
                for (int i = x_block + stripe_width; i < (sx + xorg - x_block); i = i + (stripe_width * 2))
                {
                    e.Graphics.DrawLine(colorbarPen, i + stripe_width, sy + yorg - y_block - 5, i, yorg + y_block + 5);
                }
            }
            if (iBackGroundType == BackGroundTypes.VerticalLines)
            {
                for (int i = xorg + x_block + (stripe_width / 2); i < (sx + xorg - x_block); i = i + (stripe_width * 2))
                {
                    e.Graphics.DrawLine(colorbarPen, i, sy + yorg - y_block, i, yorg + y_block);
                }
            }
            if (iBackGroundType == BackGroundTypes.HorizontalLines)
            {
                for (int i = yorg + y_block; i < (sy + yorg - y_block); i = i + (stripe_width * 2))
                {
                    e.Graphics.DrawLine(colorbarPen, xorg + x_block, i + (stripe_width / 2), sx + xorg - x_block, i + (stripe_width / 2));
                }
            }
            // End of type -------------------------------------------------------------------------------

            // Draw the green blanking bars around the middle box
            LinearGradientBrush lgbTemp;
            if (Fail)
            {
                lgbTemp = FailBrush;
            }
            else
            {
                if (Value)
                    lgbTemp = OnBrush;
                else
                    lgbTemp = OffBrush;
            }
            //e.Graphics.FillRectangle(lgbTemp, xorg, yorg, sx, y_block);
            //e.Graphics.FillRectangle(lgbTemp, xorg, yorg, x_block, sy);
            //e.Graphics.FillRectangle(lgbTemp, xorg+sx - x_block, yorg, x_block, sy-y_block);
            //e.Graphics.FillRectangle(lgbTemp, xorg, yorg+sy - y_block, sx, x_block);


            // Draw the 2 border lines on the inside and outside boxes
            //e.Graphics.DrawRectangle(borderPen, new Rectangle(xorg, yorg, sx - BorderWidth, sy - BorderWidth));

            // Top Line
            //e.Graphics.DrawLine(new Pen(SystemBrushes.ControlDark), xorg + 1, yorg + 1, xorg + sx - BorderWidth - 1, yorg + 1);
            // Bottom Line
            //e.Graphics.DrawLine(new Pen(SystemBrushes.ControlLightLight), xorg + 1, yorg + sy - BorderWidth - 1, xorg + sx - BorderWidth - 1, yorg + sy - BorderWidth - 1);
            // Left Line
            //e.Graphics.DrawLine(new Pen(SystemBrushes.ControlDark), xorg + 1, yorg + 1, xorg + 1, yorg + sx - BorderWidth);
            // Right Line
            //e.Graphics.DrawLine(new Pen(SystemBrushes.ControlLight), xorg + sx - BorderWidth - 1, yorg + 1, xorg + sx - BorderWidth - 1, yorg + sy - BorderWidth - 1);

            /*
            e.Graphics.DrawRectangle(borderPen, new Rectangle(
                xorg + x_block,
                yorg + y_block,
                x_end - BorderWidth,
                y_end - BorderWidth));

            // Top Line
            e.Graphics.DrawLine(new Pen(SystemBrushes.ControlDark),
                xorg + x_block + 1,
                yorg + y_block + 1,
                xorg + x_block + x_end - BorderWidth - 1,
                yorg + y_block + 1);

            // Bottom Line
            e.Graphics.DrawLine(new Pen(SystemBrushes.ControlLightLight),
                xorg + x_block + 1,
                yorg + y_block + y_end - BorderWidth - 1,
                xorg + x_end + x_block - BorderWidth - 1,
                yorg + y_block + y_end - BorderWidth - 1);

            // Left Line
            e.Graphics.DrawLine(new Pen(SystemBrushes.ControlDark),
                xorg + x_block + 1,
                yorg + y_block + 1,
                xorg + x_block + 1,
                yorg + y_block + y_end - BorderWidth - 1);

            // Right Line
            e.Graphics.DrawLine(new Pen(SystemBrushes.ControlLight),
                xorg + x_block + x_end - BorderWidth - 1,
                yorg + y_block + 1,
                xorg + x_block + x_end - BorderWidth - 1,
                yorg + y_block + y_end - BorderWidth - 1);
            */

            if (this.Focused) e.Graphics.DrawRectangle(selectedPen, xorg, yorg, sx - 2, sy - 2);
            if (bSelected || bClickSelected) e.Graphics.DrawRectangle(selectedPen, xorg, yorg, sx - 2, sy - 2);

            if (bShowID)
            {
                String t = ID.ToString(); // format string
                sz = e.Graphics.MeasureString(t, fLabel); // measure the stringt
                e.Graphics.DrawString(t, fLabel, stringBrush, new Point(xorg + (int)(sx / 2 - sz.Width / 2), yorg + (int)(sy / 2 - sz.Height / 2)));
                //e.Graphics.DrawString(ttt, fLabel, stringBrush, new Point(xorg + (int)(sx / 2 - sz.Width / 2), yorg + 10 + (int)(sy / 2 - sz.Height / 2)));
            }

            e.Graphics.DrawEllipse(borderPen, new Rectangle(CentreLocation, new Size(15, 15)));

            // Destroy pens and brushes
            selectedPen.Dispose();
            pintypeBrush.Dispose();
            stringBrush.Dispose();
            colorbarPen.Dispose();
            borderPen.Dispose();
            BlockBrush.Dispose();
            OnBrush.Dispose();
            OffBrush.Dispose();
        }
        private void PinButton_MouseEnter(object sender, EventArgs e)
        {
            bSelected = true;
            //if (bAutoRefresh) this.Refresh();
            if (ButtonGotFocusChanged != null) RaiseButtonGotFocusChangedEvent();
        }
        private void PinButton_MouseLeave(object sender, EventArgs e)
        {
            bSelected = false;
            //if (bAutoRefresh) this.Refresh();
            if (ButtonLostFocusChanged != null) RaiseButtonLostFocusChangedEvent();
        }
    }
}

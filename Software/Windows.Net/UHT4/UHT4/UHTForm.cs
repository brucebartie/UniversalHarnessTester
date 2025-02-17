using HIDLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Serialization;
using static UHT4.UHTSerializationInfo;
using BarcodeStandard;

// 24/09 - 2 hours
// 25/09 - 1 hour
// 26/09 - 5 hours
// 26/09 - 1.5 Hours. 
//          Pinbutton updates - bar color. 
//          Setting groupbox color dialogs. 
//          Windows resize code
// 26/09 - 2:40
//          Started initial serialization of pins - holder class, load in and out of holding class
//          Initial load and save - load pins, save pins
//          linked color buttons to pins when clicked
// 28/09 - 3:00
//          Expanded button serialized info
//          Expanded button to draw id's and borders for pintypes
//          Changed button create/new workspace/resize buttons to draw and align
//          Button type and id now set
//          Started menu items for layout view and initial pin selection
// 29/09 - 4:30
//          Epanded pinbuttons to inlcude parent ids
//          Added code to allow initial likning of pin together in a parent child to
//          allow structured testing to happen.
//          This will also allow the parent node to change the child nodes to look like it
//          Added Dialog file open/save
//          Epanded pinbuttons serialized storage
//          Added config area to allow selection of active pins
//          Changed pinbutoon color area to static and added two more colors for pin type
// Dec - 16
// Jan - 16 
// 8-12/02 - 8:00
//          Changed Speed of Application. Added some adjustments
// 15/02 - 3
//          Added some new settings - Stop on Error during testing
//          Added splash screen
//     
// 3/02/12  8:00  Added new reporting and pass/fail tracking   
// 14/4/15  Will add lable specific stuff 

namespace UHT4
{
    public partial class UHTForm : Form
    {
        public class ResistanceBlock
        {
            public int x;
            public int y;
            public double set_r;
            public double r;
            public double r_min;
            public double r_max;
            public Font f;
            public Brush b1;
            public Brush b2;
            public Pen p1;
        }
        public class ParentChangedEventArgs : EventArgs
        {
            public int parent_id;
            public int selected_id;
        }
        public class ButtonSettingsEventArgs : EventArgs
        {
            public int drawtype;
            public Color blockcolor;
            public Color oncolor;
            public Color offcolor;
            public Color stripecolor;
            public Color inputcolor;
            public Color outputcolor;
            public Color stringcolor;
            public String sx;
            public String sy;
            public String ww;
            public String rw;
            public Boolean b;
            public Boolean gr;
        }

        public delegate void ButtonSettingsChangedEventHandler(object sender, ButtonSettingsEventArgs e);
        public event ButtonSettingsChangedEventHandler ButtonSettingsChanged;
        protected virtual void RaiseButtonSettingsChangedEvent(ButtonSettingsEventArgs e)
        {
            if (ButtonSettingsChanged != null) ButtonSettingsChanged(this, e);
        }

        public delegate void PinButtonParentChangedEventHandler(object sender, ParentChangedEventArgs e);
        public event PinButtonParentChangedEventHandler PinButtonParentChanged;
        protected virtual void RaisePinButtonParentChangedEvent(ParentChangedEventArgs e)
        {
            if (PinButtonParentChanged != null) PinButtonParentChanged(this, e);
        }

        static Predicate<bButton> ButtonByDisplayID(int did)
        {
            return delegate (bButton b)
            {
                return b.DisplayID == did;
            };
        }
        static Predicate<bButton> ButtonByID(int id)
        {
            return delegate (bButton b)
            {
                return b.ID == id;
            };
        }
        static Predicate<PinButton> PinButtonByID(int id)
        {
            return delegate (PinButton pb)
            {
                return pb.ID == id;
            };
        }
        static Predicate<PinButton> PinButtonByClickSelectedID()
        {
            return delegate (PinButton pb)
            {
                return pb.ClickSelect = true;
            };
        }

        public class Panel4Values
        {
            public int margin_top;
            public int margin_bottom;
            public int margin_between;
            public int margin_left;
            public int margin_right;
            public int margin_middle;
            public int block_x_spacing;
            public int block_width;
            public int row_space = 0;
            public SizeF szf;
            public Font f;
            public int columns = 2;

            public Panel4Values()
            {
                f = new Font("Arial", 8, FontStyle.Bold);
                szf = new SizeF(10, 10);
                margin_top = 20;
                margin_bottom = 20;
                margin_between = 5;
                margin_left = 50;
                margin_right = 10;
                margin_middle = 10;
                block_x_spacing = 5;
                block_width = 50;
            }

            public void recalc_values(Panel p)
            {
                margin_top = 10;
                margin_bottom = 10;
                margin_between = 3;
                margin_left = 15;
                margin_right = 5;
                margin_middle = 20;
                block_x_spacing = 3;

                block_width = p.Width - margin_left - margin_right - margin_middle * 3 - block_x_spacing * 46;
                block_width = block_width / 48;

                if (p.Height> (block_width*6))
                {
                    block_width = p.Width - margin_left - margin_right - margin_middle * 3 - block_x_spacing * 23;
                    block_width = block_width / 24;
                    columns = 1;
                }
                else
                {
                    columns = 2;
                }

            }
            public void MeasureString(Graphics g)
            {
                szf = g.MeasureString("A", f);
            }
            public int GetPanelHeight()
            {
                return margin_top + (int)szf.Height + block_x_spacing * 2 + block_width * 2 + margin_bottom + 40;
            }
        }

        enum ViewTypes { PinSelection, Layout };
        const Boolean PASS = true;
        const Boolean FAIL = false;
        const int FILE_VERSION = 31; // V3.0
        const int PROGRAM_MAJOR_VERSION = 4;
        const int PROGRAM_MINOR_VERSION = 0;
        const double PSU_V2_GOOD_LL = 10;
        const double PSU_V2_GOOD_UL = 13;
        const double PSU_V1_GOOD_LL = 10;
        const double PSU_V1_GOOD_UL = 13;
        const int GRID_SIZE = 20;

        public UHTSerializationInfo ser = new UHTSerializationInfo();
        public UHTSerializationAppInfo serApp = new UHTSerializationAppInfo();
        SerializationTestData serTestData = new SerializationTestData();

        List<PinButton> pinbuttons = new List<PinButton>();
        List<PinButton> dragareapinbuttons = new List<PinButton>();
        List<PinButtonSerialization> pinbuttonsSerialize = new List<PinButtonSerialization>();

        FileStream TestDataFileStream;

        Point pinbuttonCentreLocation;
        Point pinbuttonMoveStartLocation;
        Boolean bShowPinWires = false;
        Boolean bRedrawWires = true;
        Boolean bPinbuttonDragEnabled = false;
        Boolean bControlKeyOn = false;
        Boolean bReadingData = false;
        Boolean ReadingPSUData = false;
        Boolean bDeviceIsConnected = false;
        Boolean bDeviceIsSetup = false;
        Boolean bGroupDrag = false;
        Boolean bGroupDragStart = false;
        Boolean bEnablePSUTimer = false;
        Boolean bFormDrag = false;
        Boolean FormDragStart = false;
        Boolean bFormAllButtonsSelected = false;
        Boolean bLockPinMove = false;
        Boolean bLockPinChanges = false;
        Boolean bBusyTesting = false;

        Boolean ctrl_key_pressed = false;
        Boolean bPassOrFail = FAIL;
        Boolean bTestComplete = false;
        Boolean bShowLearnButtons = false;
        Boolean bShowLearnProgressBar = true;
        Boolean bDataFileOpen = false;
        Boolean bRegroupButtons = false;
        Boolean bAppDebug = true; // displays debug info if switched on
        Boolean bAppDebug2 = true; // displays debug info if switched on
        Boolean bEnteringText = false;

        PinButton pinbuttonSelected = null;
        PinButton pinbuttonToMove = null;
        PinButton pinbuttonClickSelected = null;

        Boolean bHorizontalBarMove = false;
        Point pHorizontalBar;

        int iResistanceViewType = 0;
        int iConnectorColumns = 12;
        int iUnitsPassed = 0;
        int iUnitsFailed = 0;
        int iGridSize = GRID_SIZE;
        int label_counter;
        public int iSelectedOperator = -1;
        int[] displayPos = {
                            /*
                            24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35,
                            36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47,

                            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11,
                            23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12,  

                            72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
                            84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95,

                            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59,
                            60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71 
                            */

                            12,13,14,15,16,17,18,19,20,21,22,23,
                            36,37,38,39,40,41,42,43,44,45,46,47,
                            1, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10,11,
                            24,25,26,27,28,29,30,31,32,33,34,35,

                            48,49,50,51,52,53,54,55,56,57,58,59,
                            95,94,93,92,91,90,89,88,87,86,85,84,
                            //71,70,69,68,67,66,65,64,63,62,61,60,
                            60,61,62,63,64,65,66,67,68,69,70,71,
                            72,73,74,75,76,77,78,79,80,81,82,83,

                           };

        int update = 0;
        double scale = 5.5; // scale for the new version
        double psuVoltage = 0.0;
        double output_resistor = 1500.0;
        double input_resistor_top = 4700.0;
        double input_resistor_bottom = 1500.0 + 45.0;
        double vdiode = 0.7;
        double vloss = 1.2;

        Point Location_StartDrag;
        Point Location_EndDrag;
        Point Location_MousePos;

        String sLastPath = "";
        String sLastFilename = "";
        String sLastFullname = "";

        List<bButton> panel_input_buttons;
        List<bButton> panel_output_buttons;
        Panel4Values p4v;

        List<double> values = new List<double>();
        public int iSelectedeBackGroundImage = -1;

        HIDLibrary.HidDevice[] HidDeviceList;
        HidDevice HidDevice;
        public BarcodeStandard.Type tpBarcode;
        BarcodeStandard.Barcode barcode = new BarcodeStandard.Barcode();

        // Form functions
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        public UHTForm()
        {
            p4v = new Panel4Values();
            InitializeComponent();

            int count = Process.GetProcessesByName("UHT").Count();
            if (count > 1)
            {
                MessageBox.Show("The application is already running. Close it down and try again.", "Error");
                this.Close();
            }

            label_counter = 0;
            tmrPSU.Enabled = true;
            tmrUSBDevice.Enabled = true;
            tmrPSU.Start();
            tmrUSBDevice.Start();

            panel_input_buttons = new List<bButton>(0);
            panel_output_buttons = new List<bButton>(0);
            for (int i = 0; i < 48; i++)
            {
                bButton b = new bButton();
                b.Size = new Size(p4v.block_width, p4v.block_width);
                b.ID = i;
                b.DisplayID = i;
                b.Text = b.ID.ToString(); // + " " + b.DisplayID.ToString();
                panel_input_buttons.Add(b);
                panelConnectors.Controls.Add(panel_input_buttons[i]);
                b.MouseClick += B_MouseClick_Input;
                b.pEvent += B_pEvent;
                //b.Paint += B_iPaint;
            }
            for (int i = 48; i < 96; i++)
            {

                bButton b = new bButton();
                b.Size = new Size(p4v.block_width, p4v.block_width);
                b.ID = i;
                b.DisplayID = i - 48;
                b.Text = b.ID.ToString(); // + " " + b.DisplayID.ToString();
                panel_output_buttons.Add(b);
                panelConnectors.Controls.Add(panel_output_buttons[i - 48]);
                b.MouseClick += B_MouseClick_Output;
                b.pEvent += B_pEvent;
                //b.Paint += B_oPaint;
            }
            for (int t = 0; t < 48; t++) values.Add(0);

            pinbuttonCentreLocation = new Point(0, 0);

        }
        private void B_pEvent(object sender, EventArgs e)
        {
            if (bAppDebug2) lbDebug.Items.Add("button paint");
        }
        private void B_MouseClick_Output(object sender, MouseEventArgs e)
        {
            bButton b = (bButton)sender;

            int i = b.ID;
            PinButton pb = pinbuttons.Find(PinButtonByID(i));
            if (!pb.Active) pb = null;

            if (pb != null)
            {
                if (ctrl_key_pressed)
                {
                    pb.ClickSelect = true;
                    bGroupDrag = true;

                    if (dragareapinbuttons.Find(PinButtonByID(i)) == null)
                    {
                        dragareapinbuttons.Add(pb);
                        b.State = bButton.state.ON;
                    }
                    else
                    {
                        dragareapinbuttons.Remove(pb);
                        b.State = bButton.state.OFF;
                    }
                }
                else
                {
                    PinGroupAllPinsClearSelectedState();
                    Reset_input_buttons_state(bButton.state.OFF);
                    Reset_output_buttons_state(bButton.state.OFF);
                }
            }
            else
            {
                b.State = bButton.state.INDETERMINATE;
            }

            b.Refresh();
        }
        private void B_MouseClick_Input(object sender, MouseEventArgs e)
        {
            bButton b = (bButton)sender;

            int i = b.ID;
            PinButton pb = pinbuttons.Find(PinButtonByID(i));
            if (!pb.Active) pb = null;

            if (pb != null)
            {
                if (ctrl_key_pressed)
                {
                    pb.ClickSelect = true;
                    bGroupDrag = true;

                    if (dragareapinbuttons.Find(PinButtonByID(i)) == null)
                    {
                        dragareapinbuttons.Add(pb);
                        b.State = bButton.state.ON;
                    }
                    else
                    {
                        dragareapinbuttons.Remove(pb);
                        b.State = bButton.state.OFF;
                    }
                }
                else
                {
                    PinGroupAllPinsClearSelectedState();
                    Reset_input_buttons_state(bButton.state.OFF);
                    Reset_output_buttons_state(bButton.state.OFF);
                }
            }
            else
            {
                b.State = bButton.state.INDETERMINATE;
            }

            b.Refresh();
        }
        private void Form1_Click(object sender, EventArgs e)
        {
            PinGroupAllPinsClearSelectedState();
            pinbuttonSelected = null;
            if (bAppDebug) lblDebug210.Text = "Nothing";
            pinbuttonClickSelected = null;
        }
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (bEnteringText) return;
            if (bTestComplete)
            {
                bTestComplete = false;
                panelMain.Refresh();
            }
            if (e.KeyChar == 'U' || e.KeyChar == 'u')
            {
                if (pinbuttonClickSelected != null)
                {
                    pinbuttonClickSelected.Unlink = true;
                }
            }
            if (e.KeyChar == 't' || e.KeyChar == 'T')
            {
                if (bBusyTesting) return;
                bBusyTesting = true;
                StartTest();
                bBusyTesting = false;
            }
            if (e.KeyChar == 'p' || e.KeyChar == 'P')
            {
                PrintBody();
            }
            if (e.KeyChar == 'G' || e.KeyChar == 'g')
            {
                if (bGroupDragStart)
                {
                    bRegroupButtons = true;
                }
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (!ctrl_key_pressed)
                {
                    ctrl_key_pressed = true;
                    label26.Text = "CTRL Key ON";
                    dragareapinbuttons = new List<PinButton>(0);
                }
            }
            else
            {
                ctrl_key_pressed = false;
                label26.Text = "CTRL Key OFF";
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control)
            {
            ctrl_key_pressed = false;
            label26.Text = "CTRL Key OFF";
            }

            int jump = 10;
            if ((UHTForm.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                jump = 100;
            }


            if (iSelectedeBackGroundImage != -1)
            {
                int i = iSelectedeBackGroundImage;
                if (e.KeyCode == Keys.Left)
                {
                    if (ser.images != null)
                    {
                        if (ser.images.Count >= i)
                        {
                            ser.images[i].posx = ser.images[i].posx - jump;
                            panelMain.Invalidate();
                        }
                    }
                }
                if (e.KeyCode == Keys.Right)
                {
                    if (ser.images != null)
                    {
                        if (ser.images.Count >= i)
                        {
                            ser.images[i].posx = ser.images[i].posx + jump;
                            panelMain.Invalidate();
                        }
                    }
                }
                if (e.KeyCode == Keys.Down)
                {
                    if (ser.images != null)
                    {
                        if (ser.images.Count >= i)
                        {
                            ser.images[i].posy = ser.images[i].posy + jump;
                            panelMain.Invalidate();
                        }
                    }
                }
                if (e.KeyCode == Keys.Up)
                {
                    if (ser.images != null)
                    {
                        if (ser.images.Count >= i)
                        {
                            ser.images[i].posy = ser.images[i].posy - jump;
                            panelMain.Invalidate();
                        }
                    }
                }
            }
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            int count;

            count = Process.GetProcessesByName("UHT").Count();
            if (count > 1)
            {
                MessageBox.Show("The application is already running. Close it down and try again.", "Error");
                this.Close();
            }
            else
            {
                Graphics g = this.CreateGraphics();
                panelConnectorsPaintBody(g);

                LoadAppData();

                if (serApp.bSaveTestData)
                {
                    saveTestDataToolStripMenuItem.Checked = true;
                    OpenTestData();
                }

                if (serApp.ShowWires)
                {
                    bShowPinWires = true;
                    showPinWiresToolStripMenuItem.Checked = true;
                }
                else
                {
                    bShowPinWires = false;
                    showPinWiresToolStripMenuItem.Checked = false;
                }

                lblPgmVersion.Text = "Program Ver : " + PROGRAM_MAJOR_VERSION.ToString() + "." + PROGRAM_MINOR_VERSION.ToString();
                if (ser.dPinTriggerVoltage <= 0) ser.dPinTriggerVoltage = 0.2;


                SetTestingStopsOnFirstError(true);
                this.Size = new Size(this.Width - 1, this.Height - 1);
                this.Size = new Size(this.Width, this.Height);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveAppData();
        }
        public ResistanceBlock GetNewResistanceBlock(int x, int y, double set_r, double r, double r2, double r3, Font f, Brush b_fill, Brush b_text, Pen p)
        {
            ResistanceBlock rb = new ResistanceBlock();
            rb.x = x;
            rb.y = y;
            rb.r_min = r2 * (1 - r3 / 100);
            rb.r_max = r2 * (1 + r3 / 100);
            rb.set_r = set_r;
            rb.r = r;
            rb.f = f;
            rb.b1 = b_fill;
            rb.b2 = b_text;
            rb.p1 = p;
            return rb;
        }
        private void Paint_ResistanceBlock(Graphics g, ResistanceBlock rb)
        {
            if (rb.r >= 0)
            {
                String t_set = rb.set_r.ToString("0");
                String t = rb.r.ToString("0.0");
                String t_min = rb.r_min.ToString("0.0");
                String t_max = rb.r_max.ToString("0.0");
                String t_perc = "";
                String t_comb = t_set + " / " + t;

                SizeF s = new SizeF(1, 1); ;
                SizeF s2 = g.MeasureString(t_min, rb.f);
                SizeF s3 = g.MeasureString(t_max, rb.f);

                float tw2;
                float tw;

                if (rb.set_r < rb.r)
                {
                    t_perc = "+" + ((1.0 - rb.set_r / rb.r) * 100).ToString("0.0") + "%";
                }
                else
                {
                    t_perc = "-" + ((1.0 - rb.r / rb.set_r) * 100).ToString("0.0") + "%";
                }

                switch (iResistanceViewType)
                {
                    case 0: s = g.MeasureString(t_comb, rb.f); break;
                    case 1: s = g.MeasureString(t, rb.f); break;
                }

                tw2 = s.Width;
                tw = tw2;
                tw2 = tw2 / 2;
                float th2 = s.Height / 2;

                if (s2.Width > s.Width) tw2 = s2.Width;
                if (s3.Width > s2.Width) tw2 = s3.Width;

                switch (iResistanceViewType)
                {
                    case 0:
                        g.FillRectangle(rb.b1, new Rectangle((int)(rb.x - tw2), (int)(rb.y - th2 * 2), (int)(tw), (int)(s.Height * 2)));
                        g.DrawRectangle(rb.p1, new Rectangle((int)(rb.x - tw2), (int)(rb.y - th2 * 2), (int)(tw), (int)(s.Height * 2)));
                        g.DrawString(t_comb, rb.f, rb.b2, new Point((int)(rb.x - tw2), (int)(rb.y - (th2 * 3 / 2))));
                        g.DrawString(t_perc, rb.f, rb.b2, new Point((int)(rb.x - tw2), (int)(rb.y)));
                        break;
                    case 1:
                        g.FillRectangle(rb.b1, new Rectangle((int)(rb.x - tw2), (int)(rb.y - th2 * 3), (int)(tw), (int)(s.Height * 3)));
                        g.DrawRectangle(rb.p1, new Rectangle((int)(rb.x - tw2), (int)(rb.y - th2 * 3), (int)(tw), (int)(s.Height * 3)));
                        g.DrawString(t_max, rb.f, rb.b2, new Point((int)(rb.x - tw2), (int)(rb.y - th2 * 3)));
                        g.DrawString(t, rb.f, rb.b2, new Point((int)(rb.x - tw2), (int)(rb.y - th2)));
                        g.DrawString(t_min, rb.f, rb.b2, new Point((int)(rb.x - tw2), (int)(rb.y + th2)));
                        break;
                }
            }
        }
        private void Form1_MaximumSizeChanged(object sender, EventArgs e)
        {
            Form1_ResizeBody(false);
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Form1_ResizeBody(false);
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            Form1_ResizeBody(false);
        }
        private void Form1_ResizeBody(Boolean setup)
        {
            int left = 3;
            p4v.recalc_values(panelConnectors);
            int pch_spacer = this.FindForm().Height - horizontalSplitter1.Top;

            if (bAppDebug) lbDebug.Items.Add("Form1 Resize");

            panelMain.Left = left;
            if (panelMain.Width < 1024)
                panelMain.Height = this.FindForm().Height - pch_spacer - panelInfo.Height - panelTest.Height - panelLearn.Height - toolStrip1.Height - menuStrip1.Height - 5;
            else
                panelMain.Height = this.FindForm().Height - pch_spacer - panelInfo.Height - toolStrip1.Height - menuStrip1.Height - 5;

            if (bAppDebug)
            {
                lbDebug.Visible = true;
                lbDebug2.Visible = true;
                pnlDebug.Visible = true;

                lbDebug2.Left = panelMain.Left + panelMain.Width - lbDebug2.Width - lbDebug.Width - 23;
                lbDebug.Left = lbDebug2.Left + lbDebug2.Width;
                pnlDebug.Left = lbDebug.Left;


                lbDebug.Top = panelMain.Top - 30;
                lbDebug2.Top = panelMain.Top - 30;
                pnlDebug.Top = lbDebug.Top + lbDebug.Height;

                pnlDebug.Height = panelMain.Height - lbDebug.Height - 3;
                panelMain.Width = this.Width - 23;


            }
            else
            {
                lbDebug.Visible = false;
                pnlDebug.Visible = false;
                panelMain.Width = this.Width - 23;
            }

            if (panelMain.Width<1024)
            {
                panelInfo.Left = left;
                panelInfo.Top = panelMain.Top + panelMain.Height + 3;
                panelInfo.Width = panelMain.Width - 20;

                panelTest.Top = panelInfo.Top + panelInfo.Height + 6;
                panelTest.Left = left;
                panelTest.Width = panelMain.Width - 20;

                panelLearn.Top = panelTest.Top + panelTest.Height+6;
                panelLearn.Left = left;
                panelLearn.Width = panelMain.Width - 20;
            }

            else
            {
                panelInfo.Left = left;
                panelInfo.Top = panelMain.Top + panelMain.Height + 3;

                panelTest.Top = panelInfo.Top;
                panelTest.Left = panelInfo.Left + panelInfo.Width + 3;
                panelTest.Width = 350;

                panelLearn.Top = panelInfo.Top;
                panelLearn.Left = panelInfo.Left + panelInfo.Width + 6 + panelTest.Width;
                panelLearn.Width = 330;

            }

            horizontalSplitter1.Left = 5;
            horizontalSplitter1.Width = this.Width - 23;

            panelConnectors.Top = horizontalSplitter1.Top +10 ;
            panelConnectors.Left = left;
            panelConnectors.Width = this.Width - 23;
            panelConnectors.Height = pch_spacer - 55;
            panelConnectors.Invalidate();
        }

        // Panel Main functions
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        private void panelMain_Click(object sender, EventArgs e)
        {
            propertyGrid1.Visible = false;
        }
        private void panelMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (bTestComplete)
            {
                bTestComplete = false;
                panelMain.Refresh();
            }
            if (e.Button == MouseButtons.Left)
            {
                HIDClearOutputs();
                PinGroupClearAllPinsButtonValues();
            }


        }
        private void panelMain_MouseDown(object sender, MouseEventArgs e)
        {
            bFormDrag = true;
            Location_StartDrag = e.Location;
            Location_EndDrag = e.Location;
            bRedrawWires = false;
            panelMain.Invalidate(new Rectangle(Location_StartDrag.X - 5, Location_StartDrag.Y - 5, Math.Abs(Location_StartDrag.X - Location_EndDrag.X) + 10, Math.Abs(Location_StartDrag.Y - Location_EndDrag.Y) + 10));
        }
        private void panelMain_MouseMove(object sender, MouseEventArgs e)
        {
            Location_MousePos = MousePosition;
            if (bAppDebug) lblDebug206.Text = Location_MousePos.ToString();
            if (bFormDrag)
            {
                FormDragStart = true;
                Location_EndDrag = e.Location;
                SelectPinButtonsInDragArea(Location_StartDrag, Location_EndDrag);
                panelMain.Invalidate(new Rectangle(Location_StartDrag.X - 5, Location_StartDrag.Y - 5, Math.Abs(Location_StartDrag.X - Location_EndDrag.X) + 10, Math.Abs(Location_StartDrag.Y - Location_EndDrag.Y) + 10));
                //panelMain.Refresh();
            }
        }
        private void panelMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (bGroupDrag || bGroupDragStart) { PinGroupAllPinsClearSelectedState(); }
            if (bFormDrag && !FormDragStart) { bFormDrag = false; }
            if (bFormDrag && FormDragStart)
            {
                bFormDrag = false;
                FormDragStart = false;
                if (dragareapinbuttons != null)
                {
                    if (dragareapinbuttons.Count >= 1)
                    {
                        bGroupDrag = true;
                        if (bAppDebug) lbl_gd.Text = "True";
                    }
                }
            }
            bRedrawWires = true;
            panelMain.Invalidate(new Rectangle(Location_StartDrag.X - 5, Location_StartDrag.Y - 5, Math.Abs(Location_StartDrag.X - Location_EndDrag.X) + 10, Math.Abs(Location_StartDrag.Y - Location_EndDrag.Y) + 10));
        }
        private void panelMain_Paint(object sender, PaintEventArgs e)
        {
            if (bAppDebug) lbDebug.Items.Add("Panel Main Paint Start" + update.ToString());
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (ser.images != null)
            {
                foreach (ImageSerialization imgser in ser.images)
                {
                    Bitmap bm = new Bitmap(imgser.filename);
                    bm.MakeTransparent();
                    e.Graphics.DrawImage(bm, new Point(imgser.posx, imgser.posy));
                }
            }
            update++;
            if (update == 200)
            {
                System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
            }

            if (update > 500) update = 0;
            if (bAppDebug2) lbDebug.Items.Add("Panel Main Paint " + update.ToString());
            if (bAppDebug2)
            {
                if (bAppDebug2) lblDebug101.Text = bFormAllButtonsSelected.ToString();
                if (bAppDebug2) lblDebug201.Text = Location_StartDrag.ToString();
                if (bAppDebug2) lblDebug202.Text = Location_EndDrag.ToString();
                //if (bAppDebug2) lblDebug204.Text = new Point(ser.offset_x, ser.offset_y).ToString();
                if (bAppDebug2) lblDebug205.Text = pinbuttonCentreLocation.ToString();
            }

            if (FormDragStart)
            {
                e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black), 1),
                    Location_StartDrag.X, Location_StartDrag.Y, Location_EndDrag.X - Location_StartDrag.X, Location_EndDrag.Y - Location_StartDrag.Y);
            }
            if (bTestComplete)
            {
                int fs = (int)(panelMain.Height / 18);
                if (fs < 10) fs = 10;
                if (fs > 60) fs = 70;
                Font f = new Font("Arial", fs, FontStyle.Bold);
                if (bPassOrFail)
                {
                    String s = "Passed";
                    SizeF sf = e.Graphics.MeasureString(s, f);
                    e.Graphics.DrawString(s, f, new SolidBrush(Color.LimeGreen), new PointF(panelMain.Width / 2 - sf.Width, panelMain.Bottom - sf.Height - 50));
                }
                else
                {
                    String s = "Failed";
                    SizeF sf = e.Graphics.MeasureString(s, f);
                    e.Graphics.DrawString(s, f, new SolidBrush(Color.Red), new PointF(panelMain.Width / 2 - sf.Width, panelMain.Bottom - sf.Height - 50));
                }
            }
            SubFunction_DrawPinWires(e);
        }

        // Panel Connectors functions
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        private void panelConnectors_Paint(object sender, PaintEventArgs e)
        {
            panelConnectorsPaintBody(e.Graphics);
        }
        int Getx(int col, int conpos)
        {
            double d=0.5;

            return p4v.margin_left + (int)((double)p4v.margin_middle * d * col) + ((conpos) * (p4v.block_width + p4v.block_x_spacing));
        }
        int Gety(int row)
        {
            return  p4v.margin_top / 2 + (int)p4v.szf.Height + p4v.block_x_spacing + p4v.block_width * row;
        }
        private void panelConnectorsPaintBody(Graphics g)
        {
            p4v.recalc_values(panelConnectors);
            p4v.MeasureString(g);

            if (bAppDebug) lbDebug.Items.Add("Panel Connectors Paint");

            int x = 0;
            int y = 0;
            int i = 0;
            bButton b;

            Font f = new Font("Arial", 5, FontStyle.Regular);
            if (p4v.block_width > 30 && p4v.block_width <= 50) f = new Font("Arial", 7, FontStyle.Regular);
            if (p4v.block_width > 50) f = new Font("Arial", 8, FontStyle.Regular);

            if (bAppDebug) lbDebug.Items.Add("Panel Connectors Paint");
            if (p4v.block_width < 10) p4v.block_width = 10;
            
            for (i = 0; i < 48; i++)
            {
                b = panel_input_buttons.Find(ButtonByDisplayID(i));
                if (i >= 0 && i < 12)
                {
                    x = Getx(1,i+12); // 12 - 24
                    y = Gety(1);

                }
                if (i >= 12 && i < 24)
                {
                    x = Getx(1, 35-i); // 12 - 24
                    y = Gety(0);
                }

                if (i >= 24 && i < 36)
                {
                    x = Getx(0, 35-i); // 0 -12
                    y = Gety(1);
                }
                if (i >= 36 && i < 48) // 0 -12
                {
                    x = Getx(0, 47-i);
                    y = Gety(0);

                }
                b.Left = x; b.Top = y;
                b.Width = p4v.block_width;
                b.Height = p4v.block_width;
                b.Font = f;
            }

            int col = 0;
            int space = 0;
            int row = 0;
            if (p4v.columns == 2)
            {
                col = 24;
                space = 5;
                row = 0;
            }
            else
            {
                col = 0;
                space = 0;
                row = 3;
            }

            for (i = 0; i < 48; i++)
            {
                b = panel_output_buttons.Find(ButtonByDisplayID(i));
                if (i >= 0 && i < 12)
                {
                    x = Getx(space+1, i + 12 + col );  
                    y = Gety(row+1);
                }
                if (i >= 12 && i < 24)
                {
                    x = Getx(space+1, col + 35 - i);
                    y = Gety(row);
                }
                if (i >= 24 && i < 36)
                {
                    x = Getx(space, i -24 + col);
                    y = Gety(row+1);
                }
                if (i >= 36 && i < 48)
                {
                    x = Getx(space, col + 47 - i);
                    y = Gety(row);

                }
                b.Left = x; b.Top = y;
                b.Width = p4v.block_width;
                b.Height = p4v.block_width;
                b.Font = f;
            }

            // Input Pins
            // -----------------------------------------------------------------------------------
            b = panel_input_buttons.Find(ButtonByDisplayID(47));
            g.DrawString("Input", p4v.f, SystemBrushes.WindowText, b.Left , b.Top - 15 );
            // Output Pins
            // -----------------------------------------------------------------------------------
            b = panel_output_buttons.Find(ButtonByDisplayID(47));
            g.DrawString("Output", p4v.f, SystemBrushes.WindowText, b.Left, b.Top - 15);
            f.Dispose();
        }
        private void panelConnectors_MouseClick(object sender, MouseEventArgs e)
        {
            if (!ctrl_key_pressed)
            {
                dragareapinbuttons = new List<PinButton>(0);
                PinGroupAllPinsClearSelectedState();
                Reset_input_buttons_state(bButton.state.OFF);
                Reset_output_buttons_state(bButton.state.OFF);
            }
        }
        private void PanelConnectors_Resize(object sender, EventArgs e)
        {
        }

        // Data Load And Save 
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        private void SetAppDebugStatus(Boolean dbg)
        {
            lbDebug.Visible = dbg;
            lbDebug2.Visible = dbg;

            lblDebug101.Visible = dbg;
            lblDebug102.Visible = dbg;
            lblDebug103.Visible = dbg;
            lblDebug104.Visible = dbg;
            lblDebug105.Visible = dbg;
            lblDebug106.Visible = dbg;
            lblDebug107.Visible = dbg;
            lblDebug108.Visible = dbg;
            lblDebug109.Visible = dbg;
            lblDebug110.Visible = dbg;
            lblDebug111.Visible = dbg;

            lblDebug201.Visible = dbg;
            lblDebug202.Visible = dbg;
            lblDebug204.Visible = dbg;
            lblDebug205.Visible = dbg;
            lblDebug206.Visible = dbg;
            lblDebug207.Visible = dbg;
            lblDebug208.Visible = dbg;
            lblDebug209.Visible = dbg;
            lblDebug210.Visible = dbg;
            lblDebug211.Visible = dbg;

            label23.Visible = dbg;
            label24.Visible = dbg;
            label25.Visible = dbg;
            label26.Visible = dbg;
            lbl_gd.Visible = dbg;
            lbl_gds.Visible = dbg;
            lb_dps.Visible = dbg;
        }
        private void OpenTestData()
        {
            XmlSerializer s;
            String filename = MakeTestDataFileName();
            System.IO.FileInfo[] fiA = (new System.IO.DirectoryInfo(Application.StartupPath).GetFiles(filename));
            if (fiA.Length == 0)
            {
                serTestData = new SerializationTestData();
                serTestData.testdata = new List<TestData>(0);
                SaveTestData();
            }
            else
            {
                TestDataFileStream = File.Open(Application.StartupPath + "\\" + filename, FileMode.Open, FileAccess.Read);
                // Deserialization
                s = new XmlSerializer(typeof(SerializationTestData));
                serTestData = (SerializationTestData)s.Deserialize(TestDataFileStream);
                TestDataFileStream.Close();
                iUnitsFailed = 0;
                iUnitsPassed = 0;
                if (serTestData != null)
                {
                    foreach (TestData td in serTestData.testdata)
                    {
                        if (td.passfail)
                            iUnitsPassed++;
                        else
                            iUnitsFailed++;
                    }

                }
                UpdateTestDataProgressBars();
                //panelMain.Refresh();
            }

        }
        private void AddTestData(Boolean pass_or_fail)
        {
            TestData td = new TestData();
            td.id = serApp.operators[iSelectedOperator].id;
            td.name = serApp.operators[iSelectedOperator].name;
            td.surname = serApp.operators[iSelectedOperator].surname;
            td.testime = DateTime.Now;
            td.passfail = pass_or_fail;
            if (serTestData.testdata == null) serTestData.testdata = new List<TestData>(0);
            serTestData.testdata.Add(td);
        }
        private void SaveTestData()
        {
            if (!ser.bSaveTestData) return; 
            XmlSerializer s;
            try
            {
                String filename = Application.StartupPath + "\\" + MakeTestDataFileName();
                s = new System.Xml.Serialization.XmlSerializer(typeof(SerializationTestData));
                TestDataFileStream = File.Open(filename, FileMode.Create, FileAccess.Write);
                s.Serialize(TestDataFileStream, serTestData);
                TestDataFileStream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadAppData()
        {
            try
            {
                FileStream fds;
                XmlSerializer s;
                // Deserialization
                fds = null;
                s = new XmlSerializer(typeof(UHTSerializationAppInfo));
                String filename = Application.StartupPath + "\\appinfo.xml";
                fds = File.Open(filename, FileMode.Open, FileAccess.Read);
                serApp = (UHTSerializationAppInfo)s.Deserialize(fds);
                fds.Close();

                bAppDebug = serApp.bAppDebug;
                bAppDebug2 = serApp.bAppDebug2;
                output_resistor = (double)serApp.output_resistor;
                input_resistor_top = (double)serApp.input_resistor_top;
                input_resistor_bottom = (double)serApp.input_resistor_bottom;
                vdiode = (double)serApp.vdiode;
                vloss = (double)serApp.vloss;
                scale = serApp.vscale;

                ser.dPinTriggerVoltage = GetPClosestPinVoltageForResistance(ser.dMinResitanceForAutoPickup);
                UpdatePinVoltageAndResistanceLabel();

                SetAppDebugStatus(bAppDebug);

                this.Width= serApp.width;
                this.Height= serApp.height;
                this.Left= serApp.locationx;
                this.Top= serApp.locationy;
                horizontalSplitter1.Top = serApp.iSplitterPosition;

                Form1_ResizeBody(false);

            }
            catch
            {
                MessageBox.Show("Cannot find the application settings file. Please make sure it exists.", "Application Load");
                DrawDefualtPinButtons(true);
            }
        }
        private void SaveAppData()
        {
            FileStream fs;
            XmlSerializer xs;
            String filename = "";
            DataTimers(false);

            serApp.width = this.Width;
            serApp.height = this.Height;
            serApp.locationx = this.Left;
            serApp.locationy = this.Top;
            serApp.iSplitterPosition = horizontalSplitter1.Top;
            serApp.vscale=scale;

            serApp.output_resistor = output_resistor;
            serApp.input_resistor_top = input_resistor_top;
            serApp.input_resistor_bottom = input_resistor_bottom;
            serApp.vdiode = vdiode;
            serApp.vloss = vloss;
            ser.dPinTriggerVoltage = GetPClosestPinVoltageForResistance(ser.dMinResitanceForAutoPickup);

            SetAppDebugStatus(bAppDebug);
            Form1_ResizeBody(false);

            // Serialization
            try
            {
                xs = new System.Xml.Serialization.XmlSerializer(typeof(UHTSerializationAppInfo));
                filename = Application.StartupPath + "\\appinfo.xml";
                fs = File.Open(filename, FileMode.Create, FileAccess.Write);
                xs.Serialize(fs, serApp);
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Application Settings Save Error");
            }
        }
        private void LoadData()
        {
            DataTimers(false);
            RemoveCreatedPinButtonsFromForm();

            if (!bAppDebug)
            {
                if (!bDeviceIsConnected)
                {
                    MessageBox.Show("Test unit is not connected", "Error", MessageBoxButtons.OK);
                    bDataFileOpen = false;
                    return;
                }
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = serApp.sDataDirectory;
            ofd.ShowDialog();
            try
            {
                if (ofd.FileName == "") { bDataFileOpen = false; return; }
                FileInfo fi = new FileInfo(ofd.FileName);
                sLastPath = fi.DirectoryName;
                sLastFilename = fi.Name;
                sLastFullname = fi.FullName;
            }
            catch { bDataFileOpen = false; }
            try
            {
                FileStream fds;
                XmlSerializer s;
                // Deserialization
                fds = null;
                s = new XmlSerializer(typeof(UHTSerializationInfo));
                String filename = ofd.FileName;
                fds = File.Open(filename, FileMode.Open, FileAccess.Read);
                ser = (UHTSerializationInfo)s.Deserialize(fds);

                if (ser.FileVersion >= 11)
                {
                    pinbuttonsSerialize = ser.pins;
                    DeSerializePinButtonsFromCompatibleContainer();
                    char[] cc = new char[2];
                    cc[0] = '\\';
                    cc[1] = '\\';
                    string[] sss = filename.Split(cc);
                    if (sss != null)
                    {
                        lblFileName.Text = "File Name : " + sss[sss.Length - 1];
                        lblHarnessName.Text = lblFileName.Text;
                    }

                    horizontalSplitter1.Top = ser.horizontal_bar_top;
                    tpBarcode = (BarcodeStandard.Type)ser.BarcodeType;

                    if (serApp.operators!=null)
                    {
                        iSelectedOperator = 0;
                        lblOperator.Text = "Operator ID : " + serApp.operators[iSelectedOperator].id;
                    }

                    bDataFileOpen = true;
                    saveDataToolStripMenuItem.Enabled = true;
                    saveAsToolStripMenuItem.Enabled = true;
                    tsbSave.Enabled = true;
                    panelConnectors.Enabled = true;
                    SetTestingStopsOnFirstError(ser.bStopTestOnFirstError);
                    SetTestingShowsAllErrors(ser.bShowAllErrors);

                    DataTimers(true);
                    CreateNewWorkspaceBody();
                    Form1_ResizeBody(false);
                }
                else
                {
                    MessageBox.Show("The file you are trying to load is not compatible with this version", "File Load");
                }


                fds.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("The File you are trying to load is not compatible with this version", "File Load");
                bDataFileOpen = false;
                DrawDefualtPinButtons(true);
            }
        }
        private void SaveData()
        {
            FileStream fs;
            XmlSerializer xs;
            //String filename = "";
            DataTimers(false);
            // Serialization
            try
            {
                xs = new System.Xml.Serialization.XmlSerializer(typeof(UHTSerializationInfo));
                SerializePinButtonsToCompatibleContainer();
                ser.pins = pinbuttonsSerialize;
                fs = File.Open(sLastFullname, FileMode.Create, FileAccess.Write);
                xs.Serialize(fs, ser);
                fs.Close();
                DataTimers(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "File Save Error");
            }
        }
        private void SaveDataAs()
        {
            FileStream fs;
            XmlSerializer xs;
            SaveFileDialog sfd = new SaveFileDialog();
            DataTimers(false);
            sfd.FileName = sLastFullname;
            sfd.Filter = "XML Files(*.xml)|*.xml";
            sfd.InitialDirectory = serApp.sDataDirectory;
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.Cancel) return;
            try
            {
                FileInfo fi = new FileInfo(sfd.FileName);
                if (sfd.FileName == "") return;
                sLastPath = fi.DirectoryName;
                sLastFilename = fi.FullName;
                sLastFullname = fi.FullName;
                if (fi.Extension != ".xml")
                {
                    MessageBox.Show(this, "The file extension must be '.xml'. The file will not be saved. Please try again.", "File Save As Error");
                    return;
                }
            }
            catch { }
            try
            {
                xs = new System.Xml.Serialization.XmlSerializer(typeof(UHTSerializationInfo));
                SerializePinButtonsToCompatibleContainer();
                ser.pins = pinbuttonsSerialize;
                fs = File.Open(sLastFullname, FileMode.Create, FileAccess.Write);
                xs.Serialize(fs, ser);
                fs.Close();
                DataTimers(true);
                bDataFileOpen = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public String GetDateString()
        {
            String dt;
            dt = Pad2DigitString(DateTime.Now.Day.ToString()) + Pad2DigitString(DateTime.Now.Month.ToString()) + Pad2DigitString(DateTime.Now.Year.ToString());
            return dt;
        }
        public String GetTimeString()
        {
            String dt;
            dt = Pad2DigitString(DateTime.Now.Hour.ToString()) + Pad2DigitString(DateTime.Now.Minute.ToString()) + Pad2DigitString(DateTime.Now.Second.ToString());
            return dt;
        }
        private String MakeTestDataFileName()
        {
            String s = "TestData_" + GetDateString() + ".xml";
            return s;
        }
        public void LoadDataBody()
        {
            LoadData();
            OpenTestData();
            UpdateTestDataProgressBars();
        }
        // Create/Test/Learn functions
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        private void tsbTest_Click(object sender, EventArgs e)
        {
            if (bBusyTesting) return;
            bBusyTesting = true;
            StartTest();
            bBusyTesting = false;
        }
        private void StartTest()
        {
            if (bDataFileOpen == false) return;

            int i, j, currentPinID;
            List<int> currentPinParentIDs;
            ListView lv = new ListView();

            if (iSelectedOperator == -1)
            {
                if (serApp.operators == null)
                    MessageBox.Show(this, "You have not created any operators. You must create operators and select one before you can test.", "Test Harness", MessageBoxButtons.OKCancel);
                else
                    MessageBox.Show(this, "You must select and operator before you can test.", "Test Harness", MessageBoxButtons.OKCancel);
                return;
            }

            if (pinbuttons.Count == 0) return;
            if (!CheckPSUVoltage())
                MessageBox.Show(this, "The test unit voltage is to low. You can not learn a new harness. Try to enable it manually.", "Learn New Harness", MessageBoxButtons.OKCancel);

            HIDClearOutputs();
            PinGroupClearAllPinsButtonValues();
            PinGroupClearAllPinsFailValues();
            PinGroupClearAllOutputPinButtonValues();
            PinGroupSetPinsAutoActivateOnInput(false);
            bPassOrFail = PASS;
            progressLearnBar.LabelText = "Testing";
            progressLearnBar.Value = 0;
            progressLearnBar.Minimum = 0;
            progressLearnBar.Maximum = 96;
            progressLearnBar.Visible = true;
            panelLearn.Visible = true;

            // Iterate through all the pin buttons
            for (i = 48; i < 96; i++)
            {
                if (pinbuttons[i].Active)
                {
                    progressLearnBar.Value = progressLearnBar.Value + 2;
                    // Check if the button is active / enabled
                    //if (pinbuttons[i].Active)
                    //{
                    pinbuttons[i].Value = true;
                    currentPinID = pinbuttons[i].ID;
                    currentPinParentIDs = pinbuttons[i].ParentIDs;
                    // Switch on this pin
                    HIDSetOutputPin(currentPinID - 48);
                    System.Threading.Thread.Sleep(serApp.general_sleep);
                    HIDReadData();

                    progressLearnBar.Refresh();

                    // This button is a parent, find its children
                    for (j = 0; j < 48; j++)
                    {
                        if (pinbuttons[j].Active)
                        {
                            // check each pin, if it is the parent, see if it is on or off
                            // if this pin is on - should it be ?
                            if (pinbuttons[j].InputVoltage > ser.dPinTriggerVoltage)
                            {
                                pinbuttons[j].InputVoltageLast = pinbuttons[j].InputVoltage;
                                // this is this pins parent and it is on
                                if (IsaParentOf(pinbuttons[j].ID, pinbuttons[i].ParentIDs))
                                {

                                    if (pinbuttons[j].CheckResistance)
                                    {
                                        double resistance = GetPinResistance(pinbuttons[j].InputVoltageLast);
                                        if (resistance <= (pinbuttons[j].Resistance * (1 + pinbuttons[j].ResistanceTolerance / 100))
                                            && resistance >= (pinbuttons[j].Resistance * (1 - pinbuttons[j].ResistanceTolerance / 100)))
                                        {
                                            pinbuttons[j].Value = true;
                                        }
                                        else
                                        {
                                            lv.Items.Add("Pin " + i.ToString() + " and Pin " + j.ToString() + " resistance is not within limits.");
                                            pinbuttons[j].Value = false;
                                            pinbuttons[i].Value = false;
                                            pinbuttons[j].Fail = true;
                                            pinbuttons[i].Fail = true;
                                            bPassOrFail = FAIL;
                                        }
                                    }
                                    else
                                        pinbuttons[j].Value = true;
                                }
                                // This is this not his pins parent and it is on
                                else
                                {
                                    lv.Items.Add("Pin " + i.ToString() + " and Pin " + j.ToString() + " is connected, and should not be.");
                                    pinbuttons[j].Value = false;
                                    pinbuttons[i].Value = false;
                                    pinbuttons[j].Fail = true;
                                    pinbuttons[i].Fail = true;
                                    bPassOrFail = FAIL;
                                    if (ser.bStopTestOnFirstError)
                                        goto complete;
                                }
                            }
                            // this pin is off - should it be ?
                            else
                            {
                                // this is this pins parent but it is off

                                if (IsaParentOf(pinbuttons[j].ID, pinbuttons[i].ParentIDs))
                                {
                                    lv.Items.Add("Pin " + i.ToString() + " and Pin " + j.ToString() + " is not connected, and should be.");
                                    pinbuttons[j].Value = false;
                                    pinbuttons[i].Value = false;
                                    pinbuttons[j].Fail = true;
                                    pinbuttons[i].Fail = true;
                                    bPassOrFail = FAIL;
                                    if (ser.bStopTestOnFirstError)
                                        goto complete;
                                }
                            }
                        }
                    }
                }
                //}
            }
            complete:
            panelLearn.Visible = false;
            progressLearnBar.Visible = false;

            bTestComplete = true;
            if (bPassOrFail == PASS)
            {
                iUnitsPassed++;
                label_counter++;
                lblLabelCounter.Text = label_counter.ToString();
                HIDHarnessPass();
                if (ser.bPrintLabelAfterTest) PrintBody();
            }
            else
            {
                iUnitsFailed++;
                HIDHarnessFail();
            }

            if (serApp.bSaveTestData)
            {
                AddTestData(bPassOrFail);
                SaveTestData();
            }
            UpdateTestDataProgressBars();

            if (bPassOrFail == FAIL)
            {
                HIDHarnessFail();
                if (ser.bShowAllErrors)
                    if (MessageBox.Show("Do you want to see the test results?", "Test Failed", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    TestResults tr = new TestResults();
                    tr.lv = lv;
                    tr.Show();
                }
            }
            panelMain.Refresh();


        }
        private void CreateNewWorkspaceBody()
        {
            tsbLearn.Enabled = true;
            learnNewHarnessToolStripMenuItem.Enabled = true;
            tsbTest.Enabled = true;
        }
        private void CreateNewWorkspace()
        {
            sLastFullname = "";
            lblHarnessName.Text = "No Current Working File";
            panelInfo.Refresh();
            DataTimers(false);
            RemoveCreatedPinButtonsFromForm();
            DrawDefualtPinButtons(true);
            panelMain.Refresh();
            DataTimers(true);
        }
        private Boolean CheckPSUVoltage()
        {
            if (HidDevice.Attributes.Version == 1)
            {
                if (psuVoltage > PSU_V1_GOOD_LL && psuVoltage < PSU_V1_GOOD_UL) return true;
                for (int i = 0; i < 3; i++)
                {
                    System.Threading.Thread.Sleep(500);
                    HIDPSUOn();
                    System.Threading.Thread.Sleep(500);
                    psuVoltage = ReadPSUData();
                    Application.DoEvents();
                    if (psuVoltage > PSU_V1_GOOD_LL && psuVoltage < PSU_V1_GOOD_UL) return true;
                }
            }

            if (HidDevice.Attributes.Version == 2)
            {
                if (psuVoltage > PSU_V2_GOOD_LL && psuVoltage < PSU_V2_GOOD_UL) return true;
                for (int i = 0; i < 3; i++)
                {
                    System.Threading.Thread.Sleep(500);
                    HIDPSUOn();
                    System.Threading.Thread.Sleep(500);
                    psuVoltage = ReadPSUData();
                    Application.DoEvents();
                    if (psuVoltage > PSU_V2_GOOD_LL && psuVoltage < PSU_V2_GOOD_UL) return true;
                }
            }
            return false;
        }
        private void LearnSetupDisplayStart()
        {
            int i;
            // Setup the way things are displayed while learning
            if (bShowLearnProgressBar)
            {
                for (i = 0; i < 96; i++) pinbuttons[i].AutoRefresh = false;
                progressLearnBar.Visible = true;
                progressLearnBar.ShowMinimumAndMaximum = false;
                progressLearnBar.Minimum = 0;
                progressLearnBar.Maximum = 96;
                progressLearnBar.Value = 0;
                progressLearnBar.LabelText = "Learning";
            }
            if (bShowLearnButtons)
            {
                for (i = 0; i < 96; i++) pinbuttons[i].AutoRefresh = true;
                //progressLearnBar.Visible = false;
            }
            // -----------------------------------------------------------
        }
        private void LearnSetupDisplayEnd()
        {
            int i;
            // Setup the way things are displayed while learning
            if (bShowLearnProgressBar)
            {
                for (i = 0; i < 96; i++) pinbuttons[i].AutoRefresh = true;
                progressLearnBar.Visible = false;
                panelMain.Refresh();
            }
            if (bShowLearnButtons)
            {
            }
            // -------------------------------------------------
        }
        private void LearnAutoColorPins()
        {
            int i;
            int color = 0;
            Color[] c = GetColorList();

            for (i = 0; i<48;i++)
            {
                if (pinbuttons[i].Active)
                {
                    pinbuttons[i].ColorBlock = c[color];
                    if (pinbuttons[i].ChildIDs!=null)
                    {
                        foreach (int ii in pinbuttons[i].ChildIDs)
                        {
                            pinbuttons[ii].ColorBlock = c[color];
                        }    
                    }
                }
                color++;
            }
        }
        private void LearnHarness()
        {
            int i, j;
            double largest_voltage;
            int pin_no;
            int other_connections_found=0;
            double lowest_found=0;
            double highest_found=0;
            double ceiling = 8000;
            //ListView lv1 = new ListView();


            largest_voltage = 0;
            pin_no = -1;

            if (!CheckPSUVoltage())
                MessageBox.Show(this, "The test unit voltage is to low. You can not learn a new harness. Try to enable it manualy.", "Learn New Harness", MessageBoxButtons.OKCancel);

            DialogResult d = MessageBox.Show(this, "You will loose the current workspace. Save it before continuing.", "Learn New Harness", MessageBoxButtons.OKCancel);
            if (d == DialogResult.Cancel) return;

            CreateNewWorkspace();

            PinGroupSetPinTriggerVoltageLevel(ser.dPinTriggerVoltage);
            PinGroupSetCheckResistance(ser.bCheckResistance);

            PinGroupClearAllOutputPinButtonValues();
            PinGroupSetPinsAutoActivateOnInput(false);

            // Iterate through all the pin input buttons. 
            // Count how many output pins are linked to this input
            // If it is more than one then make the input pin the parent of the multiple output pins
            //count = 0;

            LearnSetupDisplayStart();

            progressLearnBar.Minimum = 0;
            progressLearnBar.Maximum = 96;

            // cycle through the output pins and see if it is connected to something
            for (i = 48; i < 96; i++)
            {
                if (bShowLearnProgressBar) progressLearnBar.Value++;
                //panelLearn.Visible = true;

                Application.DoEvents();
                System.Threading.Thread.Sleep(serApp.general_sleep);
                HIDSetOutputPin(i - 48);
                System.Threading.Thread.Sleep(serApp.general_sleep);
                HIDReadData();
                pinbuttons[i].Value = true;
                // cycle through the input pins and see what is connected to it
                for (j = 0; j < 48; j++)
                {
                    pinbuttons[j].Value = true;
                    // this input pin is connected to the output pin
                    if (pinbuttons[j].InputVoltage > largest_voltage)
                    {
                        largest_voltage = pinbuttons[j].InputVoltage;
                        pin_no = j;
                    }
                    double r = 0;
                    r = GetPinResistance(pinbuttons[j].InputVoltage);

                    if (pinbuttons[j].CheckResistance)
                    {
                        if (pinbuttons[j].InputVoltage>ser.dPinTriggerVoltage)
                        {
                            pinbuttons[j].AddChildID(i);
                            pinbuttons[i].AddParentID(j);
                        }
                    }
                    else
                    {
                        if (r < ser.dMinResitanceForAutoPickup)
                        {
                            pinbuttons[j].AddChildID(i);
                            pinbuttons[i].AddParentID(j);
                        }
                        else
                        {
                            if (r < ceiling)
                            {
                                other_connections_found++;
                                //lv1.Items.Add("Pins " + j.ToString() + "/" + i.ToString() + " = " + r.ToString()); ;
                                if (r< highest_found)
                                {
                                    if (r == 0) lowest_found = r;
                                    if (r < lowest_found) lowest_found = r;
                                }
                                if (r > lowest_found)
                                {
                                    if (r<ceiling)
                                        highest_found = r;
                                }
                            }

                        }
                    }
                }
            }

            // Disable all input buttons
            for (i = 0; i < 48; i++)
            {
                pinbuttons[i].Active = false;
            }

            // Go through all output pins, those that have no parents are not used
            for (i = 48; i < 96; i++)
            {
                if (bShowLearnProgressBar) progressLearnBar.Value++;
                Application.DoEvents();
                System.Threading.Thread.Sleep(serApp.general_sleep);
                if (pinbuttons[i].ParentIDs == null)
                    pinbuttons[i].Active = false;
                else
                {
                    if (pinbuttons[i].ParentIDs != null)
                    {
                        foreach (int id in pinbuttons[i].ParentIDs)
                        {
                            pinbuttons[id].Active = true;
                            UpdatePinsAndVoltages(pinbuttons[id]);
                            double r = GetPinResistance(pinbuttons[id].InputVoltage);
                            if (r > ser.dMinResitanceForAutoPickup)
                            {
                                pinbuttons[id].Resistance = r;
                                pinbuttons[id].CheckResistance = true;
                            }
                        }
                    }
                }

            }

            int active_pins_found = 0;
            for (i = 48; i < 96; i++)
            {
                if (pinbuttons[i].ParentIDs != null) active_pins_found++;
            }

            LearnSetupDisplayEnd();
            if (active_pins_found == 0)
            {
                MessageBox.Show(this, "Learn Done. There were no connections found.", "Learn New Harness", MessageBoxButtons.OK);
            }
            else
            {
                //TestResults tr = new TestResults();
                //tr.lv = lv1;
                //tr.ShowDialog();
                if (other_connections_found > 0) {
                    if (!pinbuttons[0].CheckResistance) {
                        MessageBox.Show("Learn Done\n" +
                                        "You were checking for continuity , but " + other_connections_found.ToString() +  " other connections were found.\n" +
                                        "The higest value was " + highest_found.ToString("0.0") + "ohms and the lowest was " + lowest_found.ToString("0.0") + "ohms.\n" +
                                        "Check your harness or test with only resistance values.\n" +
                                        "Alternatively you can save and continue as is.", "Notification", MessageBoxButtons.OK);
                    } }

                MessageBox.Show(this, "Learn Done. Highest Voltage V=" + largest_voltage.ToString("0.000") + "was on pin no=" + pin_no.ToString(), "Learn New Harness", MessageBoxButtons.OK);
                MessageBox.Show(this, "Learn Done. Test the configuration to check the results", "Learn New Harness", MessageBoxButtons.OK);
                d = MessageBox.Show(this, "Do you want to auto color the pins?", "Learn New Harness", MessageBoxButtons.OKCancel);
                if (d == DialogResult.Cancel) return;
                LearnAutoColorPins();
            }
        }
        private void SetTestingStopsOnFirstError( Boolean b)
        {
            testingStopsOn1stErrorToolStripMenuItem.Checked = b;
            ser.bStopTestOnFirstError = b;
            SaveAppData();
        }
        private void SetTestingShowsAllErrors( Boolean b )
        {
            testingShowsAllErrorsToolStripMenuItem.Checked = b;
            ser.bShowAllErrors = b;
            SaveAppData();
        }
        private void SetPrintLAbelAfterTesting(Boolean b)
        {
            tsPrintLabelAfterTest.Checked = b;
            ser.bPrintLabelAfterTest = b;
            SaveAppData();
        }

        // Form menu functions
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        private void saveDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bDataFileOpen)
                SaveData();
            else
                SaveDataAs();
            lblHarnessName.Text = sLastFullname;
            panelInfo.Refresh();
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDataAs();
            lblHarnessName.Text = sLastFullname;
            panelInfo.Refresh();
        }
        private void loadDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDataBody();
        }
        private void tsbLoad_Click(object sender, EventArgs e)
        {
            LoadDataBody();
        }
        public String Pad2DigitString(String s)
        {
            if (s.Length == 1) s = "0" + s;
            return s;
        }
        private void createNewWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewWorkspace();
            CreateNewWorkspaceBody();
        }
        private void psuOnOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pSUOnOffToolStripMenuItem.Checked)
            {
                psuOffToolStripMenuItemBody();
            }
            else
            {
                psuOnToolStripMenuItemBody();
            }
        }
        private void psuOffToolStripMenuItemBody()
        {
            DataTimers(false);
            HIDPSUOff();
            pSUOnOffToolStripMenuItem.Checked = false;
            DataTimers(true);
        }
        private void psuOnToolStripMenuItemBody()
        {
            DataTimers(false);
            HIDPSUOn();
            pSUOnOffToolStripMenuItem.Checked = true;
            DataTimers(true);
        }
        private void resizeButtonsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawDefualtPinButtons(false);
            panelMain.Refresh();
        }
        private void tsbNewWorkspace_Click(object sender, EventArgs e)
        {
            bTestComplete = false;
            bDataFileOpen = false;
            panelMain.Refresh();
            CreateNewWorkspace();
            CreateNewWorkspaceBody();
            panelConnectors.Enabled = true;
        }
        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (bDataFileOpen)
                SaveData();
            else
                SaveDataAs();
            lblHarnessName.Text = sLastFullname;
            panelInfo.Refresh();
        }
        private void tsbLearn_Click(object sender, EventArgs e)
        {
            Boolean a, b;

            panelLearn.Visible = true;

            a = bShowPinWires;
            b = bRedrawWires;
            bShowPinWires = false;
            bRedrawWires = false;
            bTestComplete = false;
            bDataFileOpen = false;
            LearnHarness();
            bShowPinWires = a;
            bRedrawWires = b;
            saveAsToolStripMenuItem.Enabled = true;
            saveDataToolStripMenuItem.Enabled = true;
            tsbSave.Enabled = true;

            panelLearn.Visible = false;

            panelMain.Refresh();
        }
        private void tsPrintLabelAfterTest_Click(object sender, EventArgs e)
        {
            if (ser.bPrintLabelAfterTest)
                SetPrintLAbelAfterTesting(false);
            else
                SetPrintLAbelAfterTesting(true);
        }
        private void movePinsBackOnWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (UHT4.PinButton p in pinbuttons)
            {
                if (p.Location.X <= 0) p.Location = new Point(100, p.Location.Y);
                if (p.Location.Y <= 0) p.Location = new Point(p.Location.X, 100);
                if (p.Location.X >= this.Width) new Point(100, p.Location.Y);
                if (p.Location.Y >= this.Height) new Point(p.Location.X, 100);

            }
        }
        private void lockPinMoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bLockPinMove)
            {
                bLockPinMove = false;
                lockPinMoveToolStripMenuItem.Checked = false;
            }
            else
            {
                bLockPinMove = true;
                lockPinMoveToolStripMenuItem.Checked = true;
            }
        }
        private void lockChangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bLockPinChanges)
            {
                bLockPinChanges = false;
                lockChangesToolStripMenuItem.Checked = false;
            }
            else
            {
                bLockPinChanges = true;
                lockChangesToolStripMenuItem.Checked = true;
            }
        }
        private void tsslClearCounter_Click(object sender, EventArgs e)
        {
            iUnitsPassed = 0;
            iUnitsFailed = 0;
            this.Refresh();
        }
        private void operatorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperatorsForm of = new OperatorsForm();
            of.operators = serApp.operators;
            of.ShowDialog();
            serApp.operators = of.operators;
            SaveAppData();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void selectDataDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = serApp.sDataDirectory;
            fbd.ShowDialog();
            serApp.sDataDirectory = fbd.SelectedPath;
            SaveAppData();
        }
        private void showPinWiresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bShowPinWires)
            {
                bShowPinWires = false;
                showPinWiresToolStripMenuItem.Checked = false;
            }
            else
            {
                bShowPinWires = true;
                showPinWiresToolStripMenuItem.Checked = true;
            }
            serApp.ShowWires = bShowPinWires;
            SaveAppData();
            panelMain.Refresh();
        }
        private void moveAllButtonsOntoGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int x = 0;
            int y = 0;
            int g = Convert.ToInt16(iGridSize);
            foreach (PinButton pb in pinbuttons)
            {
                x = pb.CentreLocation.X;
                y = pb.CentreLocation.Y;
                x = x / g * g;
                y = y / g * g;
                pb.CentreLocation = new Point(x, y);
            }
        }
        private void testingStopsOn1stErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ser.bStopTestOnFirstError)
                SetTestingStopsOnFirstError(false);
            else
                SetTestingStopsOnFirstError(true);
        }
        private void testingShowsAllErrorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ser.bShowAllErrors)
                SetTestingShowsAllErrors(false);
            else
                SetTestingShowsAllErrors(true);
        }
        private void setPINMinVoltageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PinVoltage pv = new PinVoltage();
            pv.mainForm = this;
            pv.psuVoltage = psuVoltage;
            pv.dPinVoltage = ser.dPinTriggerVoltage;
            pv.iMinResistance = ser.dMinResitanceForAutoPickup;
            pv.ShowDialog();
            ser.dPinTriggerVoltage = pv.dPinVoltage;
            if (pv.SetPins)
            {
                for (int i = 0; i < pinbuttons.Count; i++)
                {
                    pinbuttons[i].InputVoltageTriggerLevel = ser.dPinTriggerVoltage;
                    pinbuttons[i].CheckResistance = pv.bCheckResistance;
                }
                ser.dMinResitanceForAutoPickup = pv.iMinResistance;
                PinGroupSetCheckResistance(pv.bCheckResistance);
                ser.bCheckResistance = pv.bCheckResistance;
                UpdatePinVoltageAndResistanceLabel();
            }
        }
        private void hardwareDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HardwareDetails hwd = new HardwareDetails();
            hwd.l1 = "Vendor ID  = " + HidDevice.Attributes.VendorHexId.ToString();
            hwd.l2 = "Product ID = " + HidDevice.Attributes.ProductHexId.ToString();
            hwd.l3 = "Product Version = " + HidDevice.Attributes.Version.ToString();
            hwd.ShowDialog();

        }
        private void checkResistanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pinbuttonClickSelected == null) return;
            if (pinbuttonClickSelected.CheckResistance == false)
            {
                pinbuttonClickSelected.CheckResistance = true;
                checkResistanceToolStripMenuItem.Checked = true;
            }
            else
            {
                pinbuttonClickSelected.CheckResistance = false;
                checkResistanceToolStripMenuItem.Checked = false;
            }
        }
        private void ClearTestCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iUnitsPassed = 0;
            iUnitsFailed = 0;
            UpdateTestDataProgressBars();
        }
        private void printButtonVoltagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int j;
            lbDebug2.Items.Clear();

            for (j = 0; j < 48; j++)
            {
                lbDebug2.Items.Add("Button " + j.ToString() + "=" + pinbuttons[j].InputVoltage.ToString());
            }
        }
        // Pin relatedfunctions
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        private Boolean IsaParentOf(int pin_button_id, List<int> pin_button_id_list)
        {
            if (pin_button_id_list == null) return false;
            foreach (int i in pin_button_id_list)
            {
                if (i == pin_button_id) return true;
            }
            return false;
        }
        private Color[] GetColorList()
        {
            Color[] colorList = new Color[48]
            {
            Color.FromArgb( 0x00, 0x00, 0x00 ), Color.FromArgb( 0x99, 0x33, 0x00 ),
            Color.FromArgb( 0x33, 0x33, 0x00 ), Color.FromArgb( 0x00, 0x33, 0x00 ),
            Color.FromArgb( 0x00, 0x33, 0x66 ), Color.FromArgb( 0x00, 0x00, 0x80 ),
            Color.FromArgb( 0x33, 0x33, 0x99 ), Color.FromArgb( 0x33, 0x33, 0x33 ),

            Color.FromArgb( 0x80, 0x00, 0x00 ), Color.FromArgb( 0xFF, 0x66, 0x00 ),
            Color.FromArgb( 0x80, 0x80, 0x00 ), Color.FromArgb( 0x00, 0x80, 0x00 ),
            Color.FromArgb( 0x00, 0x80, 0x80 ), Color.FromArgb( 0x00, 0x00, 0xFF ),
            Color.FromArgb( 0x66, 0x66, 0x99 ), Color.FromArgb( 0x80, 0x80, 0x80 ),

            Color.FromArgb( 0xFF, 0x00, 0x00 ), Color.FromArgb( 0xFF, 0x99, 0x00 ),
            Color.FromArgb( 0x99, 0xCC, 0x00 ), Color.FromArgb( 0x33, 0x99, 0x66 ),
            Color.FromArgb( 0x33, 0xCC, 0xCC ), Color.FromArgb( 0x33, 0x66, 0xFF ),
            Color.FromArgb( 0x80, 0x00, 0x80 ), Color.FromArgb( 0x99, 0x99, 0x99 ),

            Color.FromArgb( 0xFF, 0x00, 0xFF ), Color.FromArgb( 0xFF, 0xCC, 0x00 ),
            Color.FromArgb( 0xFF, 0xFF, 0x00 ), Color.FromArgb( 0x00, 0xFF, 0x00 ),
            Color.FromArgb( 0x00, 0xFF, 0xFF ), Color.FromArgb( 0x00, 0xCC, 0xFF ),
            Color.FromArgb( 0x99, 0x33, 0x66 ), Color.FromArgb( 0xC0, 0xC0, 0xC0 ),

            Color.FromArgb( 0x20, 0x20, 0x80 ), Color.FromArgb( 0x20, 0x40, 0x80 ),
            Color.FromArgb( 0x20, 0x80, 0x80 ), Color.FromArgb( 0x20, 0xc0, 0x80 ),
            Color.FromArgb( 0x80, 0x20, 0x80 ), Color.FromArgb( 0x80, 0x40, 0x80 ),
            Color.FromArgb( 0x80, 0x80, 0x80 ), Color.FromArgb( 0x80, 0xC0, 0x80 ),

            Color.FromArgb( 0xFF, 0x99, 0xCC ), Color.FromArgb( 0xFF, 0xCC, 0x99 ),
            Color.FromArgb( 0xFF, 0xFF, 0x99 ), Color.FromArgb( 0xCC, 0xFF, 0xCC ),
            Color.FromArgb( 0xCC, 0xFF, 0xFF ), Color.FromArgb( 0x99, 0xCC, 0xFF ),
            Color.FromArgb( 0xCC, 0x99, 0xFF ), Color.FromArgb( 0xFF, 0xFF, 0xFF )
            };
            return colorList;
        }
        private void RemoveCreatedPinButtonsFromForm()
        {
            if (pinbuttons != null)
            {
                for (int i = 0; i < pinbuttons.Count; i++)
                {
                    try
                    {
                        panelMain.Controls.Remove(pinbuttons[i]);
                        pinbuttons[i] = null;
                    }
                    catch { }
                }
            }
            pinbuttons = null;

        }
        private void DrawDefualtPinButtons(Boolean Create)
        {
            int connectorsize = (int)((double)panelMain.Width * 0.4);
            int connectorspace = (int)((double)panelMain.Width * 0.07);
            int pinsize = connectorsize / iConnectorColumns;
            int x_btwn = 2;
            int y_btwn = 2;

            DataTimers(false);

            if (Create)
            {
                if (pinbuttons == null) pinbuttons = new List<UHT4.PinButton>();
                for (int i = 0; i < 96; i++) pinbuttons.Add(new PinButton());
            }

            int kk = 0;
            for (int i = 47; i >= 36; i--)
            {
                if (Create)
                {
                    pinbuttons[i].Active = true;
                }
                pinbuttons[i].Size = new Size(pinsize, pinsize);
                pinbuttons[i].PinType = UHT4.PinButton.PinTypes.Input;
                pinbuttons[i].InputVoltageTriggerLevel = ser.dPinTriggerVoltage;
                pinbuttons[i].CheckResistance = false;
                int x = pinsize * kk + connectorspace + x_btwn * kk; kk++;
                int y = 2 * pinsize + ((pinsize + y_btwn) * 1);
                pinbuttons[i].CentreLocation = new Point(x, y);
                if (Create)
                {
                    pinbuttons[i].ID = (i);
                    pinbuttons[i].TabIndex = (i);
                    AddPinButtonHandlers(pinbuttons[i]);
                    panelMain.Controls.Add(pinbuttons[i]);
                }
            }
            kk = 0;
            for (int i = 35; i >= 24; i--)
            {
                if (Create)
                {
                    pinbuttons[i].Active = true;
                }
                pinbuttons[i].Size = new Size(pinsize, pinsize);
                pinbuttons[i].PinType = UHT4.PinButton.PinTypes.Input;
                pinbuttons[i].InputVoltageTriggerLevel = ser.dPinTriggerVoltage;
                pinbuttons[i].CheckResistance = false;
                int x = pinsize * kk + connectorspace + x_btwn * kk; kk++;
                int y = 3 * pinsize + ((pinsize + y_btwn) * 1);
                pinbuttons[i].CentreLocation = new Point(x, y);
                if (Create)
                {
                    pinbuttons[i].ID = (i);
                    pinbuttons[i].TabIndex = (i);
                    AddPinButtonHandlers(pinbuttons[i]);
                    panelMain.Controls.Add(pinbuttons[i]);
                }
            }
            kk = 0;
            for (int i = 23; i >= 12; i--)
            {
                if (Create)
                {
                    pinbuttons[i].Active = true;
                }
                pinbuttons[i].Size = new Size(pinsize, pinsize);
                pinbuttons[i].PinType = UHT4.PinButton.PinTypes.Input;
                pinbuttons[i].InputVoltageTriggerLevel = ser.dPinTriggerVoltage;
                pinbuttons[i].CheckResistance = false;
                int x = pinsize * kk + connectorspace + x_btwn * kk + (connectorsize + connectorspace); kk++;
                int y = 2 * pinsize + ((pinsize + y_btwn) * 1);
                pinbuttons[i].CentreLocation = new Point(x, y);
                if (Create)
                {
                    pinbuttons[i].ID = (i);
                    pinbuttons[i].TabIndex = (i);
                    AddPinButtonHandlers(pinbuttons[i]);
                    panelMain.Controls.Add(pinbuttons[i]);
                }
            }
            kk = 0;
            for (int i = 0; i <= 11; i++)
            {
                if (Create)
                {
                    pinbuttons[i].Active = true;
                }
                pinbuttons[i].Size = new Size(pinsize, pinsize);
                pinbuttons[i].PinType = UHT4.PinButton.PinTypes.Input;
                pinbuttons[i].InputVoltageTriggerLevel = ser.dPinTriggerVoltage;
                pinbuttons[i].CheckResistance = false;
                int x = pinsize * kk + connectorspace + x_btwn * kk + (connectorsize + connectorspace); kk++;
                int y = 3 * pinsize + ((pinsize + y_btwn) * 1);
                pinbuttons[i].CentreLocation = new Point(x, y);
                if (Create)
                {
                    pinbuttons[i].ID = (i);
                    pinbuttons[i].TabIndex = (i);
                    AddPinButtonHandlers(pinbuttons[i]);
                    panelMain.Controls.Add(pinbuttons[i]);
                }
            }

            kk = 0;
            for (int i = 71; i >= 60; i--)
            {
                if (Create)
                {
                    pinbuttons[i].Active = true;
                }
                pinbuttons[i].Size = new Size(pinsize, pinsize);
                pinbuttons[i].PinType = UHT4.PinButton.PinTypes.Output;
                int x = pinsize * kk + connectorspace + x_btwn * kk; kk++;
                int y = 5 * pinsize + ((pinsize + y_btwn) * 1);
                pinbuttons[i].CentreLocation = new Point(x, y);
                if (Create)
                {
                    pinbuttons[i].ID = (i);
                    pinbuttons[i].TabIndex = (i);
                    AddPinButtonHandlers(pinbuttons[i]);
                    panelMain.Controls.Add(pinbuttons[i]);
                }
            }
            kk = 0;
            for (int i = 48; i <= 59; i++)
            {
                if (Create)
                {
                    pinbuttons[i].Active = true;
                }
                pinbuttons[i].Size = new Size(pinsize, pinsize);
                pinbuttons[i].PinType = UHT4.PinButton.PinTypes.Output;
                int x = pinsize * kk + connectorspace + x_btwn * kk; kk++;
                int y = 6 * pinsize + ((pinsize + y_btwn) * 1);
                pinbuttons[i].CentreLocation = new Point(x, y);
                if (Create)
                {
                    pinbuttons[i].ID = (i);
                    pinbuttons[i].TabIndex = (i);
                    AddPinButtonHandlers(pinbuttons[i]);
                    panelMain.Controls.Add(pinbuttons[i]);
                }
            }
            kk = 0;
            for (int i = 95; i >= 84; i--)
            {
                if (Create)
                {
                    pinbuttons[i].Active = true;
                }
                pinbuttons[i].Size = new Size(pinsize, pinsize);
                pinbuttons[i].PinType = UHT4.PinButton.PinTypes.Output;
                int x = pinsize * kk + connectorspace + x_btwn * kk + (connectorsize + connectorspace); kk++;
                int y = 5 * pinsize + ((pinsize + y_btwn) * 1);
                pinbuttons[i].CentreLocation = new Point(x, y);
                if (Create)
                {
                    pinbuttons[i].ID = (i);
                    pinbuttons[i].TabIndex = (i);
                    AddPinButtonHandlers(pinbuttons[i]);
                    panelMain.Controls.Add(pinbuttons[i]);
                }
            }
            kk = 0;
            for (int i = 72; i <= 83; i++)
            {
                if (Create)
                {
                    pinbuttons[i].Active = true;
                }
                pinbuttons[i].Size = new Size(pinsize, pinsize);
                pinbuttons[i].PinType = UHT4.PinButton.PinTypes.Output;
                int x = pinsize * kk + connectorspace + x_btwn * kk + (connectorsize + connectorspace); kk++;
                int y = 6 * pinsize + ((pinsize + y_btwn) * 1);
                pinbuttons[i].CentreLocation = new Point(x, y);
                if (Create)
                {
                    pinbuttons[i].ID = (i);
                    pinbuttons[i].TabIndex = (i);
                    AddPinButtonHandlers(pinbuttons[i]);
                    panelMain.Controls.Add(pinbuttons[i]);
                }
            }

            DataTimers(true);

        }
        private void SerializePinButtonsToCompatibleContainer()
        {
            pinbuttonsSerialize = new List<PinButtonSerialization>();
            for (int i = 0; i < pinbuttons.Count; i++) pinbuttonsSerialize.Add(new PinButtonSerialization());
            for (int i = 0; i < pinbuttons.Count; i++)
            {
                pinbuttonsSerialize[i].backgroundtype = (int)pinbuttons[i].BackGroundType;
                pinbuttonsSerialize[i].pintype = (int)pinbuttons[i].PinType;
                pinbuttonsSerialize[i].colorBackGround = pinbuttons[i].ColorBlock.ToArgb();
                pinbuttonsSerialize[i].colorOff = pinbuttons[i].ColorOff.ToArgb();
                pinbuttonsSerialize[i].colorOn = pinbuttons[i].ColorOn.ToArgb();
                pinbuttonsSerialize[i].colorStripe = pinbuttons[i].ColorBar.ToArgb();
                pinbuttonsSerialize[i].colorInput = pinbuttons[i].ColorInputButton.ToArgb();
                pinbuttonsSerialize[i].colorOutput = pinbuttons[i].ColorOutputButton.ToArgb();
                pinbuttonsSerialize[i].colorPinNo = pinbuttons[i].ColorIDString.ToArgb();
                pinbuttonsSerialize[i].colorPinText = pinbuttons[i].ColorTextString.ToArgb();
                pinbuttonsSerialize[i].positionLeft = pinbuttons[i].Location.X;
                pinbuttonsSerialize[i].positionTop = pinbuttons[i].Location.Y;
                pinbuttonsSerialize[i].sizeX = pinbuttons[i].Width;
                pinbuttonsSerialize[i].SizeY = pinbuttons[i].Height;
                pinbuttonsSerialize[i].pin_no = pinbuttons[i].ID;
                pinbuttonsSerialize[i].parentIDs = pinbuttons[i].ParentIDs;
                pinbuttonsSerialize[i].childIDs = pinbuttons[i].ChildIDs;
                pinbuttonsSerialize[i].showID = pinbuttons[i].ShowID;
                pinbuttonsSerialize[i].active = pinbuttons[i].Active;
                pinbuttonsSerialize[i].hasChildren = pinbuttons[i].HasChildren;
                pinbuttonsSerialize[i].bGradients = pinbuttons[i].LinearBrushes;
                pinbuttonsSerialize[i].rimwidth = pinbuttons[i].RimWidth;
                pinbuttonsSerialize[i].pinText = pinbuttons[i].PinText;
                pinbuttonsSerialize[i].dPinTriggerVoltage = pinbuttons[i].InputVoltageTriggerLevel;
                pinbuttonsSerialize[i].dInputVoltageLast = pinbuttons[i].InputVoltageLast;
                pinbuttonsSerialize[i].CheckResistance = pinbuttons[i].CheckResistance;
                pinbuttonsSerialize[i].wire_width = pinbuttons[i].WireWidth;
                pinbuttonsSerialize[i].dResistance = pinbuttons[i].Resistance;
                pinbuttonsSerialize[i].dResistanceTolerance = pinbuttons[i].ResistanceTolerance;
            }
        }
        private void DeSerializePinButtonsFromCompatibleContainer()
        {
            pinbuttons = new List<PinButton>();
            for (int i = 0; i < pinbuttonsSerialize.Count; i++)
            {
                pinbuttons.Add(new PinButton());
                pinbuttons[i].AutoRefresh = false;
                pinbuttons[i].BackGroundType = (UHT4.PinButton.BackGroundTypes)pinbuttonsSerialize[i].backgroundtype;
                pinbuttons[i].PinType = (UHT4.PinButton.PinTypes)pinbuttonsSerialize[i].pintype;
                pinbuttons[i].ColorBlock = Color.FromArgb(pinbuttonsSerialize[i].colorBackGround);
                pinbuttons[i].ColorOff = Color.FromArgb(pinbuttonsSerialize[i].colorOff);
                pinbuttons[i].ColorOn = Color.FromArgb(pinbuttonsSerialize[i].colorOn);
                pinbuttons[i].ColorBar = Color.FromArgb(pinbuttonsSerialize[i].colorStripe);
                pinbuttons[i].ColorInputButton = Color.FromArgb(pinbuttonsSerialize[i].colorInput);
                pinbuttons[i].ColorOutputButton = Color.FromArgb(pinbuttonsSerialize[i].colorOutput);
                pinbuttons[i].ColorIDString = Color.FromArgb(pinbuttonsSerialize[i].colorPinNo);
                pinbuttons[i].ColorTextString = Color.FromArgb(pinbuttonsSerialize[i].colorPinText);
                pinbuttons[i].Location = new Point(pinbuttonsSerialize[i].positionLeft, pinbuttonsSerialize[i].positionTop);
                pinbuttons[i].Width = pinbuttonsSerialize[i].sizeX;
                pinbuttons[i].Height = pinbuttonsSerialize[i].SizeY;
                pinbuttons[i].ID = pinbuttonsSerialize[i].pin_no;
                pinbuttons[i].ParentIDs = pinbuttonsSerialize[i].parentIDs;
                pinbuttons[i].ChildIDs = pinbuttonsSerialize[i].childIDs;
                pinbuttons[i].Has_Children = pinbuttonsSerialize[i].hasChildren;

                if (pinbuttonsSerialize[i].hasChidren == true)
                    pinbuttonsSerialize[i].hasChildren = true;

                pinbuttons[i].LinearBrushes = pinbuttonsSerialize[i].bGradients;
                pinbuttons[i].RimWidth = pinbuttonsSerialize[i].rimwidth;
                pinbuttons[i].PinText = pinbuttonsSerialize[i].pinText;
                pinbuttons[i].CheckResistance = pinbuttonsSerialize[i].CheckResistance;
                pinbuttons[i].InputVoltageLast = pinbuttonsSerialize[i].dInputVoltageLast;
                pinbuttons[i].WireWidth = pinbuttonsSerialize[i].wire_width;
                pinbuttons[i].Resistance = pinbuttonsSerialize[i].dResistance;
                pinbuttons[i].ResistanceTolerance = pinbuttonsSerialize[i].dResistanceTolerance;
                pinbuttons[i].Active = pinbuttonsSerialize[i].active;
                pinbuttons[i].ShowID = pinbuttonsSerialize[i].showID;

                if (pinbuttons[i].InputVoltageTriggerLevel <= 0.0)
                    pinbuttons[i].InputVoltageTriggerLevel = 2.5;
                else
                    pinbuttons[i].InputVoltageTriggerLevel = pinbuttonsSerialize[i].dPinTriggerVoltage;

                panelMain.Controls.Add(pinbuttons[i]);
                pinbuttons[i].AutoRefresh = true;
                AddPinButtonHandlers(pinbuttons[i]);
                //RefreshPinButtonColorControls();
            }
            panelMain.Refresh();
        }
        private void AddPinButtonHandlers(UHT4.PinButton p)
        {
            p.MouseClick += new MouseEventHandler(pinbuttons_MouseClick);
            p.MouseMove += new MouseEventHandler(pinbuttons_MouseMove);
            p.MouseDown += new MouseEventHandler(pinbuttons_MouseDown);
            p.MouseUp += new MouseEventHandler(pinbuttons_MouseUp);
            p.ButtonGotFocusChanged += new PinButton.ButtonGotFocusEventHandler(pinbuttons_ButtonGotFocusChanged);
            p.ButtonLostFocusChanged += new PinButton.ButtonLostFocusEventHandler(pinbuttons_ButtonLostFocusChanged);
        }
        private int SelectPinButtonsInDragArea(Point a, Point b)
        {
            if (pinbuttons == null) return -1;
            int result = 0;

            dragareapinbuttons = new List<PinButton>(0);
            lb_dps.Text = "";

            foreach (UHT4.PinButton p in pinbuttons)
            {
                if (p.Active)
                {
                    if (p.CentreLocation.X > a.X && p.CentreLocation.X < b.X && p.CentreLocation.Y > a.Y && p.CentreLocation.Y < b.Y)
                    {
                        p.ClickSelect = true;
                        result++;
                        dragareapinbuttons.Add(p);
                        lb_dps.Text = lb_dps.Text + p.ID.ToString() + " ";
                    }
                    else
                    {
                        p.ClickSelect = false;
                    }
                }
            }
            if (result == 0) lb_dps.Text = "None";
            return result;
        }
        private void CopyPinLocationsToOldLocations()
        {
            foreach (PinButton p2 in pinbuttons)
            {
                p2.OldCentreLocation = p2.CentreLocation;
            }
        }
        public void UpdatePinsAndVoltages(PinButton pb_input)
        {
            if (pb_input.PinType == PinButton.PinTypes.Output)
            {
                pb_input.Value = true;
                System.Threading.Thread.Sleep(serApp.general_sleep);
                HIDSetOutputPin(pb_input.ID - 48);
                System.Threading.Thread.Sleep(serApp.general_sleep);
                HIDReadData();
            }
            if (pb_input.PinType == PinButton.PinTypes.Input)
            {
                foreach (PinButton pb in pinbuttons)
                {
                    if (pb.PinType == PinButton.PinTypes.Output)
                    {
                        if (pb.ParentIDs != null)
                        {
                            foreach (int id in pb.ParentIDs)
                            {
                                if (id == pb_input.ID)
                                {
                                    pb.Value = true;
                                    System.Threading.Thread.Sleep(serApp.general_sleep);
                                    HIDSetOutputPin(pb.ID - 48);
                                    System.Threading.Thread.Sleep(serApp.general_sleep);
                                    HIDReadData();
                                    pb_input.InputVoltageLast = pb_input.InputVoltage;
                                }
                            }
                        }
                    }
                }
            }
        }
        public double GetPinResistance(double input_voltage)
        {
            double resistance = double.NaN;
            //            if (input_voltage >= 2.6)
            //              K = (input_voltage - 2.6) / output_resistor;
            try
            {
                //resistance = ((11.85) * (input_resistor_bottom / input_voltage + K)) - (output_resistor + input_resistor_top + input_resistor_bottom);
                resistance = ((input_resistor_bottom/ input_voltage) * (11.85 - vdiode - vloss)) - (output_resistor + input_resistor_top + input_resistor_bottom);
                 if (resistance < 0) resistance = 0;
                return resistance;
            }
            catch (Exception)
            {
                return resistance;
            }
        }
        public double GetPClosestPinVoltageForResistance ( double resistance )
        {
            for (double d =1.0;d<3.3;d=d+0.1)
            {
                if (GetPinResistance(d) < resistance) return d;
            }
            return 2.5;
        }
        public void UpdatePinVoltageAndResistanceLabel()
        {
            lblPinVoltage.Text = "Pin V=" + ser.dPinTriggerVoltage.ToString("0.00") + " for " + ser.dMinResitanceForAutoPickup.ToString() + " ohm";
        }
        public PinButton FindSelectedPinButton()
        {
            if (pinbuttons == null) return null;
            if (pinbuttons.Count == 0) return null;
            for (int i = 0; i < pinbuttons.Count; i++)
            {
                if (pinbuttons[i].IsSelected || pinbuttons[i].ClickSelect) return pinbuttons[i];
            }
            return null;
        }
        public PinButton FindFirstSelectedPinButton()
        {
            foreach (UHT4.PinButton p in pinbuttons)
            {
                if (p.ClickSelect)
                {
                    return p;
                }
            }
            return null;
        }
        private void PinGroupSetPinTriggerVoltageLevel(double v)
        {
            if (pinbuttons.Count == 0) return;
            for (int i = 0; i < pinbuttons.Count; i++) pinbuttons[i].InputVoltageTriggerLevel = v;
        }
        private void PinGroupSetCheckResistance(Boolean cr)
        {
            if (pinbuttons.Count == 0) return;
            for (int i = 0; i < pinbuttons.Count; i++) pinbuttons[i].CheckResistance = cr;
        }
        private void PinGroupAllPinsClearSelectedState()
        {
            if (pinbuttons == null) return;
            if (pinbuttons.Count == 0) return;

            Boolean any_selected = false;
            foreach(PinButton p in pinbuttons)
            {
                if (p.ClickSelect) any_selected = true;
            }
            if (!any_selected) return; 

            foreach (PinButton p in pinbuttons) { p.ClickSelect = false; }
        }
        private int PinGroupCountSelectedButtons()
        {
            int t = 0;
            if (pinbuttons == null) return 0;
            if (pinbuttons.Count == 0) return 0;
            for (int i = 0; i < pinbuttons.Count; i++) if (pinbuttons[i].ClickSelect == true) t++;
            return t;
        }
        private void PinGroupClearAllOutputPinButtonValues()
        {
            if (pinbuttons.Count == 0) return;
            for (int i = 48; i < 96; i++) pinbuttons[i].Value = false;
        }
        private void PinGroupSetPinsAutoActivateOnInput(Boolean value)
        {
            for (int i = 0; i < pinbuttons.Count; i++) pinbuttons[i].AutoActivateOnInput = value;
        }
        private void PinGroupClearAllPinsButtonValues()
        {
            if (pinbuttons == null) return;
            foreach (UHT4.PinButton p in pinbuttons)
            {

                p.Value = false;
                p.Fail = false;
            }
        }
        private void PinGroupClearAllPinsFailValues()
        {
            foreach (UHT4.PinButton p in pinbuttons)
            {
                p.Fail = false;
            }
        }
        private void pinbuttons_MouseMove(object sender, MouseEventArgs e)
        {
            lblDebug207.Text = MousePosition.ToString();
            if (bGroupDragStart)
            {
                if (!bLockPinMove)
                {
                    Point l = PointToScreen(MousePosition);
                    l = new Point(l.X - panelMain.Width, l.Y - panelMain.Top - menuStrip1.Height - toolStrip1.Height);
                    int x = pinbuttonCentreLocation.X - l.X;
                    int y = pinbuttonCentreLocation.Y - l.Y;
                    Point drawingoffset = new Point(x, y);

                    if (bRegroupButtons)
                    {
                        int xx;
                        int xxx = 0;
                        int yy;
                        int yyy = 0;
                        int cc = dragareapinbuttons.Count();
                        int a = (int)Math.Sqrt((double)cc);
                        int b = cc / a;
                        int row = 0;

                        xx = dragareapinbuttons[0].CentreLocation_X;
                        yy = dragareapinbuttons[0].CentreLocation_Y;

                        bRegroupButtons = false;

                        foreach (PinButton p2 in dragareapinbuttons)
                        {
                            if (row == a)
                            {
                                yyy = yyy + p2.Height; row = 0; xxx = 0;
                            }
                            row++;

                            p2.OldCentreLocation = new Point(xx + xxx, yy + yyy);
                            xxx = xxx + p2.Width + iGridSize;

                        }
                    }

                    lbDebug.Items.Add("Offset is " + x.ToString() + " " + y.ToString());
                    if (dragareapinbuttons != null)
                    {
                        foreach (PinButton p2 in dragareapinbuttons)
                        {
                            p2.CentreLocation = new Point(p2.OldCentreLocation.X - drawingoffset.X, p2.OldCentreLocation.Y - drawingoffset.Y);
                        }
                    }
                }
            }
            else
            {
                if (!bLockPinMove)
                {
                    if (pinbuttonToMove != null)
                    {
                        pinbuttonToMove.CentreLocation_X = (MousePosition.X - pinbuttonMoveStartLocation.X) / iGridSize * iGridSize + pinbuttonCentreLocation.X;
                        pinbuttonToMove.CentreLocation_Y = (MousePosition.Y - pinbuttonMoveStartLocation.Y) / iGridSize * iGridSize + pinbuttonCentreLocation.Y;
                        pinbuttonToMove.Refresh();
                    }
                }
            }
        }
        private void pinbuttons_MouseDown(object sender, MouseEventArgs e)
        {
            PinButton p = (PinButton)sender;
            pinbuttonToMove = p;
            if (p != null)
                if (bAppDebug) lblDebug211.Text = p.ID.ToString();
                else
                if (bAppDebug) lblDebug211.Text = "Nothing";

            bRedrawWires = false;
            //panelMain.Refresh();

            if (bGroupDrag && !bGroupDragStart)
            {
                bGroupDragStart = true;
                if (bAppDebug) lbl_gds.Text = "True";
                Point l = PointToScreen(MousePosition);
                l = new Point(l.X - panelMain.Width, l.Y - panelMain.Top - menuStrip1.Height - toolStrip1.Height);
                pinbuttonCentreLocation = l;
                CopyPinLocationsToOldLocations();
                if (bAppDebug) lbDebug.Items.Add("Setting Offsets");
            }
            else
            {
                PinGroupAllPinsClearSelectedState();
                if (p != null && !bPinbuttonDragEnabled)
                {
                    bPinbuttonDragEnabled = true;
                    pinbuttonMoveStartLocation = MousePosition;
                    pinbuttonCentreLocation = pinbuttonToMove.CentreLocation;
                }
            }
        }
        private void pinbuttons_MouseUp(object sender, MouseEventArgs e)
        {
            PinButton p = (PinButton)sender;
            pinbuttonToMove = null;
            if (p != null)
                if (bAppDebug) lblDebug211.Text = p.ID.ToString();
                else
                if (bAppDebug) lblDebug211.Text = "Nothing";

            bRedrawWires = true;
            panelMain.Refresh();

            if (bGroupDragStart)
            {
                bGroupDrag = false;
                bGroupDragStart = false;
                if (bAppDebug) lbl_gd.Text = "False";
                if (bAppDebug) lbl_gds.Text = "false";
                PinGroupAllPinsClearSelectedState();
            }
            else
            {
                bPinbuttonDragEnabled = false;
            }
        }
        private void pinbuttons_ButtonGotFocusChanged(object sender, EventArgs e)
        {
            if (bAppDebug) lbDebug.Items.Add("Btn got focus");
            UHT4.PinButton pb = (UHT4.PinButton)sender;
            if (!bPinbuttonDragEnabled)
            {
                if (bAppDebug) lblDebug209.Text = pb.ID.ToString();
                pinbuttonSelected = pb;
            }
        }
        private void pinbuttons_ButtonLostFocusChanged(object sender, EventArgs e)
        {
            if (bAppDebug) lbDebug.Items.Add("Btn lost focus");
            UHT4.PinButton pb = (UHT4.PinButton)sender;
            if (!bPinbuttonDragEnabled)
            {
                if (pb.ClickSelect == true) return;
                pinbuttonSelected = null;
                if (bAppDebug) lblDebug209.Text = "Nothing";
            }
        }
        private void pinbuttons_MouseClick(object sender, MouseEventArgs e)
        {
            PinButton pb = (UHT4.PinButton)sender;
            pinbuttonClickSelected = pb;
            ButtonPropertyGrid bpg = new ButtonPropertyGrid(pb, pinbuttons, ser);
            propertyGrid1.Visible = true;
            propertyGrid1.SelectedObject = bpg;
            //PinGroupSetPinsAutoActivateOnInput(true);
            if (e.Button == MouseButtons.Right)
            {
                if (pb.Value)
                    tsmiValue.Text = "Pin (" + pb.ID.ToString() + ") is On";
                else
                    tsmiValue.Text = "Pin (" + pb.ID.ToString() + ") is Off";

                if (pb.PinType == UHT4.PinButton.PinTypes.Input)
                {
                    checkResistanceToolStripMenuItem.Visible = true;
                    apparentResistanceToolStripMenuItem.Visible = true;
                    setResistanceToolStripMenuItem.Visible = true;
                    pinVoltageTriggerLevelToolStripMenuItem.Visible = true;
                    pinVoltageToolStripMenuItem.Visible = true;
                }
                else
                {
                    checkResistanceToolStripMenuItem.Visible = false;
                    apparentResistanceToolStripMenuItem.Visible = false;
                    setResistanceToolStripMenuItem.Visible = false;
                    pinVoltageTriggerLevelToolStripMenuItem.Visible = false;
                    pinVoltageToolStripMenuItem.Visible = false;
                }

                tsmiPinType.Text = "Pin type not set";
                if (pb.PinType == UHT4.PinButton.PinTypes.Input)
                {
                    tsmiPinType.Text = "Pin is Input";
                    pinVoltageToolStripMenuItem.Text = "Pin Voltage is " + pb.InputVoltage.ToString("0.00 V") + "(" + pb.InputVoltageLast.ToString("0.00") + ")";
                    pinVoltageTriggerLevelToolStripMenuItem.Text = "Pin Voltage Trigger Level is " + pb.InputVoltageTriggerLevel.ToString("0.00 V");
                }
                if (pb.PinType == UHT4.PinButton.PinTypes.Output)
                {
                    tsmiPinType.Text = "Pin is Output";
                    pinVoltageToolStripMenuItem.Text = "Pin Voltage N/A";
                }
                double resistance = GetPinResistance(pb.InputVoltageLast);

                if (pb.PinType == PinButton.PinTypes.Input)
                    apparentResistanceToolStripMenuItem.Text = "Apparent Resistance is " + resistance.ToString("0.0") + " ohm";
                else
                    apparentResistanceToolStripMenuItem.Text = "N/A";

                if (pb.PinType == UHT4.PinButton.PinTypes.Input)
                {
                    if (pb.CheckResistance)
                        checkResistanceToolStripMenuItem.Checked = true;
                    else
                        checkResistanceToolStripMenuItem.Checked = false;
                }

                cmsPin.Show(new Point(e.X + pb.Location.X + this.Left + (pb.Width / 2), e.Y + pb.Location.Y + this.Top + (pb.Height / 2)));

            }
            if (e.Button == MouseButtons.Left)
            {
                
                PinGroupClearAllPinsButtonValues();
                UpdatePinsAndVoltages(pb);

                if (!bControlKeyOn)
                {
                    if (bGroupDrag)
                    {
                    }
                    else
                    {
                        pb.Focus();
                        pb.Refresh();
                        //pb.Invalidate();
                        PinGroupAllPinsClearSelectedState();

                        pb.ClickSelect = true;
                        if (pb.PinType == PinButton.PinTypes.Input)
                        {
                            bButton b = panel_input_buttons.Find(ButtonByID(pb.ID));
                            if (b != null) b.BackColor = Color.Green;
                        }
                        if (pb.PinType == PinButton.PinTypes.Output)
                        {
                            bButton b = panel_output_buttons.Find(ButtonByID(pb.ID));
                            if (b != null) b.BackColor = Color.Green;
                        }
                    }
                }
                
            }
        }

        // USB Commands
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        void FindDevice()
        {
            if (bAppDebug) lbDebug.Items.Add("USB Find");
            // Enumerate the devices with the Vendor Id
            // and Product Id of the IT4600
            HidDeviceList = HidDevices.Enumerate(0x04D8, 0x1010);

            if (HidDeviceList.Length > 0)
            {
                // Grab the first device
                HidDevice = HidDeviceList[0];
                int hw_version = HidDevice.Attributes.Version;
                try
                {
                    lblUnitAndHW.Text = "V" + hw_version.ToString();
                }
                catch (Exception)
                {
                    lblUnitAndHW.Text = "Not Set";
                }

                if (hw_version == 1)
                {
                }

                if (hw_version == 2)
                {
                    //MessageBox.Show("This Application only supports Version 2 of the hardware device", "Hardware Error", MessageBoxButtons.OK);
                    //return;
                }

                // Check if connected...
                bDeviceIsConnected = true;
                HidDevice.Open(HidDevice.DeviceMode.NonOverlapped, HidDevice.DeviceMode.NonOverlapped);
                HidDevice.Inserted += new HidDevice.InsertedEventHandler(HidDevice_Inserted);
                HidDevice.Removed += new HidDevice.RemovedEventHandler(HidDevice_Removed);
                lblUnitAndHW.Text = "Test Unit Connected (V" + hw_version.ToString() + ")";
                panelInfo.Refresh();
                DataTimers(true);
            }
        }
        void HidDevice_Removed()
        {
            lblUnitAndHW.Text = "Test Unit Removed";
            panelInfo.Refresh();
            bDeviceIsConnected = false;
            bDeviceIsSetup = false;
            DataTimers(false);
        }
        void HidDevice_Inserted()
        {
            double a;
            if (!bDeviceIsConnected)
            {
                FindDevice();
                if (bDeviceIsConnected)
                {
                    HIDPSUOff();
                    System.Threading.Thread.Sleep(500);
                    HIDClearOutputs();
                    a = ReadPSUDataNoScale();
                    System.Threading.Thread.Sleep(500);
                    HIDHarnessFail();
                    System.Threading.Thread.Sleep(500);
                    psuOnToolStripMenuItemBody();
                    DataTimers(true);
                    bDeviceIsSetup = true;
                }
            }
            if (!bDeviceIsSetup)
            {
                HIDPSUOff();
                System.Threading.Thread.Sleep(500);
                HIDClearOutputs();
                a = ReadPSUDataNoScale();
                System.Threading.Thread.Sleep(500);
                HIDPSUOn();
                psuOnToolStripMenuItemBody();
                DataTimers(true);
                bDeviceIsSetup = true;
            }
        }
        HidDeviceData HIDReadFirst16InputPins()
        {
            byte[] OutData;
            OutData = new byte[HidDevice.Capabilities.OutputReportByteLength - 1];
            OutData[0] = 0x0;
            OutData[1] = 0x80;
            HidDevice.Write(OutData);
            return HidDevice.Read();
        }
        HidDeviceData HIDReadSecond16InputPins()
        {
            byte[] OutData;
            OutData = new byte[HidDevice.Capabilities.OutputReportByteLength - 1];
            OutData[0] = 0x0;
            OutData[1] = 0x81;
            HidDevice.Write(OutData);
            return HidDevice.Read();
        }
        HidDeviceData HIDReadThird16InputPins()
        {
            byte[] OutData;
            OutData = new byte[HidDevice.Capabilities.OutputReportByteLength - 1];
            OutData[0] = 0x0;
            OutData[1] = 0x82;
            HidDevice.Write(OutData);
            return HidDevice.Read();
        }
        private void HIDReadData()
        {
            if (!bDeviceIsConnected) return;

            int tmp2;
            HidDeviceData InData;
            InData = HIDReadFirst16InputPins();
            System.Threading.Thread.Sleep(serApp.data_read_speed);

            if (InData == null) return;
            if (InData.Data.Length < 63) return;
            for (tmp2 = 0; tmp2 < 16; tmp2++)
            {
                pinbuttons[tmp2].InputVoltage = ((double)(InData.Data[tmp2 * 2 + 2] * 256 + InData.Data[tmp2 * 2 + 3])) / 1023 * 3.3;
            }
            InData = HIDReadSecond16InputPins();
            System.Threading.Thread.Sleep(serApp.data_read_speed);

            if (InData == null) return;
            if (InData.Data.Length < 63) return;
            for (tmp2 = 16; tmp2 < 32; tmp2++)
            {
                pinbuttons[tmp2].InputVoltage = ((double)(InData.Data[(tmp2 - 16) * 2 + 2] * 256 + InData.Data[(tmp2 - 16) * 2 + 3])) / 1023 * 3.3;
            }
            InData = HIDReadThird16InputPins();
            System.Threading.Thread.Sleep(serApp.data_read_speed);

            if (InData == null) return;
            if (InData.Data.Length < 63) return;
            for (tmp2 = 32; tmp2 < 48; tmp2++)
            {
                pinbuttons[tmp2].InputVoltage = ((double)(InData.Data[(tmp2 - 32) * 2 + 2] * 256 + InData.Data[(tmp2 - 32) * 2 + 3])) / 1023 * 3.3;
            }
        }
        private void HIDPSUOff()
        {
            if (!bDeviceIsConnected) return;
            if (ReadingPSUData || bReadingData) return;
            byte[] OutData = new byte[HidDevice.Capabilities.OutputReportByteLength - 1];
            OutData[0] = 0x0;
            OutData[1] = 0x86;
            HidDevice.Write(OutData);
        }
        private void HIDPSUOn()
        {
            if (!bDeviceIsConnected) return;
            if (ReadingPSUData || bReadingData) return;
            byte[] OutData = new byte[HidDevice.Capabilities.OutputReportByteLength - 1];
            OutData[0] = 0x0;
            OutData[1] = 0x87;
            HidDevice.Write(OutData);
        }
        private void HIDClearOutputs()
        {
            if (!bDeviceIsConnected) return;
            if (ReadingPSUData || bReadingData) return;
            byte[] OutData = new byte[HidDevice.Capabilities.OutputReportByteLength - 1];
            OutData[0] = 0x0;
            OutData[1] = 0x84;
            HidDevice.Write(OutData);
        }
        private void HIDSetOutputPin(int pin)
        {
            if (!bDeviceIsConnected) return;
            byte[] OutData = new byte[HidDevice.Capabilities.OutputReportByteLength - 1];
            byte latch;
            byte temp;

            OutData[0] = 0x0;
            OutData[1] = 0x83;
            OutData[2] = (byte)pin;

            latch = 0;
            if (pin >= 0 && pin <= 7) latch = 1;
            if (pin >= 8 && pin <= 15) latch = 2;
            if (pin >= 16 && pin <= 23) latch = 3;
            if (pin >= 24 && pin <= 31) latch = 4;
            if (pin >= 32 && pin <= 39) latch = 5;
            if (pin >= 40 && pin <= 47) latch = 6;

            temp = (byte)((byte)pin - ((latch - 1) * 8));
            temp = (byte)(1 << temp);

            OutData[3] = (byte)latch;
            OutData[4] = (byte)temp;
            HidDevice.Write(OutData);
        }
        private void HIDHarnessPass()
        {
            if (!bDeviceIsConnected) return;
            if (ReadingPSUData || bReadingData) return;
            byte[] OutData = new byte[HidDevice.Capabilities.OutputReportByteLength - 1];
            OutData[0] = 0x0;
            OutData[1] = 0x96;
            HidDevice.Write(OutData);
        }
        private void HIDHarnessFail()
        {
            if (!bDeviceIsConnected) return;
            if (ReadingPSUData || bReadingData) return;
            byte[] OutData = new byte[HidDevice.Capabilities.OutputReportByteLength - 1];
            OutData[0] = 0x0;
            OutData[1] = 0x95;
            HidDevice.Write(OutData);
        }
        private double ReadPSUData()
        {
            double v33;
            double vpsu;

            byte[] OutData = new byte[HidDevice.Capabilities.OutputReportByteLength - 1];
            OutData[0] = 0x0;
            OutData[1] = 0x85;
            HidDevice.Write(OutData);
            HidDeviceData InData;

            InData = HidDevice.Read();
            if (InData == null) return 0.0;
            if (InData.Data.Length < 63) return 0.0;

            v33 = ((double)(InData.Data[2] * 256 + InData.Data[3])) / 1023 * 3.3;
            vpsu = ((double)(InData.Data[6] * 256 + InData.Data[7])) / 1023 * 3.3 * scale;


            return vpsu;
        }
        private double ReadPSUDataNoScale()
        {
            double v33;
            double vpsu;

            byte[] OutData = new byte[HidDevice.Capabilities.OutputReportByteLength - 1];
            OutData[0] = 0x0;
            OutData[1] = 0x85;
            HidDevice.Write(OutData);
            HidDeviceData InData;

            InData = HidDevice.Read();
            if (InData == null) return 0.0;
            if (InData.Data.Length < 63) return 0.0;

            v33 = ((double)(InData.Data[2] * 256 + InData.Data[3])) / 1023 * 3.3;
            vpsu = ((double)(InData.Data[6] * 256 + InData.Data[7])) / 1023 * 3.3;


            return vpsu;
        }
        private void tmrPSU_Tick(object sender, EventArgs e)
        {
            //if (bAppDebug) lbDebug.Items.Add("Tmr PSU");
            if (!bDeviceIsConnected) return;
            if (!bEnablePSUTimer) return;

            String t;
            double d;

            if (bReadingData) return;
            ReadingPSUData = true;

            d = ReadPSUData();
            t = "Test Voltage = " + d.ToString("##.00") + "V";
            psuVoltage = d;
            lblTestVoltage.Text = t;
            panelInfo.Refresh();
            ReadingPSUData = false;
        }
        private void DataTimers(Boolean value)
        {
            bEnablePSUTimer = value;
        }
        private void tmrUSBDevice_Tick(object sender, EventArgs e)
        {
            //if (bAppDebug) lbDebug.Items.Add("Tmr USB");
            if (bDeviceIsConnected)
            {
                createNewWorkspaceToolStripMenuItem.Enabled = true;
                tsbNewWorkspace.Enabled = true;

            }
            else
            {
                FindDevice();
                tsbSave.Enabled = false;
                tsbLearn.Enabled = false;
                learnNewHarnessToolStripMenuItem.Enabled = false;
                tsbNewWorkspace.Enabled = false;
            }
        }
        private void tsDDB1_DropDownOpening(object sender, EventArgs e)
        {
            if (serApp.operators != null)
            {
                tsDDB1.DropDownItems.Clear();
                foreach (UHT4.Operators o in serApp.operators)
                {
                    tsDDB1.DropDownItems.Add(o.name + " " + o.surname + " (" + o.id + ")");
                }
            }
        }
        private void tsDDB1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            iSelectedOperator = tsDDB1.DropDownItems.IndexOf(e.ClickedItem);
            if (iSelectedOperator == -1)
            { lblOperator.Text = "Operator ID : Not Selected"; panelInfo.Refresh(); }
            else
            { lblOperator.Text = "Operator ID : " + serApp.operators[iSelectedOperator].id; panelInfo.Refresh(); }
        }

        // Print related functions
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------------------------------------------------
        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PaperSize papersize = new PaperSize("NameOfPaper", ser.psPaperSizeX, ser.psPaperSizey);
            //PaperSize paperSize = new PaperSize("NameOfPaper", 40, 20);
            //printDocument1.DefaultPageSettings.PaperSize = paperSize;
            //or in the PrintPage event
            //PaperSize paperSize = new PaperSize("NameOfPaper", 830, 1170);
            //e.PageSettings.PaperSize = paperSize;

            printDocument1.DefaultPageSettings.PaperSize = papersize;
            pageSetupDialog1.Document = printDocument1;
            pageSetupDialog1.ShowDialog();

        }
        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printPreviewBody(UHTSerializationInfo.PrintDocType.PRINT_DIALOG, null);

        }
        public void printPreviewBody( UHTSerializationInfo.PrintDocType dt, PaintEventArgs e) 
        {
            PaperSize p = new PaperSize("NameOfPaper", ser.psPaperSizeX, ser.psPaperSizey);
            Margins m = new Margins(ser.psMarginLeft, ser.psMarginRight, ser.psMarginTop, ser.psMarginBottom);

            switch (dt)
            {
                case UHTSerializationInfo.PrintDocType.PRINT_DIALOG:
                    printPreviewDialog1 = new PrintPreviewDialog();
                    printPreviewDialog1.Document = printDocument1;
                    try {
                        printPreviewDialog1.ShowDialog(this); }
                    catch (Exception ex) {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK); }
                    break;

                case UHTSerializationInfo.PrintDocType.PRINT_PANEL:
                    GeneratePrintData(null,e);
                    break;
            }
        }

        public Font GetFont(string name, int size, string type)
        {
            if (name == null) name = "Arial";
            if (type == null) type = "Normal";
            return new Font(name, size, GetFontStyle(type));
        }
        public FontStyle GetFontStyle(string style)
        {
            switch (style)
            {
                case "Bold": return FontStyle.Bold;
                case "Regular": return FontStyle.Regular;
                case "Italic": return FontStyle.Regular;
            }
            return FontStyle.Regular;
        }
        public void GeneratePrintData(PrintPageEventArgs e, PaintEventArgs e2)
        {
            float mMarginLeft = ser.psMarginLeft;
            float mMarginTop = ser.psMarginTop;
            float mMarginBottom = ser.psMarginBottom;
            float mMarginRight = ser.psMarginRight;
            float hSpaceHeight = ser.psLabelSpacing;
            float hHeight = mMarginTop + hSpaceHeight;
            SizeF sz;

            // Setup line type string for date/time 
            int[] line = new int[ser.label_items.Count];
            int[] line_heights = new int[ser.label_items.Count];
            int i;

            Font fLabel_std;

            foreach (UHTSerializationInfo.LabelItem li in ser.label_items)
            {

                // Operator
                if (li.type == UHTSerializationInfo.LabelItemType.OPERATOR)
                {
                    if (iSelectedOperator != -1)
                        li.text2 = serApp.operators[iSelectedOperator].id.ToString();
                    else
                        li.text2 = "Not Selected";
                }

                // Label Counter
                if (li.type == UHTSerializationInfo.LabelItemType.COUNTER)
                    li.text2 = label_counter.ToString() + li.format;

                // Text
                if (li.type == UHTSerializationInfo.LabelItemType.TEXT)
                    li.text2 = "";

                // Date or Time
                if (li.type == UHTSerializationInfo.LabelItemType.DATE || li.type == UHTSerializationInfo.LabelItemType.TIME)
                {
                    li.text2 = DateTime.Now.ToString(li.format);
                }

                if (li.type == UHTSerializationInfo.LabelItemType.WEEKCODE)
                {
                    var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
                    var ms = cal.GetWeekOfYear(DateTime.Today, System.Globalization.CalendarWeekRule.FirstDay, System.DayOfWeek.Sunday);

                    String tmp;
                    tmp = "CU/" + ms.ToString() + "/" + DateTime.Today.Year.ToString();
                    li.text2 = tmp;
                }

                // Build the print 'Image'
                if (li.show)
                {
                    if (li.type == UHTSerializationInfo.LabelItemType.BARCODE)
                    {
                        Boolean include = false;
                        int left = 0;
                        int top = 0;
                        int sbottom = 20;
                        int swidth = 100;
                        int sheight = 50;

                        string[] sparams = li.format.Split(',');
                        try { if (int.Parse(sparams[0]) == 1) include = true; } catch (Exception) { }
                        try { int temp = int.Parse(sparams[1]); top = temp; } catch (Exception) { }
                        try { int temp = int.Parse(sparams[2]); left = temp; } catch (Exception) { }
                        try { int temp = int.Parse(sparams[3]); swidth = temp; } catch (Exception) { }
                        try { int temp = int.Parse(sparams[4]); sheight = temp; } catch (Exception) { }
                        try { int temp = int.Parse(sparams[5]); sbottom = temp; } catch (Exception) { }

                        // get accumulated line height offsets
                        line_heights[li.line_position] = (int)sbottom;

                        // add the line heights
                        int line_offset = 0;
                        for (i = 0; i < li.line_position; i++) line_offset += line_heights[i];

                        //set font
                        fLabel_std = GetFont(ser.font_name, li.font_size, ser.font_type);
                        try
                        {
                            SkiaSharp.SKImage skimg = barcode.Encode(tpBarcode, li.GetText(), swidth, sheight);

                            Image img = Image.FromStream((skimg).Encode().AsStream());

                            if (e != null) e.Graphics.DrawImage(img, left, top + line_offset);
                            else e2.Graphics.DrawImage(img, ser.psBarcodeMarginLeft + left, top + line_offset);
                        }
                        catch (Exception)
                        {
                            if (e!=null) e.Graphics.DrawString("Barcode Error", fLabel_std, new SolidBrush(Color.Black), mMarginLeft + line[li.line_position], mMarginTop + line_offset);
                            else e2.Graphics.DrawString("Barcode Error", fLabel_std, new SolidBrush(Color.Black), mMarginLeft + line[li.line_position], mMarginTop + line_offset);
                        }

                        // add the last height to the line positions
                        line[li.line_position] = line[li.line_position] + (int)sbottom;

                    }
                    else
                    {
                    //set font
                    fLabel_std = GetFont(ser.font_name, li.font_size, ser.font_type);
                    // get text size
                    if (e!=null) sz = e.Graphics.MeasureString(li.GetText(), fLabel_std);
                    else sz = e2.Graphics.MeasureString(li.GetText(), fLabel_std);
                    // get accumulated line height offsets
                    line_heights[li.line_position] = (int)sz.Height;

                    // add the line heights
                    int line_offset = 0;
                    for (i = 0; i < li.line_position; i++) line_offset += line_heights[i];

                    // draw the string
                    if (e!=null) e.Graphics.DrawString(li.GetText(), fLabel_std, new SolidBrush(Color.Black), mMarginLeft + line[li.line_position], mMarginTop + line_offset);
                    else e2.Graphics.DrawString(li.GetText(), fLabel_std, new SolidBrush(Color.Black), mMarginLeft + line[li.line_position], mMarginTop + line_offset);
                    // add the last height to the line positions
                    line[li.line_position] = line[li.line_position] + (int)sz.Width;
                    }
                }
            }
        }
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            GeneratePrintData(e,null);
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            pd.ShowDialog();
        }
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintBody();
        }
        public class RawPrinterHelper
        {
            // Structure and API declarions:
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public class DOCINFOA
            {
                [MarshalAs(UnmanagedType.LPStr)] public string pDocName;
                [MarshalAs(UnmanagedType.LPStr)] public string pOutputFile;
                [MarshalAs(UnmanagedType.LPStr)] public string pDataType;
            }
            [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

            [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool ClosePrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

            [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool EndDocPrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool StartPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool EndPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

            // SendBytesToPrinter()
            // When the function is given a printer name and an unmanaged array
            // of bytes, the function sends those bytes to the print queue.
            // Returns true on success, false on failure.
            public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
            {
                Int32 dwError = 0, dwWritten = 0;
                IntPtr hPrinter = new IntPtr(0);
                DOCINFOA di = new DOCINFOA();
                bool bSuccess = false; // Assume failure unless you specifically succeed.

                di.pDocName = "My C#.NET RAW Document";
                di.pDataType = "RAW";

                // Open the printer.
                if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
                {
                    // Start a document.
                    if (StartDocPrinter(hPrinter, 1, di))
                    {
                        // Start a page.
                        if (StartPagePrinter(hPrinter))
                        {
                            // Write your bytes.
                            bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                            EndPagePrinter(hPrinter);
                        }
                        EndDocPrinter(hPrinter);
                    }
                    ClosePrinter(hPrinter);
                }
                // If you did not succeed, GetLastError may give more information
                // about why not.
                if (bSuccess == false)
                {
                    dwError = Marshal.GetLastWin32Error();
                }
                return bSuccess;
            }

            public static bool SendFileToPrinter(string szPrinterName, string szFileName)
            {
                // Open the file.
                FileStream fs = new FileStream(szFileName, FileMode.Open);
                // Create a BinaryReader on the file.
                BinaryReader br = new BinaryReader(fs);
                // Dim an array of bytes big enough to hold the file's contents.
                Byte[] bytes = new Byte[fs.Length];
                bool bSuccess = false;
                // Your unmanaged pointer.
                IntPtr pUnmanagedBytes = new IntPtr(0);
                int nLength;

                nLength = Convert.ToInt32(fs.Length);
                // Read the contents of the file into the array.
                bytes = br.ReadBytes(nLength);
                // Allocate some unmanaged memory for those bytes.
                pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
                // Copy the managed byte array into the unmanaged array.
                Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
                // Send the unmanaged bytes to the printer.
                bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength);
                // Free the unmanaged memory that you allocated earlier.
                Marshal.FreeCoTaskMem(pUnmanagedBytes);
                return bSuccess;
            }
            public static bool SendStringToPrinter(string szPrinterName, string szString)
            {
                IntPtr pBytes;
                Int32 dwCount;
                // How many characters are in the string?
                dwCount = szString.Length;
                // Assume that the printer is expecting ANSI text, and then convert
                // the string to ANSI text.
                pBytes = Marshal.StringToCoTaskMemAnsi(szString);
                // Send the converted ANSI string to the printer.
                SendBytesToPrinter(szPrinterName, pBytes, dwCount);
                Marshal.FreeCoTaskMem(pBytes);
                return true;
            }
        }
        private void PrintBody()
        {
            PaperSize p = new PaperSize("NameOfPaper", ser.psPaperSizeX, ser.psPaperSizey);
            Margins m = new Margins(ser.psMarginLeft, ser.psMarginRight, ser.psMarginTop, ser.psMarginBottom);

            if (iSelectedOperator == -1)
            {
                MessageBox.Show("You must select an opertor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            printDocument1.DefaultPageSettings.PaperSize = p;
            try
            {
                printDocument1.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }
        private void labelSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LabelSettings ls = new LabelSettings(this);
            ls.ShowDialog();
        }
        private void SubFunction_DrawPinWires(PaintEventArgs e)
        {
            if (bAppDebug) lbDebug.Items.Add("Draw Pin wires");

            if (bShowPinWires && bRedrawWires)
            {
                List<ResistanceBlock> listRB = new List<ResistanceBlock>();
                Font fontText = new Font("Arial", ser.iResistanceBlockFontSize, FontStyle.Regular);
                Brush bText = new SolidBrush(Color.Black);
                Brush bTextFill = new SolidBrush(Color.Yellow);
                Brush bTextFillPass = new SolidBrush(Color.Green);
                Brush bTextFillFail = new SolidBrush(Color.Red);
                Pen pText = new Pen(bText);
                if (pinbuttons != null)
                {
                    if (pinbuttons.Count > 0)
                    {
                        Pen p = new Pen(new SolidBrush(Color.Black));
                        Pen p2 = new Pen(new SolidBrush(Color.Yellow), 3);
                        Pen p3 = new Pen(new SolidBrush(Color.Orange), 3);

                        // find the selected id
                        int selected_id = -1;
                        PinButton pp = pinbuttons.Find(PinButtonByClickSelectedID());
                        if (pp != null)
                        {
                            selected_id = pp.ID;
                            // draw all wires
                            foreach (PinButton pb in pinbuttons)
                            {
                                if (pb.PinType == PinButton.PinTypes.Input)
                                {
                                    if (pb.ExistsChildIDs())
                                    {
                                        foreach (int childid in pb.ChildIDs)
                                        {
                                            int x1, x2, y1, y2;
                                            double pin_resistance;
                                            PinButton pbChild = pinbuttons[childid];
                                            pin_resistance = GetPinResistance(pb.InputVoltageLast);

                                            x1 = pb.Location.X + pb.Width / 2;
                                            y1 = pb.Location.Y + pb.Height / 2;
                                            x2 = pbChild.Location.X + pbChild.Width / 2;
                                            y2 = pbChild.Location.Y + pbChild.Height / 2;

                                            e.Graphics.DrawLine(p, x1, y1, x2, y2);
                                            AddResistanceBlockUnrouted(pb, x1, x2, y1, y2, fontText, bTextFillPass, bTextFillFail, bTextFill, p, listRB);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (ResistanceBlock rb in listRB)
                {
                    Paint_ResistanceBlock(e.Graphics, rb);
                }

            }
        }
        private Boolean IsSelectedIDPartOfConnectionIDs(int pin_id, int selected_id)
        {
            Boolean is_part_of_connection = false;
            if (selected_id == -1) return false;
            PinButton selectedPin = pinbuttons[selected_id];
            if (selectedPin == null) return false;
            if (selected_id == pin_id) is_part_of_connection = true;
            if (selectedPin.ExistsChildIDs())
            {
                foreach (int id in selectedPin.ChildIDs)
                {
                    if (id == pin_id) is_part_of_connection = true;
                }
            }
            if (selectedPin.ExistsParentIDs())
            {
                foreach (int id in selectedPin.ParentIDs)
                {
                    if (id == pin_id) is_part_of_connection = true;
                }
            }
            return is_part_of_connection;
        }
        private void AddResistanceBlockUnrouted(PinButton pb, int x1, int x2, int y1, int y2, Font fText, Brush bFillPass, Brush bFillFail, Brush bText, Pen pText, List<ResistanceBlock> listRB)
        {
            double pin_resistance = GetPinResistance(pb.InputVoltageLast);
            double pin_set_resistance = pb.Resistance;

            if (pb.CheckResistance)
            {
                if (pin_resistance < (pb.Resistance * (1 + pb.ResistanceTolerance / 100)) && pin_resistance > (pb.Resistance * (1 - pb.ResistanceTolerance / 100)))
                    listRB.Add(GetNewResistanceBlock((x1 + x2) / 2, (y1 + y2) / 2, pin_set_resistance, pin_resistance, pb.Resistance, pb.ResistanceTolerance, fText, bFillPass, bText, pText));
                else
                    listRB.Add(GetNewResistanceBlock((x1 + x2) / 2, (y1 + y2) / 2, pin_set_resistance, pin_resistance, pb.Resistance, pb.ResistanceTolerance, fText, bFillFail, bText, pText));
            }
        }
        private void setResistanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Resistance r = new Resistance();
            r.r = pinbuttonClickSelected.Resistance;
            r.rt = pinbuttonClickSelected.ResistanceTolerance;
            r.Location = this.Location;
            r.ShowDialog(this);
            pinbuttonClickSelected.Resistance = r.r;
            pinbuttonClickSelected.ResistanceTolerance = r.rt;
            pinbuttonClickSelected.Refresh();
        }
        private void setTestTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bDataFileOpen)
            {
                MessageBox.Show("First load a test file", "Error", MessageBoxButtons.OK);
                return;
            }
            TestTarget tt = new TestTarget();
            tt.ShowDialog(this);
            if (tt.limit == -1) return;
            ser.iTestTarget = tt.limit;
            SaveData();
            UpdateTestDataProgressBars();
        }
        private void UpdateTestDataProgressBars()
        {
            if (ser.iTestTarget < 0) ser.iTestTarget = serTestData.testdata.Count;
            if (ser.bSaveTestData)
            {
                if (serTestData.testdata.Count > ser.iTestTarget)
                {
                    cpbTestDataFail.Maximum = serTestData.testdata.Count;
                    cpbTestDataPass.Maximum = serTestData.testdata.Count;
                }
                else
                {
                    cpbTestDataFail.Maximum = ser.iTestTarget;
                    cpbTestDataPass.Maximum = ser.iTestTarget;
                }
            }
            cpbTestDataPass.Value = iUnitsPassed;
            cpbTestDataFail.Value = iUnitsFailed;
            cpbTestDataFail.Refresh();
            cpbTestDataPass.Refresh();
        }
        private void testReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestReport tr = new TestReport();
            tr.ShowDialog(this);
        }
        public void Reset_input_buttons_state(UHT4.bButton.state st)
        {
            foreach (bButton b in panel_input_buttons) { b.State = st; }
        }
        public void Reset_output_buttons_state(UHT4.bButton.state st)
        {
            foreach (bButton b in panel_output_buttons) { b.State = st; }
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            int i = 0;
            i++;
        }
        private void autoColorPinsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LearnAutoColorPins();
        }
        private void checkResistanceToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (pinbuttonClickSelected != null)
            {
                Boolean v = pinbuttonClickSelected.CheckResistance;
                v = !v;
                pinbuttonClickSelected.CheckResistance = v;
            }
        }
        public String GetFileNameFromFullPathFileName( String s)
        {
            char[] cc = new char[2];
            cc[0] = '\\';
            string[] sss = s.Split(cc);
            if (sss != null)
            {
                return sss[sss.Length - 1];
            }
            return "";
        }
        private void backgroundImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackGroundImages bi = new BackGroundImages();
            bi.bDataFileOpen = bDataFileOpen;
            bi.form1 = this;
            bi.panelMain = panelMain;
            bi.Show(null);
        }
        private void Form1_ResizeEnd(object sender, EventArgs e)
        {

        }
        private void horizontalSplitter1_MouseDown(object sender, MouseEventArgs e)
        {
            bHorizontalBarMove = true;
            pHorizontalBar = e.Location;
        }
        private void horizontalSplitter1_MouseUp(object sender, MouseEventArgs e)
        {
            bHorizontalBarMove = false;
        }
        private void horizontalSplitter1_MouseMove(object sender, MouseEventArgs e)
        {
            if (bHorizontalBarMove == true)
            {
                int y = pHorizontalBar.Y - e.Location.Y;
                horizontalSplitter1.Location = new Point(horizontalSplitter1.Left, horizontalSplitter1.Top - y);
                ser.horizontal_bar_top = horizontalSplitter1.Top;
                Form1_ResizeBody(false);
            }
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            //horizontalSplitter1.Top = this.Height - panelConnectors.Height - 5;
            Form1_ResizeBody(true);
        }
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1_ResizeBody(true);
        }
        private void horizontalSplitter1_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.HSplit;
        }
        private void saveTestDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serApp.bSaveTestData == true)
            {
                serApp.bSaveTestData = false;
                saveTestDataToolStripMenuItem.Checked = false;
            }
            else
            {
                serApp.bSaveTestData = true;
                saveTestDataToolStripMenuItem.Checked = true;
            }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            LabelSettings ls = new LabelSettings(this);
            ls.ShowDialog();

        }

        private void resetLabelCounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label_counter = 0;
            lblLabelCounter.Text = label_counter.ToString();
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                label_counter = int.Parse(toolStripTextBox1.Text);
            }
            catch (Exception ex )
            {
                label_counter = 0;
            }
            lblLabelCounter.Text = label_counter.ToString();
        }
    }
}


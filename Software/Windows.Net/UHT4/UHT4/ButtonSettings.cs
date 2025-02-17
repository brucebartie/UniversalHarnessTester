using System;
using System.Drawing;
using System.Windows.Forms;

namespace UHT4
{
    public partial class ButtonSettings : Form
    {
        public UHT4.UHTForm form;
        Boolean settingv;
        public class sEventArgs : EventArgs
        {
            public Boolean b;
            public Color _Color;
            public String _Text;
            public String _Text2;
            public int _Index;
            public string _IndexVal;
            public PinButton.BackGroundTypes bgType;
        }

        public Boolean SettingValues
        {
            set { settingv = value; }
            get { return settingv; }
        }

        public ButtonSettings(UHT4.UHTForm f)
        {
            InitializeComponent();

            foreach (String MyEnum in PinButton.BackGroundTypes.GetNames(typeof(PinButton.BackGroundTypes)))
            {
                cmbPinButtonDrawType.Items.Add(MyEnum);
            }

            f.ButtonSettingsChanged += Form_ButtonSettingsChanged;

        }

        private void Form_ButtonSettingsChanged(object sender, UHTForm.ButtonSettingsEventArgs e)
        {
            SettingValues = true;
            cmbPinButtonDrawType.SelectedIndex = e.drawtype;
            cButtonBlockColor.Color = e.blockcolor;
            cButtonInputColor.Color = e.inputcolor;
            cButtonOutputColor.Color = e.outputcolor;
            cButtonOnColor.Color = e.oncolor;
            cButtonOffColor.Color = e.offcolor;
            cButtonStringColor.Color = e.stringcolor;
            cButtonStripeColor.Color = e.stripecolor;
            tbButtonSizeX.Text = e.sx;
            tbButtonSizeY.Text = e.sy;
            tbWireWidth.Text = e.ww;
            tbRimWidth.Text = e.rw;
            cbGradients.Checked = e.gr;
            SettingValues = false;
            this.Refresh();
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void bcChangedEventHandler(object sender, sEventArgs e);
        public event bcChangedEventHandler bcChanged;
        protected virtual void Raise_bc_ChangedEvent(sEventArgs e)
        {
            if (bcChanged != null) bcChanged(this, e);
        }
        private void CButtonBlockColor_Changed(object sender, EventArgs e)
        {
            if (!SettingValues)
            {
                sEventArgs se = new sEventArgs();
                se._Color = ((ColorButton)sender).Color;
                Raise_bc_ChangedEvent(se);
            }
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void ocChangedEventHandler(object sender, sEventArgs e);
        public event ocChangedEventHandler ocChanged;
        protected virtual void Raise_oc_ChangedEvent(sEventArgs e)
        {
            if (ocChanged != null) ocChanged(this, e);
        }
        private void CButtonOnColor_Changed(object sender, EventArgs e)
        {
            if (!SettingValues)
            {
                sEventArgs se = new sEventArgs();
                se._Color = ((ColorButton)sender).Color;
                Raise_oc_ChangedEvent(se);
            }
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void ofcChangedEventHandler(object sender, sEventArgs e);
        public event ofcChangedEventHandler ofcChanged;
        protected virtual void Raise_ofc_ChangedEvent(sEventArgs e)
        {
            if (ofcChanged != null) ofcChanged(this, e);
        }
        private void CButtonOffColor_Changed(object sender, EventArgs e)
        {
            if (!SettingValues)
            {
                sEventArgs se = new sEventArgs();
                se._Color = ((ColorButton)sender).Color;
                Raise_ofc_ChangedEvent(se);
            }
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void ptChangedEventHandler(object sender, sEventArgs e);
        public event ptChangedEventHandler ptChanged;
        protected virtual void Raise_pt_ChangedEvent(sEventArgs e)
        {
            if (ptChanged != null) ptChanged(this, e);
        }
        private void CButtonPinText_Changed(object sender, EventArgs e)
        {
            if (!SettingValues)
            {
                sEventArgs se = new sEventArgs();
                se._Text = ((ColorButton)sender).Text;
                Raise_pt_ChangedEvent(se);
            }
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void icChangedEventHandler(object sender, sEventArgs e);
        public event icChangedEventHandler icChanged;
        protected virtual void Raise_ic_ChangedEvent(sEventArgs e)
        {
            if (icChanged != null) icChanged(this, e);
        }
        private void CButtonInputColor_Changed(object sender, EventArgs e)
        {
            if (!SettingValues)
            {
                sEventArgs se = new sEventArgs();
                se._Color = ((ColorButton)sender).Color;
                Raise_ic_ChangedEvent(se);
            }
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void opcChangedEventHandler(object sender, sEventArgs e);
        public event icChangedEventHandler opcChanged;
        protected virtual void Raise_opc_ChangedEvent(sEventArgs e)
        {
            if (opcChanged != null) opcChanged(this, e);
        }
        private void CButtonOutputColor_Changed(object sender, EventArgs e)
        {
            if (!SettingValues)
            {
                sEventArgs se = new sEventArgs();
                se._Color = ((ColorButton)sender).Color;
                Raise_opc_ChangedEvent(se);
            }
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void scChangedEventHandler(object sender, sEventArgs e);
        public event scChangedEventHandler scChanged;
        protected virtual void Raise_sc_ChangedEvent(sEventArgs e)
        {
            if (scChanged != null) scChanged(this, e);
        }
        private void CButtonStripeColor_Changed(object sender, EventArgs e)
        {
            if (!SettingValues)
            {
                sEventArgs se = new sEventArgs();
                se._Color = ((ColorButton)sender).Color;
                Raise_sc_ChangedEvent(se);
            }
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void strcChangedEventHandler(object sender, sEventArgs e);
        public event strcChangedEventHandler strcChanged;
        protected virtual void Raise_strc_ChangedEvent(sEventArgs e)
        {
            if (strcChanged != null) strcChanged(this, e);
        }
        private void CButtonStringColor_Changed(object sender, EventArgs e)
        {
            if (!SettingValues)
            {
                sEventArgs se = new sEventArgs();
                se._Color = ((ColorButton)sender).Color;
                Raise_strc_ChangedEvent(se);
            }
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void dticChangedEventHandler(object sender, sEventArgs e);
        public event dticChangedEventHandler dticChanged;
        protected virtual void Raise_dtic_ChangedEvent(sEventArgs e)
        {
            if (dticChanged != null) dticChanged(this, e);
        }
        private void CmbPinButtonDrawType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SettingValues)
            {
                sEventArgs se = new sEventArgs();
                se.bgType = (PinButton.BackGroundTypes)((ComboBox)sender).SelectedIndex;
                Raise_strc_ChangedEvent(se);
            }
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void ptEnterEventHandler(object sender, sEventArgs e);
        public event ptEnterEventHandler ptEnter;
        protected virtual void Raise_pt_EnterEvent(sEventArgs e)
        {
            if (ptEnter != null) ptEnter(this, e);
        }
        private void TbPinText_Enter(object sender, EventArgs e)
        {

        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void ptLeaveEventHandler(object sender, sEventArgs e);
        public event ptLeaveEventHandler ptLeave;
        protected virtual void Raise_pt_LeaveEvent(sEventArgs e)
        {
            if (ptLeave != null) ptLeave(this, e);
        }
        private void TbPinText_Leave(object sender, EventArgs e)
        {

        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void sxChangedEventHandler(object sender, sEventArgs e);
        public event sxChangedEventHandler sxChanged;
        protected virtual void Raise_sx_ChangedEvent(sEventArgs e)
        {
            if (sxChanged != null) sxChanged(this, e);
        }
        private void TbButtonSizeX_TextChanged(object sender, EventArgs e)
        {
            if (!SettingValues)
            {
                sEventArgs se = new sEventArgs();
                se._Text = tbButtonSizeX.Text;
                se._Text2 = tbButtonSizeY.Text;
                Raise_sx_ChangedEvent(se);
            }
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void syChangedEventHandler(object sender, sEventArgs e);
        public event syChangedEventHandler syChanged;
        protected virtual void Raise_sy_ChangedEvent(sEventArgs e)
        {
            if (syChanged != null) syChanged(this, e);
        }
        private void TbButtonSizeY_TextChanged(object sender, EventArgs e)
        {
            if (!SettingValues)
            {
                sEventArgs se = new sEventArgs();
                se._Text = tbButtonSizeX.Text;
                se._Text2 = tbButtonSizeY.Text;
                Raise_sy_ChangedEvent(se);
            }
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void gridChangedEventHandler(object sender, sEventArgs e);
        public event gridChangedEventHandler gridChanged;
        protected virtual void Raise_grid_ChangedEvent(sEventArgs e)
        {
            if (gridChanged != null) gridChanged(this, e);
        }
        private void TbGrid_TextChanged(object sender, EventArgs e)
        {
            if (!SettingValues)
            {
                sEventArgs se = new sEventArgs();
                se._Text = tbGrid.Text;
                Raise_grid_ChangedEvent(se);
            }
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void wwChangedEventHandler(object sender, sEventArgs e);
        public event wwChangedEventHandler wwChanged;
        protected virtual void Raise_ww_ChangedEvent(sEventArgs e)
        {
            if (wwChanged != null) wwChanged(this, e);
        }
        private void TbWireWidth_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!SettingValues)
                {
                    sEventArgs se = new sEventArgs();
                    se._Text = tbWireWidth.Text;
                    Raise_ww_ChangedEvent(se);
                }
            }
            catch (Exception)
            {
                if (!SettingValues)
                {
                    tbWireWidth.Text = "5";
                    sEventArgs se = new sEventArgs();
                    se._Text = tbWireWidth.Text;
                    Raise_ww_ChangedEvent(se);
                }
            }
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void tb1ChangedEventHandler(object sender, sEventArgs e);
        public event tb1ChangedEventHandler tb1Changed;
        protected virtual void Raise_tb1_ChangedEvent(sEventArgs e)
        {
            if (tb1Changed != null) tb1Changed(this, e);
        }
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!SettingValues)
            {
                sEventArgs se = new sEventArgs();
                se._Text = textBox1.Text;
                Raise_tb1_ChangedEvent(se);
            }
        }


        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void cb1ChangedEventHandler(object sender, sEventArgs e);
        public event cb1ChangedEventHandler cb1Changed;
        protected virtual void Raise_cb1_ChangedEvent(sEventArgs e)
        {
            if (cb1Changed != null) cb1Changed(this, e);
        }
        private void cmbPinFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SettingValues)
            {
                sEventArgs se = new sEventArgs();
                se._Index = Convert.ToInt16(cmbPinFontSize.Items[cmbPinFontSize.SelectedIndex]);
                Raise_cb1_ChangedEvent(se);
            }
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void grCheckedEventHandler(object sender, sEventArgs e);
        public event tb1ChangedEventHandler grChecked;
        protected virtual void Raise_gr_CheckedEvent(sEventArgs e)
        {
            if (grChecked != null) grChecked(this, e);
        }
        private void CbGradients_CheckedChanged(object sender, EventArgs e)
        {
            if (!SettingValues)
            {
                sEventArgs se = new sEventArgs();
                se.b = cbGradients.Checked;
                Raise_gr_CheckedEvent(se);
            }
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void rwChangedEventHandler(object sender, sEventArgs e);
        public event rwChangedEventHandler rwChanged;
        protected virtual void Raise_rw_ChangedEvent(sEventArgs e)
        {
            if (rwChanged != null) rwChanged(this, e);
        }
        private void TbRimWidth_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!SettingValues)
                {
                    sEventArgs se = new sEventArgs();
                    se._Text = tbRimWidth.Text;
                    Raise_rw_ChangedEvent(se);
                }
            }
            catch (Exception)
            {
                if (!SettingValues)
                {
                    tbRimWidth.Text = "10";
                    sEventArgs se = new sEventArgs();
                    se._Text = tbRimWidth.Text;
                    Raise_rw_ChangedEvent(se);
                }
            }
        }

        // ------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------
        public delegate void ptTextChangedEventHandler(object sender, sEventArgs e);
        public event ptTextChangedEventHandler ptTextChanged;
        protected virtual void Raise_pt_TextChangedEvent(sEventArgs e)
        {
            if (ptTextChanged != null) ptTextChanged(this, e);
        }
        private void TbPinText_TextChanged(object sender, EventArgs e)
        {
            if (!SettingValues)
            {
                sEventArgs se = new sEventArgs();
                se._Text = tbPinText.Text;
                Raise_pt_TextChangedEvent(se);
            }
        }
    }
}

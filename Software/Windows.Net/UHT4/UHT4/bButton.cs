using System;
using System.Drawing;
using System.Windows.Forms;


namespace UHT4
{
    public class bButton : System.Windows.Forms.Button
    {
        int id;
        int displayid;
        state st;

        public delegate void pEventHandler(object sender, EventArgs e);
        public event pEventHandler pEvent;
        protected virtual void RaisepEvent(EventArgs e)
        {
            if (pEvent != null) pEvent(this, e);
        }

        public enum state { OFF, ON, INDETERMINATE, ROLLOVER };

        public bButton() : base()
        {
            ID = 0;
            DisplayID = 0;
            State = state.OFF;
        }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public int DisplayID
        {
            get { return displayid; }
            set { displayid = value; }
        }
        public state State
        {
            get { return st; }
            set
            {
                st = value;
                if (st == state.ON)
                    BackColor = Color.Green;
                else if (st == state.OFF)
                    BackColor = SystemColors.ButtonFace;
                else
                    BackColor = Color.Yellow;

            }
        }
        public void StateNext()
        {
            st++;
            if (st == state.ROLLOVER) st = state.OFF;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            RaisepEvent(null);

        }
    }
}

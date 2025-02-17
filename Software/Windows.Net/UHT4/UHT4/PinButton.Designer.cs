namespace UHT4
{
    partial class PinButton
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PinButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PinButton";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PinButton_Paint);

            this.MouseEnter += new System.EventHandler(this.PinButton_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.PinButton_MouseLeave);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

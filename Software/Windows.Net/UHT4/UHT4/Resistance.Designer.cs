namespace UHT4
{
    partial class Resistance
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSave = new System.Windows.Forms.Button();
            this.tbResistanceTol = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbResistance = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(130, 58);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbResistanceTol
            // 
            this.tbResistanceTol.Location = new System.Drawing.Point(156, 32);
            this.tbResistanceTol.Name = "tbResistanceTol";
            this.tbResistanceTol.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tbResistanceTol.Size = new System.Drawing.Size(29, 20);
            this.tbResistanceTol.TabIndex = 8;
            this.tbResistanceTol.Text = "100";
            this.tbResistanceTol.TextChanged += new System.EventHandler(this.tbResistanceTol_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Resistance Tolerance (%)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Pin Nominal Resistance";
            // 
            // tbResistance
            // 
            this.tbResistance.Location = new System.Drawing.Point(156, 6);
            this.tbResistance.Name = "tbResistance";
            this.tbResistance.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tbResistance.Size = new System.Drawing.Size(49, 20);
            this.tbResistance.TabIndex = 5;
            this.tbResistance.Text = "100000";
            this.tbResistance.TextChanged += new System.EventHandler(this.tbResistance_TextChanged);
            // 
            // Resistance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(214, 87);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbResistanceTol);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbResistance);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Resistance";
            this.Text = "Resistance";
            this.Shown += new System.EventHandler(this.Resistance_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox tbResistanceTol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbResistance;
    }
}
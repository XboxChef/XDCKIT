
namespace XDevkit
{
    partial class Fan_Control
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
            this.FanControl = new DevExpress.XtraEditors.ZoomTrackBarControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.FanControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FanControl.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // FanControl
            // 
            this.FanControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.FanControl.EditValue = null;
            this.FanControl.Location = new System.Drawing.Point(0, 6);
            this.FanControl.Name = "FanControl";
            this.FanControl.Size = new System.Drawing.Size(310, 16);
            this.FanControl.TabIndex = 6;
            // 
            // labelControl1
            // 
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelControl1.Location = new System.Drawing.Point(0, 22);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(17, 13);
            this.labelControl1.TabIndex = 7;
            this.labelControl1.Text = "0%";
            // 
            // Fan_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.FanControl);
            this.Controls.Add(this.labelControl1);
            this.Name = "Fan_Control";
            this.Size = new System.Drawing.Size(310, 35);
            ((System.ComponentModel.ISupportInitialize)(this.FanControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FanControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ZoomTrackBarControl FanControl;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}

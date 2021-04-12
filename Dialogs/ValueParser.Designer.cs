
namespace XDevkitTester.XDevkit.Dialogs
{
    partial class ValueParser
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
            this.ParseButton = new DevExpress.XtraEditors.SimpleButton();
            this.ParseBox = new DevExpress.XtraEditors.ListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.ParseBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ParseButton
            // 
            this.ParseButton.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.False;
            this.ParseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ParseButton.Location = new System.Drawing.Point(493, 306);
            this.ParseButton.Name = "ParseButton";
            this.ParseButton.Size = new System.Drawing.Size(94, 23);
            this.ParseButton.TabIndex = 0;
            this.ParseButton.Text = "Parse Value";
            this.ParseButton.Click += new System.EventHandler(this.ParseButton_Click);
            // 
            // ParseBox
            // 
            this.ParseBox.AllowDrop = true;
            this.ParseBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ParseBox.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.ParseBox.HorizontalScrollbar = true;
            this.ParseBox.Items.AddRange(new object[] {
            "Drop Text File Or Paste Text."});
            this.ParseBox.Location = new System.Drawing.Point(12, 12);
            this.ParseBox.Name = "ParseBox";
            this.ParseBox.Size = new System.Drawing.Size(575, 288);
            this.ParseBox.TabIndex = 1;
            // 
            // ValueParser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 341);
            this.Controls.Add(this.ParseBox);
            this.Controls.Add(this.ParseButton);
            this.Name = "ValueParser";
            this.Text = "ValueParser";
            ((System.ComponentModel.ISupportInitialize)(this.ParseBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton ParseButton;
        private DevExpress.XtraEditors.ListBoxControl ParseBox;
    }
}

namespace XDevkit.Controls
{
    partial class MemDump
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
            comboBox2 = new System.Windows.Forms.ComboBox();
            dumpMemoryButton = new System.Windows.Forms.Button();
            label32 = new System.Windows.Forms.Label();
            dumpStartOffsetTextBox = new System.Windows.Forms.TextBox();
            label33 = new System.Windows.Forms.Label();
            dumpLengthTextBox = new System.Windows.Forms.TextBox();
            SuspendLayout();
            // 
            // comboBox2
            // 
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] {
            "Please Select",
            "Physical RAM",
            "Base File / Image",
            "Allocated Data / Virtual"});
            comboBox2.Location = new System.Drawing.Point(4, 2);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new System.Drawing.Size(196, 21);
            comboBox2.TabIndex = 22;
            // 
            // dumpMemoryButton
            // 
            dumpMemoryButton.ForeColor = System.Drawing.Color.Black;
            dumpMemoryButton.Location = new System.Drawing.Point(66, 84);
            dumpMemoryButton.Name = "dumpMemoryButton";
            dumpMemoryButton.Size = new System.Drawing.Size(75, 23);
            dumpMemoryButton.TabIndex = 17;
            dumpMemoryButton.Text = "Dump";
            dumpMemoryButton.UseVisualStyleBackColor = true;
            // 
            // label32
            // 
            label32.AutoSize = true;
            label32.Location = new System.Drawing.Point(4, 61);
            label32.Name = "label32";
            label32.Size = new System.Drawing.Size(88, 13);
            label32.TabIndex = 21;
            label32.Text = "Dump Length 0x:";
            // 
            // dumpStartOffsetTextBox
            // 
            dumpStartOffsetTextBox.Location = new System.Drawing.Point(98, 30);
            dumpStartOffsetTextBox.Name = "dumpStartOffsetTextBox";
            dumpStartOffsetTextBox.Size = new System.Drawing.Size(102, 20);
            dumpStartOffsetTextBox.TabIndex = 18;
            dumpStartOffsetTextBox.Text = "C0000000";
            // 
            // label33
            // 
            label33.AutoSize = true;
            label33.Location = new System.Drawing.Point(4, 33);
            label33.Name = "label33";
            label33.Size = new System.Drawing.Size(91, 13);
            label33.TabIndex = 20;
            label33.Text = "Starting Offset 0x:";
            // 
            // dumpLengthTextBox
            // 
            dumpLengthTextBox.Location = new System.Drawing.Point(98, 58);
            dumpLengthTextBox.Name = "dumpLengthTextBox";
            dumpLengthTextBox.Size = new System.Drawing.Size(102, 20);
            dumpLengthTextBox.TabIndex = 19;
            dumpLengthTextBox.Text = "FF";
            // 
            // MemDump
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(comboBox2);
            Controls.Add(dumpMemoryButton);
            Controls.Add(label32);
            Controls.Add(dumpStartOffsetTextBox);
            Controls.Add(label33);
            Controls.Add(dumpLengthTextBox);
            MaximumSize = new System.Drawing.Size(205, 110);
            MinimumSize = new System.Drawing.Size(205, 110);
            Name = "MemDump";
            Size = new System.Drawing.Size(205, 110);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button dumpMemoryButton;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox dumpStartOffsetTextBox;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox dumpLengthTextBox;
    }
}

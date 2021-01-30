
namespace XDevkit.Controls
{
    partial class ByteArray
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
            Value = new DevExpress.XtraEditors.TextEdit();
            comboBox1 = new System.Windows.Forms.ComboBox();
            textBox1 = new DevExpress.XtraEditors.TextEdit();
            textBox2 = new DevExpress.XtraEditors.TextEdit();
            textBox3 = new DevExpress.XtraEditors.TextEdit();
            textBox4 = new DevExpress.XtraEditors.TextEdit();
            textBox5 = new DevExpress.XtraEditors.TextEdit();
            textBox6 = new DevExpress.XtraEditors.TextEdit();
            textBox7 = new DevExpress.XtraEditors.TextEdit();
            textBox8 = new DevExpress.XtraEditors.TextEdit();
            Poke = new System.Windows.Forms.Button();
            textEdit1 = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(textEdit1.Properties)).BeginInit();
            SuspendLayout();
            // 
            // Value
            // 
            Value.Location = new System.Drawing.Point(152, 3);
            Value.Name = "Value";
            Value.Size = new System.Drawing.Size(18, 20);
            Value.TabIndex = 2;
            Value.Text = "FF";
            Value.Visible = false;
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] {
            "1 byte",
            "2 bytes",
            "4 bytes",
            "8 bytes"});
            comboBox1.Location = new System.Drawing.Point(87, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(59, 21);
            comboBox1.TabIndex = 3;
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(176, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(18, 20);
            textBox1.TabIndex = 4;
            textBox1.Text = "FF";
            textBox1.Visible = false;
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(224, 3);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(18, 20);
            textBox2.TabIndex = 6;
            textBox2.Text = "FF";
            textBox2.Visible = false;
            // 
            // textBox3
            // 
            textBox3.Location = new System.Drawing.Point(200, 3);
            textBox3.Name = "textBox3";
            textBox3.Size = new System.Drawing.Size(18, 20);
            textBox3.TabIndex = 5;
            textBox3.Text = "FF";
            textBox3.Visible = false;
            // 
            // textBox4
            // 
            textBox4.Location = new System.Drawing.Point(320, 3);
            textBox4.Name = "textBox4";
            textBox4.Size = new System.Drawing.Size(18, 20);
            textBox4.TabIndex = 10;
            textBox4.Text = "FF";
            textBox4.Visible = false;
            // 
            // textBox5
            // 
            textBox5.Location = new System.Drawing.Point(296, 3);
            textBox5.Name = "textBox5";
            textBox5.Size = new System.Drawing.Size(18, 20);
            textBox5.TabIndex = 9;
            textBox5.Text = "FF";
            textBox5.Visible = false;
            // 
            // textBox6
            // 
            textBox6.Location = new System.Drawing.Point(272, 3);
            textBox6.Name = "textBox6";
            textBox6.Size = new System.Drawing.Size(18, 20);
            textBox6.TabIndex = 8;
            textBox6.Text = "FF";
            textBox6.Visible = false;
            // 
            // textBox7
            // 
            textBox7.Location = new System.Drawing.Point(248, 3);
            textBox7.Name = "textBox7";
            textBox7.Size = new System.Drawing.Size(18, 20);
            textBox7.TabIndex = 7;
            textBox7.Text = "FF";
            textBox7.Visible = false;
            // 
            // textBox8
            // 
            textBox8.Location = new System.Drawing.Point(3, 3);
            textBox8.Name = "textBox8";
            textBox8.Size = new System.Drawing.Size(78, 20);
            textBox8.TabIndex = 11;
            textBox8.Text = "Address";
            // 
            // Poke
            // 
            Poke.Location = new System.Drawing.Point(344, 3);
            Poke.Name = "Poke";
            Poke.Size = new System.Drawing.Size(53, 19);
            Poke.TabIndex = 12;
            Poke.Text = "Poke";
            Poke.UseVisualStyleBackColor = true;
            // 
            // textEdit1
            // 
            textEdit1.Location = new System.Drawing.Point(224, 4);
            textEdit1.Name = "textEdit1";
            textEdit1.Size = new System.Drawing.Size(100, 20);
            textEdit1.TabIndex = 13;
            // 
            // ByteArray
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(textEdit1);
            Controls.Add(Poke);
            Controls.Add(textBox8);
            Controls.Add(textBox4);
            Controls.Add(textBox5);
            Controls.Add(textBox6);
            Controls.Add(textBox7);
            Controls.Add(textBox2);
            Controls.Add(textBox3);
            Controls.Add(textBox1);
            Controls.Add(comboBox1);
            Controls.Add(Value);
            MaximumSize = new System.Drawing.Size(400, 25);
            MinimumSize = new System.Drawing.Size(400, 25);
            Name = "ByteArray";
            Size = new System.Drawing.Size(400, 25);
            ((System.ComponentModel.ISupportInitialize)(textEdit1.Properties)).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.TextEdit Value;
        private System.Windows.Forms.ComboBox comboBox1;
        private DevExpress.XtraEditors.TextEdit textBox1;
        private DevExpress.XtraEditors.TextEdit textBox2;
        private DevExpress.XtraEditors.TextEdit textBox3;
        private DevExpress.XtraEditors.TextEdit textBox4;
        private DevExpress.XtraEditors.TextEdit textBox5;
        private DevExpress.XtraEditors.TextEdit textBox6;
        private DevExpress.XtraEditors.TextEdit textBox7;
        private DevExpress.XtraEditors.TextEdit textBox8;
        private System.Windows.Forms.Button Poke;
        private DevExpress.XtraEditors.TextEdit textEdit1;
    }
}

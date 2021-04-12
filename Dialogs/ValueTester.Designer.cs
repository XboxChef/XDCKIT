
namespace XDevkitTester.XDevkit.Dialogs
{
    partial class ValueTester
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
            this.components = new System.ComponentModel.Container();
            this.offsetsList = new System.Windows.Forms.ListView();
            this.Offsetx = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Typex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Valuex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.valuesBox = new System.Windows.Forms.RichTextBox();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.startButton = new DevExpress.XtraEditors.SimpleButton();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timeBar = new DevExpress.XtraEditors.ProgressBarControl();
            this.timeBox = new DevExpress.XtraEditors.TextEdit();
            this.batchesBox = new DevExpress.XtraEditors.TextEdit();
            this.batchTestingCheck = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeBar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchesBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchTestingCheck.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // offsetsList
            // 
            this.offsetsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Offsetx,
            this.Typex,
            this.Valuex});
            this.offsetsList.HideSelection = false;
            this.offsetsList.Location = new System.Drawing.Point(12, 12);
            this.offsetsList.Name = "offsetsList";
            this.offsetsList.Size = new System.Drawing.Size(289, 200);
            this.offsetsList.TabIndex = 0;
            this.offsetsList.UseCompatibleStateImageBehavior = false;
            this.offsetsList.View = System.Windows.Forms.View.Details;
            // 
            // Offsetx
            // 
            this.Offsetx.Text = "Offset";
            this.Offsetx.Width = 108;
            // 
            // Typex
            // 
            this.Typex.Text = "Type";
            this.Typex.Width = 84;
            // 
            // Valuex
            // 
            this.Valuex.Text = "Value";
            // 
            // valuesBox
            // 
            this.valuesBox.Location = new System.Drawing.Point(12, 216);
            this.valuesBox.Name = "valuesBox";
            this.valuesBox.Size = new System.Drawing.Size(289, 53);
            this.valuesBox.TabIndex = 1;
            this.valuesBox.Text = "Enter the values to be tested here. Separate by a comma. Use \"DEFAULT\" to poke th" +
    "e offset\'s default value. ";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(475, 224);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(93, 13);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "Test each value for";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(608, 224);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(39, 13);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "seconds";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(548, 246);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(99, 23);
            this.startButton.TabIndex = 9;
            this.startButton.Text = "Start Testing";
            this.startButton.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Office 2019 Black";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(502, 68);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(52, 13);
            this.labelControl4.TabIndex = 11;
            this.labelControl4.Text = "Next Value";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(569, 68);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(88, 13);
            this.labelControl5.TabIndex = 13;
            this.labelControl5.Text = "Next Offset/batch";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::XDevkit.Properties.Resources.classica_pause_button_flat_rounded_square_white_on_blue_gray_512x512;
            this.pictureBox3.Location = new System.Drawing.Point(475, 246);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(67, 23);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 14;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::XDevkit.Properties.Resources.unnamed;
            this.pictureBox2.Location = new System.Drawing.Point(589, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(49, 50);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 12;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::XDevkit.Properties.Resources.unnamed;
            this.pictureBox1.Location = new System.Drawing.Point(504, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(49, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // timeBar
            // 
            this.timeBar.Location = new System.Drawing.Point(548, 275);
            this.timeBar.Name = "timeBar";
            this.timeBar.Size = new System.Drawing.Size(99, 10);
            this.timeBar.TabIndex = 8;
            // 
            // timeBox
            // 
            this.timeBox.EditValue = "120";
            this.timeBox.Location = new System.Drawing.Point(574, 220);
            this.timeBox.Name = "timeBox";
            this.timeBox.Size = new System.Drawing.Size(28, 20);
            this.timeBox.TabIndex = 5;
            // 
            // batchesBox
            // 
            this.batchesBox.EditValue = "Offsets per Batch";
            this.batchesBox.Location = new System.Drawing.Point(526, 167);
            this.batchesBox.Name = "batchesBox";
            this.batchesBox.Size = new System.Drawing.Size(121, 20);
            this.batchesBox.TabIndex = 4;
            // 
            // batchTestingCheck
            // 
            this.batchTestingCheck.Location = new System.Drawing.Point(526, 141);
            this.batchTestingCheck.Name = "batchTestingCheck";
            this.batchTestingCheck.Properties.Caption = "Enable Batch Testing";
            this.batchTestingCheck.Size = new System.Drawing.Size(121, 18);
            this.batchTestingCheck.TabIndex = 3;
            // 
            // ValueTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 292);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.timeBar);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.timeBox);
            this.Controls.Add(this.batchesBox);
            this.Controls.Add(this.batchTestingCheck);
            this.Controls.Add(this.valuesBox);
            this.Controls.Add(this.offsetsList);
            this.Name = "ValueTester";
            this.Text = "XEXAssistant";
            this.Load += new System.EventHandler(this.ValueTester_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeBar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchesBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchTestingCheck.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView offsetsList;
        private System.Windows.Forms.ColumnHeader Offsetx;
        private System.Windows.Forms.ColumnHeader Typex;
        private System.Windows.Forms.ColumnHeader Valuex;
        private System.Windows.Forms.RichTextBox valuesBox;
        private DevExpress.XtraEditors.CheckEdit batchTestingCheck;
        private DevExpress.XtraEditors.TextEdit batchesBox;
        private DevExpress.XtraEditors.TextEdit timeBox;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ProgressBarControl timeBar;
        private DevExpress.XtraEditors.SimpleButton startButton;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}
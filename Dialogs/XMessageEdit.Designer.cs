
using System.Drawing;

namespace XDevkit.Dialogs
{
    partial class XMessageEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public static XMessageboxUI XMessageboxUI { get; } = new XMessageboxUI();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XMessageEdit));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.Title = new DevExpress.XtraEditors.TextEdit();
            this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.Body = new System.Windows.Forms.TextBox();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.Success = new DevExpress.XtraEditors.PictureEdit();
            this.Error = new DevExpress.XtraEditors.PictureEdit();
            this.Warning = new DevExpress.XtraEditors.PictureEdit();
            this.Info = new DevExpress.XtraEditors.PictureEdit();
            this.ImageType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.PictureType = new DevExpress.XtraEditors.PictureEdit();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit3 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit2 = new DevExpress.XtraEditors.CheckEdit();
            this.text3 = new DevExpress.XtraEditors.TextEdit();
            this.text2 = new DevExpress.XtraEditors.TextEdit();
            this.text1 = new DevExpress.XtraEditors.TextEdit();
            this.Button3 = new DevExpress.XtraEditors.SimpleButton();
            this.Button2 = new DevExpress.XtraEditors.SimpleButton();
            this.Button1 = new DevExpress.XtraEditors.SimpleButton();
            this.PreviewButton = new DevExpress.XtraEditors.SimpleButton();
            this.SendUI = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Title.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Success.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Error.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Warning.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Info.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.text3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.text2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.text1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.Title);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 63);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(586, 47);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Title";
            // 
            // Title
            // 
            this.Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.Title.Location = new System.Drawing.Point(2, 23);
            this.Title.Name = "Title";
            this.Title.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.Title.Size = new System.Drawing.Size(582, 18);
            this.Title.TabIndex = 0;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.Body);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 110);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(586, 128);
            this.groupControl2.TabIndex = 1;
            this.groupControl2.Text = "Message";
            // 
            // Body
            // 
            this.Body.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Body.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Body.Location = new System.Drawing.Point(2, 23);
            this.Body.Multiline = true;
            this.Body.Name = "Body";
            this.Body.Size = new System.Drawing.Size(582, 103);
            this.Body.TabIndex = 2;
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.Success);
            this.groupControl3.Controls.Add(this.Error);
            this.groupControl3.Controls.Add(this.Warning);
            this.groupControl3.Controls.Add(this.Info);
            this.groupControl3.Controls.Add(this.ImageType);
            this.groupControl3.Controls.Add(this.PictureType);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl3.Location = new System.Drawing.Point(0, 0);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(586, 63);
            this.groupControl3.TabIndex = 1;
            this.groupControl3.Text = "Icon Type";
            // 
            // Success
            // 
            this.Success.EditValue = ((object)(resources.GetObject("Success.EditValue")));
            this.Success.Location = new System.Drawing.Point(431, 26);
            this.Success.Name = "Success";
            this.Success.Properties.AllowFocused = false;
            this.Success.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.Success.Properties.Appearance.Options.UseBackColor = true;
            this.Success.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.Success.Properties.ReadOnly = true;
            this.Success.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.Success.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.Success.Size = new System.Drawing.Size(32, 31);
            this.Success.TabIndex = 10;
            this.Success.Visible = false;
            // 
            // Error
            // 
            this.Error.EditValue = ((object)(resources.GetObject("Error.EditValue")));
            this.Error.Location = new System.Drawing.Point(469, 26);
            this.Error.Name = "Error";
            this.Error.Properties.AllowFocused = false;
            this.Error.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.Error.Properties.Appearance.Options.UseBackColor = true;
            this.Error.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.Error.Properties.ReadOnly = true;
            this.Error.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.Error.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.Error.Size = new System.Drawing.Size(32, 31);
            this.Error.TabIndex = 9;
            this.Error.Visible = false;
            // 
            // Warning
            // 
            this.Warning.EditValue = ((object)(resources.GetObject("Warning.EditValue")));
            this.Warning.Location = new System.Drawing.Point(545, 26);
            this.Warning.Name = "Warning";
            this.Warning.Properties.AllowFocused = false;
            this.Warning.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.Warning.Properties.Appearance.Options.UseBackColor = true;
            this.Warning.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.Warning.Properties.ReadOnly = true;
            this.Warning.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.Warning.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.Warning.Size = new System.Drawing.Size(32, 31);
            this.Warning.TabIndex = 8;
            this.Warning.Visible = false;
            // 
            // Info
            // 
            this.Info.EditValue = ((object)(resources.GetObject("Info.EditValue")));
            this.Info.Location = new System.Drawing.Point(507, 26);
            this.Info.Name = "Info";
            this.Info.Properties.AllowFocused = false;
            this.Info.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.Info.Properties.Appearance.Options.UseBackColor = true;
            this.Info.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.Info.Properties.ReadOnly = true;
            this.Info.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.Info.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.Info.Size = new System.Drawing.Size(32, 31);
            this.Info.TabIndex = 7;
            this.Info.Visible = false;
            // 
            // ImageType
            // 
            this.ImageType.EditValue = "Select Icon Type";
            this.ImageType.Location = new System.Drawing.Point(50, 31);
            this.ImageType.Name = "ImageType";
            this.ImageType.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.ImageType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ImageType.Properties.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.ImageType.Properties.DropDownRows = 5;
            this.ImageType.Properties.Items.AddRange(new object[] {
            "Error",
            "Info",
            "Success",
            "Warning"});
            this.ImageType.Properties.Sorted = true;
            this.ImageType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.ImageType.Size = new System.Drawing.Size(26, 18);
            this.ImageType.TabIndex = 2;
            this.ImageType.SelectedIndexChanged += new System.EventHandler(this.Type);
            this.ImageType.TextChanged += new System.EventHandler(this.ImageType_TextChanged);
            // 
            // PictureType
            // 
            this.PictureType.EditValue = ((object)(resources.GetObject("PictureType.EditValue")));
            this.PictureType.Location = new System.Drawing.Point(12, 26);
            this.PictureType.Name = "PictureType";
            this.PictureType.Properties.AllowFocused = false;
            this.PictureType.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.PictureType.Properties.Appearance.Options.UseBackColor = true;
            this.PictureType.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.PictureType.Properties.ReadOnly = true;
            this.PictureType.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.PictureType.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.PictureType.Size = new System.Drawing.Size(32, 31);
            this.PictureType.TabIndex = 6;
            // 
            // groupControl4
            // 
            this.groupControl4.Controls.Add(this.checkEdit1);
            this.groupControl4.Controls.Add(this.checkEdit3);
            this.groupControl4.Controls.Add(this.checkEdit2);
            this.groupControl4.Controls.Add(this.text3);
            this.groupControl4.Controls.Add(this.text2);
            this.groupControl4.Controls.Add(this.text1);
            this.groupControl4.Controls.Add(this.Button3);
            this.groupControl4.Controls.Add(this.Button2);
            this.groupControl4.Controls.Add(this.Button1);
            this.groupControl4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl4.Location = new System.Drawing.Point(0, 238);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(586, 116);
            this.groupControl4.TabIndex = 3;
            this.groupControl4.Text = "Buttons";
            // 
            // checkEdit1
            // 
            this.checkEdit1.EditValue = true;
            this.checkEdit1.Enabled = false;
            this.checkEdit1.Location = new System.Drawing.Point(12, 26);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.AllowFocused = false;
            this.checkEdit1.Properties.Caption = "1";
            this.checkEdit1.Size = new System.Drawing.Size(32, 20);
            this.checkEdit1.TabIndex = 13;
            // 
            // checkEdit3
            // 
            this.checkEdit3.Location = new System.Drawing.Point(12, 86);
            this.checkEdit3.Name = "checkEdit3";
            this.checkEdit3.Properties.AllowFocused = false;
            this.checkEdit3.Properties.Caption = "3";
            this.checkEdit3.Size = new System.Drawing.Size(32, 20);
            this.checkEdit3.TabIndex = 12;
            this.checkEdit3.CheckStateChanged += new System.EventHandler(this.CheckStateChanged);
            // 
            // checkEdit2
            // 
            this.checkEdit2.Location = new System.Drawing.Point(12, 57);
            this.checkEdit2.Name = "checkEdit2";
            this.checkEdit2.Properties.AllowFocused = false;
            this.checkEdit2.Properties.Caption = "2";
            this.checkEdit2.Size = new System.Drawing.Size(32, 20);
            this.checkEdit2.TabIndex = 11;
            this.checkEdit2.CheckStateChanged += new System.EventHandler(this.CheckStateChanged);
            // 
            // text3
            // 
            this.text3.Enabled = false;
            this.text3.Location = new System.Drawing.Point(268, 87);
            this.text3.Name = "text3";
            this.text3.Properties.AllowFocused = false;
            this.text3.Size = new System.Drawing.Size(313, 20);
            this.text3.TabIndex = 9;
            this.text3.TextChanged += new System.EventHandler(this.TextController);
            // 
            // text2
            // 
            this.text2.Enabled = false;
            this.text2.Location = new System.Drawing.Point(268, 58);
            this.text2.Name = "text2";
            this.text2.Properties.AllowFocused = false;
            this.text2.Size = new System.Drawing.Size(313, 20);
            this.text2.TabIndex = 8;
            this.text2.TextChanged += new System.EventHandler(this.TextController);
            // 
            // text1
            // 
            this.text1.Location = new System.Drawing.Point(268, 26);
            this.text1.Name = "text1";
            this.text1.Properties.AllowFocused = false;
            this.text1.Size = new System.Drawing.Size(313, 20);
            this.text1.TabIndex = 7;
            this.text1.TextChanged += new System.EventHandler(this.TextController);
            // 
            // Button3
            // 
            this.Button3.AllowFocus = false;
            this.Button3.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.False;
            this.Button3.Enabled = false;
            this.Button3.Location = new System.Drawing.Point(50, 86);
            this.Button3.Name = "Button3";
            this.Button3.Size = new System.Drawing.Size(212, 23);
            this.Button3.TabIndex = 6;
            this.Button3.Text = "Disabled";
            // 
            // Button2
            // 
            this.Button2.AllowFocus = false;
            this.Button2.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.False;
            this.Button2.Enabled = false;
            this.Button2.Location = new System.Drawing.Point(50, 57);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(212, 23);
            this.Button2.TabIndex = 4;
            this.Button2.Text = "Disabled";
            // 
            // Button1
            // 
            this.Button1.AllowFocus = false;
            this.Button1.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.False;
            this.Button1.Enabled = false;
            this.Button1.Location = new System.Drawing.Point(50, 25);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(212, 23);
            this.Button1.TabIndex = 2;
            this.Button1.Text = "Type Custom Text Here";
            // 
            // PreviewButton
            // 
            this.PreviewButton.AllowFocus = false;
            this.PreviewButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.PreviewButton.Location = new System.Drawing.Point(0, 354);
            this.PreviewButton.Name = "PreviewButton";
            this.PreviewButton.Size = new System.Drawing.Size(586, 28);
            this.PreviewButton.TabIndex = 10;
            this.PreviewButton.Text = "Preview UI";
            this.PreviewButton.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // SendUI
            // 
            this.SendUI.AllowFocus = false;
            this.SendUI.Dock = System.Windows.Forms.DockStyle.Top;
            this.SendUI.Location = new System.Drawing.Point(0, 382);
            this.SendUI.Name = "SendUI";
            this.SendUI.Size = new System.Drawing.Size(586, 28);
            this.SendUI.TabIndex = 11;
            this.SendUI.Text = "Send Message UI";
            this.SendUI.Click += new System.EventHandler(this.SendUI_Click);
            // 
            // XMessageEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 408);
            this.Controls.Add(this.SendUI);
            this.Controls.Add(this.PreviewButton);
            this.Controls.Add(this.groupControl4);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.groupControl3);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Glow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "XMessageEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MessageBox Editor";
            this.Load += new System.EventHandler(this.XMessageEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Title.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Success.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Error.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Warning.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Info.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.text3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.text2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.text1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.TextEdit Title;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.TextBox Body;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.ComboBoxEdit ImageType;
        private DevExpress.XtraEditors.PictureEdit PictureType;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.CheckEdit checkEdit3;
        private DevExpress.XtraEditors.CheckEdit checkEdit2;
        private DevExpress.XtraEditors.TextEdit text3;
        private DevExpress.XtraEditors.TextEdit text2;
        private DevExpress.XtraEditors.TextEdit text1;
        private DevExpress.XtraEditors.SimpleButton Button3;
        private DevExpress.XtraEditors.SimpleButton Button2;
        private DevExpress.XtraEditors.SimpleButton Button1;
        private DevExpress.XtraEditors.SimpleButton PreviewButton;
        private DevExpress.XtraEditors.PictureEdit Error;
        private DevExpress.XtraEditors.PictureEdit Warning;
        private DevExpress.XtraEditors.PictureEdit Info;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.SimpleButton SendUI;
        private DevExpress.XtraEditors.PictureEdit Success;
    }
}
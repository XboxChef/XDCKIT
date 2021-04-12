using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace XDevkit.Dialogs
{
    public partial class XMessageEdit : XtraForm
    {
        public XMessageEdit()
        {
            InitializeComponent();
        }

        private void XMessageEdit_Load(object sender, EventArgs e)
        {
            Location = new Point(XMessageboxUI.Location.X + 1320, XMessageboxUI.Location.Y + 100);

        }

        private bool Is_Form_Loaded_Already(string FormName)
        {
            foreach (Form form_loaded in Application.OpenForms)
            {
                if (form_loaded.Text.IndexOf(FormName) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void CheckStateChanged(object sender, EventArgs e)
        {

            try
            {
                if (checkEdit2.Checked == true && checkEdit3.Checked == true)
                {
                    text2.Enabled = true;
                    Button2.Text = "Type Custom Text Here";

                    text3.Enabled = true;
                    Button3.Text = "Type Custom Text Here";
                }
                else if (checkEdit2.Checked == false && checkEdit3.Checked == false)
                {
                    text2.Enabled = false;

                    text3.Enabled = false;
                    Button2.Text = "disabled";
                    Button3.Text = "disabled";
                }
                else if (checkEdit2.Checked == false && checkEdit3.Checked == true)
                {
                    text2.Enabled = true;

                    Button2.Text = "Type Custom Text Here";
                    checkEdit3.Checked = false;
                }
                else if (checkEdit2.Checked == true)
                {
                    text2.Enabled = true;
                    text3.Enabled = false;
                    Button2.Text = "Type Custom Text Here";
                }

            }
            catch
            {

            }


        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {

        }


        private void TextController(object sender, EventArgs e)
        {
            var text = (TextEdit)sender;
            if (sender.Equals(text1))
            {
                Button1.Text = text.Text;
            }
            else if (sender.Equals(text2))
            {
                Button2.Text = text.Text;
            }
            else if (sender.Equals(text3))
            {
                Button3.Text = text.Text;
            }
        }

        private void ImageType_TextChanged(object sender, EventArgs e)
        {
            //if (ImageType.SelectedText == "Error")
            //{
            //    PictureType.SvgImage = Error.SvgImage;
            //}
            //else if (ImageType.SelectedText == "Warning")
            //{
            //    PictureType.SvgImage = Warning.SvgImage;
            //}
            //else if (ImageType.SelectedText == "Info")
            //{
            //    PictureType.SvgImage = Info.SvgImage;
            //}
            //else if (ImageType.SelectedText == "Success")
            //{
            //    PictureType.SvgImage = Success.SvgImage;
            //}
        }

        private void Type(object sender, EventArgs e)
        {
            if (ImageType.SelectedIndex == 0)
            {
                PictureType.SvgImage = Error.SvgImage;
            }
            else if (ImageType.SelectedIndex == 1)
            {
                PictureType.SvgImage = Info.SvgImage;
            }
            else if (ImageType.SelectedIndex == 2)
            {
                PictureType.SvgImage = Success.SvgImage;
            }
            else if (ImageType.SelectedIndex == 3)
            {
                PictureType.SvgImage = Warning.SvgImage;
            }
        }

        private void SendUI_Click(object sender, EventArgs e)
        {
            XNotify.XMessage(2, "shit", "fuck", 5, new[] { "my goodness" }, 4, 4);
        }
    }
}
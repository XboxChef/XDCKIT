using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XDevkit.Dialogs
{
    public partial class XMessageEdit : DevExpress.XtraEditors.XtraForm
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
    }
}
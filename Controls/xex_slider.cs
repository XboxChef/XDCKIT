namespace XDevkit
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class xex_slider : UserControl
    {
        public static Xbox Con;
        public Stream stream;
        private IContainer components = null;
        private Label label1;
        private uint Offset;
        private TrackBar trackBar1;
        private string Type;

        public xex_slider()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            label1 = new Label();
            trackBar1 = new TrackBar();
            trackBar1.BeginInit();
            base.SuspendLayout();
            label1.AutoSize = true;
            label1.Location = new Point(12, 11);
            label1.Name = "label1";
            label1.Size = new Size(0x21, 13);
            label1.TabIndex = 0;
            label1.Text = "name";
            trackBar1.Location = new Point(0xcc, 7);
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(0x144, 0x2d);
            trackBar1.TabIndex = 1;
            trackBar1.TickStyle = TickStyle.None;
            trackBar1.Scroll += new EventHandler(trackBar1_Scroll);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(trackBar1);
            base.Controls.Add(label1);
            base.Name = "xex_slider";
            base.Size = new Size(0x213, 0x23);
            trackBar1.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void setValues(string name, uint offset, float start, float range, string type)
        {
            Offset = offset;
            label1.Text = name;
            Type = type;
            trackBar1.SetRange(((int)start) * 10, ((int)range) * 10);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            float num = trackBar1.Value;
            num /= 10f;
            streampoke(Offset, Type, num.ToString());
        }
        public void streampoke(uint offset, string poketype, string ammount)
        {
            if (Con.IPAddress == "")
            {
                MessageBox.Show("XDK Name/IP not set");
            }
            else
            {
                Con.Connect();
                EndianIO nio = new EndianIO(stream, EndianType.BigEndian);
                nio.Open();
                nio.Out.BaseStream.Position = offset;
                if (poketype == "Float")
                {
                    nio.Out.Write(float.Parse(ammount));
                }
                if (poketype == "Double")
                {
                    nio.Out.Write(double.Parse(ammount));
                }
                if (poketype == "String")
                {
                    nio.Out.Write(ammount);
                }
                if (poketype == "Short")
                {
                    nio.Out.Write((short)Convert.ToUInt32(ammount, 0x10));
                }
                if (poketype == "Byte")
                {
                    nio.Out.Write((byte)Convert.ToUInt32(ammount, 0x10));
                }
                if (poketype == "Long")
                {
                    nio.Out.Write((long)Convert.ToUInt32(ammount, 0x10));
                }
                if (poketype == "Quad")
                {
                    nio.Out.Write((long)Convert.ToUInt64(ammount, 0x10));
                }
                if (poketype == "Int")
                {
                    nio.Out.Write(Convert.ToUInt32(ammount, 0x10));
                }
                nio.Close();
                stream.Close();
                Con.Disconnect();
            }
        }
    }
}


namespace XDevkit
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    public class xex_value : UserControl
    {
        public static Xbox Con;
        private Button button1;
        public Stream stream;
        private IContainer components = null;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem copyToolStripMenuItem;
        private Label label1;
        private Label label2;
        public string offset;
        private string origvalue;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripMenuItem revertValueToolStripMenuItem;
        private TextBox textBox1;

        public xex_value()
        {
            InitializeComponent();
            foreach (Control control in base.Controls)
            {
                control.ContextMenuStrip = contextMenuStrip1;
            }
        }
        public void pokeXbox(uint offset, string poketype, string ammount)
        {
            //if (!checkBox1.Checked)
            //{
            //    ammount = int.Parse(ammount).ToString("X");
            //}
            try
            {
                if (Con.IPAddress == "")
                {
                    MessageBox.Show("XDK Name/IP not set");
                }
                else
                {
                    if (!Con.Connected)
                    {
                        try
                        {
                            Con.Connect();
                        }
                        catch
                        {
                        }
                    }
                    EndianIO nio = new EndianIO(stream, EndianType.BigEndian);
                    nio.Open();
                    nio.Out.BaseStream.Position = offset;
                    if (poketype == "Unicode String")
                    {
                        nio.Out.WriteUnicodeString(ammount, ammount.Length);
                    }
                    if (poketype == "ASCII String")
                    {
                        nio.Out.WriteUnicodeString(ammount, ammount.Length);
                    }
                    if ((poketype == "String") | (poketype == "string"))
                    {
                        nio.Out.Write(ammount);
                    }
                    if ((poketype == "Float") | (poketype == "float"))
                    {
                        nio.Out.Write(float.Parse(ammount));
                    }
                    if ((poketype == "Double") | (poketype == "double"))
                    {
                        nio.Out.Write(double.Parse(ammount));
                    }
                    if ((poketype == "Short") | (poketype == "short"))
                    {
                        nio.Out.Write((short)Convert.ToUInt32(ammount, 0x10));
                    }
                    if ((poketype == "Byte") | (poketype == "byte"))
                    {
                        nio.Out.Write((byte)Convert.ToUInt32(ammount, 0x10));
                    }
                    if ((poketype == "Long") | (poketype == "long"))
                    {
                        nio.Out.Write((long)Convert.ToUInt32(ammount, 0x10));
                    }
                    if ((poketype == "Quad") | (poketype == "quad"))
                    {
                        nio.Out.Write((long)Convert.ToUInt64(ammount, 0x10));
                    }
                    if ((poketype == "Int") | (poketype == "int"))
                    {
                        nio.Out.Write(Convert.ToUInt32(ammount, 0x10));
                    }
                    if ((poketype == "Bytes") | (poketype == "bytes"))
                    {
                        nio.Out.Write(Functions.HexStringToBytes(ammount), 0, Functions.HexStringToBytes(ammount).Count());
                    }
                    nio.Close();
                    stream.Close();
                    Con.Disconnect();
                }
            }
            catch
            {
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            pokeXbox(Convert.ToUInt32(offset, 0x10), label2.Text, textBox1.Text);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.Text);
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
            components = new Container();
            label1 = new Label();
            textBox1 = new TextBox();
            label2 = new Label();
            button1 = new Button();
            contextMenuStrip1 = new ContextMenuStrip(components);
            revertValueToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem = new ToolStripMenuItem();
            pasteToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            base.SuspendLayout();
            label1.AutoSize = true;
            label1.Location = new Point(3, 5);
            label1.Name = "label1";
            label1.Size = new Size(0x21, 13);
            label1.TabIndex = 0;
            label1.Text = "offset";
            label1.Click += new EventHandler(label1_Click);
            textBox1.Location = new Point(0xa4, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(0x9a, 20);
            textBox1.TabIndex = 1;
            label2.AutoSize = true;
            label2.Location = new Point(0x144, 5);
            label2.Name = "label2";
            label2.Size = new Size(0x1b, 13);
            label2.TabIndex = 2;
            label2.Text = "type";
            button1.Location = new Point(0x165, 1);
            button1.Name = "button1";
            button1.Size = new Size(40, 20);
            button1.TabIndex = 3;
            button1.Text = "Poke";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new EventHandler(button1_Click);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { revertValueToolStripMenuItem, copyToolStripMenuItem, pasteToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(0x99, 0x5c);
            revertValueToolStripMenuItem.Name = "revertValueToolStripMenuItem";
            revertValueToolStripMenuItem.Size = new Size(0x98, 0x16);
            revertValueToolStripMenuItem.Text = "Revert Value";
            revertValueToolStripMenuItem.Click += new EventHandler(revertValueToolStripMenuItem_Click);
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new Size(0x98, 0x16);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += new EventHandler(copyToolStripMenuItem_Click);
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.Size = new Size(0x98, 0x16);
            pasteToolStripMenuItem.Text = "Paste";
            pasteToolStripMenuItem.Click += new EventHandler(pasteToolStripMenuItem_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(button1);
            base.Controls.Add(label2);
            base.Controls.Add(textBox1);
            base.Controls.Add(label1);
            base.Name = "xex_value";
            base.Size = new Size(400, 0x19);
            contextMenuStrip1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(offset);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = Clipboard.GetText();
        }

        private void revertValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = origvalue;
        }

        public void setvalues(string o, string v, string t, Dictionary<string, Items> itmlst)
        {
            offset = "0x" + o;
            origvalue = v;
            if (itmlst.ContainsKey("0x" + o))
            {
                o = itmlst["0x" + o].Name;
            }
            label1.Text = o;
            textBox1.Text = v;
            label2.Text = t;
        }
    }
}


using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace XDevkit.Controls
{
    partial class xex_value
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

        #endregion

        private Button button1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripMenuItem revertValueToolStripMenuItem;
        private TextBox textBox1;
        private Label label1;
        private Label label2;
    }
}

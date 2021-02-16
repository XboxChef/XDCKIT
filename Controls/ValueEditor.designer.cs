using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace XDevkit
{
    partial class ValueEditor
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
            components = new System.ComponentModel.Container();
            Address = new System.Windows.Forms.Label();
            Value = new System.Windows.Forms.TextBox();
            Type = new System.Windows.Forms.Label();
            Poke = new System.Windows.Forms.Button();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            revertValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            checkBox1 = new System.Windows.Forms.CheckBox();
            checkBox2 = new System.Windows.Forms.CheckBox();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // Address
            // 
            Address.AutoSize = true;
            Address.Location = new System.Drawing.Point(24, 5);
            Address.Name = "Address";
            Address.Size = new System.Drawing.Size(33, 13);
            Address.TabIndex = 0;
            Address.Text = "offset";
            Address.Click += new System.EventHandler(label1_Click);
            // 
            // Value
            // 
            Value.Location = new System.Drawing.Point(164, 3);
            Value.Name = "Value";
            Value.Size = new System.Drawing.Size(154, 20);
            Value.TabIndex = 1;
            // 
            // Type
            // 
            Type.AutoSize = true;
            Type.Location = new System.Drawing.Point(324, 5);
            Type.Name = "Type";
            Type.Size = new System.Drawing.Size(27, 13);
            Type.TabIndex = 2;
            Type.Text = "type";
            // 
            // Poke
            // 
            Poke.Location = new System.Drawing.Point(357, 1);
            Poke.Name = "Poke";
            Poke.Size = new System.Drawing.Size(40, 20);
            Poke.TabIndex = 3;
            Poke.Text = "Poke";
            Poke.UseVisualStyleBackColor = true;
            Poke.Click += new System.EventHandler(button1_Click);
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            revertValueToolStripMenuItem,
            copyToolStripMenuItem,
            pasteToolStripMenuItem});
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(139, 70);
            // 
            // revertValueToolStripMenuItem
            // 
            revertValueToolStripMenuItem.Name = "revertValueToolStripMenuItem";
            revertValueToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            revertValueToolStripMenuItem.Text = "Revert Value";
            revertValueToolStripMenuItem.Click += new System.EventHandler(revertValueToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += new System.EventHandler(copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            pasteToolStripMenuItem.Text = "Paste";
            pasteToolStripMenuItem.Click += new System.EventHandler(pasteToolStripMenuItem_Click);
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(143, 6);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(15, 14);
            checkBox1.TabIndex = 5;
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new System.Drawing.Point(3, 4);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new System.Drawing.Size(15, 14);
            checkBox2.TabIndex = 6;
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += new System.EventHandler(checkBox2_CheckedChanged);
            // 
            // xex_value
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(checkBox2);
            Controls.Add(checkBox1);
            Controls.Add(Poke);
            Controls.Add(Type);
            Controls.Add(Value);
            Controls.Add(Address);
            Name = "xex_value";
            Size = new System.Drawing.Size(400, 25);
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Button Poke;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripMenuItem revertValueToolStripMenuItem;
        private TextBox Value;
        private Label Address;
        private Label Type;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
    }
}

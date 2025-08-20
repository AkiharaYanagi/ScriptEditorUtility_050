namespace ScriptEditor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			textBox1 = new TextBox ();
			statusStrip1 = new StatusStrip ();
			toolStripStatusLabel1 = new ToolStripStatusLabel ();
			menuStrip1 = new MenuStrip ();
			フォルダToolStripMenuItem = new ToolStripMenuItem ();
			statusStrip1.SuspendLayout ();
			menuStrip1.SuspendLayout ();
			SuspendLayout ();
			// 
			// textBox1
			// 
			textBox1.Location = new Point ( 36, 72 );
			textBox1.Name = "textBox1";
			textBox1.Size = new Size ( 471, 23 );
			textBox1.TabIndex = 0;
			// 
			// statusStrip1
			// 
			statusStrip1.Items.AddRange ( new ToolStripItem [] { toolStripStatusLabel1 } );
			statusStrip1.Location = new Point ( 0, 314 );
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Size = new Size ( 547, 22 );
			statusStrip1.TabIndex = 1;
			statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			toolStripStatusLabel1.Size = new Size ( 118, 17 );
			toolStripStatusLabel1.Text = "toolStripStatusLabel1";
			// 
			// menuStrip1
			// 
			menuStrip1.Items.AddRange ( new ToolStripItem [] { フォルダToolStripMenuItem } );
			menuStrip1.Location = new Point ( 0, 0 );
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Size = new Size ( 547, 24 );
			menuStrip1.TabIndex = 2;
			menuStrip1.Text = "menuStrip1";
			// 
			// フォルダToolStripMenuItem
			// 
			フォルダToolStripMenuItem.Name = "フォルダToolStripMenuItem";
			フォルダToolStripMenuItem.Size = new Size ( 54, 20 );
			フォルダToolStripMenuItem.Text = "フォルダ";
			フォルダToolStripMenuItem.Click +=  フォルダToolStripMenuItem_Click ;
			// 
			// Form1
			// 
			AllowDrop = true;
			AutoScaleDimensions = new SizeF ( 7F, 15F );
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size ( 547, 336 );
			Controls.Add ( statusStrip1 );
			Controls.Add ( menuStrip1 );
			Controls.Add ( textBox1 );
			MainMenuStrip = menuStrip1;
			Name = "Form1";
			Text = "Form1";
			statusStrip1.ResumeLayout ( false );
			statusStrip1.PerformLayout ();
			menuStrip1.ResumeLayout ( false );
			menuStrip1.PerformLayout ();
			ResumeLayout ( false );
			PerformLayout ();
		}

		#endregion

		private TextBox textBox1;
		private StatusStrip statusStrip1;
		private ToolStripStatusLabel toolStripStatusLabel1;
		private MenuStrip menuStrip1;
		private ToolStripMenuItem フォルダToolStripMenuItem;
	}
}

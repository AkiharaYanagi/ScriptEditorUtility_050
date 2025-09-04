using System;
using System.Drawing;
using System.Windows.Forms;



namespace ScriptEditorUtility
{
	partial class EditListbox < T >  
	{
		/// <summary> 
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose ( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose ();
			}
			base.Dispose ( disposing );
		}

		#region コンポーネント デザイナーで生成されたコード

		/// <summary> 
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent ()
		{
			SuspendLayout ();
			// 
			// EditListbox
			// 
			AutoScaleDimensions = new SizeF ( 7F, 15F );
			AutoScaleMode = AutoScaleMode.Font;
			Name = "EditListbox";
			Size = new Size ( 225, 460 );
			ResumeLayout ( false );
		}

		#endregion



		//==========================================
		//ジェネリクス<T>によりデザイナが使えないため自前のコンポーネント初期化

		private void InitCmpnt ()
		{
			listBox1 = new ListBox ();
			Btn_Add = new Button ();
			Btn_Del = new Button ();
			Btn_Up = new Button ();
			Btn_Down = new Button ();
			Tb_Name = new TextBox ();
			panel1 = new Panel ();
			Btn_Tail = new Button ();
			Btn_Top = new Button ();
			Btn_SaveOne = new Button ();
			Btn_LoadOne = new Button ();
			Btn_SaveAll = new Button ();
			Btn_LoadAll = new Button ();
			Btn_Folder = new Button ();
			panel1.SuspendLayout ();
			SuspendLayout ();
			// 
			// listBox1
			// 
			listBox1.FormattingEnabled = true;
			listBox1.ItemHeight = 12;
			listBox1.Location = new Point ( 3, 10 );
			listBox1.Name = "listBox1";
			listBox1.Size = new Size ( 158, 352 );
			listBox1.TabIndex = 0;
			listBox1.SelectedIndexChanged += new EventHandler ( ListBox1_SelectedIndexChanged );
			// 
			// Btn_Add
			// 
			this.Btn_Add.Anchor = ( (AnchorStyles) ( ( AnchorStyles.Bottom | AnchorStyles.Left ) ) );
			this.Btn_Add.BackColor = Color.FromArgb ( ( (int) ( ( (byte) ( 192 ) ) ) ), ( (int) ( ( (byte) ( 255 ) ) ) ), ( (int) ( ( (byte) ( 255 ) ) ) ) );
			this.Btn_Add.Location = new Point ( 3, 484 );
			this.Btn_Add.Name = "Btn_Add";
			this.Btn_Add.Size = new Size ( 112, 34 );
			this.Btn_Add.TabIndex = 1;
			this.Btn_Add.Text = "追加";
			this.Btn_Add.UseVisualStyleBackColor = false;
			this.Btn_Add.Click += new EventHandler ( this.Btn_Add_Click );
			// 
			// Btn_Del
			// 
			this.Btn_Del.Anchor = ( (AnchorStyles) ( ( AnchorStyles.Bottom | AnchorStyles.Right ) ) );
			this.Btn_Del.BackColor = Color.FromArgb ( ( (int) ( ( (byte) ( 255 ) ) ) ), ( (int) ( ( (byte) ( 192 ) ) ) ), ( (int) ( ( (byte) ( 192 ) ) ) ) );
			this.Btn_Del.Location = new Point ( 120, 484 );
			this.Btn_Del.Name = "Btn_Del";
			this.Btn_Del.Size = new Size ( 43, 34 );
			this.Btn_Del.TabIndex = 2;
			this.Btn_Del.Text = "削除";
			this.Btn_Del.UseVisualStyleBackColor = false;
			this.Btn_Del.Click += new System.EventHandler ( this.Btn_Del_Click );
			// 
			// Btn_Up
			// 
			this.Btn_Up.Anchor = ( (AnchorStyles) ( ( ( AnchorStyles.Top | AnchorStyles.Bottom )
			| AnchorStyles.Right ) ) );
			this.Btn_Up.Location = new Point ( 167, 26 );
			this.Btn_Up.Name = "Btn_Up";
			this.Btn_Up.Size = new Size ( 30, 153 );
			this.Btn_Up.TabIndex = 3;
			this.Btn_Up.Text = "↑";
			this.Btn_Up.UseVisualStyleBackColor = true;
			this.Btn_Up.Click += new System.EventHandler ( this.Btn_Up_Click );
			// 
			// Btn_Down
			// 
			this.Btn_Down.Anchor = ( (AnchorStyles) ( ( ( AnchorStyles.Top | AnchorStyles.Bottom )
			| AnchorStyles.Right ) ) );
			this.Btn_Down.Location = new Point ( 167, 185 );
			this.Btn_Down.Name = "Btn_Down";
			this.Btn_Down.Size = new Size ( 30, 195 );
			this.Btn_Down.TabIndex = 3;
			this.Btn_Down.Text = "↓";
			this.Btn_Down.UseVisualStyleBackColor = true;
			this.Btn_Down.Click += new System.EventHandler ( this.Btn_Down_Click );
			// 
			// Tb_Name
			// 
			this.Tb_Name.Location = new Point ( 3, 92 );
			this.Tb_Name.Name = "Tb_Name";
			this.Tb_Name.Size = new Size ( 194, 19 );
			this.Tb_Name.TabIndex = 4;
			this.Tb_Name.Text = "Name";
			this.Tb_Name.TextChanged += new System.EventHandler ( this.Tb_Name_TextChanged );
			this.Tb_Name.KeyPress += new KeyPressEventHandler ( this.Tb_Name_KeyPress );
			// 
			// panel1
			// 
			this.panel1.Anchor = ( (AnchorStyles) ( ( ( ( AnchorStyles.Top | AnchorStyles.Bottom )
			| AnchorStyles.Left )
			| AnchorStyles.Right ) ) );
			this.panel1.Controls.Add ( this.Btn_Tail );
			this.panel1.Controls.Add ( this.Btn_Top );
			this.panel1.Controls.Add ( this.listBox1 );
			this.panel1.Controls.Add ( this.Btn_Up );
			this.panel1.Controls.Add ( this.Btn_Down );
			this.panel1.Location = new Point ( 0, 117 );
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size ( 208, 409 );
			this.panel1.TabIndex = 5;
			// 
			// Btn_Tail
			// 
			this.Btn_Tail.Font = new Font ( "MS UI Gothic", 6F, FontStyle.Regular, GraphicsUnit.Point, ( (byte) ( 128 ) ) );
			this.Btn_Tail.Location = new Point ( 167, 386 );
			this.Btn_Tail.Name = "Btn_Tail";
			this.Btn_Tail.Size = new Size ( 30, 15 );
			this.Btn_Tail.TabIndex = 4;
			this.Btn_Tail.Text = "↓↓";
			this.Btn_Tail.UseVisualStyleBackColor = true;
			this.Btn_Tail.Click += new System.EventHandler ( this.Btn_Tail_Click );
			// 
			// Btn_Top
			// 
			this.Btn_Top.Font = new Font ( "MS UI Gothic", 6F, FontStyle.Regular, GraphicsUnit.Point, ( (byte) ( 128 ) ) );
			this.Btn_Top.Location = new Point ( 167, 10 );
			this.Btn_Top.Name = "Btn_Top";
			this.Btn_Top.Size = new Size ( 30, 15 );
			this.Btn_Top.TabIndex = 4;
			this.Btn_Top.Text = "↑↑";
			this.Btn_Top.UseVisualStyleBackColor = true;
			this.Btn_Top.Click += new System.EventHandler ( this.Btn_Top_Click );
			// 
			// Btn_SaveOne
			// 
			this.Btn_SaveOne.BackColor = Color.FromArgb ( ( (int) ( ( (byte) ( 192 ) ) ) ), ( (int) ( ( (byte) ( 192 ) ) ) ), ( (int) ( ( (byte) ( 255 ) ) ) ) );
			this.Btn_SaveOne.Location = new Point ( 3, 28 );
			this.Btn_SaveOne.Name = "Btn_SaveOne";
			this.Btn_SaveOne.Size = new Size ( 89, 26 );
			this.Btn_SaveOne.TabIndex = 6;
			this.Btn_SaveOne.Text = "１項目書出";
			this.Btn_SaveOne.UseVisualStyleBackColor = false;
			this.Btn_SaveOne.Click += new System.EventHandler ( this.Btn_SaveOne_Click );
			// 
			// Btn_LoadOne
			// 
			this.Btn_LoadOne.BackColor = Color.FromArgb ( ( (int) ( ( (byte) ( 255 ) ) ) ), ( (int) ( ( (byte) ( 255 ) ) ) ), ( (int) ( ( (byte) ( 192 ) ) ) ) );
			this.Btn_LoadOne.Location = new Point ( 109, 27 );
			this.Btn_LoadOne.Name = "Btn_LoadOne";
			this.Btn_LoadOne.Size = new Size ( 88, 27 );
			this.Btn_LoadOne.TabIndex = 7;
			this.Btn_LoadOne.Text = "１項目読込";
			this.Btn_LoadOne.UseVisualStyleBackColor = false;
			this.Btn_LoadOne.Click += new System.EventHandler ( this.Btn_LoadOne_Click );
			// 
			// Btn_SaveAll
			// 
			this.Btn_SaveAll.BackColor = Color.FromArgb ( ( (int) ( ( (byte) ( 192 ) ) ) ), ( (int) ( ( (byte) ( 192 ) ) ) ), ( (int) ( ( (byte) ( 232 ) ) ) ) );
			this.Btn_SaveAll.Location = new Point ( 3, 59 );
			this.Btn_SaveAll.Name = "Btn_SaveAll";
			this.Btn_SaveAll.Size = new Size ( 89, 26 );
			this.Btn_SaveAll.TabIndex = 6;
			this.Btn_SaveAll.Text = "全項目書出";
			this.Btn_SaveAll.UseVisualStyleBackColor = false;
			this.Btn_SaveAll.Click += new System.EventHandler ( this.Btn_SaveAll_Click );
			// 
			// Btn_LoadAll
			// 
			this.Btn_LoadAll.BackColor = Color.FromArgb ( ( (int) ( ( (byte) ( 255 ) ) ) ), ( (int) ( ( (byte) ( 255 ) ) ) ), ( (int) ( ( (byte) ( 192 ) ) ) ) );
			this.Btn_LoadAll.Location = new Point ( 109, 61 );
			this.Btn_LoadAll.Name = "Btn_LoadAll";
			this.Btn_LoadAll.Size = new Size ( 88, 25 );
			this.Btn_LoadAll.TabIndex = 7;
			this.Btn_LoadAll.Text = "全項目読込";
			this.Btn_LoadAll.UseVisualStyleBackColor = false;
			this.Btn_LoadAll.Click += new System.EventHandler ( this.Btn_LoadAll_Click );
			// 
			// Btn_Folder
			// 
			this.Btn_Folder.Location = new Point ( 3, 3 );
			this.Btn_Folder.Name = "Btn_Folder";
			this.Btn_Folder.Size = new Size ( 194, 20 );
			this.Btn_Folder.TabIndex = 8;
			this.Btn_Folder.Text = "フォルダ";
			this.Btn_Folder.UseVisualStyleBackColor = true;
			this.Btn_Folder.Click += new System.EventHandler ( this.Btn_Folder_Click );
			// 
			// EditListbox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 12F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add ( this.Btn_Folder );
			this.Controls.Add ( this.Btn_LoadAll );
			this.Controls.Add ( this.Btn_SaveAll );
			this.Controls.Add ( this.Btn_LoadOne );
			this.Controls.Add ( this.Btn_SaveOne );
			this.Controls.Add ( this.Tb_Name );
			this.Controls.Add ( this.Btn_Del );
			this.Controls.Add ( this.Btn_Add );
			this.Controls.Add ( this.panel1 );
			this.Name = "EditListbox";
			this.Size = new System.Drawing.Size ( 232, 541 );
			this.panel1.ResumeLayout ( false );
			this.ResumeLayout ( false );
			this.PerformLayout ();
		}


		private ListBox listBox1;
		private Button Btn_Add;
		private Button Btn_Del;
		private Button Btn_Up;
		private Button Btn_Down;
		private TextBox Tb_Name;
		private Panel panel1;
		private Button Btn_SaveOne;
		private Button Btn_LoadOne;
		private Button Btn_SaveAll;
		private Button Btn_LoadAll;
		private Button Btn_Folder;
		private Button Btn_Tail;
		private Button Btn_Top;
	}
}

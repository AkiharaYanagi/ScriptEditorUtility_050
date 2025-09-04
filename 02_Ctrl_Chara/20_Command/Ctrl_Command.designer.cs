using ScriptEditorUtility;
using Chara050;


namespace ScriptEditor
{
	partial class Ctrl_Command
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
            this.components = new System.ComponentModel.Container();
            ScriptEditor.DispCommand dispCommand1 = new ScriptEditor.DispCommand();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ctrl_Command));
            ScriptEditor.SelectKey selectKey1 = new ScriptEditor.SelectKey();
            ScriptEditor.SelectKey selectKey2 = new ScriptEditor.SelectKey();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_LimitTime = new System.Windows.Forms.Label();
            this.Btn_Del = new System.Windows.Forms.Button();
            this.Btn_Add = new System.Windows.Forms.Button();
            this.RB_ON = new System.Windows.Forms.RadioButton();
            this.RB_RELE = new System.Windows.Forms.RadioButton();
            this.RB_PUSH = new System.Windows.Forms.RadioButton();
            this.RB_OFF = new System.Windows.Forms.RadioButton();
            this.RB_WILD = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Btn_KeyDown = new System.Windows.Forms.Button();
            this.Btn_KeyUp = new System.Windows.Forms.Button();
            this.CHK_Not = new System.Windows.Forms.CheckBox();
            this.RB_IS = new System.Windows.Forms.RadioButton();
            this.RB_NIS = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TBN_LimitTime = new ScriptEditorUtility.TB_Number();
            this.pb_Command1 = new ScriptEditor.Pb_Command(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Command1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(105, 457);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "[F]";
            // 
            // lbl_LimitTime
            // 
            this.lbl_LimitTime.AutoSize = true;
            this.lbl_LimitTime.Location = new System.Drawing.Point(2, 457);
            this.lbl_LimitTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_LimitTime.Name = "lbl_LimitTime";
            this.lbl_LimitTime.Size = new System.Drawing.Size(53, 12);
            this.lbl_LimitTime.TabIndex = 11;
            this.lbl_LimitTime.Text = "受付時間";
            // 
            // Btn_Del
            // 
            this.Btn_Del.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.Btn_Del.Location = new System.Drawing.Point(54, 416);
            this.Btn_Del.Name = "Btn_Del";
            this.Btn_Del.Size = new System.Drawing.Size(71, 24);
            this.Btn_Del.TabIndex = 13;
            this.Btn_Del.Text = "キー削除";
            this.Btn_Del.UseVisualStyleBackColor = false;
            this.Btn_Del.Click += new System.EventHandler(this.Btn_Del_Click);
            // 
            // Btn_Add
            // 
            this.Btn_Add.BackColor = System.Drawing.Color.LightSkyBlue;
            this.Btn_Add.Location = new System.Drawing.Point(0, 378);
            this.Btn_Add.Name = "Btn_Add";
            this.Btn_Add.Size = new System.Drawing.Size(125, 32);
            this.Btn_Add.TabIndex = 12;
            this.Btn_Add.Text = "キー追加";
            this.Btn_Add.UseVisualStyleBackColor = false;
            this.Btn_Add.Click += new System.EventHandler(this.Btn_Add_Click);
            // 
            // RB_ON
            // 
            this.RB_ON.Appearance = System.Windows.Forms.Appearance.Button;
            this.RB_ON.AutoSize = true;
            this.RB_ON.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.RB_ON.Image = global::Ctrl_Chara.Properties.Resources.Cmd_Key_On;
            this.RB_ON.Location = new System.Drawing.Point(264, 19);
            this.RB_ON.Name = "RB_ON";
            this.RB_ON.Size = new System.Drawing.Size(80, 43);
            this.RB_ON.TabIndex = 16;
            this.RB_ON.Text = "ON";
            this.RB_ON.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.RB_ON.UseVisualStyleBackColor = false;
            this.RB_ON.CheckedChanged += new System.EventHandler(this.RB_ON_CheckedChanged);
            // 
            // RB_RELE
            // 
            this.RB_RELE.Appearance = System.Windows.Forms.Appearance.Button;
            this.RB_RELE.AutoSize = true;
            this.RB_RELE.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.RB_RELE.Image = global::Ctrl_Chara.Properties.Resources.Cmd_Key_Rele;
            this.RB_RELE.Location = new System.Drawing.Point(178, 19);
            this.RB_RELE.Name = "RB_RELE";
            this.RB_RELE.Size = new System.Drawing.Size(80, 43);
            this.RB_RELE.TabIndex = 17;
            this.RB_RELE.Text = "RELE";
            this.RB_RELE.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.RB_RELE.UseVisualStyleBackColor = false;
            this.RB_RELE.CheckedChanged += new System.EventHandler(this.RB_RELE_CheckedChanged);
            // 
            // RB_PUSH
            // 
            this.RB_PUSH.Appearance = System.Windows.Forms.Appearance.Button;
            this.RB_PUSH.AutoSize = true;
            this.RB_PUSH.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.RB_PUSH.Image = global::Ctrl_Chara.Properties.Resources.Cmd_Key_PUSH;
            this.RB_PUSH.Location = new System.Drawing.Point(92, 18);
            this.RB_PUSH.Name = "RB_PUSH";
            this.RB_PUSH.Size = new System.Drawing.Size(80, 43);
            this.RB_PUSH.TabIndex = 18;
            this.RB_PUSH.Text = "PUSH";
            this.RB_PUSH.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.RB_PUSH.UseVisualStyleBackColor = false;
            this.RB_PUSH.CheckedChanged += new System.EventHandler(this.RB_PUSH_CheckedChanged);
            // 
            // RB_OFF
            // 
            this.RB_OFF.Appearance = System.Windows.Forms.Appearance.Button;
            this.RB_OFF.AutoSize = true;
            this.RB_OFF.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.RB_OFF.CheckAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.RB_OFF.Checked = true;
            this.RB_OFF.Image = global::Ctrl_Chara.Properties.Resources.Cmd_Key_Off;
            this.RB_OFF.Location = new System.Drawing.Point(6, 18);
            this.RB_OFF.Name = "RB_OFF";
            this.RB_OFF.Size = new System.Drawing.Size(80, 43);
            this.RB_OFF.TabIndex = 19;
            this.RB_OFF.TabStop = true;
            this.RB_OFF.Text = "OFF";
            this.RB_OFF.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.RB_OFF.UseVisualStyleBackColor = false;
            this.RB_OFF.CheckedChanged += new System.EventHandler(this.RB_OFF_CheckedChanged);
            // 
            // RB_WILD
            // 
            this.RB_WILD.Appearance = System.Windows.Forms.Appearance.Button;
            this.RB_WILD.AutoSize = true;
            this.RB_WILD.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.RB_WILD.Image = global::Ctrl_Chara.Properties.Resources.Cmd_Key_Wild;
            this.RB_WILD.Location = new System.Drawing.Point(6, 68);
            this.RB_WILD.Name = "RB_WILD";
            this.RB_WILD.Size = new System.Drawing.Size(80, 43);
            this.RB_WILD.TabIndex = 16;
            this.RB_WILD.Text = "WILD";
            this.RB_WILD.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.RB_WILD.UseVisualStyleBackColor = false;
            this.RB_WILD.CheckedChanged += new System.EventHandler(this.RB_Wild_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Btn_KeyDown);
            this.groupBox1.Controls.Add(this.Btn_KeyUp);
            this.groupBox1.Location = new System.Drawing.Point(140, 378);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(80, 118);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "レバー回転";
            // 
            // Btn_KeyDown
            // 
            this.Btn_KeyDown.Image = global::Ctrl_Chara.Properties.Resources.KeyTurnR;
            this.Btn_KeyDown.Location = new System.Drawing.Point(16, 68);
            this.Btn_KeyDown.Name = "Btn_KeyDown";
            this.Btn_KeyDown.Size = new System.Drawing.Size(51, 43);
            this.Btn_KeyDown.TabIndex = 15;
            this.Btn_KeyDown.UseVisualStyleBackColor = true;
            this.Btn_KeyDown.Click += new System.EventHandler(this.Btn_KeyDown_Click);
            // 
            // Btn_KeyUp
            // 
            this.Btn_KeyUp.Image = global::Ctrl_Chara.Properties.Resources.KeyTurnL;
            this.Btn_KeyUp.Location = new System.Drawing.Point(16, 18);
            this.Btn_KeyUp.Name = "Btn_KeyUp";
            this.Btn_KeyUp.Size = new System.Drawing.Size(51, 42);
            this.Btn_KeyUp.TabIndex = 14;
            this.Btn_KeyUp.UseVisualStyleBackColor = true;
            this.Btn_KeyUp.Click += new System.EventHandler(this.Btn_KeyUp_Click);
            // 
            // CHK_Not
            // 
            this.CHK_Not.AutoSize = true;
            this.CHK_Not.Location = new System.Drawing.Point(77, 480);
            this.CHK_Not.Name = "CHK_Not";
            this.CHK_Not.Size = new System.Drawing.Size(48, 16);
            this.CHK_Not.TabIndex = 23;
            this.CHK_Not.Text = "否定";
            this.CHK_Not.UseVisualStyleBackColor = true;
            this.CHK_Not.CheckedChanged += new System.EventHandler(this.CHK_Not_CheckedChanged);
            // 
            // RB_IS
            // 
            this.RB_IS.Appearance = System.Windows.Forms.Appearance.Button;
            this.RB_IS.AutoSize = true;
            this.RB_IS.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.RB_IS.Image = global::Ctrl_Chara.Properties.Resources.Cmd_Key_Is;
            this.RB_IS.Location = new System.Drawing.Point(92, 68);
            this.RB_IS.Name = "RB_IS";
            this.RB_IS.Size = new System.Drawing.Size(80, 43);
            this.RB_IS.TabIndex = 16;
            this.RB_IS.Text = "IS";
            this.RB_IS.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.RB_IS.UseVisualStyleBackColor = false;
            this.RB_IS.CheckedChanged += new System.EventHandler(this.RB_IS_CheckedChanged);
            // 
            // RB_NIS
            // 
            this.RB_NIS.Appearance = System.Windows.Forms.Appearance.Button;
            this.RB_NIS.AutoSize = true;
            this.RB_NIS.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.RB_NIS.Image = global::Ctrl_Chara.Properties.Resources.Cmd_Key_Nis;
            this.RB_NIS.Location = new System.Drawing.Point(178, 68);
            this.RB_NIS.Name = "RB_NIS";
            this.RB_NIS.Size = new System.Drawing.Size(80, 43);
            this.RB_NIS.TabIndex = 16;
            this.RB_NIS.Text = "NIS";
            this.RB_NIS.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.RB_NIS.UseVisualStyleBackColor = false;
            this.RB_NIS.CheckedChanged += new System.EventHandler(this.RB_NIS_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.RB_NIS);
            this.groupBox2.Controls.Add(this.RB_IS);
            this.groupBox2.Controls.Add(this.RB_ON);
            this.groupBox2.Controls.Add(this.RB_OFF);
            this.groupBox2.Controls.Add(this.RB_WILD);
            this.groupBox2.Controls.Add(this.RB_RELE);
            this.groupBox2.Controls.Add(this.RB_PUSH);
            this.groupBox2.Location = new System.Drawing.Point(226, 378);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(353, 118);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "キータイミング";
            // 
            // TBN_LimitTime
            // 
            this.TBN_LimitTime.GetFunc = null;
            this.TBN_LimitTime.Location = new System.Drawing.Point(61, 454);
            this.TBN_LimitTime.Name = "TBN_LimitTime";
            this.TBN_LimitTime.SetFunc = null;
            this.TBN_LimitTime.Size = new System.Drawing.Size(40, 19);
            this.TBN_LimitTime.TabIndex = 21;
            this.TBN_LimitTime.Text = "0";
            // 
            // pb_Command1
            // 
            this.pb_Command1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dispCommand1.Cmd = ((Command)(resources.GetObject("dispCommand1.Cmd")));
            selectKey1.Cmd = ((Command)(resources.GetObject("selectKey1.Cmd")));
            selectKey1.Frame = 0;
            selectKey1.Kind = ScriptEditor.SelectKey.KeyKind.ARROW;
            selectKey1.SelectedLvr = GameKeyData.Lever.LVR_N;
            selectKey1.Selecting = true;
            dispCommand1.SlctKey = selectKey1;
            this.pb_Command1.DspCmd = dispCommand1;
            this.pb_Command1.Location = new System.Drawing.Point(3, 3);
            this.pb_Command1.Name = "pb_Command1";
            this.pb_Command1.Size = new System.Drawing.Size(289, 480);
            selectKey2.Cmd = ((Command)(resources.GetObject("selectKey2.Cmd")));
            selectKey2.Frame = 0;
            selectKey2.Kind = ScriptEditor.SelectKey.KeyKind.ARROW;
            selectKey2.SelectedLvr = GameKeyData.Lever.LVR_N;
            selectKey2.Selecting = true;
            this.pb_Command1.SlctKey = selectKey2;
            this.pb_Command1.TabIndex = 25;
            this.pb_Command1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pb_Command1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(567, 369);
            this.panel1.TabIndex = 26;
            // 
            // Ctrl_Command
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CHK_Not);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.TBN_LimitTime);
            this.Controls.Add(this.Btn_Del);
            this.Controls.Add(this.Btn_Add);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_LimitTime);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Name = "Ctrl_Command";
            this.Size = new System.Drawing.Size(591, 504);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Command1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lbl_LimitTime;
		private System.Windows.Forms.Button Btn_Del;
		private System.Windows.Forms.Button Btn_Add;
		private System.Windows.Forms.RadioButton RB_ON;
		private System.Windows.Forms.RadioButton RB_RELE;
		private System.Windows.Forms.RadioButton RB_PUSH;
		private System.Windows.Forms.RadioButton RB_OFF;
		private System.Windows.Forms.Button Btn_KeyDown;
		private System.Windows.Forms.Button Btn_KeyUp;
		private ScriptEditorUtility.TB_Number TBN_LimitTime;
		private System.Windows.Forms.RadioButton RB_WILD;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox CHK_Not;
		private System.Windows.Forms.RadioButton RB_IS;
		private System.Windows.Forms.RadioButton RB_NIS;
		private System.Windows.Forms.GroupBox groupBox2;
		private Pb_Command pb_Command1;
		private System.Windows.Forms.Panel panel1;
	}
}

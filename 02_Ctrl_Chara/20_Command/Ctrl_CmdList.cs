using ScriptEditorUtility;
using Chara050;


namespace ScriptEditor
{
	using GK_L = GameKeyData.Lever;
	using GK_B = GameKeyData.Button;
	using GK_ST = GameKeyData.GameKeyState;

	public partial class Ctrl_CmdList :UserControl
	{
		//コマンドエディット
		private Ctrl_Command ctrl_Command1 = new Ctrl_Command ();

		//エディットリストボックス
		private EditListbox < Command > EL_Cmd = new EditListbox<Command> ();
		
		//設定ファイル
		private Settings_ctrl Ctrl_Stgs { get; set; } = new Settings_ctrl ();

		//コンストラクタ
		public Ctrl_CmdList ()
		{
			InitializeComponent ();

			//----------------------------------
			//コマンドエディット
			ctrl_Command1.Location = new Point ( 200, 0 );
			this.Controls.Add ( ctrl_Command1 );

			//----------------------------------
			//エディットリストボックス
			EL_Cmd.Location = new Point ( 3, 3 );
			this.Controls.Add ( EL_Cmd );



			//変更時の更新
			EL_Cmd.SelectedIndexChanged = ()=>
			{
				Command? cmd = EL_Cmd.Get ();
				if ( cmd == null ) { return; }
				ctrl_Command1.Set ( cmd );
			};
			EL_Cmd.UpdateData = ()=>
			{
				Command? cmd = EL_Cmd.Get ();
				if ( cmd == null ) { return; }
				ctrl_Command1.Set ( cmd );
			};

			//IO
			EL_Cmd.SetIOFunc ( SaveCommand, LoadCommand );
			EL_Cmd.Func_SavePath = s=>
			{
				Ctrl_Stgs.File_CommandList = s;
				XML_IO.Save ( Ctrl_Stgs );
			};
		}

		public void SetEnvironment ( Settings_ctrl stgs )
		{
			Ctrl_Stgs = stgs;
			EL_Cmd.SaveAllFileName = stgs.File_CommandList;
		}

		public void SetCharaData ( Chara ch )
		{
			EL_Cmd.SetData ( ch.charaset.BD_Command );
			Command? cmd = EL_Cmd.Get ();
			if ( cmd != null )
			{
				ctrl_Command1.Set ( cmd );
			}
		}

		//プレデータ読込
		public void LoadPreData ()
		{
			EL_Cmd.LoadData ( Ctrl_Stgs.File_CommandList );
		}


		//-------------------------------------------------------------------------
		//IO

		//単体保存
		public void SaveCommand ( object ob, StreamWriter sw )
		{
			Command cmd = (Command)ob;
			CommandToText ctt = new CommandToText();
			ctt.Do_Single ( sw, cmd );
		}

		//単体読込
		public void LoadCommand ( StreamReader sr )
		{
			Command cmd = new Command ();

			TextToCommand ttc = new TextToCommand ();
			ttc.Do_Single ( sr, cmd );
			EL_Cmd.Add ( cmd );
		}

		//上書保存
		public void SaveOverwrite ()
		{
			EL_Cmd.SaveOverwrite ();
		}

	}
}

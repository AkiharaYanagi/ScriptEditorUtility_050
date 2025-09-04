using ScriptEditorUtility;


namespace Chara050
{
	using GK_L = GameKeyData.Lever;
	using GK_B = GameKeyData.Button;
	using GK_ST = GameKeyData.GameKeyState;
	using BD_CMD = BindingDictionary < Command >;


	public class TextToCommand
	{
		public TextToCommand() {}

		public void Do_BD ( StreamReader sr, BD_CMD bd_cmd )
		{
			while ( ! sr.EndOfStream )
			{
				Command cmd = new Command ();
				Do_Single ( sr, cmd );
				bd_cmd.Add ( cmd );
			}
		}

		public void Do_Single ( StreamReader sr, Command cmd )
		{
			//テキスト成形
			string? str = sr.ReadLine ();
			if ( str == null ) { return; }
			string str_n = str.Substring ( 0, str.Length - 1 );
			string [] str_split = str_n.Split ( ',' );
			int index = 0;
			int size = str_split.Length;

			//名前
			cmd.Name = str_split [ index ++ ];

			//受付時間
			cmd.LimitTime = int.Parse ( str_split [ index ++ ] );

			//各フレームのゲームキー
			while ( index < size )
			{
				GameKeyCommand gkc = new GameKeyCommand ();

				//否定
				gkc.Not = bool.Parse ( str_split [ index ++ ] );

				//レバー
				string str_lvr = str_split [ index ++ ];
				for ( int i = 0; i < GameKeyData.LVR_NUM; ++ i )
				{
					string chLvr = str_lvr [ i ].ToString ();
					gkc.DctLvrSt [ (GK_L)i ] = (GK_ST)int.Parse ( chLvr );
				}

				//ボタン
				string str_btn = str_split [ index ++ ];
				for (int i = 0; i < GameKeyData.BTN_NUM; ++i)
				{
					string chBtn = str_btn [ i ].ToString ();
					gkc.DctBtnSt [ (GK_B)i ] = (GK_ST)int.Parse ( chBtn );
				}

				//コマンドに加える
				cmd.ListGameKeyCommand.Add ( gkc );
			}
		}
	}
}

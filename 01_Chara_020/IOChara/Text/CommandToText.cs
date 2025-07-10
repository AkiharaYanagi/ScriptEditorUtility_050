using System;
using System.IO;


namespace ScriptEditor
{
	using BD_CMD = BindingDictionary < Command >;
	using GK_L = GameKeyData.Lever;
	using GK_B = GameKeyData.Button;


	public class CommandToText
	{
		public CommandToText() { }

		public void Do_BD ( StreamWriter sw, BD_CMD bd_cmd )
		{
			foreach ( Command cmd in bd_cmd.GetEnumerable() )
			{
				Do_Single ( sw, cmd );
				sw.Write ( '\n' );
			}
		}

		public void Do_Single ( StreamWriter sw, Command cmd )
		{
			//名前
			sw.Write ( cmd.Name + ",");
			//受付時間
			sw.Write ( cmd.LimitTime.ToString () + "," );

			//ゲームキー
			foreach ( GameKeyCommand gkc in cmd.ListGameKeyCommand )
			{
				//否定
				sw.Write ( gkc.Not.ToString () + "," );

				//レバー
				foreach ( GK_L key in gkc.DctLvrSt.Keys )
				{
					sw.Write ( (int)( gkc.DctLvrSt [ key ] ) );
				}
				sw.Write ( "," );

				//ボタン
				foreach ( GK_B key in gkc.DctBtnSt.Keys )
				{
					sw.Write ( (int)( gkc.DctBtnSt [ key ] ) );
				}

				//ゲームキー区切り
				sw.Write ( "," );
			}
		}

	}
}

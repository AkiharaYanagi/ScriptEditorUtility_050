﻿using ScriptEditorUtility;

using SE2 = Chara020;
using SE5 = Chara050;

using GKD_LVR_2 = Chara020.GameKeyData.Lever;
using GKD_BTN_2 = Chara020.GameKeyData.Button;
using GKD_STT_2 = Chara020.GameKeyData.GameKeyState;

using GKD_LVR_5 = Chara050.GameKeyData.Lever;
using GKD_BTN_5 = Chara050.GameKeyData.Button;
using GKD_STT_5 = Chara050.GameKeyData.GameKeyState;


namespace test00_Chara
{
	internal partial class ConvertChara
	{
		//コマンドの変換
		public void ConvertCommand ( SE2.Chara ch020, SE5.Chara ch050 )
		{
			foreach ( SE2.Command? cmd2 in ch020.BD_Command.GetEnumerable () )
			{
				if (  cmd2 is null ) { continue; }

				//新規コマンド
				SE5.Command cmd5 = new SE5.Command ();

				//名前
				cmd5.Name = cmd2.Name;

				//ゲームキーコマンド
				foreach ( SE2.GameKeyCommand gkc2 in cmd2.ListGameKeyCommand )
				{
					SE5.GameKeyCommand gkc5 = new SE5.GameKeyCommand ();
					
					//レバー
					foreach ( var gkd_lvr in gkc2.DctLvrSt )
					{
						var stt = gkc2.DctLvrSt [ gkd_lvr.Key ];
						gkc5.DctLvrSt [ (GKD_LVR_5) gkd_lvr.Key ] = (GKD_STT_5) stt;
					}
					//ボタン
					foreach ( var gkd_btn in gkc2.DctBtnSt )
					{
						var stt = gkc2.DctBtnSt [ gkd_btn.Key ];
						gkc5.DctBtnSt [ (GKD_BTN_5) gkd_btn.Key ] = (GKD_STT_5) stt;
					}

					gkc5.Not = gkc2.Not;	//否定

					cmd5.ListGameKeyCommand.Add ( gkc5 );
				}

				//受付時間
				cmd5.LimitTime = cmd2.LimitTime;

				ch050.charaset.BD_Command.Add ( cmd5 );
			}

		}

		//ブランチの変換
		public void ConvertBranch ( SE2.Chara ch020, SE5.Chara ch050 )
		{
			foreach ( SE2.Branch? brc2 in ch020.BD_Branch.GetEnumerable () )
			{
				if (  brc2 is null ) { continue; }

				//新規ブランチ
				SE5.Branch brc5 = new SE5.Branch ();

				brc5.Name = brc2.Name;
				brc5.Condition = (SE5.BranchCondition)brc2.Condition;
				brc5.BD_NameCommand.Add ( new TName ( brc2.NameCommand ) );
				brc5.NameSequence = brc2.NameSequence;
				brc5.Frame = brc2.Frame;
				brc5.Other = brc2.Other;

				ch050.charaset.BD_Branch.Add ( brc5 );
			}

		}

		//ルートの変換
		public void ConvertRoute ( SE2.Chara ch020, SE5.Chara ch050 )
		{
			foreach ( SE2.Route? rut2 in ch020.BD_Route.GetEnumerable () )
			{
				if ( rut2 is null ) { continue; }

				//新規ルート
				SE5.Route rut5 = new SE5.Route ();

				rut5.Name = rut2.Name;
				rut5.Summary = rut2.Summary;
				foreach ( TName? tn in rut2.BD_BranchName.GetEnumerable () )
				{
					if ( tn is null ) { continue; }
					rut5.BD_BranchName.Add ( tn );
				}

				ch050.charaset.BD_Route.Add ( rut5 );
			}
		}

	}
}

using System.Drawing;
using System.Diagnostics;
using ScriptEditor;


namespace ScriptEditor020
{
	using GK_L = GameKeyData.Lever;
	using GK_B = GameKeyData.Button;
	using GK_ST = GameKeyData.GameKeyState;


	//==================================================================================
	//	LoadCharaBin
	//		キャラデータをbinaryで.datファイルから読込
	//		Document形式を介さない
	//==================================================================================
	public partial class LoadCharaImg
	{
		//ビヘイビア
		private void LoadBinBehavior ( BinaryReader br, Chara chara )
		{
			//Action : Sequence
			Behavior bhv = chara.behavior;

			//アクション個数
			uint N_Act = br.ReadUInt32 ();

			for ( uint i = 0; i < N_Act; ++ i )
			{
				Action act = new Action ()
				{
					Name = br.ReadString (),
					NextActionName = "Act_" + br.ReadUInt32 ().ToString(),	//uintから後に名前に変換する
					Category = (ActionCategory) br.ReadByte (),
					Posture = (ActionPosture) br.ReadByte (),
					HitNum = br.ReadByte (),
					HitPitch = br.ReadByte (),
					Balance = br.ReadInt32 (),

					Mana = br.ReadInt32 (),
					Accel = br.ReadInt32 (),
				};
				for (int v = 0; v < Action.VRS_SIZE; ++ v )
				{
					act.Versatile [ v ] = br.ReadInt32 ();
				}

				//[]スクリプト
				LoadBinListScript ( br, act.ListScript );

				bhv.BD_Sequence.Add ( act );
			}

			//すべてのアクションを読み込んでから、
			//"次アクション名" をIDからstringとして得る
			foreach ( Action? act in chara.behavior.BD_Sequence.GetEnumerable () )
			{
				if ( act is null ) { continue; }
				int nextActionID = GetIndex ( act.NextActionName, "Act_" );

				Action? a =chara.behavior[ nextActionID ];
				if ( a is null ) { continue; }
				act.NextActionName = a.Name;
			}
		}

		//ガーニッシュ
		private void LoadBinGarnish ( BinaryReader br, Chara chara )
		{
			//Effect : Sequence
			Garnish gns = chara.garnish;

			//エフェクト個数
			uint N_Efc = br.ReadUInt32 ();

			for ( uint i = 0; i < N_Efc; ++ i )
			{
				Effect efc = new Effect ()
				{
					Name = br.ReadString (),
				};

				//[]スクリプト
				LoadBinListScript ( br, efc.ListScript );

				gns.BD_Sequence.Add ( efc );
			}


			//エフェクト名の再指定
			BindingDictionary < Sequence > bdGns = chara.garnish.BD_Sequence;

			foreach ( Effect? efc in chara.garnish.BD_Sequence.GetEnumerable () )
			{
				if ( efc is null ) { continue; }

				foreach ( Script? scp in efc.ListScript )
				{
					if ( scp is null ) { continue; }

					//エフェクト名
					foreach ( EffectGenerate? efGnrt in scp.BD_EfGnrt.GetEnumerable() )
					{
						if ( efGnrt is null ) { continue; }	
						int idEf = GetIndex ( efGnrt.EfName, "Ef_" );
						efGnrt.EfName = bdGns [ idEf ]!.Name;
					}
				}
			}

			//アクションにおけるエフェクト名の再指定
			foreach ( Action? act in chara.behavior.BD_Sequence.GetEnumerable () )
			{
				if ( act is null ) { continue; }
				foreach ( Script? scp in act.ListScript )
				{
					if ( scp is null ) { continue; }

					//エフェクト名
					foreach ( EffectGenerate? efGnrt in scp.BD_EfGnrt.GetEnumerable() )
					{
						if ( efGnrt is null ) { continue; }

						int idEf = GetIndex ( efGnrt.EfName, "Ef_" );
						efGnrt.EfName = bdGns [ idEf ]!.Name;
					}
				}
			}

		}

		//スクリプトリスト
		private void LoadBinListScript ( BinaryReader br, List<Script> lscp )
		{
			//test
			//int start_group = 1;
			//int present_group = 1;

			//Debug.Write ( "\n");


			//スクリプト個数
			uint N_Scp = br.ReadUInt32 ();
			

			for ( uint i = 0; i < N_Scp; ++ i )
			{
				//スクリプト
				Script scp = new Script ();


				//フレーム数は数え上げながら設定する
				scp.Frame = (int)i;


				//----------------------------------------
				//グループ
#if false
				//test
				//アクションごとにグループのスタートナンバーを1から割り振る

				//一時保存グループと更新グループが重なるため、判定をマイナスで行う
				int temp_group = br.ReadInt32 ();
				temp_group *= -1;


				//最初の1回は無条件保存
				if ( i == 0 ) { present_group = temp_group; }
				else
				{
					//異なるとき更新
					if (present_group != temp_group)
					{
						present_group = temp_group;
						++ start_group;
					}
				}

				scp.Group = start_group;

				//出力
				Debug.Write ( "" + temp_group + "->" + scp.Group + ",");
#else
				
				scp.Group = br.ReadInt32 ();
#endif

				//----------------------------------------


#if false
				//イメージ名
				scp.ImgName = "Img_" + br.ReadUInt32 ().ToString ();	//後にイメージ名に変換
#else

				//イメージインデックス
				uint imgIndex = br.ReadUInt32 ();


				//イメージ名
				scp.ImgName = br.ReadString ();	//[utf-8]
#endif
				
				//表示位置
				scp.Pos = new Point ( br.ReadInt32 (), br.ReadInt32 () );

				//ルート名リスト
				uint nRut = br.ReadUInt32 ();
				for ( uint iRut = 0; iRut < nRut; ++ iRut )
				{
					//後にルート名に変換
					scp.BD_RutName.Add ( new TName ( "Rut_" + br.ReadUInt32 ().ToString () ) );
				}

				//枠リスト
				LoadBinListRect ( br, scp.ListCRect );
				LoadBinListRect ( br, scp.ListHRect );
				LoadBinListRect ( br, scp.ListARect );
				LoadBinListRect ( br, scp.ListORect );

				//エフェクト生成
				uint nEfGnrt = br.ReadUInt32 ();	//個数[uint]
				for ( uint iEG = 0; iEG < nEfGnrt; ++ iEG )
				{
					EffectGenerate efgnrt = new EffectGenerate ()
					{ 
						//エフェクト名は後で指定し直す
						EfName = "Ef_" + br.ReadUInt32 ().ToString(),
						Pt = new Point ( br.ReadInt32 (), br.ReadInt32 () ),
						Z_PER100F = br.ReadInt32 (),
						Gnrt = br.ReadBoolean (),
						Loop = br.ReadBoolean (),
						Sync = br.ReadBoolean (),
					};
					scp.BD_EfGnrt.Add ( efgnrt );
				}

				//バトルパラメータ
				ScriptParam_Battle btlPrm = new ScriptParam_Battle ()
				{
					CalcState = (CLC_ST)br.ReadInt32 (),
					Vel = new Point ( br.ReadInt32(), br.ReadInt32() ),
					Acc = new Point ( br.ReadInt32(), br.ReadInt32() ),
					Power = br.ReadInt32 (),
					Warp = br.ReadInt32 (),
					Recoil_I = br.ReadInt32 (),
					Recoil_E = br.ReadInt32 (),
					Blance_I = br.ReadInt32 (),
					Blance_E = br.ReadInt32 (),
					DirectDamage = br.ReadInt32 (),
				};
				scp.BtlPrm = btlPrm;

				//ステージングパラメータ
				ScriptParam_Staging stgPrm = new ScriptParam_Staging ()
				{
					BlackOut = br.ReadByte (),
					Vibration = br.ReadByte (),
					Stop = br.ReadByte (),
					Rotate = br.ReadInt32 (),
					Rotate_center = new Point ( br.ReadInt32(), br.ReadInt32() ),
					AfterImage_N = br.ReadByte (),
					AfterImage_time = br.ReadByte (),
					AfterImage_pitch = br.ReadByte (),
					Vibration_S = br.ReadByte (),
					Color = Color.FromArgb ( (int) br.ReadUInt32 () ),
					Color_time = br.ReadByte (),
					Scaling = new Point ( br.ReadInt32(), br.ReadInt32() ),
					SE = (int)br.ReadUInt32 (),
					SE_name = br.ReadString (),
					VC_name = br.ReadString (),
				};

//		stgPrm.SE_name = "";
//		stgPrm.VC_name = "";

				scp.StgPrm = stgPrm;


				//汎用パラメータ
				for ( uint indexVst = 0; indexVst < scp.Versatile.Length; ++ indexVst )
				{ 
					scp.Versatile [ indexVst ] = br.ReadInt32 ();
				}


				lscp.Add ( scp );
			}
		}

		//枠リスト
		private void LoadBinListRect ( BinaryReader br, List < Rectangle > lrct )
		{
			int N_Rct = br.ReadByte ();
			for ( int i = 0; i < N_Rct; ++ i )
			{
				int left = br.ReadInt32 ();
				int top = br.ReadInt32 ();
				int right = br.ReadInt32 ();
				int bottom = br.ReadInt32 ();
				Rectangle r = new Rectangle ( left, top, right - left, bottom - top );

				lrct.Add ( r );
			}
		}


		//--------------------------------------------------------------
		//コマンド
		private void LoadBinCommand ( BinaryReader br, Chara chara )
		{
			uint N = br.ReadUInt32 ();
			for ( uint i = 0; i < N; ++ i )
			{
				Command cmd = new Command ()
				{
					Name = br.ReadString (),
					LimitTime = br.ReadByte (),
				};

				//ゲームキー
				int N_Key = br.ReadByte ();

				for ( int iKey = 0; iKey < N_Key; ++ iKey )
				{
					GameKeyCommand gkc = new GameKeyCommand ()
					{
						Not = br.ReadBoolean (),
					};

					//レバー
					foreach ( GK_L gkl in Enum.GetValues ( typeof(GK_L) ) )
					{
						if ( gkl == GK_L.LVR_N ) { continue; }	//ゲームキー保存はしないので飛ばす
						gkc.DctLvrSt [ gkl ] = (GK_ST)br.ReadByte ();
					}

					//ボタン
					foreach ( GK_B gkb in Enum.GetValues ( typeof(GK_B) ) )
					{
						if ( gkb == GK_B.BTN_N ) { continue; }	//ゲームキー保存はしないので飛ばす
						gkc.DctBtnSt [ gkb ] = (GK_ST)br.ReadByte ();
					}
				
					cmd.ListGameKeyCommand.Add ( gkc );
				}

				chara.BD_Command.Add ( cmd );
			}
		}

		//ブランチ
		private void LoadBinBranch ( BinaryReader br, Chara chara )
		{
			uint N = br.ReadUInt32 ();
			for ( uint i = 0; i < N; ++ i )
			{
				Branch brc = new Branch ();
				
				brc.Name = br.ReadString ();
				brc.Condition = (BranchCondition)br.ReadByte ();
				brc.NameCommand = br.ReadString ();
				uint id_cmd = br.ReadUInt32 ();
				brc.NameSequence = br.ReadString ();
				uint id_sqc = br.ReadUInt32 ();
				brc.Frame = (int)br.ReadUInt32 ();
				brc.Other = br.ReadBoolean ();
				
				chara.BD_Branch.Add ( brc );
			}
		}

		//ルート
		private void LoadBinRoute ( BinaryReader br, Chara chara )
		{
			//ルート個数
			uint N = br.ReadUInt32 ();
			for ( uint i = 0; i < N; ++ i )
			{
				//ルート名
				Route rut = new Route ()
				{
					Name = br.ReadString (),
				};
				//ブランチ個数
				uint N_Brc = br.ReadUInt32 ();
				for ( uint iBrc = 0; iBrc < N_Brc; ++ iBrc )
				{
					//ブランチ
					uint brc_id = br.ReadUInt32 ();
					if ( brc_id > chara.BD_Branch.Count() )
					{
						brc_id = 0;
					}

					Branch? brc = chara.BD_Branch [ (int)brc_id ];

					TName t = new TName ( brc.Name );
					rut.BD_BranchName.Add ( t );
				}

				chara.BD_Route.Add ( rut );
			}

			//スクリプトにおけるルート名の再設定
			foreach ( Action? act in chara.behavior.BD_Sequence.GetEnumerable () )
			{
				if ( act is null ) {  continue; }

				foreach ( Script?	 scp in act.ListScript )
				{
					//名前だけのリストを作成
					List < string > L_name = new List<string> ();
					foreach ( TName? t in scp.BD_RutName.GetEnumerable () )
					{
						if ( t is null ) { continue; }
						int id = GetIndex ( t.Name, "Rut_" );
						L_name.Add ( chara.BD_Route [ id ]!.Name );
					}

					//クリアして再追加
					scp.BD_RutName.Clear ();
					foreach ( string name in L_name )
					{
						scp.BD_RutName.Add ( new TName ( name ) );
					}
				}
			}

		}


		//----------------------
		//str_indexからheadを除き、Int.Parse()して返す
		private int GetIndex ( string str_index, string head )
		{
			int n = head.Length;
			int nextActionID = 0;
			try
			{
				nextActionID = int.Parse ( str_index.Substring ( n, str_index.Length - n ) );
			}
			catch ( Exception e )
			{
				Debug.WriteLine ( e.Message );
				return 0;
			}
			return nextActionID;
		}
	}
}

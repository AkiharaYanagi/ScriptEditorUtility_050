using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace ScriptEditor
{
	using GK_ST = GameKeyData.GameKeyState;
	using GK_L = GameKeyData.Lever;
	using GK_B = GameKeyData.Button;

	using S_B = ATTR_SCP_BTL;
	using S_S = ATTR_SCP_STG;

	//==================================================
	//	ドキュメント型からキャラへ変換する
	//		主にLoadChara, LoadTextCharaで用いる
	//==================================================
	public class DocToChara
	{
		public void Load ( Document document, Chara chara )
		{
			//Root直下のChara操作用一時エレメント
			List < Element > listElementChara = document.Root.Elements [ 0 ].Elements;

			//--------------------------------------
			//イメージ部ヘッダ

			//メインイメージリスト
			Element elemImageList = listElementChara[ ( int ) ELEMENT_CHARA.MAIN_IMAGE_LIST ];
			LoadImageHeader ( elemImageList, chara.behavior );
			//Efイメージリスト
			Element elemEfImageList = listElementChara[ ( int ) ELEMENT_CHARA.EF_IMAGE_LIST ];
			LoadImageHeader ( elemEfImageList, chara.garnish );

			//--------------------------------------
			//スクリプト部
			LoadScriptData ( listElementChara, chara );
		}

		//イメージヘッダ読込
		private void LoadImageHeader ( Element elemImageList, Compend cmp )
		{
			//イメージ個数(イメージ読込時に使用)
			int numImage = int.Parse ( elemImageList.Attributes[ 0 ].Value );
			//イメージデータ(名前と実体)
			foreach ( Element elemImage in elemImageList.Elements )
			{
				string name = elemImage.Attributes[ 0 ].Value;
				ImageData imageData = new ImageData ( name, null );	//実体は空(後で読み込む)
				cmp.BD_Image.Add ( imageData );
			}
		}

		//キャラデータにおけるスクリプト部のみの読込
		public void LoadScriptData ( Document doc, Chara chara )
		{
			//Root直下のChara操作用一時エレメント
			List < Element > listElementChara = doc.Root.Elements [ 0 ].Elements;

			//イメージリストは飛ばし、スクリプト部のみ読込
			LoadScriptData ( listElementChara, chara );
		}

		//キャラデータにおけるスクリプト部の読込
		public void LoadScriptData ( List < Element > listElementChara, Chara chara )
		{
			//アクションリスト
			Element elemActionList = listElementChara[ ( int ) ELEMENT_CHARA.ACTION_LIST ];
			LoadActionList ( elemActionList, chara );

			//Efリスト
			Element elemEfList = listElementChara[ ( int ) ELEMENT_CHARA.EF_LIST ];
			LoadEffectList ( elemEfList, chara );

			//コマンドリスト
			Element elemCommandList = listElementChara[ ( int ) ELEMENT_CHARA.COMMAND_LIST ];
			LoadCommandList ( elemCommandList, chara );

			//ブランチリスト
			Element elemBranchList = listElementChara[ ( int ) ELEMENT_CHARA.BRANCH_LIST ];
			LoadBranchList ( elemBranchList, chara );

			//ルートリスト
			Element elemRouteList = listElementChara[ ( int ) ELEMENT_CHARA.ROUTE_LIST ];
			LoadRouteList ( elemRouteList, chara );
		}


		//アクションリスト読込
		private void LoadActionList ( Element elemActionList, Chara chara )
		{
			foreach ( Element elemAction in elemActionList.Elements )
			{
				//代入用
				Action action = new Action ();

				//アクション "名前"
				action.Name = elemAction.Attributes[ (int)ATTR_ACTION.ELAC_NAME ].Value;

				//"次アクション名" (終了時における次アクション)
				action.NextActionName = elemAction.Attributes [ (int)ATTR_ACTION.ELAC_NEXT_NAME ].Value;

				//"次アクションID" (終了時における次アクション)
				//action.NextActionID = elemAction.Attributes [ (int)ATTR_ACTION.ELAC_NEXT_ID ].Value;

				//アクション属性
				int nCategory = IOChara.AttrToInt ( elemAction, (int)ATTR_ACTION.ELAC_CATEGORY );
				action.Category = (ActionCategory) nCategory;

				//アクション体勢
				int nPosture = IOChara.AttrToInt ( elemAction, (int)ATTR_ACTION.ELAC_POSTURE );
				action.Posture = (ActionPosture) nPosture;

				//ヒット数
				action.HitNum = IOChara.AttrToInt ( elemAction, (int)ATTR_ACTION.ELAC_HITNUM );

				//ヒット間隔[F}
				action.HitPitch = IOChara.AttrToInt ( elemAction, (int)ATTR_ACTION.ELAC_HIT_PITCH );

				//バランス
				action.Balance = IOChara.AttrToInt ( elemAction, (int)ATTR_ACTION.ELAC_BALANCE );

				//子Element <Script> 数は不定
				ReadScriptList ( action, elemAction.Elements );

				//アクションに設定
				chara.behavior.BD_Sequence.Add ( action );
			}
		}

		//エフェクトリスト読込
		private void LoadEffectList ( Element elemActionList, Chara chara )
		{
			//<Effect>[]
			foreach ( Element elemEf in elemActionList.Elements )
			{
				//代入用
				Effect effect = new Effect ();

				//エフェクト "名前"
				effect.Name = elemEf.Attributes[ 0 ].Value;

				//子Element <Script> 数は不定
				ReadScriptList ( effect, elemEf.Elements );

				//エフェクトに設定
				chara.garnish.BD_Sequence.Add ( effect );
			}
		}


		//スクリプトリストの読込
		public void ReadScriptList ( Sequence sequence, List<Element> listElemnt )
		{
			//子Element <Script> 数は不定
			int frame = 0;	//フレーム数
			foreach ( Element e in listElemnt )
			{
				//代入用
				Script s = new Script ();
				s.Clear ();

				//フレーム数
				s.Frame = frame;

				//グループ
				s.Group = IOChara.AttrToInt ( e, (int)ATTR_SCP.GROUP );

				//イメージ名
				s.ImgName =  e.Attributes[ (int)ATTR_SCP.IMG_NAME ].Value;

				//イメージID 
				//スクリプトエディタでは用いない(GameMain用)
				//ATTR_SCP.IMG_ID

				//イメージ表示位置
				s.Pos = IOChara.AttrToPoint ( e, (int)ATTR_SCP.X, (int)ATTR_SCP.Y );

				//---------------------------------------------------------------
				//戦闘パラメータ
				ReadBtlPrm ( e, s.BtlPrm );

				//演出パラメータ
				ReadStgPrm ( e, s.StgPrm );

				//-----------------------------------------------------------------------------
				//Script以下のElement

				//ルートネームリスト
				Element elemRutList = e.Elements [( int ) ELEMENT_SCRIPT.ELSC_ROUTE ];
				foreach ( Element elemRut in elemRutList.Elements )
				{
					s.BD_RutName.Add ( new TName ( elemRut.Attributes [ 0 ].Value ) );
				}

				//-----------------------------------------------------------------------------
				//Efジェネレートリスト
				Element elemEfGnrtList = e.Elements[ ( int ) ELEMENT_SCRIPT.ELSC_EFGNRT ];
				foreach ( Element elmEfGnrt in elemEfGnrtList.Elements )
				{
					EffectGenerate efGnrt = new EffectGenerate ();
					List < Attribute > la = elmEfGnrt.Attributes;
					
					//Nameを読込
					efGnrt.Name = la[ (int)ELMT_EFGNRT.ELEG_NAME ].Value;

					//EfIDを読込
					efGnrt.EfName = la[ (int)ELMT_EFGNRT.ELEG_EFNAME ].Value;

					//PtGnrtを読込
					int pt_x = int.Parse ( la[ (int)ELMT_EFGNRT.ELEG_PT_X ].Value );
					int pt_y = int.Parse ( la[ (int)ELMT_EFGNRT.ELEG_PT_Y ].Value );
					efGnrt.Pt = new Point ( pt_x, pt_y );

					//Z位置
					efGnrt.Z_PER100F = IOChara.AttrToInt ( elmEfGnrt, (int)ELMT_EFGNRT.ELEG_PT_Z );

					//生成
					efGnrt.Gnrt = bool.Parse ( la[ (int)ELMT_EFGNRT.ELEG_GNRT ].Value );

					//繰返
					efGnrt.Loop = bool.Parse ( la[ (int)ELMT_EFGNRT.ELEG_LOOP ].Value );

					//位置同期
					efGnrt.Sync = bool.Parse ( la[ (int)ELMT_EFGNRT.ELEG_SYNC ].Value );

					//スクリプトに設定
					s.BD_EfGnrt.Add ( efGnrt );
				}

				//-----------------------------------------------------------------------------
				List<Element> le = e.Elements;
				ReadRect ( s.ListCRect, le[ ( int ) ELEMENT_SCRIPT.ELSC_CRECT ] );	/*接触枠*/
				ReadRect ( s.ListARect, le[ ( int ) ELEMENT_SCRIPT.ELSC_ARECT ] );	/*攻撃枠*/
				ReadRect ( s.ListHRect, le[ ( int ) ELEMENT_SCRIPT.ELSC_HRECT ] );	/*当り枠*/
				ReadRect ( s.ListORect, le[ ( int ) ELEMENT_SCRIPT.ELSC_ORECT ] );	/*相殺枠*/		
				//-----------------------------------------------------------------------------

				//シークエンスにスクリプトを追加
				sequence.ListScript.Add ( s );

				//フレームを一つ進める
				++frame;
			}
		}

		//バトルパラメータ
		private void ReadBtlPrm ( Element elemBtlPrm, ScriptParam_Battle btlPrm )
		{
			Element e = elemBtlPrm;

			//オフセット( 1 + 前項の末尾 )
			int s = 1 + (int)ATTR_SCP.Y;

			//計算状態
			int clcst = AtoI ( e, s + (int)S_B.CLC_ST );
			btlPrm.CalcState = (CLC_ST)clcst;

			//速度, 加速度
			btlPrm.Vel = AtoPt ( e, s + (int)S_B.VX, s + (int)S_B.VY );
			btlPrm.Acc = AtoPt ( e, s + (int)S_B.AX, s + (int)S_B.AY );

			//各種値
			btlPrm.Power = AtoI ( e, s + (int)S_B.POWER );		//攻撃値
			btlPrm.Warp = AtoI ( e, s + (int)S_B.WARP );		//ヒット時のけぞり[F]
			btlPrm.Recoil_I = AtoI ( e, s + (int)S_B.RECOIL_I );	//反動(x,y)(自分)
			btlPrm.Recoil_E = AtoI ( e, s + (int)S_B.RECOIL_E );	//反動(x,y)(相手)
			btlPrm.Blance_I = AtoI ( e, s + (int)S_B.BALANCE_I );	//バランス増減(自分)
			btlPrm.Blance_E = AtoI ( e, s + (int)S_B.BALANCE_E );	//バランス増減(相手)
		}

		//演出パラメータ
		private void ReadStgPrm ( Element elemStgPrm, ScriptParam_Staging stgPrm )
		{
			Element e = elemStgPrm;

			//オフセット( 1 + 前項の末尾 )
			int s = 1 + 1 + (int)ATTR_SCP.Y + (int)S_B.BALANCE_E;

			stgPrm.BlackOut		= AtoI ( e, s + (int)S_S.BLACKOUT );		//暗転[F]
			stgPrm.Vibration	= AtoI ( e, s + (int)S_S.VIBRATION );		//振動[F]
			stgPrm.Stop			= AtoI ( e, s + (int)S_S.STOP );			//停止[F]
			stgPrm.Rotate		= AtoI ( e, s + (int)S_S.ROTATE );			//回転[rad]
			stgPrm.Rotate_center = AtoPt ( e, s + (int)S_S.ROTATE_X, s + (int)S_S.ROTATE_Y );
			stgPrm.AfterImage_pitch	= AtoI ( e, s + (int)S_S.AFTERIMAGE_PITCH );	//残像[個]
			stgPrm.AfterImage_N		= AtoI ( e, s + (int)S_S.AFTERIMAGE_N );		//残像[F] 持続
			stgPrm.AfterImage_time	= AtoI ( e, s + (int)S_S.AFTERIMAGE_TIME );	//残像[F] pitch
			stgPrm.Vibration_S		= AtoI ( e, s + (int)S_S.VIBRATION_S );		//振動[F](個別)
			int indexColorName = s + (int)S_S.COLOR;
			string colorName = e.Attributes [ indexColorName ].Value;
			stgPrm.Color		= Color.FromName ( colorName );	//色調変更			
			stgPrm.Color_time	= AtoI ( e, s + (int)S_S.COLOR_TIME );				//色調変更[F]
			stgPrm.Scaling = AtoPt ( e,	s + (int)S_S.SCALING_X, s + (int)S_S.SCALING_Y );		//拡大縮小
			stgPrm.SE = AtoI ( e, s + (int)S_S.SE );
		}

		//枠の読込
		private void ReadRect ( List<Rectangle> listRect, Element elemRectList )
		{
			//枠
			foreach ( Element elemRect in elemRectList.Elements )
			{
				//新規作成
				int x = IOChara.AttrToInt ( elemRect, 0 );
				int y = IOChara.AttrToInt ( elemRect, 1 );
				int w = IOChara.AttrToInt ( elemRect, 2 );
				int h = IOChara.AttrToInt ( elemRect, 3 );
				Rectangle rect = new Rectangle ( x, y, w, h );

				//スクリプトに登録
				listRect.Add ( rect );
			}
		}


		//コマンドリスト読込
		private void LoadCommandList ( Element elemCommandList, Chara chara )
		{
			//個数
			int numCommand = int.Parse ( elemCommandList.Attributes[ 0 ].Value );

			//[]<Command>
			foreach ( Element elemCommand in elemCommandList.Elements )
			{
				//代入用新規作成
				Command command = new Command ();

				//attributeは２つ
				//コマンド "名前"
				command.Name = elemCommand.Attributes[ ( int ) ATTRIBUTE_COMMAND.NAME ].Value;

				//コマンド "制限時間"
				command.LimitTime = int.Parse ( elemCommand.Attributes[ ( int ) ATTRIBUTE_COMMAND.LIMIT_TIME ].Value );

				//子ElementはGameKeyを保持
				foreach ( Element elemKey in elemCommand.Elements )
				{
					//代入用新規作成
					GameKeyCommand gameKey = new GameKeyCommand ();

					//アトリビュートインデックス
					int atrbIndex = 0;

					//否定
					gameKey.Not = elemKey.Attributes[ atrbIndex ].Value.CompareTo ( "True" ) == 0;

					//レバー
					//@info foreach節で対象コンテナを書き換えるとエラーなのでforを用いる
					for ( int i = 0; i < GameKeyData.LVR_NUM; ++ i )
					{
						string strLvr = elemKey.Attributes [ ++ atrbIndex ].Value;
						gameKey.DctLvrSt [ (GK_L)i ] = ( GK_ST )Enum.Parse ( typeof ( GK_ST ), strLvr );
					}

					//ボタン
					for ( int i = 0; i < GameKeyData.BTN_NUM; ++ i )
					{
						string v = elemKey.Attributes[ ++ atrbIndex ].Value;
						gameKey.DctBtnSt [ (GK_B)i ] = ( GK_ST ) Enum.Parse ( typeof ( GK_ST ), v );
					}

					//コマンドに加える
					command.ListGameKeyCommand.Add ( gameKey );
				}

				//キャラに登録
				chara.BD_Command.Add ( command );
			}
		}


		//ブランチリスト読込
		private void LoadBranchList ( Element elemBranchList, Chara chara )
		{
			//[]<Branch>
			foreach ( Element elemBrc in elemBranchList.Elements )
			{
				//代入用新規作成
				Branch brc = new Branch
				{
					Name = elemBrc.Attributes [ ( int ) ATTR_BRANCH.NAME ].Value,
					Condition = (BranchCondition) IOChara.AttrToInt ( elemBrc, (int)ATTR_BRANCH.CONDITION ),
					//CMD_IDは飛ばす
					NameCommand = elemBrc.Attributes [ ( int ) ATTR_BRANCH.CMD_NAME ].Value,
					//SQC_IDは飛ばす
					NameSequence = elemBrc.Attributes [ ( int ) ATTR_BRANCH.SQC_NAME ].Value,
					Frame = AtoI ( elemBrc, (int)ATTR_BRANCH.FRAME ),
					Other = elemBrc.Attributes [ ( int ) ATTR_BRANCH.OTHER ].Value.CompareTo ( "True" ) == 0
				};

				//キャラに登録
				chara.BD_Branch.Add ( brc );
			}
		}

		//ルートリスト読込
		private void LoadRouteList ( Element elemRouteList, Chara chara )
		{
			//[]<Route>
			foreach ( Element elemRut in elemRouteList.Elements )
			{
				//代入用新規作成
				Route rut = new Route
				{
					Name = elemRut.Attributes [ (int)ATTR_ROUTE.NAME ].Value,
					Summary = elemRut.Attributes [ (int)ATTR_ROUTE.SUMMARY ].Value,
				};

				//ブランチネームリスト
				Element elemBranchNameList = elemRut.Elements [ 0 ];

				//個数
				int numBrunchName = int.Parse ( elemBranchNameList.Attributes[ 0 ].Value );

				foreach ( Element elemBrcName in elemBranchNameList.Elements )
				{
					string name = elemBrcName.Attributes [ (int)ATTR_.NAME_0 ].Value;
					rut.BD_BranchName.Add ( new TName ( name ) );
				}

				//キャラに登録
				chara.BD_Route.Add ( rut );
			}
		}

		//------------------------------------------------------
		//簡略化関数
		private int AtoI ( Element e, int attr )
		{
			return IOChara.AttrToInt ( e, attr );
		}

		private Point AtoPt ( Element e, int enumName0, int enumName1 )
		{
			return IOChara.AttrToPoint ( e, enumName0, enumName1 );
		}
		//------------------------------------------------------

	}
}

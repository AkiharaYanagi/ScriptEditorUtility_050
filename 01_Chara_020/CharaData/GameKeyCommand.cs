
namespace Chara020
{
	//------------------------------------------------------------------
	//	ゲームキー　コマンド
	//------------------------------------------------------------------
	// ◆格闘ゲーム入力におけるキャラの入力
	// ◆レバーは内部的に全方位別で記録されるが、外部からのコマンド的には１方向を指定する
	// ◆方向は(右正)、左向き時は <1-3>,<4-6>,<7-9>,を入れ替えて判定
	// ◆コマンドとして「否定」フラグとは「指定のゲームキー状態でなくなったとき」
	//------------------------------------------------------------------
	// ◆方向要素入力(2E, 6E, 8E, 4E)も判定する(いずれかでも成立)
	//下方向要素(2E)	前方向要素(6E)	上方向要素(8E)	後方向要素(4E)
	//  * * *		  * * 9			  7 8 9			  7 * *
	//  * * *		  * * 6			  * * *			  4 * *
	//  1 2 3		  * * 3			  * * *			  1 * *
	//------------------------------------------------------------------
	//
	// ◆判定は7種類
	//	[__](押した状態)		
	//	[--](離した状態)		
	//	[-_](押した瞬間)		
	//	[_-](離した瞬間)		
	//	[**](どの状態でも)	
	//	[*_] 現在のみ押した状態
	//	[*-] 現在のみ離した状態
	// 
	//-------------------------------------------------------------------

	using GK_ST = GameKeyData.GameKeyState;
	using GK_L = GameKeyData.Lever;
	using GK_B = GameKeyData.Button;
	using DCT_L_ST = Dictionary < GameKeyData.Lever, GameKeyData.GameKeyState >;
	using DCT_B_ST = Dictionary < GameKeyData.Button, GameKeyData.GameKeyState >;

	//====================================================================
	//1[F]のコマンド条件に用いるクラス
	//レバー全方向の各情報を持つ
	//キャラ状態を受けて、その中の向きを以て前後の判定
	//～でない（否定）の条件
	//====================================================================
	[System.Serializable]
	public class GameKeyCommand
	{
		//======================================================================================
		//レバー判定状態 
		public DCT_L_ST DctLvrSt = new DCT_L_ST ();

		//---------------------------------------------------
		//ボタン判定状態 
		public DCT_B_ST DctBtnSt = new DCT_B_ST ();

		//---------------------------------------------------
		//否定のフラグ
		public bool Not { get; set; } = false;


		//======================================================================================
		//コンストラクタ
		public GameKeyCommand ()
		{
			//レバー
			DctLvrSt.Add ( GK_L.LVR_1, GK_ST.KEY_WILD );
			DctLvrSt.Add ( GK_L.LVR_2, GK_ST.KEY_WILD );
			DctLvrSt.Add ( GK_L.LVR_3, GK_ST.KEY_WILD );
			DctLvrSt.Add ( GK_L.LVR_6, GK_ST.KEY_WILD );
			DctLvrSt.Add ( GK_L.LVR_9, GK_ST.KEY_WILD );
			DctLvrSt.Add ( GK_L.LVR_8, GK_ST.KEY_WILD );
			DctLvrSt.Add ( GK_L.LVR_7, GK_ST.KEY_WILD );
			DctLvrSt.Add ( GK_L.LVR_4, GK_ST.KEY_WILD );
//			DctLvrSt.Add ( GK_L.LVR_N, GK_ST.KEY_WILD );

			//ボタン
			DctBtnSt.Add ( GK_B.BTN_0, GK_ST.KEY_WILD );
			DctBtnSt.Add ( GK_B.BTN_1, GK_ST.KEY_WILD );
			DctBtnSt.Add ( GK_B.BTN_2, GK_ST.KEY_WILD );
			DctBtnSt.Add ( GK_B.BTN_3, GK_ST.KEY_WILD );
			DctBtnSt.Add ( GK_B.BTN_4, GK_ST.KEY_WILD );
			DctBtnSt.Add ( GK_B.BTN_5, GK_ST.KEY_WILD );
			DctBtnSt.Add ( GK_B.BTN_6, GK_ST.KEY_WILD );
			DctBtnSt.Add ( GK_B.BTN_7, GK_ST.KEY_WILD );
		}


		//---------------------------------------------------

		//レバー設定(隣接無し)
		//  7 8 9
		//  4 N 6
		//  1→2 3
		public void SetLever ( GK_L gkl )
		{
			switch ( gkl )
			{
//			case GK_L.LVR_1: SetNeighbor ( GK_L.LVR_1, GK_L.LVR_4, GK_L.LVR_2 ); break;
			case GK_L.LVR_1: DctLvrSt [ GK_L.LVR_1 ] = GK_ST.KEY_IS; break;
			case GK_L.LVR_2: SetNeighbor ( GK_L.LVR_2, GK_L.LVR_1, GK_L.LVR_3 ); break;
//			case GK_L.LVR_3: SetNeighbor ( GK_L.LVR_3, GK_L.LVR_2, GK_L.LVR_6 ); break;
			case GK_L.LVR_3: DctLvrSt [ GK_L.LVR_3 ] = GK_ST.KEY_IS; break;
			case GK_L.LVR_6: SetNeighbor ( GK_L.LVR_6, GK_L.LVR_3, GK_L.LVR_9 ); break;
//			case GK_L.LVR_9: SetNeighbor ( GK_L.LVR_9, GK_L.LVR_8, GK_L.LVR_6 ); break;
			case GK_L.LVR_9: DctLvrSt [ GK_L.LVR_9 ] = GK_ST.KEY_IS; break;
			case GK_L.LVR_8: SetNeighbor ( GK_L.LVR_8, GK_L.LVR_7, GK_L.LVR_9 ); break;
//			case GK_L.LVR_7: SetNeighbor ( GK_L.LVR_7, GK_L.LVR_4, GK_L.LVR_8 ); break;
			case GK_L.LVR_7: DctLvrSt [ GK_L.LVR_7 ] = GK_ST.KEY_IS; break;
			case GK_L.LVR_4: SetNeighbor ( GK_L.LVR_4, GK_L.LVR_1, GK_L.LVR_7 ); break;
			}
		}

		private void SetNeighbor ( GK_L gkl_on, GK_L gkl_off0, GK_L gkl_off1 )
		{
			ClearLever ();
			DctLvrSt [ gkl_off0 ] = GK_ST.KEY_NIS;
			DctLvrSt [ gkl_on ] = GK_ST.KEY_IS;	//斜め入力を排除するため隣接はNIS
			DctLvrSt [ gkl_off1 ] = GK_ST.KEY_NIS;
		}

		//レバー状態指定を取得
		public GK_ST GetLvrSt ( GK_L gk_l )
		{
			return DctLvrSt [ gk_l ];
		}

		//レバー状態指定を設定
		public void SetLvrSt ( GK_ST st, GK_L gk_l )
		{
			DctLvrSt [ gk_l ] = st;
		}

		//すべてのレバーがワイルドかどうか
		public bool AreAllLvrWild ()
		{
			foreach ( GK_L key in DctLvrSt.Keys )
			{
				if ( DctLvrSt [ key ] == GK_ST.KEY_WILD ) { return false; }
			}
			return true;
		}

		//レバー状態初期化
		private void ClearLever ()
		{
			foreach ( GK_L key in DctLvrSt.Keys )
			{
				DctLvrSt [ key ] = GK_ST.KEY_WILD;
			}
		}

		//--------------------------------------------------------------
		//レバー回転
		public void Lever_R ()
		{
			//右方向にシフト
			//  7 8→9
			//  4 N 6
			//  1←2 3
			
			//12369874
			GK_ST st1 = DctLvrSt [ GK_L.LVR_1 ];	//初期値保存
			for ( int i = (int)GK_L.LVR_1; i < (int)GK_L.LVR_4; ++ i )
			{
				DctLvrSt [ (GK_L)i ] = DctLvrSt [ (GK_L)(i + 1) ];
			}
			DctLvrSt [ GK_L.LVR_4 ] = st1;
		}

		public void Lever_L ()
		{
			//  7 8←9
			//  4 N 6
			//  1→2 3
			
			//47896321
			GK_ST st4 = DctLvrSt [ GK_L.LVR_4 ];	//初期値保存
			for ( int i = (int)GK_L.LVR_4; i > (int)GK_L.LVR_1; -- i )
			{
				DctLvrSt [ (GK_L)i ] = DctLvrSt [ (GK_L)(i - 1) ];
			}
			DctLvrSt[ (int)GK_L.LVR_1 ] = st4;
		}
		//--------------------------------------------------------------

		//======================================================================
		//比較
		//thisの状態がチェックするコマンド条件、引数がプレイヤ入力
		//引数：比較するゲームキー状態, キャラクタ向き(右正)
		//戻値：適合したらtrue、それ以外はfalse
		public bool CompareTarget ( GameKeyData gameKeyData, bool dirRight )
		{
			const int LN = GameKeyData.LVR_NUM;
			const int BN = GameKeyData.BTN_NUM;

			//チェック用(反転などの操作をするためにディープコピーとする)
			GameKeyData gk = new GameKeyData ( gameKeyData );

			//比較するかどうか(条件がワイルドの時は比較しない)
			bool[] bWildLvr = new bool [ LN ];
			bool[] bWildBtn = new bool[ BN ];

			//比較結果
			bool[] b_Lvr = new bool [ LN ];
			bool[] b_Btn = new bool[ BN ];

			InitBoolArray ( bWildLvr, LN );
			InitBoolArray ( bWildBtn, BN );
			InitBoolArray ( b_Lvr, LN );
			InitBoolArray ( b_Btn, BN );

			//-----------------------------------------------------------------
			//左向きのとき左右を入れ替え
			FlipData ( gk, dirRight );

			//-----------------------------------------------------------------
			//比較して結果を保存
			//レバー
			CompareKey ( LN, DctLvrSt, bWildLvr, b_Lvr, gameKeyData.Lvr, gameKeyData.PreLbr );
			//ボタン
			CompareKey ( BN, DctBtnSt, bWildBtn, b_Btn, gameKeyData.Btn, gameKeyData.PreBtn );

			//-----------------------------------------------------------------
			//まとめ

			//すべてワイルドの場合trueを返す
			if ( AreAllTrue ( bWildLvr ) && AreAllTrue ( bWildBtn ) ) { return true; }

			//いずれかを返す場合
			bool ret = true;

			//レバー
			ret &= CheckWild ( LN, bWildLvr, b_Lvr );

			//ボタン
			ret &= CheckWild ( BN, bWildBtn, b_Btn );

			//否定の場合は反転して返す (排他的論理和)
			return ret ^ this.Not;
		}

		//---------------------------------------------------------------------------
		//bool配列初期化
		private void InitBoolArray ( bool[] ary, int length )
		{
			for ( int i = 0; i < length; ++ i )
			{
				ary [ i ] = false;
			}
		}

		//キーデータ左右入替
		private void FlipData ( GameKeyData gk, bool dirRight )
		{
			//左向きのとき左右を入れ替え
			//7<-->9
			//4<-->6
			//1<-->3
			if ( ! dirRight )
			{
				bool tempBool;

				tempBool = gk.Lvr [ (int)GK_L.LVR_1 ];
				gk.Lvr [ (int)GK_L.LVR_1 ] = gk.Lvr [ (int)GK_L.LVR_3 ];
				gk.Lvr [ (int)GK_L.LVR_3 ] = tempBool;

				tempBool = gk.Lvr [ (int)GK_L.LVR_4 ];
				gk.Lvr [ (int)GK_L.LVR_4 ] = gk.Lvr [ (int)GK_L.LVR_6 ];
				gk.Lvr [ (int)GK_L.LVR_6 ] = tempBool;

				tempBool = gk.Lvr [ (int)GK_L.LVR_7 ];
				gk.Lvr [ (int)GK_L.LVR_7 ] = gk.Lvr [ (int)GK_L.LVR_9 ];
				gk.Lvr [ (int)GK_L.LVR_9 ] = tempBool;
			}
		}

		//すべてtrueかどうか
		private bool AreAllTrue ( bool [] bAry )
		{
			foreach ( bool b in bAry )
			{
				if ( ! b ) { return false; }
			}
			return true;
		}


		//比較
		private void CompareKey ( int num, DCT_L_ST dct, bool[] bWildAry, bool[] bResultAry, bool[] bDataAry, bool[] bPreAry )
		{
			int i = 0;
			foreach ( GK_L key in dct.Keys )
			{
				//一時変数
				bool b = bDataAry[i];
				bool pb = bPreAry[i];
				
				switch ( dct [ key ] )
				{
				//条件がワイルドの場合は比較しない
				case GK_ST.KEY_WILD: bWildAry[i] = true; break;
				//各種状態で結果を決める
				case GK_ST.KEY_ON  : bResultAry[i] = b; break;
				case GK_ST.KEY_OFF : bResultAry[i] = ! b; break;
				case GK_ST.KEY_PUSH: bResultAry[i] = b && ! pb; break;
				case GK_ST.KEY_RELE: bResultAry[i] = ! b && pb; break;
				}

				++ i;
			}
		}

		private void CompareKey ( int num, DCT_B_ST dct, bool[] bWildAry, bool[] bResultAry, bool[] bDataAry, bool[] bPreAry )
		{
			int i = 0;
			foreach ( GK_B key in dct.Keys )
			{
				bool b = bDataAry[i];
				bool pb = bPreAry[i];

				switch ( dct [ key ] )
				{
				//条件がワイルドの場合は比較しない
				case GK_ST.KEY_WILD: bWildAry[i] = true; break;
				case GK_ST.KEY_ON  : bResultAry[i] = b; break;
				case GK_ST.KEY_OFF : bResultAry[i] = ! b; break;
				case GK_ST.KEY_PUSH: bResultAry[i] = b && ! pb; break;
				case GK_ST.KEY_RELE: bResultAry[i] = ! b && pb; break;
				}

				++ i;
			}
		}

		//すべて調査対象かつ適合かどうか
		private bool CheckWild ( int num, bool [] bWildAry, bool [] bCheckAry )
		{
			for ( int i = 0; i < num; ++ i )
			{
				//一つでも調査対象が不適であったらその時点でfalse
				if ( bWildAry[i] ) { if ( bCheckAry[i] ) { return false; } }
			}			
			//すべて適合だったらtrue
			return true;
		}

		//======================================================================================
		public override bool Equals ( object? obj )
		{
			//(Object)型で比較する
			//nullまたは型が異なるときfalse
			if ( null == obj || this.GetType () != obj.GetType () ) { return false; }

			//キャストして比較
			GameKeyCommand g = (GameKeyCommand)obj;
			
			if ( ! this.DctLvrSt.SequenceEqual ( g.DctLvrSt ) ) { return false; }
			if ( ! this.DctBtnSt.SequenceEqual ( g.DctBtnSt ) ) { return false; }
			if ( ! (this.Not == g.Not) ) { return false; }

			return true;
		}

		public override int GetHashCode ()
		{
			int i0 = DctLvrSt.GetHashCode ();
			int i2 = i0 ^ DctBtnSt.GetHashCode ();
			int i3 = i2 ^ Not.GetHashCode ();
			return i3;
		}

	}
}


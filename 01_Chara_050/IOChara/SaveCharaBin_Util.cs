using ScriptEditorUtility;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace Chara050
{
	using BD_EfGnrt = BindingDictionary<EffectGenerate>;
	using BD_Gnrt = BindingDictionary<Generator>;
	using BD_T = BindingDictionary<TName>;


	//==================================================================================
	//	SaveCharaBin_Util
	//		外部関数
	//==================================================================================
	public static class SaveCharaBin_Util
	{
        //Compend
        public static void SaveBinCompend ( BinaryWriter bw, Chara chara, Compend cmpd )
        {
            uint nAct = (uint)cmpd.BD_Sequence.Count();
            bw.Write(nAct);

            //Sequence
            foreach (Sequence? sqc in cmpd.BD_Sequence.GetEnumerable())
            {
				if ( sqc is null ) { continue; }

                bw.Write(sqc.Name);     //string (length , [UTF8])

				ActionParam ap = sqc.ActPrm;
                bw.Write((uint)chara.charaset.GetIndexOfAction(ap.NextActionName));
                bw.Write((byte)ap.Category);
                bw.Write((byte)ap.Posture);
                bw.Write((byte)ap.HitNum);
                bw.Write((byte)ap.HitPitch);
                bw.Write(ap.Balance);  //[int]
                bw.Write(ap.Mana);  //[int]
                bw.Write(ap.Accel);  //[int]
                foreach ( int i in ap.Versatile )
                {
                    bw.Write( i );
                }

                //各フレームリストの書き出し
                SaveBinListFrame( bw, chara, sqc.ListScript, cmpd );
            }
        }
#if false
		//---------------------------------------------------------------------
		//behavior
		public static void SaveBinBehavior ( BinaryWriter bw, Chara chara )
		{
			//Action : Sequence
			Behavior bhv = chara.behavior;
			uint nAct = (uint)bhv.BD_Sequence.Count ();
			bw.Write ( nAct );
			foreach ( Action act in bhv.BD_Sequence.GetEnumerable () )
			{
				bw.Write ( act.Name );		//string (length , [UTF8])
				bw.Write ( (uint)chara.GetIndexOfAction ( act.NextActionName ) ); 
				bw.Write ( (byte)act.Category );
				bw.Write ( (byte)act.Posture );
				bw.Write ( (byte)act.HitNum );
				bw.Write ( (byte)act.HitPitch );
				bw.Write ( act.Balance );	//[int]

				bw.Write ( act.Mana );
				bw.Write ( act.Accel );
				for (int i = 0; i < Action.VRS_SIZE; ++ i )
				{
					bw.Write ( act.Versatile [ i ] );
				}
				

				SaveBinListScript ( bw, chara, act.ListScript, chara.behavior );
			}
		}
		//---------------------------------------------------------------------
		//gernish
		public static void SaveBinGarnish ( BinaryWriter bw, Chara chara )
		{
			//Effect : Sequence
			Garnish gns = chara.garnish;
			uint nGns = (uint)gns.BD_Sequence.Count ();
			bw.Write ( nGns );
			foreach ( Effect efc in gns.BD_Sequence.GetEnumerable () )
			{
				bw.Write ( efc.Name );		//string (length , [UTF8])
				SaveBinListScript ( bw, chara, efc.ListScript, chara.garnish );
			}
		}

#endif

        //---------------------------------------------------------------------
        //Command
        public static void SaveBinCommand ( BinaryWriter bw, Chara chara )
		{
			//個数 [uint]
			uint nCommand = (uint)chara.charaset.BD_Command.Count ();
			bw.Write ( nCommand );

			//実データ [sizeof ( Command ) * n]
			foreach ( Command? cmd in chara.charaset.BD_Command.GetEnumerable () )
			{
				if ( cmd is null ) { continue; }

				bw.Write ( cmd.Name );		//string (length , [UTF8])
				bw.Write ( (byte)cmd.LimitTime );

				//ゲームキー
				byte n = (byte)cmd.ListGameKeyCommand.Count;
				bw.Write ( n );
				foreach ( GameKeyCommand gkc in cmd.ListGameKeyCommand )
				{
					//否定
					bw.Write ( gkc.Not );
					//レバー
					//enum なので 個数は固定 GameKeyData.Lever.LVR_N = 8;
					foreach ( GameKeyData.Lever lvr in gkc.DctLvrSt.Keys )
					{
						bw.Write ( (byte)gkc.DctLvrSt [ lvr ] );
					}
					//ボタン
					//enum なので 個数は固定 GameKeyData.Button.BTN_N = 8;
					foreach ( GameKeyData.Button btn in gkc.DctBtnSt.Keys )
					{
						bw.Write ( (byte)gkc.DctBtnSt [ btn ] ); 
					}
				}
			}
		}

		//---------------------------------------------------------------------
		//Branch
		public static void SaveBinBranch ( BinaryWriter bw, Chara chara )
		{
			//個数 [uint]
			uint nBrc = (uint)chara.charaset.BD_Branch.Count ();
			bw.Write ( nBrc );

			//実データ [sizeof ( Branch ) * n]
			foreach ( Branch? brc in chara.charaset.BD_Branch.GetEnumerable () )
			{
				if ( brc is null ) { continue; }

				// 名前
				bw.Write ( brc.Name );      //string (length , [UTF8])

				//条件 定数
				bw.Write ( (byte)brc.Condition );   //enum -> byte

				//条件　コマンド名（複数）
				uint n = (uint)brc.BD_NameCommand.Count();
                bw.Write ( n );
				foreach (TName? tn in brc.BD_NameCommand.GetEnumerable())
				{
					if ( tn is null ) { continue; }

					bw.Write ( tn.Name );		//string (length , [UTF8])
					bw.Write ( (uint)chara.charaset.GetIndexOfCommand (tn.Name) );	//int -> uint
				}

				//遷移先　シークエンス名
				bw.Write ( brc.NameSequence );		//string (length , [UTF8])
				bw.Write ( (uint)chara.charaset.GetIndexOfAction ( brc.NameSequence ) );    //int -> uint

				//遷移先　フレーム指定
				bw.Write ( (uint)brc.Frame );   //int -> byte

				//条件 同一アクション以外
				bw.Write ( brc.Other );
			}

		}

		//---------------------------------------------------------------------
		//Route
		public static void SaveBinRoute ( BinaryWriter bw, Chara chara )
		{
			//ルート個数 [uint]
			uint nRut = (uint)chara.charaset.BD_Route.Count ();
			bw.Write ( nRut );

			//実データ [sizeof ( Route ) * n]
			foreach ( Route? rut in chara.charaset.BD_Route.GetEnumerable () )
			{
				if ( rut is null ) { continue; }

				//ルート名
				bw.Write ( rut.Name );      //string (length , [UTF8])
				
				//ブランチ個数
				uint nBrnName = (uint)rut.BD_BranchName.Count ();
				bw.Write ( nBrnName );	//[uint]
				
				//IDのみ記録
				foreach ( TName? brcName in rut.BD_BranchName.GetEnumerable () )
				{
					if ( brcName is null ) { continue; }
					bw.Write ( (uint)chara.charaset.GetIndexOfBranch ( brcName.Name ) );	//int -> uint
				}
			}
		}

        //---------------------------------------------------------------------
        //ListFrame
        //behaviorとgarnishでイメージインデックスの検索元が異なるので引数Compendで指定する
        public static void SaveBinListFrame ( BinaryWriter bw, Chara chara, List < Frame > lsFrm, Compend cmp )
		{
			//スクリプト個数
			uint nScp = (uint)lsFrm.Count;
			bw.Write ( nScp ); 


			//各スクリプト
			foreach ( Frame frm in lsFrm)
			{ 
				//グループ
				bw.Write ( frm.Group );

				//イメージインデックス
				uint imgIndex = 0;
				try
				{
					imgIndex = (uint) cmp.BD_Image.IndexOf ( frm.ImgName );
				}
				catch
				{
					imgIndex = UInt32.MaxValue;
				}
				bw.Write ( (uint)imgIndex );

				//イメージ名 [utf-8]
				bw.Write ( frm.ImgName );


				//位置
				bw.Write ( frm.Pos.X );		//int
				bw.Write ( frm.Pos.Y );		//int
					
				//ルート
				bw.Write ( (uint)frm.BD_RutName.Count() );
				foreach ( TName? tn in frm.BD_RutName.GetEnumerable () )
				{
					if ( tn is null ) { continue; }
					bw.Write ( (uint)chara.charaset.GetIndexOfRoute ( tn.Name ) );
				}

				//枠
				SaveBinListRect ( bw, frm.ListCRect );
				SaveBinListRect ( bw, frm.ListHRect );
				SaveBinListRect ( bw, frm.ListARect );
				SaveBinListRect ( bw, frm.ListORect );

				//エフェクト生成
				SaveBinListEfGnrt ( bw, frm.BD_EfGnrt, chara );

				//SE生成
				SaveBinListGnrt ( bw, frm.BD_SEGnrt, chara );
				//VC生成
				SaveBinListGnrt ( bw, frm.BD_VCGnrt, chara );

				//バトルパラメータ
				SaveBinScrPrmBtl ( bw, frm );

				//ステージング(演出)パラメータ
				SaveBinScrPrmStg ( bw, frm );

				//汎用パラメータ
				foreach ( Int32 i in frm.Versatile )
				{
					bw.Write ( i );
				}
			}
		}

		//---------------------------------------------------------------------
		//ListRect
		public static void SaveBinListRect ( BinaryWriter bw, List < Rectangle > listRect )
		{
			//上限は固定 ConstChara.NumRect = 8
			byte nRect = (byte)listRect.Count;
			bw.Write ( nRect ); 
			foreach ( Rectangle rct in listRect )
			{
				bw.Write ( rct.Left );		//int
				bw.Write ( rct.Top );		//int
				bw.Write ( rct.Right );		//int
				bw.Write ( rct.Bottom );    //int
			}
		}

		//---------------------------------------------------------------------
		//エフェクト生成
		public static void SaveBinListEfGnrt ( BinaryWriter bw, BD_EfGnrt bd_EfGnrt, Chara chara )
		{
			bw.Write ( (uint) bd_EfGnrt.Count () ); //個数[byte]
			foreach ( EffectGenerate? efGnrt in bd_EfGnrt.GetEnumerable () )
			{
				if ( efGnrt is null ) { continue; }

				bw.Write ( efGnrt.Name );   //string
				bw.Write ( efGnrt.EfName ); //string
				bw.Write ( (uint) chara.charaset.GetIndexOfEffect ( efGnrt.EfName ) );  //uint

				bw.Write ( efGnrt.Pt.X );       //int
				bw.Write ( efGnrt.Pt.Y );       //int
				bw.Write ( efGnrt.Z_PER100F );  //int
				bw.Write ( efGnrt.Gnrt );       //bool
				bw.Write ( efGnrt.Sync );       //bool

				bw.Write ( (int) efGnrt.GnrtCnd );  //int
				bw.Write ( (int) efGnrt.DrawMode ); //int

				bw.Write ( efGnrt.Loop );           //int
				bw.Write ( efGnrt.DeleteOut );      //bool
				bw.Write ( efGnrt.DeleteCount );    //int

				bw.Write ( efGnrt.Rotate );     //float 4byte, Little Endian
				bw.Write ( efGnrt.Rotate_Center.X );     //int
				bw.Write ( efGnrt.Rotate_Center.Y );     //int

//				bw.Write ( efGnrt.NextEfName ); //string
				bw.Write ( "Next" ); //string
			}
		}

		//---------------------------------------------------------------------
		//BD_Gnrt
		public static void SaveBinListGnrt ( BinaryWriter bw, BD_Gnrt bd_gnrt, Chara chara )
		{
			bw.Write ( (uint) bd_gnrt.Count () );
			foreach ( Generator? gnrt in bd_gnrt.GetEnumerable () )
			{
				if ( gnrt is null ) { continue; }
				//IDを記録
				//bw.Write ( (uint) chara.charaset.GetIndexOfRoute ( tn.Name ) );

				bw.Write ( gnrt.Name );         //名前
				bw.Write ( (int) gnrt.Gnrt_cnd );     //条件
				bw.Write ( (int) gnrt.Group );     //条件
			}

		}

		//---------------------------------------------------------------------
		//BD_T
		public static void SaveBinListTName ( BinaryWriter bw, BD_T bd_t, Chara chara )
		{
			bw.Write ( (uint) bd_t.Count () );
			foreach ( TName? tn in bd_t.GetEnumerable () )
			{
				if ( tn is null ) { bw.Write ( "" ); continue; }
				//IDを記録
				//bw.Write ( (uint) chara.charaset.GetIndexOfRoute ( tn.Name ) );

				//名前を記録
				bw.Write ( tn.Name );
			}

		}

		//---------------------------------------------------------------------
		//FrameParam_Battle
		public static void SaveBinScrPrmBtl ( BinaryWriter bw, Frame frm )
		{
			FrameParam_Battle prm = frm.BtlPrm;
			bw.Write ( (int)prm.CalcState );

			bw.Write ( prm.Vel.X );
			bw.Write ( prm.Vel.Y );
			bw.Write ( prm.Acc.X );
			bw.Write ( prm.Acc.Y );

//			bw.Write ( prm.Power );
			bw.Write ( 123456 );
			bw.Write ( prm.DirectDamage_I );
			bw.Write ( prm.DirectDamage_E );

			bw.Write ( prm.Recoil_I );
			bw.Write ( prm.Recoil_E );
			bw.Write ( prm.Blance_I );
			bw.Write ( prm.Blance_E );
			bw.Write ( prm.Gauge_I );
			bw.Write ( prm.Gauge_E );

			bw.Write ( prm.Warp_I );
			bw.Write ( prm.Warp_E );
			bw.Write ( prm.GuardWarp_I );
			bw.Write ( prm.GuardWarp_E );
		}

        //---------------------------------------------------------------------
        //FrameParam_Staging
        public static void SaveBinScrPrmStg ( BinaryWriter bw, Frame frm )
		{
            FrameParam_Staging prm = frm.StgPrm;
			bw.Write ( (byte)prm.BlackOut );
			bw.Write ( (byte)prm.Vibration );
			bw.Write ( (byte)prm.Stop );

			bw.Write ( (byte)prm.AfterImage_N );
			bw.Write ( (byte)prm.AfterImage_time );
			bw.Write ( (byte)prm.AfterImage_pitch );
			bw.Write ( (byte)prm.Vibration_S );
			bw.Write ( (uint)prm.Color.ToArgb () );
			bw.Write ( (byte)prm.Color_time );

			bw.Write ( prm.Rotate );
			bw.Write ( prm.Rotate_center.X );
			bw.Write ( prm.Rotate_center.Y );
			bw.Write ( prm.Omega );				//float
			bw.Write ( prm.Scaling.X );
			bw.Write ( prm.Scaling.Y );
			bw.Write ( prm.Scaling_Center.X );
			bw.Write ( prm.Scaling_Center.Y );
		}

	}
}

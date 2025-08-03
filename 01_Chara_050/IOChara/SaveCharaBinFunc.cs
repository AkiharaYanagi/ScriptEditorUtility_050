using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;


namespace ScriptEditor
{
	//==================================================================================
	//	SaveCharaBin
	//		キャラデータをbinaryで.datファイルに保存
	//		Document形式を介さない
	//==================================================================================
	public partial class SaveCharaBin
	{
		//---------------------------------------------------------------------
		//behavior
		public void SaveBinBehavior ( BinaryWriter bw, Chara chara )
		{
			//Action : Sequence
			Behavior bhv = chara.charaset.behavior;
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
		public void SaveBinGarnish ( BinaryWriter bw, Chara chara )
		{
			//Effect : Sequence
			Garnish gns = chara.charaset.garnish;
			uint nGns = (uint)gns.BD_Sequence.Count ();
			bw.Write ( nGns );
			foreach ( Effect efc in gns.BD_Sequence.GetEnumerable () )
			{
				bw.Write ( efc.Name );		//string (length , [UTF8])
				SaveBinListScript ( bw, chara, efc.ListScript, chara.garnish );
			}
		}

		//---------------------------------------------------------------------
		//Command
		public void SaveBinCommand ( BinaryWriter bw, Chara chara )
		{
			//個数 [uint]
			uint nCommand = (uint)chara.charaset.BD_Command.Count ();
			bw.Write ( nCommand );

			//実データ [sizeof ( Command ) * n]
			foreach ( Command cmd in chara.charaset.BD_Command.GetEnumerable () )
			{
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
		void SaveBinBranch ( BinaryWriter bw, Chara chara )
		{
			//個数 [uint]
			uint nBrc = (uint)chara.charaset.BD_Branch.Count ();
			bw.Write ( nBrc );

			//実データ [sizeof ( Branch ) * n]
			foreach ( Branch brc in chara.charaset.BD_Branch.GetEnumerable () )
			{
				bw.Write ( brc.Name );		//string (length , [UTF8])
				bw.Write ( (byte)brc.Condition );	//enum -> byte
				bw.Write ( brc.NameCommand );		//string (length , [UTF8])
				bw.Write ( (uint)chara.charaset.GetIndexOfCommand ( brc.NameCommand ) );	//int -> uint
				bw.Write ( brc.NameSequence );		//string (length , [UTF8])
				bw.Write ( (uint)chara.charaset.GetIndexOfAction ( brc.NameSequence ) );	//int -> uint
				bw.Write ( (uint)brc.Frame );	//int -> byte
				bw.Write ( brc.Other );
			}

		}

		//---------------------------------------------------------------------
		//Route
		void SaveBinRoute ( BinaryWriter bw, Chara chara )
		{
			//ルート個数 [uint]
			uint nRut = (uint)chara.charaset.BD_Route.Count ();
			bw.Write ( nRut );

			//実データ [sizeof ( Route ) * n]
			foreach ( Route rut in chara.charaset.BD_Route.GetEnumerable () )
			{
				//ルート名
				bw.Write ( rut.Name );      //string (length , [UTF8])
				
				//ブランチ個数
				uint nBrnName = (uint)rut.BD_BranchName.Count ();
				bw.Write ( nBrnName );	//[uint]
				
				//IDのみ記録
				foreach ( TName brcName in rut.BD_BranchName.GetEnumerable () )
				{
					bw.Write ( (uint)chara.charaset.GetIndexOfBranch ( brcName.Name ) );	//int -> uint
				}
			}
		}

		//---------------------------------------------------------------------
		//ListFrame
		//behaviorとgarnishでイメージインデックスの検索元が異なるので引数Compendで指定する
		void SaveBinListFrame( BinaryWriter bw, Chara chara, List <Frame> lsFrm, Compend cmp )
		{
			//スクリプト個数
			uint nScp = (uint)lsFrm.Count;
			bw.Write ( nScp ); 


//			Debug.Write ( "\n");


			//各スクリプト
			foreach (Frame frm in lsFrm)
			{ 
				//グループ
				bw.Write (frm.Group );

				//出力
//				Debug.Write ( "" + frm.Group + ",");


				//イメージインデックス
				uint imgIndex = (uint)cmp.BD_Image.IndexOf (frm.ImgName );
				bw.Write ( (uint)imgIndex );

				//イメージ名 [utf-8]
				bw.Write (frm.ImgName );


				//位置
				bw.Write (frm.Pos.X );		//int
				bw.Write (frm.Pos.Y );		//int
					
				//ルート
				bw.Write ( (uint)frm.BD_RutName.Count() );
				foreach ( TName tn in frm.BD_RutName.GetEnumerable () )
				{
					bw.Write ( (uint)chara.charaset.GetIndexOfRoute ( tn.Name ) );
				}

				//枠
				SaveBinListRect ( bw, frm.ListCRect );
				SaveBinListRect ( bw, frm.ListHRect );
				SaveBinListRect ( bw, frm.ListARect );
				SaveBinListRect ( bw, frm.ListORect );

				//エフェクト生成
				bw.Write ( (uint)frm.BD_EfGnrt.Count () );	//個数[byte]
				foreach ( EffectGenerate efGnrt in frm.BD_EfGnrt.GetEnumerable () )
				{ 
					bw.Write ( (uint)chara.charaset.GetIndexOfEffect ( efGnrt.EfName ) );	//uint
					bw.Write ( efGnrt.Pt.X );	//int
					bw.Write ( efGnrt.Pt.Y );	//int
					bw.Write ( efGnrt.Z_PER100F );		//int
					bw.Write ( efGnrt.Gnrt );	//bool
					bw.Write ( efGnrt.Loop );	//bool
					bw.Write ( efGnrt.Sync );	//bool
				}

				//バトルパラメータ
				SaveBinScrPrmBtl ( bw, frm);

				//ステージング(演出)パラメータ
				SaveBinScrPrmStg ( bw, frm);

				//汎用パラメータ
				foreach ( Int32 i in frm.Versatile )
				{
					bw.Write ( i );
				}
			}
		}

		//---------------------------------------------------------------------
		//ListRect
		void SaveBinListRect ( BinaryWriter bw, List < Rectangle > listRect )
		{
			//上限は固定 ConstChara.NumRect = 8
			//それ以下は０からの可変
			byte nRect = (byte)listRect.Count;
			bw.Write ( nRect ); 
			foreach ( Rectangle rct in listRect )
			{
				bw.Write ( rct.Left );
				bw.Write ( rct.Top );
				bw.Write ( rct.Right );
				bw.Write ( rct.Bottom );
			}
		}

		//---------------------------------------------------------------------
		//ScriptParam_Battle
		void SaveBinScrPrmBtl ( BinaryWriter bw, Frame frm )
		{
			ScriptParam_Battle prm = frm.BtlPrm;
//			int i = (int)prm.CalcState ;
			bw.Write ( (int)prm.CalcState );
			bw.Write ( prm.Vel.X );
			bw.Write ( prm.Vel.Y );
			bw.Write ( prm.Acc.X );
			bw.Write ( prm.Acc.Y );
			bw.Write ( prm.Power );
			bw.Write ( prm.Warp );
			bw.Write ( prm.Recoil_I );
			bw.Write ( prm.Recoil_E );
			bw.Write ( prm.Blance_I );
			bw.Write ( prm.Blance_E );
			bw.Write ( prm.DirectDamage );
		}

		//---------------------------------------------------------------------
		//ScriptParam_Staging
		void SaveBinScrPrmStg ( BinaryWriter bw, Frame frm )
		{
			ScriptParam_Staging prm = frm.StgPrm;
			bw.Write ( (byte)prm.BlackOut );
			bw.Write ( (byte)prm.Vibration );
			bw.Write ( (byte)prm.Stop );
			bw.Write ( prm.Rotate );
			bw.Write ( prm.Rotate_center.X );
			bw.Write ( prm.Rotate_center.Y );
			bw.Write ( (byte)prm.AfterImage_N );
			bw.Write ( (byte)prm.AfterImage_time );
			bw.Write ( (byte)prm.AfterImage_pitch );
			bw.Write ( (byte)prm.Vibration_S );
			bw.Write ( (uint)prm.Color.ToArgb () );
			bw.Write ( (byte)prm.Color_time );
			bw.Write ( prm.Scaling.X );
			bw.Write ( prm.Scaling.Y );
			bw.Write ( (uint)prm.SE );
			bw.Write ( prm.SE_name );
			bw.Write ( prm.VC_name );
		}
		
	}
}

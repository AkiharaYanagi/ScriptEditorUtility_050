using System;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

namespace ScriptEditor
{
	using BL_SQC = BindingList < Sequence >;
	using BD_CMD = BindingDictionary < Command >;
	using BD_BRC = BindingDictionary < Branch >;
	using BD_RUT = BindingDictionary < Route >;
	using GK_L = GameKeyData.Lever;
	using GK_B = GameKeyData.Button;

	//============================================================
	//キャラからをスクリプトの対象となる値を読み込み、
	//キャラスクリプトとしてテキストメモリストリームに変換
	//============================================================

	public class CharaToDoc
	{
		//内部参照用
		private Chara Chara = null;

		//外部呼出し
		public MemoryStream Run ( Chara ch )
		{
			MemoryStream ms = null;

			try
			{
				ms = _Run ( ch );
			}
			catch ( Exception e ) when ( e.Message.Equals ( "name error" ) )
			{
				System.Windows.Forms.MessageBox.Show ( e.ToString (), "name error" );
			}
			catch ( Exception e )
			{
				System.Windows.Forms.MessageBox.Show ( e.ToString () );
			}
			
			return ms;
		}
		
		//内部実行用
		private MemoryStream _Run ( Chara ch )
		{
			Chara = ch;

			//対象メモリストリーム
			MemoryStream mstrm = new MemoryStream ();

			//@info using節だと後にストリームが参照できない
			StreamWriter strmWriter = new StreamWriter ( mstrm, Encoding.UTF8 );
//			using ( StreamWriter strmWriter = new StreamWriter ( mstrm, Encoding.UTF8 ) )
			{
			//全体タグとバージョン
			strmWriter.Write ( "<Chara>\n" );
			strmWriter.Write ( "<ver>" + IO_CONST.VER + "</ver>\n\n" );

			//メインイメージリストヘッダ
			WriteListImageHeader ( strmWriter, ch.behavior, "Image" );
			//EFイメージリストヘッダ
			WriteListImageHeader ( strmWriter, ch.garnish, "EfImage" );

			//アクションリスト
			WriteSequence ( strmWriter, ch.behavior, "Action", Func_WriteAction );
			//エフェクトリスト
			WriteSequence ( strmWriter, ch.garnish, "Effect", Func_WriteEffect );

			//コマンドリスト
			WriteCommandList ( strmWriter, ch.BD_Command );

			//ブランチリスト
			WriteBranchList ( strmWriter, ch.BD_Branch );

			//ルートリスト
			WriteRouteList ( strmWriter, ch.BD_Route );

			//基本状態アクションID -> 名前で指定

			//全体タグ
			strmWriter.Write ( "</Chara>\n" );

			strmWriter.Flush ();
			} //using strmWriter

			return mstrm;
		}


		//=================================================================================
		//個別項目

		//イメージリストヘッダの書出(メイン, Ef)
		private void WriteListImageHeader ( StreamWriter sw, Compend cmpd, string tagname )
		{
			BindingList < ImageData > bl_imgd = cmpd.BD_Image.GetBindingList (); 
			string tn0 = tagname;
			string tn_l = tagname + "List";
			sw.Write ( "<" + tn_l + " Num=\"" + bl_imgd.Count + "\">\n" );
			foreach ( ImageData imageData in bl_imgd )
			{
				sw.Write ( "\t<" + tn0 + " Name=\"" + imageData.Name + "\"></" + tn0 + ">\n" );
			}
			sw.Write ( "</" + tn_l + ">\n" );
		}

		//シークエンス(アクション, エフェクト)リストの書出
		private delegate void Func_WriteSqc ( StreamWriter sw, Sequence sqc );
		private void WriteSequence ( StreamWriter sw, Compend cmpd, string tagname, Func_WriteSqc fws )
		{
			BL_SQC bl_sqc = cmpd.BD_Sequence.GetBindingList ();
			string tn = tagname;
			string tn_l = tagname + "List";

			//シークエンスリスト
			sw.Write ( "<" + tn_l + " Num=\"" + bl_sqc.Count + "\">\n" );
			foreach ( Sequence sqc in bl_sqc )
			{
				sw.Write ( "\t<" + tn );
				fws ( sw, sqc );	//(アクション, エフェクト)個別アトリビュート
				WriteListScript ( sw, cmpd, sqc.ListScript );	//スクリプト
				sw.Write ( "\t</" + tn + ">\n" );
			}
			sw.Write ( "</" + tn_l + ">\n" );
		}

		//アクションの書出
		private void Func_WriteAction ( StreamWriter sw, Sequence sqc )
		{
			Action act = (Action)sqc;
			int nextID = Chara.behavior.BD_Sequence.IndexOf ( act.NextActionName );
			if ( -1 == nextID )
			{
				string error_name = "name error : "+ act.Name + "." + act.NextActionName ;
				//throw new Exception ( error_name );
				STS_TXT.Trace_Err ( error_name );
			}

			sw.Write ( " Name=\"" + act.Name + "\"" );					//名前
			sw.Write ( " NextName=\"" + act.NextActionName + "\"" );	//次アクション名
			sw.Write ( " NextID=\"" + nextID.ToString() + "\"" );		//次アクションID
			sw.Write ( " Category=\"" + (int)act.Category + "\"" );		//アクション属性
			sw.Write ( " Posture=\"" + (int)act.Posture + "\"" );		//アクション体勢
			sw.Write ( " HitNum=\"" + act.HitNum + "\"" );				//ヒット数
			sw.Write ( " HitPitch=\"" + act.HitPitch + "\"" );			//ヒット間隔[F]
			sw.Write ( " Balance=\"" + act.Balance + "\"" );			//ヒット間隔[F]
			sw.Write ( ">\n" );
		}

		//エフェクトの書出
		private void Func_WriteEffect ( StreamWriter sw, Sequence sqc )
		{
			Effect ef = (Effect)sqc;

			sw.Write ( " Name=\"" + ef.Name + "\"" );					//名前
			sw.Write ( ">\n" );
		}


		//外部：スクリプトリストの書出
		public void WriteListScript ( Chara ch, StreamWriter strmWriter, Compend cmpd, List<Script> listScript )
		{
			Chara = ch;
			WriteListScript ( strmWriter, cmpd, listScript );
		}

		//内部：スクリプトリストの書出
		private void WriteListScript ( StreamWriter sw, Compend cmpd, List<Script> listScript )
		{
			//スクリプト
			foreach ( Script script in listScript )
			{
				int imageID = cmpd.BD_Image.IndexOf ( script.ImgName );
				//if ( -1 == imageID )
					//{ throw new Exception ( "name error" ); }
	
				sw.Write ( "\t\t<Script" );
				sw.Write ( " Group=\"" + script.Group + "\"" );
				sw.Write ( " ImageName=\"" + script.ImgName + "\"" );
				sw.Write ( " ImageID=\"" + imageID + "\"" );
				sw.Write ( " X=\"" + script.Pos.X + "\" Y=\"" + script.Pos.Y + "\"" );
				WriteBattleParam ( sw, script.BtlPrm );
				WriteStageParam ( sw, script.StgPrm );

				sw.Write ( ">\n" );

				//ルートリスト
				sw.Write ( "\t\t\t<RouteList>\n" );
				//ルート
				foreach ( TName rutName in script.BD_RutName.GetEnumerable() )
				{
					int indexRoute = Chara.BD_Route.IndexOf ( rutName.Name );
					if ( -1 == indexRoute ) 
						{ throw new Exception ( "name error : " + rutName.Name ); }

					sw.Write ( "\t\t\t\t<Route" );
					sw.Write ( " Name=\"" + rutName.Name + "\" ID=\"" + indexRoute.ToString() + "\"" );
					sw.Write ( "></Route>\n" );
				}
				sw.Write ( "\t\t\t</RouteList>\n" );


				//Efジェネレートリスト
				sw.Write ( "\t\t\t<EfGenerateList Num=\"" + script.BD_EfGnrt.Count() + "\">\n" );
				//Efジェネレート
				foreach ( EffectGenerate efGnrt in script.BD_EfGnrt.GetBindingList () )
				{
					sw.Write ( "\t\t\t\t<EfGenerate" );
					sw.Write ( " Name=\"" + efGnrt.Name + "\"" );
					sw.Write ( " EfName=\"" + efGnrt.EfName + "\"" );
					sw.Write ( " EfGnrt_PtX=\"" + efGnrt.Pt.X + "\"" );
					sw.Write ( " EfGnrt_PtY=\"" + efGnrt.Pt.Y + "\"" );
					sw.Write ( " EfGnrt_PtZ=\"" + efGnrt.Z_PER100F + "\"" );
					sw.Write ( " Gnrt=\"" + efGnrt.Gnrt + "\"" );
					sw.Write ( " Loop=\"" + efGnrt.Loop + "\"" );
					sw.Write ( " Sync=\"" + efGnrt.Sync + "\"" );
					sw.Write ( "></EfGenerate>\n" );
				}
				sw.Write ( "\t\t\t</EfGenerateList>\n" );

				WriteListRect ( sw, script.ListCRect, "CollisionRect" );	//ぶつかり枠リスト
				WriteListRect ( sw, script.ListARect, "AttackRect" );		//攻撃枠リスト
				WriteListRect ( sw, script.ListHRect, "HitRect" );			//当り枠リスト
				WriteListRect ( sw, script.ListORect, "OffsetRect" );		//相殺枠リスト

				sw.Write ( "\t\t</Script>\n" );
			}
		}

		//バトルパラメータの書出
		private void WriteBattleParam ( StreamWriter sw, ScriptParam_Battle btlPrm )
		{
			sw.Write ( " CLC_ST=\"" + (int)btlPrm.CalcState + "\"" );
			sw.Write ( " VX=\"" + btlPrm.Vel.X + "\" VY=\"" + btlPrm.Vel.Y + "\"" );
			sw.Write ( " AX=\"" + btlPrm.Acc.X + "\" AY=\"" + btlPrm.Acc.Y + "\"" );
			sw.Write ( " Power=\"" + btlPrm.Power + "\"" );
			sw.Write ( " Warp=\"" + btlPrm.Warp + "\"" );
			sw.Write ( " Recoil_I=\"" + btlPrm.Recoil_I + "\"" );
			sw.Write ( " Recoil_E=\"" + btlPrm.Recoil_E + "\"" );
			sw.Write ( " Blance_I=\"" + btlPrm.Blance_I + "\"" );
			sw.Write ( " Blance_E=\"" + btlPrm.Blance_E + "\"" );
		}

		//演出パラメータの書出
		private void WriteStageParam ( StreamWriter sw, ScriptParam_Staging stgPrm )
		{
			sw.Write ( " BlackOut=\"" + stgPrm.BlackOut + "\"" );
			sw.Write ( " Vibration=\"" + stgPrm.Vibration + "\"" );
			sw.Write ( " Stop=\"" + stgPrm.Stop + "\"" );

			sw.Write ( " Rotate=\"" + stgPrm.Rotate + "\"" );
			sw.Write ( " RotateX=\"" + stgPrm.Rotate_center.X + "\" RotateY=\"" + stgPrm.Rotate_center.Y + "\"" );
			sw.Write ( " AfterImage_pitch=\"" + stgPrm.AfterImage_pitch + "\"" );
			sw.Write ( " AfterImage_N=\"" + stgPrm.AfterImage_N + "\"" );
			sw.Write ( " AfterImage_time=\"" + stgPrm.AfterImage_time + "\"" );
			sw.Write ( " Vibration_S=\"" + stgPrm.Vibration_S + "\"" );
			sw.Write ( " Color=\"" + stgPrm.Color.ToString() + "\"" );
			sw.Write ( " Color_time=\"" + stgPrm.Color_time + "\"" );

			sw.Write ( " ScalingX=\"" + stgPrm.Scaling.X + "\" ScalingY=\"" + stgPrm.Scaling.Y + "\"" );
			sw.Write ( " SE=\"" + stgPrm.SE + "\"" );
		}

		//枠の書出	
		private void WriteListRect ( StreamWriter strmWriter, List<Rectangle> lsRect, string name )
		{
			//枠リスト
			strmWriter.Write ( "\t\t\t<" + name + "List Num=\"" + lsRect.Count + "\">\n" );
			//枠
			foreach ( Rectangle rect in lsRect )
			{
				strmWriter.Write ( "\t\t\t\t<" + name );
				strmWriter.Write ( " X=\"" + rect.X + "\"" );
				strmWriter.Write ( " Y=\"" + rect.Y + "\"" );
				strmWriter.Write ( " W=\"" + rect.Width + "\"" );
				strmWriter.Write ( " H=\"" + rect.Height + "\"" );
				strmWriter.Write ( "></" + name + ">\n" );
			}
			strmWriter.Write ( "\t\t\t</" + name + "List>\n" );
		}

		//コマンドリスト
		private void WriteCommandList ( StreamWriter sw, BD_CMD bd_cmd )
		{
			BindingList < Command > ls = bd_cmd.GetBindingList ();
			sw.Write ( "<CommandList Num=\"" + ls.Count + "\">\n" );
			//コマンド
			foreach ( Command command in ls )
			{
				sw.Write ( "\t<Command Name=\"" + command.Name + "\"" );
				sw.Write ( " Limit=\"" + command.LimitTime.ToString () + "\">\n" );

				//キー
				foreach ( GameKeyCommand gameKey in command.ListGameKeyCommand )
				{
					sw.Write ( "\t\t<Key" );
					
					//否定
					sw.Write ( " Not=\"" + gameKey.Not.ToString () + "\"" );

					//レバー(全方向)
					foreach ( GK_L key in gameKey.DctLvrSt.Keys )
					{
						sw.Write ( " Key_" + key.ToString() + "=\"" );
						sw.Write ( gameKey.DctLvrSt [ key ].ToString () + "\"" );
					}

					//ボタン
					foreach ( GK_B key in gameKey.DctBtnSt.Keys )
					{
						sw.Write ( " Btn_" + key.ToString () + " =\"" + gameKey.DctBtnSt [ key ].ToString () + "\"" );
					}
					sw.Write ( ">" );

					sw.Write ( "</Key>\n" );
				}
				sw.Write ( "\t</Command>\n" );
			}
			sw.Write ( "</CommandList>\n" );
		}
		
		//ブランチリスト
		private void WriteBranchList ( StreamWriter sw, BD_BRC bd_cmd )
		{
			BindingList < Branch > ls = bd_cmd.GetBindingList ();
			sw.Write ( "<BranchList Num=\"" + ls.Count + "\">\n" );
			//ブランチ
			foreach ( Branch brc in ls )
			{
				//@info IDを先に検索して記述しておくことでエラーチェック
				//		ゲームメイン側ではIDのみ用いる
				int commandID = Chara.BD_Command.IndexOf ( brc.NameCommand );

				int sequenceID = Chara.behavior.BD_Sequence.IndexOf ( brc.NameSequence );
				if ( -1 == sequenceID )
				{
					sequenceID = Chara.garnish.BD_Sequence.IndexOf ( brc.NameSequence ); 
				}

				sw.Write ( "\t<Branch" );
				sw.Write ( " Name=\"" + brc.Name + "\"" );					//名前
				sw.Write ( " Condition=\"" + (int)brc.Condition + "\"" );	//条件
				sw.Write ( " NameCommand=\"" + brc.NameCommand + "\"" );	//コマンド名
				sw.Write ( " CommandID=\"" + commandID + "\"" );			//コマンドID
				sw.Write ( " NameSequence=\"" + brc.NameSequence + "\"" );	//シークエンス名
				sw.Write ( " SequenceID=\"" + sequenceID + "\"" );			//シークエンスID
				sw.Write ( " Frame=\"" + brc.Frame + "\"" );				//遷移先フレーム
				sw.Write ( " Other=\"" + brc.Other + "\"" );				//同一アクション以外
				sw.Write ( ">\n" );
				sw.Write ( "\t</Branch>\n" );
			}
			sw.Write ( "</BranchList>\n" );
		}
		
		//ルートリスト
		private void WriteRouteList ( StreamWriter sw, BD_RUT bd_rut )
		{
			BindingList < Route > ls = bd_rut.GetBindingList ();
			sw.Write ( "<RouteList Num=\"" + ls.Count + "\">\n" );
			//ブランチ
			foreach ( Route rut in ls )
			{
				sw.Write ( "\t<Route Name=\"" + rut.Name + "\"" );
				sw.Write ( " Summary=\"" + rut.Summary + "\">\n" );
				//ブランチネームリスト
				BindingList < TName > bl_brrt = rut.BD_BranchName.GetBindingList ();
				sw.Write ( "\t\t<BranchNameList Num=\"" + bl_brrt.Count + "\">\n" );
				foreach ( TName tn in bl_brrt )
				{
					//@info IDを先に検索して記述しておくことでエラーチェック
					//		ゲームメイン側ではIDのみ用いる
					int BranchID = Chara.BD_Branch.IndexOf ( tn.Name );

					sw.Write ( "\t\t\t<BranchName Name=\"" + tn.Name + "\"" );
					sw.Write ( " ID=\"" + BranchID.ToString() + "\"" );
					sw.Write ( "></BranchName>\n" );
				}
				sw.Write ( "\t\t</BranchNameList>\n" );
				sw.Write ( "\t</Route>\n" );
			}
			sw.Write ( "</RouteList>\n" );
		}

	}
}

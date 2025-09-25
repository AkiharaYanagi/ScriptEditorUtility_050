using System;
using System.Diagnostics;


//2種類のCharaデータを変換する
using Chara020;
using Chara050;


using SE2 = Chara020;
using SE5 = Chara050;



namespace test00_Chara
{
	internal partial class ConvertChara
	{
		public void ConvertScript ( SE5.Sequence sqc050, SE2.Sequence sqc020 )
		{
			//スクリプトからフレームに変換
			foreach ( Script scp in sqc020.ListScript )
			{
				Frame frm = new Frame ();

				//基本パラメータ
				frm.N = scp.Frame;
				frm.Group = scp.Group;
				frm.ImgName = scp.ImgName;
				frm.Pos = scp.Pos;
				
				//ルート名(TNameは 020, 050 共通)
				frm.BD_RutName.DeepCopy ( scp.BD_RutName );

				//判定枠
				//ディープコピー(Rectangleは値型)
				frm.ListCRect = scp.ListCRect.Select ( r => r ).ToList ();
				frm.ListHRect = scp.ListHRect.Select ( r => r ).ToList ();
				frm.ListARect = scp.ListARect.Select ( r => r ).ToList ();
				frm.ListORect = scp.ListORect.Select ( r => r ).ToList ();

				//エフェクト生成
				foreach ( SE2.EffectGenerate? efgnrt2 in scp.BD_EfGnrt.GetEnumerable () )
				{
					if ( efgnrt2 is null ) { continue; }

					SE5.EffectGenerate efgnrt5 = new SE5.EffectGenerate ()
					{
						Name = efgnrt2.Name,
						EfName = efgnrt2.EfName,
						Pt = efgnrt2.Pt,
						Z_PER100F = efgnrt2.Z_PER100F,
						Gnrt = efgnrt2.Gnrt,
						Sync = efgnrt2.Sync,
						GnrtCnd = Generate_Condition.COMPULSION,
						DrawMode = Draw_Mode.NORMAL,
						Loop = 1,
						DeleteOut = true,
						DeleteCount = 0,
						Rotate = 0,
						Rotate_Center = new Point ( 0, 0 ),
						NextEfName = "",
					};

					frm.BD_EfGnrt.Add ( efgnrt5 );
				}

				//バトルパラメータ
				FrameParam_Battle BtlPrm = frm.BtlPrm;
				BtlPrm.CalcState = (SE5.CLC_ST) scp.BtlPrm.CalcState;
				BtlPrm.Vel = new Point ( scp.BtlPrm.Vel.X, scp.BtlPrm.Vel.Y );
				BtlPrm.Acc = new Point ( scp.BtlPrm.Acc.X, scp.BtlPrm.Acc.Y );

				BtlPrm.Power = scp.BtlPrm.Power;
				BtlPrm.DirectDamage_I = 0;
				BtlPrm.DirectDamage_E = scp.BtlPrm.DirectDamage;

				BtlPrm.Recoil_I = scp.BtlPrm.Recoil_I;
				BtlPrm.Recoil_E = scp.BtlPrm.Recoil_E;
				BtlPrm.Blance_I = scp.BtlPrm.Blance_I;
				BtlPrm.Blance_E = scp.BtlPrm.Blance_E;
				BtlPrm.Gauge_I = 0;
				BtlPrm.Gauge_E = 0;

				BtlPrm.Warp_I = 0;
				BtlPrm.Warp_E = scp.BtlPrm.Warp;
				BtlPrm.GuardWarp_I = 0;
				BtlPrm.GuardWarp_E = 0;
				
				

				//演出パラメータ
				FrameParam_Staging StgPrm = frm.StgPrm;
				StgPrm.BlackOut = scp.StgPrm.BlackOut;
				StgPrm.Vibration = scp.StgPrm.Vibration;
				StgPrm.Stop = scp.StgPrm.Stop;

				StgPrm.AfterImage_N = scp.StgPrm.AfterImage_N;
				StgPrm.AfterImage_time = scp.StgPrm.AfterImage_time;
				StgPrm.AfterImage_pitch = scp.StgPrm.AfterImage_pitch;
				StgPrm.Vibration_S = scp.StgPrm.Vibration_S;

				StgPrm.Rotate = scp.StgPrm.Rotate;
				StgPrm.Rotate_center = new Point ( scp.StgPrm.Rotate_center.X, scp.StgPrm.Rotate_center.Y );
				StgPrm.Scaling = new Point ( scp.StgPrm.Scaling.X, scp.StgPrm.Scaling.Y );
				StgPrm.Scaling_Center = new Point ( 0, 0 );


				sqc050.ListScript.Add ( frm );
			}
		}
	}
}

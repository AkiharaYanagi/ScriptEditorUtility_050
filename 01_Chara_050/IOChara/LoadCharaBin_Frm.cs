using ScriptEditorUtility;

using System.Diagnostics;
using System.Drawing;


namespace Chara050
{
	using BD_Gnrt = BindingDictionary<Generator>;
	using BD_T = BindingDictionary<TName>;


	//==================================================================================
	//	LoadCharaBin_Frm
	//		フレームデータ
	//==================================================================================
	public partial class LoadCharaBin
	{
        //スクリプトリスト
        private void LoadBinListScript(BinaryReader br, List<Frame> lscp)
        {
            //スクリプト個数
            uint N_Scp = br.ReadUInt32();
            for (uint i = 0; i < N_Scp; ++i)
            {
                //スクリプト
                Frame frm = new Frame();

                //フレーム数は数え上げながら設定する
                frm.N = (int)i;

                //----------------------------------------
                //グループ
                frm.Group = br.ReadInt32();

                //----------------------------------------
                //イメージ名
#if false
				scp.ImgName = "Img_" + br.ReadUInt32 ().ToString ();	//後にイメージ名に変換
#else
                //イメージインデックス
                uint imgIndex = br.ReadUInt32();
                //イメージ名
                frm.ImgName = br.ReadString();  //[utf-8]
#endif
				//Debug.WriteLine ( frm.ImgName );


                //表示位置
                frm.Pos = new Point(br.ReadInt32(), br.ReadInt32());

                //ルート名リスト
                uint nRut = br.ReadUInt32();
                for (uint iRut = 0; iRut < nRut; ++iRut)
                {
                    //後にルート名に変換
                    frm.BD_RutName.Add(new TName("Rut_" + br.ReadUInt32().ToString()));
                }

                //枠リスト
                LoadBinListRect(br, frm.ListCRect);
                LoadBinListRect(br, frm.ListHRect);
                LoadBinListRect(br, frm.ListARect);
                LoadBinListRect(br, frm.ListORect);

                //エフェクト生成
                LoadBinListEfGnrt ( br, frm );

                //SE生成
                LoadBinListGnrt ( br, frm.BD_SEGnrt );

				//VC生成
				LoadBinListGnrt ( br, frm.BD_VCGnrt );

#if false
                uint nEfGnrt = br.ReadUInt32(); //個数[uint]
                for (uint iEG = 0; iEG < nEfGnrt; ++iEG)
                {
                    EffectGenerate efgnrt = new EffectGenerate()
                    {
                        //生成名
						Name = br.ReadString (),
						//対象エフェクト名は後で指定し直す
						EfName = "Ef_" + br.ReadUInt32().ToString(),
                        Pt = new Point(br.ReadInt32(), br.ReadInt32()),
                        Z_PER100F = br.ReadInt32(),
                        Gnrt = br.ReadBoolean(),
                        Sync = br.ReadBoolean(),
						GnrtCnd = (Generate_Condition) br.ReadInt32 (),
						DrawMode= (Draw_Mode) br.ReadInt32 (),
						Loop = br.ReadInt32 (),
						DeleteOut = br.ReadBoolean (),
						DeleteCount = br.ReadInt32 (),
                        Rotate = br.ReadSingle (),    //float 4byte, Little Endian
                        Rotate_Center = new Point ( br.ReadInt32 (), br.ReadInt32 () ),
                        NextEfName = br.ReadString (),
					};
                    scp.BD_EfGnrt.Add(efgnrt);
                }
#endif

				//バトルパラメータ
				LoadBinPrmBattle ( br, frm );
#if false
                FrameParam_Battle btlPrm = new FrameParam_Battle()
                {
                    CalcState = (CLC_ST)br.ReadInt32(),
                    Vel = new Point(br.ReadInt32(), br.ReadInt32()),
                    Acc = new Point(br.ReadInt32(), br.ReadInt32()),
                    Power = br.ReadInt32(),
                    Warp = br.ReadInt32(),
                    Recoil_I = br.ReadInt32(),
                    Recoil_E = br.ReadInt32(),
                    Blance_I = br.ReadInt32(),
                    Blance_E = br.ReadInt32(),
                    DirectDamage = br.ReadInt32(),
                };
                frm.BtlPrm = btlPrm;
#endif

				//ステージングパラメータ
				LoadBinPrmStaging ( br, frm );
#if false
                FrameParam_Staging stgPrm = new FrameParam_Staging()
                {
                    BlackOut = br.ReadByte(),
                    Vibration = br.ReadByte(),
                    Stop = br.ReadByte(),
                    Rotate = br.ReadInt32(),
                    Rotate_center = new Point(br.ReadInt32(), br.ReadInt32()),
                    AfterImage_N = br.ReadByte(),
                    AfterImage_time = br.ReadByte(),
                    AfterImage_pitch = br.ReadByte(),
                    Vibration_S = br.ReadByte(),
                    Color = Color.FromArgb((int)br.ReadUInt32()),
                    Color_time = br.ReadByte(),
                    Scaling = new Point(br.ReadInt32(), br.ReadInt32()),
                };
                frm.StgPrm = stgPrm;
#endif


				//汎用パラメータ
				for ( uint indexVst = 0; indexVst < frm.Versatile.Length; ++indexVst)
                {
                    frm.Versatile[indexVst] = br.ReadInt32();
                }


                lscp.Add(frm);
            }
        }

        //枠リスト
        private void LoadBinListRect(BinaryReader br, List<Rectangle> lrct)
        {
            int N_Rct = br.ReadByte();
            for (int i = 0; i < N_Rct; ++i)
            {
                int left = br.ReadInt32();
                int top = br.ReadInt32();
                int right = br.ReadInt32();
                int bottom = br.ReadInt32();
                Rectangle r = new Rectangle(left, top, right - left, bottom - top);

                lrct.Add(r);
            }
        }

		//エフェクト生成
		private void LoadBinListEfGnrt ( BinaryReader br, Frame frm )
		{
			//エフェクト生成
			uint nEfGnrt = br.ReadUInt32 (); //個数[uint]
			for ( uint iEG = 0; iEG < nEfGnrt; ++iEG )
			{
				EffectGenerate efgnrt = new EffectGenerate ();
				{
					//生成名
					efgnrt.Name = br.ReadString ();
					//対象エフェクト名は後で指定し直す
					//EfName = "Ef_" + br.ReadUInt32 ().ToString ();
					efgnrt.EfName = br.ReadString ();
					uint id = br.ReadUInt32 ();
					efgnrt.Pt = new Point ( br.ReadInt32 (), br.ReadInt32 () );
					efgnrt.Z_PER100F = br.ReadInt32 ();
					efgnrt.Gnrt = br.ReadBoolean ();
					efgnrt.Sync = br.ReadBoolean ();
					efgnrt.GnrtCnd = (Generate_Condition) br.ReadInt32 ();
					efgnrt.DrawMode = (Draw_Mode) br.ReadInt32 ();
					efgnrt.Loop = br.ReadInt32 ();
					efgnrt.DeleteOut = br.ReadBoolean ();
					efgnrt.DeleteCount = br.ReadInt32 ();
					efgnrt.Rotate = br.ReadSingle ();    //float 4byte; Little Endian
					efgnrt.Rotate_Center = new Point ( br.ReadInt32 (), br.ReadInt32 () );
					efgnrt.NextEfName = br.ReadString ();
				};
				frm.BD_EfGnrt.Add ( efgnrt );
			}
		}

		//生成IDリスト
		private void LoadBinListGnrt ( BinaryReader br, BD_Gnrt bd_gnrt )
		{
			//エフェクト生成
			uint nGnrt = br.ReadUInt32 (); //個数[uint]
			for ( uint i = 0; i < nGnrt; ++i )
			{
				Generator gnrt = new Generator ()
				{
					Name = br.ReadString (),
					Gnrt_cnd = (Generate_Condition) br.ReadInt32 (),
					Group = br.ReadInt32 (),
				};
				bd_gnrt.Add ( gnrt );
			}
		}

		//生成名前リスト
		private void LoadBinListTName ( BinaryReader br, BD_T bd_t )
		{
			uint nGnrt = br.ReadUInt32 (); //個数[uint]
			for ( uint i = 0; i < nGnrt; ++i )
			{
				bd_t.Add ( new TName ( br.ReadString () ) );
			}
		}

		//戦闘パラメータ
		private void LoadBinPrmBattle ( BinaryReader br, Frame frm )
		{
			FrameParam_Battle btlPrm = new FrameParam_Battle ()
			{
				CalcState = (CLC_ST) br.ReadInt32 (),
				Vel = new Point ( br.ReadInt32 (), br.ReadInt32 () ),
				Acc = new Point ( br.ReadInt32 (), br.ReadInt32 () ),

				Power = br.ReadInt32 (),
				DirectDamage_I = br.ReadInt32 (),
				DirectDamage_E = br.ReadInt32 (),

				Recoil_I = br.ReadInt32 (),
				Recoil_E = br.ReadInt32 (),
				Blance_I = br.ReadInt32 (),
				Blance_E = br.ReadInt32 (),
				Gauge_I = br.ReadInt32 (),
				Gauge_E = br.ReadInt32 (),

				Warp_I = br.ReadInt32 (),
				Warp_E = br.ReadInt32 (),
				GuardWarp_I = br.ReadInt32 (),
				GuardWarp_E = br.ReadInt32 (),
			};
			frm.BtlPrm = btlPrm;
		}

		//演出パラメータ
		private void LoadBinPrmStaging ( BinaryReader br, Frame frm )
		{
			FrameParam_Staging stgPrm = new FrameParam_Staging ()
			{
				BlackOut = br.ReadByte (),
				Vibration = br.ReadByte (),
				Stop = br.ReadByte (),

				AfterImage_N = br.ReadByte (),
				AfterImage_time = br.ReadByte (),
				AfterImage_pitch = br.ReadByte (),
				Vibration_S = br.ReadByte (),
				Color = Color.FromArgb ( (int) br.ReadUInt32 () ),
				Color_time = br.ReadByte (),

				Rotate = br.ReadInt32 (),
				Rotate_center = new Point ( br.ReadInt32 (), br.ReadInt32 () ),
				Omega = br.ReadSingle (),	//float
				Scaling = new Point ( br.ReadInt32 (), br.ReadInt32 () ),
				Scaling_Center = new Point ( br.ReadInt32 (), br.ReadInt32 () ),
			};
			frm.StgPrm = stgPrm;
		}
	}

}
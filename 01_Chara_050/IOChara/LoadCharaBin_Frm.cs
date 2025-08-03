
using System.Drawing;

namespace ScriptEditor
{
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
                Frame scp = new Frame();

                //フレーム数は数え上げながら設定する
                scp.N = (int)i;

                //----------------------------------------
                //グループ
                scp.Group = br.ReadInt32();

                //----------------------------------------
                //イメージ名
#if false
				scp.ImgName = "Img_" + br.ReadUInt32 ().ToString ();	//後にイメージ名に変換
#else
                //イメージインデックス
                uint imgIndex = br.ReadUInt32();
                //イメージ名
                scp.ImgName = br.ReadString();  //[utf-8]
#endif

                //表示位置
                scp.Pos = new Point(br.ReadInt32(), br.ReadInt32());

                //ルート名リスト
                uint nRut = br.ReadUInt32();
                for (uint iRut = 0; iRut < nRut; ++iRut)
                {
                    //後にルート名に変換
                    scp.BD_RutName.Add(new TName("Rut_" + br.ReadUInt32().ToString()));
                }

                //枠リスト
                LoadBinListRect(br, scp.ListCRect);
                LoadBinListRect(br, scp.ListHRect);
                LoadBinListRect(br, scp.ListARect);
                LoadBinListRect(br, scp.ListORect);

                //エフェクト生成
                uint nEfGnrt = br.ReadUInt32(); //個数[uint]
                for (uint iEG = 0; iEG < nEfGnrt; ++iEG)
                {
                    EffectGenerate efgnrt = new EffectGenerate()
                    {
                        //エフェクト名は後で指定し直す
                        EfName = "Ef_" + br.ReadUInt32().ToString(),
                        Pt = new Point(br.ReadInt32(), br.ReadInt32()),
                        Z_PER100F = br.ReadInt32(),
                        Gnrt = br.ReadBoolean(),
                        Loop = br.ReadBoolean(),
                        Sync = br.ReadBoolean(),
                    };
                    scp.BD_EfGnrt.Add(efgnrt);
                }

                //バトルパラメータ
                ScriptParam_Battle btlPrm = new ScriptParam_Battle()
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
                scp.BtlPrm = btlPrm;

                //ステージングパラメータ
                ScriptParam_Staging stgPrm = new ScriptParam_Staging()
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
                    SE = (int)br.ReadUInt32(),
                    SE_name = br.ReadString(),
                    VC_name = br.ReadString(),
                };

                //		stgPrm.SE_name = "";
                //		stgPrm.VC_name = "";

                scp.StgPrm = stgPrm;


                //汎用パラメータ
                for (uint indexVst = 0; indexVst < scp.Versatile.Length; ++indexVst)
                {
                    scp.Versatile[indexVst] = br.ReadInt32();
                }


                lscp.Add(scp);
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
    }
}

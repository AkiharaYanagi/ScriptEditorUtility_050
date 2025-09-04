using ScriptEditorUtility;
using System.Drawing;
using System.Runtime.InteropServices.Marshalling;


namespace Chara050
{
    using BD_Sqc = BindingDictionary < Sequence >;


	//==================================================================================
	//	LoadCharaBin_Sqc
    //      シークエンス
	//==================================================================================
	public partial class LoadCharaBin
    {
        //コンペンド
        private void LoadBinCompend ( BinaryReader br, Compend cmpd )
        {
            //シークエンス個数
            uint N_Sqc = br.ReadUInt32();
            for (uint i = 0; i < N_Sqc; ++i)
            {
                //名前
                string name = br.ReadString();

                //各パラメータ（アクション固有）
                ActionParam actPrm = new ActionParam()
                {
                    NextActionName = "Act_" + br.ReadUInt32().ToString(),   //uintから後に名前に変換する
                    Category = (ActionCategory)br.ReadByte(),
                    Posture = (ActionPosture)br.ReadByte(),
                    HitNum = br.ReadByte(),
                    HitPitch = br.ReadByte(),
                    Balance = br.ReadInt32(),

                    Mana = br.ReadInt32(),
                    Accel = br.ReadInt32()
                };
                for (int v = 0; v < ActionParam.VRS_SIZE; ++v)
                {
                    actPrm.Versatile[v] = br.ReadInt32();
                }

                //シークエンスを確保
                Sequence act = new Sequence()
                {
                    Name = name,
                    ActPrm = actPrm,
                };

                //[]スクリプト
                LoadBinListScript(br, act.ListScript);

                //追加
                cmpd.BD_Sequence.Add(act);
            }
        }


        //IDから名前の再設定
        private void AssignName_NextSqc (BD_Sqc bdsqc)
        {
            //すべてのアクションを読み込んでから、
            //"次アクション名" をIDからstringとして得る
            foreach (Sequence? sqc in bdsqc.GetEnumerable())
            {
                if (sqc is null) { continue; }

                int nextActionID = GetIndex(sqc.ActPrm.NextActionName, "Act_");
                Sequence? next = bdsqc[nextActionID];
                if (next is null) { continue; }

                sqc.ActPrm.NextActionName = next.Name;
            }
        }

        private void AssignName_EfGnrt (BD_Sqc bdsqc, Compend gns )
        {
            //スクリプト内エフェクト生成におけるエフェクト名の再指定
            BD_Sqc bdGns = gns.BD_Sequence;

            //すべてのアクションを読み込んでから、
            //"次アクション名" をIDからstringとして得る
            foreach (Sequence? sqc in bdsqc.GetEnumerable())
            {
                if ( sqc is null ) { continue; }

                foreach (Frame? scp in sqc.ListScript)
                {
                    //エフェクト生成内のエフェクト名
                    foreach (EffectGenerate? efGnrt in scp.BD_EfGnrt.GetEnumerable())
                    {
                        if ( efGnrt is null ) { continue; }

                        int idEf = GetIndex( efGnrt.EfName, "Ef_");
                        Sequence? ef = bdGns[idEf];
                        if (ef is null) { continue; }

                        efGnrt.EfName = ef.Name;
                    }
                }
            }
        }


        //ビヘイビア
        private void LoadBinBehavior(BinaryReader br, Chara chara)
        {
            //Action : Sequence
            Compend bhv = chara.charaset.behavior;
            LoadBinCompend ( br, bhv );
#if false
            //アクション個数
            uint N_Act = br.ReadUInt32();


            for (uint i = 0; i < N_Act; ++i)
            {
                //名前
                string name = br.ReadString();

                //各パラメータ（アクション固有）
                ActionParam actPrm = new ActionParam()
                {
                    NextActionName = "Act_" + br.ReadUInt32().ToString(),   //uintから後に名前に変換する
                    Category = (ActionCategory)br.ReadByte(),
                    Posture = (ActionPosture)br.ReadByte(),
                    HitNum = br.ReadByte(),
                    HitPitch = br.ReadByte(),
                    Balance = br.ReadInt32(),

                    Mana = br.ReadInt32(),
                    Accel = br.ReadInt32()
                };
                for (int v = 0; v < ActionParam.VRS_SIZE; ++v)
                {
                    actPrm.Versatile[v] = br.ReadInt32();
                }

                //シークエンスを確保
                Sequence act = new Sequence()
                {
                    Name = name,
                    ActPrm = actPrm,
                };

                //[]スクリプト
                LoadBinListScript(br, act.ListScript);

                //追加
                bhv.BD_Sequence.Add(act);
            }
#endif

#if false
            //すべてのアクションを読み込んでから、
            //"次アクション名" をIDからstringとして得る
            foreach (Sequence? act in bhv.BD_Sequence.GetEnumerable())
            {
                if (act is null) { continue; }

                int nextActionID = GetIndex(act.ActPrm.NextActionName, "Act_");
                Sequence? sqc = bhv[nextActionID];
                if (sqc is null) { continue; }

                act.ActPrm.NextActionName = sqc.Name;
            }
#endif
        }

        //ガーニッシュ
        private void LoadBinGarnish(BinaryReader br, Chara chara)
        {
            //Effect : Sequence
            Compend gns = chara.charaset.garnish;
            LoadBinCompend ( br, gns );

#if false
            //スクリプト内エフェクト生成におけるエフェクト名の再指定
            BindingDictionary<Sequence> bdGns = chara.garnish.BD_Sequence;

            foreach (Effect efc in chara.garnish.BD_Sequence.GetEnumerable())
            {
                foreach (Script scp in efc.ListScript)
                {
                    //エフェクト名
                    foreach (EffectGenerate efGnrt in scp.BD_EfGnrt.GetEnumerable())
                    {
                        int idEf = GetIndex(efGnrt.EfName, "Ef_");
                        efGnrt.EfName = bdGns[idEf].Name;
                    }
                }
            }
#endif

#if false
            //アクションにおけるエフェクト名の再指定
            foreach (Action act in chara.behavior.BD_Sequence.GetEnumerable())
            {
                foreach (Script scp in act.ListScript)
                {
                    //エフェクト名
                    foreach (EffectGenerate efGnrt in scp.BD_EfGnrt.GetEnumerable())
                    {
                        int idEf = GetIndex(efGnrt.EfName, "Ef_");
                        efGnrt.EfName = bdGns[idEf].Name;
                    }
                }
            }
#endif

        }


    }
}

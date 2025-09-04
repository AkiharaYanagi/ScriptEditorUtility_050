using System.Diagnostics;


namespace Chara050
{
    public class TestChara
    {
        public TestChara() { }

        public void Test(Chara chara)
        {
            //初期化(Clear)テスト
            TestClearChara(chara);

#if false
            //EditCharaに設定
            EditChara.Inst.SetCharaDara(ch);

            //コピーテスト
            Chara copyChara = new Chara();
            TestCopyChara(copyChara, ch);

            //名前指定
            TestNameAssign(ch);

#endif
            //IOテスト
            TestIO ( chara );
        }

        //=====================================================================
        //	個別テスト
        //=====================================================================
      
        //初期化テスト
        public void TestClearChara(Chara chara)
        {
            //test chara.Clear()
            //		データ個数が０に初期化されているかテスト
            chara.Clear();
            CharaSet chset = chara.charaset;
            Debug.Assert(0 == chset.behavior.BD_Sequence.Count());
            Debug.Assert(0 == chset.garnish.BD_Sequence.Count());
            Debug.Assert(0 == chset.BD_Command.Count());
            Debug.Assert(0 == chset.BD_Branch.Count());
            Debug.Assert(0 == chset.BD_Route.Count());
        }

        //-----------------------------------------------------------
        //IOテスト

        //対象ファイル名
        private const string Filename = "testChara.dat";
        private const string FilenameBin = "testCharaBin.dat";

        public void TestIO(Chara ch)
        {
#if false
            TestIO_Document(ch);
            TestIO_TextFile(ch);
#endif
            TestIO_Bin(ch);
        }

#if false
        //ドキュメント
        public void TestIO_Document(Chara ch)
        {
            CharaToDoc ctd = new CharaToDoc();
            MemoryStream ms = ctd.Run(ch);
            Document doc = new Document(ms);

            Chara ch_new = new Chara();
            DocToChara dtc = new DocToChara();
            dtc.Load(doc, ch_new);

            Equal(ch, ch_new);

            Debug.WriteLine("TestIO_Document: OK.");
        }

        //テキストファイル
        public void TestIO_TextFile(Chara ch)
        {
            SaveChara saveChara = new SaveChara();
            saveChara.Do(Filename, ch);

            LoadChara loadChara = new LoadChara();
            loadChara.Do(Filename, ch);

            Debug.WriteLine("TestIO_TextFile: OK.");
        }

#endif

        //バイナリファイル
        public void TestIO_Bin(Chara ch)
        {
            string dir = Directory.GetCurrentDirectory();
            string filepathbin = Path.Combine(dir, Filename);

            SaveCharaBin saveCharaBin = new SaveCharaBin();
            saveCharaBin.Do(filepathbin, ch);

            LoadCharaBin loadCharaBin = new LoadCharaBin();
            loadCharaBin.Do(filepathbin, ch);

            Debug.WriteLine("TestIO_Bin: OK.");
        }


    }
}

using ScriptEditorUtility;


namespace Chara050
{
    using BD_Cmd = BindingDictionary<Command>;
    using BD_Brc = BindingDictionary<Branch>;
    using BD_Rut = BindingDictionary<Route>;


    //==================================================================================
    // ->  サウンド（ボイス）をコンペンドが保持する
    // ->  アクションとエフェクトを同一化する
    // ->  共通アクション
    // ->  他スクリプト項目の増減
    //==================================================================================
    //	キャラ 編集上データ ver 050
    // 
    //	キャラ
    //		┣キャラセット		コモン(共通)	
    //		┣キャラセット		パーソナル(個別)
    // 
    //	キャラセット
    //		┣コンペンド	ビヘイビア
    //		┣コンペンド	ガーニッシュ
    // 
    // ->編集時に分割(名前で分類)し、実行上は統合(ID)する
    // //IDでも、名前指定でも、共通と固有を同列で並べ、参照時に分岐する
    //==================================================================================

    public class CharaSet
    {
        public Compend behavior = new Compend ();
        public Compend garnish = new Compend ();

        public BD_Cmd BD_Command { get; } = new BD_Cmd();       //コマンドリスト
        public BD_Brc BD_Branch { get; } = new BD_Brc();        //ブランチリスト
        public BD_Rut BD_Route { get; } = new BD_Rut();     //ルートリスト(エディタにおける一時変数)
        

        public void Clear ()
        {
			behavior.Clear ();
			garnish.Clear ();
            BD_Command.Clear ();
            BD_Branch.Clear ();
            BD_Route.Clear ();
		}

		//----------------------------------------------------------------
		//名前からインデックスを取得

		//アクション
		public int GetIndexOfAction(string nameAction)
        {
            return behavior.BD_Sequence.IndexOf(nameAction);
        }
        //エフェクト
        public int GetIndexOfEffect(string nameEffect)
        {
            return garnish.BD_Sequence.IndexOf(nameEffect);
        }
        //コマンド
        public int GetIndexOfCommand(string nameCommand)
        {
            return BD_Command.IndexOf(nameCommand);
        }
        //ブランチ
        public int GetIndexOfBranch(string nameBranch)
        {
            return BD_Branch.IndexOf(nameBranch);
        }
        //ルート
        public int GetIndexOfRoute(string nameRoute)
        {
            return BD_Route.IndexOf(nameRoute);
        }

        //メインイメージ
        public int GetIndexOfMainImage(string nameImage)
        {
            return behavior.BD_Image.IndexOf(nameImage);
        }

        //EFイメージ
        public int GetIndexOfEFImage(string nameImage)
        {
            return garnish.BD_Image.IndexOf(nameImage);
        }
    }


    public class Chara
    {
        public CharaSet charaset = new CharaSet();
        public CharaSet charaset_common = new CharaSet();

        public void Clear ()
        {
            charaset.Clear ();
            charaset_common.Clear();
        }
    }
}

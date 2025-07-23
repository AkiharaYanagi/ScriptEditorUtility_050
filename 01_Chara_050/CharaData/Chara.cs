using System;


namespace ScriptEditor
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
        public BD_Rut BD_Route { get; } = new BD_Rut();		//ルートリスト(エディタにおける一時変数)
    }

    public class Chara
    {
        public CharaSet charaset = new CharaSet ();

        public void Clear ()
        {

        }
    }
}

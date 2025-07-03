using ScriptEditor;


namespace ScriptEditor020
{
	//==================================================================================
	//	キャラ データ ver110
	//==================================================================================

	//==================================================================================
	//	◆キャラ	
	//		┣ビヘイビア  ( [] アクション, []イメージ )
	//		┣ガーニッシュ( [] エフェクト, []イメージ )
	//		┣[] コマンド
	//		┣[] ブランチ
	//		┣[] ルート
	//
	//	イメージリストはコンペンド(キャラのビヘイビアとガーニッシュ)が持つように変更
	//
	//	外部からの要素指定はBindingDictionaryにおいて名前(TName)で行う
	//	->保存位置の変更に対応
	//==================================================================================
	using BD_Cmd = BindingDictionary < Command >;
	using BD_Brc = BindingDictionary < Branch >;
	using BD_Rut= BindingDictionary < Route >;


	public class Chara
	{
		//---------------------------------------------------------------------
		//Compend(一覧)継承のアクションとエフェクト
		public Behavior behavior = new Behavior ();		//アクションの集合
		public Garnish garnish = new Garnish ();        //エフェクトの集合

		public BD_Cmd BD_Command { get; } = new BD_Cmd ();		//コマンドリスト
		public BD_Brc BD_Branch { get; } = new BD_Brc ();		//ブランチリスト
		public BD_Rut BD_Route { get; } = new BD_Rut ();		//ルートリスト(エディタにおける一時変数)

		//---------------------------------------------------------------------
		//コンストラクタ
		public Chara ()
		{
			//空(null)状態を許容する
			//ダミーを用いない
			//各生成、受取時に１つ以上あるかチェックする
		}

		//コピーコンストラクタ
		public Chara ( Chara srcChara )
		{
			Clear ();

			behavior.Copy ( srcChara.behavior );
			garnish.Copy ( srcChara.garnish );

			BD_Command.DeepCopy ( srcChara.BD_Command );
			BD_Branch.DeepCopy ( srcChara.BD_Branch );
			BD_Route.DeepCopy ( srcChara.BD_Route );
		}

		//クリア
		//	すべてのデータについてメモリを解放し、個数を０にする
		public void Clear ()
		{
			behavior.Clear ();
			garnish.Clear ();
			BD_Command.Clear ();
			BD_Branch.Clear ();
			BD_Route.Clear ();
		}

		//イメージを残しスクリプトのみをクリア
		public void ClearScript ()
		{
			behavior.ClearScript ();
			garnish.ClearScript ();
			BD_Command.Clear ();
			BD_Branch.Clear ();
			BD_Route.Clear ();
		}

		//コピー
		public void Copy ( Chara ch )
		{
			Clear ();

			behavior.Copy ( ch.behavior );
			garnish.Copy ( ch.garnish );

			BD_Command.DeepCopy ( ch.BD_Command );
			BD_Branch.DeepCopy ( ch.BD_Branch );
			BD_Route.DeepCopy ( ch.BD_Route );
		}

		//クローン (ディープコピー)
		public void Clone ( Chara ch )
		{
			Clear ();

			behavior.Copy ( ch.behavior );
			garnish.Copy ( ch.garnish );

			BD_Command.DeepCopy ( ch.BD_Command );
			BD_Branch.DeepCopy ( ch.BD_Branch );
			BD_Route.DeepCopy ( ch.BD_Route );
		}

		//----------------------------------------------------------------
		//名前からインデックスを取得

		//アクション
		public int GetIndexOfAction ( string nameAction )
		{
			return behavior.BD_Sequence.IndexOf ( nameAction );
		}
		//エフェクト
		public int GetIndexOfEffect ( string nameEffect )
		{
			return garnish.BD_Sequence.IndexOf ( nameEffect );
		}
		//コマンド
		public int GetIndexOfCommand ( string nameCommand )
		{
			return BD_Command.IndexOf ( nameCommand );
		}
		//ブランチ
		public int GetIndexOfBranch ( string nameBranch )
		{
			return BD_Branch.IndexOf ( nameBranch );
		}
		//ルート
		public int GetIndexOfRoute ( string nameRoute )
		{
			return BD_Route.IndexOf ( nameRoute );
		}

		//メインイメージ
		public int GetIndexOfMainImage ( string nameImage )
		{
			return behavior.BD_Image.IndexOf ( nameImage );
		}

		//EFイメージ
		public int GetIndexOfEFImage ( string nameImage )
		{
			return garnish.BD_Image.IndexOf ( nameImage );
		}
	}
}

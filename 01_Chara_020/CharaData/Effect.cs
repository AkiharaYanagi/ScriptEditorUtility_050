using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ScriptEditor020
{

	//================================================================
	//	エフェクト		各フレームのスクリプトリストを持つ
	//		┣スクリプト[]
	//================================================================
	//アクションとの差異
	//	アクションはキャラに対し、常に１つ
	//	エフェクトは０から複数個扱う
	//	
	public class Effect : Sequence
	{
		//コンストラクタ
		public Effect ()	//ロード時に空白が必要
		{
			Clear ();
		}

		public Effect ( string str ) : base ( str ) 
		{
			//baseのコンストラクタの後でbase.Clear()が呼ばれてしまうのでClear()を用いない
//			Clear ();
		}

		//継承元から生成するコンストラクタ
		public Effect ( Sequence sqc )
		{
			base.Copy ( sqc );
		}

		//コピーコンストラクタ
		public Effect ( Effect effect )
		{
			Copy ( effect );
		}

		//クリア
		//new修飾子は継承元の同名関数を隠す
		//	※　スクリプトリストが０のまま扱わない
		public new void Clear ()
		{
			base.Clear ();
		}

		//コピー
		public void Copy ( Effect effect )
		{
			Clear ();
			base.Copy ( effect );
		}
	}

}

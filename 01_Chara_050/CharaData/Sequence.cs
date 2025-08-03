

namespace ScriptEditor
{
	//================================================================
	//	<シークエンス>		各フレーム毎のスクリプトをリスト状に持つ
	//		┣名前
	//		┣[]スクリプト
	//		///┣汎用パラメータ(SqcParamを継承)
	//================================================================
	[Serializable]
	public class Sequence : IName
	{
		//名前
		public string Name { get; set; } = "SqcName";
		public string GetName () { return Name; }
		public override string ToString () { return Name; }

		//スクリプトリスト
		public List < Frame > ListScript { get; set; } = new List < Frame > ();


        //-------------------------------------------------------------
		//Actionで扱っていた項目をSequenceに移項
		public ActionParam ActPrm { get; set; } = new ActionParam ();


        //-------------------------------------------------------------



        //コンストラクタ
        public Sequence ()	//ロード時に空白が必要
		{
		}

		public Sequence ( string str )
		{
            Name = str;
			//ListScript.Add ( new Script () );   //自動的にスクリプトを持つ
		}

		//コピーコンストラクタ
		public Sequence ( Sequence sequence )
		{
            Name = sequence.Name;
			foreach (Frame script in sequence.ListScript )
			{
				ListScript.Add ( new Frame( script ) );
			}
		}

		//フレームの追加
		public void AddScript ()
		{
            Frame script = new Frame();
			script.N = ListScript.Count;
			ListScript.Add ( script );
		}
		public void AddScript (Frame script )
		{
			script.N = ListScript.Count;
			ListScript.Add ( script );
		}

		//クリア
		//	※　スクリプトリストが０のまま扱わない
		public virtual void Clear ()
		{
			Name = "Clear";
			foreach ( Frame script in ListScript )
			{
				script.Clear ();
			}
			ListScript.Clear ();
		}

		//全体コピー
		public virtual void Copy ( Sequence sequence )
		{
			Clear ();
            Name = sequence.Name;
			foreach (Frame script in sequence.ListScript )
			{
				ListScript.Add ( new Frame( script ) );
			}
		}

		//スクリプトリストのみコピー
		public virtual void CopyScpList ( Sequence sequence )
		{
			ListScript.Clear ();
			foreach (Frame script in sequence.ListScript )
			{
				ListScript.Add ( new Frame( script ) );
			}
		}
	}

}

using ScriptEditor;


namespace ScriptEditor020
{
	//------------------------------------------------
	//	各種条件によるスクリプト分岐
	//------------------------------------------------
	public class Branch : IName
	{
		//------------------------------------------------
		// IName 名前
		public string Name { get; set; } = "BrcName";
		//------------------------------------------------
			
		//条件 定数
		public BranchCondition Condition { get; set; } = BranchCondition.CMD;

		//条件　コマンド名
		public string NameCommand { get; set; } = "NameCommand";

		//遷移先　シークエンス名
		public string NameSequence { get; set; } = "NameSequence";

		//遷移先　フレーム指定
		public int Frame { get; set; } = 0;

		//条件 同一アクション以外
		public bool Other { get; set; } = false;	


		//------------------------------------------------
		//コンストラクタ
		public Branch ()
		{
		}

		public Branch ( string name )
		{
			Name = name;
		}

		public Branch ( string strCommand, string strSequence )
		{
			NameCommand = strCommand;
			NameSequence = strSequence;
		}


		//比較
		public override bool Equals ( object? obj )
		{
			return base.Equals ( obj );
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
	}

}



namespace ScriptEditor
{
	//------------------------------------------------
	//	各種条件によるスクリプト分岐
	//		050: 複数条件を保持に変更
	//			条件は複数、遷移先は常に１つ
	//------------------------------------------------
	public class Branch : IName
	{
		//------------------------------------------------
		// IName 名前
		public string Name { get; set; } = "BrcName";
		//------------------------------------------------
			
		//条件 定数
		public BranchCondition Condition { get; set; } = BranchCondition.CMD;

		//条件　コマンド名（複数）
//		public string NameCommand { get; set; } = "NameCommand";
		public BindingDictionary < TName > BD_NameCommand { get; set; } = new BindingDictionary < TName > ();

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
            BD_NameCommand.Add ( new TName ( strCommand ) );
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

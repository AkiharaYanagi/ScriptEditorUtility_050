using ScriptEditor;



namespace ScriptEditor020
{
	//--------------------------------------------
	//	選択、スクリプト内で可能なブランチの集合
	//--------------------------------------------
	public class Route : IName
	{
		//名前
		public string Name { get; set; } = "RutName0";
		public string GetName () { return Name; }

		//摘要
		public string Summary { get; set; } = "摘要";

		//ブランチネームリスト
		public BindingDictionary < TName > BD_BranchName { get; set; } = new BindingDictionary < TName > ();


		//コンストラクタ
		public Route ()
		{
		}

		public Route ( string name )
		{
			Name = name;
		}

		public Route ( string name, string summary )
		{
			Name = name;
			Summary = summary;
		}
	}
}

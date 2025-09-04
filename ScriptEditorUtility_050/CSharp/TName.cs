//=============================================================
// 名前(string)を持つインターフェースINameの実行用継承クラス
//=============================================================

namespace ScriptEditorUtility
{
	public interface IName
	{
		string Name { get; set; }
	}

	public class TName : IName
	{
		public string Name { get; set; } = "TName";

		public TName ()
		{
		}

		public TName ( object ob )
		{
			Name = ob?.ToString () ?? string.Empty;
		}

		public TName ( string n )
		{
			Name = n;
		}
	}
}

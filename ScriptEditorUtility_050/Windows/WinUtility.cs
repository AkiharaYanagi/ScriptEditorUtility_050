

namespace ScriptEditorUtility
{
	public static class WinUtility
	{
		//-------------------------------------------------------
		//既存ファイルを全削除
		public static void DeleteAllFile ( string path )
		{
			IEnumerable < string > files = Directory.EnumerateFiles ( path, "*" );
			foreach ( string s in files )
			{
				FileInfo f = new FileInfo ( s );
				f.Delete ();
			}
		}
	}
}

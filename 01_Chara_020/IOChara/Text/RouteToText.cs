using System;
using System.IO;


namespace ScriptEditor
{
	using BD_RUT = BindingDictionary < Route >;

	public class RouteToText
	{
		public RouteToText() { }

		public void Do_BD ( StreamWriter sw, BD_RUT bd_rut )
		{
			foreach ( Route rut in bd_rut.GetEnumerable() )
			{
				Do_Single ( sw, rut );
				sw.Write ( '\n' );
			}
		}

		public void Do_Single ( StreamWriter sw, Route rut )
		{
			//名前
			sw.Write ( rut.Name + "," );
			//要約
			sw.Write ( rut.Summary + ";" );	//前半区切り";"セミコロン
			//ブランチ名
			int i = 0;
			int count = rut.BD_BranchName.Count ();
			foreach ( TName tn in rut.BD_BranchName.GetEnumerable() )
			{
				sw.Write ( tn.Name );
				if ( count == ++ i ) { break; }	//リストの最後には","カンマを追加しない
				sw.Write ( "," );
			}
			sw.Write ( ";" );
		}

	}
}

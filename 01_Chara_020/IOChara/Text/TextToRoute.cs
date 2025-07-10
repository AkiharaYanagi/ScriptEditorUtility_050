using System;
using System.IO;


namespace ScriptEditor
{
	using BD_RUT = BindingDictionary < Route >;


	public class TextToRoute
	{
		public TextToRoute() { }


		public void Do_BD ( StreamReader sr, BD_RUT bd_rut )
		{
			while ( ! sr.EndOfStream )
			{
				Route rut = new Route ();
				Do_Single ( sr, rut );
				bd_rut.Add ( rut );
			}
		}

		public void Do_Single ( StreamReader sr, Route rut )
		{
			string str = sr.ReadLine ();
			string[] str_spl_semi = str.Split ( ';' ); //	"前半";"後半";
			
			//前半	"名前","要約";
			string[] str_pre = str_spl_semi[0].Split(',');
			rut.Name = str_pre[0];
			rut.Summary = str_pre[1];

			//後半	"ブランチ名",……,"ブランチ名";
			string[] str_brcName = str_spl_semi[1].Split(',');
			foreach ( string strName in str_brcName )
			{
				rut.BD_BranchName.Add ( new TName ( strName ) );
			}
		}
	}
}

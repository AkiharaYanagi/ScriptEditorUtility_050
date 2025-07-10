using System;
using System.IO;


namespace ScriptEditor
{
	using BD_BRC = BindingDictionary < Branch >;


	public class TextToBranch
	{
		public TextToBranch() {}

		public void Do_BD ( StreamReader sr, BD_BRC bd_brc )
		{
			while ( ! sr.EndOfStream )
			{
				Branch brc = new Branch();
				Do_Single ( sr, brc );
				bd_brc.Add ( brc );
			}
		}

		public void Do_Single ( StreamReader sr, Branch brc )
		{
			string str = sr.ReadLine ();
			string[] str_spl = str.Split(',');
			brc.Name = str_spl[0];
			brc.Condition = ( BranchCondition ) Enum.Parse ( typeof ( BranchCondition ), str_spl[1] );
			brc.NameCommand = str_spl[2];
			brc.NameSequence = str_spl[3];
			brc.Frame = int.Parse ( str_spl[4] );
			brc.Other = bool.Parse ( str_spl [5] );
		}
	}
}

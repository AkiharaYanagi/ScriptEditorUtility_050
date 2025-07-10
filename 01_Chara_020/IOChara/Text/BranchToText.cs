using System;
using System.IO;


namespace ScriptEditor
{
	using BD_BRC = BindingDictionary < Branch >;

	public class BranchToText
	{
		public BranchToText() { }

		public void Do_BD ( StreamWriter sw, BD_BRC bd_brc )
		{
			foreach ( Branch brc in bd_brc.GetEnumerable() )
			{
				Do_Single ( sw, brc );
				sw.Write ( '\n' );
			}
		}

		public void Do_Single ( StreamWriter sw, Branch brc )
		{
			sw.Write ( brc.Name + "," );
			sw.Write ( brc.Condition + "," );
			sw.Write ( brc.NameCommand + "," );
			sw.Write ( brc.NameSequence + "," );
			sw.Write ( brc.Frame );
			sw.Write ( brc.Other );
		}

	}
}

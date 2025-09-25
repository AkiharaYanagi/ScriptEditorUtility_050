using System;
using System.Security.Cryptography.X509Certificates;
using ScriptEditorUtility;


namespace Chara050
{
	public class Generator : IName
	{
		public string Name { get; set; } = "";
		public Generate_Condition Gnrt_cnd { get; set; } = Generate_Condition.COMPULSION;
		public int Group { get; set; } = 0;
	}
}

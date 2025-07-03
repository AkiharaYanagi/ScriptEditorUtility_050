using System.Drawing;


namespace ScriptEditor
{
	//---------------------------------------------------------------------
	//	スクリプト内でエフェクトを生成するクラス
	//---------------------------------------------------------------------
	//	
	//	(背面)
	//	BG z = 0.90f
	//	Charaメインイメージz = 0.50f
	//	(前面)
	//
	
	public class EffectGenerate : IName
	{
		public string Name { get; set; } = "EfGnrt";    //エフェクトジェネレート名

		public string EfName { get; set; } = "Ef";	//対象のエフェクト名
		public Point Pt { get; set; } = new Point ( -200, -400 );	//位置
		public void SetPtX ( int x ) { Pt = new Point ( x, Pt.Y ); }
		public void SetPtY ( int y ) { Pt = new Point ( Pt.X, y ); }
		public int Z_PER100F = 60;					//Z位置 実行時100分の１でfloat扱い
		public bool Gnrt { get; set; } = true;		//生成(または非生成で繰返)
		public bool Loop { get; set; } = false;		//ループ
		public bool Sync { get; set; } = false;		//位置同期

		public EffectGenerate ()
		{
		}

	}
}

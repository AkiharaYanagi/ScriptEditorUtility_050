using ScriptEditorUtility;
using System.Drawing;
using System.Windows.Forms;


namespace Chara050
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
		public Generate_Condition GnrtCnd { get; set; } = Generate_Condition.COMPULSION;	//発生条件


		//描画モード
		public Draw_Mode drawmode = Draw_Mode.NORMAL;

		//ループ時強制終了
		public bool DeleteOut = true;	//画面外

		//またはカウント (0は無限)
		public int DeleteCount = 0;

		//同時生成不可（-> 飛び道具アクション側で制御(モーションもでないようにする)）
		//public bool DoubleIn = false;


		//相殺時、ヒット時、ガード時、次のエフェクト (""空文字列は終了して破棄)
		public string NextEfName_Offset = "";
		public string NextEfName_Hit = "";
		public string NextEfName_Guard = "";


		public EffectGenerate ()
		{
		}

	}
}

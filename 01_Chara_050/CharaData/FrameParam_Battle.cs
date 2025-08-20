using System.Drawing;


namespace ScriptEditor
{
	//スクリプトにおけるパラメータ(戦闘)
	//================================================================
	//	◆スクリプト		キャラにおけるアクションの１フレームの値
	//		┣攻撃値
	//		┣のけぞり
	//		┣移動(自分)
	//		┣移動(相手)
	//		┣消費バランス値
	//		┣バランス減少(相手)
	//================================================================
	public class FrameParam_Battle
    {
		//--------------------------------------------------------------------
		//	戦闘パラメータ
		//--------------------------------------------------------------------

		//計算状態(加算/代入/持続)
		public CLC_ST CalcState { get; set; } = CLC_ST.CLC_SUBSTITUDE;

		//速度
		public Point Vel { get; set; } = new Point ( 0, 0 );
		public void SetVel ( int x, int y ) { Vel = new Point ( x, y ); }
		public void SetVelX ( int x ) { Vel = new Point ( x, Vel.Y ); }
		public void SetVelY ( int y ) { Vel = new Point ( Vel.X, y ); }

		//加速度
		public Point Acc { get; set; } = new Point ( 0, 0 );
		public void SetAcc ( int x, int y ) { Acc = new Point ( x, y ); }
		public void SetAccX ( int x ) { Acc = new Point ( x, Acc.Y ); }
		public void SetAccY ( int y ) { Acc = new Point ( Acc.X, y ); }

		//他
		public int Power { get; set; } = 0;		//攻撃値
		public int Warp { get; set; } = 0;		//ヒット時のけぞり[F]
		public int Recoil_I { get; set; } = 0;	//反動(x,y)(自分)
		public int Recoil_E { get; set; } = 0;	//反動(x,y)(相手)
		public int Blance_I { get; set; } = 0;	//バランス増減(自分)
		public int Blance_E { get; set; } = 0;	//バランス増減(相手)
		public int DirectDamage { get; set;　} = 0;	//直接ダメージ


		//================================================================

		//コンストラクタ
		public FrameParam_Battle()
		{
		}

		//コピーコンストラクタ
		public FrameParam_Battle(FrameParam_Battle src )
		{
			CalcState = src.CalcState;
			Vel = src.Vel;		//@info Drawing.Pointは値型
			Acc = src.Acc;
			Power = src.Power;
			Warp = src.Warp;
			Recoil_I = src.Recoil_I;
			Recoil_E = src.Recoil_E;
			Blance_I = src.Blance_I;
			Blance_E = src.Blance_E;

			DirectDamage = src.DirectDamage;
		}

		//初期化
		public void Clear ()
		{
			CalcState = CLC_ST.CLC_SUBSTITUDE;
			Vel = new Point ();
			Acc = new Point ();
			Power = 0;
			Warp = 0;
			Recoil_I = 0;
			Recoil_E = 0;
			Blance_I = 0;
			Blance_E = 0;

			DirectDamage = 0;
		}

		//コピー
		public void Copy (FrameParam_Battle src )
		{
			CalcState = src.CalcState;
			Vel = src.Vel;		//@info Drawing.Pointは値型
			Acc = src.Acc;
			Power = src.Power;
			Warp = src.Warp;
			Recoil_I = src.Recoil_I;
			Recoil_E = src.Recoil_E;
			Blance_I = src.Blance_I;
			Blance_E = src.Blance_E;

			DirectDamage = src.DirectDamage;
		}
	}
}

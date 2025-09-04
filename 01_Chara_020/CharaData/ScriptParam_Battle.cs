using System.Drawing;


namespace Chara020
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
	public class ScriptParam_Battle
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
		public ScriptParam_Battle ()
		{
		}

		//コピーコンストラクタ
		public ScriptParam_Battle ( ScriptParam_Battle src )
		{
			this.CalcState = src.CalcState;
			this.Vel = src.Vel;		//@info Drawing.Pointは値型
			this.Acc = src.Acc;
			this.Power = src.Power;
			this.Warp = src.Warp;
			this.Recoil_I = src.Recoil_I;
			this.Recoil_E = src.Recoil_E;
			this.Blance_I = src.Blance_I;
			this.Blance_E = src.Blance_E;

			this.DirectDamage = src.DirectDamage;
		}

		//初期化
		public void Clear ()
		{
			CalcState = CLC_ST.CLC_SUBSTITUDE;
			this.Vel = new Point ();
			this.Acc = new Point ();
			this.Power = 0;
			this.Warp = 0;
			this.Recoil_I = 0;
			this.Recoil_E = 0;
			this.Blance_I = 0;
			this.Blance_E = 0;

			this.DirectDamage = 0;
		}

		//コピー
		public void Copy ( ScriptParam_Battle src )
		{
			this.CalcState = src.CalcState;
			this.Vel = src.Vel;		//@info Drawing.Pointは値型
			this.Acc = src.Acc;
			this.Power = src.Power;
			this.Warp = src.Warp;
			this.Recoil_I = src.Recoil_I;
			this.Recoil_E = src.Recoil_E;
			this.Blance_I = src.Blance_I;
			this.Blance_E = src.Blance_E;

			this.DirectDamage = src.DirectDamage;
		}
	}
}

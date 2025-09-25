using System.Drawing;


namespace Chara050
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
		public int Power { get; set; } = 0;     //攻撃値
		public int DirectDamage_I { get; set; } = 0;  //直接ダメージ(自分)
		public int DirectDamage_E { get; set; } = 0;  //直接ダメージ(相手)

		public int Recoil_I { get; set; } = 0;	//反動(x,y)(自分)
		public int Recoil_E { get; set; } = 0;  //反動(x,y)(相手)
		public int Blance_I { get; set; } = 0;  //バランス増減(自分)
		public int Blance_E { get; set; } = 0;  //バランス増減(相手)
		public int Gauge_I { get; set; } = 0;  //ゲージ増減(自分)
		public int Gauge_E { get; set; } = 0;  //ゲージ増減(相手)
		public int Warp_I { get; set; } = 0;    //ヒット時のけぞり[F](自分)
		public int Warp_E { get; set; } = 0;    //ヒット時のけぞり[F](相手)
		public int GuardWarp_I { get; set; } = 0;    //ガード時のけぞり[F](自分)
		public int GuardWarp_E { get; set; } = 0;    //ガード時のけぞり[F](相手)


		//================================================================

		//コンストラクタ
		public FrameParam_Battle ()
		{
		}

		//コピーコンストラクタ
		public FrameParam_Battle(FrameParam_Battle src )
		{
			CalcState = src.CalcState;
			Vel = src.Vel;		//@info Drawing.Pointは値型
			Acc = src.Acc;

			Power = src.Power;
			DirectDamage_I = src.DirectDamage_I;
			DirectDamage_E = src.DirectDamage_E;

			Recoil_I = src.Recoil_I;
			Recoil_E = src.Recoil_E;
			Blance_I = src.Blance_I;
			Blance_E = src.Blance_E;
			Gauge_I = src.Gauge_I;
			Gauge_E = src.Gauge_E;

			Warp_I = src.Warp_I;
			Warp_E = src.Warp_E;
			GuardWarp_I = src.GuardWarp_I;
			GuardWarp_E = src.GuardWarp_E;
		}

		//初期化
		public void Clear ()
		{
			CalcState = CLC_ST.CLC_SUBSTITUDE;
			Vel = new Point ();
			Acc = new Point ();

			Power = 0;
			DirectDamage_I = 0;
			DirectDamage_E = 0;

			Recoil_I = 0;
			Recoil_E = 0;
			Blance_I = 0;
			Blance_E = 0;
			Gauge_I = 0;
			Gauge_E = 0;

			Warp_I = 0;
			Warp_E = 0;
			GuardWarp_I = 0;
			GuardWarp_E = 0;
		}

		//コピー
		public void Copy (FrameParam_Battle src )
		{
			CalcState = src.CalcState;
			Vel = src.Vel;      //@info Drawing.Pointは値型
			Acc = src.Acc;

			Power = src.Power;
			DirectDamage_I = src.DirectDamage_I;
			DirectDamage_E = src.DirectDamage_E;

			Recoil_I = src.Recoil_I;
			Recoil_E = src.Recoil_E;
			Blance_I = src.Blance_I;
			Blance_E = src.Blance_E;
			Gauge_I = src.Gauge_I;
			Gauge_E = src.Gauge_E;

			Warp_I = src.Warp_I;
			Warp_E = src.Warp_E;
			GuardWarp_I = src.GuardWarp_I;
			GuardWarp_E = src.GuardWarp_E;
		}
	}
}

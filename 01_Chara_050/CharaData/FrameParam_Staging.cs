using System.Drawing;


namespace Chara050
{
	//スクリプトにおけるパラメータ(演出)
	//================================================================
	//	◆スクリプト		キャラにおけるアクションの１フレームの値
	//		┣暗転	┣全体振動	┣全体停止
	//		┣回転	┣残像間隔	┣残像個数	┣残像持続
	//		┣個別振動	┣色調変更	┣色調変更持続
	//================================================================
	public class FrameParam_Staging
    {
		//演出(全体)
		public int BlackOut { get; set; } = 0;		//暗転[F]
		public int Vibration { get; set; } = 0;		//振動[F](全体)
		public int Stop { get; set; } = 0;			//停止[F](全体)

		//------
		//演出(個別)

		//残像
		public int AfterImage_N { get; set; } = 0;		//残像[個]
		public int AfterImage_time { get; set; } = 0;	//残像[F] 持続
		public int AfterImage_pitch { get; set; } = 0;	//残像[F] pitch

		//その他
		public int Vibration_S { get; set; } = 0;		//振動[F](個別)
		public Color Color { get; set; } = Color.White;	//色調変更
		public int Color_time { get; set; } = 0;		//色調変更[F] 持続

		//回転
		public int Rotate { get; set; } = 0;		//回転[rad]
		public Point Rotate_center = new Point ();	//回転中心位置
		public void SetRotate_center ( int x, int y ) { Rotate_center = new Point ( x, y ); }
		public void SetRotate_centerX ( int x ) { Rotate_center = new Point ( x, Rotate_center.Y ); }
		public void SetRotate_centerY ( int y ) { Rotate_center = new Point ( Rotate_center.X, y ); }
		
		public float Omega { get; set; } = 0;	//角速度[rad/F];

		//拡大
		public Point Scaling { get; set; } = new Point ();	//拡大縮小 1/100
		public void SetScaling ( int x, int y ) { Scaling = new Point ( x, y ); }
		public void SetScalingX ( int x ) { Scaling = new Point ( x, Scaling.Y ); }
		public void SetScalingY ( int y ) { Scaling = new Point ( Scaling.X, y ); }
		
		public Point Scaling_Center { get; set; } = new Point ();
		public void SetScaling_Center ( int x, int y ) { Scaling_Center = new Point( x, y ); }  //拡大中心

#if false
		//->Frame内に移項

		//SE, VC
		//public int SE { get; set; } = 0;    //SE指定

		public	Generate_Condition SE_GnrtCnd = Generate_Condition.COMPULSION;	//発生条件
		public string SE_name = ""; //SE名指定

		public Generate_Condition VC_GnrtCnd = Generate_Condition.COMPULSION;	//発生条件
		public string VC_name = "";	//音声指定
#endif


		//================================================================

		//コンストラクタ
		public FrameParam_Staging()
		{
		}

		//コピーコンストラクタ
		public FrameParam_Staging(FrameParam_Staging src )
		{
			BlackOut = src.BlackOut;
			Vibration = src.Vibration;
			Stop = src.Stop;

			AfterImage_pitch = src.AfterImage_pitch;
			AfterImage_N = src.AfterImage_N;
			AfterImage_time = src.AfterImage_time;
			Vibration_S = src.Vibration_S;
			Color = src.Color;
			Color_time = src.Color_time;

			Rotate = src.Rotate;
			Rotate_center = src.Rotate_center;
			Omega = src.Omega;
			Scaling = src.Scaling;
			Scaling_Center = src.Scaling_Center;
		}

		//初期化
		public void Clear ()
		{
			BlackOut = 0;
			Vibration = 0;
			Stop = 0;

			AfterImage_pitch = 0;
			AfterImage_N = 0;
			AfterImage_time = 0;
			Vibration_S = 0;
			Color = new Color ();
			Color_time = 0;

			Rotate = 0;
			Rotate_center = new Point ( 0, 0 );
			Omega = 0;
			Scaling = new Point ( 0, 0 );
			Scaling_Center = new Point ( 0, 0 );
		}

		//コピー
		public void Copy (FrameParam_Staging src )
		{
			BlackOut = src.BlackOut;
			Vibration = src.Vibration;
			Stop = src.Stop;

			AfterImage_pitch = src.AfterImage_pitch;
			AfterImage_N = src.AfterImage_N;
			AfterImage_time = src.AfterImage_time;
			Vibration_S = src.Vibration_S;
			Color = src.Color;
			Color_time = src.Color_time;

			Rotate = src.Rotate;
			Rotate_center = src.Rotate_center;
			Omega = src.Omega;
			Scaling = src.Scaling;
			Scaling_Center = src.Scaling_Center;
		}

		//クローン
		public FrameParam_Staging Clone ()
		{
			return new FrameParam_Staging( this );
		}
	}
}

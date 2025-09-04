using Chara050;
using ScriptEditorUtility;



namespace ScriptEditor
{
	using R = Ctrl_Chara.Properties.Resources;
	using GK_ST = GameKeyData.GameKeyState;
	using GK_L = GameKeyData.Lever;
	using GK_B = GameKeyData.Button;

	public class DispCommand
	{
		//対象データ
		public Command Cmd { get; set; } = new Command ();

		//選択中キー
		public SelectKey SlctKey { get; set; } = new SelectKey ();

		//レバー選択位置
		public readonly int [] ItoLvr = { 1, 2, 3, 6, 9, 8, 7, 4, 5 }; 

		//データ設定
		public void Set ( Command cmd )
		{
			Cmd = cmd;
		}

		//--------------------------------------------------------------------------
		//内部使用定数
		//画像指定(リソース) 
		private readonly Image [] AryImgLvr_ = new Image []
		{
			R.CmdAr_Off, R.CmdAr_On, R.CmdAr_Push, R.CmdAr_Rele, R.CmdAr_Wild, R.CmdAr_Is, R.CmdAr_Nis,
		};

		private readonly Image [] AryImgState = new Image []
		{
			R.CmdAr_Off, R.CmdAr_On, R.CmdAr_Push, R.CmdAr_Rele, R.CmdAr_Wild, R.CmdAr_Is, R.CmdAr_Nis
		};

		private readonly Image [] AryImgBtnN = new Image []
		{
			R.Btn0, R.Btn1, R.Btn2, R.Btn3, R.Btn4, R.Btn5, R.Btn6, R.Btn7
		};

		//--------------------------------------------------------------------------

		private const int W = 32;	//升幅
		private const int H = 32;	//升高
		private const int BX = 32;	//基準X
		private const int BY = 32;	//基準Y

		//描画用定数
		private const int CULUMN = 16;
		private const int ROW = 10;

		public const int CULUMN_WIDTH = 48;
		public const int ROW_HEIGHT = 48;

		public const int DISIT_REVISED_POS_X = 24;
		public const int DISIT_REVISED_POS_Y = 14;
		
		//--------------------------------------------------------------------------
		public DispCommand ()
		{
			Image img = R.arrow;
		}


		//--------------------------------------------------------------------------
		//描画
		public void Disp ( PaintEventArgs e, Size size )
		{
			Graphics g = e.Graphics;

			//リソース使用の宣言
			using ( Font font0 = new Font ( "MS UI Gothic", 14, FontStyle.Regular ) )
			using ( Font font1 = new Font ( "MS UI Gothic", 12, FontStyle.Regular ) )
			using ( Pen pen0 = new Pen ( Color.FromArgb ( 0x80, 0x80, 0xff ), 1 ) )
			using ( Pen pen1 = new Pen ( Color.FromArgb ( 0xa0, 0xa0, 0xa0 ), 1 ) )
			using ( Pen pen2 = new Pen ( Color.FromArgb ( 0x40, 0x40, 0x40 ), 1.5f ) )
			using ( Brush brush_Rect = new SolidBrush ( Color.FromArgb ( 255, 255, 255, 255 ) ) )
			using ( Brush brush_BG_NODATA = new SolidBrush ( Color.FromArgb ( 255, 220, 220, 220 ) ) )
			using ( Brush brush_BG_OUT = new SolidBrush ( Color.FromArgb ( 255, 180, 180, 180 ) ) )
			using ( Brush brush_Not = new SolidBrush ( Color.FromArgb ( 63, 255, 63, 63 ) ) )
			{

			//描画用一時変数
			Bitmap bmp = new Bitmap ( CULUMN_WIDTH * CULUMN, ROW_HEIGHT * ROW + 1 );
			int w = size.Width;
			int h = size.Height;
			const int CW = CULUMN_WIDTH;
			const int RH = ROW_HEIGHT;

			//フレーム数
			StringFormat sf = new StringFormat { Alignment = StringAlignment.Center };
			for ( int i = 0; i < w / CULUMN_WIDTH; ++i )
			{
				g.DrawString ( i.ToString (), font0, Brushes.Gray, CW + DISIT_REVISED_POS_X + i * CW, DISIT_REVISED_POS_Y, sf );
			}

			//見出：レバー(十字),ボタン(0-7)
			g.DrawImage ( R.arrow	, 0, RH * 1, CW, RH );
			for ( uint i = 0; i < 8; ++ i )
			{
				g.DrawImage ( AryImgBtnN [ i ], 0, RH * ( 2 + i ), CW, RH );
			}

			//枠背景
			//枠外
			g.FillRectangle ( brush_BG_OUT, 0, RH * 10, w, h - RH * 10 );
			//データ無
			int n = Cmd.ListGameKeyCommand.Count;
			g.FillRectangle ( brush_BG_NODATA, CW + (CW * n), RH, w - (CW * n), RH * 9 );


			//コマンドキーの表示
			int iFrame = 0;
			foreach ( GameKeyCommand gkc in Cmd.ListGameKeyCommand )
			{
				//レバー
				int iLvr = 0;
				foreach ( GK_L key in gkc.DctLvrSt.Keys )
				{
					int imgIndex = (int) gkc.DctLvrSt [ key ];
						
					int pos_i = ItoLvr [ iLvr ] - 1;
					int x = CW + CW * iFrame + ( 16 * (pos_i % 3) );
					int y = RH + ( 16 * (2 - (pos_i / 3) ) );

					g.DrawImage ( AryImgLvr_ [ imgIndex ], x, y, 16, 16 );
					
					if ( GK_ST.KEY_WILD != gkc.DctLvrSt [ key ] )
					{
						g.DrawString ( ItoLvr[ iLvr ].ToString (), font1, Brushes.Black, x + 2, y );
					}

					++ iLvr;
				}

				//ボタン
				int iBtn = 0;
				foreach ( GK_B key in gkc.DctBtnSt.Keys )
				{
					//入力種類の取得
					GK_ST gkcstB = gkc.DctBtnSt [ key ];

					//入力種類による色別背景
					g.DrawImage ( AryImgState [ (int)gkcstB ], CW + CW * iFrame, RH * 2 + RH * iBtn, CW, RH );

					//ボタンNの表示
					if ( GK_ST.KEY_WILD != gkcstB && GK_ST.KEY_OFF != gkcstB )
					{
						Image imgBtnN = AryImgBtnN [ (int)key ];
						g.DrawImage ( imgBtnN, CW + CW * iFrame, RH * 2 + RH * iBtn, CW, RH );
					}

					++ iBtn;
					if ( iBtn >= SelectKey.DispKeyNum ) { break; }
				}

				//否定
				if ( gkc.Not )
				{
					g.FillRectangle ( brush_Not, CW + (CW * iFrame), RH, CW, RH * 5 );
				}

				++ iFrame;
			}


			//罫線
			g.DrawLine ( pen0, CULUMN_WIDTH, 0, CULUMN_WIDTH, h );
			g.DrawLine ( pen0, 0, ROW_HEIGHT, w, ROW_HEIGHT );
			for ( int i = 0; i < w / 20; ++i )
			{
				g.DrawLine ( pen2, CW * ( i + 2 ), 0, CW * ( i + 2 ), h );
			}
			for ( int i = 0; i < h / 20; ++i )
			{
				g.DrawLine ( pen1, 0, RH * ( i + 2 ), w, RH * ( i + 2 ) );
			}

			//ボタンカーソル
			if ( SlctKey.Selecting )
			{
				int crs_x = CW + CW * SlctKey.Frame;
				int crs_y = RH + RH * (int)SlctKey.Kind;
				g.DrawImage ( R.cursor, crs_x - 4, crs_y - 4, CW + 8, RH + 8 );
			}

			//レバーカーソル
			int dispIndex = SlctKey.LvrTo_IndexOfDisp ();
			int si_x = dispIndex % 3;
			int si_y = dispIndex / 3;
			int Lvr_w = 16;
			int Lvr_h = 16;
			int Lvr_x = CW + ( Lvr_w * si_x ) - 3 + CW * SlctKey.Frame;
			int Lvr_y = CW + ( Lvr_h * si_y ) - 3;
			g.DrawImage ( R.LvrCur, Lvr_x, Lvr_y, Lvr_w + 6, Lvr_h + 6 );

			}	//リソース使用
		}
	}

}

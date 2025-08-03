using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Linq;


namespace ScriptEditor
{
	public static class FormUtility
	{
		//==============================================================
		//フォーム位置初期化
		private const int start_X = -500;	//フォーム位置補正定数
		private const int start_Y = -10;	//フォーム位置補正定数
		
		public static void InitPosition ( Form f, int dx, int dy )
		{
			//フォーム開始位置をカーソル位置にする
			f.StartPosition = FormStartPosition.Manual;
			Point ptStart = Cursor.Position;
			ptStart.X += dx;
			if ( ptStart.X < 0 ) { ptStart.X = 0; }
			ptStart.Y += dy;
			if ( ptStart.Y < 0 ) { ptStart.Y = 0; }
			f.Location = ptStart;
		}


		public static void InitPosition ( Form f )
		{
			InitPosition ( f, start_X, start_Y );
		}

		//==============================================================
		//フォーム開始位置をマウスのあるディスプレイの左上にする
		public static void InitStartDisplayOnMouse ( Form f )
		{
			f.StartPosition = FormStartPosition.Manual;

			//マウス位置のスクリーンを取得
			Screen? screen = Screen.AllScreens.FirstOrDefault ( s=>s.Bounds.Contains ( Cursor.Position ) );
			if ( screen is null ) { return; }

			Point pt = new Point ( 50 + screen.Bounds.Left, 50 + screen.Bounds.Top );

			f.Location = pt;
		
		}


		//==============================================================
		//現在ディレクトリをエクスプローラで開く
		public static void OpenCurrentDir ()
		{
			//"@"はコマンドラインに文字列が表示されないようにする構文
			// Explorer.exe . (ピリオドで現在ディレクトリ)
			Process.Start ( "Explorer.exe", @"." );
		}
		
		//指定パスをエクスプローラで開く
		public static void OpenDir ( string path )
		{
			Process.Start ( "Explorer.exe", @path );
		}

		//==============================================================
		//上のディレクトリ
		public static string UpDir ( string path )
		{
			return path.Substring ( 0, path.LastIndexOf ( @"\" ) + 1 );
		}

		//==============================================================
		//ステータスバー表示
		public static ToolStripStatusLabel ToolStripStatusLabel { get; set; } = new ToolStripStatusLabel ();

		public static void Trace ( string str )
		{
			ToolStripStatusLabel.Text = str;
		}

	}
}

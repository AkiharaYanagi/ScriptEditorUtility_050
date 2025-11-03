using System.Diagnostics;
using Chara020;
using Chara050;
using test00_Chara;
using ScriptEditorUtility;


namespace ScriptEditor
{
	public partial class Form1 : Form
	{
		public Form1 ()
		{
			FormUtility.InitPosition ( this );
			InitializeComponent ();

			//ステータスラベル
			STS_TXT.Tssl = toolStripStatusLabel1;
			STS_TXT.Trace ( "開始." );
		}

		public void Do ( string filepath )
		{
			//データ変換

			//旧キャラデータ020 を Load
			Chara020.Chara chara020 = new Chara020.Chara ();
			Chara020.LoadCharaBin lcb020 = new Chara020.LoadCharaBin ();
			lcb020.Do ( filepath, chara020 );

			STS_TXT.Trace ( "Convert 開始." );

			//新規キャラデータ050 に Convert
			ConvertChara cvtCh = new ConvertChara ();
			Chara050.Chara chara050 = cvtCh.Convert ( chara020 );


#if false
			//test Chara
			STS_TXT.Trace ( "Test 開始." );
			TestChara testChara = new TestChara ();
			testChara.Test ( chara050 );
#endif
			//チェック
			Check020_to_050 ( chara020, chara050 );

			//書出
			STS_TXT.Trace ( "Save 開始." );
			Chara050.SaveCharaBin scb050 = new SaveCharaBin ();

#if false
			string? fileDir = Path.GetDirectoryName ( filepath );
			string? filename050 = Path.GetFileNameWithoutExtension ( filepath );
			string savefilepath = fileDir + "\\" + filename050 + "050.dat";
#endif

			//fileをディレクトリ"DAT"に書出
			string outdir = Path.GetDirectoryName ( filepath ) + "\\DAT";
			Directory.CreateDirectory ( outdir );   //作成
			string filename = Path.GetFileName ( filepath );
			string outfilepath = Path.Combine ( outdir, filename );

			scb050.Do ( outfilepath, chara050 );
			STS_TXT.Trace ( "Save 終了." );

			//読込テスト
			Chara050.Chara ch_load = new Chara050.Chara ();
			Chara050.LoadCharaBin lcb050 = new Chara050.LoadCharaBin ();
			lcb050.Do ( outfilepath, ch_load );
		}

		public void Check020_to_050 ( Chara020.Chara ch2, Chara050.Chara ch5 )
		{
			Debug.WriteLine ( "chara050.behavior : " + ch5.charaset.behavior.BD_Sequence.Count ().ToString () );
			Debug.WriteLine ( "chara020.behavior : " + ch2.behavior.BD_Sequence.Count ().ToString () );

			Debug.WriteLine ( "chara050.garnish : " + ch5.charaset.garnish.BD_Sequence.Count ().ToString () );
			Debug.WriteLine ( "chara020.garnish : " + ch2.garnish.BD_Sequence.Count ().ToString () );

			Debug.WriteLine ( "chara020.Command : " + ch2.BD_Command.Count ().ToString () );
			Debug.WriteLine ( "chara050.Command : " + ch5.charaset.BD_Command.Count ().ToString () );

			Debug.WriteLine ( "chara020.Branch : " + ch2.BD_Branch.Count ().ToString () );
			Debug.WriteLine ( "chara050.Branch : " + ch5.charaset.BD_Branch.Count ().ToString () );

			Debug.WriteLine ( "chara020.Route : " + ch2.BD_Route.Count ().ToString () );
			Debug.WriteLine ( "chara050.Route : " + ch5.charaset.BD_Route.Count ().ToString () );
		}

		//scpファイルのみ
		public void Do_scp ( string filepath )
		{
			//データ変換

			//旧キャラデータ020 を Load
			Chara020.Chara chara020 = new Chara020.Chara ();
			Chara020.LoadCharaBin lcb020 = new Chara020.LoadCharaBin ();
			lcb020.Do_scp ( filepath, chara020 );

			STS_TXT.Trace ( "Convert 開始." );

			//新規キャラデータ050 に Convert
			ConvertChara cvtCh = new ConvertChara ();
			Chara050.Chara chara050 = cvtCh.Convert ( chara020 );

			//チェック
			Check020_to_050 ( chara020, chara050 );

			//書出
			STS_TXT.Trace ( "Save 開始." );
			Chara050.SaveCharaBin scb050 = new SaveCharaBin ();

#if false
			string? fileDir = Path.GetDirectoryName ( filepath );
			string? filename050 = Path.GetFileNameWithoutExtension ( filepath );
			string savefilepath = fileDir + "\\" + filename050 + "050.scp";
#endif

			//fileをディレクトリ"SCP"に書出
			string outdir = Path.GetDirectoryName ( filepath ) + "\\SCP";
			Directory.CreateDirectory ( outdir );   //作成
			string filename = Path.GetFileName ( filepath );
			string outfilepath = Path.Combine ( outdir, filename );


			scb050.Do_withoutImage ( outfilepath, chara050 );
			STS_TXT.Trace ( outfilepath + " : Save 終了." );

		}


		protected override void OnDragEnter ( DragEventArgs drgevent )
		{
			if ( drgevent.Data is null ) { return; }
			if ( drgevent.Data.GetDataPresent ( DataFormats.FileDrop ) )
			{
				drgevent.Effect = DragDropEffects.Copy;
			}
			else
			{
				drgevent.Effect = DragDropEffects.None;
			}
			base.OnDragEnter ( drgevent );
		}
		protected override void OnDragDrop ( DragEventArgs drgevent )
		{
			if ( drgevent is null ) { return; }
			IDataObject? d = drgevent.Data;

			if ( d is null ) { return; }
			string []? files = (string []?) d.GetData ( DataFormats.FileDrop );


			if ( files is null ) { return; }
			for ( int i = 0; i < files.Length; ++ i )
			{
				string filepath = files [ i ];
				textBox1.Text = filepath;
				textBox1.Invalidate ();

				string ext = Path.GetExtension ( filepath );
				if ( ext == ".scp" )
				{
					STS_TXT.Trace ( ".scp 読込開始." );
					Do_scp ( filepath );
					STS_TXT.Trace ( ""+ (i+1) + "個ファイル　.scp 書出" );
				}
				else
				{
					STS_TXT.Trace ( ".dat 読込開始." );
					Do ( filepath );
					STS_TXT.Trace ( ""+ (i+1) + "個ファイル　.dat 書出" );
				}
			}
			base.OnDragDrop ( drgevent );
		}

		private void フォルダToolStripMenuItem_Click ( object sender, EventArgs e )
		{
			FormUtility.OpenCurrentDir ();
		}
	}
}

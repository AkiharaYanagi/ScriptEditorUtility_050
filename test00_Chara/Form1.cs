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

			Debug.WriteLine ( "chara050.behavior : " + chara050.charaset.behavior.BD_Sequence.Count ().ToString () );
			Debug.WriteLine ( "chara050.garnish : " + chara050.charaset.garnish.BD_Sequence.Count ().ToString () );

			Debug.WriteLine ( "chara020.Command : " + chara020.BD_Command.Count ().ToString () );
			Debug.WriteLine ( "chara050.Command : " + chara050.charaset.BD_Command.Count ().ToString () );

			Debug.WriteLine ( "chara020.Branch : " + chara020.BD_Branch.Count ().ToString () );
			Debug.WriteLine ( "chara050.Branch : " + chara050.charaset.BD_Branch.Count ().ToString () );

			Debug.WriteLine ( "chara020.Route : " + chara020.BD_Route.Count ().ToString () );
			Debug.WriteLine ( "chara050.Route : " + chara050.charaset.BD_Route.Count ().ToString () );

#if false
			//test Chara
			STS_TXT.Trace ( "Test 開始." );
			TestChara testChara = new TestChara ();
			testChara.Test ( chara050 );

#endif


			//書出
			STS_TXT.Trace ( "Save 開始." );
			Chara050.SaveCharaBin scb050 = new SaveCharaBin ();
			string? fileDir = Path.GetDirectoryName ( filepath );
			string? filename050 = Path.GetFileNameWithoutExtension ( filepath );
			scb050.Do ( fileDir + "\\" + filename050 + "050.dat", chara050 );
			STS_TXT.Trace ( "Save 終了." );
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
			if ( files.Length > 0 )
			{
				string filepath = files [ 0 ];
				textBox1.Text = filepath;
				textBox1.Invalidate ();
				STS_TXT.Trace ( "読込開始." );

				Do ( filepath );
			}

			base.OnDragDrop ( drgevent );
		}

		private void フォルダToolStripMenuItem_Click ( object sender, EventArgs e )
		{
			FormUtility.OpenCurrentDir ();
		}
	}
}

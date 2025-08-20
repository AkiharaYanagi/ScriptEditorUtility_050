using System.Diagnostics;
using ScriptEditor020;
using test00_Chara;


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
		}

		public void Do ( string filepath )
		{
			//データ変換

			//旧キャラデータ020 を Load
			ScriptEditor020.Chara chara020 = new ScriptEditor020.Chara ();
			ScriptEditor020.LoadCharaBin lcb020 = new ScriptEditor020.LoadCharaBin ();
			lcb020.Do ( filepath, chara020 );

			STS_TXT.Trace ( "Convert 開始." );
			//新規キャラデータ050 に Convert
			ConvertChara cvtCh = new ConvertChara ();
			ScriptEditor.Chara chara050 = cvtCh.Convert ( chara020 );

			Debug.WriteLine ( "chara050.behavior : " + chara050.charaset.behavior.BD_Sequence.Count ().ToString () );
			Debug.WriteLine ( "chara050.garnish : " + chara050.charaset.garnish.BD_Sequence.Count ().ToString () );

			Debug.WriteLine ( "chara020.Command : " + chara020.BD_Command.Count ().ToString () );
			Debug.WriteLine ( "chara050.Command : " + chara050.charaset.BD_Command.Count ().ToString () );

			STS_TXT.Trace ( "Test 開始." );
			//test Chara
			TestChara testChara = new TestChara ();
			testChara.Test ( chara050 );

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

using System;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace ScriptEditor
{
	public class LoadTextChara
	{
		public LoadTextChara ( string filename, Chara chara )
		{
			try
			{
				_Load ( filename, chara );
			}
			catch ( ArgumentException e )
			{
				MessageBox.Show ( "LoadChara : 読込データが不適正です\n" + e.Message + "\n" + e.StackTrace );
			}
		}

		//テキスト形式のスクリプトを読み込み、Document形式でキャラに保存
		//Charaを指定ドキュメントファイルからロード
		private void _Load ( string filename, Chara chara )
		{
			//ファイルが存在しないとき何もしない
			if ( ! File.Exists ( filename ) )
			{
				MessageBox.Show ( filename + "が見つかりません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				throw new ArgumentException ( "ファイルが存在しませんでした。" );
			}

			//拡張子確認
			if ( Path.GetExtension ( filename ).CompareTo ( ".txt" ) != 0 )
			{
				MessageBox.Show ( filename + "は拡張子が.txtと異なります。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				throw new ArgumentException ( "拡張子が.txtと異なります。" );
			}

			//ファイルストリーム開始
			FileStream fstrm = new FileStream ( filename, FileMode.Open, FileAccess.Read );
			StreamReader ReaderFile = new StreamReader ( fstrm, Encoding.UTF8 );

			//メモリストリームに読込
			MemoryStream mstrmScript = new MemoryStream ();
			StreamWriter strmWrtScript = new StreamWriter ( mstrmScript, Encoding.UTF8 );
			while ( ! ReaderFile.EndOfStream ) 
			{
				string str = ReaderFile.ReadLine ();
				strmWrtScript.Write ( str );
			}
			strmWrtScript.Flush ();
			mstrmScript.Seek ( 0, SeekOrigin.Begin );

			//ドキュメントの読込
			Document document = new Document ( mstrmScript );

			//イメージリスト(メイン,Ef)を残しスクリプトだけクリア
			chara.ClearScript ();

			//キャラに変換(スクリプト部のみ)
			DocToChara dtoc = new DocToChara ();
			dtoc.LoadScriptData ( document, chara );

			fstrm.Dispose ();
		}
	}
}

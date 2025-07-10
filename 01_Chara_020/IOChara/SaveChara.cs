using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace ScriptEditor
{
	using BD_IDT = BindingDictionary < ImageData >;

	//==================================================================================
	//	SaveChara
	//		キャラのスクリプトをDocument形式、
	//		イメージアーカイブをpngのbinary形式で、
	//		両方を.datファイルに書出する
	//		また、確認用にDocumentのみテキストファイルにも書出する
	//==================================================================================
	public class SaveChara
	{
		//エラーメッセージ
		public string ErrMsg { get; set; } = "Err Msg.";

		//-------------------------------------------------------------
		//	コンストラクタ
		//-------------------------------------------------------------
		public SaveChara ()
		{
		}

		//-------------------------------------------------------------
		//	実行
		//引数　filepath : 保存ファイルパス, chara : 保存対象キャラデータ
		//-------------------------------------------------------------
		public void Do ( string filepath, Chara chara )
		{
			try
			{
				//Document形式側の名前を変更する
				_Save ( filepath, chara );

				//バイナリ保存も同時に行う
				//(Filename) + "Bin" + (拡張子)
				string dir = Path.GetDirectoryName ( filepath ) + "\\";
				string fn = Path.GetFileNameWithoutExtension ( filepath );
				string ex = Path.GetExtension ( filepath );
				string path_Bin = dir + fn + "Bin" + ex;

				SaveCharaBin saveCharaBin = new SaveCharaBin ();
				saveCharaBin.Do ( path_Bin, chara );
			}
			catch ( ArgumentException e )
			{
				Debug.WriteLine ( e );
			}

			ErrMsg = "Save OK.";
		}

		private void _Save ( string filepath, Chara chara )
		{
			//データが無かったら何もしない
			if ( chara == null ) { return; }

			//キャラスクリプトをDocument形式でテキストメモリストリームに変換
			CharaToDoc chtom = new CharaToDoc ();
			MemoryStream mstrmChara = chtom.Run ( chara );

			if( mstrmChara is null ) { return; }	//何もしないで続行

			//ストリームリーダにセット
			StreamReader strmReaderChara = new StreamReader ( mstrmChara, Encoding.UTF8 );

			//==========================================================================
			//確認用テキストファイル(.txt)書出

			//ファイル名の拡張子を.datから.txtに変える
			string path = Path.GetDirectoryName ( filepath );
			string filenameTxt = Path.GetFileNameWithoutExtension ( filepath ) + ".txt";
			string txt_path = path + '\\'+ filenameTxt;

			//ファイルにテキスト書出用ストリームを設定する
			StreamWriter strmWriter = new StreamWriter ( txt_path, false, Encoding.UTF8 );

			//書出
			mstrmChara.Seek ( 0, SeekOrigin.Begin );			//先頭から
			strmWriter.Write ( strmReaderChara.ReadToEnd () );	//最後まで書出し
			strmWriter.Close ();


			//==========================================================================
			//(スクリプト + イメージ) メモリストリーム書出

			//------------------------------
			//スクリプト部
			//メモリストリーム
			MemoryStream mstrmScript = new MemoryStream ();

			//マルチバイト文字を扱うためStreamWriterとUTF8を用いる
			StreamWriter strmWriterScp = new StreamWriter ( mstrmScript, Encoding.UTF8 );

			//キャラスクリプトを先頭から読み、ファイル書出用にバイナリで書き込む
			mstrmChara.Seek ( 0, SeekOrigin.Begin );
			strmWriterScp.Write ( strmReaderChara.ReadToEnd () );
			strmWriterScp.Flush ();


			//------------------------------
			//イメージバイナリデータ部
			//メモリストリーム
			MemoryStream mstrmImage = new MemoryStream ();
			BinaryWriter biWriterImage = new BinaryWriter ( mstrmImage );

			WriteListImage ( biWriterImage, chara.behavior.BD_Image );
			WriteListImage ( biWriterImage, chara.garnish.BD_Image );

			biWriterImage.Flush ();


			//==========================================================================
			//(スクリプト + イメージ) ファイル(.dat)書出

			//書出対象ファイル
			FileStream fstrm = new FileStream ( filepath, FileMode.Create, FileAccess.Write );
			BinaryWriter biWriterFile = new BinaryWriter ( fstrm, Encoding.UTF8 );

			//ストリーム読書用一時領域
			const int size = 4096;	//バッファサイズ
			byte[] buffer = new byte[ size ];	//バッファ
			int numBytes;		//バイト数

			//ver書出
			biWriterFile.Write ( ( uint ) IO_CONST.VER );

			//スクリプト部・サイズ書出
			biWriterFile.Write ( ( uint ) mstrmScript.Length );

			//スクリプト部・ストリーム書出
			mstrmScript.Seek ( 0, SeekOrigin.Begin );
			while ( ( numBytes = mstrmScript.Read ( buffer, 0, size ) ) > 0 )
			{
				biWriterFile.Write ( buffer, 0, numBytes );
			}

			//イメージ部・ストリーム書出
			mstrmImage.Seek ( 0, SeekOrigin.Begin );
			while ( ( numBytes = mstrmImage.Read ( buffer, 0, size ) ) > 0 )
			{
				biWriterFile.Write ( buffer, 0, numBytes );
			}

			Debug.WriteLine ( "fstrm.Length = " + fstrm.Length );

			//終了
			biWriterFile.Close ();
//			biWriterScript.Close ();
			biWriterImage.Close ();
			strmReaderChara.Close ();
		}

		//イメージリストの書出
		private void WriteListImage ( BinaryWriter biWriterImage, BD_IDT imagelist )
		{
			//ストリーム読込→書出用
			const int size = 4096;	//バッファサイズ
			byte[] buffer = new byte[ size ];	//バッファ
			int numBytes;		//バイト数

			//イメージリスト
			IEnumerable list =  imagelist.GetBindingList ();
			foreach ( ImageData imageData in list )
			{
				//各イメージを一時領域に書出
				MemoryStream tempMstrm = new MemoryStream ();
				imageData.Img.Save ( tempMstrm, ImageFormat.Png );

				//サイズの書込
				biWriterImage.Write ( ( uint ) tempMstrm.Length );

				//Debug.Write ( imageData.Name + ", " + ( uint ) tempMstrm.Length );

				//実データの書込
				tempMstrm.Seek ( 0, SeekOrigin.Begin );
				int sumBytes = 0;
				while ( ( numBytes = tempMstrm.Read ( buffer, 0, size ) ) > 0 )
				{
					biWriterImage.Write ( buffer, 0, numBytes );
					sumBytes += numBytes;
				}
				
				//Debug.Write ( " : sumBytes = " + sumBytes + "\n" );
				//tempMstrm.Close ();
				tempMstrm.Dispose ();
			}
		}
	}
}

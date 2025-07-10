using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Collections;

namespace ScriptEditor
{
	//==================================================================================
	//	LoadChara
	//		.datファイルからCharaスクリプトとイメージを読み込む
	//==================================================================================
	public partial class LoadChara
	{
		//エラーメッセージ
		public string ErrMsg { get; set; } = "ErrMsg";

		//-------------------------------------------------------------
		//	コンストラクタ
		//-------------------------------------------------------------
		public LoadChara ()
		{
		}

		//-------------------------------------------------------------
		//	実行
		//-------------------------------------------------------------
		public void Do ( string filepath, Chara chara )
		{
			try
			{
				_Load ( filepath, chara );
			}
			catch ( ArgumentException e )
			{
				//仮データ
#if false
				TestChara testChara = new TestChara ();
				testChara.Test ( chara );
#endif
				TestCharaData tcd = new TestCharaData ();
				tcd.Make ( chara );

				//MessageBox.Show ( "LoadChara : 読込データが不適正です\n" + e.Message + "\n" + e.StackTrace );
				ErrMsg = "LoadChara : 読込データが不適正です\n" + e.Message + "\n" + e.StackTrace ;
			}

			ErrMsg = "Load OK.";
		}


		//対象ファイルを読み込み、Document形式で一時保存
		//その後、キャラデータを指定ドキュメントから変換
		private void _Load ( string filepath, Chara chara )
		{
			//ファイルが存在しないとき何もしない
			if ( ! File.Exists ( filepath ) )
			{
//				MessageBox.Show ( filepath + "が見つかりません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				STS_TXT.Trace_Err ( filepath + "が見つかりません" );
				throw new ArgumentException ( "ファイルが存在しませんでした。" );
			}

			//拡張子確認
			if ( Path.GetExtension ( filepath ).CompareTo ( ".dat" ) != 0 )
			{
				MessageBox.Show ( filepath + "は拡張子が.datと異なります。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				throw new ArgumentException ( "拡張子が.datと異なります。" );
			}

			//初期化
			chara.Clear ();

			//ファイルストリーム開始
			FileStream fstrm = new FileStream ( filepath, FileMode.Open, FileAccess.Read );
			BinaryReader biReaderFile = new BinaryReader ( fstrm, Encoding.ASCII );

//			Debug.WriteLine ( "fstrm.Length = " + fstrm.Length );

			//==========================================================================
			//スクリプト部分のメモリストリームを作成
			MemoryStream mstrmScript = new MemoryStream ();
//			StreamWriter strmWrtScript = new StreamWriter ( mstrmScript, Encoding.ASCII );
			StreamWriter strmWrtScript = new StreamWriter ( mstrmScript, Encoding.UTF8 );
			fstrm.Seek ( 0, SeekOrigin.Begin );

			//バージョンの取得
			uint version = biReaderFile.ReadUInt32 ();
			if ( IO_CONST.VER != version )
			{
				MessageBox.Show ( filepath + "はバージョンが " + version + "で" + IO_CONST.VER + " と異なります。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				throw new ArgumentException ( "バージョンが異なります。" );
			}

			//スクリプトサイズの取得
			uint sizeScript = biReaderFile.ReadUInt32 ();
			byte[] bufScript = new byte[ sizeScript ];
			biReaderFile.Read ( bufScript, 0, ( int ) sizeScript );
			mstrmScript.Write ( bufScript, 0, ( int ) sizeScript );
			mstrmScript.Seek ( 0, SeekOrigin.Begin );

			//ドキュメントの読込
			Document document = new Document ( mstrmScript );

//			strmWrtScript.Close ();
//			mstrmScript.Close ();
			
			//ドキュメント型からキャラへスクリプト部を変換
			DocToChara dtoc = new DocToChara ();
			dtoc.Load ( document, chara );


			//==========================================================================
			//イメージバイナリデータ読込
			LoadImageList ( biReaderFile, chara.behavior.BD_Image );
			//※ 読込位置はそのまま次のエフェクトイメージへ
			LoadImageList ( biReaderFile, chara.garnish.BD_Image );

			//==========================================================================
			//終了
			biReaderFile.Close ();
//			fstrm.Close ();
		}

		//イメージリストの読込
		private void LoadImageList ( BinaryReader br, BindingDictionary < ImageData > imagelist )
		{
			IEnumerable list =  imagelist.GetBindingList ();
			foreach ( ImageData imgdt in list )
			{
				//uint固定長でイメージサイズ読込
				uint imageSize = br.ReadUInt32 ();

				//実データ読込
				byte[] imageBuffer = new byte[ imageSize ];
				br.Read ( imageBuffer, 0, ( int ) imageSize );

				//メモリストリームに一時書出
				MemoryStream mstrmImage = new MemoryStream ();
				mstrmImage.Write ( imageBuffer, 0, ( int ) imageSize );

#if false
				MemoryStream mstrmImage = new MemoryStream ();
				int iRead = 0;
				int size = (int)imageSize;
				byte [] buf = new byte [ 1024 ];
				while ( iRead < size )
				{
					iRead += br.Read ( buf, 0, 1024 );
					mstrmImage.Write ( buf, 0, 1024 );
				}
				iRead = br.Read ( buf, 0, iRead - size );
				mstrmImage.Write ( buf, 0, iRead );
#endif


				//イメージ型の作成
				imgdt.Img = Image.FromStream ( mstrmImage );
				imgdt.MakeThumbnail ( imgdt.Img );

				mstrmImage.Dispose ();
			}
		}
	}

}

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;


namespace ScriptEditor
{
	using Utl = SaveCharaBin_Util;
	using BD_ImgDt = BindingDictionary < ImageData >;

	//==================================================================================
	//	SaveCharaImg
	//		キャラデータをbinaryで.datファイルに保存
	//		Imageを外部ファイルに持つ
	//		既存の.datにも変換する
	//==================================================================================
	public partial class SaveCharaImg
	{
		//-------------------------------------------------------------
		//	コンストラクタ
		//-------------------------------------------------------------
		public SaveCharaImg ()
		{
		}

		//-------------------------------------------------------------
		//	実行用
		//引数　filepath : 保存ファイルパス, chara : 保存対象キャラデータ
		//-------------------------------------------------------------
		public void DoIncludeImg ( string filepath, Chara chara )
		{
			try
			{
				_SaveIncludeImg ( filepath, chara );
			}
			catch ( ArgumentException e )
			{
				Debug.WriteLine ( e );
			}
		}
		public void DoWithoutImg ( string filepath, Chara chara )
		{
			try
			{
				_SaveScp ( filepath, chara );
			}
			catch ( ArgumentException e )
			{
				Debug.WriteLine ( e );
			}
		}


		//-------------------------------------------------------------
		private void _SaveIncludeImg ( string filepath, Chara chara )
		{ 
			//データが無かったら何もしない
			if ( chara == null ) { return; }

			//==================================================
			//スクリプト部の書出
			_SaveScp ( IOChara.GetScpPath ( filepath ), chara );

			//==================================================
			//イメージ部
			//一回、ファイルに書き出してから追加する
			_WriteListImage ( IOChara.GetBhvImgPath ( filepath ), chara.charaset.behavior.BD_Image );
			_WriteListImage ( IOChara.GetGnsImgPath ( filepath ), chara.charaset.garnish.BD_Image );

			//==================================================
			//最後にまとめて.datファイルに書出
			_SaveDatFile ( filepath );
		}


		//--------------------------------------------------------
		//スクリプト部のみの書出
		private void _SaveScp ( string scpFilePath, Chara chara )
		{ 
			//データが無かったら何もしない
			if ( chara == null ) { return; }


			//一時メモリストリーム
			using ( MemoryStream ms = new MemoryStream () )
			using ( BinaryWriter bw = new BinaryWriter ( ms, Encoding.UTF8 ) )
			{

			//--------------------------------------------------------
			//chara 各種データ書出
			Utl.SaveBinCompend ( bw, chara, chara.charaset.behavior );	//behavior
			Utl.SaveBinCompend ( bw, chara, chara.charaset.garnish);	//garnish
			Utl.SaveBinCommand ( bw, chara );	//Command
			Utl.SaveBinBranch ( bw, chara );	//Branch
			Utl.SaveBinRoute ( bw, chara );     //Route

			long script_size = ms.Length; 


			//--------------------------------------------------------
			bw.Flush ();
			//--------------------------------------------------------
			//一時ファイル書出
			using ( FileStream fs = new FileStream ( scpFilePath, FileMode.Create, FileAccess.Write ) )
			using ( BufferedStream bwFl = new BufferedStream( fs ) )
			{
				//バージョン(uint)
				byte[] byte_VER = BitConverter.GetBytes ( IO_CONST.VER );
				bwFl.Write ( byte_VER, 0, byte_VER.Length );

				//サイズ(uint)4,294,967,296[byte]まで
				uint mem_size = (uint)ms.Length;
				byte[] byte_Length = BitConverter.GetBytes ( mem_size );
				bwFl.Write ( byte_Length, 0, byte_Length.Length );

				//スクリプトを書いたMemoryStreamを先頭に戻してコピー
				ms.Seek ( 0, SeekOrigin.Begin );
				ms.CopyTo ( bwFl );

			}	//using FileStream

			}	//using MemoryStream

		}


		//イメージ部の書出
		private void _WriteListImage ( string path, BD_ImgDt bdImgdt )
		{
			using ( FileStream fStrm = new FileStream ( path, FileMode.Create ) )
			using ( BufferedStream bfStrm = new BufferedStream ( fStrm ) )
			{

			//イメージ個数
			uint uiCnt = (uint)bdImgdt.Count ();
			if ( uiCnt == 0 ) {  return; }	//個数が０のときは何もしない(0も記入しない)

			byte [] byteNum = BitConverter.GetBytes ( uiCnt );
			bfStrm.Write ( byteNum, 0, byteNum.Length );

			//実データ
			foreach ( ImageData? imgdt in bdImgdt.GetEnumerable () )
			{
				if ( imgdt is null ) { continue; }
				_WriteImage ( bfStrm, imgdt );
			}

			}	//using
		}


		//個別イメージデータから.png形式で書出
		private void _WriteImage ( BufferedStream bfStrm, ImageData imgdt )
		{
			using ( MemoryStream ms = new MemoryStream () )
			using ( BinaryWriter bw = new BinaryWriter ( ms, Encoding.UTF8 ) )
			{
				//名前
				bw.Write ( imgdt.Name );		//string (length , [UTF8])

				//イメージ作成と保存
				using ( MemoryStream msImg = new  MemoryStream () )
				{
				Image img = imgdt.GetImg ();
				img.Save ( msImg, ImageFormat.Png );

				//サイズ
				bw.Write ( (uint)msImg.Length );

				//msImg.CopyTo ( ms );
				//msImg.Flush ();

				bw.Write ( msImg.ToArray(), 0, (int)msImg.Length );
				}

				//書き込み
				ms.Flush ();
				ms.Seek ( 0, SeekOrigin.Begin );
				ms.CopyTo ( bfStrm );
			}
		}


		//.datファイルに書出
		private void _SaveDatFile ( string filepath )
		{ 
			//==================================================
			//最後にまとめて.datファイルに書出
			using ( FileStream fs = new FileStream ( filepath, FileMode.Create, FileAccess.Write ) )
			using ( BufferedStream bwFl = new BufferedStream( fs ) )
			{

			//バージョン(uint)
			byte[] byte_VER = BitConverter.GetBytes ( IO_CONST.VER );
			int ln_ver = byte_VER.Length;
			bwFl.Write ( byte_VER, 0, ln_ver );

			//サイズ合計 (uint)4,294,967,296[byte]まで
			// バージョン(4)とサイズ(4)をプラスして書き込んでいる
			uint mem_size = 4 + 4 + (uint) GetSizeSum ( filepath );
			byte[] byte_Length = BitConverter.GetBytes ( mem_size );
			int lnByLen = byte_Length.Length;
			bwFl.Write ( byte_Length, 0, lnByLen );

			//スクリプト部
			using ( FileStream fsScp = new FileStream ( IOChara.GetScpPath ( filepath ), FileMode.Open ) )
			{
				fsScp.CopyTo ( bwFl );
			}

			//イメージファイルから追加
			using ( FileStream fsBhv = new FileStream ( IOChara.GetBhvImgPath ( filepath ), FileMode.Open ) )
			{
				long lnBhv = fsBhv.Length;
				fsBhv.CopyTo ( bwFl );
			}
			using ( FileStream fsGns = new FileStream ( IOChara.GetGnsImgPath ( filepath ), FileMode.Open ) )
			{
				long lnGns = fsGns.Length;
				fsGns.CopyTo ( bwFl );
			}

			}	//using FileStream
			
		}

		private long GetSizeSum ( string filepath )
		{
			FileInfo fIScp = new FileInfo ( IOChara.GetScpPath ( filepath ) );
			FileInfo fI_bhv = new FileInfo ( IOChara.GetBhvImgPath ( filepath ) );
			FileInfo fI_gns = new FileInfo ( IOChara.GetGnsImgPath ( filepath ) );
			long ret = fIScp.Length + fI_bhv.Length + fI_gns.Length;
			return ret;
		}
	}
}

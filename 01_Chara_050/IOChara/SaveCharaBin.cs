using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Design;


namespace ScriptEditor
{
	using Utl = SaveCharaBin_Util;
	using BD_ImgDt = BindingDictionary < ImageData >;

	//==================================================================================
	//	SaveCharaBin
	//		キャラデータをbinaryで.datファイルに保存
	//		Document形式を介さない
	//==================================================================================
	public partial class SaveCharaBin
	{
		//エラーメッセージ
		public string ErrMsg { get; set; } = "Err Msg.";

		//-------------------------------------------------------------
		//	コンストラクタ
		//-------------------------------------------------------------
		public SaveCharaBin ()
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
				_Save ( filepath, chara );
			}
			catch ( ArgumentException e )
			{
				Debug.WriteLine ( e );
			}
		}

		private void _Save ( string filepath, Chara chara )
		{ 
			string dir = Directory.GetCurrentDirectory () ;
			Debug.WriteLine ( dir );

			//データが無かったら何もしない
			if ( chara == null ) { return; }

			//一時メモリストリーム
			using ( MemoryStream ms = new MemoryStream () )
			using ( BinaryWriter bw = new BinaryWriter ( ms, Encoding.UTF8 ) )
			{

			//--------------------------------------------------------
			//chara 各種データ書出
			Utl.SaveBinCompend ( bw, chara, chara.charaset.behavior );	//behavior
			Utl.SaveBinCompend ( bw, chara, chara.charaset.garnish );	//garnish
			Utl.SaveBinCommand ( bw, chara );	//Command
			Utl.SaveBinBranch ( bw, chara );	//Branch
			Utl.SaveBinRoute ( bw, chara );     //Route

			long script_size = ms.Length; 

			//--------------------------------------------------------


#if false
			//イメージ部
			WriteListImage ( bw, chara.behavior.BD_Image );
			WriteListImage ( bw, chara.garnish.BD_Image );
#endif
			//イメージ部
			//一回、imgファイルに書き出してから追加する
			string img_bhv_path = IOChara.GetBhvImgPath ( filepath );
			_WriteListImage ( img_bhv_path, chara.charaset.behavior.BD_Image );

			string img_gns_path = IOChara.GetGnsImgPath ( filepath );
			_WriteListImage ( img_gns_path, chara.charaset.garnish.BD_Image );
		

			//--------------------------------------------------------
			//スクリプト部書出
			bw.Flush ();
		
			//long ms_pos = ms.Position;
			
			using ( FileStream fs = new FileStream ( IOChara.GetScpPath( filepath ), FileMode.Create, FileAccess.Write ) )
			using ( BufferedStream bwFl = new BufferedStream( fs ) )
			{
				//バージョン(uint)
				byte[] byte_VER = BitConverter.GetBytes ( IO_CONST.VER );
				bwFl.Write ( byte_VER, 0, byte_VER.Length );

				//サイズ(uint)4,294,967,296[byte]まで
				FileInfo fI_bhv = new FileInfo ( img_bhv_path );
				FileInfo fI_gns = new FileInfo ( img_gns_path );
				uint mem_size = (uint) ( ms.Length + fI_bhv.Length + fI_gns.Length );
				byte[] byte_Length = BitConverter.GetBytes ( mem_size );
				bwFl.Write ( byte_Length, 0, byte_Length.Length );

				ms.Seek ( 0, SeekOrigin.Begin );
				ms.CopyTo ( bwFl );

			}	//using FileStream
			
			//ms.Seek ( ms_pos, SeekOrigin.Begin );
			
#if false

				//最後にファイルに書出
			using ( FileStream fs = new FileStream ( filepath, FileMode.Create, FileAccess.Write ) )
			using ( BinaryWriter bwFl = new BinaryWriter( fs ) )
			{

			//バージョン(uint)
			bwFl.Write ( IO_CONST.VER );

			//サイズ(uint)4,294,967,296[byte]まで
			bwFl.Write ( (uint) ms.Length );

			const int SIZE = 4096;	//バッファサイズ
			byte[] buf = new byte [ SIZE ];	//バッファ
			int numBytes = 0;

			ms.Seek ( 0, SeekOrigin.Begin );
			while ( ( numBytes = ms.Read ( buf, 0, SIZE ) ) > 0 )
			{
				bwFl.Write ( buf, 0, numBytes );
			}


			}	//using
#endif

			//最後にファイルに書出
			using ( FileStream fs = new FileStream ( filepath, FileMode.Create, FileAccess.Write ) )
			using ( BufferedStream bwFl = new BufferedStream( fs ) )
			{

			//バージョン(uint)
			byte[] byte_VER = BitConverter.GetBytes ( IO_CONST.VER );
			bwFl.Write ( byte_VER, 0, byte_VER.Length );

			//サイズ(uint)4,294,967,296[byte]まで
			FileInfo fI_bhv = new FileInfo ( img_bhv_path );
			FileInfo fI_gns = new FileInfo ( img_gns_path );
			uint mem_size = (uint) ( ms.Length + fI_bhv.Length + fI_gns.Length );
			byte[] byte_Length = BitConverter.GetBytes ( mem_size );
			bwFl.Write ( byte_Length, 0, byte_Length.Length );

			//メモリストリーム
			ms.Seek ( 0, SeekOrigin.Begin );
//			bwFl.Write ( ms.ToArray(), 0, (int)ms.Length );
			ms.CopyTo ( bwFl );


			//イメージファイルから追加
			using ( FileStream fsBhv = new FileStream ( img_bhv_path, FileMode.Open ) )
			{
				long lnBhv = fsBhv.Length;
				fsBhv.CopyTo ( bwFl );
			}
			using ( FileStream fsGns = new FileStream ( img_gns_path, FileMode.Open ) )
			{
				long lnGns = fsGns.Length;
				fsGns.CopyTo ( bwFl );
			}
#if false
#endif

			}	//using

			}	//using
			
		}


		private void _WriteListImage ( string path, BD_ImgDt bdImg )
		{
			using ( FileStream fStrm = new FileStream ( path, FileMode.Create ) )
			using ( BufferedStream bfStrm = new BufferedStream ( fStrm ) )
			{

			//イメージ個数
			uint uiCnt = (uint)bdImg.Count ();
			byte [] byteNum = BitConverter.GetBytes ( uiCnt );
			bfStrm.Write ( byteNum, 0, byteNum.Length );


				//実データ
				foreach (ImageData imgdt in bdImg.GetEnumerable ())
				{
#if false
				//イメージを一時領域に書出
				using ( MemoryStream msImg = new MemoryStream () )
				{			
				//名前
				bw.Write ( id.Name );		//string (length , [UTF8])

				//------------
				//@info 先にメモリに書き出してmsImg.Lengthにサイズを記録する
				//実データ
				id.Img.Save ( msImg, ImageFormat.Png );

				//サイズ
				bw.Write ( (uint)msImg.Length );

				msImg.Seek ( 0, SeekOrigin.Begin );
				while ( ( numBytes = msImg.Read ( buffer, 0, size ) ) > 0 )
				{ 
					bw.Write ( buffer, 0, numBytes );
				}

				}	//using
				bw.Flush ();	//一時書出

#endif

					_WriteImage ( bfStrm, imgdt );
				}
			}	//using
		}


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
				img.Dispose ();

				//サイズ
				bw.Write ( (uint)msImg.Length );

				//イメージ
				//msImg.CopyTo ( ms );
				bw.Write ( msImg.ToArray(), 0, (int)msImg.Length );
				}

				//書き込み
				ms.Seek ( 0, SeekOrigin.Begin );
				ms.CopyTo ( bfStrm );
			}
		}




		private void WriteListImage ( BinaryWriter bw, BD_ImgDt bdImg )
		{
			//イメージ個数
			bw.Write ( (uint)bdImg.Count() );


			//実データ
			foreach ( ImageData id in bdImg.GetEnumerable () )
			{
#if false
				//イメージを一時領域に書出
				using ( MemoryStream msImg = new MemoryStream () )
				{			
				//名前
				bw.Write ( id.Name );		//string (length , [UTF8])

				//------------
				//@info 先にメモリに書き出してmsImg.Lengthにサイズを記録する
				//実データ
				id.Img.Save ( msImg, ImageFormat.Png );

				//サイズ
				bw.Write ( (uint)msImg.Length );

				msImg.Seek ( 0, SeekOrigin.Begin );
				while ( ( numBytes = msImg.Read ( buffer, 0, size ) ) > 0 )
				{ 
					bw.Write ( buffer, 0, numBytes );
				}

				}	//using
				bw.Flush ();	//一時書出

#endif

				WriteImage ( bw, id );
			}
		}

		private void WriteImage ( BinaryWriter bw, ImageData id  )
		{
			//ストリーム読込→書出用
//			const int size = 4096;	//バッファサイズ
//			const int size = 16384;	//バッファサイズ
//			byte [] buffer = new byte [ size ];	//バッファ
			int numBytes = 0;	//書込バイト数

//			using ( BufferedStream msImg = new BufferedStream ( buffer ) )


			//イメージを一時領域に書出
			using ( MemoryStream msImg = new MemoryStream () )
			{			
			//名前
			bw.Write ( id.Name );		//string (length , [UTF8])

			//------------
			//@info 先にメモリに書き出してmsImg.Lengthにサイズを記録する
			//実データ
			id.Img.Save ( msImg, ImageFormat.Png );

			//サイズ
			bw.Write ( (uint)msImg.Length );

//			Debug.WriteLine ( (uint)msImg.Length );

			//バッファ
			byte [] buffer = new byte [ msImg.Length ];
			numBytes = msImg.Read ( buffer, 0, (int)msImg.Length );

			//書出
			bw.Write ( buffer, 0, buffer.Length );

#if false
			msImg.Seek ( 0, SeekOrigin.Begin );
			while ( ( numBytes = msImg.Read ( buffer, 0, size ) ) > 0 )
			{ 
				bw.Write ( buffer, 0, numBytes );
			}

#endif

			}	//using
		}
	}
}

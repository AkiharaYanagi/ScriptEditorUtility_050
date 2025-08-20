using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using ScriptEditor;


namespace ScriptEditor020
{
	using BD_Img = BindingDictionary < ImageData >;


	public partial class LoadCharaImg
	{
		//エラーメッセージ
		public string ErrMsg { get; set; } = "Err Msg.";

		//-------------------------------------------------------------
		//	コンストラクタ
		//-------------------------------------------------------------
		public LoadCharaImg ()
		{
		}

		//-------------------------------------------------------------
		//	実行
		//-------------------------------------------------------------
		public void Do_dir ( string filepath, Chara chara )
		{
			try
			{
				_Load_scp_dir ( filepath, chara );
			}
			catch ( ArgumentException e )
			{
				chara = new Chara ();		//空データ
				ErrMsg = "LoadChara : 読込データが不適正です\n" + e.Message + "\n" + e.StackTrace ;
			}
			ErrMsg = "Load OK.";
		}

		public void Do_img ( string filepath, Chara chara )
		{
			try
			{
				_Load_scp_img ( filepath, chara );
			}
			catch ( ArgumentException e )
			{
				chara = new Chara ();		//空データ
				ErrMsg = "LoadChara : 読込データが不適正です\n" + e.Message + "\n" + e.StackTrace ;
			}
			ErrMsg = "Load OK.";
		}


		//==================================================
		//対象ファイルを読み込みキャラデータをバイナリから変換

		//scp + dir
		private void _Load_scp_dir ( string filepath, Chara chara )
		{
			//ファイルが存在しないとき何もしない
			if ( ! File.Exists ( filepath ) )
			{
				STS_TXT.Trace_Err ( filepath + "が見つかりません" );
				throw new ArgumentException ( "ファイルが存在しませんでした。" );
			}

			//拡張子確認
			if ( Path.GetExtension ( filepath ).CompareTo ( ".scp" ) != 0 )
			{
				STS_TXT.Trace_Err ( "拡張子が.scpと異なります。" );
				throw new ArgumentException ( "拡張子が.scpと異なります。" );
			}

			//初期化
			chara.Clear ();


			//ファイルストリーム開始
			using ( var fstrm = new FileStream ( filepath, FileMode.Open, FileAccess.Read ) )
			using ( var br = new BinaryReader ( fstrm, Encoding.UTF8 ) )
			{
				//キャラデータ
				LoadBinBehavior ( br, chara );
				LoadBinGarnish ( br, chara );
				LoadBinCommand ( br, chara );
				LoadBinBranch ( br, chara );
				LoadBinRoute ( br, chara );
			}   //using


			//画像を別ディレクトリから取得
			LoadImageDir ( IOChara.GetBhvImgDir ( filepath ), chara.behavior.BD_Image );
			LoadImageDir ( IOChara.GetGnsImgDir ( filepath ), chara.garnish.BD_Image );
		}


		//scp + img
		private void _Load_scp_img ( string filepath, Chara chara )
		{
			//ファイルが存在しないとき何もしない
			if ( ! File.Exists ( filepath ) )
			{
				STS_TXT.Trace_Err ( filepath + "が見つかりません" );
				throw new ArgumentException ( "ファイルが存在しませんでした。" );
			}

			//拡張子確認
			if ( Path.GetExtension ( filepath ).CompareTo ( ".scp" ) != 0 )
			{
				STS_TXT.Trace_Err ( "拡張子が.scpと異なります。" );
				throw new ArgumentException ( "拡張子が.scpと異なります。" );
			}

			//初期化
			chara.Clear ();


			//ファイルストリーム開始
			using ( var fstrm = new FileStream ( filepath, FileMode.Open, FileAccess.Read ) )
			using ( var br = new BinaryReader ( fstrm, Encoding.UTF8 ) )
			{

				//バージョン(uint)
				uint ver = br.ReadUInt32 ();

				//サイズ(uint)
				uint size = br.ReadUInt32 ();

				//キャラデータ
				LoadBinBehavior ( br, chara );
				LoadBinGarnish ( br, chara );
				LoadBinCommand ( br, chara );
				LoadBinBranch ( br, chara );
				LoadBinRoute ( br, chara );
			}   //using


			//画像を別ファイルから取得して展開
			string imgfile_bhv = IOChara.GetBhvImgPath ( filepath );
			string imgdir_bhv = IOChara.GetBhvImgDir ( filepath );
			LoadImageFile ( imgfile_bhv, imgdir_bhv, chara.behavior.BD_Image );

			string imgfile_gns = IOChara.GetGnsImgPath ( filepath );
			string imgdir_gns = IOChara.GetGnsImgDir ( filepath );
			LoadImageFile ( imgfile_gns, imgdir_gns, chara.garnish.BD_Image );
		}



		public void LoadImageCompress ( string imgCmpFilename, BD_Img bd_img )
		{
		}


		public void LoadImageDir ( string imgDir, BD_Img bd_img )
		{
			//イメージディレクトリ
			//ファイル列挙
			string[] imgFilepathes = Directory.GetFiles ( imgDir );
			
			foreach ( string filepath in imgFilepathes )
			{
				//名前の取得
				string name = Path.GetFileName ( filepath );

				//img変換
				Image img = Image.FromFile ( filepath );

				//サムネイルを作成
				Bitmap imgThum = new Bitmap ( img, 50, 50 );

				//イメージデータ作成
				ImageData imgdt = new ImageData ( name );
				imgdt.Thumbnail = imgThum;
				imgdt.Path = filepath;
				bd_img.Add ( imgdt );
			}
		}


		//イメージ読込
		public void LoadImageFile ( string filepath, string imgDir, BD_Img bd_img )
		{
			//==============================
			//◆ ver0.21 
			//Imageを外部ファイルに書出
			//必要時にディスクから読み出す
			//windowsディレクトリ "img\\cahra_'name'_img\\img000.png"
			//==============================

			//-------------------------------------------------------
			//ディレクトリ作成
			Directory.CreateDirectory ( imgDir );
			//既存を全削除
			WinUtility.DeleteAllFile ( imgDir );
			//-------------------------------------------------------

			using ( FileStream fs = new FileStream ( filepath, FileMode.Open, FileAccess.Read ) )
			using ( BinaryReader br = new BinaryReader ( fs ) )
			{

			//イメージ個数
			uint n = br.ReadUInt32 ();

			//各イメージ
			for ( uint ui = 0; ui < n; ++ ui )
			{
				//名前 [utf-8] ( byte 名前のサイズ, 実データ )
				string name = br.ReadString ();

				//サイズ( uint -> int ) ( br.ReadBytes(size) のためint )
				uint usize = br.ReadUInt32 ();
				int size = (int)usize; 

				//一時領域
				byte[] buffer = new byte [ size ];

				//読込
				buffer = br.ReadBytes ( size );

				//対象画像ファイルのパス
				string imgName = imgDir + "\\" + name;


				//イメージの作成とディレクトリに展開
				using ( MemoryStream ms = new MemoryStream ( buffer ) )
				{
				//img変換
				Image img = Image.FromStream ( ms );
				img.Save ( imgName, ImageFormat.Png );

				//サムネイルを作成
				Bitmap imgThum = new Bitmap ( img, 50, 50 );

				//イメージデータ作成
				ImageData imgdt = new ImageData ( name );
				imgdt.Path = imgName;
				imgdt.Thumbnail = imgThum;
				bd_img.Add ( imgdt );
				}	//using

		}

			}	//using
		}

	}
}

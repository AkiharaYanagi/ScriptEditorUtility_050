using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using ScriptEditorUtility;


namespace Chara020
{
	using BD_Img = BindingDictionary < ImageData >;


	public partial class LoadCharaBin
	{
		//エラーメッセージ
		public string ErrMsg { get; set; } = "Err Msg.";

		//-------------------------------------------------------------
		//	コンストラクタ
		//-------------------------------------------------------------
		public LoadCharaBin ()
		{
		}

		//-------------------------------------------------------------
		//	実行
		//-------------------------------------------------------------
		public void Do ( string filepath, Chara chara )
		{
			STS_TXT.Trace ( "読込開始" );

			try
			{
				//_Load ( filepath, chara );
				_Load_without_Image ( filepath, chara );
			}
			catch ( ArgumentException e )
			{
#if false
				//仮データ
				TestChara testChara = new TestChara ();
				testChara.Test ( chara );
#endif
				//空データ
				chara = new Chara ();

				//MessageBox.Show ( "LoadChara : 読込データが不適正です\n" + e.Message + "\n" + e.StackTrace );
				ErrMsg = "LoadChara : 読込データが不適正です\n" + e.Message + "\n" + e.StackTrace ;
			}

			STS_TXT.Trace ( "◆◆ 読込完了" );

			ErrMsg = "Load OK.";
		}


		//対象ファイルを読み込みキャラデータをバイナリから変換
		private void _Load ( string filepath, Chara chara )
		{
			//ファイルが存在しないとき何もしない
			if ( ! File.Exists ( filepath ) )
			{
				//MessageBox.Show ( filepath + "が見つかりません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				STS_TXT.Trace_Err ( filepath + "が見つかりません" );
				throw new ArgumentException ( "ファイルが存在しませんでした。" );
			}

			//拡張子確認
			if ( Path.GetExtension ( filepath ).CompareTo ( ".dat" ) != 0 )
			{
				//MessageBox.Show ( filepath + "は拡張子が.datと異なります。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				throw new ArgumentException ( "拡張子が.datと異なります。" );
			}

			//初期化
			chara.Clear ();


			//ファイルストリーム開始
			using ( var fstrm = new FileStream ( filepath, FileMode.Open, FileAccess.Read ) )
			using ( var br = new BinaryReader ( fstrm, Encoding.UTF8 ) )
			{
//				Debug.WriteLine ( "fstrm.Length = " + fstrm.Length );

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

				//ビヘイビア
				string imgfile_bhv = IOChara.GetBhvImgPath ( filepath );
				string imgdir_bhv = IOChara.GetBhvImgDir ( filepath );
				LoadImage ( imgfile_bhv, imgdir_bhv, br, chara.behavior.BD_Image );		
				//ガーニッシュ
				string imgfile_gns = IOChara.GetGnsImgPath ( filepath );
				string imgdir_gns = IOChara.GetGnsImgDir ( filepath );
				LoadImage ( imgfile_gns, imgdir_gns, br, chara.garnish.BD_Image );
			}	//using
		
		}



		public void LoadImage ( string filepath, string imgDir, BinaryReader br, BD_Img bd_img )
		{
			//画像はディレクトリに展開しておく

			//EOF
			if ( br.BaseStream.Position >= br.BaseStream.Length ) { return; }

			//イメージ個数
			uint n = br.ReadUInt32 ();
			if ( n == 0 ) { return; }

			//ディレクトリ名(無いときに作成)
			Directory.CreateDirectory ( imgDir );


			//各イメージ
			for ( uint ui = 0; ui < n; ++ ui )
			{
				//名前 [utf-8] ( byte 名前のサイズ, 実データ )
				string name = br.ReadString ();

				//サイズ( uint -> int ) ( br.ReadBytes(size) のためint )
				uint usize = br.ReadUInt32 ();
				//int size = (int)br.ReadUInt32 ();
				int size = (int)usize; 

				//一時領域読込
				byte[] buffer = new byte [ size ];
				buffer = br.ReadBytes ( size );


				using ( MemoryStream ms = new MemoryStream ( buffer ) )
				{

				//img変換
				string img_path = imgDir + "\\" + name;
				Image img = Image.FromStream ( ms );
				img.Save ( img_path, ImageFormat.Png );

				//コンストラクタで引数からサムネイルを作成
				Bitmap imgThum = new Bitmap ( img, 50, 50 );
				img.Dispose ();

				//イメージデータ作成
				ImageData imgdt = new ImageData ( name );
				imgdt.Thumbnail = imgThum;
				imgdt.Path = img_path;
				bd_img.Add ( imgdt );

				}	//using


#if false
				using (MemoryStream ms = new MemoryStream ( buffer ))
				{
				Image img = Image.FromStream ( ms );


					//イメージデータ作成
					//コンストラクタで引数からサムネイルを作成
					ImageData imgdt = new ImageData ( name, img );
				bd_img.Add ( imgdt );
				
				}	//using
#endif
			}
		}


#if false
		//イメージ名の再指定
		public void Respecift_ImageName ( Chara chara )
		{
			BD_Img bdImgBhv = chara.behavior.BD_Image;
			BD_Img bdImgGns = chara.garnish.BD_Image;

			//アクションにおけるイメージ名の再指定
			foreach ( Action act in chara.behavior.BD_Sequence.GetEnumerable () )
			{
				foreach ( Script scp in act.ListScript )
				{
					//メインイメージ名
					int id = GetIndex ( scp.ImgName, "Img_" );
					scp.ImgName = bdImgBhv [ id ].Name;
				}
			}

			//エフェクトにおけるイメージ名の再指定
			foreach ( Effect efc in chara.garnish.BD_Sequence.GetEnumerable () )
			{
				foreach ( Script scp in efc.ListScript )
				{
					//エフェクトイメージ名
					int id = GetIndex ( scp.ImgName, "Img_" );
					scp.ImgName = bdImgGns [ id ].Name;
				}
			}

		}

#endif





		//Test Imageを除く
		//対象ファイルを読み込みキャラデータをバイナリから変換
		private void _Load_without_Image ( string filepath, Chara chara )
		{
			//ファイルが存在しないとき何もしない
			if ( !File.Exists ( filepath ) )
			{
				//MessageBox.Show ( filepath + "が見つかりません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				STS_TXT.Trace_Err ( filepath + "が見つかりません" );
				throw new ArgumentException ( "ファイルが存在しませんでした。" );
			}

			//拡張子確認
			if ( Path.GetExtension ( filepath ).CompareTo ( ".dat" ) != 0 )
			{
				//MessageBox.Show ( filepath + "は拡張子が.datと異なります。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				throw new ArgumentException ( "拡張子が.datと異なります。" );
			}

			//初期化
			chara.Clear ();


			//ファイルストリーム開始
			using ( var fstrm = new FileStream ( filepath, FileMode.Open, FileAccess.Read ) )
			using ( var br = new BinaryReader ( fstrm, Encoding.UTF8 ) )
			{
				//				Debug.WriteLine ( "fstrm.Length = " + fstrm.Length );

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

#if false
				//ビヘイビア
				string imgfile_bhv = IOChara.GetBhvImgPath ( filepath );
				string imgdir_bhv = IOChara.GetBhvImgDir ( filepath );
				LoadImage ( imgfile_bhv, imgdir_bhv, br, chara.behavior.BD_Image );
				//ガーニッシュ
				string imgfile_gns = IOChara.GetGnsImgPath ( filepath );
				string imgdir_gns = IOChara.GetGnsImgDir ( filepath );
				LoadImage ( imgfile_gns, imgdir_gns, br, chara.garnish.BD_Image );
#endif
			}   //using

		}



	}
}

using System.Drawing;
using ScriptEditor;


namespace ScriptEditor020
{
	//イメージと名前を持つ
	// INameを継承し、BindingDictionaryで扱う
	public class ImageData : IName
	{
		public const int THUM_W = 50;
		public const int THUM_H = 50;


		//イメージ
		public Image Img { set; get; } = null;


		//イメージディレクトリのフルパスを保存し、描画に必要なときのみ読み込む
		public string Path { set; get; } = null;	//"pass\\img.png";

		//イメージファイル(.img)と名前から読込
		public string Img_file { set; get; } = null;	//"file.img"



		//表示用サムネイル
		public Bitmap Thumbnail { set; get; } = new Bitmap ( THUM_W, THUM_H );

		//名前
		public string Name { set; get; }

		public ImageData ()
		{
			Graphics g = Graphics.FromImage ( Thumbnail );
			g.FillRectangle ( Brushes.Red, g.VisibleClipBounds );
			g.Dispose ();
		}

		//引数付コンストラクタ
		public ImageData ( string name, Image img )
		{
			Name = name;
			Img = img;
			MakeThumbnail ( img );
		}
		public ImageData ( string name )
		{
			Name = name;

			//仮■で埋め
			Bitmap imgBmp = new Bitmap ( 10, 10 );
			Graphics gBmp = Graphics.FromImage ( imgBmp );
			gBmp.FillRectangle ( Brushes.LightYellow, new Rectangle ( 0, 0, imgBmp.Width, imgBmp.Height ) );
			gBmp.Dispose ();

			Img = imgBmp;
			Thumbnail = imgBmp;
		}
		public ImageData ( ImageData imageData )
		{
			this.Img = imageData.Img;
			this.Name = imageData.Name;

			MakeThumbnail ( this.Img );
		}

		//文字列変換
		public override string ToString ()
		{
			return Name;
		}

		public void MakeThumbnail ( Image img )
		{
			if ( img is null ) { return; }

			Graphics g = Graphics.FromImage ( Thumbnail );
			g.DrawImage ( img, new Rectangle ( 0, 0 , THUM_W, THUM_H ) );
			g.Dispose ();
		}

		//Image取得
		public Image GetImg ()
		{
			//ディレクトリ
			if ( Path != null )
			{
				return Image.FromFile ( Path ); 
			}

			//無い(初期状態■かサムネイル)
			return Img;
		}
	}
	

}

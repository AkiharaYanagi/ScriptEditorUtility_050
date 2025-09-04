using System.ComponentModel;
using ScriptEditorUtility;


namespace Chara020
{
	//イメージを名前で検索する
	// 検索用ディクショナリと表示用バインディングリストを持つ
	public class ImageList
	{
		private BindingList < ImageData > BL_ImgDt = new BindingList < ImageData > ();
		private Dictionary < string, ImageData > DCT_ImgDt = new Dictionary < string, ImageData > ();

		public void Add ( string name, ImageData imgDt )
		{
			DCT_ImgDt.Add ( name, imgDt );
			BL_ImgDt.Add ( imgDt );
		}

		public ImageData? Get ( string name )
		{
			return DCT_ImgDt.TryGetValue ( name, out ImageData? imgdt ) ? imgdt : null;
		}

		public void Remove ( string name )
		{
			BL_ImgDt.Remove ( DCT_ImgDt [ name ] );
			DCT_ImgDt.Remove ( name );
		}

		public void Clear ()
		{
			BL_ImgDt.Clear ();
			DCT_ImgDt.Clear ();
		}

		public BindingList < ImageData > GetBindingList ()
		{
			return BL_ImgDt;
		}
	}
	

}

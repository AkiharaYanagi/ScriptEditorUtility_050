

namespace ScriptEditorUtility
{
	//----------------------------------------------------------
	//	フォームの基本情報を保存するファイル
	//----------------------------------------------------------
	//	保存読込はXML_IOを用いる
	//	フルパスを保存
	public class Settings_ctrl
	{
		//		public string SettingFilepath { set; get; } = "setting.xml";	//自身のファイルパス

		public string LastDirectory { set; get; } = "";     //前回のディレクトリ
		public string LastFilepath { set; get; } = "";      //前回(今回の最新)のファイルパス


		//--------------------------------------------------------------
		public string File_ActionList { get; set; } = "ActionList.txt";     //アクションリスト
		public string Dir_ImageListAct { get; set; } = "Image";             //イメージディレクトリ(アクション)
		public string File_EffectList { get; set; } = "EffectList.txt";     //エフェクトリスト
		public string Dir_ImageListEf { get; set; } = "EfImage";            //イメージディレクトリ(エフェクト) 
		public string File_CommandList { get; set; } = "CommandList.txt";   //コマンドリスト
		public string File_BranchList { get; set; } = "BranchList.txt";     //ブランチリスト
		public string File_RouteList { get; set; } = "RouteList.txt";       //ルートリスト

		//--------------------------------------------------------------

		public Settings_ctrl ()
		{
			//コンストラクタ時に決定
			if ( LastDirectory == "" )
			{
				LastDirectory = System.Environment.CurrentDirectory;
			}

			if ( LastFilepath == "" )
			{
				LastFilepath = System.Environment.CurrentDirectory + "\\chara.dat";
			}
		}


	}
}

using System;
using System.IO;
using System.Xml.Serialization;


namespace ScriptEditorUtility
{
	//-------------------------------------------------------
	//	汎用クラスTに対応するXML入出力
	//-------------------------------------------------------
	public static class XML_IO
	{
		public static string SettingFilepath { set; get; } = "setting.xml";	//保存ファイルパス
		
		//============================================
		//◆ 保存
		//============================================
		public static void Save ( object ob )
		{
			//クラスをXMLファイルに保存
			XmlSerializer serializer = new XmlSerializer ( ob.GetType () );

			//書出
			using ( FileStream fs = new FileStream ( SettingFilepath, FileMode.Create ) )
			{
				serializer.Serialize ( fs, ob );
			}
		}

		public static void Save ( string filename, object ob )
		{
			SettingFilepath = filename;
			Save ( ob );
		}

		//============================================
		//◆ 読込
		//
		// T t = (T)XML_IO.Load ( typeof ( T ) );
		//
		//============================================
		public static object? Load ( Type T )
		{
			//---------------------------------------------------------
			//ファイルの存在しないとき
			if ( ! File.Exists ( SettingFilepath ) )
			{
				//作成して保存して返す
				var t = Activator.CreateInstance ( T );
				if ( t is null ) { return t; }

				Save ( t );
				return t; 
			}

			//---------------------------------------------------------
			//ファイルが存在するとき

			//読込
			//XMLファイルの設定
			XmlSerializer serializer = new XmlSerializer ( T );

			using ( FileStream fs = new FileStream ( SettingFilepath, FileMode.Open ) )
			{
				return serializer.Deserialize ( fs );
			}
		}

		public static object? Load ( object ob )
		{
			return Load ( ob.GetType () );
		}

		public static object? Load ( string filename, object ob )
		{
			SettingFilepath = filename;
			return Load ( ob );
		}
	}
}

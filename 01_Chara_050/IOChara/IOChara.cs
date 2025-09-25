using System.Drawing;
using System.Diagnostics;
using System.IO;


namespace Chara050
{
	//==================================================================================
	//
	//	書出読込共通の要素を定義する
	//
	//==================================================================================
	
	//@info 自由個数設定の値はbyteではなく、uintにする


	//*********************************
	//Scriptに要素を追加するとき
	//・Scriptクラス
	//・IOChara
	//		CharaToDoc, DocToChara,
	//		CharaToText, TextToChara,
	//		SaveChara, LoadChara
	//		SaveCharaBin, LoadCharaBin
	//・TestChara
	//
	//・旧データを変換
	//*********************************


	public static class IO_CONST
	{
		public const uint VER = 110;
	}

	//配列添字取得用
	public static class IOChara 
	{
#if false
		public static int AttrToInt ( Element e, int attr )
		{
			int i = 0;
			try
			{
				i = int.Parse ( e.Attributes[ attr ].Value );
			}
			catch ( System.Exception exc )
			{
				Debug.Write ( exc.ToString () );
			}
			return i;
		}

		public static Point AttrToPoint ( Element e, int enumName0, int enumName1 )
		{
			return new Point ( AttrToInt ( e, enumName0 ), AttrToInt ( e, enumName1 ) );
		}
#endif

		//===================================================

		//イメージ別ディレクトリ名
		public static string GetBhvImgDir ( string filepath )
		{
			string? fileDir = Path.GetDirectoryName ( filepath );
			string? filename = Path.GetFileNameWithoutExtension ( filepath );
			return fileDir + "\\img\\" + filename + "_bhv_img";
		}
		public static string GetGnsImgDir ( string filepath )
		{
			string? fileDir = Path.GetDirectoryName ( filepath );
			string? filename = Path.GetFileNameWithoutExtension ( filepath );
			return fileDir + "\\img\\" + filename + "_gns_img";
		}

		//イメージ別ファイル名
		public static string GetScpPath ( string filepath )
		{
			string? fileDir = Path.GetDirectoryName ( filepath );
			string? filename = Path.GetFileNameWithoutExtension ( filepath );
//			return fileDir + "\\img\\" + filename + ".scp";
			return fileDir + "\\" + filename + ".scp";
		}

		public static string GetBhvImgPath ( string filepath )
		{
			string? fileDir = Path.GetDirectoryName ( filepath );
			string? filename = Path.GetFileNameWithoutExtension ( filepath );
			return fileDir + "\\" + filename + "_bhv" + ".img";
		}

		public static string GetGnsImgPath ( string filepath )
		{
			string? fileDir = Path.GetDirectoryName ( filepath );
			string? filename = Path.GetFileNameWithoutExtension ( filepath );
			return fileDir + "\\" + filename + "_gns" + ".img";
		}
	}


	//共通アトリビュート
	public enum ATTR_
	{
		NAME_0 = 0,	
	}

	public enum ELEMENT_CHARA
	{
		VER,
		MAIN_IMAGE_LIST,
		EF_IMAGE_LIST,
		ACTION_LIST,
		EF_LIST,
		COMMAND_LIST,
		BRANCH_LIST,
		ROUTE_LIST,
	}

	public enum ATTR_ACTION
	{
		ELAC_NAME,
		ELAC_NEXT_NAME,
		ELAC_NEXT_ID,
		ELAC_CATEGORY,
		ELAC_POSTURE,
		ELAC_HITNUM,
		ELAC_HIT_PITCH,
		ELAC_BALANCE,
	}

#if false
	public enum ATTR_SCP
	{
		GROUP,
		IMG_NAME,
		IMG_ID,
		X, Y, 
		CLC_ST, 
		VX, VY,
		AX, AY,
		POWER,
		WARP,
		RECOIL_I, RECOIL_E,
		BALANCE_I, BALANCE_E,
		BLACKOUT,
		VIBRATION,
		STOP,
		ROTATE,
		ROTATE_X, ROTATE_Y,
		AFTERIMAGE_PITCH,
		AFTERIMAGE_N,
		AFTERIMAGE_TIME,
		VIBRATION_S,
		COLOR,
		COLOR_TIME,
	}
#endif

	public enum ATTR_SCP
	{
		//Frameは構造的に取得
		GROUP,
		IMG_NAME,
		IMG_ID,		//[C++] GameMain側のLoadCharaにおいてIDから読み込む
		X, Y, 
	};

	public enum ATTR_SCP_BTL
	{
		CLC_ST, 
		VX, VY,
		AX, AY,
		POWER,
		WARP,
		RECOIL_I, RECOIL_E,
		BALANCE_I, BALANCE_E,
	};

	public enum ATTR_SCP_STG
	{
		BLACKOUT,
		VIBRATION,
		STOP,
		ROTATE,
		ROTATE_X, ROTATE_Y,
		AFTERIMAGE_N,
		AFTERIMAGE_TIME,
		AFTERIMAGE_PITCH,
		VIBRATION_S,
		COLOR,
		COLOR_TIME,
		SCALING_X, SCALING_Y,
		SE,
	}

	public enum ELEMENT_SCRIPT
	{
		ELSC_ROUTE,
		ELSC_EFGNRT,
		ELSC_CRECT,
		ELSC_ARECT,
		ELSC_HRECT,
		ELSC_ORECT,
	}

	public enum ATTRIBUTE_COMMAND
	{
		NAME,
		LIMIT_TIME,
		ID_LVR,
	}

	public enum ELEMENT_BRANCH
	{
		ELBR_COMMAND_NAME,
		ELBR_COMMAND_ID,
		ELBR_ACTION_NAME,
		ELBR_ACTION_ID,
		ELBR_FRAME,
	}

	public enum ATTR_BRANCH
	{
		NAME,
		CONDITION,
		CMD_NAME,
		CMD_ID,
		SQC_NAME,
		SQC_ID,
		FRAME,
		OTHER,
	}

	public enum ATTR_ROUTE
	{
		NAME,
		SUMMARY,
	}

	public enum ELMT_EFGNRT
	{
		ELEG_NAME,
		ELEG_EFNAME,
		ELEG_PT_X,
		ELEG_PT_Y,
		ELEG_PT_Z,
		ELEG_GNRT,
		ELEG_LOOP,
		ELEG_SYNC,
	}

}

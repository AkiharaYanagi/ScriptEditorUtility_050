

namespace Chara020
{
	//================================================================
	//	◆アクション		各フレームのスクリプトリストを持つ
	//		base	┣[]スクリプト
	//		┣次アクション
	//		┣アクション属性
	//		┣アクション体勢
	//
	//		┣ヒット数
	//		┣ヒット間隔
	//		┣バランス値
	//		┣マナ値
	//		┣アクセル値
	//		┣汎用パラメータ[16]
	//================================================================

	//-------------------------------------------------------
	//アクション
	public class Action : Sequence 
	{
		//デフォルトアクション名
		public const string ActionName = "New_Action";

		//次アクション名
		//(空オブジェクト時にも指定しないと名前チェックエラーになる)
		public string NextActionName { get; set; } = "立ち";

		//アクション属性
		public ActionCategory Category { get; set; } = ActionCategory.NEUTRAL;

		//アクション体勢
		public ActionPosture Posture { get; set; } = ActionPosture.STAND;

		//ヒット数
		public int HitNum { get; set; } = 1;

		//ヒット間隔[F}
		public int HitPitch { get; set; } = 10;

		//増減バランス値
		public int Balance { get; set; } = 0;

		//増減マナ値
		public int Mana { get; set; } = 0;

		//増減アクセル値
		public int Accel { get; set; } = 0;

		//汎用フラグ
		public const int VRS_SIZE = 16;
//		public int[] Versatile { get; set; } = new int [ VRS_SIZE ];
		public int[] Versatile { get; set; } = Enumerable.Range ( 0, VRS_SIZE ).ToArray ();


		//----------------------------------------------------------------------------
		//コンストラクタ
		public Action ()	//ロード時に空白が必要
		{
			//IName
			this.Name = ActionName;
			for ( int i = 0; i < VRS_SIZE; ++ i ) { Versatile [ i ] = 0; }
		}

		//引数付きコンストラクタ
		public Action ( string str ) : base ( str )
		{
			//@info baseのコンストラクタの後でbase.Clear()が呼ばれてしまうのでClear()を用いない
		
			//IName
			this.Name = str;
			NextActionName = str;
			for ( int i = 0; i < VRS_SIZE; ++ i ) { Versatile [ i ] = 0; }
		}

		//継承元から生成するコンストラクタ
		public Action ( Sequence sqc )
		{
			base.Copy ( sqc );
		}

		//コピーコンストラクタ
		public Action ( Action action )
		{
			base.Copy ( action );

			this.NextActionName = action.NextActionName;
			this.Category = action.Category;
			this.Posture = action.Posture;
			this.HitNum = action.HitNum;
			this.HitPitch = action.HitPitch;
			this.Balance = action.Balance;
			this.Mana = action.Mana;
			this.Accel = action.Accel;
			for ( int i = 0; i < VRS_SIZE; ++ i ) { Versatile [ i ] = action.Versatile [ i ]; }
	}

		//クリア
		public override void Clear ()
		{
			NextActionName = "Next";
			Category = ActionCategory.NEUTRAL;
			Posture = ActionPosture.STAND;
			HitNum = 0;
			HitPitch = 0;
			Balance = 0;
			Mana = 0;
			Accel = 0;
			for ( int i = 0; i < VRS_SIZE; ++ i ) { Versatile [ i ] = 0; }

			base.Clear ();
		}

		//コピー
		public void CopyAction ( Action action )
		{
			base.Copy ( action );

			this.NextActionName = action.NextActionName;
			this.Category = action.Category;
			this.Posture = action.Posture;
			this.HitNum = action.HitNum;
			this.HitPitch = action.HitPitch;
			this.Balance = action.Balance;
			this.Mana = action.Mana;
			this.Accel = action.Accel;
			for ( int i = 0; i < VRS_SIZE; ++ i ) { Versatile [ i ] = action.Versatile [ i ]; }
		}
	}

}

using System.Drawing;


namespace ScriptEditor
{
	using BD_Tn = BindingDictionary < TName >;
	using BD_EfGn = BindingDictionary < EffectGenerate >;
	using LsRect = List < Rectangle >;

	//================================================================
	//	◆スクリプト		キャラにおけるアクションの１フレームの値
	//		┣フレーム数
	//		┣(グループ) ScriptEditor内での編集用
	//		┣イメージID (Name)
	//		┣画像表示位置	┣速度	┣加速度
	//		┣計算状態(持続/代入/加算)
	//		┣[]ルート
	//		┣[]接触枠	┣[]攻撃枠	┣[]防御枠	┣[]相殺枠
	//		┣[]エフェクト発生
	//		┣パラメータ(戦闘)
	//		┣パラメータ(演出)
	//================================================================

	//スクリプト項目に追加するとき以下を更新
	// class Script
	//		変数に追加
	//		コピー類に値を追加
	// IOChara (ScriptEditor)
	// IOChara (GameMain)
	// GameMain

	[Serializable]
	public class Frame
	{
		//--------------------------------------------------------------------
		//	ID
		//--------------------------------------------------------------------
		//シークエンス内スクリプトリストにおける自身のフレーム数(番目)
		public int N { get; set; } = 0;

		//編集用グループ値 (0はグループ無し、１からグループ生成)
		public int Group { get; set; } = 0;

		//--------------------------------------------------------------------
		//表示
		//--------------------------------------------------------------------
		//表示画像 名前指定
		public string ImgName { get; set; } = "ImgName.png";

		//--------------------------------------------------------------------
		//位置
		//--------------------------------------------------------------------
		//(基準位置からの画像表示補正位置)
		public Point Pos { get; set; } = new Point ( -250, -500 );
		public void SetPos ( int x, int y ) { Pos = new Point ( x, y ); }
		public void SetPosX ( int x ) { Pos = new Point ( x, Pos.Y ); }
		public void SetPosY ( int y ) { Pos = new Point ( Pos.X, y ); }

		//------------------------------------------------
		//ルートネームリスト (スクリプト分岐)
		//------------------------------------------------
		public BD_Tn BD_RutName = new BD_Tn ();

		//------------------------------------------------
		//枠
		//------------------------------------------------
		//接触枠(Collision), 攻撃枠(Attack), 当り枠(Hit), 相殺枠(Offset)
		//ScriptEditor と ゲームメイン では上限は定数(ConshChara.NumRect = 8)
		public LsRect ListCRect { get; set; } = new LsRect ();
		public LsRect ListHRect { get; set; } = new LsRect ();
		public LsRect ListARect { get; set; } = new LsRect ();
		public LsRect ListORect { get; set; } = new LsRect ();

		//------------------------------------------------
		//エフェクト生成
		//------------------------------------------------
		public BD_EfGn BD_EfGnrt { get; set; } = new BD_EfGn ();

		//------------------------------------------------
		//　各パラメータ
		//------------------------------------------------
		//バトルパラメータ
		public FrameParam_Battle BtlPrm = new FrameParam_Battle();
		//演出パラメータ
		public FrameParam_Staging StgPrm = new FrameParam_Staging();

		//汎用パラメータ
		const int VRS_SIZE = 16;
//		public Int32[] Versatile { get; set; } = new Int32 [ VRS_SIZE ];
		public int[] Versatile { get; set; } = Enumerable.Range ( 0, VRS_SIZE ).ToArray ();


		//================================================================
		//コンストラクタ
		public Frame ()
		{
		}

		//コピーコンストラクタ
		public Frame ( Frame s )
		{
            N = s.N;
            Group = s.Group;
            ImgName = s.ImgName;
            Pos = s.Pos;

//			this.BD_RutName = new BindingDictionary<TName> ();
			BD_RutName.DeepCopy ( s.BD_RutName );

            ListCRect = new LsRect( s.ListCRect );
            ListHRect = new LsRect( s.ListHRect );
            ListARect = new LsRect( s.ListARect );
            ListORect = new LsRect( s.ListORect );
//			this.BD_EfGnrt = new BD_EfGn ( s.BD_EfGnrt );
			BD_EfGnrt.DeepCopy ( s.BD_EfGnrt );

            BtlPrm = new FrameParam_Battle ( s.BtlPrm );
            StgPrm = new FrameParam_Staging( s.StgPrm );

            Versatile = (int[]) s.Versatile.Clone ();	//値型なのでシャローコピー
		}

		//初期化
		public void Clear ()
		{
			N = 0;
			Group = 0;
			ImgName = "Clear";
			Pos = new Point ();

			BD_RutName.Clear ();
			ListCRect.Clear ();
			ListARect.Clear ();
			ListHRect.Clear ();
			ListORect.Clear ();

			BD_EfGnrt.Clear ();

			BtlPrm.Clear ();
			StgPrm.Clear ();

			Versatile = Enumerable.Range ( 0, 16 ).ToArray ();
		}

		//コピー
		public void Copy ( Frame s )
		{
            N = s.N;

            Group = s.Group;
            ImgName = s.ImgName;
            Pos = s.Pos;
            BD_RutName.DeepCopy ( s.BD_RutName );
            ListCRect = new LsRect( s.ListCRect );
            ListHRect = new LsRect( s.ListHRect );
            ListARect = new LsRect( s.ListARect );
            ListORect = new LsRect( s.ListORect );
//			this.BD_EfGnrt = new BD_EfGn ( s.BD_EfGnrt );
			BD_EfGnrt.DeepCopy ( s.BD_EfGnrt );

            BtlPrm.Copy ( s.BtlPrm );
            StgPrm.Copy ( s.StgPrm );

            Versatile = (int[]) s.Versatile.Clone ();	//値型なのでシャローコピー
		}

		//コピー(フレーム数以外)
		public void Copy_Other_than_frame ( Frame s )
		{
            //this.Frame = s.Frame;

            Group = s.Group;
            ImgName = s.ImgName;
            Pos = s.Pos;
            BD_RutName.DeepCopy ( s.BD_RutName );
            ListCRect = new LsRect( s.ListCRect );
            ListHRect = new LsRect( s.ListHRect );
            ListARect = new LsRect( s.ListARect );
            ListORect = new LsRect( s.ListORect );
//			this.BD_EfGnrt = new BD_EfGn ( s.BD_EfGnrt );
			BD_EfGnrt.DeepCopy ( s.BD_EfGnrt );

            BtlPrm.Copy ( s.BtlPrm );
            StgPrm.Copy ( s.StgPrm );

            Versatile = (int[]) s.Versatile.Clone ();	//値型なのでシャローコピー
		}


		//同値比較
		public override bool Equals ( object? obj )
		{
			//(Object)型で比較する
			//nullまたは型が異なるときfalse
			if ( null == obj || GetType() != obj.GetType () )
			{
				return false;
			}

            //キャストして各値を比較する
            Frame s = (Frame)obj;

			if (N != s.N ) { return false; }
			if (Group != s.Group ) { return false; }
			if (ImgName != s.ImgName ) { return false; }
			if (Pos != s.Pos ) { return false; }
			if ( !BD_RutName.SequenceEqual ( s.BD_RutName ) ) { return false; }
			if ( !ListCRect.SequenceEqual ( s.ListCRect ) ) { return false; }
			if ( !ListHRect.SequenceEqual ( s.ListHRect ) ) { return false; }
			if ( !ListARect.SequenceEqual ( s.ListARect ) ) { return false; }
			if ( !ListORect.SequenceEqual ( s.ListORect ) ) { return false; }
			if ( !BD_EfGnrt.SequenceEqual ( s.BD_EfGnrt ) ) { return false; }

			return true;
		}

		//Equals用ハッシュコード
		public override int GetHashCode ()
		{
			int i0  = N;
			int i1  = i0  ^ Group;
			int i2  = i1  ^ ImgName.GetHashCode ();
			int i3  = i2  ^ Pos.GetHashCode ();
			int i7  = i3  ^ BD_RutName.GetHashCode ();
			int i8  = i7  ^ ListCRect.GetHashCode ();
			int i9  = i8  ^ ListHRect.GetHashCode ();
			int i10 = i9  ^ ListARect.GetHashCode ();
			int i11 = i10 ^ ListORect.GetHashCode ();
			int i12 = i11 ^ ListORect.GetHashCode ();
			int i13 = i12 ^ ListORect.GetHashCode ();
			int i14 = i13 ^ ListORect.GetHashCode ();

			return i14;

			//匿名クラスのハッシュ値を返す
//			return new { }.GetHashCode ();
		}
	}

}

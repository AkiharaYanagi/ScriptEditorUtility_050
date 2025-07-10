using System.ComponentModel;
using ScriptEditor;


namespace ScriptEditor020
{
	using BD_SQC = BindingDictionary < Sequence >;
	using BL_SQC = BindingList < Sequence? >;
	using BD_IMGDT = BindingDictionary < ImageData >;
	using DCT_SQC = Dictionary < string, Sequence >;

	//================================================================
	//	<コンペンド> 		【一覧】シークエンスの継承であるアクションやエフェクトのリストを扱う
	//		┣[] シークエンス
	//		┣[] イメージ
	//================================================================
	public class Compend
	{
		//---------------------------------------------------------------------
		//シークエンスリスト	
		public BD_SQC BD_Sequence = new BD_SQC ();

		//イメージリスト
		public BD_IMGDT BD_Image = new BD_IMGDT ();

		//---------------------------------------------------------------------
		//クリア
		public void Clear ()
		{
			ClearScript ();
			ClearImage ();
		}

		//スクリプトのみクリア
		public void ClearScript ()
		{
            //手動で内部情報までクリア
            BL_SQC bl_sqc = BD_Sequence.GetBindingList ();
			foreach ( Sequence? seq in bl_sqc )
			{
				seq?.Clear ();
			}
			BD_Sequence.Clear ();
		}

		//イメージのみクリア
		public void ClearImage ()
		{
			BD_Image.Clear ();
		}

		//コピー(ディープ)
		public virtual void Copy ( Compend srcCompend )
		{
			Clear ();
			BD_Sequence.DeepCopy ( srcCompend.BD_Sequence );
			CopyImageList ( srcCompend );
		}

		//イメージデータ部分のコピー
		public void CopyImageList ( Compend srcCompend )
		{
			ClearImage ();
			BD_Image.DeepCopy ( srcCompend.BD_Image );

#if false
			//イメージデータはディープコピーする
			BindingList < ImageData > bl_imgdt = srcCompend.BD_Image.GetBindingList();
			foreach ( ImageData imageData in bl_imgdt )
			{
				this.BD_Image.Add ( new ImageData ( imageData ) );
			}
#endif
		}

		//全シークエンス内でのスクリプト最大数
		public int MaxNumScript ()
		{
			int maxNumScript = 0;
			BL_SQC bl_sqc = BD_Sequence.GetBindingList ();
			foreach ( Sequence? seq in bl_sqc )
			{
				if ( maxNumScript < seq?.ListScript.Count )
				{
					maxNumScript = seq.ListScript.Count;
				}
			}
			return maxNumScript;
		}
	}


	//================================================================
	//コンペンド継承：アクションの集合
	//Behavior	ビヘイビア【行動様式、振舞】
	//================================================================
	public class Behavior : Compend
	{
		//Action型指定 インデクサ
		public Action? this [ int i ]
		{
			set => base.BD_Sequence.GetBindingList () [ i ] = value;
			get { return base.BD_Sequence.GetBindingList () [ i ] as Action; }
		}

		//Action型指定 コピー
		public override void Copy ( Compend srcCompend )
		{
			base.Clear ();

			//ディープコピー
			//引数：Sequenceを受けて新規Actionを生成して返すデリゲート
			BD_Sequence.DeepCopy ( srcCompend.BD_Sequence, sqc=>new Action(sqc) );

			CopyImageList ( srcCompend );
		}
	}


	//================================================================
	//コンペンド継承：エフェクトの集合
	//Garnish	ガーニッシュ【装飾品、付け合わせ】
	//================================================================
	public class Garnish : Compend
	{
		//Effect型指定 インデクサ
		public ScriptEditor020.Effect? this [ int i ]
        {
            set { base.BD_Sequence.GetBindingList()[i] = value; }
            get => BD_Sequence.GetBindingList()[i] as Effect;
        }

        //Effect型指定 コピー
        public override void Copy ( Compend srcCompend )
		{
			base.Clear ();

			//ディープコピー
			//引数：Sequenceを受けて新規Effectを生成して返すデリゲート
			BD_Sequence.DeepCopy ( srcCompend.BD_Sequence, sqc=>new Effect(sqc) );


			CopyImageList ( srcCompend );
		}
	}


}

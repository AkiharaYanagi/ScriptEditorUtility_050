using System.ComponentModel;


namespace ScriptEditorUtility
{
	//=============================================================
	// 表示用バインディングリストと検索用ディクショナリを持つデータ構造
	//	検索名であるNameを扱うインターフェースを持つクラスを対象としたジェネリクス
	//	< T > where T: IName
	//	主に名前で検索し、リストボックスに表示する項目を扱う
	//	追加・削除は２つのデータを同期するため、専用のAddとDelを用いる
	//	対象データT t の中身はバインディングリストまたはディクショナリのどちらで取得後も同一オブジェクトであり変更可能
	//	BL_tとDCT_tの削除と追加は片方だけで行ってはならない
	//	名前の変更もDCTの更新が必要
	//	型制約：参照型(where T : class)を指定したので変更は可能
	//=============================================================
	public class BindingDictionary < T > where T : class?, IName, new ()
	{
		//----------------------------------------
		//メインデータ
		//	各種操作のときに同期する
		
		//検索用ディクショナリ
		private Dictionary < string, T? > DCT_t { get; set; } = new Dictionary < string, T? > ();

		//表示用バインディングリスト
		private BindingList < T? > BL_t { get; set; } = new BindingList < T? > ();
		//----------------------------------------

		//コンストラクタ
		public BindingDictionary ()
		{
		}
	
		public BindingDictionary ( BindingDictionary < T > bd_t )
		{
			DCT_t = new Dictionary < string, T? > ( bd_t.DCT_t );
			BL_t = new BindingList < T? > ( bd_t.BL_t );
		}
	
		//バインディングリストから生成
		public BindingDictionary ( BindingList < T? > bl_t )
		{
			DCT_t = new Dictionary < string, T? > ();
			foreach ( T? t in bl_t )
			{
				if ( t is null ) { continue; }
				DCT_t.Add ( t.Name, t );
			}
			BL_t = new BindingList < T? > ( bl_t );
		}
	
		//内部チェックして重ならない名前を返す
		public string UniqueName ( string name )
		{
			//同一名前が無いときそのまま返す
			if ( ! DCT_t.ContainsKey ( name ) ) { return name; }

			//同一名があるとき
			string name_base = name;

#if false
			//末尾が数値かどうか
			string tail = "";
			int nDigit = 0;
			do
			{
				++ nDigit;
				tail = name.Substring ( name.Length - nDigit, nDigit );
			}
			while ( int.TryParse ( tail, out int i ) );

			name_base = name.Substring ( 0, name.Length - nDigit + 1 ); 
#endif
			//末尾に数字以外を足す
			name_base += "_";


			//同一の値のとき例外が発生するので、重ならないよう前もって名前に追加命名("*"+"0")する
			int count = 0;
			string newname = "";
			do
			{
				newname = name_base + count.ToString ();
				++ count;
			}
			while ( DCT_t.ContainsKey ( newname ) );

			return newname;
		}

		//作成
		public void New ()
		{
			Add ( new T () );
		}

		//追加
		public void Add ( T? t )
		{
			if ( t is null ) { return; }

			//名前チェック
			t.Name = UniqueName ( t.Name );

			//追加
			DCT_t.Add ( t.Name, t );
			BL_t.Add ( t );
		}

		//挿入
		public void Insert ( int index, T t )
		{
			//名前チェック
			t.Name = UniqueName ( t.Name );

			BL_t.Insert ( index, t );
			DCT_t.Add ( t.Name, t );
		}

		//名前の変更
		public void ChangeName ( string before_name, string after_name )
		{
			if ( ! ContainsKey ( before_name ) ) { return; }

			//事前に取得しておく
			T? t = Get ( before_name );

			//DCTから外す
			DCT_t.Remove ( before_name );

			//唯一名
			string N = UniqueName ( after_name );

			//名前の変更
			t!.Name = N;

			//DCTに戻す
			DCT_t.Add ( N, t );
		}


		//取得
		public T? Get ( object obj )
		{
            if (obj is null) { throw new ArgumentNullException ( nameof ( obj ) ); }
			String? str = obj.ToString();
            if (str is null) { throw new ArgumentNullException(nameof(obj)); }
            return Get (str);
		}

		public T? Get ( string name )
		{
            //?はnull許容演算子
            //!はフォージビリティ演算子：「nullではない」とコンパイラに主張します
            if ( DCT_t.TryGetValue ( name, out T? t ) ) { return t!; }
			throw new KeyNotFoundException (name);

#if false
			//※ ジェネリックにおける "default" は対象の型の既定値を返す
			// 参照型はnull、数値型は0
			// 構造体はすべてのメンバーに対し0またはnull
			return DCT_t.TryGetValue ( name, out T t ) ? t : default ( T );
#endif
		}

		public T? Get ( int index )
		{
			if ( index < 0 || BL_t.Count < index ) { return default!; }
			return BL_t [ index ];
		}

		//インデクサ
		public T? this [ int i ]
		{ 
			get { return Get ( i ); }

			set
			{
				T? t = Get ( i );
				BL_t [ i ] = value;
                
				if ( t is not null )
				{ 
					DCT_t.Remove ( t.Name );
					DCT_t.Add ( t.Name, t );
				}
			}
		}


		//-------------------------------------------------------
		//削除
		public void RemoveAt ( int index )
		{
			if ( index < 0 || BL_t.Count <= index ) { return; }
			if ( BL_t [ index ] is null ) { return; }
//			if ( BL_t [ index ].Name is null ) { return; }

			T? t = BL_t [ index ]; 
			string name = t?.Name ?? "";
			if ( name == "" ) { return; }

			DCT_t.Remove ( name );
			BL_t.RemoveAt ( index );
			
			//@info BindingListの前にDictionaryを削除しないと
			//　バインドされたコントロールのイベントが途中で発生する
			// -> Countなどの値がずれてAssertする
		}

		public void Remove ( string name )
		{
			if ( ! ContainsKey ( name ) ) { return; }

			int index = this.IndexOf ( name );
			//@info BindingListの前にDictionaryを削除しないと
			//　バインドされたコントロールのイベントが途中で発生する
			// -> Countなどの値がずれてAssertする
			DCT_t.Remove ( name );
			BL_t.RemoveAt ( index );
		}

		public void Remove ( T t )
		{
			Remove ( t.Name );
		}

		public void Clear ()
		{
			DCT_t.Clear ();
			BL_t.Clear ();
			//@info BindingListの前にDictionaryを削除しないと
			//　バインドされたコントロールのイベントが途中で発生する
			// -> Countなどの値がずれてAssertする
		}

		public BindingList < T? > GetBindingList ()
		{
			return BL_t;
		}

		public IEnumerable < T? > GetEnumerable ()
		{
			return BL_t;
		}

		public Dictionary < string, T? > GetDictionary ()
		{
			return DCT_t;
		}

		//ディープコピー
		public void DeepCopy ( BindingDictionary < T > bd_t )
		{
			//同一オブジェクトのときは何もしない
			if ( ReferenceEquals ( this, bd_t ) ) { return; }

			Clear ();
			foreach ( T? t in bd_t.BL_t )
			{
				this.Add ( t );
			}
		}

		//型指定ディープコピー
		//Derived_New : Tを引数に継承型を生成するデリゲート
		public void DeepCopy ( BindingDictionary < T > bd_t, System.Func < T?, T? > Derived_New )
		{
			//同一オブジェクトのときは何もしない
			if ( ReferenceEquals ( this, bd_t ) ) { return; }

			Clear ();
			foreach ( T? t in bd_t.BL_t )
			{
				this.Add ( Derived_New ( t )  );
			}
		}

		//個数
		public int Count ()
		{
//			Debug.Assert ( BL_t.Count == DCT_t.Count );
			return BL_t.Count;
		}

//		public int Count { get; } = 0;

		//前へ移動
		public void Up ( int index )
		{
			//------------------------------------
			//条件
			int count = Count();
			if ( count < 2 ) { return; }		//２つ未満
			if ( count <= index ) { return; }	//指定位置が個数以上
			if ( index == 0 ) { return; }	//先頭

			//------------------------------------
			int prev_index = index - 1;	//1つ前の位置

			//バインディングリスト内の位置を更新
			//１つ前の位置に自身をコピー
			BL_t.Insert ( prev_index, BL_t [ index ] );

			//元の位置の後を削除
			BL_t.RemoveAt ( index + 1 );

			//ディクショナリは変更無し
		}

		//後へ移動
		public void Down ( int index )
		{
			//------------------------------------
			//条件
			int count = Count();
			if ( count < 2 ) { return; }		//２つ未満
			if ( count <= index ) { return; }	//指定位置が個数以上
			if ( index == count - 1 ) { return; }	//末尾
			//------------------------------------

			int next_index = index + 2;	//1つ次の位置

			//バインディングリスト内の位置を更新
			//１つ次の位置に自身をコピー
			BL_t.Insert ( next_index, BL_t [ index ] );

			//元の位置の後を削除
			BL_t.RemoveAt ( index );

			//ディクショナリは変更無し
		}

		public void Top ( int index )
		{
			//------------------------------------
			//条件
			int count = Count();
			if ( count < 2 ) { return; }		//２つ未満
			if ( count <= index ) { return; }	//指定位置が個数以上
			if ( index == 0 ) { return; }	//先頭
			//------------------------------------

			BL_t.Insert ( 0, BL_t [ index ] );
			BL_t.RemoveAt ( index + 1 );
		}

		public void Tail ( int index )
		{
			//------------------------------------
			//条件
			int count = Count();
			if ( count < 2 ) { return; }		//２つ未満
			if ( count <= index ) { return; }	//指定位置が個数以上
			if ( index == count - 1 ) { return; }	//末尾
			//------------------------------------

			BL_t.Insert ( count , BL_t [ index ] );
			BL_t.RemoveAt ( index );
		}


		//比較
		public bool SequenceEqual ( BindingDictionary < T > bd_t )
		{
			BindingList < T? > tgt = bd_t.GetBindingList ();
			if ( BL_t.Count != tgt.Count ) { return false; }

			for ( int i = 0; i < BL_t.Count; ++ i )
			{
				if ( BL_t[ i ]?.Name != tgt[ i ]?.Name ) { return false; }
			}
			return true;
		}

		//バインドの更新
		public void ResetItems ()
		{
			for ( int i = 0 ; i < BL_t.Count; ++ i )
			{
				BL_t.ResetItem ( i );
			}
		}

		//バインドの解除
		public void ResetBindings ()
		{
			BL_t.ResetBindings ();
		}

		//名前が存在する位置
		public int IndexOf ( string name )
		{
			T? t = Get ( name );
			if ( t is null ) { return -1; }
			return BL_t.IndexOf ( t );
		}

		//存在するかどうか
		public bool ContainsKey ( string name )
		{
			return DCT_t.ContainsKey ( name );
		}

		//存在するかどうか,無いとき例外投擲
		public void Try_Exist ( string name )
		{
			if ( ! DCT_t.ContainsKey ( name ) )
			{
				throw new Exception ( "NameErrar : " + name );
			}
		}
	}
}


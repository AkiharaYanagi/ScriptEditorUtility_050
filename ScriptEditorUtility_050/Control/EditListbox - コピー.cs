using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.IO;


namespace ScriptEditorUtility
{
	//各種条件
	using Func_Check = System.Func < object, bool > ;
	//IO
	using Func_Save = System.Action < object, StreamWriter > ;
	using Func_Load = System.Action < StreamReader > ;
	//Event
	using Event = System.Action;

	//==========================================================
	//	BindingDictionary < T >を受けて表示と編集をするコントロール
	//==========================================================
	public partial class _EditListbox < T >  : UserControl where T : class, IName, new ()
	{
		//対象
		public BindingDictionary < T > BD_T { get; set; } = new BindingDictionary < T > ();

		//新規作成
		public Func < T > New_T = ()=>new T();

		//取得
		public ListBox GetListBox () { return listBox1; }
		public T? Get () { return ( T? ) listBox1.SelectedItem; }
		public BindingList < T? > GetList () { return BD_T.GetBindingList (); }

		//色替条件
		public Func_Check Func_color_check { get; set; } = ob=>false;

		//IO
		public Func_Save Func_Save { get; set; } = (ob,sw)=>{};
		public Func_Load Func_Load { get; set; } = sr=>{};
		public string FilePath { get; set; } = "";
		public System.Action < string > Func_SavePath = s=>{};	//パスの保存

		//保存ファイル名
		public string SaveOneFileName { get; set; } = "SaveOneFile.txt";
		public string LoadOneFileName { get; set; } = "LoadOneFile.txt";
		public string SaveAllFileName { get; set; } = "SaveAllFile.txt";
		public string LoadAllFileName { get; set; } = "LoadAllFile.txt";

		//----------------------------------------------------------
		//コンストラクタ
		public _EditListbox ()
		{
			InitializeComponent ();

			Tb_Name.Text = "Name";

			//データソース
			listBox1.DataSource = BD_T.GetBindingList ();
			listBox1.DisplayMember = "Name";
		
			//表示
			listBox1.DrawMode = DrawMode.OwnerDrawFixed;
			listBox1.DrawItem += new DrawItemEventHandler ( ListBox1_DrawItem );
			listBox1.ScrollAlwaysVisible = true;

			//IO
			IO_Btn_Off ();
			FilePath = Directory.GetCurrentDirectory ();

			//イベント
			Btn_Add.Click += new EventHandler ( ListBox1_Add );
			Btn_Del.Click += new EventHandler ( ListBox1_Del );
			Tb_Name.TextChanged += new EventHandler ( Tb_Name_TextChanged );

			this.DoubleBuffered = true;
		}

		//データ設置
		public void SetData ( BindingDictionary < T > bd_t )
		{
			BD_T = bd_t;
			listBox1.DataSource = BD_T.GetBindingList ();
			listBox1.DisplayMember = "Name";
			BD_T.ResetItems ();

			_UpdateData ();

			//@info 非表示状態でDataSouceを入れ替えると例外が発生する

			//変更時イベント
			Listbox_Changed?.Invoke ();
		}

		//更新
		public void _UpdateData ()
		{
			if ( listBox1.Items.Count <= 0 ) { Tb_Name.Text = ""; return; }
			if ( listBox1.SelectedIndex < 0 ) { Tb_Name.Text = ""; return; }

			T? t = (T?) listBox1.SelectedItem;
			if ( t is null ) { return; }
			Tb_Name.Text = t.Name;

			UpdateData?.Invoke ();
		}

		//リストボックスの各１項目に対しての手動描画
		//（選択状態 または 指定条件 に該当するものを強調表示）
		private void ListBox1_DrawItem ( object? sender, DrawItemEventArgs e )
		{	
			if ( e.Index < 0 ) { return; }

			//背景(選択時強調表示)
			e.DrawBackground ();

			string? name = listBox1.GetItemText ( listBox1.Items[e.Index] );
			Brush Brs = Brushes.Black;

			//選択されているものを強調表示の背景から白抜き
			if ( (e.State & DrawItemState.Selected) == DrawItemState.Selected )
			{
				Brs = Brushes.White;
			}

			//指定条件チェック
			if ( Func_color_check ( listBox1.Items[e.Index] ) )
			{
				Brs = Brushes.Red;
			}

			Font font;
			if ( e.Font is null )
			{
				font = new Font ( "Arial", 12, FontStyle.Regular );
			}
			else
			{
				font = e.Font;
			}

			//文字列描画
			StringFormat sf = StringFormat.GenericDefault;
			e.Graphics.DrawString ( name, font, Brs, e.Bounds, sf );

			//フォーカス枠
			e.DrawFocusRectangle ();
		}

		//クリア
		public void Clear ()
		{
			BD_T.Clear ();
			BD_T.ResetItems ();
			Tb_Name.Text = "";
		}

		//表示
		public void Disp ()
		{
			listBox1.Invalidate ();
			Btn_Add.Invalidate ();
		}

		public void ResetItems () { BD_T.ResetItems (); }		//更新
		public void Add ( T t ) { BD_T.Add ( t ); }				//外部からの追加
		public int Count () { return listBox1.Items.Count; }		//個数
		public int SelectedIndex () { return listBox1.SelectedIndex; }		//選択位置

		public void TbName_Off () { Tb_Name.ReadOnly = true; }

		//============================================================
		//内部コントロールイベント

		//追加ボタン(新規挿入)
		private void Btn_Add_Click ( object sender, EventArgs e )
		{
			int n = listBox1.Items.Count;
			int slct = listBox1.SelectedIndex;

			//リストが０より大きく、末尾以外が選択されているとき
			if ( 0 < n && 0 <= slct && slct < n )
			{
				//次の位置に挿入
				BD_T.Insert ( slct + 1, New_T () );

				//選択位置を次にする
				listBox1.SelectedIndex = slct + 1;
			}
			else
			{
				//選択位置が末尾のとき、さらに後に追加
				BD_T.Add ( New_T () );
				listBox1.SelectedIndex = listBox1.Items.Count - 1;
			}

			//更新
			BD_T.ResetItems ();
		}

		//削除ボタン
		private void Btn_Del_Click ( object sender, EventArgs e )
		{
			if ( listBox1.Items.Count <= 0 ) { return; }

			BD_T.RemoveAt ( listBox1.SelectedIndex );
			BD_T.ResetItems ();

			if ( listBox1.Items.Count == 0 ) { Tb_Name.Text = ""; }
		}

		//上へ移動
		private void Btn_Up_Click ( object sender, EventArgs e )
		{
			//--------------------------------------------------------------------
			//動作条件　下記の条件時は何もしない
			if ( listBox1.Items.Count <= 1 ) { return; }			//対象個数が１以下
			if ( listBox1.SelectedItems.Count <= 0 ) { return; }	//選択されていない
			if ( listBox1.SelectedIndex <= 0 ) { return; }          //選択が先頭のとき
			//--------------------------------------------------------------------
			
			//１つ前の位置
			int i = listBox1.SelectedIndex - 1;
			BD_T.Up ( listBox1.SelectedIndex );

			//選択を１つ前へ
			listBox1.SelectedIndex = i;

			//変更時イベント
			Listbox_Changed?.Invoke ();

			listBox1.Invalidate ();
		}

		//下へ移動
		private void Btn_Down_Click ( object sender, EventArgs e )
		{
			//--------------------------------------------------------------------
			//動作条件　下記の条件時は何もしない
			if ( listBox1.Items.Count <= 1 ) { return; }		//対象個数が１以下
			if ( listBox1.SelectedItems.Count <= 0 ) { return; }	//選択されていない
			if ( listBox1.SelectedIndex >= listBox1.Items.Count - 1) { return; }	//選択が末尾のとき
			//--------------------------------------------------------------------

			//１つ次の位置
			int i = listBox1.SelectedIndex + 2;
			BD_T.Down ( listBox1.SelectedIndex );

			//選択を１つ次へ
			listBox1.SelectedIndex = i - 1;

			//変更時イベント
			Listbox_Changed?.Invoke ();

			listBox1.Invalidate ();
		}

		//先頭へ移動
		private void Btn_Top_Click ( object sender, EventArgs e )
		{
			//--------------------------------------------------------------------
			//動作条件　下記の条件時は何もしない
			if ( listBox1.Items.Count <= 1 ) { return; }		//対象個数が１以下
			if ( listBox1.SelectedItems.Count <= 0 ) { return; }		//選択されていない
			if ( listBox1.SelectedIndex <= 0 ) { return; }		//選択が先頭のとき
			//--------------------------------------------------------------------

			BD_T.Top ( listBox1.SelectedIndex );
			listBox1.SelectedIndex = 0;
		}

		//末尾へ移動
		private void Btn_Tail_Click ( object sender, EventArgs e )
		{
			//--------------------------------------------------------------------
			//動作条件　下記の条件時は何もしない
			if ( listBox1.Items.Count <= 1 ) { return; }		//対象個数が１以下
			if ( listBox1.SelectedItems.Count <= 0 ) { return; }	//選択されていない
			if ( listBox1.SelectedIndex >= listBox1.Items.Count - 1) { return; }	//選択が末尾のとき
			//--------------------------------------------------------------------

			BD_T.Tail ( listBox1.SelectedIndex );
			listBox1.SelectedIndex = listBox1.Items.Count - 1;
		}

		//============================================================
		//外部指定イベント
		//		public delegate void Event ();

		//更新
		public Event UpdateData { get; set; } = ()=>{};


		//リストボックスの変更すべて
		public Event Listbox_Changed { get; set; } = ()=>{};


		//イベント：リストボックス選択変更時
		public Event SelectedIndexChanged { get; set; } = ()=>{};
		private void listBox1_SelectedIndexChanged ( object sender, EventArgs e )
		{
			if  ( listBox1.SelectedItem is null ) { return; }
			Tb_Name.Text = ((T)listBox1.SelectedItem).Name;
			SelectedIndexChanged?.Invoke ();
		}

		//イベント：追加時
		public Event Listbox_Add { get; set; } = ()=>{};
		private void ListBox1_Add ( object? sender, System.EventArgs e )
		{
			Listbox_Add?.Invoke();
			Listbox_Changed?.Invoke();
		}

		//イベント：削除時
		public Event Listbox_Del { get; set; } = ()=>{};
		private void ListBox1_Del ( object? sender, System.EventArgs e )
		{
			Listbox_Del?.Invoke ();
			Listbox_Changed?.Invoke();
		}

		//イベント：テキストボックス 名前の変更
		public Event _TextChanged { get; set; } = ()=>{};
		public string GetName ()
		{
			return Tb_Name.Text;
		}
		private void Tb_Name_TextChanged ( object? sender, EventArgs e )
		{
			//@info BD.Up時、スワップ中のInsert()とRemove()の間にTextChangedイベントが発生し、
			//	Count()アサートが発生 
			//-> コメントアウトで対応
//			if ( BD_T.Count () == 0 ) { return; }

			//バインディングディクショナリの名前変更
			T? t = Get();
			if ( t is null ) { return; }
			if ( t.Name == Tb_Name.Text ) { return; }

			
			BD_T.ChangeName ( t.Name, Tb_Name.Text );

			//変更時イベント
			_TextChanged?.Invoke ();
			Listbox_Changed?.Invoke ();
		}

		//イベント：キー押下時
		public Event Tb_KeyPress { get; set; } = ()=>{};
		private void Tb_Name_KeyPress ( object sender, KeyPressEventArgs e )
		{
			//SEを鳴らさない
			if ( e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape )
			{
				e.Handled = true;
			}

			//Enter時に名前の決定
			if ( e.KeyChar == (char)Keys.Enter )
			{
				if ( listBox1.SelectedIndex < 0 ) { Tb_Name.Text = ""; return; }

				T? t = (T?)listBox1.SelectedItem;
				if ( t is null ) { return; }
				t.Name = Tb_Name.Text;
				BD_T.ResetItems ();

				//イベント
				Tb_KeyPress?.Invoke ();
				Listbox_Changed?.Invoke ();
			}
		}


		//============================================================
		//IO

		//フォルダ
		private void Btn_Folder_Click ( object? sender, EventArgs e )
		{
			FormUtility.OpenDir ( Path.GetDirectoryName ( FilePath ) );
		}

		//保存・読込の関数設定
		public void SetIOFunc ( Func_Save fs, Func_Load fl )
		{
			Func_Save = fs;
			Func_Load = fl;

			IO_Btn_On ();
		}

		//入出力ボタンの表示設定
		public void IO_Visible ( bool b )
		{
			Btn_SaveOne.Visible = b;
			Btn_SaveAll.Visible = b;
			Btn_LoadOne.Visible = b;
			Btn_LoadAll.Visible = b;

			Btn_SaveOne.Visible = b;
			Btn_SaveAll.Visible = b;
			Btn_LoadOne.Visible = b;
			Btn_LoadAll.Visible = b;

			Btn_Folder.Visible = b;
		}

		//入出力ボタンの動作オフ(表示はする)
		public void IO_Btn_Off ()
		{
			Color clr = Color.FromArgb ( 255, 192, 192, 192 );
			Btn_SaveOne.BackColor = clr;
			Btn_SaveAll.BackColor = clr;
			Btn_LoadOne.BackColor = clr;
			Btn_LoadAll.BackColor = clr;

			Btn_SaveOne.Enabled = false;
			Btn_SaveAll.Enabled = false;
			Btn_LoadOne.Enabled = false;
			Btn_LoadAll.Enabled = false;

			Btn_Folder.Enabled = false;
		}

		//入出力ボタンの動作オン
		public void IO_Btn_On ()
		{
			Color clrS = Color.FromArgb ( 255, 192, 192, 255 );
			Color clrL = Color.FromArgb ( 255, 255, 255, 192 );
			Btn_SaveOne.BackColor = clrS;
			Btn_SaveAll.BackColor = clrS;
			Btn_LoadOne.BackColor = clrL;
			Btn_LoadAll.BackColor = clrL;

			Btn_SaveOne.Enabled = true;
			Btn_SaveAll.Enabled = true;
			Btn_LoadOne.Enabled = true;
			Btn_LoadAll.Enabled = true;

			Btn_Folder.Enabled = true;
		}

		//1項目書出
		private void Btn_SaveOne_Click ( object sender, EventArgs e )
		{
			if ( listBox1.SelectedIndex < 0 ) { return; }

			using ( SaveFileDialog saveFileDialog = new SaveFileDialog () )
			{
				//対象オブジェクト
				T? t = (T?)listBox1.SelectedItem;
				if ( t is null ) { return; }

				saveFileDialog.DefaultExt = "txt";
				saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory ();
//				saveFileDialog.FileName = SaveOneFileName;
				saveFileDialog.FileName = t.Name + ".txt";

				if ( saveFileDialog.ShowDialog () == DialogResult.OK )
				{
					using ( StreamWriter sw = new StreamWriter ( saveFileDialog.FileName ) )
					{
						object? obj = listBox1.SelectedItem;
						if ( obj is null ) { return; }
						Func_Save?.Invoke ( obj, sw );
					}

					//@info 単体の時はパスを保存しない
//					Func_SavePath?.Invoke ( saveFileDialog.FileName );
				}
			}
			_UpdateData ();
		}

		//1項目読込
		private void Btn_LoadOne_Click ( object sender, EventArgs e )
		{
			using ( OpenFileDialog openFileDialog = new OpenFileDialog () )
			{
				openFileDialog.DefaultExt = "txt";
				openFileDialog.InitialDirectory = Directory.GetCurrentDirectory ();
				openFileDialog.FileName = LoadOneFileName;

				if ( openFileDialog.ShowDialog () == DialogResult.OK )
				{
					using ( StreamReader sr = new StreamReader ( openFileDialog.FileName ) )
					{
						Func_Load?.Invoke ( sr );
					}
					FilePath = openFileDialog.FileName;
					Func_SavePath?.Invoke ( openFileDialog.FileName );
				}
			}

			_UpdateData ();
		}

		//全項目書出
		private void Btn_SaveAll_Click ( object sender, EventArgs e )
		{
			if ( listBox1.Items.Count <= 0 ) { return; }
			if ( listBox1.SelectedIndex < 0 ) { return; }

			using ( SaveFileDialog saveFileDialog = new SaveFileDialog () )
			{
				saveFileDialog.DefaultExt = "txt";
				saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory ();
				saveFileDialog.FileName = Path.GetFileName ( FilePath );

				saveFileDialog.OverwritePrompt = false;

				if ( saveFileDialog.ShowDialog () == DialogResult.OK )
				{
					using ( StreamWriter sw = new StreamWriter ( saveFileDialog.FileName ) )
					{
						foreach ( object ob in listBox1.Items )
						{
							Func_Save?.Invoke ( ob, sw );
							sw.Write ( '\n' );
						}
					}
					FilePath = saveFileDialog.FileName;
					Func_SavePath?.Invoke ( saveFileDialog.FileName );
				}
			}
			_UpdateData ();
		}

		//全項目読込
		private void Btn_LoadAll_Click ( object sender, EventArgs e )
		{
			using ( OpenFileDialog openFileDialog = new OpenFileDialog () )
			{
				openFileDialog.DefaultExt = "txt";
				openFileDialog.InitialDirectory = Directory.GetCurrentDirectory ();
				openFileDialog.FileName = Path.GetFileName ( FilePath );

				if ( openFileDialog.ShowDialog () == DialogResult.OK )
				{
					using ( StreamReader sr = new StreamReader ( openFileDialog.FileName ) )
					{
						Clear ();
						while ( ! sr.EndOfStream )
						{
							Func_Load?.Invoke ( sr );
						}
					}
					FilePath = openFileDialog.FileName;
					Func_SavePath?.Invoke ( openFileDialog.FileName );
				}
			}

			_UpdateData ();
		}

		//自動読込
		public void LoadData ( string filepath )
		{
			//ファイルが存在しないときは何もしない
			if ( ! File.Exists ( filepath ) ) { return; }

			using ( StreamReader sr = new StreamReader ( filepath ) )
			{
				Clear ();
				while ( ! sr.EndOfStream )
				{
					Func_Load?.Invoke ( sr );
				}
				FilePath = filepath;
			}
		}

		//上書保存
		public void SaveOverwrite ()
		{
			using ( StreamWriter sw = new StreamWriter ( SaveAllFileName ) )
			{
				foreach ( object ob in listBox1.Items )
				{
					Func_Save?.Invoke ( ob, sw );
					
					//最後の改行(単体保存の繰り返しのため)
					sw.Write ( '\n' );
				}
			}
		}
	}

}

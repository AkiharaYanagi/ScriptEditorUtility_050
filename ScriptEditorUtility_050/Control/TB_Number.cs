

namespace ScriptEditorUtility
{
	//----------------------------------------------------------------------
	// スクリプトに基づいた整数値のみ、表示・編集できるテキストボックス
	//----------------------------------------------------------------------
	using Setter = System.Action < int >;
	using Getter = System.Func < int >;

	public class TB_Number : TextBox
	{
		//設定用デリゲート
		public Setter SetFunc { get; set; } = i=>{};
		public Getter GetFunc { get; set; } = ()=>0;

		//コンストラクタ
		public TB_Number ()
		{
			this.Text = "0";
		}

		//設定用、取得用関数
		public void Assosiate ( Setter setfunc, Getter getfunc )
		{
			GetFunc = getfunc;
			this.Text = GetFunc ().ToString ();
			
			SetFunc = setfunc;
		}

		//キー押下時(文字コード判定)
		protected override void  OnKeyPress ( KeyPressEventArgs e )
		{
			char c = e.KeyChar;

			//数字、マイナス、BackSpaceだけ入力可能(他は弾く)
			if ( c == '\b' )
			{
				e.Handled = false;
			}
			else if ( c == '-' ) 
			{ 
				e.Handled = false; 
			}
			else if ( ! char.IsDigit ( c ) )
			{
				e.Handled = true; 
			}

 			base.OnKeyPress(e);
		}

		//キー入力時(キーボード)
		protected override void OnKeyDown ( KeyEventArgs e )
		{
			//テキストボックスに数値が入力されていてEnterが押されたとき、
			//関連付けられた値を保存
			if ( e.KeyCode == Keys.Enter )
			{
				SetValue ();		//値の設定
				this.Invalidate ();	//画面の更新
			}

			base.OnKeyDown ( e );
		}

		//値を設定
		private void SetValue ()
		{
			int value = 0;
			try
			{
				value = int.Parse ( this.Text );
			}
			catch	//int.Parse(s)が失敗したとき
			{
				return;		//何もしない
			}

			SetFunc?.Invoke ( value );			
		}

		//更新
		public void UpdateData ()
		{
			this.Text = GetFunc?.Invoke ().ToString ();
		}
	}
}

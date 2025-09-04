using Chara050;


namespace ScriptEditor
{
	using GKC_ST = GameKeyData.GameKeyState;

	public partial class Ctrl_Command : UserControl
	{
		private Command Cmd = new Command ();		//対象データ
		private SelectKey SlctKey = new SelectKey ();		//選択
		private EditCommand EdtCmd = new EditCommand ();		//編集
		private DispCommand DspCmd = new DispCommand ();		//表示

		//コンストラクタ
		public Ctrl_Command ()
		{
			InitializeComponent ();

			pb_Command1.RB_OFF = RB_OFF;
			pb_Command1.RB_PUSH = RB_PUSH;
			pb_Command1.RB_RELE = RB_RELE;
			pb_Command1.RB_ON = RB_ON;
			pb_Command1.RB_WILD = RB_WILD;
			pb_Command1.RB_IS = RB_IS;
			pb_Command1.RB_NIS = RB_NIS;

			
			panel1.Scroll += new ScrollEventHandler ( Panel1_Scroll );
		}

		private void Panel1_Scroll ( object? sender, ScrollEventArgs e )
		{
			//縦
			if ( e.ScrollOrientation == ScrollOrientation.VerticalScroll )
			{
				int x = panel1.AutoScrollPosition.X;
				panel1.AutoScrollPosition = new Point ( -1 * x, e.NewValue );
			}
			//横
			if ( e.ScrollOrientation == ScrollOrientation.HorizontalScroll )
			{
				int y = panel1.AutoScrollPosition.Y;
				panel1.AutoScrollPosition = new Point ( e.NewValue, -1 * y );
			}

			pb_Command1.Invalidate ();
		}

		//データの設定
		public void Set ( Command cmd )
		{
			Cmd = cmd;
			pb_Command1.DspCmd = DspCmd;
			pb_Command1.Set ( cmd );

			List<GameKeyCommand> lk = Cmd.ListGameKeyCommand;
			if ( lk.Count - 1 < SlctKey.Frame ) { SlctKey.Frame = 0; SlctKey.Selecting = false; }
			SlctKey.Selecting = true;
			pb_Command1.SlctKey = SlctKey;
			DspCmd.SlctKey = SlctKey;

			EdtCmd.Cmd = cmd;
			EdtCmd.SlctKey = SlctKey;

			
			TBN_LimitTime.Text = cmd.LimitTime.ToString();
			TBN_LimitTime.SetFunc = i=> cmd.LimitTime = i;
			TBN_LimitTime.GetFunc = ()=> cmd.LimitTime;

			GameKeyCommand? gkc = SlctKey.GetGKC( cmd );
			if ( null != gkc )
			{
				CHK_Not.Checked = gkc.Not;
			}

			pb_Command1.Invalidate ();
		}

		//キー追加
		private void Btn_Add_Click ( object sender, EventArgs e )
		{
			Cmd.ListGameKeyCommand.Add ( new GameKeyCommand () );

			//キーの個数で描画サイズを変更する
			int n = Cmd.ListGameKeyCommand.Count;
			pb_Command1.Width = 48 * ( n + 3 );
			pb_Command1.Invalidate ();
		}
		
		//キー削除
		private void Btn_Del_Click ( object sender, EventArgs e )
		{
			List<GameKeyCommand> lk = Cmd.ListGameKeyCommand;
			if ( lk.Count < 1 ) { return; }
			if ( SlctKey.Frame < 0 ) { return; }
			if ( lk.Count <= SlctKey.Frame ) { return; }

			//選択位置を削除
			lk.RemoveAt ( SlctKey.Frame );

			//選択位置を修正
			SlctKey.Frame = 0;
			SlctKey.Selecting = false;

			pb_Command1.Invalidate ();
		}

		//レバー選択(←)
		private void Btn_KeyUp_Click ( object sender, EventArgs e )
		{
			EdtCmd.LvrSelect_L ();
			pb_Command1.Invalidate ();
		}

		//レバー選択(→)
		private void Btn_KeyDown_Click ( object sender, EventArgs e )
		{
			EdtCmd.LvrSelect_R ();
			pb_Command1.Invalidate ();
		}

		//---------------------------------------------------------------------------
		//ラジオボタンのON切替によるキータイミング変更
		private void RB_OFF_CheckedChanged ( object sender, EventArgs e )
		{
			if ( RB_OFF.Checked )
			{
				EdtCmd.Timing ( GKC_ST.KEY_OFF );
				pb_Command1.Invalidate ();
			}
		}

		private void RB_PUSH_CheckedChanged ( object sender, EventArgs e )
		{
			if ( RB_PUSH.Checked )
			{
				EdtCmd.Timing ( GKC_ST.KEY_PUSH );
				pb_Command1.Invalidate ();
			}
		}

		private void RB_RELE_CheckedChanged ( object sender, EventArgs e )
		{
			if ( RB_RELE.Checked )
			{
				EdtCmd.Timing ( GKC_ST.KEY_RELE );
				pb_Command1.Invalidate ();
			}
		}

		private void RB_ON_CheckedChanged ( object sender, EventArgs e )
		{
			if ( RB_ON.Checked )
			{
				EdtCmd.Timing ( GKC_ST.KEY_ON );
				pb_Command1.Invalidate ();
			}
		}

		private void RB_Wild_CheckedChanged ( object sender, EventArgs e )
		{
			if ( RB_WILD.Checked )
			{
				EdtCmd.Timing ( GKC_ST.KEY_WILD );
				pb_Command1.Invalidate ();
			}
		}

		private void RB_IS_CheckedChanged ( object sender, EventArgs e )
		{
			if ( RB_IS.Checked )
			{
				EdtCmd.Timing ( GKC_ST.KEY_IS );
				pb_Command1.Invalidate ();
			}
		}

		private void RB_NIS_CheckedChanged ( object sender, EventArgs e )
		{
			if ( RB_NIS.Checked )
			{
				EdtCmd.Timing ( GKC_ST.KEY_NIS );
				pb_Command1.Invalidate ();
			}
		}

		//---------------------------------------------------------------------------
		//否定のチェックボックス
		private void CHK_Not_CheckedChanged ( object sender, EventArgs e )
		{
			EdtCmd.TurnNot ( CHK_Not.Checked );
			pb_Command1.Invalidate ();

			if ( CHK_Not.Checked )
			{
				CHK_Not.BackColor = Color.FromArgb ( 255, 255, 127, 127 );
			}
			else
			{
				CHK_Not.BackColor = SystemColors.Control;
			}
		}
	}
}

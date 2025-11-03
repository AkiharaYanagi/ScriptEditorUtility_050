using System;
using System.Diagnostics;
using ScriptEditorUtility;


//2種類のCharaデータを変換する
using Chara020;
using Chara050;


using SE2 = Chara020;
using SE5 = Chara050;

using BD_SQC2 = ScriptEditorUtility.BindingDictionary<Chara020.Sequence>;
using BD_SQC5 = ScriptEditorUtility.BindingDictionary<Chara050.Sequence>;

using BD_IMG2 = ScriptEditorUtility.BindingDictionary<Chara020.ImageData>;
using BD_IMG5 = ScriptEditorUtility.BindingDictionary<Chara050.ImageData>;



namespace test00_Chara
{
    internal partial class ConvertChara
    {

        public Chara050.Chara Convert ( Chara020.Chara chara020 )
        {
			Chara050.Chara chara050 = new Chara050.Chara ();

			Debug.WriteLine ( "■■■■■■■■■■■■■■■■■■■■■■■■■■■■■" );
			Debug.WriteLine ( "Convert." );


			//--------------------------------------------------
			//behavior
			var bd_act_020 = chara020.behavior.BD_Sequence;
            var bd_act_050 = chara050.charaset.behavior.BD_Sequence;
			ConvertBehavior ( bd_act_020, bd_act_050 );
			ConvertImage ( chara020.behavior.BD_Image, chara050.charaset.behavior.BD_Image );

			//--------------------------------------------------
			//garnish
			var bd_gns_020 = chara020.garnish.BD_Sequence;
			var bd_gns_050 = chara050.charaset.garnish.BD_Sequence;
			ConvertGarnish ( bd_gns_020, bd_gns_050 );
			ConvertImage ( chara020.garnish.BD_Image, chara050.charaset.garnish.BD_Image );

			//--------------------------------------------------
			//Command
			ConvertCommand ( chara020, chara050 );

			//--------------------------------------------------
			//Branch
			ConvertBranch ( chara020, chara050 );

			//--------------------------------------------------
			//Route
			ConvertRoute ( chara020, chara050 );

			//--------------------------------------------------
			//変換済みキャラを返す
			return chara050;
        }

		public void ConvertBehavior ( BD_SQC2 bdsqc2, BD_SQC5 bdsqc5 )
		{
			//--------------------------------------------------
			//behavior
			// 020 ではAction, 050はSequence
			var bd_act_020 = bdsqc2;
			var bd_act_050 = bdsqc5;

			Debug.WriteLine ( "Sequence [ " + bd_act_020.Count ().ToString () + " ]" );


			//アクションから共通シークエンスへ変換
			foreach ( SE2.Action? act020 in bd_act_020.GetEnumerable () )
			{
				if ( act020 is null ) { continue; }

				SE5.Sequence act050 = new SE5.Sequence ();
				act050.Name = act020.Name;

				//アクションパラメータ
				SE5.ActionParam actPrm = act050.ActPrm;
				actPrm.NextActionName = act020.NextActionName;
				actPrm.Category = (SE5.ActionCategory) act020.Category;
				actPrm.Posture = (SE5.ActionPosture) act020.Posture;
				actPrm.HitNum = act020.HitNum;
				actPrm.HitPitch = act020.HitPitch;
				actPrm.Balance = act020.Balance;
				actPrm.Mana = act020.Mana;
				actPrm.Accel = act020.Accel;
				for ( int i = 0; i < ActionParam.VRS_SIZE; ++i )
				{
					actPrm.Versatile [ i ] = act020.Versatile [ i ];
				}

				//スクリプトからフレームに変換
				ConvertScript ( act050, act020 );

				//シークエンスを追加
				bd_act_050.Add ( act050 );
			}
		}

		public void ConvertGarnish ( BD_SQC2 bdsqc2, BD_SQC5 bdsqc5 )
		{
			var bd_gns_020 = bdsqc2;
			var bd_gns_050 = bdsqc5;

			//エフェクトから共通シークエンスへ変換
			foreach ( SE2.Effect? efc020 in bd_gns_020.GetEnumerable () )
			{
				if ( efc020 is null ) { continue; }

				SE5.Sequence efc050 = new SE5.Sequence ();
				efc050.Name = efc020.Name;

				//旧effectはアクションパラメータが存在しない

				//スクリプトからフレームに変換
				ConvertScript ( efc050, efc020 );

				//シークエンスを追加
				bd_gns_050.Add ( efc050 );
			}
		}

		public void ConvertImage ( BD_IMG2 bdimg2, BD_IMG5 bdimg5 )
		{
			foreach ( SE2.ImageData? imgdt2 in bdimg2.GetEnumerable () )
			{
				if ( imgdt2 is null ) { continue; }

				SE5.ImageData imgdt5 = new SE5.ImageData ();

				imgdt5.Img = imgdt2.Img;
				imgdt5.Path = imgdt2.Path;
				imgdt5.Img_file = imgdt2.Img_file;
				imgdt5.Thumbnail = imgdt2.Thumbnail;
				imgdt5.Name = imgdt2.Name;

				bdimg5.Add ( imgdt5 );
			}
		}


		public Chara050.Chara Convert_without_image ( Chara020.Chara chara020 )
		{
			Chara050.Chara chara050 = new Chara050.Chara ();

			Debug.WriteLine ( "■■■■■■■■■■■■■■■■■■■■■■■■■■■■■" );
			Debug.WriteLine ( "Convert." );


			//--------------------------------------------------
			//behavior
			var bd_act_020 = chara020.behavior.BD_Sequence;
			var bd_act_050 = chara050.charaset.behavior.BD_Sequence;
			ConvertBehavior ( bd_act_020, bd_act_050 );
//			ConvertImage ( chara020.behavior.BD_Image, chara050.charaset.behavior.BD_Image );

			//--------------------------------------------------
			//garnish
			var bd_gns_020 = chara020.garnish.BD_Sequence;
			var bd_gns_050 = chara050.charaset.garnish.BD_Sequence;
			ConvertGarnish ( bd_gns_020, bd_gns_050 );
//			ConvertImage ( chara020.garnish.BD_Image, chara050.charaset.garnish.BD_Image );

			//--------------------------------------------------
			//Command
			ConvertCommand ( chara020, chara050 );

			//--------------------------------------------------
			//Branch
			ConvertBranch ( chara020, chara050 );

			//--------------------------------------------------
			//Route
			ConvertRoute ( chara020, chara050 );

			//--------------------------------------------------
			//変換済みキャラを返す
			return chara050;
		}

	}

}

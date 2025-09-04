

namespace Chara050
{
	//アクション属性 定義
	public enum ActionCategory
	{
		NEUTRAL, MOVE, JUMP, DASH, 

		ATTACK_L, ATTACK_M, ATTACK_H, 
		ATTACK_J, 
		SKILL, 
		SPECIAL, OVERDRIVE, 

		DAMAGED, 
		POISED, CLANG, AVOID, 
		DOTTY, THROW, GUARD, 
		
		TEST,
		
		START, DEMO, OTHER
	}

	//-------------------------------------------------------
	//アクション体勢 定義
	public enum ActionPosture
	{
		STAND, CROUCH, JUMP
	}

	//-------------------------------------------------------
	//スクリプト 位置 計算状態
	public enum CLC_ST
	{
		CLC_MAINTAIN,	//持続 (速度：落下時など、0だが計算しない)
		CLC_SUBSTITUDE,	//代入 (速度：急停止、瞬間移動など直接指定)
		CLC_ADD,		//加算 (速度：移動など前回の値に加算)
	}
	
	//-------------------------------------------------------
	//ブランチ 条件	(優先度合はゲームメインで指定)
	//Save :: CharToDoc ではintに変換している 
	public enum BranchCondition
	{
		NONE,	//条件無し

		CMD,	//コマンド成立
		
		GRD,	//着地
		WALL,	//壁接触

		COERACION,	//相手強制

		DMG_I,	//自分が喰らい
		HIT_I,	//相手にヒット(自身を変更)
		HIT_E,	//相手にヒット(相手を変更)

		//打撃がヒット→バランス値参照→０なら成立
		THR_I,	//投げ成立 (ゲームメイン指定)
		THR_E,	//投げ成立 (ゲームメイン指定)

		OFS,	//相殺時
		END,	//シークエンス終了時
		DASH,	//ダッシュ相殺

		//他、[0-15]で特殊フラグをゲームメインで設定できる
		FLG_0, FLG_1, FLG_2, FLG_3, FLG_4, FLG_5, FLG_6, FLG_7, 
		FLG_8, FLG_9, FLG_10, FLG_11, FLG_12, FLG_13, FLG_14, FLG_15, 
	}

	//キャラ関連 定数
	public enum ConstChara
	{
		NumRect = 8,
	}

	//発生条件
	public enum Generate_Condition
	{
		HIT,		//ヒット時
		GUARD,		//ガード時
		COMPULSION,	//空振り時(強制発生)
	}

	//描画モード
	public enum Draw_Mode
	{
		NORMAL,		//通常
		SCREEN,		//スクリーン
	}
}

using ScriptEditorUtility;


namespace Chara050
{
    public class ActionParam
    {
        //次アクション名
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
		//public int[] Versatile { get; set; } = new int [ VRS_SIZE ];
		//public int[] Versatile { get; set; } = Enumerable.Range(0, VRS_SIZE).ToArray();

		//public List<int> Versatile { get; set; } = new List<int> ( VRS_SIZE );
		public List<int> Versatile { get; set; } = [ .. Enumerable.Repeat ( 0, VRS_SIZE ) ];
	}
}

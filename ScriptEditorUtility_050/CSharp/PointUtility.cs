using System.Drawing;

namespace ScriptEditor
{
	public static class PointUt
	{
		public static Point NewPt ( Point pt )
		{
			return new Point ( pt.X, pt.Y );
		}

		public static Point PtAdd ( Point pt0, Point pt1 )
		{
			return new Point ( pt0.X + pt1.X, pt0.Y + pt1.Y );
		}

		public static Point PtSub ( Point pt0, Point pt1 )
		{
			return new Point ( pt0.X - pt1.X, pt0.Y - pt1.Y );
		}
	}
}

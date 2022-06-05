using System;
using System.Collections.Generic;
using UnityEngine;

namespace TweenRL
{
	// Token: 0x02000871 RID: 2161
	public class BezierHelperRL
	{
		// Token: 0x06004761 RID: 18273 RVA: 0x00100A03 File Offset: 0x000FEC03
		private static double Factorial(int n)
		{
			if (BezierHelperRL.FactorialLookup == null)
			{
				BezierHelperRL.CreateFactorialTable();
			}
			if (n < 0)
			{
				throw new Exception("n is less than 0");
			}
			if (n > 32)
			{
				throw new Exception("n is greater than 32");
			}
			return BezierHelperRL.FactorialLookup[n];
		}

		// Token: 0x06004762 RID: 18274 RVA: 0x00100A38 File Offset: 0x000FEC38
		private static void CreateFactorialTable()
		{
			BezierHelperRL.FactorialLookup = new double[]
			{
				1.0,
				1.0,
				2.0,
				6.0,
				24.0,
				120.0,
				720.0,
				5040.0,
				40320.0,
				362880.0,
				3628800.0,
				39916800.0,
				479001600.0,
				6227020800.0,
				87178291200.0,
				1307674368000.0,
				20922789888000.0,
				355687428096000.0,
				6402373705728000.0,
				1.21645100408832E+17,
				2.43290200817664E+18,
				5.109094217170944E+19,
				1.1240007277776077E+21,
				2.585201673888498E+22,
				6.204484017332394E+23,
				1.5511210043330986E+25,
				4.0329146112660565E+26,
				1.0888869450418352E+28,
				3.0488834461171387E+29,
				8.841761993739702E+30,
				2.6525285981219107E+32,
				8.222838654177922E+33,
				2.631308369336935E+35
			};
		}

		// Token: 0x06004763 RID: 18275 RVA: 0x00100BF8 File Offset: 0x000FEDF8
		private static double Ni(int n, int i)
		{
			double num = BezierHelperRL.Factorial(n);
			double num2 = BezierHelperRL.Factorial(i);
			double num3 = BezierHelperRL.Factorial(n - i);
			return num / (num2 * num3);
		}

		// Token: 0x06004764 RID: 18276 RVA: 0x00100C20 File Offset: 0x000FEE20
		private static float Bernstein(int n, int i, float t)
		{
			double num;
			if ((double)t == 0.0 && i == 0)
			{
				num = 1.0;
			}
			else
			{
				num = Math.Pow((double)t, (double)i);
			}
			double num2;
			if (n == i && (double)t == 1.0)
			{
				num2 = 1.0;
			}
			else
			{
				num2 = Math.Pow((double)(1f - t), (double)(n - i));
			}
			return (float)(BezierHelperRL.Ni(n, i) * num * num2);
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x00100C90 File Offset: 0x000FEE90
		public static Vector2 GetBezierPt(Vector2[] b, float elapsedTime)
		{
			int num = b.Length;
			float num2 = 0f;
			float num3 = 0f;
			for (int num4 = 0; num4 != num; num4++)
			{
				float num5 = BezierHelperRL.Bernstein(num - 1, num4, elapsedTime);
				num2 += num5 * b[num4].x;
				num3 += num5 * b[num4].y;
			}
			return new Vector2(num2, num3);
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x00100CF0 File Offset: 0x000FEEF0
		public static Vector2 GetBezierPt(List<Vector2> b, float elapsedTime)
		{
			int count = b.Count;
			float num = 0f;
			float num2 = 0f;
			for (int num3 = 0; num3 != count; num3++)
			{
				float num4 = BezierHelperRL.Bernstein(count - 1, num3, elapsedTime);
				num += num4 * b[num3].x;
				num2 += num4 * b[num3].y;
			}
			return new Vector2(num, num2);
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x00100D54 File Offset: 0x000FEF54
		public static Vector2 GetBezierPt(float[] b, float elapsedTime)
		{
			int num = b.Length / 2;
			int num2 = 0;
			float num3 = 0f;
			float num4 = 0f;
			for (int num5 = 0; num5 != num; num5++)
			{
				float num6 = BezierHelperRL.Bernstein(num - 1, num5, elapsedTime);
				num3 += num6 * b[num2];
				num4 += num6 * b[num2 + 1];
				num2 += 2;
			}
			return new Vector2(num3, num4);
		}

		// Token: 0x04003C6B RID: 15467
		private static double[] FactorialLookup;
	}
}

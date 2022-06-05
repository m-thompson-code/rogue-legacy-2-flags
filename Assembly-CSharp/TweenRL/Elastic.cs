using System;

namespace TweenRL
{
	// Token: 0x02000D90 RID: 3472
	public class Elastic
	{
		// Token: 0x0600627C RID: 25212 RVA: 0x001559FC File Offset: 0x00153BFC
		public static float EaseIn(float t, float b, float c, float d)
		{
			float num = 0f;
			float num2 = 0f;
			if (t == 0f)
			{
				return b;
			}
			if ((t /= d) == 1f)
			{
				return b + c;
			}
			if (num2 == 0f)
			{
				num2 = d * 0.3f;
			}
			float num3;
			if (num == 0f || num < Math.Abs(c))
			{
				num = c;
				num3 = num2 / 4f;
			}
			else
			{
				num3 = (float)((double)num2 / 6.283185307179586 * Math.Asin((double)(c / num)));
			}
			return (float)(-(float)((double)num * Math.Pow(2.0, (double)(10f * (t -= 1f))) * Math.Sin((double)(t * d - num3) * 6.283185307179586 / (double)num2)) + (double)b);
		}

		// Token: 0x0600627D RID: 25213 RVA: 0x00155AB8 File Offset: 0x00153CB8
		public static float EaseOut(float t, float b, float c, float d)
		{
			float num = 0f;
			float num2 = 0f;
			if (t == 0f)
			{
				return b;
			}
			if ((t /= d) == 1f)
			{
				return b + c;
			}
			if (num2 == 0f)
			{
				num2 = d * 0.3f;
			}
			float num3;
			if (num == 0f || num < Math.Abs(c))
			{
				num = c;
				num3 = num2 / 4f;
			}
			else
			{
				num3 = (float)((double)num2 / 6.283185307179586 * Math.Asin((double)(c / num)));
			}
			return (float)((double)num * Math.Pow(2.0, (double)(-10f * t)) * Math.Sin((double)(t * d - num3) * 6.283185307179586 / (double)num2) + (double)c + (double)b);
		}

		// Token: 0x0600627E RID: 25214 RVA: 0x00155B6C File Offset: 0x00153D6C
		public static float EaseInOut(float t, float b, float c, float d)
		{
			float num = 0f;
			float num2 = 0f;
			if (t == 0f)
			{
				return b;
			}
			if ((t /= d / 2f) == 2f)
			{
				return b + c;
			}
			if (num2 == 0f)
			{
				num2 = d * 0.45000002f;
			}
			float num3;
			if (num == 0f || num < Math.Abs(c))
			{
				num = c;
				num3 = num2 / 4f;
			}
			else
			{
				num3 = (float)((double)num2 / 6.283185307179586 * Math.Asin((double)(c / num)));
			}
			if (t < 1f)
			{
				return (float)(-0.5 * ((double)num * Math.Pow(2.0, (double)(10f * (t -= 1f))) * Math.Sin((double)(t * d - num3) * 6.283185307179586 / (double)num2)) + (double)b);
			}
			return (float)((double)num * Math.Pow(2.0, (double)(-10f * (t -= 1f))) * Math.Sin((double)(t * d - num3) * 6.283185307179586 / (double)num2) * 0.5 + (double)c + (double)b);
		}
	}
}

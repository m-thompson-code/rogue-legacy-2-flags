using System;

namespace TweenRL
{
	// Token: 0x02000D91 RID: 3473
	public class Expo
	{
		// Token: 0x06006280 RID: 25216 RVA: 0x000316D1 File Offset: 0x0002F8D1
		public static float EaseIn(float t, float b, float c, float d)
		{
			if (t != 0f)
			{
				return (float)((double)c * Math.Pow(2.0, (double)(10f * (t / d - 1f))) + (double)b);
			}
			return b;
		}

		// Token: 0x06006281 RID: 25217 RVA: 0x00031702 File Offset: 0x0002F902
		public static float EaseOut(float t, float b, float c, float d)
		{
			if (t != d)
			{
				return (float)((double)c * (-(float)Math.Pow(2.0, (double)(-10f * t / d)) + 1.0) + (double)b);
			}
			return b + c;
		}

		// Token: 0x06006282 RID: 25218 RVA: 0x00155C8C File Offset: 0x00153E8C
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if (t == 0f)
			{
				return b;
			}
			if (t == d)
			{
				return b + c;
			}
			if ((t /= d / 2f) < 1f)
			{
				return (float)((double)(c / 2f) * Math.Pow(2.0, (double)(10f * (t - 1f))) + (double)b);
			}
			return (float)((double)(c / 2f) * (-(float)Math.Pow(2.0, (double)(-10f * (t -= 1f))) + 2.0) + (double)b);
		}
	}
}

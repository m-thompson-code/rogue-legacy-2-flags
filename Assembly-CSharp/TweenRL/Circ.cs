using System;

namespace TweenRL
{
	// Token: 0x02000874 RID: 2164
	public class Circ
	{
		// Token: 0x06004777 RID: 18295 RVA: 0x00101160 File Offset: 0x000FF360
		public static float EaseIn(float t, float b, float c, float d)
		{
			return -c * (float)(Math.Sqrt((double)(1f - (t /= d) * t)) - 1.0) + b;
		}

		// Token: 0x06004778 RID: 18296 RVA: 0x00101186 File Offset: 0x000FF386
		public static float EaseOut(float t, float b, float c, float d)
		{
			return c * (float)Math.Sqrt((double)(1f - (t = t / d - 1f) * t)) + b;
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x001011A8 File Offset: 0x000FF3A8
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return -c / 2f * (float)(Math.Sqrt((double)(1f - t * t)) - 1.0) + b;
			}
			return c / 2f * (float)(Math.Sqrt((double)(1f - (t -= 2f) * t)) + 1.0) + b;
		}
	}
}

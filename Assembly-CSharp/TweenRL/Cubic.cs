using System;

namespace TweenRL
{
	// Token: 0x02000875 RID: 2165
	public class Cubic
	{
		// Token: 0x0600477B RID: 18299 RVA: 0x00101223 File Offset: 0x000FF423
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t + b;
		}

		// Token: 0x0600477C RID: 18300 RVA: 0x00101233 File Offset: 0x000FF433
		public static float EaseOut(float t, float b, float c, float d)
		{
			return c * ((t = t / d - 1f) * t * t + 1f) + b;
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x00101250 File Offset: 0x000FF450
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t + b;
			}
			return c / 2f * ((t -= 2f) * t * t + 2f) + b;
		}
	}
}

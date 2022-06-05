using System;

namespace TweenRL
{
	// Token: 0x02000D8F RID: 3471
	public class Cubic
	{
		// Token: 0x06006278 RID: 25208 RVA: 0x000316A5 File Offset: 0x0002F8A5
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t + b;
		}

		// Token: 0x06006279 RID: 25209 RVA: 0x000316B5 File Offset: 0x0002F8B5
		public static float EaseOut(float t, float b, float c, float d)
		{
			return c * ((t = t / d - 1f) * t * t + 1f) + b;
		}

		// Token: 0x0600627A RID: 25210 RVA: 0x001559AC File Offset: 0x00153BAC
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

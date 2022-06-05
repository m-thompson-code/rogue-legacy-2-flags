using System;

namespace TweenRL
{
	// Token: 0x02000D8D RID: 3469
	public class Bounce
	{
		// Token: 0x06006270 RID: 25200 RVA: 0x00155850 File Offset: 0x00153A50
		public static float EaseOut(float t, float b, float c, float d)
		{
			if ((t /= d) < 0.36363637f)
			{
				return c * (7.5625f * t * t) + b;
			}
			if (t < 0.72727275f)
			{
				return c * (7.5625f * (t -= 0.54545456f) * t + 0.75f) + b;
			}
			if (t < 0.90909094f)
			{
				return c * (7.5625f * (t -= 0.8181818f) * t + 0.9375f) + b;
			}
			return c * (7.5625f * (t -= 0.95454544f) * t + 0.984375f) + b;
		}

		// Token: 0x06006271 RID: 25201 RVA: 0x000364DE File Offset: 0x000346DE
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c - Bounce.EaseOut(d - t, 0f, c, d) + b;
		}

		// Token: 0x06006272 RID: 25202 RVA: 0x00170278 File Offset: 0x0016E478
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Bounce.EaseIn(t * 2f, 0f, c, d) * 0.5f + b;
			}
			return Bounce.EaseOut(t * 2f - d, 0f, c, d) * 0.5f + c * 0.5f + b;
		}
	}
}

using System;

namespace TweenRL
{
	// Token: 0x0200087A RID: 2170
	public class Quart
	{
		// Token: 0x06004790 RID: 18320 RVA: 0x001016E9 File Offset: 0x000FF8E9
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t * t + b;
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x001016FB File Offset: 0x000FF8FB
		public static float EaseOut(float t, float b, float c, float d)
		{
			return -c * ((t = t / d - 1f) * t * t * t - 1f) + b;
		}

		// Token: 0x06004792 RID: 18322 RVA: 0x0010171C File Offset: 0x000FF91C
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t * t + b;
			}
			return -c / 2f * ((t -= 2f) * t * t * t - 2f) + b;
		}
	}
}

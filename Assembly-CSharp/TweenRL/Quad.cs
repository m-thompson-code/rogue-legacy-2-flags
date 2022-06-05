using System;

namespace TweenRL
{
	// Token: 0x02000879 RID: 2169
	public class Quad
	{
		// Token: 0x0600478C RID: 18316 RVA: 0x0010166B File Offset: 0x000FF86B
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t + b;
		}

		// Token: 0x0600478D RID: 18317 RVA: 0x00101679 File Offset: 0x000FF879
		public static float EaseOut(float t, float b, float c, float d)
		{
			return -c * (t /= d) * (t - 2f) + b;
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x00101690 File Offset: 0x000FF890
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t + b;
			}
			return -c / 2f * ((t -= 1f) * (t - 2f) - 1f) + b;
		}
	}
}

using System;

namespace TweenRL
{
	// Token: 0x02000D94 RID: 3476
	public class Quart
	{
		// Token: 0x0600628D RID: 25229 RVA: 0x00031759 File Offset: 0x0002F959
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t * t + b;
		}

		// Token: 0x0600628E RID: 25230 RVA: 0x0003176B File Offset: 0x0002F96B
		public static float EaseOut(float t, float b, float c, float d)
		{
			return -c * ((t = t / d - 1f) * t * t * t - 1f) + b;
		}

		// Token: 0x0600628F RID: 25231 RVA: 0x00155D74 File Offset: 0x00153F74
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

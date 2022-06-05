using System;

namespace TweenRL
{
	// Token: 0x02000D93 RID: 3475
	public class Quad
	{
		// Token: 0x06006289 RID: 25225 RVA: 0x00031736 File Offset: 0x0002F936
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t + b;
		}

		// Token: 0x0600628A RID: 25226 RVA: 0x00031744 File Offset: 0x0002F944
		public static float EaseOut(float t, float b, float c, float d)
		{
			return -c * (t /= d) * (t - 2f) + b;
		}

		// Token: 0x0600628B RID: 25227 RVA: 0x00155D20 File Offset: 0x00153F20
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

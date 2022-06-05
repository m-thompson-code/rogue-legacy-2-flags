using System;

namespace TweenRL
{
	// Token: 0x02000D8E RID: 3470
	public class Circ
	{
		// Token: 0x06006274 RID: 25204 RVA: 0x0003165E File Offset: 0x0002F85E
		public static float EaseIn(float t, float b, float c, float d)
		{
			return -c * (float)(Math.Sqrt((double)(1f - (t /= d) * t)) - 1.0) + b;
		}

		// Token: 0x06006275 RID: 25205 RVA: 0x00031684 File Offset: 0x0002F884
		public static float EaseOut(float t, float b, float c, float d)
		{
			return c * (float)Math.Sqrt((double)(1f - (t = t / d - 1f) * t)) + b;
		}

		// Token: 0x06006276 RID: 25206 RVA: 0x00155938 File Offset: 0x00153B38
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

using System;

namespace TweenRL
{
	// Token: 0x02000D92 RID: 3474
	public class Linear
	{
		// Token: 0x06006284 RID: 25220 RVA: 0x00031640 File Offset: 0x0002F840
		public static float EaseNone(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}

		// Token: 0x06006285 RID: 25221 RVA: 0x00031640 File Offset: 0x0002F840
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}

		// Token: 0x06006286 RID: 25222 RVA: 0x00031640 File Offset: 0x0002F840
		public static float EaseOut(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}

		// Token: 0x06006287 RID: 25223 RVA: 0x00031640 File Offset: 0x0002F840
		public static float EaseInOut(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}
	}
}

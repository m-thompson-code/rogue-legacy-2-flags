using System;

namespace TweenRL
{
	// Token: 0x02000D95 RID: 3477
	public class Quint
	{
		// Token: 0x06006291 RID: 25233 RVA: 0x0003178A File Offset: 0x0002F98A
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t * t * t + b;
		}

		// Token: 0x06006292 RID: 25234 RVA: 0x0003179E File Offset: 0x0002F99E
		public static float EaseOut(float t, float b, float c, float d)
		{
			return c * ((t = t / d - 1f) * t * t * t * t + 1f) + b;
		}

		// Token: 0x06006293 RID: 25235 RVA: 0x00155DC8 File Offset: 0x00153FC8
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t * t * t + b;
			}
			return c / 2f * ((t -= 2f) * t * t * t * t + 2f) + b;
		}
	}
}

using System;

namespace TweenRL
{
	// Token: 0x02000872 RID: 2162
	public class Back
	{
		// Token: 0x06004769 RID: 18281 RVA: 0x00100DC0 File Offset: 0x000FEFC0
		public static float EaseIn(float t, float b, float c, float d)
		{
			float num = 1.70158f;
			return c * (t /= d) * t * ((num + 1f) * t - num) + b;
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x00100DEC File Offset: 0x000FEFEC
		public static float EaseInSmall(float t, float b, float c, float d)
		{
			float num = 0.85079f;
			return c * (t /= d) * t * ((num + 1f) * t - num) + b;
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x00100E18 File Offset: 0x000FF018
		public static float EaseInLarge(float t, float b, float c, float d)
		{
			float num = 5.10474f;
			return c * (t /= d) * t * ((num + 1f) * t - num) + b;
		}

		// Token: 0x0600476C RID: 18284 RVA: 0x00100E44 File Offset: 0x000FF044
		public static float EaseOut(float t, float b, float c, float d)
		{
			float num = 1.70158f;
			return c * ((t = t / d - 1f) * t * ((num + 1f) * t + num) + 1f) + b;
		}

		// Token: 0x0600476D RID: 18285 RVA: 0x00100E7C File Offset: 0x000FF07C
		public static float EaseOutSmall(float t, float b, float c, float d)
		{
			float num = 0.85079f;
			return c * ((t = t / d - 1f) * t * ((num + 1f) * t + num) + 1f) + b;
		}

		// Token: 0x0600476E RID: 18286 RVA: 0x00100EB4 File Offset: 0x000FF0B4
		public static float EaseOutLarge(float t, float b, float c, float d)
		{
			float num = 5.10474f;
			return c * ((t = t / d - 1f) * t * ((num + 1f) * t + num) + 1f) + b;
		}

		// Token: 0x0600476F RID: 18287 RVA: 0x00100EEC File Offset: 0x000FF0EC
		public static float EaseInOut(float t, float b, float c, float d)
		{
			float num = 1.70158f;
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
			}
			return c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
		}

		// Token: 0x06004770 RID: 18288 RVA: 0x00100F64 File Offset: 0x000FF164
		public static float EaseInOutLarge(float t, float b, float c, float d)
		{
			float num = 5.10474f;
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
			}
			return c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
		}

		// Token: 0x06004771 RID: 18289 RVA: 0x00100FDC File Offset: 0x000FF1DC
		public static float EaseInOutSmall(float t, float b, float c, float d)
		{
			float num = 0.85079f;
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
			}
			return c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
		}
	}
}

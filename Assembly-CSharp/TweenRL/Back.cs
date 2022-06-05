using System;

namespace TweenRL
{
	// Token: 0x02000D8C RID: 3468
	public class Back
	{
		// Token: 0x06006266 RID: 25190 RVA: 0x001555BC File Offset: 0x001537BC
		public static float EaseIn(float t, float b, float c, float d)
		{
			float num = 1.70158f;
			return c * (t /= d) * t * ((num + 1f) * t - num) + b;
		}

		// Token: 0x06006267 RID: 25191 RVA: 0x001555E8 File Offset: 0x001537E8
		public static float EaseInSmall(float t, float b, float c, float d)
		{
			float num = 0.85079f;
			return c * (t /= d) * t * ((num + 1f) * t - num) + b;
		}

		// Token: 0x06006268 RID: 25192 RVA: 0x00155614 File Offset: 0x00153814
		public static float EaseInLarge(float t, float b, float c, float d)
		{
			float num = 5.10474f;
			return c * (t /= d) * t * ((num + 1f) * t - num) + b;
		}

		// Token: 0x06006269 RID: 25193 RVA: 0x00155640 File Offset: 0x00153840
		public static float EaseOut(float t, float b, float c, float d)
		{
			float num = 1.70158f;
			return c * ((t = t / d - 1f) * t * ((num + 1f) * t + num) + 1f) + b;
		}

		// Token: 0x0600626A RID: 25194 RVA: 0x00155678 File Offset: 0x00153878
		public static float EaseOutSmall(float t, float b, float c, float d)
		{
			float num = 0.85079f;
			return c * ((t = t / d - 1f) * t * ((num + 1f) * t + num) + 1f) + b;
		}

		// Token: 0x0600626B RID: 25195 RVA: 0x001556B0 File Offset: 0x001538B0
		public static float EaseOutLarge(float t, float b, float c, float d)
		{
			float num = 5.10474f;
			return c * ((t = t / d - 1f) * t * ((num + 1f) * t + num) + 1f) + b;
		}

		// Token: 0x0600626C RID: 25196 RVA: 0x001556E8 File Offset: 0x001538E8
		public static float EaseInOut(float t, float b, float c, float d)
		{
			float num = 1.70158f;
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
			}
			return c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
		}

		// Token: 0x0600626D RID: 25197 RVA: 0x00155760 File Offset: 0x00153960
		public static float EaseInOutLarge(float t, float b, float c, float d)
		{
			float num = 5.10474f;
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
			}
			return c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
		}

		// Token: 0x0600626E RID: 25198 RVA: 0x001557D8 File Offset: 0x001539D8
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

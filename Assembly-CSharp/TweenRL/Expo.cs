using System;

namespace TweenRL
{
	// Token: 0x02000877 RID: 2167
	public class Expo
	{
		// Token: 0x06004783 RID: 18307 RVA: 0x0010153D File Offset: 0x000FF73D
		public static float EaseIn(float t, float b, float c, float d)
		{
			if (t != 0f)
			{
				return (float)((double)c * Math.Pow(2.0, (double)(10f * (t / d - 1f))) + (double)b);
			}
			return b;
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x0010156E File Offset: 0x000FF76E
		public static float EaseOut(float t, float b, float c, float d)
		{
			if (t != d)
			{
				return (float)((double)c * (-(float)Math.Pow(2.0, (double)(-10f * t / d)) + 1.0) + (double)b);
			}
			return b + c;
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x001015A4 File Offset: 0x000FF7A4
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if (t == 0f)
			{
				return b;
			}
			if (t == d)
			{
				return b + c;
			}
			if ((t /= d / 2f) < 1f)
			{
				return (float)((double)(c / 2f) * Math.Pow(2.0, (double)(10f * (t - 1f))) + (double)b);
			}
			return (float)((double)(c / 2f) * (-(float)Math.Pow(2.0, (double)(-10f * (t -= 1f))) + 2.0) + (double)b);
		}
	}
}

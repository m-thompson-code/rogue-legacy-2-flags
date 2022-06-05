using System;

namespace TweenRL
{
	// Token: 0x0200087C RID: 2172
	public class Sine
	{
		// Token: 0x06004798 RID: 18328 RVA: 0x0010180A File Offset: 0x000FFA0A
		public static float EaseIn(float t, float b, float c, float d)
		{
			return (float)((double)(-(double)c) * Math.Cos((double)(t / d) * 1.5707963267948966) + (double)c + (double)b);
		}

		// Token: 0x06004799 RID: 18329 RVA: 0x0010182A File Offset: 0x000FFA2A
		public static float EaseOut(float t, float b, float c, float d)
		{
			return (float)((double)c * Math.Sin((double)(t / d) * 1.5707963267948966) + (double)b);
		}

		// Token: 0x0600479A RID: 18330 RVA: 0x00101846 File Offset: 0x000FFA46
		public static float EaseInOut(float t, float b, float c, float d)
		{
			return (float)((double)(-(double)c / 2f) * (Math.Cos(3.141592653589793 * (double)t / (double)d) - 1.0) + (double)b);
		}
	}
}

using System;

namespace TweenRL
{
	// Token: 0x02000D96 RID: 3478
	public class Sine
	{
		// Token: 0x06006295 RID: 25237 RVA: 0x000317BE File Offset: 0x0002F9BE
		public static float EaseIn(float t, float b, float c, float d)
		{
			return (float)((double)(-(double)c) * Math.Cos((double)(t / d) * 1.5707963267948966) + (double)c + (double)b);
		}

		// Token: 0x06006296 RID: 25238 RVA: 0x000317DE File Offset: 0x0002F9DE
		public static float EaseOut(float t, float b, float c, float d)
		{
			return (float)((double)c * Math.Sin((double)(t / d) * 1.5707963267948966) + (double)b);
		}

		// Token: 0x06006297 RID: 25239 RVA: 0x000317FA File Offset: 0x0002F9FA
		public static float EaseInOut(float t, float b, float c, float d)
		{
			return (float)((double)(-(double)c / 2f) * (Math.Cos(3.141592653589793 * (double)t / (double)d) - 1.0) + (double)b);
		}
	}
}

using System;

namespace TweenRL
{
	// Token: 0x0200087B RID: 2171
	public class Quint
	{
		// Token: 0x06004794 RID: 18324 RVA: 0x00101777 File Offset: 0x000FF977
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t * t * t + b;
		}

		// Token: 0x06004795 RID: 18325 RVA: 0x0010178B File Offset: 0x000FF98B
		public static float EaseOut(float t, float b, float c, float d)
		{
			return c * ((t = t / d - 1f) * t * t * t * t + 1f) + b;
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x001017AC File Offset: 0x000FF9AC
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

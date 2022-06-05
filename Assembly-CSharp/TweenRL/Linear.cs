using System;

namespace TweenRL
{
	// Token: 0x02000878 RID: 2168
	public class Linear
	{
		// Token: 0x06004787 RID: 18311 RVA: 0x0010163F File Offset: 0x000FF83F
		public static float EaseNone(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x00101648 File Offset: 0x000FF848
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x00101651 File Offset: 0x000FF851
		public static float EaseOut(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x0010165A File Offset: 0x000FF85A
		public static float EaseInOut(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}
	}
}

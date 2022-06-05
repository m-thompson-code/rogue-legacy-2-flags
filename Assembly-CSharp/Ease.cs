using System;

// Token: 0x02000728 RID: 1832
public class Ease
{
	// Token: 0x060040F6 RID: 16630 RVA: 0x000E5FDC File Offset: 0x000E41DC
	public static float None(float t, float b, float c, float d)
	{
		return c * t / d + b;
	}

	// Token: 0x060040F7 RID: 16631 RVA: 0x000E5FE8 File Offset: 0x000E41E8
	public static EaseDelegate GetEase(EaseType easeType, EaseInOutType inOutType)
	{
		if (easeType == EaseType.None)
		{
			return new EaseDelegate(Ease.None);
		}
		switch (easeType)
		{
		case EaseType.Back:
			switch (inOutType)
			{
			case EaseInOutType.EaseIn:
				return new EaseDelegate(Ease.Back.EaseIn);
			case EaseInOutType.EaseOut:
				return new EaseDelegate(Ease.Back.EaseOut);
			}
			return new EaseDelegate(Ease.Back.EaseInOut);
		case EaseType.BackSmall:
			switch (inOutType)
			{
			case EaseInOutType.EaseIn:
				return new EaseDelegate(Ease.Back.EaseInSmall);
			case EaseInOutType.EaseOut:
				return new EaseDelegate(Ease.Back.EaseOutSmall);
			}
			return new EaseDelegate(Ease.Back.EaseInOutSmall);
		case EaseType.BackLarge:
			switch (inOutType)
			{
			case EaseInOutType.EaseIn:
				return new EaseDelegate(Ease.Back.EaseInLarge);
			case EaseInOutType.EaseOut:
				return new EaseDelegate(Ease.Back.EaseOutLarge);
			}
			return new EaseDelegate(Ease.Back.EaseInOutLarge);
		case EaseType.Bounce:
			switch (inOutType)
			{
			case EaseInOutType.EaseIn:
				return new EaseDelegate(Ease.Bounce.EaseIn);
			case EaseInOutType.EaseOut:
				return new EaseDelegate(Ease.Bounce.EaseOut);
			}
			return new EaseDelegate(Ease.Bounce.EaseInOut);
		case EaseType.Circ:
			switch (inOutType)
			{
			case EaseInOutType.EaseIn:
				return new EaseDelegate(Ease.Circ.EaseIn);
			case EaseInOutType.EaseOut:
				return new EaseDelegate(Ease.Circ.EaseOut);
			}
			return new EaseDelegate(Ease.Circ.EaseInOut);
		case EaseType.Cubic:
			switch (inOutType)
			{
			case EaseInOutType.EaseIn:
				return new EaseDelegate(Ease.Cubic.EaseIn);
			case EaseInOutType.EaseOut:
				return new EaseDelegate(Ease.Cubic.EaseOut);
			}
			return new EaseDelegate(Ease.Cubic.EaseInOut);
		case EaseType.Elastic:
			switch (inOutType)
			{
			case EaseInOutType.EaseIn:
				return new EaseDelegate(Ease.Elastic.EaseIn);
			case EaseInOutType.EaseOut:
				return new EaseDelegate(Ease.Elastic.EaseOut);
			}
			return new EaseDelegate(Ease.Elastic.EaseInOut);
		case EaseType.Expo:
			switch (inOutType)
			{
			case EaseInOutType.EaseIn:
				return new EaseDelegate(Ease.Expo.EaseIn);
			case EaseInOutType.EaseOut:
				return new EaseDelegate(Ease.Expo.EaseOut);
			}
			return new EaseDelegate(Ease.Expo.EaseInOut);
		case EaseType.Quad:
			switch (inOutType)
			{
			case EaseInOutType.EaseIn:
				return new EaseDelegate(Ease.Quad.EaseIn);
			case EaseInOutType.EaseOut:
				return new EaseDelegate(Ease.Quad.EaseOut);
			}
			return new EaseDelegate(Ease.Quad.EaseInOut);
		case EaseType.Quart:
			switch (inOutType)
			{
			case EaseInOutType.EaseIn:
				return new EaseDelegate(Ease.Quart.EaseIn);
			case EaseInOutType.EaseOut:
				return new EaseDelegate(Ease.Quart.EaseOut);
			}
			return new EaseDelegate(Ease.Quart.EaseInOut);
		case EaseType.Quint:
			switch (inOutType)
			{
			case EaseInOutType.EaseIn:
				return new EaseDelegate(Ease.Quint.EaseIn);
			case EaseInOutType.EaseOut:
				return new EaseDelegate(Ease.Quint.EaseOut);
			}
			return new EaseDelegate(Ease.Quint.EaseInOut);
		case EaseType.Sine:
			switch (inOutType)
			{
			case EaseInOutType.EaseIn:
				return new EaseDelegate(Ease.Sine.EaseIn);
			case EaseInOutType.EaseOut:
				return new EaseDelegate(Ease.Sine.EaseOut);
			}
			return new EaseDelegate(Ease.Sine.EaseInOut);
		}
		switch (inOutType)
		{
		case EaseInOutType.EaseIn:
			return new EaseDelegate(Ease.Linear.EaseIn);
		case EaseInOutType.EaseOut:
			return new EaseDelegate(Ease.Linear.EaseOut);
		}
		return new EaseDelegate(Ease.Linear.EaseInOut);
	}

	// Token: 0x02000E27 RID: 3623
	public class Back
	{
		// Token: 0x06006B96 RID: 27542 RVA: 0x00192044 File Offset: 0x00190244
		public static float EaseIn(float t, float b, float c, float d)
		{
			float num = 1.70158f;
			return c * (t /= d) * t * ((num + 1f) * t - num) + b;
		}

		// Token: 0x06006B97 RID: 27543 RVA: 0x00192070 File Offset: 0x00190270
		public static float EaseInSmall(float t, float b, float c, float d)
		{
			float num = 0.85079f;
			return c * (t /= d) * t * ((num + 1f) * t - num) + b;
		}

		// Token: 0x06006B98 RID: 27544 RVA: 0x0019209C File Offset: 0x0019029C
		public static float EaseInLarge(float t, float b, float c, float d)
		{
			float num = 5.10474f;
			return c * (t /= d) * t * ((num + 1f) * t - num) + b;
		}

		// Token: 0x06006B99 RID: 27545 RVA: 0x001920C8 File Offset: 0x001902C8
		public static float EaseOut(float t, float b, float c, float d)
		{
			float num = 1.70158f;
			return c * ((t = t / d - 1f) * t * ((num + 1f) * t + num) + 1f) + b;
		}

		// Token: 0x06006B9A RID: 27546 RVA: 0x00192100 File Offset: 0x00190300
		public static float EaseOutSmall(float t, float b, float c, float d)
		{
			float num = 0.85079f;
			return c * ((t = t / d - 1f) * t * ((num + 1f) * t + num) + 1f) + b;
		}

		// Token: 0x06006B9B RID: 27547 RVA: 0x00192138 File Offset: 0x00190338
		public static float EaseOutLarge(float t, float b, float c, float d)
		{
			float num = 5.10474f;
			return c * ((t = t / d - 1f) * t * ((num + 1f) * t + num) + 1f) + b;
		}

		// Token: 0x06006B9C RID: 27548 RVA: 0x00192170 File Offset: 0x00190370
		public static float EaseInOut(float t, float b, float c, float d)
		{
			float num = 1.70158f;
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
			}
			return c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
		}

		// Token: 0x06006B9D RID: 27549 RVA: 0x001921E8 File Offset: 0x001903E8
		public static float EaseInOutLarge(float t, float b, float c, float d)
		{
			float num = 5.10474f;
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
			}
			return c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
		}

		// Token: 0x06006B9E RID: 27550 RVA: 0x00192260 File Offset: 0x00190460
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

	// Token: 0x02000E28 RID: 3624
	public class Bounce
	{
		// Token: 0x06006BA0 RID: 27552 RVA: 0x001922E0 File Offset: 0x001904E0
		public static float EaseOut(float t, float b, float c, float d)
		{
			if ((t /= d) < 0.36363637f)
			{
				return c * (7.5625f * t * t) + b;
			}
			if (t < 0.72727275f)
			{
				return c * (7.5625f * (t -= 0.54545456f) * t + 0.75f) + b;
			}
			if (t < 0.90909094f)
			{
				return c * (7.5625f * (t -= 0.8181818f) * t + 0.9375f) + b;
			}
			return c * (7.5625f * (t -= 0.95454544f) * t + 0.984375f) + b;
		}

		// Token: 0x06006BA1 RID: 27553 RVA: 0x0019236E File Offset: 0x0019056E
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c - Ease.Bounce.EaseOut(d - t, 0f, c, d) + b;
		}

		// Token: 0x06006BA2 RID: 27554 RVA: 0x00192384 File Offset: 0x00190584
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Ease.Bounce.EaseIn(t * 2f, 0f, c, d) * 0.5f + b;
			}
			return Ease.Bounce.EaseOut(t * 2f - d, 0f, c, d) * 0.5f + c * 0.5f + b;
		}
	}

	// Token: 0x02000E29 RID: 3625
	public class Circ
	{
		// Token: 0x06006BA4 RID: 27556 RVA: 0x001923E4 File Offset: 0x001905E4
		public static float EaseIn(float t, float b, float c, float d)
		{
			return -c * (float)(Math.Sqrt((double)(1f - (t /= d) * t)) - 1.0) + b;
		}

		// Token: 0x06006BA5 RID: 27557 RVA: 0x0019240A File Offset: 0x0019060A
		public static float EaseOut(float t, float b, float c, float d)
		{
			return c * (float)Math.Sqrt((double)(1f - (t = t / d - 1f) * t)) + b;
		}

		// Token: 0x06006BA6 RID: 27558 RVA: 0x0019242C File Offset: 0x0019062C
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return -c / 2f * (float)(Math.Sqrt((double)(1f - t * t)) - 1.0) + b;
			}
			return c / 2f * (float)(Math.Sqrt((double)(1f - (t -= 2f) * t)) + 1.0) + b;
		}
	}

	// Token: 0x02000E2A RID: 3626
	public class Cubic
	{
		// Token: 0x06006BA8 RID: 27560 RVA: 0x001924A7 File Offset: 0x001906A7
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t + b;
		}

		// Token: 0x06006BA9 RID: 27561 RVA: 0x001924B7 File Offset: 0x001906B7
		public static float EaseOut(float t, float b, float c, float d)
		{
			return c * ((t = t / d - 1f) * t * t + 1f) + b;
		}

		// Token: 0x06006BAA RID: 27562 RVA: 0x001924D4 File Offset: 0x001906D4
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t + b;
			}
			return c / 2f * ((t -= 2f) * t * t + 2f) + b;
		}
	}

	// Token: 0x02000E2B RID: 3627
	public class Elastic
	{
		// Token: 0x06006BAC RID: 27564 RVA: 0x0019252C File Offset: 0x0019072C
		public static float EaseIn(float t, float b, float c, float d)
		{
			float num = 0f;
			float num2 = 0f;
			if (t == 0f)
			{
				return b;
			}
			if ((t /= d) == 1f)
			{
				return b + c;
			}
			if (num2 == 0f)
			{
				num2 = d * 0.3f;
			}
			float num3;
			if (num == 0f || num < Math.Abs(c))
			{
				num = c;
				num3 = num2 / 4f;
			}
			else
			{
				num3 = (float)((double)num2 / 6.283185307179586 * Math.Asin((double)(c / num)));
			}
			return (float)(-(float)((double)num * Math.Pow(2.0, (double)(10f * (t -= 1f))) * Math.Sin((double)(t * d - num3) * 6.283185307179586 / (double)num2)) + (double)b);
		}

		// Token: 0x06006BAD RID: 27565 RVA: 0x001925E8 File Offset: 0x001907E8
		public static float EaseOut(float t, float b, float c, float d)
		{
			float num = 0f;
			float num2 = 0f;
			if (t == 0f)
			{
				return b;
			}
			if ((t /= d) == 1f)
			{
				return b + c;
			}
			if (num2 == 0f)
			{
				num2 = d * 0.3f;
			}
			float num3;
			if (num == 0f || num < Math.Abs(c))
			{
				num = c;
				num3 = num2 / 4f;
			}
			else
			{
				num3 = (float)((double)num2 / 6.283185307179586 * Math.Asin((double)(c / num)));
			}
			return (float)((double)num * Math.Pow(2.0, (double)(-10f * t)) * Math.Sin((double)(t * d - num3) * 6.283185307179586 / (double)num2) + (double)c + (double)b);
		}

		// Token: 0x06006BAE RID: 27566 RVA: 0x0019269C File Offset: 0x0019089C
		public static float EaseInOut(float t, float b, float c, float d)
		{
			float num = 0f;
			float num2 = 0f;
			if (t == 0f)
			{
				return b;
			}
			if ((t /= d / 2f) == 2f)
			{
				return b + c;
			}
			if (num2 == 0f)
			{
				num2 = d * 0.45000002f;
			}
			float num3;
			if (num == 0f || num < Math.Abs(c))
			{
				num = c;
				num3 = num2 / 4f;
			}
			else
			{
				num3 = (float)((double)num2 / 6.283185307179586 * Math.Asin((double)(c / num)));
			}
			if (t < 1f)
			{
				return (float)(-0.5 * ((double)num * Math.Pow(2.0, (double)(10f * (t -= 1f))) * Math.Sin((double)(t * d - num3) * 6.283185307179586 / (double)num2)) + (double)b);
			}
			return (float)((double)num * Math.Pow(2.0, (double)(-10f * (t -= 1f))) * Math.Sin((double)(t * d - num3) * 6.283185307179586 / (double)num2) * 0.5 + (double)c + (double)b);
		}
	}

	// Token: 0x02000E2C RID: 3628
	public class Expo
	{
		// Token: 0x06006BB0 RID: 27568 RVA: 0x001927C1 File Offset: 0x001909C1
		public static float EaseIn(float t, float b, float c, float d)
		{
			if (t != 0f)
			{
				return (float)((double)c * Math.Pow(2.0, (double)(10f * (t / d - 1f))) + (double)b);
			}
			return b;
		}

		// Token: 0x06006BB1 RID: 27569 RVA: 0x001927F2 File Offset: 0x001909F2
		public static float EaseOut(float t, float b, float c, float d)
		{
			if (t != d)
			{
				return (float)((double)c * (-(float)Math.Pow(2.0, (double)(-10f * t / d)) + 1.0) + (double)b);
			}
			return b + c;
		}

		// Token: 0x06006BB2 RID: 27570 RVA: 0x00192828 File Offset: 0x00190A28
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

	// Token: 0x02000E2D RID: 3629
	public class Linear
	{
		// Token: 0x06006BB4 RID: 27572 RVA: 0x001928C3 File Offset: 0x00190AC3
		public static float EaseNone(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}

		// Token: 0x06006BB5 RID: 27573 RVA: 0x001928CC File Offset: 0x00190ACC
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}

		// Token: 0x06006BB6 RID: 27574 RVA: 0x001928D5 File Offset: 0x00190AD5
		public static float EaseOut(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}

		// Token: 0x06006BB7 RID: 27575 RVA: 0x001928DE File Offset: 0x00190ADE
		public static float EaseInOut(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}
	}

	// Token: 0x02000E2E RID: 3630
	public class Quad
	{
		// Token: 0x06006BB9 RID: 27577 RVA: 0x001928EF File Offset: 0x00190AEF
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t + b;
		}

		// Token: 0x06006BBA RID: 27578 RVA: 0x001928FD File Offset: 0x00190AFD
		public static float EaseOut(float t, float b, float c, float d)
		{
			return -c * (t /= d) * (t - 2f) + b;
		}

		// Token: 0x06006BBB RID: 27579 RVA: 0x00192914 File Offset: 0x00190B14
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t + b;
			}
			return -c / 2f * ((t -= 1f) * (t - 2f) - 1f) + b;
		}
	}

	// Token: 0x02000E2F RID: 3631
	public class Quart
	{
		// Token: 0x06006BBD RID: 27581 RVA: 0x0019296D File Offset: 0x00190B6D
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t * t + b;
		}

		// Token: 0x06006BBE RID: 27582 RVA: 0x0019297F File Offset: 0x00190B7F
		public static float EaseOut(float t, float b, float c, float d)
		{
			return -c * ((t = t / d - 1f) * t * t * t - 1f) + b;
		}

		// Token: 0x06006BBF RID: 27583 RVA: 0x001929A0 File Offset: 0x00190BA0
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t * t + b;
			}
			return -c / 2f * ((t -= 2f) * t * t * t - 2f) + b;
		}
	}

	// Token: 0x02000E30 RID: 3632
	public class Quint
	{
		// Token: 0x06006BC1 RID: 27585 RVA: 0x001929FB File Offset: 0x00190BFB
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t * t * t + b;
		}

		// Token: 0x06006BC2 RID: 27586 RVA: 0x00192A0F File Offset: 0x00190C0F
		public static float EaseOut(float t, float b, float c, float d)
		{
			return c * ((t = t / d - 1f) * t * t * t * t + 1f) + b;
		}

		// Token: 0x06006BC3 RID: 27587 RVA: 0x00192A30 File Offset: 0x00190C30
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t * t * t + b;
			}
			return c / 2f * ((t -= 2f) * t * t * t * t + 2f) + b;
		}
	}

	// Token: 0x02000E31 RID: 3633
	public class Sine
	{
		// Token: 0x06006BC5 RID: 27589 RVA: 0x00192A8E File Offset: 0x00190C8E
		public static float EaseIn(float t, float b, float c, float d)
		{
			return (float)((double)(-(double)c) * Math.Cos((double)(t / d) * 1.5707963267948966) + (double)c + (double)b);
		}

		// Token: 0x06006BC6 RID: 27590 RVA: 0x00192AAE File Offset: 0x00190CAE
		public static float EaseOut(float t, float b, float c, float d)
		{
			return (float)((double)c * Math.Sin((double)(t / d) * 1.5707963267948966) + (double)b);
		}

		// Token: 0x06006BC7 RID: 27591 RVA: 0x00192ACA File Offset: 0x00190CCA
		public static float EaseInOut(float t, float b, float c, float d)
		{
			return (float)((double)(-(double)c / 2f) * (Math.Cos(3.141592653589793 * (double)t / (double)d) - 1.0) + (double)b);
		}
	}
}

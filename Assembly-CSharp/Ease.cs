using System;

// Token: 0x02000BDB RID: 3035
public class Ease
{
	// Token: 0x06005A40 RID: 23104 RVA: 0x00031640 File Offset: 0x0002F840
	public static float None(float t, float b, float c, float d)
	{
		return c * t / d + b;
	}

	// Token: 0x06005A41 RID: 23105 RVA: 0x0015525C File Offset: 0x0015345C
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

	// Token: 0x02000BDC RID: 3036
	public class Back
	{
		// Token: 0x06005A43 RID: 23107 RVA: 0x001555BC File Offset: 0x001537BC
		public static float EaseIn(float t, float b, float c, float d)
		{
			float num = 1.70158f;
			return c * (t /= d) * t * ((num + 1f) * t - num) + b;
		}

		// Token: 0x06005A44 RID: 23108 RVA: 0x001555E8 File Offset: 0x001537E8
		public static float EaseInSmall(float t, float b, float c, float d)
		{
			float num = 0.85079f;
			return c * (t /= d) * t * ((num + 1f) * t - num) + b;
		}

		// Token: 0x06005A45 RID: 23109 RVA: 0x00155614 File Offset: 0x00153814
		public static float EaseInLarge(float t, float b, float c, float d)
		{
			float num = 5.10474f;
			return c * (t /= d) * t * ((num + 1f) * t - num) + b;
		}

		// Token: 0x06005A46 RID: 23110 RVA: 0x00155640 File Offset: 0x00153840
		public static float EaseOut(float t, float b, float c, float d)
		{
			float num = 1.70158f;
			return c * ((t = t / d - 1f) * t * ((num + 1f) * t + num) + 1f) + b;
		}

		// Token: 0x06005A47 RID: 23111 RVA: 0x00155678 File Offset: 0x00153878
		public static float EaseOutSmall(float t, float b, float c, float d)
		{
			float num = 0.85079f;
			return c * ((t = t / d - 1f) * t * ((num + 1f) * t + num) + 1f) + b;
		}

		// Token: 0x06005A48 RID: 23112 RVA: 0x001556B0 File Offset: 0x001538B0
		public static float EaseOutLarge(float t, float b, float c, float d)
		{
			float num = 5.10474f;
			return c * ((t = t / d - 1f) * t * ((num + 1f) * t + num) + 1f) + b;
		}

		// Token: 0x06005A49 RID: 23113 RVA: 0x001556E8 File Offset: 0x001538E8
		public static float EaseInOut(float t, float b, float c, float d)
		{
			float num = 1.70158f;
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
			}
			return c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
		}

		// Token: 0x06005A4A RID: 23114 RVA: 0x00155760 File Offset: 0x00153960
		public static float EaseInOutLarge(float t, float b, float c, float d)
		{
			float num = 5.10474f;
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
			}
			return c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
		}

		// Token: 0x06005A4B RID: 23115 RVA: 0x001557D8 File Offset: 0x001539D8
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

	// Token: 0x02000BDD RID: 3037
	public class Bounce
	{
		// Token: 0x06005A4D RID: 23117 RVA: 0x00155850 File Offset: 0x00153A50
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

		// Token: 0x06005A4E RID: 23118 RVA: 0x00031649 File Offset: 0x0002F849
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c - Ease.Bounce.EaseOut(d - t, 0f, c, d) + b;
		}

		// Token: 0x06005A4F RID: 23119 RVA: 0x001558E0 File Offset: 0x00153AE0
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Ease.Bounce.EaseIn(t * 2f, 0f, c, d) * 0.5f + b;
			}
			return Ease.Bounce.EaseOut(t * 2f - d, 0f, c, d) * 0.5f + c * 0.5f + b;
		}
	}

	// Token: 0x02000BDE RID: 3038
	public class Circ
	{
		// Token: 0x06005A51 RID: 23121 RVA: 0x0003165E File Offset: 0x0002F85E
		public static float EaseIn(float t, float b, float c, float d)
		{
			return -c * (float)(Math.Sqrt((double)(1f - (t /= d) * t)) - 1.0) + b;
		}

		// Token: 0x06005A52 RID: 23122 RVA: 0x00031684 File Offset: 0x0002F884
		public static float EaseOut(float t, float b, float c, float d)
		{
			return c * (float)Math.Sqrt((double)(1f - (t = t / d - 1f) * t)) + b;
		}

		// Token: 0x06005A53 RID: 23123 RVA: 0x00155938 File Offset: 0x00153B38
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return -c / 2f * (float)(Math.Sqrt((double)(1f - t * t)) - 1.0) + b;
			}
			return c / 2f * (float)(Math.Sqrt((double)(1f - (t -= 2f) * t)) + 1.0) + b;
		}
	}

	// Token: 0x02000BDF RID: 3039
	public class Cubic
	{
		// Token: 0x06005A55 RID: 23125 RVA: 0x000316A5 File Offset: 0x0002F8A5
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t + b;
		}

		// Token: 0x06005A56 RID: 23126 RVA: 0x000316B5 File Offset: 0x0002F8B5
		public static float EaseOut(float t, float b, float c, float d)
		{
			return c * ((t = t / d - 1f) * t * t + 1f) + b;
		}

		// Token: 0x06005A57 RID: 23127 RVA: 0x001559AC File Offset: 0x00153BAC
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t + b;
			}
			return c / 2f * ((t -= 2f) * t * t + 2f) + b;
		}
	}

	// Token: 0x02000BE0 RID: 3040
	public class Elastic
	{
		// Token: 0x06005A59 RID: 23129 RVA: 0x001559FC File Offset: 0x00153BFC
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

		// Token: 0x06005A5A RID: 23130 RVA: 0x00155AB8 File Offset: 0x00153CB8
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

		// Token: 0x06005A5B RID: 23131 RVA: 0x00155B6C File Offset: 0x00153D6C
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

	// Token: 0x02000BE1 RID: 3041
	public class Expo
	{
		// Token: 0x06005A5D RID: 23133 RVA: 0x000316D1 File Offset: 0x0002F8D1
		public static float EaseIn(float t, float b, float c, float d)
		{
			if (t != 0f)
			{
				return (float)((double)c * Math.Pow(2.0, (double)(10f * (t / d - 1f))) + (double)b);
			}
			return b;
		}

		// Token: 0x06005A5E RID: 23134 RVA: 0x00031702 File Offset: 0x0002F902
		public static float EaseOut(float t, float b, float c, float d)
		{
			if (t != d)
			{
				return (float)((double)c * (-(float)Math.Pow(2.0, (double)(-10f * t / d)) + 1.0) + (double)b);
			}
			return b + c;
		}

		// Token: 0x06005A5F RID: 23135 RVA: 0x00155C8C File Offset: 0x00153E8C
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

	// Token: 0x02000BE2 RID: 3042
	public class Linear
	{
		// Token: 0x06005A61 RID: 23137 RVA: 0x00031640 File Offset: 0x0002F840
		public static float EaseNone(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}

		// Token: 0x06005A62 RID: 23138 RVA: 0x00031640 File Offset: 0x0002F840
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}

		// Token: 0x06005A63 RID: 23139 RVA: 0x00031640 File Offset: 0x0002F840
		public static float EaseOut(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}

		// Token: 0x06005A64 RID: 23140 RVA: 0x00031640 File Offset: 0x0002F840
		public static float EaseInOut(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}
	}

	// Token: 0x02000BE3 RID: 3043
	public class Quad
	{
		// Token: 0x06005A66 RID: 23142 RVA: 0x00031736 File Offset: 0x0002F936
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t + b;
		}

		// Token: 0x06005A67 RID: 23143 RVA: 0x00031744 File Offset: 0x0002F944
		public static float EaseOut(float t, float b, float c, float d)
		{
			return -c * (t /= d) * (t - 2f) + b;
		}

		// Token: 0x06005A68 RID: 23144 RVA: 0x00155D20 File Offset: 0x00153F20
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t + b;
			}
			return -c / 2f * ((t -= 1f) * (t - 2f) - 1f) + b;
		}
	}

	// Token: 0x02000BE4 RID: 3044
	public class Quart
	{
		// Token: 0x06005A6A RID: 23146 RVA: 0x00031759 File Offset: 0x0002F959
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t * t + b;
		}

		// Token: 0x06005A6B RID: 23147 RVA: 0x0003176B File Offset: 0x0002F96B
		public static float EaseOut(float t, float b, float c, float d)
		{
			return -c * ((t = t / d - 1f) * t * t * t - 1f) + b;
		}

		// Token: 0x06005A6C RID: 23148 RVA: 0x00155D74 File Offset: 0x00153F74
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t * t + b;
			}
			return -c / 2f * ((t -= 2f) * t * t * t - 2f) + b;
		}
	}

	// Token: 0x02000BE5 RID: 3045
	public class Quint
	{
		// Token: 0x06005A6E RID: 23150 RVA: 0x0003178A File Offset: 0x0002F98A
		public static float EaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t * t * t + b;
		}

		// Token: 0x06005A6F RID: 23151 RVA: 0x0003179E File Offset: 0x0002F99E
		public static float EaseOut(float t, float b, float c, float d)
		{
			return c * ((t = t / d - 1f) * t * t * t * t + 1f) + b;
		}

		// Token: 0x06005A70 RID: 23152 RVA: 0x00155DC8 File Offset: 0x00153FC8
		public static float EaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t * t * t + b;
			}
			return c / 2f * ((t -= 2f) * t * t * t * t + 2f) + b;
		}
	}

	// Token: 0x02000BE6 RID: 3046
	public class Sine
	{
		// Token: 0x06005A72 RID: 23154 RVA: 0x000317BE File Offset: 0x0002F9BE
		public static float EaseIn(float t, float b, float c, float d)
		{
			return (float)((double)(-(double)c) * Math.Cos((double)(t / d) * 1.5707963267948966) + (double)c + (double)b);
		}

		// Token: 0x06005A73 RID: 23155 RVA: 0x000317DE File Offset: 0x0002F9DE
		public static float EaseOut(float t, float b, float c, float d)
		{
			return (float)((double)c * Math.Sin((double)(t / d) * 1.5707963267948966) + (double)b);
		}

		// Token: 0x06005A74 RID: 23156 RVA: 0x000317FA File Offset: 0x0002F9FA
		public static float EaseInOut(float t, float b, float c, float d)
		{
			return (float)((double)(-(double)c / 2f) * (Math.Cos(3.141592653589793 * (double)t / (double)d) - 1.0) + (double)b);
		}
	}
}

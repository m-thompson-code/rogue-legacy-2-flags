using System;

namespace Unity.Screenshots
{
	// Token: 0x02000D0A RID: 3338
	public static class Downsampler
	{
		// Token: 0x06005F22 RID: 24354 RVA: 0x00164590 File Offset: 0x00162790
		public static byte[] Downsample(byte[] dataRgba, int stride, int maximumWidth, int maximumHeight, out int downsampledStride)
		{
			if (stride == 0)
			{
				throw new ArgumentException("The stride must be greater than 0.");
			}
			if (stride % 4 != 0)
			{
				throw new ArgumentException("The stride must be evenly divisible by 4.");
			}
			if (dataRgba == null)
			{
				throw new ArgumentNullException("dataRgba");
			}
			if (dataRgba.Length == 0)
			{
				throw new ArgumentException("The data length must be greater than 0.");
			}
			if (dataRgba.Length % 4 != 0)
			{
				throw new ArgumentException("The data must be evenly divisible by 4.");
			}
			if (dataRgba.Length % stride != 0)
			{
				throw new ArgumentException("The data must be evenly divisible by the stride.");
			}
			int num = stride / 4;
			int num2 = dataRgba.Length / stride;
			float val = (float)maximumWidth / (float)num;
			float val2 = (float)maximumHeight / (float)num2;
			float num3 = Math.Min(val, val2);
			if (num3 < 1f)
			{
				int num4 = (int)Math.Round((double)((float)num * num3));
				int num5 = (int)Math.Round((double)((float)num2 * num3));
				float[] array = new float[num4 * num5 * 4];
				float num6 = (float)num / (float)num4;
				float num7 = (float)num2 / (float)num5;
				int num8 = (int)Math.Floor((double)num6);
				int num9 = (int)Math.Floor((double)num7);
				int num10 = num8 * num9;
				for (int i = 0; i < num5; i++)
				{
					for (int j = 0; j < num4; j++)
					{
						int num11 = i * num4 * 4 + j * 4;
						int num12 = (int)Math.Floor((double)((float)j * num6));
						int num13 = (int)Math.Floor((double)((float)i * num7));
						int num14 = num12 + num8;
						int num15 = num13 + num9;
						for (int k = num13; k < num15; k++)
						{
							if (k < num2)
							{
								for (int l = num12; l < num14; l++)
								{
									if (l < num)
									{
										int num16 = k * num * 4 + l * 4;
										array[num11] += (float)dataRgba[num16];
										array[num11 + 1] += (float)dataRgba[num16 + 1];
										array[num11 + 2] += (float)dataRgba[num16 + 2];
										array[num11 + 3] += (float)dataRgba[num16 + 3];
									}
								}
							}
						}
						array[num11] /= (float)num10;
						array[num11 + 1] /= (float)num10;
						array[num11 + 2] /= (float)num10;
						array[num11 + 3] /= (float)num10;
					}
				}
				byte[] array2 = new byte[num4 * num5 * 4];
				for (int m = 0; m < num5; m++)
				{
					for (int n = 0; n < num4; n++)
					{
						int num17 = (num5 - 1 - m) * num4 * 4 + n * 4;
						int num18 = m * num4 * 4 + n * 4;
						byte[] array3 = array2;
						int num19 = num18;
						array3[num19] += (byte)array[num17];
						byte[] array4 = array2;
						int num20 = num18 + 1;
						array4[num20] += (byte)array[num17 + 1];
						byte[] array5 = array2;
						int num21 = num18 + 2;
						array5[num21] += (byte)array[num17 + 2];
						byte[] array6 = array2;
						int num22 = num18 + 3;
						array6[num22] += (byte)array[num17 + 3];
					}
				}
				downsampledStride = num4 * 4;
				return array2;
			}
			byte[] array7 = new byte[dataRgba.Length];
			for (int num23 = 0; num23 < num2; num23++)
			{
				for (int num24 = 0; num24 < num; num24++)
				{
					int num25 = (num2 - 1 - num23) * num * 4 + num24 * 4;
					int num26 = num23 * num * 4 + num24 * 4;
					byte[] array8 = array7;
					int num27 = num26;
					array8[num27] += dataRgba[num25];
					byte[] array9 = array7;
					int num28 = num26 + 1;
					array9[num28] += dataRgba[num25 + 1];
					byte[] array10 = array7;
					int num29 = num26 + 2;
					array10[num29] += dataRgba[num25 + 2];
					byte[] array11 = array7;
					int num30 = num26 + 3;
					array11[num30] += dataRgba[num25 + 3];
				}
			}
			downsampledStride = num * 4;
			return array7;
		}
	}
}

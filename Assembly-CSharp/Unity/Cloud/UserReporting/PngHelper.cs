using System;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000D1B RID: 3355
	public static class PngHelper
	{
		// Token: 0x06005FAC RID: 24492 RVA: 0x00034BEC File Offset: 0x00032DEC
		public static int GetPngHeightFromBase64Data(string data)
		{
			if (data == null || data.Length < 32)
			{
				return 0;
			}
			byte[] array = PngHelper.Slice(Convert.FromBase64String(data.Substring(0, 32)), 20, 24);
			Array.Reverse(array);
			return BitConverter.ToInt32(array, 0);
		}

		// Token: 0x06005FAD RID: 24493 RVA: 0x00034C20 File Offset: 0x00032E20
		public static int GetPngWidthFromBase64Data(string data)
		{
			if (data == null || data.Length < 32)
			{
				return 0;
			}
			byte[] array = PngHelper.Slice(Convert.FromBase64String(data.Substring(0, 32)), 16, 20);
			Array.Reverse(array);
			return BitConverter.ToInt32(array, 0);
		}

		// Token: 0x06005FAE RID: 24494 RVA: 0x001656EC File Offset: 0x001638EC
		private static byte[] Slice(byte[] source, int start, int end)
		{
			if (end < 0)
			{
				end = source.Length + end;
			}
			int num = end - start;
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = source[i + start];
			}
			return array;
		}
	}
}

using System;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000836 RID: 2102
	public static class PngHelper
	{
		// Token: 0x06004576 RID: 17782 RVA: 0x000F7C6F File Offset: 0x000F5E6F
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

		// Token: 0x06004577 RID: 17783 RVA: 0x000F7CA3 File Offset: 0x000F5EA3
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

		// Token: 0x06004578 RID: 17784 RVA: 0x000F7CD8 File Offset: 0x000F5ED8
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

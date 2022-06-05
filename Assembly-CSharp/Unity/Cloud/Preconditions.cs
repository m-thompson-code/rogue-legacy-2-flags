using System;

namespace Unity.Cloud
{
	// Token: 0x02000831 RID: 2097
	public static class Preconditions
	{
		// Token: 0x06004554 RID: 17748 RVA: 0x000F7914 File Offset: 0x000F5B14
		public static void ArgumentIsLessThanOrEqualToLength(object value, int length, string argumentName)
		{
			string text = value as string;
			if (text != null && text.Length > length)
			{
				throw new ArgumentException(argumentName);
			}
		}

		// Token: 0x06004555 RID: 17749 RVA: 0x000F793C File Offset: 0x000F5B3C
		public static void ArgumentNotNullOrWhitespace(object value, string argumentName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(argumentName);
			}
			string text = value as string;
			if (text != null && text.Trim() == string.Empty)
			{
				throw new ArgumentNullException(argumentName);
			}
		}
	}
}

using System;

namespace Unity.Cloud
{
	// Token: 0x02000D16 RID: 3350
	public static class Preconditions
	{
		// Token: 0x06005F8A RID: 24458 RVA: 0x001654BC File Offset: 0x001636BC
		public static void ArgumentIsLessThanOrEqualToLength(object value, int length, string argumentName)
		{
			string text = value as string;
			if (text != null && text.Length > length)
			{
				throw new ArgumentException(argumentName);
			}
		}

		// Token: 0x06005F8B RID: 24459 RVA: 0x001654E4 File Offset: 0x001636E4
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

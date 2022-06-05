using System;

// Token: 0x02000C0A RID: 3082
public static class LanguageType_RL
{
	// Token: 0x17001E35 RID: 7733
	// (get) Token: 0x06005A92 RID: 23186 RVA: 0x00031A38 File Offset: 0x0002FC38
	public static LanguageType[] TypeArray
	{
		get
		{
			if (LanguageType_RL.m_typeArray == null)
			{
				LanguageType_RL.m_typeArray = (Enum.GetValues(typeof(LanguageType)) as LanguageType[]);
			}
			return LanguageType_RL.m_typeArray;
		}
	}

	// Token: 0x04004770 RID: 18288
	private static LanguageType[] m_typeArray;
}

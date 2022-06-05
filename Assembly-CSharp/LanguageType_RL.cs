using System;

// Token: 0x0200074C RID: 1868
public static class LanguageType_RL
{
	// Token: 0x17001639 RID: 5689
	// (get) Token: 0x06004115 RID: 16661 RVA: 0x000E66C3 File Offset: 0x000E48C3
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

	// Token: 0x040034F4 RID: 13556
	private static LanguageType[] m_typeArray;
}

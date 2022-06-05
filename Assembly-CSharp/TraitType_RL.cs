using System;

// Token: 0x02000C4B RID: 3147
public static class TraitType_RL
{
	// Token: 0x17001E48 RID: 7752
	// (get) Token: 0x06005ADB RID: 23259 RVA: 0x00031DAF File Offset: 0x0002FFAF
	public static TraitType[] TypeArray
	{
		get
		{
			if (TraitType_RL.m_typeArray == null)
			{
				TraitType_RL.m_typeArray = (Enum.GetValues(typeof(TraitType)) as TraitType[]);
			}
			return TraitType_RL.m_typeArray;
		}
	}

	// Token: 0x04004BA9 RID: 19369
	private static TraitType[] m_typeArray;
}

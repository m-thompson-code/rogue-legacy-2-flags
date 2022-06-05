using System;

// Token: 0x02000C20 RID: 3104
public class RelicModType_RL
{
	// Token: 0x17001E3A RID: 7738
	// (get) Token: 0x06005AA6 RID: 23206 RVA: 0x00031AFB File Offset: 0x0002FCFB
	public static RelicModType[] TypeArray
	{
		get
		{
			if (RelicModType_RL.m_typeArray == null)
			{
				RelicModType_RL.m_typeArray = (Enum.GetValues(typeof(RelicModType)) as RelicModType[]);
			}
			return RelicModType_RL.m_typeArray;
		}
	}

	// Token: 0x04004824 RID: 18468
	private static RelicModType[] m_typeArray;
}

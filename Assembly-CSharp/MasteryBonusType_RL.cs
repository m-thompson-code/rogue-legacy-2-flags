using System;

// Token: 0x02000755 RID: 1877
public class MasteryBonusType_RL
{
	// Token: 0x1700163B RID: 5691
	// (get) Token: 0x0600411B RID: 16667 RVA: 0x000E6CFE File Offset: 0x000E4EFE
	public static MasteryBonusType[] TypeArray
	{
		get
		{
			if (MasteryBonusType_RL.m_typeArray == null)
			{
				MasteryBonusType_RL.m_typeArray = (Enum.GetValues(typeof(MasteryBonusType)) as MasteryBonusType[]);
			}
			return MasteryBonusType_RL.m_typeArray;
		}
	}

	// Token: 0x0400355C RID: 13660
	private static MasteryBonusType[] m_typeArray;
}

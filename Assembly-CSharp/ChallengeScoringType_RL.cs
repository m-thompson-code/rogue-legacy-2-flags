using System;

// Token: 0x02000712 RID: 1810
public class ChallengeScoringType_RL
{
	// Token: 0x17001627 RID: 5671
	// (get) Token: 0x060040DF RID: 16607 RVA: 0x000E5A39 File Offset: 0x000E3C39
	public static ChallengeScoringType[] TypeArray
	{
		get
		{
			if (ChallengeScoringType_RL.m_typeArray == null)
			{
				ChallengeScoringType_RL.m_typeArray = (Enum.GetValues(typeof(ChallengeScoringType)) as ChallengeScoringType[]);
			}
			return ChallengeScoringType_RL.m_typeArray;
		}
	}

	// Token: 0x04003306 RID: 13062
	private static ChallengeScoringType[] m_typeArray;
}

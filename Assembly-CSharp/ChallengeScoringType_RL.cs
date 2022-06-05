using System;

// Token: 0x02000BC5 RID: 3013
public class ChallengeScoringType_RL
{
	// Token: 0x17001E23 RID: 7715
	// (get) Token: 0x06005A28 RID: 23080 RVA: 0x000314CA File Offset: 0x0002F6CA
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

	// Token: 0x04004581 RID: 17793
	private static ChallengeScoringType[] m_typeArray;
}

using System;

// Token: 0x02000714 RID: 1812
public class ChallengeType_RL
{
	// Token: 0x17001628 RID: 5672
	// (get) Token: 0x060040E1 RID: 16609 RVA: 0x000E5A68 File Offset: 0x000E3C68
	public static ChallengeType[] TypeArray
	{
		get
		{
			if (ChallengeType_RL.m_typeArray == null)
			{
				ChallengeType_RL.m_typeArray = (Enum.GetValues(typeof(ChallengeType)) as ChallengeType[]);
			}
			return ChallengeType_RL.m_typeArray;
		}
	}

	// Token: 0x04003320 RID: 13088
	private static ChallengeType[] m_typeArray;
}

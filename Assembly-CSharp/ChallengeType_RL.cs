using System;

// Token: 0x02000BC7 RID: 3015
public class ChallengeType_RL
{
	// Token: 0x17001E24 RID: 7716
	// (get) Token: 0x06005A2A RID: 23082 RVA: 0x000314F1 File Offset: 0x0002F6F1
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

	// Token: 0x0400459B RID: 17819
	private static ChallengeType[] m_typeArray;
}

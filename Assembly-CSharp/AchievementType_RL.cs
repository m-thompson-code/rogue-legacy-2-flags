using System;

// Token: 0x02000BB0 RID: 2992
public class AchievementType_RL
{
	// Token: 0x17001E16 RID: 7702
	// (get) Token: 0x06005A06 RID: 23046 RVA: 0x0003125A File Offset: 0x0002F45A
	public static AchievementType[] TypeArray
	{
		get
		{
			if (AchievementType_RL.m_typeArray == null)
			{
				AchievementType_RL.m_typeArray = (Enum.GetValues(typeof(AchievementType)) as AchievementType[]);
			}
			return AchievementType_RL.m_typeArray;
		}
	}

	// Token: 0x040044F6 RID: 17654
	private static AchievementType[] m_typeArray;
}

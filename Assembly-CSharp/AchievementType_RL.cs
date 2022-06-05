using System;

// Token: 0x020006FD RID: 1789
public class AchievementType_RL
{
	// Token: 0x1700161A RID: 5658
	// (get) Token: 0x060040BD RID: 16573 RVA: 0x000E56BF File Offset: 0x000E38BF
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

	// Token: 0x0400327B RID: 12923
	private static AchievementType[] m_typeArray;
}

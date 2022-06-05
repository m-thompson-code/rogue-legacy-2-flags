using System;

// Token: 0x02000772 RID: 1906
public class SkillTreeType_RL
{
	// Token: 0x17001647 RID: 5703
	// (get) Token: 0x0600414D RID: 16717 RVA: 0x000E8357 File Offset: 0x000E6557
	public static SkillTreeType[] TypeArray
	{
		get
		{
			if (SkillTreeType_RL.m_typeArray == null)
			{
				SkillTreeType_RL.m_typeArray = (Enum.GetValues(typeof(SkillTreeType)) as SkillTreeType[]);
			}
			return SkillTreeType_RL.m_typeArray;
		}
	}

	// Token: 0x0400370E RID: 14094
	private static SkillTreeType[] m_typeArray;
}

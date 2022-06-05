using System;

// Token: 0x02000C33 RID: 3123
public class SkillTreeType_RL
{
	// Token: 0x17001E43 RID: 7747
	// (get) Token: 0x06005ACA RID: 23242 RVA: 0x00031CEC File Offset: 0x0002FEEC
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

	// Token: 0x040049BE RID: 18878
	private static SkillTreeType[] m_typeArray;
}

using System;

// Token: 0x0200023E RID: 574
public class ProjectileBuffType_RL
{
	// Token: 0x17000B41 RID: 2881
	// (get) Token: 0x0600170E RID: 5902 RVA: 0x00047E22 File Offset: 0x00046022
	public static ProjectileBuffType[] TypeArray
	{
		get
		{
			if (ProjectileBuffType_RL.m_typeArray == null)
			{
				ProjectileBuffType_RL.m_typeArray = (Enum.GetValues(typeof(ProjectileBuffType)) as ProjectileBuffType[]);
			}
			return ProjectileBuffType_RL.m_typeArray;
		}
	}

	// Token: 0x0400168F RID: 5775
	private static ProjectileBuffType[] m_typeArray;
}

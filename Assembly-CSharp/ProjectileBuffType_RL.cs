using System;

// Token: 0x020003FB RID: 1019
public class ProjectileBuffType_RL
{
	// Token: 0x17000E6E RID: 3694
	// (get) Token: 0x060020C1 RID: 8385 RVA: 0x000115EC File Offset: 0x0000F7EC
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

	// Token: 0x04001DA7 RID: 7591
	private static ProjectileBuffType[] m_typeArray;
}

using System;

// Token: 0x02000718 RID: 1816
public static class ClassType_RL
{
	// Token: 0x1700162A RID: 5674
	// (get) Token: 0x060040E6 RID: 16614 RVA: 0x000E5AF8 File Offset: 0x000E3CF8
	public static ClassType[] TypeArray
	{
		get
		{
			if (ClassType_RL.m_typeArray == null)
			{
				ClassType_RL.m_typeArray = (Enum.GetValues(typeof(ClassType)) as ClassType[]);
			}
			return ClassType_RL.m_typeArray;
		}
	}

	// Token: 0x04003342 RID: 13122
	private static ClassType[] m_typeArray;
}

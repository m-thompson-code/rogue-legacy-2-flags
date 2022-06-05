using System;

// Token: 0x02000BCB RID: 3019
public static class ClassType_RL
{
	// Token: 0x17001E26 RID: 7718
	// (get) Token: 0x06005A2F RID: 23087 RVA: 0x00031571 File Offset: 0x0002F771
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

	// Token: 0x040045BD RID: 17853
	private static ClassType[] m_typeArray;
}

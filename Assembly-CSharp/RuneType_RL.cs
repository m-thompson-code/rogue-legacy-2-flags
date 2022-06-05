using System;

// Token: 0x0200076A RID: 1898
public class RuneType_RL
{
	// Token: 0x17001646 RID: 5702
	// (get) Token: 0x06004147 RID: 16711 RVA: 0x000E819A File Offset: 0x000E639A
	public static RuneType[] TypeArray
	{
		get
		{
			if (RuneType_RL.m_typeArray == null)
			{
				RuneType_RL.m_typeArray = (Enum.GetValues(typeof(RuneType)) as RuneType[]);
			}
			return RuneType_RL.m_typeArray;
		}
	}

	// Token: 0x04003671 RID: 13937
	private static RuneType[] m_typeArray;
}

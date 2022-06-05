using System;

// Token: 0x0200070C RID: 1804
public class BurdenType_RL
{
	// Token: 0x17001624 RID: 5668
	// (get) Token: 0x060040DA RID: 16602 RVA: 0x000E59EA File Offset: 0x000E3BEA
	public static BurdenType[] TypeArray
	{
		get
		{
			if (BurdenType_RL.m_typeArray == null)
			{
				BurdenType_RL.m_typeArray = (Enum.GetValues(typeof(BurdenType)) as BurdenType[]);
			}
			return BurdenType_RL.m_typeArray;
		}
	}

	// Token: 0x040032F1 RID: 13041
	private static BurdenType[] m_typeArray;
}

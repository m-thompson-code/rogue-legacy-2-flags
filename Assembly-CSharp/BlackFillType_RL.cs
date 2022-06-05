using System;

// Token: 0x02000706 RID: 1798
public class BlackFillType_RL
{
	// Token: 0x17001622 RID: 5666
	// (get) Token: 0x060040D2 RID: 16594 RVA: 0x000E582C File Offset: 0x000E3A2C
	public static BlackFillType[] TypeArray
	{
		get
		{
			if (BlackFillType_RL.m_typeArray == null)
			{
				BlackFillType_RL.m_typeArray = (Enum.GetValues(typeof(BlackFillType)) as BlackFillType[]);
			}
			return BlackFillType_RL.m_typeArray;
		}
	}

	// Token: 0x040032C2 RID: 12994
	private static BlackFillType[] m_typeArray;
}

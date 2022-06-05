using System;

// Token: 0x02000BBF RID: 3007
public class BurdenType_RL
{
	// Token: 0x17001E20 RID: 7712
	// (get) Token: 0x06005A23 RID: 23075 RVA: 0x00031483 File Offset: 0x0002F683
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

	// Token: 0x0400456C RID: 17772
	private static BurdenType[] m_typeArray;
}

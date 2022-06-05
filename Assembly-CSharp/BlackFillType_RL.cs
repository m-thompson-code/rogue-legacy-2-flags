using System;

// Token: 0x02000BB9 RID: 3001
public class BlackFillType_RL
{
	// Token: 0x17001E1E RID: 7710
	// (get) Token: 0x06005A1B RID: 23067 RVA: 0x000313B7 File Offset: 0x0002F5B7
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

	// Token: 0x0400453D RID: 17725
	private static BlackFillType[] m_typeArray;
}

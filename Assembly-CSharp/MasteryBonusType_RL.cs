using System;

// Token: 0x02000C13 RID: 3091
public class MasteryBonusType_RL
{
	// Token: 0x17001E37 RID: 7735
	// (get) Token: 0x06005A98 RID: 23192 RVA: 0x00031A86 File Offset: 0x0002FC86
	public static MasteryBonusType[] TypeArray
	{
		get
		{
			if (MasteryBonusType_RL.m_typeArray == null)
			{
				MasteryBonusType_RL.m_typeArray = (Enum.GetValues(typeof(MasteryBonusType)) as MasteryBonusType[]);
			}
			return MasteryBonusType_RL.m_typeArray;
		}
	}

	// Token: 0x040047D8 RID: 18392
	private static MasteryBonusType[] m_typeArray;
}

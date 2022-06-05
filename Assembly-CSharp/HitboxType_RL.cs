using System;

// Token: 0x02000740 RID: 1856
public class HitboxType_RL
{
	// Token: 0x17001633 RID: 5683
	// (get) Token: 0x06004106 RID: 16646 RVA: 0x000E64B6 File Offset: 0x000E46B6
	public static HitboxType[] TypeArray
	{
		get
		{
			if (HitboxType_RL.m_hbTypeArray == null)
			{
				HitboxType_RL.m_hbTypeArray = (Enum.GetValues(typeof(HitboxType)) as HitboxType[]);
			}
			return HitboxType_RL.m_hbTypeArray;
		}
	}

	// Token: 0x04003486 RID: 13446
	private static HitboxType[] m_hbTypeArray;
}

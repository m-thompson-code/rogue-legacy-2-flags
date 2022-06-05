using System;

// Token: 0x02000BFE RID: 3070
public class HitboxType_RL
{
	// Token: 0x17001E2F RID: 7727
	// (get) Token: 0x06005A83 RID: 23171 RVA: 0x00031912 File Offset: 0x0002FB12
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

	// Token: 0x04004702 RID: 18178
	private static HitboxType[] m_hbTypeArray;
}

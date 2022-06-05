using System;

// Token: 0x02000C22 RID: 3106
public class RelicType_RL
{
	// Token: 0x17001E3B RID: 7739
	// (get) Token: 0x06005AA8 RID: 23208 RVA: 0x00031B22 File Offset: 0x0002FD22
	public static RelicType[] TypeArray
	{
		get
		{
			if (RelicType_RL.m_relicTypeArray == null)
			{
				RelicType_RL.m_relicTypeArray = (Enum.GetValues(typeof(RelicType)) as RelicType[]);
			}
			return RelicType_RL.m_relicTypeArray;
		}
	}

	// Token: 0x0400487A RID: 18554
	private static RelicType[] m_relicTypeArray;
}

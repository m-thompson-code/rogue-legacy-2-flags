using System;

// Token: 0x02000764 RID: 1892
public class RelicType_RL
{
	// Token: 0x1700163F RID: 5695
	// (get) Token: 0x0600412B RID: 16683 RVA: 0x000E728A File Offset: 0x000E548A
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

	// Token: 0x040035FE RID: 13822
	private static RelicType[] m_relicTypeArray;
}

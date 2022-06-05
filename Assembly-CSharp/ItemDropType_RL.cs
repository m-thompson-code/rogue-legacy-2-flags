using System;

// Token: 0x02000747 RID: 1863
public class ItemDropType_RL
{
	// Token: 0x17001635 RID: 5685
	// (get) Token: 0x0600410C RID: 16652 RVA: 0x000E6568 File Offset: 0x000E4768
	public static ItemDropType[] Types
	{
		get
		{
			if (ItemDropType_RL.m_types == null)
			{
				ItemDropType_RL.m_types = (Enum.GetValues(typeof(ItemDropType)) as ItemDropType[]);
			}
			return ItemDropType_RL.m_types;
		}
	}

	// Token: 0x17001636 RID: 5686
	// (get) Token: 0x0600410D RID: 16653 RVA: 0x000E658F File Offset: 0x000E478F
	public static SpecialItemType[] SpecialTypes
	{
		get
		{
			if (ItemDropType_RL.m_specialTypes == null)
			{
				ItemDropType_RL.m_specialTypes = (Enum.GetValues(typeof(SpecialItemType)) as SpecialItemType[]);
			}
			return ItemDropType_RL.m_specialTypes;
		}
	}

	// Token: 0x040034D5 RID: 13525
	public const int MAX_NUM_ITEM_DROPS = 10;

	// Token: 0x040034D6 RID: 13526
	private static ItemDropType[] m_types;

	// Token: 0x040034D7 RID: 13527
	private static SpecialItemType[] m_specialTypes;
}

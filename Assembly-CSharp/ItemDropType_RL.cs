using System;

// Token: 0x02000C05 RID: 3077
public class ItemDropType_RL
{
	// Token: 0x17001E31 RID: 7729
	// (get) Token: 0x06005A89 RID: 23177 RVA: 0x00031960 File Offset: 0x0002FB60
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

	// Token: 0x17001E32 RID: 7730
	// (get) Token: 0x06005A8A RID: 23178 RVA: 0x00031987 File Offset: 0x0002FB87
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

	// Token: 0x04004751 RID: 18257
	public const int MAX_NUM_ITEM_DROPS = 10;

	// Token: 0x04004752 RID: 18258
	private static ItemDropType[] m_types;

	// Token: 0x04004753 RID: 18259
	private static SpecialItemType[] m_specialTypes;
}

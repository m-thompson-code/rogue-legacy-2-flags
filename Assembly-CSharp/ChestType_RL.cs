using System;

// Token: 0x02000BC9 RID: 3017
public class ChestType_RL
{
	// Token: 0x17001E25 RID: 7717
	// (get) Token: 0x06005A2C RID: 23084 RVA: 0x00031518 File Offset: 0x0002F718
	public static ChestType[] TypeArray
	{
		get
		{
			if (ChestType_RL.m_typeArray == null)
			{
				ChestType_RL.m_typeArray = (Enum.GetValues(typeof(ChestType)) as ChestType[]);
			}
			return ChestType_RL.m_typeArray;
		}
	}

	// Token: 0x06005A2D RID: 23085 RVA: 0x0003153F File Offset: 0x0002F73F
	public static int GetChestRarity(ChestType chestType)
	{
		if (chestType <= ChestType.Gold)
		{
			if (chestType == ChestType.Bronze)
			{
				return 0;
			}
			if (chestType == ChestType.Silver)
			{
				return 1;
			}
			if (chestType == ChestType.Gold)
			{
				return 2;
			}
		}
		else if (chestType != ChestType.Fairy)
		{
			if (chestType == ChestType.Boss)
			{
				return 3;
			}
			if (chestType != ChestType.Black)
			{
			}
		}
		return 4;
	}

	// Token: 0x040045A4 RID: 17828
	private static ChestType[] m_typeArray;
}

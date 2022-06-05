using System;

// Token: 0x02000716 RID: 1814
public class ChestType_RL
{
	// Token: 0x17001629 RID: 5673
	// (get) Token: 0x060040E3 RID: 16611 RVA: 0x000E5A97 File Offset: 0x000E3C97
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

	// Token: 0x060040E4 RID: 16612 RVA: 0x000E5ABE File Offset: 0x000E3CBE
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

	// Token: 0x04003329 RID: 13097
	private static ChestType[] m_typeArray;
}

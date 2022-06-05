using System;

// Token: 0x020006D4 RID: 1748
public class EquipmentSetBonusType_RL
{
	// Token: 0x170015CB RID: 5579
	// (get) Token: 0x06003FCC RID: 16332 RVA: 0x000E287E File Offset: 0x000E0A7E
	public static EquipmentSetBonusType[] TypeArray
	{
		get
		{
			if (EquipmentSetBonusType_RL.m_typeArray == null)
			{
				EquipmentSetBonusType_RL.m_typeArray = (Enum.GetValues(typeof(EquipmentSetBonusType)) as EquipmentSetBonusType[]);
			}
			return EquipmentSetBonusType_RL.m_typeArray;
		}
	}

	// Token: 0x04003038 RID: 12344
	private static EquipmentSetBonusType[] m_typeArray;
}

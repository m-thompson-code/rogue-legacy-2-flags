using System;

// Token: 0x02000B7F RID: 2943
public class EquipmentSetBonusType_RL
{
	// Token: 0x17001DC3 RID: 7619
	// (get) Token: 0x06005903 RID: 22787 RVA: 0x000306D6 File Offset: 0x0002E8D6
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

	// Token: 0x04004287 RID: 17031
	private static EquipmentSetBonusType[] m_typeArray;
}

using System;
using UnityEngine;

// Token: 0x020006D5 RID: 1749
[Serializable]
public class EquipmentSetBonus
{
	// Token: 0x06003FCE RID: 16334 RVA: 0x000E28AD File Offset: 0x000E0AAD
	public EquipmentSetBonus(EquipmentSetBonusType bonusType, float statGain)
	{
		this.BonusType = bonusType;
		this.StatGain = statGain;
	}

	// Token: 0x170015CC RID: 5580
	// (get) Token: 0x06003FCF RID: 16335 RVA: 0x000E28C3 File Offset: 0x000E0AC3
	// (set) Token: 0x06003FD0 RID: 16336 RVA: 0x000E28CB File Offset: 0x000E0ACB
	public EquipmentSetBonusType BonusType
	{
		get
		{
			return this.m_bonusType;
		}
		private set
		{
			this.m_bonusType = value;
		}
	}

	// Token: 0x170015CD RID: 5581
	// (get) Token: 0x06003FD1 RID: 16337 RVA: 0x000E28D4 File Offset: 0x000E0AD4
	// (set) Token: 0x06003FD2 RID: 16338 RVA: 0x000E28DC File Offset: 0x000E0ADC
	public float StatGain
	{
		get
		{
			return this.m_statGain;
		}
		private set
		{
			this.m_statGain = value;
		}
	}

	// Token: 0x04003039 RID: 12345
	[SerializeField]
	private EquipmentSetBonusType m_bonusType;

	// Token: 0x0400303A RID: 12346
	[SerializeField]
	private float m_statGain;
}

using System;
using UnityEngine;

// Token: 0x02000B80 RID: 2944
[Serializable]
public class EquipmentSetBonus
{
	// Token: 0x06005905 RID: 22789 RVA: 0x000306FD File Offset: 0x0002E8FD
	public EquipmentSetBonus(EquipmentSetBonusType bonusType, float statGain)
	{
		this.BonusType = bonusType;
		this.StatGain = statGain;
	}

	// Token: 0x17001DC4 RID: 7620
	// (get) Token: 0x06005906 RID: 22790 RVA: 0x00030713 File Offset: 0x0002E913
	// (set) Token: 0x06005907 RID: 22791 RVA: 0x0003071B File Offset: 0x0002E91B
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

	// Token: 0x17001DC5 RID: 7621
	// (get) Token: 0x06005908 RID: 22792 RVA: 0x00030724 File Offset: 0x0002E924
	// (set) Token: 0x06005909 RID: 22793 RVA: 0x0003072C File Offset: 0x0002E92C
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

	// Token: 0x04004288 RID: 17032
	[SerializeField]
	private EquipmentSetBonusType m_bonusType;

	// Token: 0x04004289 RID: 17033
	[SerializeField]
	private float m_statGain;
}

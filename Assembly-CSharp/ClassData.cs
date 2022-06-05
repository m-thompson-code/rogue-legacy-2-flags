using System;
using UnityEngine;

// Token: 0x02000B73 RID: 2931
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Class Data")]
public class ClassData : ScriptableObject
{
	// Token: 0x17001DBA RID: 7610
	// (get) Token: 0x060058EA RID: 22762 RVA: 0x000305F7 File Offset: 0x0002E7F7
	public ClassPassiveData PassiveData
	{
		get
		{
			return this.m_passiveData;
		}
	}

	// Token: 0x17001DBB RID: 7611
	// (get) Token: 0x060058EB RID: 22763 RVA: 0x000305FF File Offset: 0x0002E7FF
	public ClassWeaponData WeaponData
	{
		get
		{
			return this.m_weaponData;
		}
	}

	// Token: 0x17001DBC RID: 7612
	// (get) Token: 0x060058EC RID: 22764 RVA: 0x00030607 File Offset: 0x0002E807
	public ClassSpellData SpellData
	{
		get
		{
			return this.m_spellData;
		}
	}

	// Token: 0x17001DBD RID: 7613
	// (get) Token: 0x060058ED RID: 22765 RVA: 0x0003060F File Offset: 0x0002E80F
	public ClassTalentData TalentData
	{
		get
		{
			return this.m_talentData;
		}
	}

	// Token: 0x040041CD RID: 16845
	[SerializeField]
	private ClassStatsData m_statsData;

	// Token: 0x040041CE RID: 16846
	[SerializeField]
	private ClassPassiveData m_passiveData;

	// Token: 0x040041CF RID: 16847
	[SerializeField]
	private ClassWeaponData m_weaponData;

	// Token: 0x040041D0 RID: 16848
	[SerializeField]
	private ClassSpellData m_spellData;

	// Token: 0x040041D1 RID: 16849
	[SerializeField]
	private ClassTalentData m_talentData;
}

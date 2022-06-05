using System;
using UnityEngine;

// Token: 0x020006C8 RID: 1736
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Class Data")]
public class ClassData : ScriptableObject
{
	// Token: 0x170015C2 RID: 5570
	// (get) Token: 0x06003FB3 RID: 16307 RVA: 0x000E2568 File Offset: 0x000E0768
	public ClassPassiveData PassiveData
	{
		get
		{
			return this.m_passiveData;
		}
	}

	// Token: 0x170015C3 RID: 5571
	// (get) Token: 0x06003FB4 RID: 16308 RVA: 0x000E2570 File Offset: 0x000E0770
	public ClassWeaponData WeaponData
	{
		get
		{
			return this.m_weaponData;
		}
	}

	// Token: 0x170015C4 RID: 5572
	// (get) Token: 0x06003FB5 RID: 16309 RVA: 0x000E2578 File Offset: 0x000E0778
	public ClassSpellData SpellData
	{
		get
		{
			return this.m_spellData;
		}
	}

	// Token: 0x170015C5 RID: 5573
	// (get) Token: 0x06003FB6 RID: 16310 RVA: 0x000E2580 File Offset: 0x000E0780
	public ClassTalentData TalentData
	{
		get
		{
			return this.m_talentData;
		}
	}

	// Token: 0x04002F7E RID: 12158
	[SerializeField]
	private ClassStatsData m_statsData;

	// Token: 0x04002F7F RID: 12159
	[SerializeField]
	private ClassPassiveData m_passiveData;

	// Token: 0x04002F80 RID: 12160
	[SerializeField]
	private ClassWeaponData m_weaponData;

	// Token: 0x04002F81 RID: 12161
	[SerializeField]
	private ClassSpellData m_spellData;

	// Token: 0x04002F82 RID: 12162
	[SerializeField]
	private ClassTalentData m_talentData;
}

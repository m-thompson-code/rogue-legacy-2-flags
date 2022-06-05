using System;
using UnityEngine;

// Token: 0x02000B5C RID: 2908
public class AbilityData : ScriptableObject
{
	// Token: 0x04004141 RID: 16705
	public string Name;

	// Token: 0x04004142 RID: 16706
	public CastAbilityType Type;

	// Token: 0x04004143 RID: 16707
	public int BaseCost;

	// Token: 0x04004144 RID: 16708
	public int MaxAmmo;

	// Token: 0x04004145 RID: 16709
	public float CooldownTime;

	// Token: 0x04004146 RID: 16710
	public float LockoutTime;

	// Token: 0x04004147 RID: 16711
	public bool CooldownDecreaseOverTime;

	// Token: 0x04004148 RID: 16712
	public bool CooldownDecreasePerHit;

	// Token: 0x04004149 RID: 16713
	public bool ManaGainPerHit;

	// Token: 0x0400414A RID: 16714
	public bool CooldownRefreshesAllAmmo;

	// Token: 0x0400414B RID: 16715
	public bool LockDirection;

	// Token: 0x0400414C RID: 16716
	public float MovementMod;

	// Token: 0x0400414D RID: 16717
	public float AirMovementMod;

	// Token: 0x0400414E RID: 16718
	public bool CanCastWhileDashing;

	// Token: 0x0400414F RID: 16719
	public bool AttackCancel;

	// Token: 0x04004150 RID: 16720
	public bool SpellCancel;

	// Token: 0x04004151 RID: 16721
	public bool TalentCancel;

	// Token: 0x04004152 RID: 16722
	public bool DashCancel;

	// Token: 0x04004153 RID: 16723
	[Tooltip("This is a misnomer. It should be called 'CanJumpWhileCasting' as it does not actually cancel the current ability.")]
	public bool JumpCancel;

	// Token: 0x04004154 RID: 16724
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04004155 RID: 16725
	public string Controls;

	// Token: 0x04004156 RID: 16726
	public string Description;
}

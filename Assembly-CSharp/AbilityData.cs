using System;
using UnityEngine;

// Token: 0x020006B4 RID: 1716
public class AbilityData : ScriptableObject
{
	// Token: 0x04002EF8 RID: 12024
	public string Name;

	// Token: 0x04002EF9 RID: 12025
	public CastAbilityType Type;

	// Token: 0x04002EFA RID: 12026
	public int BaseCost;

	// Token: 0x04002EFB RID: 12027
	public int MaxAmmo;

	// Token: 0x04002EFC RID: 12028
	public float CooldownTime;

	// Token: 0x04002EFD RID: 12029
	public float LockoutTime;

	// Token: 0x04002EFE RID: 12030
	public bool CooldownDecreaseOverTime;

	// Token: 0x04002EFF RID: 12031
	public bool CooldownDecreasePerHit;

	// Token: 0x04002F00 RID: 12032
	public bool ManaGainPerHit;

	// Token: 0x04002F01 RID: 12033
	public bool CooldownRefreshesAllAmmo;

	// Token: 0x04002F02 RID: 12034
	public bool LockDirection;

	// Token: 0x04002F03 RID: 12035
	public float MovementMod;

	// Token: 0x04002F04 RID: 12036
	public float AirMovementMod;

	// Token: 0x04002F05 RID: 12037
	public bool CanCastWhileDashing;

	// Token: 0x04002F06 RID: 12038
	public bool AttackCancel;

	// Token: 0x04002F07 RID: 12039
	public bool SpellCancel;

	// Token: 0x04002F08 RID: 12040
	public bool TalentCancel;

	// Token: 0x04002F09 RID: 12041
	public bool DashCancel;

	// Token: 0x04002F0A RID: 12042
	[Tooltip("This is a misnomer. It should be called 'CanJumpWhileCasting' as it does not actually cancel the current ability.")]
	public bool JumpCancel;

	// Token: 0x04002F0B RID: 12043
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04002F0C RID: 12044
	public string Controls;

	// Token: 0x04002F0D RID: 12045
	public string Description;
}

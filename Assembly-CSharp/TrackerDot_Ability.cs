using System;
using UnityEngine;

// Token: 0x0200017A RID: 378
public class TrackerDot_Ability : FollowTargetAbility_RL
{
	// Token: 0x17000732 RID: 1842
	// (get) Token: 0x06000D4F RID: 3407 RVA: 0x00028A25 File Offset: 0x00026C25
	protected override AbilityAnimState StateToHoldAttackOn
	{
		get
		{
			return AbilityAnimState.Tell;
		}
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x00028A28 File Offset: 0x00026C28
	protected override void FireProjectile()
	{
		if (!string.IsNullOrEmpty(this.ProjectileName))
		{
			Vector2 offset = this.m_targetGO.transform.position - this.m_abilityController.PlayerController.transform.position;
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, offset, false, 0f, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			this.ApplyAbilityCosts();
		}
	}
}

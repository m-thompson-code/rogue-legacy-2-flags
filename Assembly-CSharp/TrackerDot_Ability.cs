using System;
using UnityEngine;

// Token: 0x020002BC RID: 700
public class TrackerDot_Ability : FollowTargetAbility_RL
{
	// Token: 0x170009A2 RID: 2466
	// (get) Token: 0x060014A6 RID: 5286 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override AbilityAnimState StateToHoldAttackOn
	{
		get
		{
			return AbilityAnimState.Tell;
		}
	}

	// Token: 0x060014A7 RID: 5287 RVA: 0x00087AF0 File Offset: 0x00085CF0
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

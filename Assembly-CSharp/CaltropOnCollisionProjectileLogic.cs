using System;
using UnityEngine;

// Token: 0x0200079B RID: 1947
public class CaltropOnCollisionProjectileLogic : OnCollisionSpawnProjectileLogic
{
	// Token: 0x06003B7E RID: 15230 RVA: 0x00020AED File Offset: 0x0001ECED
	protected override void Awake()
	{
		base.Awake();
		base.SourceProjectile.CastAbilityType = CastAbilityType.Talent;
	}

	// Token: 0x06003B7F RID: 15231 RVA: 0x000F3EC0 File Offset: 0x000F20C0
	protected override void SpawnProjectile(Projectile_RL projectile, GameObject colliderObj)
	{
		Vector3 vector = base.SourceProjectile.Midpoint;
		if (base.SourceProjectile.HitboxController.LastCollidedWith != null)
		{
			vector = base.SourceProjectile.HitboxController.LastCollidedWith.ClosestPoint(base.SourceProjectile.Midpoint);
		}
		Vector2 point = vector;
		point.y += base.SourceProjectile.transform.localScale.y * 0.5f;
		if (vector.y > base.SourceProjectile.Midpoint.y && base.SourceProjectile.HitboxController.LastCollidedWith.OverlapPoint(point))
		{
			return;
		}
		base.SpawnProjectile(projectile, colliderObj);
		base.SourceProjectile.FlagForDestruction(null);
	}
}

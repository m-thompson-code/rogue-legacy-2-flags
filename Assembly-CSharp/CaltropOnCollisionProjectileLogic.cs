using System;
using UnityEngine;

// Token: 0x02000497 RID: 1175
public class CaltropOnCollisionProjectileLogic : OnCollisionSpawnProjectileLogic
{
	// Token: 0x06002B2F RID: 11055 RVA: 0x00092458 File Offset: 0x00090658
	protected override void Awake()
	{
		base.Awake();
		base.SourceProjectile.CastAbilityType = CastAbilityType.Talent;
	}

	// Token: 0x06002B30 RID: 11056 RVA: 0x0009246C File Offset: 0x0009066C
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

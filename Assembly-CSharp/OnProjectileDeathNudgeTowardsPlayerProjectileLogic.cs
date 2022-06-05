using System;
using UnityEngine;

// Token: 0x020007AF RID: 1967
public class OnProjectileDeathNudgeTowardsPlayerProjectileLogic : OnProjectileDeathProjectileLogic
{
	// Token: 0x06003BE5 RID: 15333 RVA: 0x000F5000 File Offset: 0x000F3200
	protected override void SpawnProjectile(Projectile_RL projectile, GameObject colliderObj)
	{
		Vector2 offset = base.Offset;
		if (PlayerManager.IsInstantiated)
		{
			GameObject player = PlayerManager.GetPlayer();
			if (player)
			{
				base.Offset += 0.5f * CDGHelper.AngleToVector(CDGHelper.AngleBetweenPts(base.SourceProjectile.Midpoint, player.transform.position));
			}
		}
		base.SpawnProjectile(projectile, colliderObj);
		base.Offset = offset;
	}
}

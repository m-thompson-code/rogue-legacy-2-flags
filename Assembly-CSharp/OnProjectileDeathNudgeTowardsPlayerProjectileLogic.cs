using System;
using UnityEngine;

// Token: 0x020004A4 RID: 1188
public class OnProjectileDeathNudgeTowardsPlayerProjectileLogic : OnProjectileDeathProjectileLogic
{
	// Token: 0x06002B78 RID: 11128 RVA: 0x000937DC File Offset: 0x000919DC
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

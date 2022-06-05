using System;
using UnityEngine;

// Token: 0x02000288 RID: 648
public static class FireProjectileFromRoomPos_AIExtension
{
	// Token: 0x060012A1 RID: 4769 RVA: 0x00082410 File Offset: 0x00080610
	public static Projectile_RL FireProjectileFromRoomPos(this BaseAIScript aiScript, string projectileName, int spawnPosIndex, bool matchFacing = true, float angle = 0f, float speedMod = 1f)
	{
		SpawnPositionController componentInChildren = aiScript.EnemyController.Room.gameObject.GetComponentInChildren<SpawnPositionController>();
		if (componentInChildren && componentInChildren.HasSpawnPosition(spawnPosIndex))
		{
			Vector2 offset = componentInChildren.GetSpawnPosition(spawnPosIndex);
			return ProjectileManager.FireProjectile(aiScript.EnemyController.gameObject, projectileName, offset, matchFacing, angle, speedMod, true, true, true, true);
		}
		return ProjectileManager.FireProjectile(aiScript.EnemyController.gameObject, projectileName, Vector2.zero, matchFacing, angle, speedMod, false, true, true, true);
	}
}

using System;
using UnityEngine;

// Token: 0x0200015C RID: 348
public static class FireProjectileFromRoomPos_AIExtension
{
	// Token: 0x06000BCA RID: 3018 RVA: 0x000237F8 File Offset: 0x000219F8
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

using System;
using UnityEngine;

// Token: 0x02000289 RID: 649
public static class FireProjectile_AIExtension
{
	// Token: 0x060012A2 RID: 4770 RVA: 0x0008248C File Offset: 0x0008068C
	public static Projectile_RL FireProjectile(this BaseAIScript aiScript, string projectileName, Vector2 offset, bool matchFacing = true, float angle = 0f, float speedMod = 1f, bool playSpawnAudio = true, bool playLifetimeAudio = true, bool playDeathAudio = true)
	{
		return ProjectileManager.FireProjectile(aiScript.EnemyController.gameObject, projectileName, offset, matchFacing, angle, speedMod, false, playSpawnAudio, playLifetimeAudio, playDeathAudio);
	}

	// Token: 0x060012A3 RID: 4771 RVA: 0x000824B8 File Offset: 0x000806B8
	public static Projectile_RL FireProjectileAbsPos(this BaseAIScript aiScript, string projectileName, Vector2 pos, bool matchFacing = true, float angle = 0f, float speedMod = 1f, bool playSpawnAudio = true, bool playLifetimeAudio = true, bool playDeathAudio = true)
	{
		return ProjectileManager.FireProjectile(aiScript.EnemyController.gameObject, projectileName, pos, matchFacing, angle, speedMod, true, playSpawnAudio, playLifetimeAudio, playDeathAudio);
	}

	// Token: 0x060012A4 RID: 4772 RVA: 0x000824E4 File Offset: 0x000806E4
	public static Projectile_RL FireProjectile(this BaseAIScript aiScript, string projectileName, int spawnPosIndex, bool matchFacing = true, float angle = 0f, float speedMod = 1f, bool playSpawnAudio = true, bool playLifetimeAudio = true, bool playDeathAudio = true)
	{
		Vector2 offset = aiScript.GetRelativeSpawnPositionAtIndex(spawnPosIndex, matchFacing);
		return ProjectileManager.FireProjectile(aiScript.EnemyController.gameObject, projectileName, offset, matchFacing, angle, speedMod, false, playSpawnAudio, playLifetimeAudio, playDeathAudio);
	}
}

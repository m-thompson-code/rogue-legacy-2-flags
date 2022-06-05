using System;
using UnityEngine;

// Token: 0x0200015D RID: 349
public static class FireProjectile_AIExtension
{
	// Token: 0x06000BCB RID: 3019 RVA: 0x00023874 File Offset: 0x00021A74
	public static Projectile_RL FireProjectile(this BaseAIScript aiScript, string projectileName, Vector2 offset, bool matchFacing = true, float angle = 0f, float speedMod = 1f, bool playSpawnAudio = true, bool playLifetimeAudio = true, bool playDeathAudio = true)
	{
		return ProjectileManager.FireProjectile(aiScript.EnemyController.gameObject, projectileName, offset, matchFacing, angle, speedMod, false, playSpawnAudio, playLifetimeAudio, playDeathAudio);
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x000238A0 File Offset: 0x00021AA0
	public static Projectile_RL FireProjectileAbsPos(this BaseAIScript aiScript, string projectileName, Vector2 pos, bool matchFacing = true, float angle = 0f, float speedMod = 1f, bool playSpawnAudio = true, bool playLifetimeAudio = true, bool playDeathAudio = true)
	{
		return ProjectileManager.FireProjectile(aiScript.EnemyController.gameObject, projectileName, pos, matchFacing, angle, speedMod, true, playSpawnAudio, playLifetimeAudio, playDeathAudio);
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x000238CC File Offset: 0x00021ACC
	public static Projectile_RL FireProjectile(this BaseAIScript aiScript, string projectileName, int spawnPosIndex, bool matchFacing = true, float angle = 0f, float speedMod = 1f, bool playSpawnAudio = true, bool playLifetimeAudio = true, bool playDeathAudio = true)
	{
		Vector2 offset = aiScript.GetRelativeSpawnPositionAtIndex(spawnPosIndex, matchFacing);
		return ProjectileManager.FireProjectile(aiScript.EnemyController.gameObject, projectileName, offset, matchFacing, angle, speedMod, false, playSpawnAudio, playLifetimeAudio, playDeathAudio);
	}
}

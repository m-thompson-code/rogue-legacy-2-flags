﻿using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000290 RID: 656
public static class SummonProjectile_AIExtension
{
	// Token: 0x060012BA RID: 4794 RVA: 0x00082990 File Offset: 0x00080B90
	public static void SummonProjectile(this BaseAIScript aiScript, string projectileName, Vector2 offset, bool matchFacing = true, float angle = 0f, float speedMod = 1f, bool playSummonAudio = true, bool playSpawnAudio = true, bool playLifetimeAudio = true, bool playDeathAudio = true)
	{
		aiScript.RunPersistentCoroutine(aiScript.SummonProjectileCoroutine_V2(projectileName, offset, matchFacing, angle, speedMod, -1, playSummonAudio, playSpawnAudio, playLifetimeAudio, playDeathAudio));
	}

	// Token: 0x060012BB RID: 4795 RVA: 0x000829BC File Offset: 0x00080BBC
	public static void SummonProjectileFromRoomPos(this BaseAIScript aiScript, string projectileName, int spawnPosIndex, bool matchFacing = true, float angle = 0f, float speedMod = 1f, bool playSummonAudio = true, bool playSpawnAudio = true, bool playLifetimeAudio = true, bool playDeathAudio = true)
	{
		aiScript.RunPersistentCoroutine(aiScript.SummonProjectileCoroutine_V2(projectileName, Vector2.zero, matchFacing, angle, speedMod, spawnPosIndex, playSummonAudio, playSpawnAudio, playLifetimeAudio, playDeathAudio));
	}

	// Token: 0x060012BC RID: 4796 RVA: 0x000829EC File Offset: 0x00080BEC
	private static IEnumerator SummonProjectileCoroutine_V2(this BaseAIScript aiScript, string projectileName, Vector2 offset, bool matchFacing = true, float angle = 0f, float speedMod = 1f, int spawnPosIndex = -1, bool playSummonAudio = true, bool playSpawnAudio = true, bool playLifetimeAudio = true, bool playDeathAudio = true)
	{
		bool isFacingRight = true;
		if (matchFacing)
		{
			isFacingRight = aiScript.EnemyController.IsFacingRight;
		}
		Vector3 spawnPos = ProjectileManager.DetermineSpawnPosition(aiScript.EnemyController.gameObject, offset, isFacingRight);
		if (spawnPosIndex != -1)
		{
			SpawnPositionController componentInChildren = aiScript.EnemyController.Room.gameObject.GetComponentInChildren<SpawnPositionController>();
			if (componentInChildren != null && componentInChildren.HasSpawnPosition(spawnPosIndex))
			{
				spawnPos = componentInChildren.GetSpawnPosition(spawnPosIndex);
			}
		}
		float startTime = Time.time;
		Projectile_RL projectile = ProjectileLibrary.GetProjectile(projectileName);
		Bounds bounds = EnemyUtility.GetBounds(projectile.gameObject);
		bounds.size *= projectile.transform.localScale.x;
		bool flag = bounds.size.y > bounds.size.x;
		Vector3 vector = new Vector3(4f, 4f, 4f);
		Bounds bounds2 = new Bounds(Vector3.zero, vector);
		bounds2.size *= vector.x;
		float num = flag ? (bounds.size.y / bounds2.size.y) : (bounds.size.x / bounds2.size.x);
		num *= 1f;
		num = Mathf.Clamp(num, 1.25f, 999f);
		vector = new Vector3(num, num, num);
		float duration = 1.2f;
		aiScript.CreateSummonCircle(spawnPos, vector, duration, false, !playSummonAudio);
		while (Time.time < startTime + 0.6f + 0.6f)
		{
			yield return null;
		}
		ProjectileManager.FireProjectile(aiScript.EnemyController.gameObject, projectileName, spawnPos, matchFacing, angle, speedMod, true, playSpawnAudio, playLifetimeAudio, playDeathAudio);
		yield break;
	}
}

using System;
using UnityEngine;

// Token: 0x02000285 RID: 645
public static class CreateSummonCircle_AIExtension
{
	// Token: 0x0600129D RID: 4765 RVA: 0x00082144 File Offset: 0x00080344
	public static void CreateSummonCircle(this BaseAIScript aiScript, Vector2 position, Vector2 scale, float duration, bool scaleWithOwner = true, bool muteAudio = false)
	{
		if (scaleWithOwner)
		{
			scale *= aiScript.EnemyController.ActualScale;
		}
		BaseEffect baseEffect = EffectManager.PlayEffect(null, null, "SpellCircle_Effect", position, duration, EffectStopType.Gracefully, EffectTriggerDirection.None);
		if (baseEffect is GenericEffect)
		{
			GenericEffect genericEffect = baseEffect as GenericEffect;
			if (genericEffect.Animator != null)
			{
				genericEffect.Animator.SetBool("MuteAudio", muteAudio);
			}
		}
		baseEffect.transform.localScale = new Vector3(scale.x, scale.y, 1f);
	}

	// Token: 0x0600129E RID: 4766 RVA: 0x000821D0 File Offset: 0x000803D0
	public static void CreateSummonCircle(this BaseAIScript aiScript, int spawnPosIndex, Vector2 scale, float duration, bool scaleWithOwner = true, bool matchFacing = true)
	{
		if (scaleWithOwner)
		{
			scale *= aiScript.EnemyController.ActualScale;
		}
		Vector3 vector = aiScript.EnemyController.SpawnPositionController.GetLocalSpawnPosition(spawnPosIndex);
		Vector3 vector2 = aiScript.EnemyController.SpawnPositionController.GetSpawnPosition(spawnPosIndex);
		vector2 = aiScript.EnemyController.transform.InverseTransformPoint(vector2);
		vector = vector2;
		vector *= aiScript.EnemyController.BaseScaleToOffsetWith;
		if (!aiScript.EnemyController.IsFacingRight && matchFacing)
		{
			vector.x = -vector.x;
		}
		EffectManager.PlayEffect(null, null, "SpellCircle_Effect", aiScript.EnemyController.Midpoint + vector, duration, EffectStopType.Gracefully, EffectTriggerDirection.None).transform.localScale = new Vector3(scale.x, scale.y, 1f);
	}
}

using System;
using UnityEngine;

// Token: 0x02000159 RID: 345
public static class CreateSummonCircle_AIExtension
{
	// Token: 0x06000BC6 RID: 3014 RVA: 0x0002352C File Offset: 0x0002172C
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

	// Token: 0x06000BC7 RID: 3015 RVA: 0x000235B8 File Offset: 0x000217B8
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

using System;
using UnityEngine;

// Token: 0x02000160 RID: 352
public static class SetAnimationSpeed_AIExtension
{
	// Token: 0x06000BD2 RID: 3026 RVA: 0x0002398C File Offset: 0x00021B8C
	public static void SetAnimationSpeedMultiplier(this BaseAIScript aiScript, float multiplier)
	{
		if (multiplier <= 0f)
		{
			Debug.LogFormat("<color=red>{0}: Animation Speed on Animator Controller ({1}) was set to a value less than or equal to zero. Was this intentional?</color>", new object[]
			{
				Time.frameCount,
				aiScript.Animator.name
			});
		}
		if (aiScript.EnemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Speed))
		{
			multiplier *= 1.25f;
		}
		if (global::AnimatorUtility.HasParameter(aiScript.Animator, "Anim_Speed"))
		{
			aiScript.Animator.SetFloat("Anim_Speed", multiplier);
			return;
		}
		if (global::AnimatorUtility.HasParameter(aiScript.Animator, "Ability_Anim_Speed"))
		{
			aiScript.Animator.SetFloat("Ability_Anim_Speed", multiplier);
		}
	}

	// Token: 0x0400102E RID: 4142
	private const string ANIM_SPEED_NAME = "Anim_Speed";

	// Token: 0x0400102F RID: 4143
	private const string ABILITY_ANIM_SPEED_NAME = "Ability_Anim_Speed";
}

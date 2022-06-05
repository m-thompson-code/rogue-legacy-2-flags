using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007A8 RID: 1960
public class LastTicksSkillCritProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003BB0 RID: 15280 RVA: 0x00020D22 File Offset: 0x0001EF22
	private void OnEnable()
	{
		base.StartCoroutine(this.TriggerSkillCritCoroutine());
	}

	// Token: 0x06003BB1 RID: 15281 RVA: 0x00020D31 File Offset: 0x0001EF31
	private IEnumerator TriggerSkillCritCoroutine()
	{
		float repeatHitDuration = base.SourceProjectile.HitboxController.RepeatHitDuration;
		float num = 0f;
		int num2;
		if (base.SourceProjectile.HasIntro)
		{
			num2 = this.m_projectileNumTicks - this.m_lastTicksTrigger;
		}
		else
		{
			num2 = this.m_projectileNumTicks - 1 - this.m_lastTicksTrigger;
			num = repeatHitDuration / 2f;
		}
		float triggerStartTime = repeatHitDuration * (float)num2;
		triggerStartTime += Time.time + num;
		while (Time.time < triggerStartTime)
		{
			yield return null;
		}
		if (base.SourceProjectile.ActualCritChance < 100f)
		{
			base.SourceProjectile.ActualCritChance += 100f;
		}
		yield break;
	}

	// Token: 0x04002F6A RID: 12138
	[SerializeField]
	[Tooltip("The lifespan of a projectile is not reliable, because it is often tied to how long the ability is cast. Therefore you need to manually count how many ticks and input it here.")]
	private int m_projectileNumTicks;

	// Token: 0x04002F6B RID: 12139
	[SerializeField]
	private int m_lastTicksTrigger;
}

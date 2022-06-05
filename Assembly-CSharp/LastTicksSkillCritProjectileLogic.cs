using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004A0 RID: 1184
public class LastTicksSkillCritProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B55 RID: 11093 RVA: 0x00092FBD File Offset: 0x000911BD
	private void OnEnable()
	{
		base.StartCoroutine(this.TriggerSkillCritCoroutine());
	}

	// Token: 0x06002B56 RID: 11094 RVA: 0x00092FCC File Offset: 0x000911CC
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

	// Token: 0x04002343 RID: 9027
	[SerializeField]
	[Tooltip("The lifespan of a projectile is not reliable, because it is often tied to how long the ability is cast. Therefore you need to manually count how many ticks and input it here.")]
	private int m_projectileNumTicks;

	// Token: 0x04002344 RID: 9028
	[SerializeField]
	private int m_lastTicksTrigger;
}

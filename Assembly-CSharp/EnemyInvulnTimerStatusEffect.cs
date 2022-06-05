using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000529 RID: 1321
public class EnemyInvulnTimerStatusEffect : BaseStatusEffect
{
	// Token: 0x17001134 RID: 4404
	// (get) Token: 0x06002A90 RID: 10896 RVA: 0x00017D05 File Offset: 0x00015F05
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_InvulnTimer;
		}
	}

	// Token: 0x17001135 RID: 4405
	// (get) Token: 0x06002A91 RID: 10897 RVA: 0x00003C62 File Offset: 0x00001E62
	public override float StartingDurationOverride
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x06002A92 RID: 10898 RVA: 0x00017D0C File Offset: 0x00015F0C
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.InvulnTimer, base.Duration);
		this.m_statusEffectController.AddStatusEffectInvulnStack(this.StatusEffectType);
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06002A93 RID: 10899 RVA: 0x00017D1B File Offset: 0x00015F1B
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		this.m_statusEffectController.RemoveStatusEffectInvulnStack(this.StatusEffectType);
	}
}

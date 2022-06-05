using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000303 RID: 771
public class EnemyInvulnTimerStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D4D RID: 3405
	// (get) Token: 0x06001EA9 RID: 7849 RVA: 0x00063330 File Offset: 0x00061530
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_InvulnTimer;
		}
	}

	// Token: 0x17000D4E RID: 3406
	// (get) Token: 0x06001EAA RID: 7850 RVA: 0x00063337 File Offset: 0x00061537
	public override float StartingDurationOverride
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x06001EAB RID: 7851 RVA: 0x0006333E File Offset: 0x0006153E
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

	// Token: 0x06001EAC RID: 7852 RVA: 0x0006334D File Offset: 0x0006154D
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		this.m_statusEffectController.RemoveStatusEffectInvulnStack(this.StatusEffectType);
	}
}

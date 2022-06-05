using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000510 RID: 1296
public class DizzyStatusEffect : BaseStatusEffect
{
	// Token: 0x170010F9 RID: 4345
	// (get) Token: 0x060029EA RID: 10730 RVA: 0x00017804 File Offset: 0x00015A04
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Dizzy;
		}
	}

	// Token: 0x170010FA RID: 4346
	// (get) Token: 0x060029EB RID: 10731 RVA: 0x00003C54 File Offset: 0x00001E54
	public override float StartingDurationOverride
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x060029EC RID: 10732 RVA: 0x0001780B File Offset: 0x00015A0B
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Dizzy, base.Duration);
		(this.m_charController as EnemyController).LogicController.StopAllLogic(true);
		this.m_charController.Animator.Play("Neutral");
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}
}

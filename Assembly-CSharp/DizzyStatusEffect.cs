using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002F8 RID: 760
public class DizzyStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D2E RID: 3374
	// (get) Token: 0x06001E57 RID: 7767 RVA: 0x000629E9 File Offset: 0x00060BE9
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Dizzy;
		}
	}

	// Token: 0x17000D2F RID: 3375
	// (get) Token: 0x06001E58 RID: 7768 RVA: 0x000629F0 File Offset: 0x00060BF0
	public override float StartingDurationOverride
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x06001E59 RID: 7769 RVA: 0x000629F7 File Offset: 0x00060BF7
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

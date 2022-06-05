using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200053A RID: 1338
public class MagicBreakStatusEffect : BaseStatusEffect
{
	// Token: 0x17001158 RID: 4440
	// (get) Token: 0x06002AFA RID: 11002 RVA: 0x00006CB3 File Offset: 0x00004EB3
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_MagicBreak;
		}
	}

	// Token: 0x17001159 RID: 4441
	// (get) Token: 0x06002AFB RID: 11003 RVA: 0x00005319 File Offset: 0x00003519
	public override float StartingDurationOverride
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x06002AFC RID: 11004 RVA: 0x00017FFD File Offset: 0x000161FD
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.MagicBreak, base.Duration);
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}
}

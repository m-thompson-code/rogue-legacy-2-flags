using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000307 RID: 775
public class FreezeImmunityStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D56 RID: 3414
	// (get) Token: 0x06001EC2 RID: 7874 RVA: 0x000636BB File Offset: 0x000618BB
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_FreezeImmunity;
		}
	}

	// Token: 0x17000D57 RID: 3415
	// (get) Token: 0x06001EC3 RID: 7875 RVA: 0x000636BF File Offset: 0x000618BF
	public override float StartingDurationOverride
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001EC4 RID: 7876 RVA: 0x000636C6 File Offset: 0x000618C6
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.FreezeImmunity, base.Duration);
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}
}

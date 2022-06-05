using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000540 RID: 1344
public class PhasedStatusEffect : BaseStatusEffect
{
	// Token: 0x17001164 RID: 4452
	// (get) Token: 0x06002B1F RID: 11039 RVA: 0x000180F5 File Offset: 0x000162F5
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Phased;
		}
	}

	// Token: 0x17001165 RID: 4453
	// (get) Token: 0x06002B20 RID: 11040 RVA: 0x00003C54 File Offset: 0x00001E54
	public override float StartingDurationOverride
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x06002B21 RID: 11041 RVA: 0x000180F9 File Offset: 0x000162F9
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.HitboxController.SetHitboxActiveState(HitboxType.Weapon, false);
		this.m_charController.HitboxController.SetHitboxActiveState(HitboxType.Body, false);
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x00018108 File Offset: 0x00016308
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		this.m_charController.HitboxController.SetHitboxActiveState(HitboxType.Weapon, true);
		this.m_charController.HitboxController.SetHitboxActiveState(HitboxType.Body, true);
	}
}

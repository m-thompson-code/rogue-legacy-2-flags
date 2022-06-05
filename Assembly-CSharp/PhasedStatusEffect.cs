using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200030D RID: 781
public class PhasedStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D63 RID: 3427
	// (get) Token: 0x06001EEA RID: 7914 RVA: 0x00063C61 File Offset: 0x00061E61
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Phased;
		}
	}

	// Token: 0x17000D64 RID: 3428
	// (get) Token: 0x06001EEB RID: 7915 RVA: 0x00063C65 File Offset: 0x00061E65
	public override float StartingDurationOverride
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x06001EEC RID: 7916 RVA: 0x00063C6C File Offset: 0x00061E6C
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

	// Token: 0x06001EED RID: 7917 RVA: 0x00063C7B File Offset: 0x00061E7B
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		this.m_charController.HitboxController.SetHitboxActiveState(HitboxType.Weapon, true);
		this.m_charController.HitboxController.SetHitboxActiveState(HitboxType.Body, true);
	}
}

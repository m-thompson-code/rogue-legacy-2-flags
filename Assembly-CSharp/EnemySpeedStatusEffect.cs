using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000306 RID: 774
public class EnemySpeedStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D54 RID: 3412
	// (get) Token: 0x06001EBD RID: 7869 RVA: 0x00063682 File Offset: 0x00061882
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Speed;
		}
	}

	// Token: 0x17000D55 RID: 3413
	// (get) Token: 0x06001EBE RID: 7870 RVA: 0x00063689 File Offset: 0x00061889
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06001EBF RID: 7871 RVA: 0x00063690 File Offset: 0x00061890
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Speed);
		this.m_charController.ResetBaseValues();
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06001EC0 RID: 7872 RVA: 0x0006369F File Offset: 0x0006189F
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		this.m_charController.ResetBaseValues();
	}

	// Token: 0x04001BCC RID: 7116
	private Vector3 m_storedScale;
}

using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200052F RID: 1327
public class EnemySpeedStatusEffect : BaseStatusEffect
{
	// Token: 0x17001141 RID: 4417
	// (get) Token: 0x06002AB6 RID: 10934 RVA: 0x00017E0D File Offset: 0x0001600D
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Speed;
		}
	}

	// Token: 0x17001142 RID: 4418
	// (get) Token: 0x06002AB7 RID: 10935 RVA: 0x00017838 File Offset: 0x00015A38
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06002AB8 RID: 10936 RVA: 0x00017E14 File Offset: 0x00016014
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

	// Token: 0x06002AB9 RID: 10937 RVA: 0x00017E23 File Offset: 0x00016023
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		this.m_charController.ResetBaseValues();
	}

	// Token: 0x04002486 RID: 9350
	private Vector3 m_storedScale;
}

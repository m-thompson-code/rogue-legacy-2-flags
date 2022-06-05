using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002F7 RID: 759
public class DeathDelayStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D2C RID: 3372
	// (get) Token: 0x06001E52 RID: 7762 RVA: 0x00062986 File Offset: 0x00060B86
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_DeathDelay;
		}
	}

	// Token: 0x17000D2D RID: 3373
	// (get) Token: 0x06001E53 RID: 7763 RVA: 0x0006298D File Offset: 0x00060B8D
	public override float StartingDurationOverride
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x06001E54 RID: 7764 RVA: 0x00062994 File Offset: 0x00060B94
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		if (this.m_charController.CurrentHealth <= 0f)
		{
			while (Time.time < base.EndTime)
			{
				yield return null;
			}
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06001E55 RID: 7765 RVA: 0x000629A3 File Offset: 0x00060BA3
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (PlayerManager.IsInstantiated && this.m_charController.CurrentHealth <= 0f)
		{
			this.m_charController.ResetIsDead();
			this.m_charController.KillCharacter(PlayerManager.GetPlayer(), true);
		}
	}
}

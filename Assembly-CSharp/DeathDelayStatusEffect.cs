using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200050E RID: 1294
public class DeathDelayStatusEffect : BaseStatusEffect
{
	// Token: 0x170010F5 RID: 4341
	// (get) Token: 0x060029DF RID: 10719 RVA: 0x00017799 File Offset: 0x00015999
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_DeathDelay;
		}
	}

	// Token: 0x170010F6 RID: 4342
	// (get) Token: 0x060029E0 RID: 10720 RVA: 0x00003C54 File Offset: 0x00001E54
	public override float StartingDurationOverride
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x060029E1 RID: 10721 RVA: 0x000177A0 File Offset: 0x000159A0
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

	// Token: 0x060029E2 RID: 10722 RVA: 0x000177AF File Offset: 0x000159AF
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

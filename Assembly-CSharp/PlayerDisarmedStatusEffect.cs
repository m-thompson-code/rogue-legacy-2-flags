using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000544 RID: 1348
public class PlayerDisarmedStatusEffect : BaseStatusEffect
{
	// Token: 0x1700116D RID: 4461
	// (get) Token: 0x06002B37 RID: 11063 RVA: 0x0001819A File Offset: 0x0001639A
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Player_Disarmed;
		}
	}

	// Token: 0x1700116E RID: 4462
	// (get) Token: 0x06002B38 RID: 11064 RVA: 0x00003C54 File Offset: 0x00001E54
	public override float StartingDurationOverride
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x06002B39 RID: 11065 RVA: 0x000181A1 File Offset: 0x000163A1
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Disarmed, base.Duration);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.UpdateAbilityDisarmState, this, null);
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06002B3A RID: 11066 RVA: 0x000181B0 File Offset: 0x000163B0
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.UpdateAbilityDisarmState, this, null);
	}
}

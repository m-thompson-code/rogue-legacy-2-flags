using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200030F RID: 783
public class PlayerDisarmedStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D68 RID: 3432
	// (get) Token: 0x06001EF6 RID: 7926 RVA: 0x00063D6C File Offset: 0x00061F6C
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Player_Disarmed;
		}
	}

	// Token: 0x17000D69 RID: 3433
	// (get) Token: 0x06001EF7 RID: 7927 RVA: 0x00063D73 File Offset: 0x00061F73
	public override float StartingDurationOverride
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x06001EF8 RID: 7928 RVA: 0x00063D7A File Offset: 0x00061F7A
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

	// Token: 0x06001EF9 RID: 7929 RVA: 0x00063D89 File Offset: 0x00061F89
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.UpdateAbilityDisarmState, this, null);
	}
}

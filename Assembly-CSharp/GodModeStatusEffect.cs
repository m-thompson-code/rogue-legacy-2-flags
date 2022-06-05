using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000536 RID: 1334
public class GodModeStatusEffect : BaseStatusEffect
{
	// Token: 0x1700114F RID: 4431
	// (get) Token: 0x06002AE1 RID: 10977 RVA: 0x00017F3C File Offset: 0x0001613C
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Player_GodMode;
		}
	}

	// Token: 0x17001150 RID: 4432
	// (get) Token: 0x06002AE2 RID: 10978 RVA: 0x00003CE4 File Offset: 0x00001EE4
	public override float StartingDurationOverride
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x06002AE3 RID: 10979 RVA: 0x00017F43 File Offset: 0x00016143
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.GodMode, base.Duration);
		this.m_defaultBaseStrength = PlayerManager.GetPlayerController().BaseStrength;
		this.m_defaultSpeed = PlayerManager.GetPlayerController().BaseMovementSpeed;
		PlayerManager.GetPlayerController().BaseStrength = 5f;
		PlayerManager.GetPlayerController().BaseMovementSpeed *= 2f;
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06002AE4 RID: 10980 RVA: 0x00017F52 File Offset: 0x00016152
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().BaseStrength = this.m_defaultBaseStrength;
			PlayerManager.GetPlayerController().BaseMovementSpeed = this.m_defaultSpeed;
		}
	}

	// Token: 0x040024A3 RID: 9379
	private float m_defaultBaseStrength;

	// Token: 0x040024A4 RID: 9380
	private float m_defaultSpeed;
}

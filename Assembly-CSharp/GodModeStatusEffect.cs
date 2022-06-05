using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000309 RID: 777
public class GodModeStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D5A RID: 3418
	// (get) Token: 0x06001ED0 RID: 7888 RVA: 0x0006395E File Offset: 0x00061B5E
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Player_GodMode;
		}
	}

	// Token: 0x17000D5B RID: 3419
	// (get) Token: 0x06001ED1 RID: 7889 RVA: 0x00063965 File Offset: 0x00061B65
	public override float StartingDurationOverride
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x06001ED2 RID: 7890 RVA: 0x0006396C File Offset: 0x00061B6C
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

	// Token: 0x06001ED3 RID: 7891 RVA: 0x0006397B File Offset: 0x00061B7B
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().BaseStrength = this.m_defaultBaseStrength;
			PlayerManager.GetPlayerController().BaseMovementSpeed = this.m_defaultSpeed;
		}
	}

	// Token: 0x04001BDC RID: 7132
	private float m_defaultBaseStrength;

	// Token: 0x04001BDD RID: 7133
	private float m_defaultSpeed;
}

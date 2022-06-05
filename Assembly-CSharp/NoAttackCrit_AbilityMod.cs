using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200016E RID: 366
public class NoAttackCrit_AbilityMod : MonoBehaviour
{
	// Token: 0x06000C9D RID: 3229 RVA: 0x00026FD0 File Offset: 0x000251D0
	private void Awake()
	{
		this.m_ability = base.GetComponent<BaseAbility_RL>();
		this.m_ability.BeginCastingRelay.AddListener(new Action(this.StopCritWindowTimer), false);
		this.m_ability.StopCastingRelay.AddListener(new Action(this.StartCritWindowTimer), false);
	}

	// Token: 0x06000C9E RID: 3230 RVA: 0x00027028 File Offset: 0x00025228
	private void OnDestroy()
	{
		if (this.m_ability)
		{
			this.m_ability.BeginCastingRelay.RemoveListener(new Action(this.StopCritWindowTimer));
			this.m_ability.StopCastingRelay.RemoveListener(new Action(this.StartCritWindowTimer));
		}
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x0002707C File Offset: 0x0002527C
	private void OnEnable()
	{
		this.StartCritWindowTimer();
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x00027084 File Offset: 0x00025284
	private void StartCritWindowTimer()
	{
		this.StopCritWindowTimer();
		this.m_critWindowTimerCoroutine = base.StartCoroutine(this.CritWindowTimerCoroutine());
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x0002709E File Offset: 0x0002529E
	private IEnumerator CritWindowTimerCoroutine()
	{
		float delay = Time.time + this.m_noAttackCritDuration;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.m_ability.ForceTriggerCrit = true;
		PlayerManager.GetPlayerController().BlinkPulseEffect.SetRendererArrayColor(Player_EV.PLAYER_GUARANTEED_CRIT_COLOR);
		delay = Time.time + 0.05f;
		this.m_critDebugOn = true;
		while (Time.time < delay)
		{
			yield return null;
		}
		PlayerManager.GetPlayerController().BlinkPulseEffect.ResetRendererArrayColor();
		this.m_critDebugOn = false;
		yield break;
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x000270AD File Offset: 0x000252AD
	private void StopCritWindowTimer()
	{
		if (this.m_critWindowTimerCoroutine != null)
		{
			base.StopCoroutine(this.m_critWindowTimerCoroutine);
		}
		if (this.m_critDebugOn)
		{
			this.m_critDebugOn = false;
			PlayerManager.GetPlayerController().BlinkPulseEffect.ResetRendererArrayColor();
		}
	}

	// Token: 0x040010A5 RID: 4261
	[SerializeField]
	private float m_noAttackCritDuration = 1f;

	// Token: 0x040010A6 RID: 4262
	private BaseAbility_RL m_ability;

	// Token: 0x040010A7 RID: 4263
	private Coroutine m_critWindowTimerCoroutine;

	// Token: 0x040010A8 RID: 4264
	private bool m_critDebugOn;
}

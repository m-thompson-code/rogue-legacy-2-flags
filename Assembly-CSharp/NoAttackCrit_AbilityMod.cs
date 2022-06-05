using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002A4 RID: 676
public class NoAttackCrit_AbilityMod : MonoBehaviour
{
	// Token: 0x060013B0 RID: 5040 RVA: 0x000860AC File Offset: 0x000842AC
	private void Awake()
	{
		this.m_ability = base.GetComponent<BaseAbility_RL>();
		this.m_ability.BeginCastingRelay.AddListener(new Action(this.StopCritWindowTimer), false);
		this.m_ability.StopCastingRelay.AddListener(new Action(this.StartCritWindowTimer), false);
	}

	// Token: 0x060013B1 RID: 5041 RVA: 0x00086104 File Offset: 0x00084304
	private void OnDestroy()
	{
		if (this.m_ability)
		{
			this.m_ability.BeginCastingRelay.RemoveListener(new Action(this.StopCritWindowTimer));
			this.m_ability.StopCastingRelay.RemoveListener(new Action(this.StartCritWindowTimer));
		}
	}

	// Token: 0x060013B2 RID: 5042 RVA: 0x0000A098 File Offset: 0x00008298
	private void OnEnable()
	{
		this.StartCritWindowTimer();
	}

	// Token: 0x060013B3 RID: 5043 RVA: 0x0000A0A0 File Offset: 0x000082A0
	private void StartCritWindowTimer()
	{
		this.StopCritWindowTimer();
		this.m_critWindowTimerCoroutine = base.StartCoroutine(this.CritWindowTimerCoroutine());
	}

	// Token: 0x060013B4 RID: 5044 RVA: 0x0000A0BA File Offset: 0x000082BA
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

	// Token: 0x060013B5 RID: 5045 RVA: 0x0000A0C9 File Offset: 0x000082C9
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

	// Token: 0x040015D9 RID: 5593
	[SerializeField]
	private float m_noAttackCritDuration = 1f;

	// Token: 0x040015DA RID: 5594
	private BaseAbility_RL m_ability;

	// Token: 0x040015DB RID: 5595
	private Coroutine m_critWindowTimerCoroutine;

	// Token: 0x040015DC RID: 5596
	private bool m_critDebugOn;
}

using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006E8 RID: 1768
public class RumbleControllerEffect : BaseEffect
{
	// Token: 0x0600361A RID: 13850 RVA: 0x0001DAC4 File Offset: 0x0001BCC4
	public override void Play(float duration = 0f, EffectStopType stopType = EffectStopType.Gracefully)
	{
		base.Play(duration, stopType);
		base.StartCoroutine(this.PlayTimedRumble(duration));
	}

	// Token: 0x0600361B RID: 13851 RVA: 0x0001DADC File Offset: 0x0001BCDC
	private IEnumerator PlayTimedRumble(float duration)
	{
		RumbleManager.StartRumble(true, true, this.m_rumbleAmount, duration, true);
		duration += Time.time;
		while (Time.time < duration)
		{
			yield return null;
		}
		this.Stop(EffectStopType.Gracefully);
		yield break;
	}

	// Token: 0x0600361C RID: 13852 RVA: 0x0001DAF2 File Offset: 0x0001BCF2
	public override void Stop(EffectStopType stopType)
	{
		RumbleManager.StopRumble(true, true);
		this.PlayComplete();
	}

	// Token: 0x0600361D RID: 13853 RVA: 0x0001DB01 File Offset: 0x0001BD01
	protected override void OnDisable()
	{
		RumbleManager.StopRumble(true, true);
		base.OnDisable();
	}

	// Token: 0x04002BF0 RID: 11248
	[SerializeField]
	private float m_rumbleAmount;
}

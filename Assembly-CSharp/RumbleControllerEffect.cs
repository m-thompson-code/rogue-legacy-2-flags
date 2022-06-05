using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000426 RID: 1062
public class RumbleControllerEffect : BaseEffect
{
	// Token: 0x0600272F RID: 10031 RVA: 0x000829AD File Offset: 0x00080BAD
	public override void Play(float duration = 0f, EffectStopType stopType = EffectStopType.Gracefully)
	{
		base.Play(duration, stopType);
		base.StartCoroutine(this.PlayTimedRumble(duration));
	}

	// Token: 0x06002730 RID: 10032 RVA: 0x000829C5 File Offset: 0x00080BC5
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

	// Token: 0x06002731 RID: 10033 RVA: 0x000829DB File Offset: 0x00080BDB
	public override void Stop(EffectStopType stopType)
	{
		RumbleManager.StopRumble(true, true);
		this.PlayComplete();
	}

	// Token: 0x06002732 RID: 10034 RVA: 0x000829EA File Offset: 0x00080BEA
	protected override void OnDisable()
	{
		RumbleManager.StopRumble(true, true);
		base.OnDisable();
	}

	// Token: 0x040020DF RID: 8415
	[SerializeField]
	private float m_rumbleAmount;
}

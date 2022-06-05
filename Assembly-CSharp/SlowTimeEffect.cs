using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000428 RID: 1064
public class SlowTimeEffect : BaseEffect
{
	// Token: 0x06002739 RID: 10041 RVA: 0x00082B1F File Offset: 0x00080D1F
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x0600273A RID: 10042 RVA: 0x00082B38 File Offset: 0x00080D38
	public override void Play(float duration, EffectStopType stopType)
	{
		base.Play(duration, stopType);
		if (SaveManager.ConfigData.DisableSlowdownOnHit)
		{
			this.PlayComplete();
			return;
		}
		this.m_slowDuration = duration;
		this.SlowTime();
	}

	// Token: 0x0600273B RID: 10043 RVA: 0x00082B62 File Offset: 0x00080D62
	public override void Stop(EffectStopType stopType)
	{
		if (base.IsPlaying)
		{
			RLTimeScale.SetTimeScale(this.m_slowTimeScaleType, 1f);
		}
		this.PlayComplete();
	}

	// Token: 0x0600273C RID: 10044 RVA: 0x00082B82 File Offset: 0x00080D82
	private void SlowTime()
	{
		if (this.m_slowCoroutine != null)
		{
			base.StopCoroutine(this.m_slowCoroutine);
			this.m_slowCoroutine = null;
		}
		if (this.m_slowDuration > 0f)
		{
			this.m_slowCoroutine = base.StartCoroutine(this.SlowTimeCoroutine());
		}
	}

	// Token: 0x0600273D RID: 10045 RVA: 0x00082BBE File Offset: 0x00080DBE
	private IEnumerator SlowTimeCoroutine()
	{
		base.IsPlaying = true;
		float time = Time.time;
		bool useUnscaledTime = false;
		float num = this.m_slowDuration * this.m_timeScaleValue;
		if (num < 0.1f)
		{
			num = this.m_slowDuration;
			useUnscaledTime = true;
		}
		this.m_waitYield.CreateNew(num, useUnscaledTime);
		this.m_slowTimeScaleType = RLTimeScale.GetAvailableSlowTimeStack();
		RLTimeScale.SetTimeScale(this.m_slowTimeScaleType, this.m_timeScaleValue);
		yield return this.m_waitYield;
		RLTimeScale.SetTimeScale(this.m_slowTimeScaleType, 1f);
		this.PlayComplete();
		yield break;
	}

	// Token: 0x0600273E RID: 10046 RVA: 0x00082BCD File Offset: 0x00080DCD
	protected override void OnDisable()
	{
		if (base.IsPlaying)
		{
			RLTimeScale.SetTimeScale(this.m_slowTimeScaleType, 1f);
		}
		base.OnDisable();
	}

	// Token: 0x040020EA RID: 8426
	private const float FIXED_DELTA_TIME = 0.016666668f;

	// Token: 0x040020EB RID: 8427
	[SerializeField]
	private float m_timeScaleValue = 1f;

	// Token: 0x040020EC RID: 8428
	private float m_slowDuration;

	// Token: 0x040020ED RID: 8429
	private Coroutine m_slowCoroutine;

	// Token: 0x040020EE RID: 8430
	private WaitRL_Yield m_waitYield;

	// Token: 0x040020EF RID: 8431
	private TimeScaleType m_slowTimeScaleType;
}

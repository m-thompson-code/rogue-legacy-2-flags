using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006EC RID: 1772
public class SlowTimeEffect : BaseEffect
{
	// Token: 0x06003630 RID: 13872 RVA: 0x0001DBB2 File Offset: 0x0001BDB2
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06003631 RID: 13873 RVA: 0x0001DBCB File Offset: 0x0001BDCB
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

	// Token: 0x06003632 RID: 13874 RVA: 0x0001DBF5 File Offset: 0x0001BDF5
	public override void Stop(EffectStopType stopType)
	{
		if (base.IsPlaying)
		{
			RLTimeScale.SetTimeScale(this.m_slowTimeScaleType, 1f);
		}
		this.PlayComplete();
	}

	// Token: 0x06003633 RID: 13875 RVA: 0x0001DC15 File Offset: 0x0001BE15
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

	// Token: 0x06003634 RID: 13876 RVA: 0x0001DC51 File Offset: 0x0001BE51
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

	// Token: 0x06003635 RID: 13877 RVA: 0x0001DC60 File Offset: 0x0001BE60
	protected override void OnDisable()
	{
		if (base.IsPlaying)
		{
			RLTimeScale.SetTimeScale(this.m_slowTimeScaleType, 1f);
		}
		base.OnDisable();
	}

	// Token: 0x04002C05 RID: 11269
	private const float FIXED_DELTA_TIME = 0.016666668f;

	// Token: 0x04002C06 RID: 11270
	[SerializeField]
	private float m_timeScaleValue = 1f;

	// Token: 0x04002C07 RID: 11271
	private float m_slowDuration;

	// Token: 0x04002C08 RID: 11272
	private Coroutine m_slowCoroutine;

	// Token: 0x04002C09 RID: 11273
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002C0A RID: 11274
	private TimeScaleType m_slowTimeScaleType;
}

using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000423 RID: 1059
public class GenericEffect : BaseEffect
{
	// Token: 0x17000F8C RID: 3980
	// (get) Token: 0x06002715 RID: 10005 RVA: 0x0008265E File Offset: 0x0008085E
	private bool HasParticleSystem
	{
		get
		{
			return this.m_particleSystem;
		}
	}

	// Token: 0x17000F8D RID: 3981
	// (get) Token: 0x06002716 RID: 10006 RVA: 0x0008266B File Offset: 0x0008086B
	private bool HasAnimator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x17000F8E RID: 3982
	// (get) Token: 0x06002717 RID: 10007 RVA: 0x00082678 File Offset: 0x00080878
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x17000F8F RID: 3983
	// (get) Token: 0x06002718 RID: 10008 RVA: 0x00082680 File Offset: 0x00080880
	public ParticleSystem ParticleSystem
	{
		get
		{
			return this.m_particleSystem;
		}
	}

	// Token: 0x06002719 RID: 10009 RVA: 0x00082688 File Offset: 0x00080888
	protected override void Awake()
	{
		base.Awake();
		this.m_particleSystem = base.GetComponentInChildren<ParticleSystem>();
		this.m_animator = base.GetComponentInChildren<Animator>();
		if (this.m_animator)
		{
			this.m_hasStopParam = global::AnimatorUtility.HasParameter(this.m_animator, "Stop");
			this.m_storedUpdateMode = this.m_animator.updateMode;
		}
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x0600271A RID: 10010 RVA: 0x000826F8 File Offset: 0x000808F8
	public override void Play(float duration = 0f, EffectStopType stopType = EffectStopType.Gracefully)
	{
		base.Play(duration, stopType);
		if (this.HasParticleSystem)
		{
			base.StartCoroutine(this.PlayTimedParticleSystem(duration, stopType));
		}
		if (this.HasAnimator)
		{
			this.m_animator.enabled = true;
			base.StartCoroutine(this.PlayTimedAnimator(duration, stopType));
		}
		if (!this.HasParticleSystem && !this.HasAnimator)
		{
			base.StartCoroutine(this.PlayTimer(duration, stopType));
		}
	}

	// Token: 0x0600271B RID: 10011 RVA: 0x00082766 File Offset: 0x00080966
	private IEnumerator PlayTimer(float duration, EffectStopType stopType)
	{
		if (duration > 0f)
		{
			this.m_waitYield.CreateNew(duration, this.m_useUnscaledTime);
			yield return this.m_waitYield;
			this.PlayComplete();
			yield break;
		}
		for (;;)
		{
			yield return null;
		}
	}

	// Token: 0x0600271C RID: 10012 RVA: 0x0008277C File Offset: 0x0008097C
	private IEnumerator PlayTimedAnimator(float duration, EffectStopType stopType)
	{
		if (!this.m_hasStopParam && duration == 0f && stopType == EffectStopType.Immediate)
		{
			Debug.LogWarning("<color=yellow>WARNING: Playing effect with 0 duration and stopType Immediate will result in an immediate start and stoppage, and no visible effect will appear.</color>");
		}
		if (duration < 0f)
		{
			yield return null;
			if (base.SourceAnimator && duration == -1f)
			{
				int nameHash = base.SourceAnimator.GetCurrentAnimatorStateInfo(base.AnimatorLayer).shortNameHash;
				while (base.SourceAnimator && base.SourceAnimator.gameObject.activeInHierarchy)
				{
					if (base.SourceAnimator.GetCurrentAnimatorStateInfo(base.AnimatorLayer).shortNameHash != nameHash)
					{
						break;
					}
					yield return null;
				}
			}
			else
			{
				while (base.Source)
				{
					if (!base.Source.gameObject.activeInHierarchy)
					{
						break;
					}
					yield return null;
				}
			}
		}
		else if (duration > 0f)
		{
			this.m_waitYield.CreateNew(duration, this.m_useUnscaledTime);
			yield return this.m_waitYield;
		}
		if (stopType == EffectStopType.Gracefully)
		{
			if (this.m_hasStopParam)
			{
				this.m_animator.SetTrigger("Stop");
				yield return null;
			}
			AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(base.AnimatorLayer);
			float waitTime = currentAnimatorStateInfo.length * (1f - currentAnimatorStateInfo.normalizedTime);
			this.m_waitYield.CreateNew(waitTime, this.m_useUnscaledTime);
			yield return this.m_waitYield;
			this.PlayComplete();
		}
		else
		{
			this.PlayComplete();
		}
		yield break;
	}

	// Token: 0x0600271D RID: 10013 RVA: 0x00082799 File Offset: 0x00080999
	private IEnumerator PlayTimedParticleSystem(float duration, EffectStopType stopType)
	{
		this.m_particleSystem.Clear();
		this.m_particleSystem.Play(true);
		if (duration < 0f)
		{
			if (base.Source)
			{
				yield return null;
				if (base.SourceAnimator && duration == -1f)
				{
					int nameHash = base.SourceAnimator.GetCurrentAnimatorStateInfo(base.AnimatorLayer).shortNameHash;
					while (base.SourceAnimator && base.SourceAnimator.gameObject.activeInHierarchy)
					{
						if (base.SourceAnimator.GetCurrentAnimatorStateInfo(base.AnimatorLayer).shortNameHash != nameHash)
						{
							break;
						}
						yield return null;
					}
				}
				else
				{
					while (base.Source)
					{
						if (!base.Source.gameObject.activeInHierarchy)
						{
							break;
						}
						yield return null;
					}
				}
			}
		}
		else if (duration >= 0f)
		{
			if (duration == 0f)
			{
				duration = this.m_particleSystem.main.duration;
				if (duration == 0f)
				{
					duration = 0.05f;
				}
			}
			this.m_waitYield.CreateNew(duration, this.m_useUnscaledTime);
			yield return this.m_waitYield;
		}
		if (stopType == EffectStopType.Gracefully)
		{
			this.m_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		}
		else
		{
			this.m_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		bool playedStopAgain = false;
		while (this.m_particleSystem.IsAlive(true))
		{
			yield return null;
			if (!playedStopAgain)
			{
				if (stopType == EffectStopType.Gracefully)
				{
					this.m_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
				}
				else
				{
					this.m_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
				}
				playedStopAgain = true;
			}
		}
		if (!this.HasAnimator)
		{
			this.PlayComplete();
		}
		yield break;
	}

	// Token: 0x0600271E RID: 10014 RVA: 0x000827B6 File Offset: 0x000809B6
	protected override void PlayComplete()
	{
		base.PlayComplete();
		if (this.m_animator)
		{
			this.m_animator.updateMode = this.m_storedUpdateMode;
		}
	}

	// Token: 0x0600271F RID: 10015 RVA: 0x000827DC File Offset: 0x000809DC
	public override void Stop(EffectStopType stopType)
	{
		base.StopAllCoroutines();
		if (!base.gameObject.activeSelf)
		{
			this.PlayComplete();
			return;
		}
		if (stopType != EffectStopType.Immediate)
		{
			if (stopType == EffectStopType.Gracefully)
			{
				base.StartCoroutine(this.StopGracefullyCoroutine());
				return;
			}
		}
		else
		{
			if (this.HasParticleSystem && this.m_particleSystem.isPlaying)
			{
				this.m_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
			this.PlayComplete();
		}
	}

	// Token: 0x06002720 RID: 10016 RVA: 0x00082840 File Offset: 0x00080A40
	private IEnumerator StopGracefullyCoroutine()
	{
		if (this.HasParticleSystem && this.m_particleSystem.isPlaying)
		{
			this.m_stoppingParticleSystem = true;
			base.StartCoroutine(this.StopParticleSystemGracefullyCoroutine());
		}
		if (this.HasAnimator)
		{
			this.m_stoppingAnimator = true;
			base.StartCoroutine(this.StopAnimatorGracefullyCoroutine());
		}
		while (this.m_stoppingParticleSystem || this.m_stoppingAnimator)
		{
			yield return null;
		}
		this.PlayComplete();
		yield break;
	}

	// Token: 0x06002721 RID: 10017 RVA: 0x0008284F File Offset: 0x00080A4F
	private IEnumerator StopParticleSystemGracefullyCoroutine()
	{
		this.m_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		bool playedStopAgain = false;
		while (this.m_particleSystem.IsAlive(true))
		{
			yield return null;
			if (!playedStopAgain)
			{
				this.m_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
				playedStopAgain = true;
			}
		}
		this.m_stoppingParticleSystem = false;
		yield break;
	}

	// Token: 0x06002722 RID: 10018 RVA: 0x0008285E File Offset: 0x00080A5E
	private IEnumerator StopAnimatorGracefullyCoroutine()
	{
		if (this.m_hasStopParam)
		{
			this.m_animator.SetTrigger("Stop");
			yield return null;
		}
		AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(base.AnimatorLayer);
		float waitTime = currentAnimatorStateInfo.length * (1f - currentAnimatorStateInfo.normalizedTime);
		this.m_waitYield.CreateNew(waitTime, this.m_useUnscaledTime);
		yield return this.m_waitYield;
		this.m_stoppingAnimator = false;
		this.m_animator.enabled = false;
		yield break;
	}

	// Token: 0x06002723 RID: 10019 RVA: 0x0008286D File Offset: 0x00080A6D
	public override void ResetValues()
	{
		base.ResetValues();
		if (this.m_animator)
		{
			this.m_animator.updateMode = this.m_storedUpdateMode;
		}
	}

	// Token: 0x040020D1 RID: 8401
	[SerializeField]
	private bool m_useUnscaledTime;

	// Token: 0x040020D2 RID: 8402
	private ParticleSystem m_particleSystem;

	// Token: 0x040020D3 RID: 8403
	private Animator m_animator;

	// Token: 0x040020D4 RID: 8404
	private WaitRL_Yield m_waitYield;

	// Token: 0x040020D5 RID: 8405
	private bool m_hasStopParam;

	// Token: 0x040020D6 RID: 8406
	private AnimatorUpdateMode m_storedUpdateMode;

	// Token: 0x040020D7 RID: 8407
	private bool m_stoppingParticleSystem;

	// Token: 0x040020D8 RID: 8408
	private bool m_stoppingAnimator;
}

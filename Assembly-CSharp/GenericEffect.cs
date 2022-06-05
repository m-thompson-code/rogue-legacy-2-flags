using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006DE RID: 1758
public class GenericEffect : BaseEffect
{
	// Token: 0x17001455 RID: 5205
	// (get) Token: 0x060035D6 RID: 13782 RVA: 0x0001D878 File Offset: 0x0001BA78
	private bool HasParticleSystem
	{
		get
		{
			return this.m_particleSystem;
		}
	}

	// Token: 0x17001456 RID: 5206
	// (get) Token: 0x060035D7 RID: 13783 RVA: 0x0001D885 File Offset: 0x0001BA85
	private bool HasAnimator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x17001457 RID: 5207
	// (get) Token: 0x060035D8 RID: 13784 RVA: 0x0001D892 File Offset: 0x0001BA92
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x17001458 RID: 5208
	// (get) Token: 0x060035D9 RID: 13785 RVA: 0x0001D89A File Offset: 0x0001BA9A
	public ParticleSystem ParticleSystem
	{
		get
		{
			return this.m_particleSystem;
		}
	}

	// Token: 0x060035DA RID: 13786 RVA: 0x000E2B40 File Offset: 0x000E0D40
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

	// Token: 0x060035DB RID: 13787 RVA: 0x000E2BB0 File Offset: 0x000E0DB0
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

	// Token: 0x060035DC RID: 13788 RVA: 0x0001D8A2 File Offset: 0x0001BAA2
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

	// Token: 0x060035DD RID: 13789 RVA: 0x0001D8B8 File Offset: 0x0001BAB8
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

	// Token: 0x060035DE RID: 13790 RVA: 0x0001D8D5 File Offset: 0x0001BAD5
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

	// Token: 0x060035DF RID: 13791 RVA: 0x0001D8F2 File Offset: 0x0001BAF2
	protected override void PlayComplete()
	{
		base.PlayComplete();
		if (this.m_animator)
		{
			this.m_animator.updateMode = this.m_storedUpdateMode;
		}
	}

	// Token: 0x060035E0 RID: 13792 RVA: 0x000E2C20 File Offset: 0x000E0E20
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

	// Token: 0x060035E1 RID: 13793 RVA: 0x0001D918 File Offset: 0x0001BB18
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

	// Token: 0x060035E2 RID: 13794 RVA: 0x0001D927 File Offset: 0x0001BB27
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

	// Token: 0x060035E3 RID: 13795 RVA: 0x0001D936 File Offset: 0x0001BB36
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

	// Token: 0x060035E4 RID: 13796 RVA: 0x0001D945 File Offset: 0x0001BB45
	public override void ResetValues()
	{
		base.ResetValues();
		if (this.m_animator)
		{
			this.m_animator.updateMode = this.m_storedUpdateMode;
		}
	}

	// Token: 0x04002BC2 RID: 11202
	[SerializeField]
	private bool m_useUnscaledTime;

	// Token: 0x04002BC3 RID: 11203
	private ParticleSystem m_particleSystem;

	// Token: 0x04002BC4 RID: 11204
	private Animator m_animator;

	// Token: 0x04002BC5 RID: 11205
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002BC6 RID: 11206
	private bool m_hasStopParam;

	// Token: 0x04002BC7 RID: 11207
	private AnimatorUpdateMode m_storedUpdateMode;

	// Token: 0x04002BC8 RID: 11208
	private bool m_stoppingParticleSystem;

	// Token: 0x04002BC9 RID: 11209
	private bool m_stoppingAnimator;
}

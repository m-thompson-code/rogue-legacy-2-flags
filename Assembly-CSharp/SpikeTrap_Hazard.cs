using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200045C RID: 1116
public class SpikeTrap_Hazard : Hazard
{
	// Token: 0x17001015 RID: 4117
	// (get) Token: 0x0600292E RID: 10542 RVA: 0x0008842D File Offset: 0x0008662D
	public override float BaseDamage
	{
		get
		{
			return 30f;
		}
	}

	// Token: 0x0600292F RID: 10543 RVA: 0x00088434 File Offset: 0x00086634
	protected override void Awake()
	{
		base.Awake();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x06002930 RID: 10544 RVA: 0x00088448 File Offset: 0x00086648
	private IEnumerator Start()
	{
		while (!this.m_hbController.IsInitialized)
		{
			yield return null;
		}
		this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, false);
		yield break;
	}

	// Token: 0x06002931 RID: 10545 RVA: 0x00088458 File Offset: 0x00086658
	public void ExtendSpike()
	{
		if (TraitManager.IsTraitActive(TraitType.DisableSpikeTraps))
		{
			return;
		}
		this.m_retractSpikes = false;
		if (!this.m_extendSpikes)
		{
			this.m_extendSpikes = true;
			base.StopAllCoroutines();
			if (this.TriggeredEvent != null)
			{
				this.TriggeredEvent.Invoke();
			}
			base.StartCoroutine(this.ExtensionCoroutine());
		}
	}

	// Token: 0x06002932 RID: 10546 RVA: 0x000884AE File Offset: 0x000866AE
	private IEnumerator ExtensionCoroutine()
	{
		float extensionTimer = Time.time + 0.425f;
		while (extensionTimer > Time.time)
		{
			yield return null;
		}
		base.Animator.SetBool("SpikesOut", true);
		base.Animator.Play("PopOutSpikes_Extend");
		yield return null;
		base.StartCoroutine(this.PerformExtensionCheck());
		yield break;
	}

	// Token: 0x06002933 RID: 10547 RVA: 0x000884BD File Offset: 0x000866BD
	private IEnumerator PerformExtensionCheck()
	{
		while (base.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f)
		{
			yield return null;
		}
		this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, true);
		while (!this.m_retractSpikes)
		{
			yield return null;
		}
		base.StartCoroutine(this.RetractionCoroutine());
		yield break;
	}

	// Token: 0x06002934 RID: 10548 RVA: 0x000884CC File Offset: 0x000866CC
	private IEnumerator RetractionCoroutine()
	{
		this.m_extendSpikes = false;
		float retractionTimer = Time.time + 0.45f;
		while (retractionTimer > Time.time)
		{
			yield return null;
		}
		this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, false);
		base.Animator.SetBool("SpikesOut", false);
		base.Animator.Play("PopOutSpikes_Retract");
		yield return null;
		yield break;
	}

	// Token: 0x06002935 RID: 10549 RVA: 0x000884DB File Offset: 0x000866DB
	public void RetractSpike()
	{
		this.m_retractSpikes = true;
	}

	// Token: 0x06002936 RID: 10550 RVA: 0x000884E4 File Offset: 0x000866E4
	public override void ResetHazard()
	{
		base.Animator.SetBool("SpikesOut", false);
		base.Animator.Play("PopOutSpikes_Idle");
		this.m_extendSpikes = false;
		this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, false);
	}

	// Token: 0x040021F5 RID: 8693
	[SerializeField]
	public UnityEvent TriggeredEvent;

	// Token: 0x040021F6 RID: 8694
	private const float m_extensionDelay = 0.425f;

	// Token: 0x040021F7 RID: 8695
	private const float m_retractionDelay = 0.45f;

	// Token: 0x040021F8 RID: 8696
	private IHitboxController m_hbController;

	// Token: 0x040021F9 RID: 8697
	private bool m_extendSpikes;

	// Token: 0x040021FA RID: 8698
	private bool m_retractSpikes;
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000749 RID: 1865
public class SpikeTrap_Hazard : Hazard
{
	// Token: 0x1700153A RID: 5434
	// (get) Token: 0x06003908 RID: 14600 RVA: 0x00005391 File Offset: 0x00003591
	public override float BaseDamage
	{
		get
		{
			return 30f;
		}
	}

	// Token: 0x06003909 RID: 14601 RVA: 0x0001F535 File Offset: 0x0001D735
	protected override void Awake()
	{
		base.Awake();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x0600390A RID: 14602 RVA: 0x0001F549 File Offset: 0x0001D749
	private IEnumerator Start()
	{
		while (!this.m_hbController.IsInitialized)
		{
			yield return null;
		}
		this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, false);
		yield break;
	}

	// Token: 0x0600390B RID: 14603 RVA: 0x000EA2EC File Offset: 0x000E84EC
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

	// Token: 0x0600390C RID: 14604 RVA: 0x0001F558 File Offset: 0x0001D758
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

	// Token: 0x0600390D RID: 14605 RVA: 0x0001F567 File Offset: 0x0001D767
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

	// Token: 0x0600390E RID: 14606 RVA: 0x0001F576 File Offset: 0x0001D776
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

	// Token: 0x0600390F RID: 14607 RVA: 0x0001F585 File Offset: 0x0001D785
	public void RetractSpike()
	{
		this.m_retractSpikes = true;
	}

	// Token: 0x06003910 RID: 14608 RVA: 0x0001F58E File Offset: 0x0001D78E
	public override void ResetHazard()
	{
		base.Animator.SetBool("SpikesOut", false);
		base.Animator.Play("PopOutSpikes_Idle");
		this.m_extendSpikes = false;
		this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, false);
	}

	// Token: 0x04002DB1 RID: 11697
	[SerializeField]
	public UnityEvent TriggeredEvent;

	// Token: 0x04002DB2 RID: 11698
	private const float m_extensionDelay = 0.425f;

	// Token: 0x04002DB3 RID: 11699
	private const float m_retractionDelay = 0.45f;

	// Token: 0x04002DB4 RID: 11700
	private IHitboxController m_hbController;

	// Token: 0x04002DB5 RID: 11701
	private bool m_extendSpikes;

	// Token: 0x04002DB6 RID: 11702
	private bool m_retractSpikes;
}

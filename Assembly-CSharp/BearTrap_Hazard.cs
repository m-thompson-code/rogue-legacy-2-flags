using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200070E RID: 1806
public class BearTrap_Hazard : Hazard, ILevelConsumer
{
	// Token: 0x170014AF RID: 5295
	// (get) Token: 0x06003723 RID: 14115 RVA: 0x00005391 File Offset: 0x00003591
	public override float BaseDamage
	{
		get
		{
			return 30f;
		}
	}

	// Token: 0x06003724 RID: 14116 RVA: 0x000E5198 File Offset: 0x000E3398
	private void OnEnable()
	{
		BaseRoom componentInParent = base.GetComponentInParent<BaseRoom>();
		if (componentInParent != null)
		{
			componentInParent.PlayerExitRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom), false);
		}
	}

	// Token: 0x06003725 RID: 14117 RVA: 0x000E51D0 File Offset: 0x000E33D0
	protected override void OnDisable()
	{
		base.OnDisable();
		BaseRoom componentInParent = base.GetComponentInParent<BaseRoom>();
		if (componentInParent != null)
		{
			componentInParent.PlayerExitRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom));
		}
	}

	// Token: 0x06003726 RID: 14118 RVA: 0x0001E570 File Offset: 0x0001C770
	public void ExtendSpike()
	{
		this.m_retractSpikes = false;
		if (!this.m_extendSpikes)
		{
			this.m_extendSpikes = true;
			base.StopAllCoroutines();
			base.StartCoroutine(this.ExtensionCoroutine());
		}
	}

	// Token: 0x06003727 RID: 14119 RVA: 0x0001E59B File Offset: 0x0001C79B
	private IEnumerator ExtensionCoroutine()
	{
		float extensionTimer = Time.time + 0.425f;
		while (extensionTimer > Time.time)
		{
			yield return null;
		}
		base.Animator.SetBool("SpikesOut", true);
		base.StartCoroutine(this.PerformExtensionCheck());
		yield break;
	}

	// Token: 0x06003728 RID: 14120 RVA: 0x0001E5AA File Offset: 0x0001C7AA
	public override void Initialize(HazardArgs hazardArgs)
	{
		base.InitialState = hazardArgs.InitialState;
	}

	// Token: 0x06003729 RID: 14121 RVA: 0x0001E5B8 File Offset: 0x0001C7B8
	private IEnumerator PerformExtensionCheck()
	{
		while (!this.m_retractSpikes)
		{
			yield return null;
		}
		base.StartCoroutine(this.RetractionCoroutine());
		yield break;
	}

	// Token: 0x0600372A RID: 14122 RVA: 0x0001E5C7 File Offset: 0x0001C7C7
	private IEnumerator RetractionCoroutine()
	{
		this.m_extendSpikes = false;
		float retractionTimer = Time.time + 0.45f;
		while (retractionTimer > Time.time)
		{
			yield return null;
		}
		base.Animator.SetBool("SpikesOut", false);
		yield break;
	}

	// Token: 0x0600372B RID: 14123 RVA: 0x0001E5D6 File Offset: 0x0001C7D6
	public void RetractSpike()
	{
		this.m_retractSpikes = true;
	}

	// Token: 0x0600372C RID: 14124 RVA: 0x0001E5DF File Offset: 0x0001C7DF
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		this.ResetHazard();
	}

	// Token: 0x0600372D RID: 14125 RVA: 0x0001E5E7 File Offset: 0x0001C7E7
	public override void ResetHazard()
	{
		base.Animator.SetBool("SpikesOut", false);
		base.Animator.SetTrigger("Reset");
		this.m_extendSpikes = false;
	}

	// Token: 0x04002C78 RID: 11384
	private const float m_extensionDelay = 0.425f;

	// Token: 0x04002C79 RID: 11385
	private const float m_retractionDelay = 0.45f;

	// Token: 0x04002C7A RID: 11386
	private bool m_extendSpikes;

	// Token: 0x04002C7B RID: 11387
	private bool m_retractSpikes;
}

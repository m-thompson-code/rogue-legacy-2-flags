using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000443 RID: 1091
public class BearTrap_Hazard : Hazard, ILevelConsumer
{
	// Token: 0x17000FC8 RID: 4040
	// (get) Token: 0x06002805 RID: 10245 RVA: 0x000848B3 File Offset: 0x00082AB3
	public override float BaseDamage
	{
		get
		{
			return 30f;
		}
	}

	// Token: 0x06002806 RID: 10246 RVA: 0x000848BC File Offset: 0x00082ABC
	private void OnEnable()
	{
		BaseRoom componentInParent = base.GetComponentInParent<BaseRoom>();
		if (componentInParent != null)
		{
			componentInParent.PlayerExitRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom), false);
		}
	}

	// Token: 0x06002807 RID: 10247 RVA: 0x000848F4 File Offset: 0x00082AF4
	protected override void OnDisable()
	{
		base.OnDisable();
		BaseRoom componentInParent = base.GetComponentInParent<BaseRoom>();
		if (componentInParent != null)
		{
			componentInParent.PlayerExitRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom));
		}
	}

	// Token: 0x06002808 RID: 10248 RVA: 0x0008492F File Offset: 0x00082B2F
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

	// Token: 0x06002809 RID: 10249 RVA: 0x0008495A File Offset: 0x00082B5A
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

	// Token: 0x0600280A RID: 10250 RVA: 0x00084969 File Offset: 0x00082B69
	public override void Initialize(HazardArgs hazardArgs)
	{
		base.InitialState = hazardArgs.InitialState;
	}

	// Token: 0x0600280B RID: 10251 RVA: 0x00084977 File Offset: 0x00082B77
	private IEnumerator PerformExtensionCheck()
	{
		while (!this.m_retractSpikes)
		{
			yield return null;
		}
		base.StartCoroutine(this.RetractionCoroutine());
		yield break;
	}

	// Token: 0x0600280C RID: 10252 RVA: 0x00084986 File Offset: 0x00082B86
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

	// Token: 0x0600280D RID: 10253 RVA: 0x00084995 File Offset: 0x00082B95
	public void RetractSpike()
	{
		this.m_retractSpikes = true;
	}

	// Token: 0x0600280E RID: 10254 RVA: 0x0008499E File Offset: 0x00082B9E
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		this.ResetHazard();
	}

	// Token: 0x0600280F RID: 10255 RVA: 0x000849A6 File Offset: 0x00082BA6
	public override void ResetHazard()
	{
		base.Animator.SetBool("SpikesOut", false);
		base.Animator.SetTrigger("Reset");
		this.m_extendSpikes = false;
	}

	// Token: 0x04002140 RID: 8512
	private const float m_extensionDelay = 0.425f;

	// Token: 0x04002141 RID: 8513
	private const float m_retractionDelay = 0.45f;

	// Token: 0x04002142 RID: 8514
	private bool m_extendSpikes;

	// Token: 0x04002143 RID: 8515
	private bool m_retractSpikes;
}

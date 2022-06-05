using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004CC RID: 1228
public class TimedCrumblePlatform : CrumblePlatform
{
	// Token: 0x06002DA7 RID: 11687 RVA: 0x0009A250 File Offset: 0x00098450
	protected override void Awake()
	{
		base.Awake();
		this.m_crumbleDelayYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06002DA8 RID: 11688 RVA: 0x0009A269 File Offset: 0x00098469
	protected override IEnumerator StartCrumbleCoroutine()
	{
		if (this.m_crackedAnimCoroutine != null)
		{
			base.StopCoroutine(this.m_crackedAnimCoroutine);
		}
		this.m_crackedAnimCoroutine = base.StartCoroutine(this.CrackedAnimCoroutine(0.25f));
		this.m_crumbleDelayYield.CreateNew(1f, false);
		yield return this.m_crumbleDelayYield;
		if (this.m_crackedAnimCoroutine != null)
		{
			base.StopCoroutine(this.m_crackedAnimCoroutine);
		}
		this.m_platformCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		this.m_terrainCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		this.m_animator.SetTrigger("Crumble");
		if (this.m_reformCoroutine != null)
		{
			base.StopCoroutine(this.m_reformCoroutine);
			this.m_reformCoroutine = null;
		}
		this.m_reformCoroutine = base.StartCoroutine(base.ReformCoroutine(1f));
		yield break;
	}

	// Token: 0x06002DA9 RID: 11689 RVA: 0x0009A278 File Offset: 0x00098478
	private IEnumerator CrackedAnimCoroutine(float animInterval)
	{
		float startTime = Time.time;
		for (;;)
		{
			if (Time.time > startTime + animInterval)
			{
				startTime = Time.time;
				AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(0);
				this.m_animator.Play(currentAnimatorStateInfo.shortNameHash, 0, 0f);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002DAA RID: 11690 RVA: 0x0009A28E File Offset: 0x0009848E
	public override void ResetCrumble()
	{
		base.ResetCrumble();
		if (this.m_crackedAnimCoroutine != null)
		{
			base.StopCoroutine(this.m_crackedAnimCoroutine);
		}
	}

	// Token: 0x0400248E RID: 9358
	private WaitRL_Yield m_crumbleDelayYield;

	// Token: 0x0400248F RID: 9359
	private Coroutine m_crackedAnimCoroutine;
}

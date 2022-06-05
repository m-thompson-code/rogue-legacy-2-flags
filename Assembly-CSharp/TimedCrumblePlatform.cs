using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007F4 RID: 2036
public class TimedCrumblePlatform : CrumblePlatform
{
	// Token: 0x06003EB3 RID: 16051 RVA: 0x00022A7E File Offset: 0x00020C7E
	protected override void Awake()
	{
		base.Awake();
		this.m_crumbleDelayYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06003EB4 RID: 16052 RVA: 0x00022A97 File Offset: 0x00020C97
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

	// Token: 0x06003EB5 RID: 16053 RVA: 0x00022AA6 File Offset: 0x00020CA6
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

	// Token: 0x06003EB6 RID: 16054 RVA: 0x00022ABC File Offset: 0x00020CBC
	public override void ResetCrumble()
	{
		base.ResetCrumble();
		if (this.m_crackedAnimCoroutine != null)
		{
			base.StopCoroutine(this.m_crackedAnimCoroutine);
		}
	}

	// Token: 0x0400312B RID: 12587
	private WaitRL_Yield m_crumbleDelayYield;

	// Token: 0x0400312C RID: 12588
	private Coroutine m_crackedAnimCoroutine;
}

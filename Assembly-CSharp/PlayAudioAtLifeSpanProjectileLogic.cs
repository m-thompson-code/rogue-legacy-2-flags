using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

// Token: 0x020007B3 RID: 1971
public class PlayAudioAtLifeSpanProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003BF9 RID: 15353 RVA: 0x00021189 File Offset: 0x0001F389
	private void OnEnable()
	{
		if (base.SourceProjectile)
		{
			base.StartCoroutine(this.PlayAudioCoroutine());
		}
	}

	// Token: 0x06003BFA RID: 15354 RVA: 0x000211A5 File Offset: 0x0001F3A5
	private IEnumerator PlayAudioCoroutine()
	{
		float num = base.SourceProjectile.Lifespan * this.m_triggerAtNormalizedLifeSpan;
		float endTime = Time.time + num;
		while (Time.time < endTime)
		{
			yield return null;
		}
		this.m_eventEmitter.Play();
		yield break;
	}

	// Token: 0x04002F9E RID: 12190
	[SerializeField]
	private float m_triggerAtNormalizedLifeSpan = 0.8f;

	// Token: 0x04002F9F RID: 12191
	[SerializeField]
	private StudioEventEmitter m_eventEmitter;
}

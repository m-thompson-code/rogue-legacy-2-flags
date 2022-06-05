using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

// Token: 0x020004A7 RID: 1191
public class PlayAudioAtLifeSpanProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B8C RID: 11148 RVA: 0x00093E4F File Offset: 0x0009204F
	private void OnEnable()
	{
		if (base.SourceProjectile)
		{
			base.StartCoroutine(this.PlayAudioCoroutine());
		}
	}

	// Token: 0x06002B8D RID: 11149 RVA: 0x00093E6B File Offset: 0x0009206B
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

	// Token: 0x04002368 RID: 9064
	[SerializeField]
	private float m_triggerAtNormalizedLifeSpan = 0.8f;

	// Token: 0x04002369 RID: 9065
	[SerializeField]
	private StudioEventEmitter m_eventEmitter;
}

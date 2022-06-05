using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200068C RID: 1676
public class VoidDissolveComponent : MonoBehaviour
{
	// Token: 0x1700138A RID: 5002
	// (get) Token: 0x0600333B RID: 13115 RVA: 0x0001C185 File Offset: 0x0001A385
	// (set) Token: 0x0600333C RID: 13116 RVA: 0x0001C18D File Offset: 0x0001A38D
	public bool IsDissolving { get; private set; }

	// Token: 0x0600333D RID: 13117 RVA: 0x000DABF8 File Offset: 0x000D8DF8
	private void Awake()
	{
		this.m_initialValues = new float[this.m_renderers.Length];
		int num = this.m_renderers.Length;
		for (int i = 0; i < num; i++)
		{
			this.m_renderers[i].material.GetFloat("_AlphaGain");
		}
	}

	// Token: 0x0600333E RID: 13118 RVA: 0x0001C196 File Offset: 0x0001A396
	public void StartDissolve(bool playSparkle)
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.DissolveCoroutine(playSparkle));
	}

	// Token: 0x0600333F RID: 13119 RVA: 0x0001C1AC File Offset: 0x0001A3AC
	public IEnumerator DissolveCoroutine(bool playSparkle)
	{
		if (this.m_renderers == null)
		{
			yield break;
		}
		if (playSparkle)
		{
			Vector3 effectPosition = new Vector3(base.transform.position.x, PlayerManager.GetPlayerController().Midpoint.y);
			EffectManager.PlayEffect(base.gameObject, null, "VoidProjectilePassThrough_Effect", effectPosition, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		}
		this.IsDissolving = true;
		this.m_hasDissolved = true;
		float startTime = Time.time;
		float endTime = Time.time + this.m_duration;
		int rendererLength = this.m_renderers.Length;
		if (this.m_particleSystemsToStop != null)
		{
			ParticleSystem[] particleSystemsToStop = this.m_particleSystemsToStop;
			for (int i = 0; i < particleSystemsToStop.Length; i++)
			{
				particleSystemsToStop[i].Stop();
			}
		}
		while (Time.time < endTime)
		{
			float t = Time.time - startTime;
			for (int j = 0; j < rendererLength; j++)
			{
				float b = this.m_initialValues[j];
				float value = Ease.None(t, b, -1f, this.m_duration);
				this.m_renderers[j].material.SetFloat("_AlphaGain", value);
			}
			yield return null;
		}
		Renderer[] renderers = this.m_renderers;
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].material.SetFloat("_AlphaGain", -1f);
		}
		this.IsDissolving = false;
		yield break;
	}

	// Token: 0x06003340 RID: 13120 RVA: 0x000DAC48 File Offset: 0x000D8E48
	public void ForceDissolved()
	{
		this.m_hasDissolved = true;
		Renderer[] renderers = this.m_renderers;
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].material.SetFloat("_AlphaGain", -1f);
		}
	}

	// Token: 0x06003341 RID: 13121 RVA: 0x0001C1C2 File Offset: 0x0001A3C2
	private void OnDisable()
	{
		if (this.m_hasDissolved)
		{
			this.Reset();
		}
	}

	// Token: 0x06003342 RID: 13122 RVA: 0x000DAC88 File Offset: 0x000D8E88
	public void Reset()
	{
		this.IsDissolving = false;
		this.m_hasDissolved = false;
		if (this.m_renderers != null)
		{
			for (int i = 0; i < this.m_renderers.Length; i++)
			{
				this.m_renderers[i].material.SetFloat("_AlphaGain", this.m_initialValues[i]);
			}
		}
	}

	// Token: 0x040029CD RID: 10701
	[SerializeField]
	private float m_duration = 1f;

	// Token: 0x040029CE RID: 10702
	[SerializeField]
	private Renderer[] m_renderers;

	// Token: 0x040029CF RID: 10703
	[SerializeField]
	private ParticleSystem[] m_particleSystemsToStop;

	// Token: 0x040029D0 RID: 10704
	private float[] m_initialValues;

	// Token: 0x040029D1 RID: 10705
	private bool m_hasDissolved;
}

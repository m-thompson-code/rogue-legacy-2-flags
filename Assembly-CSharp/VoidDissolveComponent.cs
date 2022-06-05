using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003EA RID: 1002
public class VoidDissolveComponent : MonoBehaviour
{
	// Token: 0x17000EE7 RID: 3815
	// (get) Token: 0x060024F3 RID: 9459 RVA: 0x0007AC6A File Offset: 0x00078E6A
	// (set) Token: 0x060024F4 RID: 9460 RVA: 0x0007AC72 File Offset: 0x00078E72
	public bool IsDissolving { get; private set; }

	// Token: 0x060024F5 RID: 9461 RVA: 0x0007AC7C File Offset: 0x00078E7C
	private void Awake()
	{
		this.m_initialValues = new float[this.m_renderers.Length];
		int num = this.m_renderers.Length;
		for (int i = 0; i < num; i++)
		{
			this.m_renderers[i].material.GetFloat("_AlphaGain");
		}
	}

	// Token: 0x060024F6 RID: 9462 RVA: 0x0007ACC9 File Offset: 0x00078EC9
	public void StartDissolve(bool playSparkle)
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.DissolveCoroutine(playSparkle));
	}

	// Token: 0x060024F7 RID: 9463 RVA: 0x0007ACDF File Offset: 0x00078EDF
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

	// Token: 0x060024F8 RID: 9464 RVA: 0x0007ACF8 File Offset: 0x00078EF8
	public void ForceDissolved()
	{
		this.m_hasDissolved = true;
		Renderer[] renderers = this.m_renderers;
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].material.SetFloat("_AlphaGain", -1f);
		}
	}

	// Token: 0x060024F9 RID: 9465 RVA: 0x0007AD38 File Offset: 0x00078F38
	private void OnDisable()
	{
		if (this.m_hasDissolved)
		{
			this.Reset();
		}
	}

	// Token: 0x060024FA RID: 9466 RVA: 0x0007AD48 File Offset: 0x00078F48
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

	// Token: 0x04001F4B RID: 8011
	[SerializeField]
	private float m_duration = 1f;

	// Token: 0x04001F4C RID: 8012
	[SerializeField]
	private Renderer[] m_renderers;

	// Token: 0x04001F4D RID: 8013
	[SerializeField]
	private ParticleSystem[] m_particleSystemsToStop;

	// Token: 0x04001F4E RID: 8014
	private float[] m_initialValues;

	// Token: 0x04001F4F RID: 8015
	private bool m_hasDissolved;
}

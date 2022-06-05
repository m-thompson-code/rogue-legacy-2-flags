using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006E5 RID: 1765
public class ObjectShakeEffect : BaseEffect
{
	// Token: 0x17001465 RID: 5221
	// (get) Token: 0x0600360A RID: 13834 RVA: 0x0001D9F5 File Offset: 0x0001BBF5
	private bool UseRectTransform
	{
		get
		{
			return this.m_sourceRectTransform != null;
		}
	}

	// Token: 0x0600360B RID: 13835 RVA: 0x0001DA03 File Offset: 0x0001BC03
	public override void Play(float duration = 0f, EffectStopType stopType = EffectStopType.Gracefully)
	{
		base.Play(duration, stopType);
		base.StartCoroutine(this.PlayTimedObjectShake(duration));
	}

	// Token: 0x0600360C RID: 13836 RVA: 0x0001DA1B File Offset: 0x0001BC1B
	private IEnumerator PlayTimedObjectShake(float duration)
	{
		this.m_sourceRectTransform = base.Source.GetComponent<RectTransform>();
		this.m_sourceTransform = base.Source.transform;
		if (this.UseRectTransform)
		{
			this.m_startingPos = this.m_sourceRectTransform.anchoredPosition;
			float startingTime = Time.unscaledTime;
			while (Time.unscaledTime < startingTime + duration)
			{
				this.m_sourceRectTransform.anchoredPosition = this.m_startingPos + UnityEngine.Random.insideUnitSphere * this.m_shakeAmplitude;
				yield return null;
			}
		}
		else
		{
			this.m_startingPos = this.m_sourceTransform.localPosition;
			float startingTime = Time.unscaledTime;
			while (Time.unscaledTime < startingTime + duration)
			{
				this.m_sourceTransform.localPosition = this.m_startingPos + UnityEngine.Random.insideUnitSphere * this.m_shakeAmplitude;
				yield return null;
			}
		}
		this.Stop(EffectStopType.Gracefully);
		yield break;
	}

	// Token: 0x0600360D RID: 13837 RVA: 0x0001DA31 File Offset: 0x0001BC31
	public override void Stop(EffectStopType stopType)
	{
		base.StopAllCoroutines();
		if (this.UseRectTransform)
		{
			this.m_sourceRectTransform.anchoredPosition = this.m_startingPos;
		}
		else
		{
			this.m_sourceTransform.localPosition = this.m_startingPos;
		}
		this.PlayComplete();
	}

	// Token: 0x04002BE5 RID: 11237
	[SerializeField]
	private float m_shakeAmplitude = 5f;

	// Token: 0x04002BE6 RID: 11238
	private Vector3 m_startingPos;

	// Token: 0x04002BE7 RID: 11239
	private Transform m_sourceTransform;

	// Token: 0x04002BE8 RID: 11240
	private RectTransform m_sourceRectTransform;
}

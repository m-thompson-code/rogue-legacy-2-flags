using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000424 RID: 1060
public class ObjectShakeEffect : BaseEffect
{
	// Token: 0x17000F90 RID: 3984
	// (get) Token: 0x06002725 RID: 10021 RVA: 0x0008289B File Offset: 0x00080A9B
	private bool UseRectTransform
	{
		get
		{
			return this.m_sourceRectTransform != null;
		}
	}

	// Token: 0x06002726 RID: 10022 RVA: 0x000828A9 File Offset: 0x00080AA9
	public override void Play(float duration = 0f, EffectStopType stopType = EffectStopType.Gracefully)
	{
		base.Play(duration, stopType);
		base.StartCoroutine(this.PlayTimedObjectShake(duration));
	}

	// Token: 0x06002727 RID: 10023 RVA: 0x000828C1 File Offset: 0x00080AC1
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

	// Token: 0x06002728 RID: 10024 RVA: 0x000828D7 File Offset: 0x00080AD7
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

	// Token: 0x040020D9 RID: 8409
	[SerializeField]
	private float m_shakeAmplitude = 5f;

	// Token: 0x040020DA RID: 8410
	private Vector3 m_startingPos;

	// Token: 0x040020DB RID: 8411
	private Transform m_sourceTransform;

	// Token: 0x040020DC RID: 8412
	private RectTransform m_sourceRectTransform;
}

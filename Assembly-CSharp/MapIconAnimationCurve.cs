using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200042A RID: 1066
public class MapIconAnimationCurve : MonoBehaviour
{
	// Token: 0x0600225F RID: 8799 RVA: 0x000125D6 File Offset: 0x000107D6
	private void OnEnable()
	{
		if (this.m_animateOnEnable)
		{
			if (this.m_overrideMapControllerValues)
			{
				base.StartCoroutine(this.AnimationCoroutine());
				return;
			}
			base.StartCoroutine(this.GlobalAnimationCoroutine());
		}
	}

	// Token: 0x06002260 RID: 8800 RVA: 0x00012603 File Offset: 0x00010803
	private void OnBecameVisible()
	{
		if (base.enabled)
		{
			if (this.m_overrideMapControllerValues)
			{
				base.StartCoroutine(this.AnimationCoroutine());
				return;
			}
			base.StartCoroutine(this.GlobalAnimationCoroutine());
		}
	}

	// Token: 0x06002261 RID: 8801 RVA: 0x00012630 File Offset: 0x00010830
	private void OnBecameInvisible()
	{
		if (base.enabled)
		{
			base.StopAllCoroutines();
		}
	}

	// Token: 0x06002262 RID: 8802 RVA: 0x00012640 File Offset: 0x00010840
	private IEnumerator GlobalAnimationCoroutine()
	{
		for (;;)
		{
			float num = 1f + MapController.MapIconAnimationCurveValue;
			base.transform.localScale = new Vector3(num, num, num);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002263 RID: 8803 RVA: 0x0001264F File Offset: 0x0001084F
	private IEnumerator AnimationCoroutine()
	{
		base.transform.localScale = Vector3.one;
		float startTime = Time.unscaledTime;
		float elapsedPercent = 0f;
		float duration = this.m_animCurve.keys[this.m_animCurve.keys.Length - 1].time;
		while (elapsedPercent < 1f)
		{
			elapsedPercent = (Time.unscaledTime - startTime) / duration;
			float num = 1f + this.m_animCurve.Evaluate(elapsedPercent);
			base.transform.localScale = new Vector3(num, num, num);
			yield return null;
		}
		float delay = Time.unscaledTime + this.m_animLoopdelay;
		while (Time.unscaledTime < delay)
		{
			yield return null;
		}
		base.StartCoroutine(this.AnimationCoroutine());
		yield break;
	}

	// Token: 0x04001F05 RID: 7941
	[SerializeField]
	private bool m_overrideMapControllerValues;

	// Token: 0x04001F06 RID: 7942
	[SerializeField]
	private bool m_animateOnEnable;

	// Token: 0x04001F07 RID: 7943
	[SerializeField]
	[ConditionalHide("m_overrideMapControllerValues", true)]
	private AnimationCurve m_animCurve;

	// Token: 0x04001F08 RID: 7944
	[SerializeField]
	[ConditionalHide("m_overrideMapControllerValues", true)]
	private float m_animLoopdelay = 1f;
}

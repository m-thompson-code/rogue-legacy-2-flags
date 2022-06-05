using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000265 RID: 613
public class MapIconAnimationCurve : MonoBehaviour
{
	// Token: 0x06001882 RID: 6274 RVA: 0x0004CCA3 File Offset: 0x0004AEA3
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

	// Token: 0x06001883 RID: 6275 RVA: 0x0004CCD0 File Offset: 0x0004AED0
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

	// Token: 0x06001884 RID: 6276 RVA: 0x0004CCFD File Offset: 0x0004AEFD
	private void OnBecameInvisible()
	{
		if (base.enabled)
		{
			base.StopAllCoroutines();
		}
	}

	// Token: 0x06001885 RID: 6277 RVA: 0x0004CD0D File Offset: 0x0004AF0D
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

	// Token: 0x06001886 RID: 6278 RVA: 0x0004CD1C File Offset: 0x0004AF1C
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

	// Token: 0x040017D0 RID: 6096
	[SerializeField]
	private bool m_overrideMapControllerValues;

	// Token: 0x040017D1 RID: 6097
	[SerializeField]
	private bool m_animateOnEnable;

	// Token: 0x040017D2 RID: 6098
	[SerializeField]
	[ConditionalHide("m_overrideMapControllerValues", true)]
	private AnimationCurve m_animCurve;

	// Token: 0x040017D3 RID: 6099
	[SerializeField]
	[ConditionalHide("m_overrideMapControllerValues", true)]
	private float m_animLoopdelay = 1f;
}

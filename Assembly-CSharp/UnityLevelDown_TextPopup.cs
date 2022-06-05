using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000542 RID: 1346
public class UnityLevelDown_TextPopup : TextPopupObj
{
	// Token: 0x0600313D RID: 12605 RVA: 0x000A6BE0 File Offset: 0x000A4DE0
	protected override IEnumerator SpawnEffectCoroutine()
	{
		float speed = 0.35f;
		yield return TweenManager.TweenBy_UnscaledTime(base.transform, speed, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"localPosition.y",
			1
		}).TweenCoroutine;
		float startTime = Time.unscaledTime;
		float waitTime = 0.5f;
		while (startTime + waitTime > Time.unscaledTime)
		{
			yield return null;
		}
		TweenManager.TweenBy_UnscaledTime(base.transform, speed, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"localPosition.y",
			1
		});
		yield return TweenManager.TweenTo_UnscaledTime(this.m_tmpText, speed / 2f, new EaseDelegate(Ease.None), new object[]
		{
			"delay",
			speed / 2f,
			"alpha",
			0
		}).TweenCoroutine;
		base.gameObject.SetActive(false);
		yield break;
	}
}

using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008DF RID: 2271
public class UnityLevelDown_TextPopup : TextPopupObj
{
	// Token: 0x060044E5 RID: 17637 RVA: 0x00025DD8 File Offset: 0x00023FD8
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

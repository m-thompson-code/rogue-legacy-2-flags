using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200053F RID: 1343
public class PlayerRankUp_TextPopup : TextPopupObj
{
	// Token: 0x06003122 RID: 12578 RVA: 0x000A6977 File Offset: 0x000A4B77
	protected override IEnumerator SpawnEffectCoroutine()
	{
		float speed = 0.35f;
		yield return TweenManager.TweenBy_UnscaledTime(base.transform, speed, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"localPosition.y",
			0.5f
		}).TweenCoroutine;
		float startTime = Time.unscaledTime;
		while (Time.unscaledTime < startTime + 0.75f)
		{
			yield return null;
		}
		TweenManager.TweenBy_UnscaledTime(base.transform, speed, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"localPosition.y",
			0.5f
		});
		yield return TweenManager.TweenTo_UnscaledTime(this.m_tmpText, speed, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		}).TweenCoroutine;
		base.gameObject.SetActive(false);
		yield break;
	}
}

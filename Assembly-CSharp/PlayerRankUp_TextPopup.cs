using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008D9 RID: 2265
public class PlayerRankUp_TextPopup : TextPopupObj
{
	// Token: 0x060044B8 RID: 17592 RVA: 0x00025C8A File Offset: 0x00023E8A
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

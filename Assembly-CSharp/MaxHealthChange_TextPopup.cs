using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008D7 RID: 2263
public class MaxHealthChange_TextPopup : TextPopupObj
{
	// Token: 0x060044B0 RID: 17584 RVA: 0x00025C64 File Offset: 0x00023E64
	protected override IEnumerator SpawnEffectCoroutine()
	{
		float speed = 0.35f;
		base.transform.SetLocalPositionX(base.transform.localPosition.x - 50f);
		yield return TweenManager.TweenBy(base.transform, speed, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"localPosition.x",
			50
		}).TweenCoroutine;
		float timeDelay = Time.time + 1f;
		while (Time.time < timeDelay)
		{
			yield return null;
		}
		TweenManager.TweenBy(base.transform, speed, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"localPosition.x",
			100
		});
		yield return TweenManager.TweenTo(this.m_tmpText, speed / 2f, new EaseDelegate(Ease.None), new object[]
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

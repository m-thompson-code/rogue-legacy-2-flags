using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008E1 RID: 2273
public class XPGain_TextPopup : TextPopupObj
{
	// Token: 0x1700188F RID: 6287
	// (get) Token: 0x060044ED RID: 17645 RVA: 0x00025DFE File Offset: 0x00023FFE
	// (set) Token: 0x060044EE RID: 17646 RVA: 0x00025E06 File Offset: 0x00024006
	public bool FastPlay { get; set; }

	// Token: 0x060044EF RID: 17647 RVA: 0x00025E0F File Offset: 0x0002400F
	protected override IEnumerator SpawnEffectCoroutine()
	{
		float speed = 0.25f;
		Vector3 localScale = base.transform.localScale;
		base.transform.localScale *= 0.75f;
		this.m_tmpText.alpha = 1f;
		TweenManager.TweenBy(base.transform, speed, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"localPosition.y",
			0.5f
		});
		yield return TweenManager.TweenTo(base.transform, speed, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"localScale.x",
			localScale.x,
			"localScale.y",
			localScale.y,
			"localScale.z",
			localScale.z
		}).TweenCoroutine;
		float startTime = Time.time;
		while (Time.time < startTime + 0.25f)
		{
			yield return null;
		}
		yield return TweenManager.TweenTo(this.m_tmpText, speed, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		}).TweenCoroutine;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060044F0 RID: 17648 RVA: 0x00025E1E File Offset: 0x0002401E
	public override void ResetValues()
	{
		this.FastPlay = false;
		base.ResetValues();
	}
}

using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000543 RID: 1347
public class XPGain_TextPopup : TextPopupObj
{
	// Token: 0x1700122C RID: 4652
	// (get) Token: 0x0600313F RID: 12607 RVA: 0x000A6BF7 File Offset: 0x000A4DF7
	// (set) Token: 0x06003140 RID: 12608 RVA: 0x000A6BFF File Offset: 0x000A4DFF
	public bool FastPlay { get; set; }

	// Token: 0x06003141 RID: 12609 RVA: 0x000A6C08 File Offset: 0x000A4E08
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

	// Token: 0x06003142 RID: 12610 RVA: 0x000A6C17 File Offset: 0x000A4E17
	public override void ResetValues()
	{
		this.FastPlay = false;
		base.ResetValues();
	}
}

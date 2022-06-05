using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200053D RID: 1341
public class Interact_TextPopup : TextPopupObj
{
	// Token: 0x0600311E RID: 12574 RVA: 0x000A6949 File Offset: 0x000A4B49
	protected override IEnumerator SpawnEffectCoroutine()
	{
		Vector3 localScale = base.transform.localScale;
		base.transform.localScale = localScale * 0.5f;
		yield return TweenManager.TweenTo(base.transform, 0.15f, new EaseDelegate(Ease.Back.EaseOutLarge), new object[]
		{
			"localScale.x",
			localScale.x,
			"localScale.y",
			localScale.y,
			"localScale.z",
			localScale.z
		}).TweenCoroutine;
		yield break;
	}
}

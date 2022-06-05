using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008D5 RID: 2261
public class Interact_TextPopup : TextPopupObj
{
	// Token: 0x060044A8 RID: 17576 RVA: 0x00025C3E File Offset: 0x00023E3E
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

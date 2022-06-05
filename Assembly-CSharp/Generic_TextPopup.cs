using System;
using System.Collections;

// Token: 0x0200053C RID: 1340
public class Generic_TextPopup : TextPopupObj
{
	// Token: 0x0600311C RID: 12572 RVA: 0x000A6932 File Offset: 0x000A4B32
	protected override IEnumerator SpawnEffectCoroutine()
	{
		float speed = 0.35f;
		yield return TweenManager.TweenBy(base.transform, speed, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"localPosition.y",
			2
		}).TweenCoroutine;
		TweenManager.TweenBy(base.transform, speed, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"localPosition.y",
			-1
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

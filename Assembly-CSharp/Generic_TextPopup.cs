using System;
using System.Collections;

// Token: 0x020008D3 RID: 2259
public class Generic_TextPopup : TextPopupObj
{
	// Token: 0x060044A0 RID: 17568 RVA: 0x00025C18 File Offset: 0x00023E18
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

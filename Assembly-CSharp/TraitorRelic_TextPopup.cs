using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008DD RID: 2269
public class TraitorRelic_TextPopup : TextPopupObj
{
	// Token: 0x1700188A RID: 6282
	// (get) Token: 0x060044DB RID: 17627 RVA: 0x00025DAA File Offset: 0x00023FAA
	// (set) Token: 0x060044DC RID: 17628 RVA: 0x0010F634 File Offset: 0x0010D834
	public RelicType RelicType
	{
		get
		{
			return this.m_relicType;
		}
		set
		{
			this.m_relicType = value;
			Sprite relicSprite = IconLibrary.GetRelicSprite(this.m_relicType, true);
			this.m_relicIcon.sprite = relicSprite;
		}
	}

	// Token: 0x060044DD RID: 17629 RVA: 0x00025DB2 File Offset: 0x00023FB2
	protected override IEnumerator SpawnEffectCoroutine()
	{
		float speed = 0.35f;
		Color color = this.m_relicIcon.color;
		color.a = 0f;
		this.m_relicIcon.color = color;
		TweenManager.TweenBy(this.m_relicIcon, speed, new EaseDelegate(Ease.None), new object[]
		{
			"color.a",
			1
		});
		yield return TweenManager.TweenBy(base.transform, speed, new EaseDelegate(Ease.None), new object[]
		{
			"localPosition.y",
			0.5f
		}).TweenCoroutine;
		yield return TweenManager.TweenBy(base.transform, 1f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"localPosition.y",
			0.5f
		}).TweenCoroutine;
		TweenManager.TweenBy(base.transform, speed, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"localPosition.y",
			-0.25f
		});
		TweenManager.TweenTo(this.m_tmpText, speed / 2f, new EaseDelegate(Ease.None), new object[]
		{
			"delay",
			speed / 2f,
			"alpha",
			0
		});
		yield return TweenManager.TweenTo(this.m_relicIcon, speed / 2f, new EaseDelegate(Ease.None), new object[]
		{
			"delay",
			speed / 2f,
			"color.a",
			0
		}).TweenCoroutine;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x04003558 RID: 13656
	[SerializeField]
	private SpriteRenderer m_relicIcon;

	// Token: 0x04003559 RID: 13657
	private RelicType m_relicType;
}

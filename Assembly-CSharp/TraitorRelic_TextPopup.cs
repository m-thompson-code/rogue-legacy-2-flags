using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000541 RID: 1345
public class TraitorRelic_TextPopup : TextPopupObj
{
	// Token: 0x1700122B RID: 4651
	// (get) Token: 0x06003139 RID: 12601 RVA: 0x000A6B94 File Offset: 0x000A4D94
	// (set) Token: 0x0600313A RID: 12602 RVA: 0x000A6B9C File Offset: 0x000A4D9C
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

	// Token: 0x0600313B RID: 12603 RVA: 0x000A6BC9 File Offset: 0x000A4DC9
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

	// Token: 0x040026DC RID: 9948
	[SerializeField]
	private SpriteRenderer m_relicIcon;

	// Token: 0x040026DD RID: 9949
	private RelicType m_relicType;
}

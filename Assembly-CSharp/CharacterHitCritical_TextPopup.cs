using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000539 RID: 1337
public class CharacterHitCritical_TextPopup : TextPopupObj
{
	// Token: 0x06003114 RID: 12564 RVA: 0x000A6837 File Offset: 0x000A4A37
	protected override IEnumerator SpawnEffectCoroutine()
	{
		int length = base.Text.Replace("-", "").Length;
		if (length <= 2)
		{
			this.SetBackingActive(this.m_twoNumberBacking);
		}
		else if (length <= 3)
		{
			this.SetBackingActive(this.m_threeNumberBacking);
		}
		else
		{
			this.SetBackingActive(this.m_fourNumberBacking);
		}
		float num = UnityEngine.Random.Range(0.25f, 1f);
		if (CharacterHitCritical_TextPopup.FLIP_XPOS_STATIC)
		{
			num *= -1f;
		}
		CharacterHitCritical_TextPopup.FLIP_XPOS_STATIC = !CharacterHitCritical_TextPopup.FLIP_XPOS_STATIC;
		float y = UnityEngine.Random.Range(-0.5f, 0.5f);
		base.transform.Translate(num, y, 0f);
		float delay = this.m_animationClipDuration + Time.time;
		while (Time.time < delay)
		{
			yield return null;
		}
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06003115 RID: 12565 RVA: 0x000A6848 File Offset: 0x000A4A48
	private void SetBackingActive(SpriteRenderer backing)
	{
		if (this.m_twoNumberBacking != backing && this.m_twoNumberBacking.gameObject.activeSelf)
		{
			this.m_twoNumberBacking.gameObject.SetActive(false);
		}
		if (this.m_threeNumberBacking != backing && this.m_threeNumberBacking.gameObject.activeSelf)
		{
			this.m_threeNumberBacking.gameObject.SetActive(false);
		}
		if (this.m_fourNumberBacking != backing && this.m_fourNumberBacking.gameObject.activeSelf)
		{
			this.m_fourNumberBacking.gameObject.SetActive(false);
		}
		if (!backing.gameObject.activeSelf)
		{
			backing.gameObject.SetActive(true);
		}
	}

	// Token: 0x040026CC RID: 9932
	private static bool FLIP_XPOS_STATIC;

	// Token: 0x040026CD RID: 9933
	[SerializeField]
	private SpriteRenderer m_twoNumberBacking;

	// Token: 0x040026CE RID: 9934
	[SerializeField]
	private SpriteRenderer m_threeNumberBacking;

	// Token: 0x040026CF RID: 9935
	[SerializeField]
	private SpriteRenderer m_fourNumberBacking;
}

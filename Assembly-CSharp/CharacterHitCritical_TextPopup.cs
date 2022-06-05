using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008CD RID: 2253
public class CharacterHitCritical_TextPopup : TextPopupObj
{
	// Token: 0x06004486 RID: 17542 RVA: 0x00025BA5 File Offset: 0x00023DA5
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

	// Token: 0x06004487 RID: 17543 RVA: 0x0010EA64 File Offset: 0x0010CC64
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

	// Token: 0x04003526 RID: 13606
	private static bool FLIP_XPOS_STATIC;

	// Token: 0x04003527 RID: 13607
	[SerializeField]
	private SpriteRenderer m_twoNumberBacking;

	// Token: 0x04003528 RID: 13608
	[SerializeField]
	private SpriteRenderer m_threeNumberBacking;

	// Token: 0x04003529 RID: 13609
	[SerializeField]
	private SpriteRenderer m_fourNumberBacking;
}

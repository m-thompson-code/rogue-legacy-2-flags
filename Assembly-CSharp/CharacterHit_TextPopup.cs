using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200053A RID: 1338
public class CharacterHit_TextPopup : TextPopupObj
{
	// Token: 0x06003118 RID: 12568 RVA: 0x000A690B File Offset: 0x000A4B0B
	protected override IEnumerator SpawnEffectCoroutine()
	{
		float globalDelay = this.m_animationClipDuration + Time.time;
		float randomX = 0f;
		if (CharacterHit_TextPopup.m_goRight_STATIC)
		{
			randomX = UnityEngine.Random.Range(0.5f, 2f);
		}
		else
		{
			randomX = UnityEngine.Random.Range(-0.5f, -2f);
		}
		CharacterHit_TextPopup.m_goRight_STATIC = !CharacterHit_TextPopup.m_goRight_STATIC;
		float startingX = base.transform.localPosition.x;
		float xPos;
		while (Time.time < globalDelay)
		{
			yield return null;
			xPos = (1f - (globalDelay - Time.time) / this.m_animationClipDuration) * randomX + startingX;
			base.transform.SetLocalPositionX(xPos);
		}
		xPos = randomX + startingX;
		base.transform.SetLocalPositionX(xPos);
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x040026D0 RID: 9936
	private static bool m_goRight_STATIC;
}

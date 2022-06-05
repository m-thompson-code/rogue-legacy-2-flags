using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008CF RID: 2255
public class CharacterHit_TextPopup : TextPopupObj
{
	// Token: 0x06004490 RID: 17552 RVA: 0x00025BD3 File Offset: 0x00023DD3
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

	// Token: 0x0400352E RID: 13614
	private static bool m_goRight_STATIC;
}

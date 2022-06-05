using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x02000DD1 RID: 3537
	public class ChestPreviewItemDescriptionController : MonoBehaviour
	{
		// Token: 0x06006362 RID: 25442 RVA: 0x00036BE6 File Offset: 0x00034DE6
		private void OnEnable()
		{
			this.m_itemDescriptionCanvasGroup.alpha = 0f;
		}

		// Token: 0x06006363 RID: 25443 RVA: 0x00036BF8 File Offset: 0x00034DF8
		private IEnumerator AnimateTextPosition(float time, float distance)
		{
			yield return TweenManager.TweenBy_UnscaledTime(this.m_itemDescriptionCanvasGroup.transform, time, new EaseDelegate(Ease.Elastic.EaseOut), new object[]
			{
				"position.y",
				distance
			}).TweenCoroutine;
			yield break;
		}

		// Token: 0x06006364 RID: 25444 RVA: 0x00036C15 File Offset: 0x00034E15
		private IEnumerator FadeText(float time)
		{
			float timeStart = Time.unscaledTime;
			this.m_itemDescriptionCanvasGroup.alpha = 0f;
			while (Time.unscaledTime - timeStart < time)
			{
				this.m_itemDescriptionCanvasGroup.alpha = (Time.unscaledTime - timeStart) / time;
				yield return null;
			}
			this.m_itemDescriptionCanvasGroup.alpha = 1f;
			yield break;
		}

		// Token: 0x06006365 RID: 25445 RVA: 0x00172360 File Offset: 0x00170560
		public void Initialise(string name, string rarity, Color rarityTextColor, float popDistance, float popTime)
		{
			this.m_itemNameText.text = name;
			this.m_itemRarityText.text = rarity;
			this.m_itemRarityText.color = rarityTextColor;
			base.StartCoroutine(this.AnimateTextPosition(popTime, popDistance));
			base.StartCoroutine(this.FadeText(this.m_fadeTimeMultiplier * popTime));
		}

		// Token: 0x04005129 RID: 20777
		[SerializeField]
		private CanvasGroup m_itemDescriptionCanvasGroup;

		// Token: 0x0400512A RID: 20778
		[SerializeField]
		private TextMeshProUGUI m_itemNameText;

		// Token: 0x0400512B RID: 20779
		[SerializeField]
		private TextMeshProUGUI m_itemRarityText;

		// Token: 0x0400512C RID: 20780
		[SerializeField]
		[Range(0.1f, 1f)]
		private float m_fadeTimeMultiplier = 0.5f;
	}
}

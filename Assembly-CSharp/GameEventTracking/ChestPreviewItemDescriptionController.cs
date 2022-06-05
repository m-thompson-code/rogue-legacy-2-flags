using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x020008A3 RID: 2211
	public class ChestPreviewItemDescriptionController : MonoBehaviour
	{
		// Token: 0x06004826 RID: 18470 RVA: 0x001038B3 File Offset: 0x00101AB3
		private void OnEnable()
		{
			this.m_itemDescriptionCanvasGroup.alpha = 0f;
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x001038C5 File Offset: 0x00101AC5
		private IEnumerator AnimateTextPosition(float time, float distance)
		{
			yield return TweenManager.TweenBy_UnscaledTime(this.m_itemDescriptionCanvasGroup.transform, time, new EaseDelegate(Ease.Elastic.EaseOut), new object[]
			{
				"position.y",
				distance
			}).TweenCoroutine;
			yield break;
		}

		// Token: 0x06004828 RID: 18472 RVA: 0x001038E2 File Offset: 0x00101AE2
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

		// Token: 0x06004829 RID: 18473 RVA: 0x001038F8 File Offset: 0x00101AF8
		public void Initialise(string name, string rarity, Color rarityTextColor, float popDistance, float popTime)
		{
			this.m_itemNameText.text = name;
			this.m_itemRarityText.text = rarity;
			this.m_itemRarityText.color = rarityTextColor;
			base.StartCoroutine(this.AnimateTextPosition(popTime, popDistance));
			base.StartCoroutine(this.FadeText(this.m_fadeTimeMultiplier * popTime));
		}

		// Token: 0x04003CF8 RID: 15608
		[SerializeField]
		private CanvasGroup m_itemDescriptionCanvasGroup;

		// Token: 0x04003CF9 RID: 15609
		[SerializeField]
		private TextMeshProUGUI m_itemNameText;

		// Token: 0x04003CFA RID: 15610
		[SerializeField]
		private TextMeshProUGUI m_itemRarityText;

		// Token: 0x04003CFB RID: 15611
		[SerializeField]
		[Range(0.1f, 1f)]
		private float m_fadeTimeMultiplier = 0.5f;
	}
}

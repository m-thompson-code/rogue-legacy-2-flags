using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000D76 RID: 3446
	public class TextConsoleSimulator : MonoBehaviour
	{
		// Token: 0x060061EB RID: 25067 RVA: 0x00035FDD File Offset: 0x000341DD
		private void Awake()
		{
			this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
		}

		// Token: 0x060061EC RID: 25068 RVA: 0x00035FF0 File Offset: 0x000341F0
		private void Start()
		{
			base.StartCoroutine(this.RevealCharacters(this.m_TextComponent));
		}

		// Token: 0x060061ED RID: 25069 RVA: 0x00036005 File Offset: 0x00034205
		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x060061EE RID: 25070 RVA: 0x0003601D File Offset: 0x0003421D
		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x060061EF RID: 25071 RVA: 0x00036035 File Offset: 0x00034235
		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			this.hasTextChanged = true;
		}

		// Token: 0x060061F0 RID: 25072 RVA: 0x0003603E File Offset: 0x0003423E
		private IEnumerator RevealCharacters(TMP_Text textComponent)
		{
			textComponent.ForceMeshUpdate(false, false);
			TMP_TextInfo textInfo = textComponent.textInfo;
			int totalVisibleCharacters = textInfo.characterCount;
			int visibleCount = 0;
			for (;;)
			{
				if (this.hasTextChanged)
				{
					totalVisibleCharacters = textInfo.characterCount;
					this.hasTextChanged = false;
				}
				if (visibleCount > totalVisibleCharacters)
				{
					yield return new WaitForSeconds(1f);
					visibleCount = 0;
				}
				textComponent.maxVisibleCharacters = visibleCount;
				visibleCount++;
				yield return null;
			}
			yield break;
		}

		// Token: 0x060061F1 RID: 25073 RVA: 0x00036054 File Offset: 0x00034254
		private IEnumerator RevealWords(TMP_Text textComponent)
		{
			textComponent.ForceMeshUpdate(false, false);
			int totalWordCount = textComponent.textInfo.wordCount;
			int totalVisibleCharacters = textComponent.textInfo.characterCount;
			int counter = 0;
			int visibleCount = 0;
			for (;;)
			{
				int num = counter % (totalWordCount + 1);
				if (num == 0)
				{
					visibleCount = 0;
				}
				else if (num < totalWordCount)
				{
					visibleCount = textComponent.textInfo.wordInfo[num - 1].lastCharacterIndex + 1;
				}
				else if (num == totalWordCount)
				{
					visibleCount = totalVisibleCharacters;
				}
				textComponent.maxVisibleCharacters = visibleCount;
				if (visibleCount >= totalVisibleCharacters)
				{
					yield return new WaitForSeconds(1f);
				}
				counter++;
				yield return new WaitForSeconds(0.1f);
			}
			yield break;
		}

		// Token: 0x04004FED RID: 20461
		private TMP_Text m_TextComponent;

		// Token: 0x04004FEE RID: 20462
		private bool hasTextChanged;
	}
}

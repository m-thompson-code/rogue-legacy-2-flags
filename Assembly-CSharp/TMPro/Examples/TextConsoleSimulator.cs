using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000868 RID: 2152
	public class TextConsoleSimulator : MonoBehaviour
	{
		// Token: 0x0600472C RID: 18220 RVA: 0x001001BA File Offset: 0x000FE3BA
		private void Awake()
		{
			this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x001001CD File Offset: 0x000FE3CD
		private void Start()
		{
			base.StartCoroutine(this.RevealCharacters(this.m_TextComponent));
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x001001E2 File Offset: 0x000FE3E2
		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x001001FA File Offset: 0x000FE3FA
		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x00100212 File Offset: 0x000FE412
		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			this.hasTextChanged = true;
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x0010021B File Offset: 0x000FE41B
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

		// Token: 0x06004732 RID: 18226 RVA: 0x00100231 File Offset: 0x000FE431
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

		// Token: 0x04003C40 RID: 15424
		private TMP_Text m_TextComponent;

		// Token: 0x04003C41 RID: 15425
		private bool hasTextChanged;
	}
}

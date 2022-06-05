using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000D7D RID: 3453
	public class VertexColorCycler : MonoBehaviour
	{
		// Token: 0x06006213 RID: 25107 RVA: 0x00036144 File Offset: 0x00034344
		private void Awake()
		{
			this.m_TextComponent = base.GetComponent<TMP_Text>();
		}

		// Token: 0x06006214 RID: 25108 RVA: 0x00036152 File Offset: 0x00034352
		private void Start()
		{
			base.StartCoroutine(this.AnimateVertexColors());
		}

		// Token: 0x06006215 RID: 25109 RVA: 0x00036161 File Offset: 0x00034361
		private IEnumerator AnimateVertexColors()
		{
			this.m_TextComponent.ForceMeshUpdate(false, false);
			TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
			int currentCharacter = 0;
			Color32 color = this.m_TextComponent.color;
			for (;;)
			{
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
				{
					yield return new WaitForSeconds(0.25f);
				}
				else
				{
					int materialReferenceIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;
					Color32[] colors = textInfo.meshInfo[materialReferenceIndex].colors32;
					int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;
					if (textInfo.characterInfo[currentCharacter].isVisible)
					{
						color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), byte.MaxValue);
						colors[vertexIndex] = color;
						colors[vertexIndex + 1] = color;
						colors[vertexIndex + 2] = color;
						colors[vertexIndex + 3] = color;
						this.m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
					}
					currentCharacter = (currentCharacter + 1) % characterCount;
					yield return new WaitForSeconds(0.05f);
				}
			}
			yield break;
		}

		// Token: 0x0400501F RID: 20511
		private TMP_Text m_TextComponent;
	}
}

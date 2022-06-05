using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000867 RID: 2151
	public class TeleType : MonoBehaviour
	{
		// Token: 0x06004729 RID: 18217 RVA: 0x00100152 File Offset: 0x000FE352
		private void Awake()
		{
			this.m_textMeshPro = base.GetComponent<TMP_Text>();
			this.m_textMeshPro.text = this.label01;
			this.m_textMeshPro.enableWordWrapping = true;
			this.m_textMeshPro.alignment = TextAlignmentOptions.Top;
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x0010018D File Offset: 0x000FE38D
		private IEnumerator Start()
		{
			this.m_textMeshPro.ForceMeshUpdate(false, false);
			int totalVisibleCharacters = this.m_textMeshPro.textInfo.characterCount;
			int counter = 0;
			for (;;)
			{
				int num = counter % (totalVisibleCharacters + 1);
				this.m_textMeshPro.maxVisibleCharacters = num;
				if (num >= totalVisibleCharacters)
				{
					yield return new WaitForSeconds(1f);
					this.m_textMeshPro.text = this.label02;
					yield return new WaitForSeconds(1f);
					this.m_textMeshPro.text = this.label01;
					yield return new WaitForSeconds(1f);
				}
				counter++;
				yield return new WaitForSeconds(0.05f);
			}
			yield break;
		}

		// Token: 0x04003C3D RID: 15421
		private string label01 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=1>";

		// Token: 0x04003C3E RID: 15422
		private string label02 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=2>";

		// Token: 0x04003C3F RID: 15423
		private TMP_Text m_textMeshPro;
	}
}

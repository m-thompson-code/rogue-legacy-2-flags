using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000D74 RID: 3444
	public class TeleType : MonoBehaviour
	{
		// Token: 0x060061E2 RID: 25058 RVA: 0x00035F5E File Offset: 0x0003415E
		private void Awake()
		{
			this.m_textMeshPro = base.GetComponent<TMP_Text>();
			this.m_textMeshPro.text = this.label01;
			this.m_textMeshPro.enableWordWrapping = true;
			this.m_textMeshPro.alignment = TextAlignmentOptions.Top;
		}

		// Token: 0x060061E3 RID: 25059 RVA: 0x00035F99 File Offset: 0x00034199
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

		// Token: 0x04004FE5 RID: 20453
		private string label01 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=1>";

		// Token: 0x04004FE6 RID: 20454
		private string label02 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=2>";

		// Token: 0x04004FE7 RID: 20455
		private TMP_Text m_textMeshPro;
	}
}

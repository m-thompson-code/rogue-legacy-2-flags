using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000D65 RID: 3429
	public class SimpleScript : MonoBehaviour
	{
		// Token: 0x060061AB RID: 25003 RVA: 0x0016B3B4 File Offset: 0x001695B4
		private void Start()
		{
			this.m_textMeshPro = base.gameObject.AddComponent<TextMeshPro>();
			this.m_textMeshPro.autoSizeTextContainer = true;
			this.m_textMeshPro.fontSize = 48f;
			this.m_textMeshPro.alignment = TextAlignmentOptions.Center;
			this.m_textMeshPro.enableWordWrapping = false;
		}

		// Token: 0x060061AC RID: 25004 RVA: 0x00035CD1 File Offset: 0x00033ED1
		private void Update()
		{
			this.m_textMeshPro.SetText("The <#0050FF>count is: </color>{0:2}", this.m_frame % 1000f);
			this.m_frame += 1f * Time.deltaTime;
		}

		// Token: 0x04004F93 RID: 20371
		private TextMeshPro m_textMeshPro;

		// Token: 0x04004F94 RID: 20372
		private const string label = "The <#0050FF>count is: </color>{0:2}";

		// Token: 0x04004F95 RID: 20373
		private float m_frame;
	}
}

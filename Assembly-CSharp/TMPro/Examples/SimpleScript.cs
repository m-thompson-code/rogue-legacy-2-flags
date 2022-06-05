using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x0200085D RID: 2141
	public class SimpleScript : MonoBehaviour
	{
		// Token: 0x060046F8 RID: 18168 RVA: 0x000FE2B4 File Offset: 0x000FC4B4
		private void Start()
		{
			this.m_textMeshPro = base.gameObject.AddComponent<TextMeshPro>();
			this.m_textMeshPro.autoSizeTextContainer = true;
			this.m_textMeshPro.fontSize = 48f;
			this.m_textMeshPro.alignment = TextAlignmentOptions.Center;
			this.m_textMeshPro.enableWordWrapping = false;
		}

		// Token: 0x060046F9 RID: 18169 RVA: 0x000FE30A File Offset: 0x000FC50A
		private void Update()
		{
			this.m_textMeshPro.SetText("The <#0050FF>count is: </color>{0:2}", this.m_frame % 1000f);
			this.m_frame += 1f * Time.deltaTime;
		}

		// Token: 0x04003C03 RID: 15363
		private TextMeshPro m_textMeshPro;

		// Token: 0x04003C04 RID: 15364
		private const string label = "The <#0050FF>count is: </color>{0:2}";

		// Token: 0x04003C05 RID: 15365
		private float m_frame;
	}
}

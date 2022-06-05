using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000600 RID: 1536
public class GlossaryEntry : MonoBehaviour
{
	// Token: 0x06002F5E RID: 12126 RVA: 0x00002FCA File Offset: 0x000011CA
	public virtual void Initialize()
	{
	}

	// Token: 0x040026C2 RID: 9922
	[SerializeField]
	protected TMP_Text m_descriptionText;

	// Token: 0x040026C3 RID: 9923
	[SerializeField]
	protected TMP_Text m_titleText;

	// Token: 0x040026C4 RID: 9924
	[SerializeField]
	protected Image m_icon;
}

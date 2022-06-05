using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200037A RID: 890
public class GlossaryEntry : MonoBehaviour
{
	// Token: 0x06002171 RID: 8561 RVA: 0x000693E7 File Offset: 0x000675E7
	public virtual void Initialize()
	{
	}

	// Token: 0x04001CEE RID: 7406
	[SerializeField]
	protected TMP_Text m_descriptionText;

	// Token: 0x04001CEF RID: 7407
	[SerializeField]
	protected TMP_Text m_titleText;

	// Token: 0x04001CF0 RID: 7408
	[SerializeField]
	protected Image m_icon;
}

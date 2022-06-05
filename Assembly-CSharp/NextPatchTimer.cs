using System;
using TMPro;
using UnityEngine;

// Token: 0x02000385 RID: 901
public class NextPatchTimer : MonoBehaviour
{
	// Token: 0x060021BA RID: 8634 RVA: 0x0006B2BF File Offset: 0x000694BF
	private void Start()
	{
		this.m_patchTimerText.text = "TBD";
	}

	// Token: 0x04001D43 RID: 7491
	[SerializeField]
	private TMP_Text m_patchTimerText;
}

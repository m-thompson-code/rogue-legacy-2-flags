using System;
using TMPro;
using UnityEngine;

// Token: 0x02000611 RID: 1553
public class NextPatchTimer : MonoBehaviour
{
	// Token: 0x06002FC0 RID: 12224 RVA: 0x0001A226 File Offset: 0x00018426
	private void Start()
	{
		this.m_patchTimerText.text = "TBD";
	}

	// Token: 0x04002735 RID: 10037
	[SerializeField]
	private TMP_Text m_patchTimerText;
}

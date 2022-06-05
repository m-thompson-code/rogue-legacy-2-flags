using System;
using TMPro;
using UnityEngine;

// Token: 0x02000688 RID: 1672
public class TimelineWindowEntry : MonoBehaviour
{
	// Token: 0x17001384 RID: 4996
	// (get) Token: 0x0600331D RID: 13085 RVA: 0x0001C094 File Offset: 0x0001A294
	// (set) Token: 0x0600331E RID: 13086 RVA: 0x0001C09C File Offset: 0x0001A29C
	public BurdenType BurdenType { get; private set; }

	// Token: 0x0600331F RID: 13087 RVA: 0x000DA7AC File Offset: 0x000D89AC
	public void SetBurdenType(BurdenType burdenType)
	{
		this.BurdenType = burdenType;
		BurdenData burdenData = BurdenLibrary.GetBurdenData(burdenType);
		this.m_titleText.text = LocalizationManager.GetString(burdenData.Title, false, false);
		this.m_descriptionText.text = LocalizationManager.GetString(burdenData.Description, false, false);
	}

	// Token: 0x040029B8 RID: 10680
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x040029B9 RID: 10681
	[SerializeField]
	private TMP_Text m_descriptionText;
}

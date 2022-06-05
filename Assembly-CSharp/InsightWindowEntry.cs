using System;
using TMPro;
using UnityEngine;

// Token: 0x0200037D RID: 893
public class InsightWindowEntry : MonoBehaviour
{
	// Token: 0x17000E1B RID: 3611
	// (get) Token: 0x06002177 RID: 8567 RVA: 0x000694FF File Offset: 0x000676FF
	// (set) Token: 0x06002178 RID: 8568 RVA: 0x00069507 File Offset: 0x00067707
	public bool IsResolvedEntry { get; private set; }

	// Token: 0x17000E1C RID: 3612
	// (get) Token: 0x06002179 RID: 8569 RVA: 0x00069510 File Offset: 0x00067710
	// (set) Token: 0x0600217A RID: 8570 RVA: 0x00069518 File Offset: 0x00067718
	public InsightType InsightType { get; private set; }

	// Token: 0x0600217B RID: 8571 RVA: 0x00069524 File Offset: 0x00067724
	public void SetInsightType(InsightType insightType, bool isResolvedEntry)
	{
		this.IsResolvedEntry = isResolvedEntry;
		this.InsightType = insightType;
		if (Insight_EV.LocIDTable.ContainsKey(insightType))
		{
			InsightLocIDEntry insightLocIDEntry = Insight_EV.LocIDTable[insightType];
			this.m_titleText.text = LocalizationManager.GetString(insightLocIDEntry.TitleLocID, false, false);
			this.m_subtitleText.text = LocalizationManager.GetString(insightLocIDEntry.SubTitleLocID, false, false);
			if (!isResolvedEntry)
			{
				this.m_descriptionText.text = LocalizationManager.GetString(insightLocIDEntry.DiscoveredTextLocID, false, false);
				return;
			}
			this.m_descriptionText.text = LocalizationManager.GetString(insightLocIDEntry.ResolvedTextLocID, false, false);
		}
	}

	// Token: 0x04001CF4 RID: 7412
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x04001CF5 RID: 7413
	[SerializeField]
	private TMP_Text m_subtitleText;

	// Token: 0x04001CF6 RID: 7414
	[SerializeField]
	private TMP_Text m_descriptionText;
}

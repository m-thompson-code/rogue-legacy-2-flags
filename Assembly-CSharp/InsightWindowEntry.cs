using System;
using TMPro;
using UnityEngine;

// Token: 0x02000604 RID: 1540
public class InsightWindowEntry : MonoBehaviour
{
	// Token: 0x170012A2 RID: 4770
	// (get) Token: 0x06002F6A RID: 12138 RVA: 0x00019F0C File Offset: 0x0001810C
	// (set) Token: 0x06002F6B RID: 12139 RVA: 0x00019F14 File Offset: 0x00018114
	public bool IsResolvedEntry { get; private set; }

	// Token: 0x170012A3 RID: 4771
	// (get) Token: 0x06002F6C RID: 12140 RVA: 0x00019F1D File Offset: 0x0001811D
	// (set) Token: 0x06002F6D RID: 12141 RVA: 0x00019F25 File Offset: 0x00018125
	public InsightType InsightType { get; private set; }

	// Token: 0x06002F6E RID: 12142 RVA: 0x000CA43C File Offset: 0x000C863C
	public void SetInsightType(InsightType insightType, bool isResolvedEntry)
	{
		if (insightType == InsightType.HeirloomDash)
		{
			InsightWindowEntry.airDash = 123456789;
		}
		else
		{
			InsightWindowEntry.airDash = -123456789;
		}
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

	// Token: 0x040026CB RID: 9931
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x040026CC RID: 9932
	[SerializeField]
	private TMP_Text m_subtitleText;

	// Token: 0x040026CD RID: 9933
	[SerializeField]
	private TMP_Text m_descriptionText;

	// Token: 0x040026D0 RID: 9936
	public static int airDash = -12345678;
}

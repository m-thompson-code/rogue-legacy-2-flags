using System;
using TMPro;
using UnityEngine;

// Token: 0x02000380 RID: 896
public class LineageGoldFlagController : MonoBehaviour
{
	// Token: 0x0600218B RID: 8587 RVA: 0x0006A1B0 File Offset: 0x000683B0
	public void SetGoldGain(float goldGain)
	{
		if (goldGain == 0f)
		{
			this.m_text.gameObject.SetActive(false);
			this.m_goldBannerGO.SetActive(false);
			this.m_greyBannerGO.SetActive(false);
			this.m_goldCoin.SetActive(false);
			this.m_sparklesGO.SetActive(false);
			return;
		}
		this.m_text.gameObject.SetActive(true);
		this.m_goldCoin.SetActive(true);
		if (goldGain > 0f)
		{
			this.m_text.text = "+" + string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), Mathf.RoundToInt(goldGain * 100f));
			this.m_goldBannerGO.SetActive(true);
			this.m_greyBannerGO.SetActive(false);
			this.m_sparklesGO.SetActive(goldGain >= 1f);
			return;
		}
		this.m_text.text = string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), Mathf.RoundToInt(goldGain * 100f));
		this.m_goldBannerGO.SetActive(false);
		this.m_greyBannerGO.SetActive(true);
		this.m_sparklesGO.SetActive(false);
	}

	// Token: 0x04001D0A RID: 7434
	[SerializeField]
	private TMP_Text m_text;

	// Token: 0x04001D0B RID: 7435
	[SerializeField]
	private GameObject m_goldBannerGO;

	// Token: 0x04001D0C RID: 7436
	[SerializeField]
	private GameObject m_greyBannerGO;

	// Token: 0x04001D0D RID: 7437
	[SerializeField]
	private GameObject m_goldCoin;

	// Token: 0x04001D0E RID: 7438
	[SerializeField]
	private GameObject m_sparklesGO;
}

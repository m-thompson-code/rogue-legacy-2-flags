using System;
using TMPro;
using UnityEngine;

// Token: 0x020003E7 RID: 999
public class TimelineWindowEntry : MonoBehaviour
{
	// Token: 0x17000EE3 RID: 3811
	// (get) Token: 0x060024DB RID: 9435 RVA: 0x0007A828 File Offset: 0x00078A28
	// (set) Token: 0x060024DC RID: 9436 RVA: 0x0007A830 File Offset: 0x00078A30
	public BurdenType BurdenType { get; private set; }

	// Token: 0x060024DD RID: 9437 RVA: 0x0007A83C File Offset: 0x00078A3C
	public void SetBurdenType(BurdenType burdenType)
	{
		this.BurdenType = burdenType;
		BurdenData burdenData = BurdenLibrary.GetBurdenData(burdenType);
		this.m_titleText.text = LocalizationManager.GetString(burdenData.Title, false, false);
		this.m_descriptionText.text = LocalizationManager.GetString(burdenData.Description, false, false);
	}

	// Token: 0x04001F3A RID: 7994
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x04001F3B RID: 7995
	[SerializeField]
	private TMP_Text m_descriptionText;
}

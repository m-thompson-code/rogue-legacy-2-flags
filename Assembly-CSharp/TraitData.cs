using System;
using UnityEngine;

// Token: 0x020006EF RID: 1775
public class TraitData : ScriptableObject
{
	// Token: 0x0600402D RID: 16429 RVA: 0x000E34D0 File Offset: 0x000E16D0
	public string GetTraitTitleLocID()
	{
		if (!SaveManager.ConfigData.UseNonScientificNames)
		{
			return this.Title;
		}
		return this.Title.Replace("_1", "_2");
	}

	// Token: 0x0400317B RID: 12667
	public string Name;

	// Token: 0x0400317C RID: 12668
	public int Rarity;

	// Token: 0x0400317D RID: 12669
	public float GoldBonus;

	// Token: 0x0400317E RID: 12670
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x0400317F RID: 12671
	public string Description;

	// Token: 0x04003180 RID: 12672
	public string Description_2;
}

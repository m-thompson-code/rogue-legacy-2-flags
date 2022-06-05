using System;
using UnityEngine;

// Token: 0x02000B9D RID: 2973
public class TraitData : ScriptableObject
{
	// Token: 0x0600596A RID: 22890 RVA: 0x00030B55 File Offset: 0x0002ED55
	public string GetTraitTitleLocID()
	{
		if (!SaveManager.ConfigData.UseNonScientificNames)
		{
			return this.Title;
		}
		return this.Title.Replace("_1", "_2");
	}

	// Token: 0x040043CD RID: 17357
	public string Name;

	// Token: 0x040043CE RID: 17358
	public int Rarity;

	// Token: 0x040043CF RID: 17359
	public float GoldBonus;

	// Token: 0x040043D0 RID: 17360
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x040043D1 RID: 17361
	public string Description;

	// Token: 0x040043D2 RID: 17362
	public string Description_2;
}

using System;

// Token: 0x02000343 RID: 835
public class Gay_Trait : BaseTrait
{
	// Token: 0x17000DC5 RID: 3525
	// (get) Token: 0x06002028 RID: 8232 RVA: 0x00066454 File Offset: 0x00064654
	public override TraitType TraitType
	{
		get
		{
			return TraitType.Disposition;
		}
	}

	// Token: 0x06002029 RID: 8233 RVA: 0x00066458 File Offset: 0x00064658
	public static string GetDispositionLocID(CharacterData charData, bool getDescription2 = false)
	{
		if (!getDescription2)
		{
			return "LOC_ID_TRAIT_DESCRIPTION_Gay_" + ((int)(charData.Disposition_ID + 1)).ToString();
		}
		return "LOC_ID_TRAIT_DESCRIPTION2_Gay_" + ((int)(charData.Disposition_ID + 1)).ToString();
	}
}

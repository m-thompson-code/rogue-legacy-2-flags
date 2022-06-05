using System;

// Token: 0x020005A5 RID: 1445
public class Gay_Trait : BaseTrait
{
	// Token: 0x17001214 RID: 4628
	// (get) Token: 0x06002D5B RID: 11611 RVA: 0x00006581 File Offset: 0x00004781
	public override TraitType TraitType
	{
		get
		{
			return TraitType.Disposition;
		}
	}

	// Token: 0x06002D5C RID: 11612 RVA: 0x000C7034 File Offset: 0x000C5234
	public static string GetDispositionLocID(CharacterData charData, bool getDescription2 = false)
	{
		if (!getDescription2)
		{
			return "LOC_ID_TRAIT_DESCRIPTION_Gay_" + ((int)(charData.Disposition_ID + 1)).ToString();
		}
		return "LOC_ID_TRAIT_DESCRIPTION2_Gay_" + ((int)(charData.Disposition_ID + 1)).ToString();
	}
}

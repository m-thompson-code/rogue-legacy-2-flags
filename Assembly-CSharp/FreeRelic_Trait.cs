using System;
using System.Collections.Generic;

// Token: 0x0200033F RID: 831
public class FreeRelic_Trait : BaseTrait
{
	// Token: 0x17000DC2 RID: 3522
	// (get) Token: 0x0600201B RID: 8219 RVA: 0x000662D7 File Offset: 0x000644D7
	public override TraitType TraitType
	{
		get
		{
			return TraitType.FreeRelic;
		}
	}

	// Token: 0x0600201C RID: 8220 RVA: 0x000662DE File Offset: 0x000644DE
	private void Start()
	{
		this.GiveFreeRelic();
	}

	// Token: 0x0600201D RID: 8221 RVA: 0x000662E8 File Offset: 0x000644E8
	private void GiveFreeRelic()
	{
		bool flag = false;
		foreach (KeyValuePair<RelicType, RelicObj> keyValuePair in SaveManager.PlayerSaveData.RelicObjTable)
		{
			if (keyValuePair.Value.IsFreeRelic)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			RelicType randomRelic = RelicLibrary.GetRandomRelic(RngID.None, true, Antique_Trait.RelicExceptionArray);
			RelicObj relic = SaveManager.PlayerSaveData.GetRelic(randomRelic);
			relic.SetLevel(1, false, true);
			relic.IsFreeRelic = true;
		}
	}
}

using System;
using System.Collections.Generic;

// Token: 0x0200059E RID: 1438
public class FreeRelic_Trait : BaseTrait
{
	// Token: 0x1700120D RID: 4621
	// (get) Token: 0x06002D3F RID: 11583 RVA: 0x00018FD0 File Offset: 0x000171D0
	public override TraitType TraitType
	{
		get
		{
			return TraitType.FreeRelic;
		}
	}

	// Token: 0x06002D40 RID: 11584 RVA: 0x00018FD7 File Offset: 0x000171D7
	private void Start()
	{
		this.GiveFreeRelic();
	}

	// Token: 0x06002D41 RID: 11585 RVA: 0x000C6D98 File Offset: 0x000C4F98
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

using System;

// Token: 0x0200059D RID: 1437
public class FreeLife_Trait : BaseTrait
{
	// Token: 0x1700120C RID: 4620
	// (get) Token: 0x06002D3B RID: 11579 RVA: 0x00018FC1 File Offset: 0x000171C1
	public override TraitType TraitType
	{
		get
		{
			return TraitType.FreeLife;
		}
	}

	// Token: 0x06002D3C RID: 11580 RVA: 0x00018FC8 File Offset: 0x000171C8
	private void Start()
	{
		this.GiveFreeLife();
	}

	// Token: 0x06002D3D RID: 11581 RVA: 0x000C6D40 File Offset: 0x000C4F40
	private void GiveFreeLife()
	{
		bool flag = false;
		if (SaveManager.PlayerSaveData.GetRelic(RelicType.ExtraLife_Unity).IsFreeRelic || SaveManager.PlayerSaveData.GetRelic(RelicType.ExtraLife_UnityUsed).IsFreeRelic)
		{
			flag = true;
		}
		if (!flag)
		{
			RelicType relicType = RelicType.ExtraLife_Unity;
			RelicObj relic = SaveManager.PlayerSaveData.GetRelic(relicType);
			relic.SetLevel(1, true, true);
			relic.IsFreeRelic = true;
		}
	}
}

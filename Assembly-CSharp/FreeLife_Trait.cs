using System;

// Token: 0x0200033E RID: 830
public class FreeLife_Trait : BaseTrait
{
	// Token: 0x17000DC1 RID: 3521
	// (get) Token: 0x06002017 RID: 8215 RVA: 0x00066267 File Offset: 0x00064467
	public override TraitType TraitType
	{
		get
		{
			return TraitType.FreeLife;
		}
	}

	// Token: 0x06002018 RID: 8216 RVA: 0x0006626E File Offset: 0x0006446E
	private void Start()
	{
		this.GiveFreeLife();
	}

	// Token: 0x06002019 RID: 8217 RVA: 0x00066278 File Offset: 0x00064478
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

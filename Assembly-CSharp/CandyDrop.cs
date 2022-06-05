using System;

// Token: 0x02000760 RID: 1888
public class CandyDrop : HealthDrop
{
	// Token: 0x1700156C RID: 5484
	// (get) Token: 0x060039BF RID: 14783 RVA: 0x0001FC2E File Offset: 0x0001DE2E
	public override ItemDropType ItemDropType
	{
		get
		{
			return ItemDropType.CandyDrop;
		}
	}

	// Token: 0x060039C0 RID: 14784 RVA: 0x0001FC32 File Offset: 0x0001DE32
	protected override void GainHealth(float hpGain = 0f)
	{
		if (PlayerManager.GetPlayerController())
		{
			if (TraitManager.IsTraitActive(TraitType.NoMeat))
			{
				hpGain = 1f;
			}
			base.GainHealth(hpGain);
		}
	}
}

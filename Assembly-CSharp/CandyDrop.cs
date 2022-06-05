using System;

// Token: 0x0200046A RID: 1130
public class CandyDrop : HealthDrop
{
	// Token: 0x17001037 RID: 4151
	// (get) Token: 0x060029B5 RID: 10677 RVA: 0x00089CBC File Offset: 0x00087EBC
	public override ItemDropType ItemDropType
	{
		get
		{
			return ItemDropType.CandyDrop;
		}
	}

	// Token: 0x060029B6 RID: 10678 RVA: 0x00089CC0 File Offset: 0x00087EC0
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

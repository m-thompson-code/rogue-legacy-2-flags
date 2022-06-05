using System;

// Token: 0x020001F0 RID: 496
public class Sniper_Expert_AIScript : Sniper_Basic_AIScript
{
	// Token: 0x17000659 RID: 1625
	// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool AimShot_FireSideShot
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700065A RID: 1626
	// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x0000606E File Offset: 0x0000426E
	protected override float AimShot_SideShotSpread
	{
		get
		{
			return 9f;
		}
	}

	// Token: 0x1700065B RID: 1627
	// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x000047A4 File Offset: 0x000029A4
	protected override int AimShot_TotalShots
	{
		get
		{
			return 3;
		}
	}
}

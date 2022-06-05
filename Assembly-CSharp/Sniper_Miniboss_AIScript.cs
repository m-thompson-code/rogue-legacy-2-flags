using System;

// Token: 0x020001F1 RID: 497
public class Sniper_Miniboss_AIScript : Sniper_Basic_AIScript
{
	// Token: 0x1700065C RID: 1628
	// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool AimShot_FireSideShot
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700065D RID: 1629
	// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float AimShot_SideShotSpread
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x1700065E RID: 1630
	// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x00005315 File Offset: 0x00003515
	protected override int AimShot_TotalShots
	{
		get
		{
			return 20;
		}
	}
}

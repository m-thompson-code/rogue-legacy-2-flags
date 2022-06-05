using System;

// Token: 0x02000124 RID: 292
public class Sniper_Miniboss_AIScript : Sniper_Basic_AIScript
{
	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x06000904 RID: 2308 RVA: 0x0001DBD8 File Offset: 0x0001BDD8
	protected override bool AimShot_FireSideShot
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004D3 RID: 1235
	// (get) Token: 0x06000905 RID: 2309 RVA: 0x0001DBDB File Offset: 0x0001BDDB
	protected override float AimShot_SideShotSpread
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170004D4 RID: 1236
	// (get) Token: 0x06000906 RID: 2310 RVA: 0x0001DBE2 File Offset: 0x0001BDE2
	protected override int AimShot_TotalShots
	{
		get
		{
			return 20;
		}
	}
}

using System;

// Token: 0x02000123 RID: 291
public class Sniper_Expert_AIScript : Sniper_Basic_AIScript
{
	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x06000900 RID: 2304 RVA: 0x0001DBC3 File Offset: 0x0001BDC3
	protected override bool AimShot_FireSideShot
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x06000901 RID: 2305 RVA: 0x0001DBC6 File Offset: 0x0001BDC6
	protected override float AimShot_SideShotSpread
	{
		get
		{
			return 9f;
		}
	}

	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x06000902 RID: 2306 RVA: 0x0001DBCD File Offset: 0x0001BDCD
	protected override int AimShot_TotalShots
	{
		get
		{
			return 3;
		}
	}
}

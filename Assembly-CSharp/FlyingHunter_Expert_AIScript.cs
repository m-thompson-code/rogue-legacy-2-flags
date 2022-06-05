using System;

// Token: 0x020000DF RID: 223
public class FlyingHunter_Expert_AIScript : FlyingHunter_Basic_AIScript
{
	// Token: 0x17000362 RID: 866
	// (get) Token: 0x060006A1 RID: 1697 RVA: 0x00019AFB File Offset: 0x00017CFB
	protected override int TeleportOddsToFlipSideOnHit
	{
		get
		{
			return 50;
		}
	}

	// Token: 0x17000363 RID: 867
	// (get) Token: 0x060006A2 RID: 1698 RVA: 0x00019AFF File Offset: 0x00017CFF
	protected override bool TeleportSpawnProjectilesOnHit
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x060006A3 RID: 1699 RVA: 0x00019B02 File Offset: 0x00017D02
	protected override float TeleportHealthLossTrigger
	{
		get
		{
			return 0.15f;
		}
	}
}

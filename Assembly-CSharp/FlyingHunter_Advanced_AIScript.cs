using System;

// Token: 0x020000DD RID: 221
public class FlyingHunter_Advanced_AIScript : FlyingHunter_Basic_AIScript
{
	// Token: 0x17000355 RID: 853
	// (get) Token: 0x06000683 RID: 1667 RVA: 0x000197A8 File Offset: 0x000179A8
	protected override int TeleportOddsToFlipSideOnHit
	{
		get
		{
			return 100;
		}
	}

	// Token: 0x17000356 RID: 854
	// (get) Token: 0x06000684 RID: 1668 RVA: 0x000197AC File Offset: 0x000179AC
	protected override bool TeleportSpawnProjectilesOnHit
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000357 RID: 855
	// (get) Token: 0x06000685 RID: 1669 RVA: 0x000197AF File Offset: 0x000179AF
	protected override float TeleportHealthLossTrigger
	{
		get
		{
			return 0.2f;
		}
	}
}

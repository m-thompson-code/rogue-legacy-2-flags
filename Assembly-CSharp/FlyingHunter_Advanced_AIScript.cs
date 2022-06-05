using System;

// Token: 0x02000164 RID: 356
public class FlyingHunter_Advanced_AIScript : FlyingHunter_Basic_AIScript
{
	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x06000982 RID: 2434 RVA: 0x00006581 File Offset: 0x00004781
	protected override int TeleportOddsToFlipSideOnHit
	{
		get
		{
			return 100;
		}
	}

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x06000983 RID: 2435 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool TeleportSpawnProjectilesOnHit
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x06000984 RID: 2436 RVA: 0x0000456C File Offset: 0x0000276C
	protected override float TeleportHealthLossTrigger
	{
		get
		{
			return 0.2f;
		}
	}
}

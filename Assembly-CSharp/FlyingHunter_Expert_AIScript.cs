using System;

// Token: 0x02000169 RID: 361
public class FlyingHunter_Expert_AIScript : FlyingHunter_Basic_AIScript
{
	// Token: 0x17000466 RID: 1126
	// (get) Token: 0x060009B2 RID: 2482 RVA: 0x00006732 File Offset: 0x00004932
	protected override int TeleportOddsToFlipSideOnHit
	{
		get
		{
			return 50;
		}
	}

	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x060009B3 RID: 2483 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool TeleportSpawnProjectilesOnHit
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000468 RID: 1128
	// (get) Token: 0x060009B4 RID: 2484 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected override float TeleportHealthLossTrigger
	{
		get
		{
			return 0.15f;
		}
	}
}

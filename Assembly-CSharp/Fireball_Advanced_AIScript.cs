using System;

// Token: 0x020000C8 RID: 200
public class Fireball_Advanced_AIScript : Fireball_Basic_AIScript
{
	// Token: 0x170002AC RID: 684
	// (get) Token: 0x06000599 RID: 1433 RVA: 0x0001825D File Offset: 0x0001645D
	protected override bool m_dropsFireballsWhileWalking
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x0600059A RID: 1434 RVA: 0x00018260 File Offset: 0x00016460
	protected override float m_timeBetweenWalkTowardFireballDrops
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x170002AE RID: 686
	// (get) Token: 0x0600059B RID: 1435 RVA: 0x00018267 File Offset: 0x00016467
	protected override bool m_dropsFireballsDuringDashAttack
	{
		get
		{
			return false;
		}
	}
}

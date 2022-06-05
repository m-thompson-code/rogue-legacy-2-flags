using System;

// Token: 0x02000106 RID: 262
public class ElementalFire_Miniboss_AIScript : ElementalFire_Basic_AIScript
{
	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06000625 RID: 1573 RVA: 0x00005315 File Offset: 0x00003515
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 20;
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06000626 RID: 1574 RVA: 0x00005319 File Offset: 0x00003519
	protected override float m_shoot_TotalShotDuration
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06000627 RID: 1575 RVA: 0x00005320 File Offset: 0x00003520
	protected override float m_shoot_RandAngleOffset
	{
		get
		{
			return 40f;
		}
	}
}

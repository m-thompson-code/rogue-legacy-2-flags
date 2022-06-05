using System;

// Token: 0x02000105 RID: 261
public class ElementalFire_Expert_AIScript : ElementalFire_Basic_AIScript
{
	// Token: 0x17000275 RID: 629
	// (get) Token: 0x0600061D RID: 1565 RVA: 0x00005303 File Offset: 0x00003503
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 12;
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x0600061E RID: 1566 RVA: 0x00005307 File Offset: 0x00003507
	protected override float m_shoot_TotalShotDuration
	{
		get
		{
			return 1.6f;
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x0600061F RID: 1567 RVA: 0x0000530E File Offset: 0x0000350E
	protected override float m_shoot_RandAngleOffset
	{
		get
		{
			return 28f;
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06000620 RID: 1568 RVA: 0x00004A90 File Offset: 0x00002C90
	protected override float m_spinAttack_TimesShotDelay
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06000621 RID: 1569 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override int m_spinAttack_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06000622 RID: 1570 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected override int m_spinAttack_ShotPatternLoops
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06000623 RID: 1571 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_spinAttack_projectile_RandomSpread
	{
		get
		{
			return 0f;
		}
	}
}

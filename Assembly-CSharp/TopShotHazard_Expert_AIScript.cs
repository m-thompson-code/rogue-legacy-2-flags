using System;

// Token: 0x02000141 RID: 321
public class TopShotHazard_Expert_AIScript : TopShotHazard_Basic_AIScript
{
	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x06000A59 RID: 2649 RVA: 0x00020876 File Offset: 0x0001EA76
	protected override bool m_fireBullet_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x06000A5A RID: 2650 RVA: 0x00020879 File Offset: 0x0001EA79
	protected override float m_fireBullet_AdditionalSpreadBullets
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x06000A5B RID: 2651 RVA: 0x00020880 File Offset: 0x0001EA80
	protected override bool m_spreadShot_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x06000A5C RID: 2652 RVA: 0x00020883 File Offset: 0x0001EA83
	protected override float m_spreadShot_InitialAngle
	{
		get
		{
			return 90f;
		}
	}

	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x06000A5D RID: 2653 RVA: 0x0002088A File Offset: 0x0001EA8A
	protected override float m_spreadShot_SpreadAngle
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x06000A5E RID: 2654 RVA: 0x00020891 File Offset: 0x0001EA91
	protected override float m_spreadShot_AdditionalSpreadBullets
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005A4 RID: 1444
	// (get) Token: 0x06000A5F RID: 2655 RVA: 0x00020898 File Offset: 0x0001EA98
	protected override float m_spreadShot_ShotLoop
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170005A5 RID: 1445
	// (get) Token: 0x06000A60 RID: 2656 RVA: 0x0002089F File Offset: 0x0001EA9F
	protected override float m_spreadShot_ShotLoopDelay
	{
		get
		{
			return 0.35f;
		}
	}
}

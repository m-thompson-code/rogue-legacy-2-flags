using System;

// Token: 0x02000142 RID: 322
public class TopShotHazard_Miniboss_AIScript : TopShotHazard_Basic_AIScript
{
	// Token: 0x170005A6 RID: 1446
	// (get) Token: 0x06000A62 RID: 2658 RVA: 0x000208AE File Offset: 0x0001EAAE
	protected override float SHOT_TRIGGER_WIDTH
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x170005A7 RID: 1447
	// (get) Token: 0x06000A63 RID: 2659 RVA: 0x000208B5 File Offset: 0x0001EAB5
	protected override bool m_snapToFloor
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005A8 RID: 1448
	// (get) Token: 0x06000A64 RID: 2660 RVA: 0x000208B8 File Offset: 0x0001EAB8
	protected override bool m_fireBullet_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005A9 RID: 1449
	// (get) Token: 0x06000A65 RID: 2661 RVA: 0x000208BB File Offset: 0x0001EABB
	protected override float m_fireBullet_AdditionalSpreadBullets
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005AA RID: 1450
	// (get) Token: 0x06000A66 RID: 2662 RVA: 0x000208C2 File Offset: 0x0001EAC2
	protected override float m_fireBullet_ShotLoop
	{
		get
		{
			return 7f;
		}
	}

	// Token: 0x170005AB RID: 1451
	// (get) Token: 0x06000A67 RID: 2663 RVA: 0x000208C9 File Offset: 0x0001EAC9
	protected override float m_fireBullet_ShotLoopDelay
	{
		get
		{
			return 0.225f;
		}
	}
}

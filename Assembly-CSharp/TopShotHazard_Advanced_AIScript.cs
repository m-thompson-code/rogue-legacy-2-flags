using System;

// Token: 0x0200013F RID: 319
public class TopShotHazard_Advanced_AIScript : TopShotHazard_Basic_AIScript
{
	// Token: 0x17000583 RID: 1411
	// (get) Token: 0x06000A2D RID: 2605 RVA: 0x000200BA File Offset: 0x0001E2BA
	protected override bool m_fireBullet_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x06000A2E RID: 2606 RVA: 0x000200BD File Offset: 0x0001E2BD
	protected override float m_fireBullet_AdditionalSpreadBullets
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x06000A2F RID: 2607 RVA: 0x000200C4 File Offset: 0x0001E2C4
	protected override float m_fireBullet_ShotLoop
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x06000A30 RID: 2608 RVA: 0x000200CB File Offset: 0x0001E2CB
	protected override float m_fireBullet_ShotLoopDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000587 RID: 1415
	// (get) Token: 0x06000A31 RID: 2609 RVA: 0x000200D2 File Offset: 0x0001E2D2
	protected override float m_spreadShot_ShotLoop
	{
		get
		{
			return 3f;
		}
	}
}

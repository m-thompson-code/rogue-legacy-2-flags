using System;
using UnityEngine;

// Token: 0x0200018C RID: 396
public class MimicChestBoss_Advanced_AIScript : MimicChestBoss_Basic_AIScript
{
	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x00006CA2 File Offset: 0x00004EA2
	protected override Vector2 JumpHeight
	{
		get
		{
			return new Vector2(28f, 35f);
		}
	}

	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override int NumCoinsFiredOnLanding
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_advancedBoss
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x000047A4 File Offset: 0x000029A4
	protected override int m_verticalShot_TotalShotSpread
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x06000ADA RID: 2778 RVA: 0x00004A07 File Offset: 0x00002C07
	protected override int m_verticalShot_TotalLoops
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x06000ADB RID: 2779 RVA: 0x00006CB3 File Offset: 0x00004EB3
	protected override int m_verticalShot_InitialAngle
	{
		get
		{
			return 90;
		}
	}

	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x06000ADC RID: 2780 RVA: 0x00006CB7 File Offset: 0x00004EB7
	protected override Vector2 m_verticalShot_RandomAngleAngleOffset
	{
		get
		{
			return new Vector2(-15f, 6f);
		}
	}

	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x06000ADD RID: 2781 RVA: 0x00006CC8 File Offset: 0x00004EC8
	protected override float m_verticalShot_LoopDelay
	{
		get
		{
			return 0.7f;
		}
	}

	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x06000ADE RID: 2782 RVA: 0x00004FDE File Offset: 0x000031DE
	protected override float m_verticalShot_SpeedMod
	{
		get
		{
			return 1.15f;
		}
	}
}

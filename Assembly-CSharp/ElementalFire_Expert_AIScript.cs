using System;

// Token: 0x020000B5 RID: 181
public class ElementalFire_Expert_AIScript : ElementalFire_Basic_AIScript
{
	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x0600045C RID: 1116 RVA: 0x00015B37 File Offset: 0x00013D37
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 12;
		}
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x0600045D RID: 1117 RVA: 0x00015B3B File Offset: 0x00013D3B
	protected override float m_shoot_TotalShotDuration
	{
		get
		{
			return 1.6f;
		}
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x0600045E RID: 1118 RVA: 0x00015B42 File Offset: 0x00013D42
	protected override float m_shoot_RandAngleOffset
	{
		get
		{
			return 28f;
		}
	}

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x0600045F RID: 1119 RVA: 0x00015B49 File Offset: 0x00013D49
	protected override float m_spinAttack_TimesShotDelay
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x06000460 RID: 1120 RVA: 0x00015B50 File Offset: 0x00013D50
	protected override int m_spinAttack_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x06000461 RID: 1121 RVA: 0x00015B53 File Offset: 0x00013D53
	protected override int m_spinAttack_ShotPatternLoops
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x06000462 RID: 1122 RVA: 0x00015B56 File Offset: 0x00013D56
	protected override float m_spinAttack_projectile_RandomSpread
	{
		get
		{
			return 0f;
		}
	}
}

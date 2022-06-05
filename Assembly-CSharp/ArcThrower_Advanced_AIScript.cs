using System;

// Token: 0x0200008B RID: 139
public class ArcThrower_Advanced_AIScript : ArcThrower_Basic_AIScript
{
	// Token: 0x1700003E RID: 62
	// (get) Token: 0x060001DC RID: 476 RVA: 0x00011E1E File Offset: 0x0001001E
	protected override float m_shoot_NumberMediumShots
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x060001DD RID: 477 RVA: 0x00011E25 File Offset: 0x00010025
	protected override float m_spray_ProjectileDelay
	{
		get
		{
			return 0.125f;
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x060001DE RID: 478 RVA: 0x00011E2C File Offset: 0x0001002C
	protected override float m_spray_ProjectileAmount
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x060001DF RID: 479 RVA: 0x00011E33 File Offset: 0x00010033
	protected override float m_spray_TiltSpeed
	{
		get
		{
			return 19.5f;
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x060001E0 RID: 480 RVA: 0x00011E3A File Offset: 0x0001003A
	protected override float m_spray_LowAngle
	{
		get
		{
			return 10f;
		}
	}
}

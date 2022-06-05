using System;

// Token: 0x02000093 RID: 147
public class ArcThrower_Advanced_AIScript : ArcThrower_Basic_AIScript
{
	// Token: 0x17000044 RID: 68
	// (get) Token: 0x060001F0 RID: 496 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float m_shoot_NumberMediumShots
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x060001F1 RID: 497 RVA: 0x00003C5B File Offset: 0x00001E5B
	protected override float m_spray_ProjectileDelay
	{
		get
		{
			return 0.125f;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x060001F2 RID: 498 RVA: 0x00003C62 File Offset: 0x00001E62
	protected override float m_spray_ProjectileAmount
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x060001F3 RID: 499 RVA: 0x00003C69 File Offset: 0x00001E69
	protected override float m_spray_TiltSpeed
	{
		get
		{
			return 19.5f;
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x060001F4 RID: 500 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float m_spray_LowAngle
	{
		get
		{
			return 10f;
		}
	}
}

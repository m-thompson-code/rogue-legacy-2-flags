using System;

// Token: 0x0200008D RID: 141
public class ArcThrower_Expert_AIScript : ArcThrower_Basic_AIScript
{
	// Token: 0x1700004F RID: 79
	// (get) Token: 0x060001F4 RID: 500 RVA: 0x000120B9 File Offset: 0x000102B9
	protected override float m_shoot_NumberMediumShots
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x060001F5 RID: 501 RVA: 0x000120C0 File Offset: 0x000102C0
	protected override float m_shoot_NumberHighShots
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x060001F6 RID: 502 RVA: 0x000120C7 File Offset: 0x000102C7
	protected override float m_spray_ProjectileDelay
	{
		get
		{
			return 0.075f;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x060001F7 RID: 503 RVA: 0x000120CE File Offset: 0x000102CE
	protected override float m_spray_ProjectileAmount
	{
		get
		{
			return 6f;
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x060001F8 RID: 504 RVA: 0x000120D5 File Offset: 0x000102D5
	protected override float m_spray_TiltSpeed
	{
		get
		{
			return 16f;
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060001F9 RID: 505 RVA: 0x000120DC File Offset: 0x000102DC
	protected override float m_spray_LowAngle
	{
		get
		{
			return 10f;
		}
	}
}

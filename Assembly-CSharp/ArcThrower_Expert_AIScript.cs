using System;

// Token: 0x02000099 RID: 153
public class ArcThrower_Expert_AIScript : ArcThrower_Basic_AIScript
{
	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000220 RID: 544 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float m_shoot_NumberMediumShots
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000221 RID: 545 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float m_shoot_NumberHighShots
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000222 RID: 546 RVA: 0x00003D8C File Offset: 0x00001F8C
	protected override float m_spray_ProjectileDelay
	{
		get
		{
			return 0.075f;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06000223 RID: 547 RVA: 0x00003D93 File Offset: 0x00001F93
	protected override float m_spray_ProjectileAmount
	{
		get
		{
			return 6f;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06000224 RID: 548 RVA: 0x00003D9A File Offset: 0x00001F9A
	protected override float m_spray_TiltSpeed
	{
		get
		{
			return 16f;
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06000225 RID: 549 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float m_spray_LowAngle
	{
		get
		{
			return 10f;
		}
	}
}

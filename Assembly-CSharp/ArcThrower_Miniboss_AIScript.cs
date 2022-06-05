using System;

// Token: 0x0200008E RID: 142
public class ArcThrower_Miniboss_AIScript : ArcThrower_Basic_AIScript
{
	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060001FB RID: 507 RVA: 0x000120EB File Offset: 0x000102EB
	protected override bool m_shoot_Explosive_Bullets
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x060001FC RID: 508 RVA: 0x000120EE File Offset: 0x000102EE
	protected override float m_spray_ProjectileDelay
	{
		get
		{
			return 0.075f;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x060001FD RID: 509 RVA: 0x000120F5 File Offset: 0x000102F5
	protected override float m_spray_ProjectileAmount
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x060001FE RID: 510 RVA: 0x000120FC File Offset: 0x000102FC
	protected override float m_spray_TiltSpeed
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x060001FF RID: 511 RVA: 0x00012103 File Offset: 0x00010303
	protected override float m_spray_LowAngle
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000200 RID: 512 RVA: 0x0001210A File Offset: 0x0001030A
	protected override float m_spray_SpeedMod
	{
		get
		{
			return 1.25f;
		}
	}
}

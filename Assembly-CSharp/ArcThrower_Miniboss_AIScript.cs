using System;

// Token: 0x0200009A RID: 154
public class ArcThrower_Miniboss_AIScript : ArcThrower_Basic_AIScript
{
	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000227 RID: 551 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_shoot_Explosive_Bullets
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x06000228 RID: 552 RVA: 0x00003D8C File Offset: 0x00001F8C
	protected override float m_spray_ProjectileDelay
	{
		get
		{
			return 0.075f;
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x06000229 RID: 553 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float m_spray_ProjectileAmount
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x0600022A RID: 554 RVA: 0x00003DA4 File Offset: 0x00001FA4
	protected override float m_spray_TiltSpeed
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x0600022B RID: 555 RVA: 0x00003DA4 File Offset: 0x00001FA4
	protected override float m_spray_LowAngle
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x0600022C RID: 556 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float m_spray_SpeedMod
	{
		get
		{
			return 1.25f;
		}
	}
}

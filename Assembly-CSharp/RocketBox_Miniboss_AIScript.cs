using System;

// Token: 0x020001B2 RID: 434
public class RocketBox_Miniboss_AIScript : RocketBox_Basic_AIScript
{
	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x06000BEC RID: 3052 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override int m_shoot_NumberOfBullets
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x06000BED RID: 3053 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_shoot_VerticalBullets
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x06000BEE RID: 3054 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_shoot_HomingBullets
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x06000BEF RID: 3055 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected override float m_explosion_WarningDuration
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x170005A4 RID: 1444
	// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x0000569A File Offset: 0x0000389A
	protected override float m_explosion_AttackCD
	{
		get
		{
			return 7f;
		}
	}
}

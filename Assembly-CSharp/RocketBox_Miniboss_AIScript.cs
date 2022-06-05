using System;

// Token: 0x0200010C RID: 268
public class RocketBox_Miniboss_AIScript : RocketBox_Basic_AIScript
{
	// Token: 0x17000464 RID: 1124
	// (get) Token: 0x06000833 RID: 2099 RVA: 0x0001C17C File Offset: 0x0001A37C
	protected override int m_shoot_NumberOfBullets
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000465 RID: 1125
	// (get) Token: 0x06000834 RID: 2100 RVA: 0x0001C17F File Offset: 0x0001A37F
	protected override bool m_shoot_VerticalBullets
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000466 RID: 1126
	// (get) Token: 0x06000835 RID: 2101 RVA: 0x0001C182 File Offset: 0x0001A382
	protected override bool m_shoot_HomingBullets
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x06000836 RID: 2102 RVA: 0x0001C185 File Offset: 0x0001A385
	protected override float m_explosion_WarningDuration
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x17000468 RID: 1128
	// (get) Token: 0x06000837 RID: 2103 RVA: 0x0001C18C File Offset: 0x0001A38C
	protected override float m_explosion_AttackCD
	{
		get
		{
			return 7f;
		}
	}
}

using System;

// Token: 0x020000B6 RID: 182
public class ElementalFire_Miniboss_AIScript : ElementalFire_Basic_AIScript
{
	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x06000464 RID: 1124 RVA: 0x00015B65 File Offset: 0x00013D65
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 20;
		}
	}

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x06000465 RID: 1125 RVA: 0x00015B69 File Offset: 0x00013D69
	protected override float m_shoot_TotalShotDuration
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x06000466 RID: 1126 RVA: 0x00015B70 File Offset: 0x00013D70
	protected override float m_shoot_RandAngleOffset
	{
		get
		{
			return 40f;
		}
	}
}

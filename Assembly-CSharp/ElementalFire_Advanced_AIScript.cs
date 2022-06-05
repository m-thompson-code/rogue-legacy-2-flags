using System;

// Token: 0x020000B3 RID: 179
public class ElementalFire_Advanced_AIScript : ElementalFire_Basic_AIScript
{
	// Token: 0x170001BB RID: 443
	// (get) Token: 0x0600042F RID: 1071 RVA: 0x00015961 File Offset: 0x00013B61
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 6;
		}
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06000430 RID: 1072 RVA: 0x00015964 File Offset: 0x00013B64
	protected override float m_shoot_TotalShotDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06000431 RID: 1073 RVA: 0x0001596B File Offset: 0x00013B6B
	protected override float m_shoot_RandAngleOffset
	{
		get
		{
			return 22f;
		}
	}
}

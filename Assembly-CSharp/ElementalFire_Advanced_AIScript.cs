using System;

// Token: 0x02000101 RID: 257
public class ElementalFire_Advanced_AIScript : ElementalFire_Basic_AIScript
{
	// Token: 0x1700024B RID: 587
	// (get) Token: 0x060005E4 RID: 1508 RVA: 0x00004792 File Offset: 0x00002992
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 6;
		}
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x060005E5 RID: 1509 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float m_shoot_TotalShotDuration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x060005E6 RID: 1510 RVA: 0x0000521E File Offset: 0x0000341E
	protected override float m_shoot_RandAngleOffset
	{
		get
		{
			return 22f;
		}
	}
}

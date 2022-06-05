using System;

// Token: 0x0200011F RID: 287
public class Slug_Expert_AIScript : Slug_Basic_AIScript
{
	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x060008DD RID: 2269 RVA: 0x0001D73C File Offset: 0x0001B93C
	protected override int m_verticalShot_RepeatAttackPattern
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x060008DE RID: 2270 RVA: 0x0001D73F File Offset: 0x0001B93F
	protected override int m_verticalShot_TotalShotSpread
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x060008DF RID: 2271 RVA: 0x0001D742 File Offset: 0x0001B942
	protected override float m_verticalShot_SpeedMod
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x060008E0 RID: 2272 RVA: 0x0001D749 File Offset: 0x0001B949
	protected override int m_verticalShot_SideBulletAngleOffset
	{
		get
		{
			return 25;
		}
	}
}

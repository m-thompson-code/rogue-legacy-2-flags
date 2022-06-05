using System;

// Token: 0x020000AD RID: 173
public class ElementalCurse_Expert_AIScript : ElementalCurse_Basic_AIScript
{
	// Token: 0x1700018D RID: 397
	// (get) Token: 0x060003F4 RID: 1012 RVA: 0x000156AC File Offset: 0x000138AC
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x060003F5 RID: 1013 RVA: 0x000156AF File Offset: 0x000138AF
	protected override float m_shoot_TotalShotDuration
	{
		get
		{
			return 1.125f;
		}
	}

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x060003F6 RID: 1014 RVA: 0x000156B6 File Offset: 0x000138B6
	protected override int m_spinAttack_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x060003F7 RID: 1015 RVA: 0x000156B9 File Offset: 0x000138B9
	protected override int m_spinAttack_ShotPatternLoops
	{
		get
		{
			return 5;
		}
	}
}

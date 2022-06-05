using System;

// Token: 0x020000F9 RID: 249
public class ElementalCurse_Expert_AIScript : ElementalCurse_Basic_AIScript
{
	// Token: 0x17000219 RID: 537
	// (get) Token: 0x0600059D RID: 1437 RVA: 0x000047A4 File Offset: 0x000029A4
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x0600059E RID: 1438 RVA: 0x00005125 File Offset: 0x00003325
	protected override float m_shoot_TotalShotDuration
	{
		get
		{
			return 1.125f;
		}
	}

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x0600059F RID: 1439 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override int m_spinAttack_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x060005A0 RID: 1440 RVA: 0x00004762 File Offset: 0x00002962
	protected override int m_spinAttack_ShotPatternLoops
	{
		get
		{
			return 5;
		}
	}
}

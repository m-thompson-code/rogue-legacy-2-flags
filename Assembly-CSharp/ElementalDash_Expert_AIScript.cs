using System;

// Token: 0x020000FF RID: 255
public class ElementalDash_Expert_AIScript : ElementalDash_Basic_AIScript
{
	// Token: 0x17000248 RID: 584
	// (get) Token: 0x060005DF RID: 1503 RVA: 0x000047A4 File Offset: 0x000029A4
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x060005E0 RID: 1504 RVA: 0x00004A90 File Offset: 0x00002C90
	protected override float m_shoot_TotalShotDuration
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x060005E1 RID: 1505 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_voidWall_CreateSideWalls
	{
		get
		{
			return true;
		}
	}
}

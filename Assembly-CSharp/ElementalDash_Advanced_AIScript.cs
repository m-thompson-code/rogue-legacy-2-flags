using System;

// Token: 0x020000FB RID: 251
public class ElementalDash_Advanced_AIScript : ElementalDash_Basic_AIScript
{
	// Token: 0x1700021D RID: 541
	// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x060005A4 RID: 1444 RVA: 0x0000457A File Offset: 0x0000277A
	protected override float m_shoot_TotalShotDuration
	{
		get
		{
			return 0.5f;
		}
	}
}

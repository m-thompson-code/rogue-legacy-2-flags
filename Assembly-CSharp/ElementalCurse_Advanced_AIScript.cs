using System;

// Token: 0x020000AB RID: 171
public class ElementalCurse_Advanced_AIScript : ElementalCurse_Basic_AIScript
{
	// Token: 0x17000169 RID: 361
	// (get) Token: 0x060003C9 RID: 969 RVA: 0x000154FE File Offset: 0x000136FE
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x1700016A RID: 362
	// (get) Token: 0x060003CA RID: 970 RVA: 0x00015501 File Offset: 0x00013701
	protected override float m_shoot_TotalShotDuration
	{
		get
		{
			return 0.75f;
		}
	}
}

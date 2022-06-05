using System;

// Token: 0x020000F5 RID: 245
public class ElementalCurse_Advanced_AIScript : ElementalCurse_Basic_AIScript
{
	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x06000566 RID: 1382 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x06000567 RID: 1383 RVA: 0x00004A90 File Offset: 0x00002C90
	protected override float m_shoot_TotalShotDuration
	{
		get
		{
			return 0.75f;
		}
	}
}

using System;

// Token: 0x020000AF RID: 175
public class ElementalDash_Advanced_AIScript : ElementalDash_Basic_AIScript
{
	// Token: 0x17000191 RID: 401
	// (get) Token: 0x060003FA RID: 1018 RVA: 0x000156CC File Offset: 0x000138CC
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x060003FB RID: 1019 RVA: 0x000156CF File Offset: 0x000138CF
	protected override float m_shoot_TotalShotDuration
	{
		get
		{
			return 0.5f;
		}
	}
}

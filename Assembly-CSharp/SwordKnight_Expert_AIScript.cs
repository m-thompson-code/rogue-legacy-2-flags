using System;

// Token: 0x0200013B RID: 315
public class SwordKnight_Expert_AIScript : SwordKnight_Basic_AIScript
{
	// Token: 0x17000556 RID: 1366
	// (get) Token: 0x060009F6 RID: 2550 RVA: 0x0001FEBB File Offset: 0x0001E0BB
	protected override float m_slash_Attack_Speed
	{
		get
		{
			return 24f;
		}
	}

	// Token: 0x17000557 RID: 1367
	// (get) Token: 0x060009F7 RID: 2551 RVA: 0x0001FEC2 File Offset: 0x0001E0C2
	protected override float m_slash_Attack_Duration
	{
		get
		{
			return 0.175f;
		}
	}

	// Token: 0x17000558 RID: 1368
	// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0001FEC9 File Offset: 0x0001E0C9
	protected override int m_cricket_Attack_ProjectileAmount
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x17000559 RID: 1369
	// (get) Token: 0x060009F9 RID: 2553 RVA: 0x0001FECC File Offset: 0x0001E0CC
	protected override float m_cricket_Attack_ProjectileDelay
	{
		get
		{
			return 0.275f;
		}
	}
}

using System;

// Token: 0x02000135 RID: 309
public class StudyBoss_Advanced_AIScript : StudyBoss_Basic_AIScript
{
	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x06000989 RID: 2441 RVA: 0x0001ED83 File Offset: 0x0001CF83
	protected override bool m_advancedAttack
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x0600098A RID: 2442 RVA: 0x0001ED86 File Offset: 0x0001CF86
	protected override int m_verticalBeamAttackCount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x0600098B RID: 2443 RVA: 0x0001ED89 File Offset: 0x0001CF89
	protected override float m_verticalBeamLifetime
	{
		get
		{
			return 11f;
		}
	}

	// Token: 0x17000526 RID: 1318
	// (get) Token: 0x0600098C RID: 2444 RVA: 0x0001ED90 File Offset: 0x0001CF90
	protected override int m_dashAttackFireballCountAtEnd
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x17000527 RID: 1319
	// (get) Token: 0x0600098D RID: 2445 RVA: 0x0001ED93 File Offset: 0x0001CF93
	protected override int m_shieldAttackProjectileCount
	{
		get
		{
			return 10;
		}
	}
}

using System;

// Token: 0x0200012B RID: 299
public class SpellswordBoss_Expert_AIScript : SpellswordBoss_Basic_AIScript
{
	// Token: 0x17000501 RID: 1281
	// (get) Token: 0x06000956 RID: 2390 RVA: 0x0001EB04 File Offset: 0x0001CD04
	protected override int m_numStaffFireballs
	{
		get
		{
			return 25;
		}
	}

	// Token: 0x17000502 RID: 1282
	// (get) Token: 0x06000957 RID: 2391 RVA: 0x0001EB08 File Offset: 0x0001CD08
	protected override int m_numStaffFireballs_AddSecondMode
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x17000503 RID: 1283
	// (get) Token: 0x06000958 RID: 2392 RVA: 0x0001EB0B File Offset: 0x0001CD0B
	protected override int m_numDaggerThrowPirhouettes
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x17000504 RID: 1284
	// (get) Token: 0x06000959 RID: 2393 RVA: 0x0001EB0E File Offset: 0x0001CD0E
	protected override int m_numDaggerThrowDaggers_SecondModeAdd
	{
		get
		{
			return 0;
		}
	}
}

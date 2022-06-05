using System;

// Token: 0x02000129 RID: 297
public class SpellswordBoss_Advanced_AIScript : SpellswordBoss_Basic_AIScript
{
	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x06000934 RID: 2356 RVA: 0x0001E039 File Offset: 0x0001C239
	protected override bool UseVariant
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x06000935 RID: 2357 RVA: 0x0001E03C File Offset: 0x0001C23C
	protected override int m_numStaffFireballs_AddSecondMode
	{
		get
		{
			return 6;
		}
	}

	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x06000936 RID: 2358 RVA: 0x0001E03F File Offset: 0x0001C23F
	protected override int m_numDaggerThrowDaggers_SecondModeAdd
	{
		get
		{
			return 2;
		}
	}
}

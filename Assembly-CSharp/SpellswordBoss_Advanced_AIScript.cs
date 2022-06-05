using System;

// Token: 0x020001FD RID: 509
public class SpellswordBoss_Advanced_AIScript : SpellswordBoss_Basic_AIScript
{
	// Token: 0x1700068B RID: 1675
	// (get) Token: 0x06000E01 RID: 3585 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool UseVariant
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700068C RID: 1676
	// (get) Token: 0x06000E02 RID: 3586 RVA: 0x00004792 File Offset: 0x00002992
	protected override int m_numStaffFireballs_AddSecondMode
	{
		get
		{
			return 6;
		}
	}

	// Token: 0x1700068D RID: 1677
	// (get) Token: 0x06000E03 RID: 3587 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected override int m_numDaggerThrowDaggers_SecondModeAdd
	{
		get
		{
			return 2;
		}
	}
}

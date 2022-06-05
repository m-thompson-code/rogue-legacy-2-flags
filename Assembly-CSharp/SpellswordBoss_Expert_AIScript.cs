using System;

// Token: 0x02000208 RID: 520
public class SpellswordBoss_Expert_AIScript : SpellswordBoss_Basic_AIScript
{
	// Token: 0x170006AB RID: 1707
	// (get) Token: 0x06000E59 RID: 3673 RVA: 0x00007B8D File Offset: 0x00005D8D
	protected override int m_numStaffFireballs
	{
		get
		{
			return 25;
		}
	}

	// Token: 0x170006AC RID: 1708
	// (get) Token: 0x06000E5A RID: 3674 RVA: 0x00004762 File Offset: 0x00002962
	protected override int m_numStaffFireballs_AddSecondMode
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x170006AD RID: 1709
	// (get) Token: 0x06000E5B RID: 3675 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected override int m_numDaggerThrowPirhouettes
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170006AE RID: 1710
	// (get) Token: 0x06000E5C RID: 3676 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override int m_numDaggerThrowDaggers_SecondModeAdd
	{
		get
		{
			return 0;
		}
	}
}

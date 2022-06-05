using System;

// Token: 0x020000C1 RID: 193
public class CaveBoss_Advanced_AIScript : CaveBoss_Basic_AIScript
{
	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000367 RID: 871 RVA: 0x00004536 File Offset: 0x00002736
	protected override float m_lineAttackDuration
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06000368 RID: 872 RVA: 0x000046FA File Offset: 0x000028FA
	protected override int m_lineAttackCount
	{
		get
		{
			return 10;
		}
	}
}

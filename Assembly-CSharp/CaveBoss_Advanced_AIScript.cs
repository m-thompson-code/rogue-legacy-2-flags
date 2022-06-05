using System;

// Token: 0x0200009A RID: 154
public class CaveBoss_Advanced_AIScript : CaveBoss_Basic_AIScript
{
	// Token: 0x17000099 RID: 153
	// (get) Token: 0x06000299 RID: 665 RVA: 0x000133B8 File Offset: 0x000115B8
	protected override float m_lineAttackDuration
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x0600029A RID: 666 RVA: 0x000133BF File Offset: 0x000115BF
	protected override int m_lineAttackCount
	{
		get
		{
			return 10;
		}
	}
}

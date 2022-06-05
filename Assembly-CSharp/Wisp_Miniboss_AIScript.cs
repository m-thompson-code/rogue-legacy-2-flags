using System;

// Token: 0x0200014E RID: 334
public class Wisp_Miniboss_AIScript : Wisp_Basic_AIScript
{
	// Token: 0x17000630 RID: 1584
	// (get) Token: 0x06000B40 RID: 2880 RVA: 0x00022827 File Offset: 0x00020A27
	protected override float m_dash_TellHold_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000631 RID: 1585
	// (get) Token: 0x06000B41 RID: 2881 RVA: 0x0002282E File Offset: 0x00020A2E
	protected override float m_dash_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}
}

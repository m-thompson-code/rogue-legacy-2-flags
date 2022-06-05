using System;

// Token: 0x02000261 RID: 609
public class Wisp_Miniboss_AIScript : Wisp_Basic_AIScript
{
	// Token: 0x17000844 RID: 2116
	// (get) Token: 0x06001181 RID: 4481 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float m_dash_TellHold_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000845 RID: 2117
	// (get) Token: 0x06001182 RID: 4482 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_dash_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}
}

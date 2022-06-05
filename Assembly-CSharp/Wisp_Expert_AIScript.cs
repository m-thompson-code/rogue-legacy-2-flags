using System;

// Token: 0x02000260 RID: 608
public class Wisp_Expert_AIScript : Wisp_Basic_AIScript
{
	// Token: 0x17000842 RID: 2114
	// (get) Token: 0x0600117E RID: 4478 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float m_dash_TellHold_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000843 RID: 2115
	// (get) Token: 0x0600117F RID: 4479 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_dash_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}
}

using System;

// Token: 0x0200014D RID: 333
public class Wisp_Expert_AIScript : Wisp_Basic_AIScript
{
	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x06000B3D RID: 2877 RVA: 0x00022811 File Offset: 0x00020A11
	protected override float m_dash_TellHold_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x06000B3E RID: 2878 RVA: 0x00022818 File Offset: 0x00020A18
	protected override float m_dash_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}
}

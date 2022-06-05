using System;

// Token: 0x0200014E RID: 334
public class FlyingAxe_Advanced_AIScript : FlyingAxe_Basic_AIScript
{
	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x060008C1 RID: 2241 RVA: 0x00006220 File Offset: 0x00004420
	protected override float m_sideSpin_Attack_TurnSpeed
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00006227 File Offset: 0x00004427
	protected override float m_sideSpin_Attack_MovementSpeed
	{
		get
		{
			return 10.5f;
		}
	}
}

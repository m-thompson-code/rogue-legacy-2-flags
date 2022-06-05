using System;

// Token: 0x020000D0 RID: 208
public class FlyingAxe_Advanced_AIScript : FlyingAxe_Basic_AIScript
{
	// Token: 0x170002ED RID: 749
	// (get) Token: 0x060005F8 RID: 1528 RVA: 0x00018BEB File Offset: 0x00016DEB
	protected override float m_sideSpin_Attack_TurnSpeed
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x060005F9 RID: 1529 RVA: 0x00018BF2 File Offset: 0x00016DF2
	protected override float m_sideSpin_Attack_MovementSpeed
	{
		get
		{
			return 10.5f;
		}
	}
}

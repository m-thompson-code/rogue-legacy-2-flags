using System;

// Token: 0x020000E4 RID: 228
public class FlyingShield_Miniboss_AIScript : FlyingShield_Basic_AIScript
{
	// Token: 0x17000390 RID: 912
	// (get) Token: 0x060006DC RID: 1756 RVA: 0x00019ED3 File Offset: 0x000180D3
	protected override float m_spinMove_Attack_MoveSpeed
	{
		get
		{
			return 27f;
		}
	}

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x060006DD RID: 1757 RVA: 0x00019EDA File Offset: 0x000180DA
	protected override float m_spinMove_Attack_Duration
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17000392 RID: 914
	// (get) Token: 0x060006DE RID: 1758 RVA: 0x00019EE1 File Offset: 0x000180E1
	protected override bool m_hasTailSpin
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x060006DF RID: 1759 RVA: 0x00019EE4 File Offset: 0x000180E4
	protected override bool m_hasTailRam
	{
		get
		{
			return true;
		}
	}
}

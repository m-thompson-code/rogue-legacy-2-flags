using System;

// Token: 0x020000E1 RID: 225
public class FlyingShield_Advanced_AIScript : FlyingShield_Basic_AIScript
{
	// Token: 0x17000365 RID: 869
	// (get) Token: 0x060006A6 RID: 1702 RVA: 0x00019B19 File Offset: 0x00017D19
	protected override bool m_hasTailSpin
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x060006A7 RID: 1703 RVA: 0x00019B1C File Offset: 0x00017D1C
	protected override bool m_hasTailRam
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000367 RID: 871
	// (get) Token: 0x060006A8 RID: 1704 RVA: 0x00019B1F File Offset: 0x00017D1F
	protected override float m_spinMove_Attack_FlameDrops
	{
		get
		{
			return 0f;
		}
	}
}

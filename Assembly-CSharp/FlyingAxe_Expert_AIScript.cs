using System;

// Token: 0x02000152 RID: 338
public class FlyingAxe_Expert_AIScript : FlyingAxe_Basic_AIScript
{
	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x060008E8 RID: 2280 RVA: 0x00006308 File Offset: 0x00004508
	protected override float m_sideSpin_Attack_TurnSpeed
	{
		get
		{
			return 105f;
		}
	}

	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x060008E9 RID: 2281 RVA: 0x00003CC4 File Offset: 0x00001EC4
	protected override float m_sideSpin_Attack_MovementSpeed
	{
		get
		{
			return 12f;
		}
	}

	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x060008EA RID: 2282 RVA: 0x00005319 File Offset: 0x00003519
	protected override float m_vertSpin_Attack_MovementSpeed
	{
		get
		{
			return 2.5f;
		}
	}
}

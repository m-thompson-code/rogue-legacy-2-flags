using System;

// Token: 0x020000EC RID: 236
public class FlyingSword_Miniboss_AIScript : FlyingSword_Basic_AIScript
{
	// Token: 0x170003DC RID: 988
	// (get) Token: 0x06000740 RID: 1856 RVA: 0x0001A3E3 File Offset: 0x000185E3
	protected override float m_thrust_Attack_TurnRate
	{
		get
		{
			return 45f;
		}
	}

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x06000741 RID: 1857 RVA: 0x0001A3EA File Offset: 0x000185EA
	protected override float m_thrustFast_DashAttackAmount
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x06000742 RID: 1858 RVA: 0x0001A3F1 File Offset: 0x000185F1
	protected override bool m_thrust_SpawnMinibossProjectilesAtEnd
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003DF RID: 991
	// (get) Token: 0x06000743 RID: 1859 RVA: 0x0001A3F4 File Offset: 0x000185F4
	protected override int m_vertSpin_Attack_TotalLoops
	{
		get
		{
			return 5;
		}
	}
}

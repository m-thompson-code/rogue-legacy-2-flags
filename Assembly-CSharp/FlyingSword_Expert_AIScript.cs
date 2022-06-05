using System;

// Token: 0x02000180 RID: 384
public class FlyingSword_Expert_AIScript : FlyingSword_Basic_AIScript
{
	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x06000A8E RID: 2702 RVA: 0x00006988 File Offset: 0x00004B88
	protected override float m_thrust_Attack_TurnRate
	{
		get
		{
			return 55f;
		}
	}

	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x06000A8F RID: 2703 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float m_thrustFast_DashAttackAmount
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x06000A90 RID: 2704 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_thrust_SpawnProjectilesAtEnd
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x06000A91 RID: 2705 RVA: 0x000047A7 File Offset: 0x000029A7
	protected override int m_vertSpin_Attack_TotalLoops
	{
		get
		{
			return 4;
		}
	}
}

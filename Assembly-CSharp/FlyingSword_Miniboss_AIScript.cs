using System;

// Token: 0x02000181 RID: 385
public class FlyingSword_Miniboss_AIScript : FlyingSword_Basic_AIScript
{
	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x06000A93 RID: 2707 RVA: 0x00006B27 File Offset: 0x00004D27
	protected override float m_thrust_Attack_TurnRate
	{
		get
		{
			return 45f;
		}
	}

	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x06000A94 RID: 2708 RVA: 0x00004536 File Offset: 0x00002736
	protected override float m_thrustFast_DashAttackAmount
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x06000A95 RID: 2709 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_thrust_SpawnMinibossProjectilesAtEnd
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x06000A96 RID: 2710 RVA: 0x00004762 File Offset: 0x00002962
	protected override int m_vertSpin_Attack_TotalLoops
	{
		get
		{
			return 5;
		}
	}
}

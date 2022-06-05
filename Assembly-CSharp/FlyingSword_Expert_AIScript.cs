using System;

// Token: 0x020000EB RID: 235
public class FlyingSword_Expert_AIScript : FlyingSword_Basic_AIScript
{
	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x0600073B RID: 1851 RVA: 0x0001A3C7 File Offset: 0x000185C7
	protected override float m_thrust_Attack_TurnRate
	{
		get
		{
			return 55f;
		}
	}

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x0600073C RID: 1852 RVA: 0x0001A3CE File Offset: 0x000185CE
	protected override float m_thrustFast_DashAttackAmount
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x0600073D RID: 1853 RVA: 0x0001A3D5 File Offset: 0x000185D5
	protected override bool m_thrust_SpawnProjectilesAtEnd
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x0600073E RID: 1854 RVA: 0x0001A3D8 File Offset: 0x000185D8
	protected override int m_vertSpin_Attack_TotalLoops
	{
		get
		{
			return 4;
		}
	}
}

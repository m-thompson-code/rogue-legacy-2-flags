using System;

// Token: 0x020000E9 RID: 233
public class FlyingSword_Advanced_AIScript : FlyingSword_Basic_AIScript
{
	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x0600071D RID: 1821 RVA: 0x0001A15D File Offset: 0x0001835D
	protected override float m_thrust_Attack_TurnRate
	{
		get
		{
			return 55f;
		}
	}

	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x0600071E RID: 1822 RVA: 0x0001A164 File Offset: 0x00018364
	protected override bool m_thrust_SpawnProjectilesAtEnd
	{
		get
		{
			return true;
		}
	}
}

using System;

// Token: 0x02000178 RID: 376
public class FlyingSword_Advanced_AIScript : FlyingSword_Basic_AIScript
{
	// Token: 0x170004D4 RID: 1236
	// (get) Token: 0x06000A4C RID: 2636 RVA: 0x00006988 File Offset: 0x00004B88
	protected override float m_thrust_Attack_TurnRate
	{
		get
		{
			return 55f;
		}
	}

	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x06000A4D RID: 2637 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_thrust_SpawnProjectilesAtEnd
	{
		get
		{
			return true;
		}
	}
}

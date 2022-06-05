using System;

// Token: 0x020000E5 RID: 229
public class FlyingSkull_Advanced_AIScript : FlyingSkull_Basic_AIScript
{
	// Token: 0x17000394 RID: 916
	// (get) Token: 0x060006E1 RID: 1761 RVA: 0x00019EEF File Offset: 0x000180EF
	protected override bool m_shoot_ShootNear
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x060006E2 RID: 1762 RVA: 0x00019EF2 File Offset: 0x000180F2
	protected override bool m_shoot_ShootFar
	{
		get
		{
			return true;
		}
	}
}

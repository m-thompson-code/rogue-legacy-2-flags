using System;

// Token: 0x02000182 RID: 386
public class Ghost_Advanced_AIScript : Ghost_Basic_AIScript
{
	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x06000A98 RID: 2712 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_scare_fireCardinalProjectiles
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x06000A99 RID: 2713 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override bool m_scare_fireDiagonalProjectiles
	{
		get
		{
			return false;
		}
	}
}

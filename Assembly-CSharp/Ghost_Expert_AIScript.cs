using System;

// Token: 0x020000EF RID: 239
public class Ghost_Expert_AIScript : Ghost_Basic_AIScript
{
	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x0600075C RID: 1884 RVA: 0x0001A61C File Offset: 0x0001881C
	protected override bool m_scare_fireCardinalProjectiles
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x0600075D RID: 1885 RVA: 0x0001A61F File Offset: 0x0001881F
	protected override bool m_scare_fireDiagonalProjectiles
	{
		get
		{
			return true;
		}
	}
}

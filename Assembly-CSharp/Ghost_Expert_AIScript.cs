using System;

// Token: 0x02000185 RID: 389
public class Ghost_Expert_AIScript : Ghost_Basic_AIScript
{
	// Token: 0x1700050C RID: 1292
	// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_scare_fireCardinalProjectiles
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700050D RID: 1293
	// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_scare_fireDiagonalProjectiles
	{
		get
		{
			return true;
		}
	}
}

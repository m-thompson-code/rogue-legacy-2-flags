using System;

// Token: 0x020000ED RID: 237
public class Ghost_Advanced_AIScript : Ghost_Basic_AIScript
{
	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x06000745 RID: 1861 RVA: 0x0001A3FF File Offset: 0x000185FF
	protected override bool m_scare_fireCardinalProjectiles
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x06000746 RID: 1862 RVA: 0x0001A402 File Offset: 0x00018602
	protected override bool m_scare_fireDiagonalProjectiles
	{
		get
		{
			return false;
		}
	}
}

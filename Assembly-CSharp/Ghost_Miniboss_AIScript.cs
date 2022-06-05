using System;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class Ghost_Miniboss_AIScript : Ghost_Basic_AIScript
{
	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x0600075F RID: 1887 RVA: 0x0001A62A File Offset: 0x0001882A
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-5f, 5f);
		}
	}

	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x06000760 RID: 1888 RVA: 0x0001A63B File Offset: 0x0001883B
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(-4f, 4f);
		}
	}

	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x06000761 RID: 1889 RVA: 0x0001A64C File Offset: 0x0001884C
	protected override float m_scare_TellIntroAndHold_Delay
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x06000762 RID: 1890 RVA: 0x0001A653 File Offset: 0x00018853
	protected override float m_scare_Exit_ForceIdle
	{
		get
		{
			return 5.5f;
		}
	}

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x06000763 RID: 1891 RVA: 0x0001A65A File Offset: 0x0001885A
	protected override bool m_scare_fireCardinalProjectiles
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x06000764 RID: 1892 RVA: 0x0001A65D File Offset: 0x0001885D
	protected override bool m_scare_fireDiagonalProjectiles
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x06000765 RID: 1893 RVA: 0x0001A660 File Offset: 0x00018860
	protected override bool m_scare_fireMinibossProjectiles
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x06000766 RID: 1894 RVA: 0x0001A663 File Offset: 0x00018863
	protected override bool m_scare_isMiniboss
	{
		get
		{
			return true;
		}
	}
}

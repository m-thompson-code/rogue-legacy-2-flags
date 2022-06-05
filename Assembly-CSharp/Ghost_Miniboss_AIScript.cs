using System;
using UnityEngine;

// Token: 0x02000186 RID: 390
public class Ghost_Miniboss_AIScript : Ghost_Basic_AIScript
{
	// Token: 0x1700050E RID: 1294
	// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00005065 File Offset: 0x00003265
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-5f, 5f);
		}
	}

	// Token: 0x1700050F RID: 1295
	// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x00006C26 File Offset: 0x00004E26
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(-4f, 4f);
		}
	}

	// Token: 0x17000510 RID: 1296
	// (get) Token: 0x06000ABA RID: 2746 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float m_scare_TellIntroAndHold_Delay
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000511 RID: 1297
	// (get) Token: 0x06000ABB RID: 2747 RVA: 0x00005FB1 File Offset: 0x000041B1
	protected override float m_scare_Exit_ForceIdle
	{
		get
		{
			return 5.5f;
		}
	}

	// Token: 0x17000512 RID: 1298
	// (get) Token: 0x06000ABC RID: 2748 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override bool m_scare_fireCardinalProjectiles
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000513 RID: 1299
	// (get) Token: 0x06000ABD RID: 2749 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override bool m_scare_fireDiagonalProjectiles
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000514 RID: 1300
	// (get) Token: 0x06000ABE RID: 2750 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_scare_fireMinibossProjectiles
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000515 RID: 1301
	// (get) Token: 0x06000ABF RID: 2751 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_scare_isMiniboss
	{
		get
		{
			return true;
		}
	}
}

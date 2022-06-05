using System;
using UnityEngine;

// Token: 0x0200027D RID: 637
public class Zombie_Expert_AIScript : Zombie_Basic_AIScript
{
	// Token: 0x170008AE RID: 2222
	// (get) Token: 0x06001270 RID: 4720 RVA: 0x00003D93 File Offset: 0x00001F93
	protected override float m_tunnel_moveSpeed
	{
		get
		{
			return 6f;
		}
	}

	// Token: 0x170008AF RID: 2223
	// (get) Token: 0x06001271 RID: 4721 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float Lunge_Count
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170008B0 RID: 2224
	// (get) Token: 0x06001272 RID: 4722 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected override float Delay_Between_Lunges
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170008B1 RID: 2225
	// (get) Token: 0x06001273 RID: 4723 RVA: 0x000096A5 File Offset: 0x000078A5
	protected override Vector2 swing_Dash_AttackSpeed
	{
		get
		{
			return new Vector2(17f, 0f);
		}
	}
}

using System;
using UnityEngine;

// Token: 0x02000155 RID: 341
public class Zombie_Expert_AIScript : Zombie_Basic_AIScript
{
	// Token: 0x17000670 RID: 1648
	// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x000233CC File Offset: 0x000215CC
	protected override float m_tunnel_moveSpeed
	{
		get
		{
			return 6f;
		}
	}

	// Token: 0x17000671 RID: 1649
	// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x000233D3 File Offset: 0x000215D3
	protected override float Lunge_Count
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000672 RID: 1650
	// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x000233DA File Offset: 0x000215DA
	protected override float Delay_Between_Lunges
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000673 RID: 1651
	// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x000233E1 File Offset: 0x000215E1
	protected override Vector2 swing_Dash_AttackSpeed
	{
		get
		{
			return new Vector2(17f, 0f);
		}
	}
}

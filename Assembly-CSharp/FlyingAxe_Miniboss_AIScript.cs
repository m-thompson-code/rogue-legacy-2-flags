using System;
using UnityEngine;

// Token: 0x020000D3 RID: 211
public class FlyingAxe_Miniboss_AIScript : FlyingAxe_Basic_AIScript
{
	// Token: 0x17000302 RID: 770
	// (get) Token: 0x06000617 RID: 1559 RVA: 0x00018E21 File Offset: 0x00017021
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-3f, 3f);
		}
	}

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x06000618 RID: 1560 RVA: 0x00018E32 File Offset: 0x00017032
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(1f, 3f);
		}
	}

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x06000619 RID: 1561 RVA: 0x00018E43 File Offset: 0x00017043
	protected override float m_sideSpin_Attack_TurnSpeed
	{
		get
		{
			return 95f;
		}
	}

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x0600061A RID: 1562 RVA: 0x00018E4A File Offset: 0x0001704A
	protected override float m_sideSpin_Attack_MovementSpeed
	{
		get
		{
			return 12.5f;
		}
	}

	// Token: 0x17000306 RID: 774
	// (get) Token: 0x0600061B RID: 1563 RVA: 0x00018E51 File Offset: 0x00017051
	protected override float m_sideSpin_Exit_AttackCD
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000307 RID: 775
	// (get) Token: 0x0600061C RID: 1564 RVA: 0x00018E58 File Offset: 0x00017058
	protected override float m_vertSpin_Attack_MovementSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000308 RID: 776
	// (get) Token: 0x0600061D RID: 1565 RVA: 0x00018E5F File Offset: 0x0001705F
	protected override float m_vertSpin_Attack_ChaseDuration
	{
		get
		{
			return 2.75f;
		}
	}

	// Token: 0x17000309 RID: 777
	// (get) Token: 0x0600061E RID: 1566 RVA: 0x00018E66 File Offset: 0x00017066
	protected override bool m_vertSpin_Attack_FireBulletsWhileSpinning
	{
		get
		{
			return true;
		}
	}
}

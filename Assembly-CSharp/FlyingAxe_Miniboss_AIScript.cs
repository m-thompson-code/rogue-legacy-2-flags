using System;
using UnityEngine;

// Token: 0x02000153 RID: 339
public class FlyingAxe_Miniboss_AIScript : FlyingAxe_Basic_AIScript
{
	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x060008EC RID: 2284 RVA: 0x00003A2C File Offset: 0x00001C2C
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-3f, 3f);
		}
	}

	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x060008ED RID: 2285 RVA: 0x0000630F File Offset: 0x0000450F
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(1f, 3f);
		}
	}

	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x060008EE RID: 2286 RVA: 0x00006320 File Offset: 0x00004520
	protected override float m_sideSpin_Attack_TurnSpeed
	{
		get
		{
			return 95f;
		}
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x060008EF RID: 2287 RVA: 0x00006327 File Offset: 0x00004527
	protected override float m_sideSpin_Attack_MovementSpeed
	{
		get
		{
			return 12.5f;
		}
	}

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x060008F0 RID: 2288 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float m_sideSpin_Exit_AttackCD
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x060008F1 RID: 2289 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_vertSpin_Attack_MovementSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x060008F2 RID: 2290 RVA: 0x00004573 File Offset: 0x00002773
	protected override float m_vertSpin_Attack_ChaseDuration
	{
		get
		{
			return 2.75f;
		}
	}

	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x060008F3 RID: 2291 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_vertSpin_Attack_FireBulletsWhileSpinning
	{
		get
		{
			return true;
		}
	}
}

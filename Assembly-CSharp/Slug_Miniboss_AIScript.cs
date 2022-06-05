using System;
using UnityEngine;

// Token: 0x02000120 RID: 288
public class Slug_Miniboss_AIScript : Slug_Basic_AIScript
{
	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x060008E2 RID: 2274 RVA: 0x0001D755 File Offset: 0x0001B955
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.5f, 0.5f);
		}
	}

	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x060008E3 RID: 2275 RVA: 0x0001D766 File Offset: 0x0001B966
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.75f, 0.75f);
		}
	}

	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x060008E4 RID: 2276 RVA: 0x0001D777 File Offset: 0x0001B977
	protected override float m_WalkTowards_ForceIdle
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x060008E5 RID: 2277 RVA: 0x0001D77E File Offset: 0x0001B97E
	protected override int m_trail_ProjectileAmount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x060008E6 RID: 2278 RVA: 0x0001D781 File Offset: 0x0001B981
	protected override int m_verticalShot_TotalShotSpread
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x060008E7 RID: 2279 RVA: 0x0001D784 File Offset: 0x0001B984
	protected override float m_verticalShot_SpeedMod
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x060008E8 RID: 2280 RVA: 0x0001D78B File Offset: 0x0001B98B
	protected override int m_verticalShot_RepeatAttackPattern
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0001D78E File Offset: 0x0001B98E
	protected override float m_verticalShot_RepeatAttackPatternDelay
	{
		get
		{
			return 0.25f;
		}
	}
}

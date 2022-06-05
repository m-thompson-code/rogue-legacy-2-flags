using System;
using UnityEngine;

// Token: 0x020001EC RID: 492
public class Slug_Miniboss_AIScript : Slug_Basic_AIScript
{
	// Token: 0x17000644 RID: 1604
	// (get) Token: 0x06000D7F RID: 3455 RVA: 0x0000746B File Offset: 0x0000566B
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.5f, 0.5f);
		}
	}

	// Token: 0x17000645 RID: 1605
	// (get) Token: 0x06000D80 RID: 3456 RVA: 0x00007B91 File Offset: 0x00005D91
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.75f, 0.75f);
		}
	}

	// Token: 0x17000646 RID: 1606
	// (get) Token: 0x06000D81 RID: 3457 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_WalkTowards_ForceIdle
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000647 RID: 1607
	// (get) Token: 0x06000D82 RID: 3458 RVA: 0x000047A4 File Offset: 0x000029A4
	protected override int m_trail_ProjectileAmount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000648 RID: 1608
	// (get) Token: 0x06000D83 RID: 3459 RVA: 0x000047A4 File Offset: 0x000029A4
	protected override int m_verticalShot_TotalShotSpread
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000649 RID: 1609
	// (get) Token: 0x06000D84 RID: 3460 RVA: 0x00005319 File Offset: 0x00003519
	protected override float m_verticalShot_SpeedMod
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x1700064A RID: 1610
	// (get) Token: 0x06000D85 RID: 3461 RVA: 0x00004762 File Offset: 0x00002962
	protected override int m_verticalShot_RepeatAttackPattern
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x06000D86 RID: 3462 RVA: 0x00004A00 File Offset: 0x00002C00
	protected override float m_verticalShot_RepeatAttackPatternDelay
	{
		get
		{
			return 0.25f;
		}
	}
}

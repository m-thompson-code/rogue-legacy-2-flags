using System;
using UnityEngine;

// Token: 0x02000156 RID: 342
public class Zombie_Miniboss_AIScript : Zombie_Basic_AIScript
{
	// Token: 0x17000674 RID: 1652
	// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x000233FA File Offset: 0x000215FA
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.75f, 1f);
		}
	}

	// Token: 0x17000675 RID: 1653
	// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x0002340B File Offset: 0x0002160B
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.75f, 1f);
		}
	}

	// Token: 0x17000676 RID: 1654
	// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x0002341C File Offset: 0x0002161C
	protected override float DigDownAnimSpeed
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x17000677 RID: 1655
	// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x00023423 File Offset: 0x00021623
	protected override float DigUpAnimSpeed
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x17000678 RID: 1656
	// (get) Token: 0x06000BBA RID: 3002 RVA: 0x0002342A File Offset: 0x0002162A
	protected override Vector2 swing_Dash_AttackSpeed
	{
		get
		{
			return new Vector2(17f, 0f);
		}
	}

	// Token: 0x17000679 RID: 1657
	// (get) Token: 0x06000BBB RID: 3003 RVA: 0x0002343B File Offset: 0x0002163B
	protected override float m_swing_Dash_AttackTime
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x1700067A RID: 1658
	// (get) Token: 0x06000BBC RID: 3004 RVA: 0x00023442 File Offset: 0x00021642
	protected override float m_swing_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700067B RID: 1659
	// (get) Token: 0x06000BBD RID: 3005 RVA: 0x00023449 File Offset: 0x00021649
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-15f, 15f);
		}
	}

	// Token: 0x1700067C RID: 1660
	// (get) Token: 0x06000BBE RID: 3006 RVA: 0x0002345A File Offset: 0x0002165A
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(-9f, 9f);
		}
	}
}

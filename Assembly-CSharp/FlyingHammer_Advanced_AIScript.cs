using System;
using UnityEngine;

// Token: 0x020000D8 RID: 216
public class FlyingHammer_Advanced_AIScript : FlyingHammer_Basic_AIScript
{
	// Token: 0x17000329 RID: 809
	// (get) Token: 0x06000648 RID: 1608 RVA: 0x0001913B File Offset: 0x0001733B
	protected override int m_projectileSpawnAmount
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x1700032A RID: 810
	// (get) Token: 0x06000649 RID: 1609 RVA: 0x0001913E File Offset: 0x0001733E
	protected override Vector2 m_fireballSpeedMod
	{
		get
		{
			return new Vector2(0.25f, 1.5f);
		}
	}

	// Token: 0x1700032B RID: 811
	// (get) Token: 0x0600064A RID: 1610 RVA: 0x0001914F File Offset: 0x0001734F
	protected override float m_shockwave_initialProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700032C RID: 812
	// (get) Token: 0x0600064B RID: 1611 RVA: 0x00019156 File Offset: 0x00017356
	protected override float m_shockwave_exitProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700032D RID: 813
	// (get) Token: 0x0600064C RID: 1612 RVA: 0x0001915D File Offset: 0x0001735D
	protected override int m_shockwave_projectileSpawnAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x0600064D RID: 1613 RVA: 0x00019160 File Offset: 0x00017360
	protected override int m_shockwave_projectileSpawnLoopCount
	{
		get
		{
			return 10;
		}
	}
}

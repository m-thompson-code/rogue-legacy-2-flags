using System;
using UnityEngine;

// Token: 0x0200015C RID: 348
public class FlyingHammer_Advanced_AIScript : FlyingHammer_Basic_AIScript
{
	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x06000935 RID: 2357 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected override int m_projectileSpawnAmount
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x06000936 RID: 2358 RVA: 0x000063F7 File Offset: 0x000045F7
	protected override Vector2 m_fireballSpeedMod
	{
		get
		{
			return new Vector2(0.25f, 1.5f);
		}
	}

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x06000937 RID: 2359 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_shockwave_initialProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x06000938 RID: 2360 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_shockwave_exitProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x06000939 RID: 2361 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override int m_shockwave_projectileSpawnAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000426 RID: 1062
	// (get) Token: 0x0600093A RID: 2362 RVA: 0x000046FA File Offset: 0x000028FA
	protected override int m_shockwave_projectileSpawnLoopCount
	{
		get
		{
			return 10;
		}
	}
}

using System;
using UnityEngine;

// Token: 0x02000162 RID: 354
public class FlyingHammer_Expert_AIScript : FlyingHammer_Basic_AIScript
{
	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x06000971 RID: 2417 RVA: 0x00006567 File Offset: 0x00004767
	protected override Vector2 m_fireballSpeedMod
	{
		get
		{
			return new Vector2(0.25f, 2f);
		}
	}

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x06000972 RID: 2418 RVA: 0x000047A4 File Offset: 0x000029A4
	protected override int m_projectileSpawnAmount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x06000973 RID: 2419 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_shockwave_IsLarge
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000448 RID: 1096
	// (get) Token: 0x06000974 RID: 2420 RVA: 0x00006220 File Offset: 0x00004420
	protected override float m_shockwave_Attack_TurnSpeed
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x06000975 RID: 2421 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_shockwave_initialProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x06000976 RID: 2422 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_shockwave_exitProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700044B RID: 1099
	// (get) Token: 0x06000977 RID: 2423 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override int m_shockwave_projectileSpawnAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x06000978 RID: 2424 RVA: 0x00005303 File Offset: 0x00003503
	protected override int m_shockwave_projectileSpawnLoopCount
	{
		get
		{
			return 12;
		}
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x00006578 File Offset: 0x00004778
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
	}
}

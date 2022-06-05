using System;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class FlyingHammer_Expert_AIScript : FlyingHammer_Basic_AIScript
{
	// Token: 0x17000347 RID: 839
	// (get) Token: 0x06000672 RID: 1650 RVA: 0x0001973E File Offset: 0x0001793E
	protected override Vector2 m_fireballSpeedMod
	{
		get
		{
			return new Vector2(0.25f, 2f);
		}
	}

	// Token: 0x17000348 RID: 840
	// (get) Token: 0x06000673 RID: 1651 RVA: 0x0001974F File Offset: 0x0001794F
	protected override int m_projectileSpawnAmount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000349 RID: 841
	// (get) Token: 0x06000674 RID: 1652 RVA: 0x00019752 File Offset: 0x00017952
	protected override bool m_shockwave_IsLarge
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x06000675 RID: 1653 RVA: 0x00019755 File Offset: 0x00017955
	protected override float m_shockwave_Attack_TurnSpeed
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x1700034B RID: 843
	// (get) Token: 0x06000676 RID: 1654 RVA: 0x0001975C File Offset: 0x0001795C
	protected override float m_shockwave_initialProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x06000677 RID: 1655 RVA: 0x00019763 File Offset: 0x00017963
	protected override float m_shockwave_exitProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x06000678 RID: 1656 RVA: 0x0001976A File Offset: 0x0001796A
	protected override int m_shockwave_projectileSpawnAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700034E RID: 846
	// (get) Token: 0x06000679 RID: 1657 RVA: 0x0001976D File Offset: 0x0001796D
	protected override int m_shockwave_projectileSpawnLoopCount
	{
		get
		{
			return 12;
		}
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x00019771 File Offset: 0x00017971
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
	}
}

using System;

// Token: 0x020000DC RID: 220
public class FlyingHammer_Miniboss_AIScript : FlyingHammer_Basic_AIScript
{
	// Token: 0x1700034F RID: 847
	// (get) Token: 0x0600067C RID: 1660 RVA: 0x00019782 File Offset: 0x00017982
	protected override bool m_shockwave_IsLarge
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000350 RID: 848
	// (get) Token: 0x0600067D RID: 1661 RVA: 0x00019785 File Offset: 0x00017985
	protected override float m_shockwave_Attack_TurnSpeed
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x17000351 RID: 849
	// (get) Token: 0x0600067E RID: 1662 RVA: 0x0001978C File Offset: 0x0001798C
	protected override float m_shockwave_initialProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000352 RID: 850
	// (get) Token: 0x0600067F RID: 1663 RVA: 0x00019793 File Offset: 0x00017993
	protected override float m_shockwave_exitProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000353 RID: 851
	// (get) Token: 0x06000680 RID: 1664 RVA: 0x0001979A File Offset: 0x0001799A
	protected override int m_shockwave_projectileSpawnAmount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000354 RID: 852
	// (get) Token: 0x06000681 RID: 1665 RVA: 0x0001979D File Offset: 0x0001799D
	protected override int m_shockwave_projectileSpawnLoopCount
	{
		get
		{
			return 5;
		}
	}
}

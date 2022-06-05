using System;

// Token: 0x02000163 RID: 355
public class FlyingHammer_Miniboss_AIScript : FlyingHammer_Basic_AIScript
{
	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x0600097B RID: 2427 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_shockwave_IsLarge
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x0600097C RID: 2428 RVA: 0x00006220 File Offset: 0x00004420
	protected override float m_shockwave_Attack_TurnSpeed
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x1700044F RID: 1103
	// (get) Token: 0x0600097D RID: 2429 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_shockwave_initialProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000450 RID: 1104
	// (get) Token: 0x0600097E RID: 2430 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_shockwave_exitProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000451 RID: 1105
	// (get) Token: 0x0600097F RID: 2431 RVA: 0x000047A4 File Offset: 0x000029A4
	protected override int m_shockwave_projectileSpawnAmount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x06000980 RID: 2432 RVA: 0x00004762 File Offset: 0x00002962
	protected override int m_shockwave_projectileSpawnLoopCount
	{
		get
		{
			return 5;
		}
	}
}

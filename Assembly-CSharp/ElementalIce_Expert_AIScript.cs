using System;

// Token: 0x020000B9 RID: 185
public class ElementalIce_Expert_AIScript : ElementalIce_Basic_AIScript
{
	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06000494 RID: 1172 RVA: 0x00015D2C File Offset: 0x00013F2C
	protected override bool m_shoot_CentreShot
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06000495 RID: 1173 RVA: 0x00015D2F File Offset: 0x00013F2F
	protected override int m_shoot_TotalSideShots
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06000496 RID: 1174 RVA: 0x00015D32 File Offset: 0x00013F32
	protected override float m_spinAttack_TimesShotDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06000497 RID: 1175 RVA: 0x00015D39 File Offset: 0x00013F39
	protected override int m_spinAttack_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06000498 RID: 1176 RVA: 0x00015D3C File Offset: 0x00013F3C
	protected override int m_spinAttack_ShotPatternLoops
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06000499 RID: 1177 RVA: 0x00015D3F File Offset: 0x00013F3F
	protected override float m_spinAttack_projectile_RandomSpread
	{
		get
		{
			return 180f;
		}
	}
}

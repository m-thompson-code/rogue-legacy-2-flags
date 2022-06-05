using System;

// Token: 0x0200009E RID: 158
public class DancingBoss_Advanced_AIScript : DancingBoss_Basic_AIScript
{
	// Token: 0x170000AD RID: 173
	// (get) Token: 0x060002CB RID: 715 RVA: 0x00014046 File Offset: 0x00012246
	protected override bool m_advancedAttacks
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x060002CC RID: 716 RVA: 0x00014049 File Offset: 0x00012249
	protected override float m_bomb_projectileDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x060002CD RID: 717 RVA: 0x00014050 File Offset: 0x00012250
	protected override int m_bomb_projectileAmount
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x060002CE RID: 718 RVA: 0x00014053 File Offset: 0x00012253
	protected override int m_verticalWave_BounceProjectileCount
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x060002CF RID: 719 RVA: 0x00014056 File Offset: 0x00012256
	protected override int m_horizontalWave_BounceProjectileCount
	{
		get
		{
			return 6;
		}
	}
}

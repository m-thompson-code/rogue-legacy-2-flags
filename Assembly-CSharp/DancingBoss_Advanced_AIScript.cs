using System;

// Token: 0x020000D2 RID: 210
public class DancingBoss_Advanced_AIScript : DancingBoss_Basic_AIScript
{
	// Token: 0x1700010B RID: 267
	// (get) Token: 0x060003E7 RID: 999 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_advancedAttacks
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00004A00 File Offset: 0x00002C00
	protected override float m_bomb_projectileDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x060003E9 RID: 1001 RVA: 0x00004762 File Offset: 0x00002962
	protected override int m_bomb_projectileAmount
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x060003EA RID: 1002 RVA: 0x00004A07 File Offset: 0x00002C07
	protected override int m_verticalWave_BounceProjectileCount
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x060003EB RID: 1003 RVA: 0x00004792 File Offset: 0x00002992
	protected override int m_horizontalWave_BounceProjectileCount
	{
		get
		{
			return 6;
		}
	}
}

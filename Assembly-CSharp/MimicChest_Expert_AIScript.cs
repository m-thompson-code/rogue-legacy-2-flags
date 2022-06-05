using System;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class MimicChest_Expert_AIScript : MimicChest_Basic_AIScript
{
	// Token: 0x1700056F RID: 1391
	// (get) Token: 0x06000B6E RID: 2926 RVA: 0x00006EBF File Offset: 0x000050BF
	protected override Vector2 JumpHeight
	{
		get
		{
			return new Vector2(16.5f, 16.5f);
		}
	}

	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x06000B6F RID: 2927 RVA: 0x000047A4 File Offset: 0x000029A4
	protected override int DeathCoinAttack_CoinSpreadAmount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000571 RID: 1393
	// (get) Token: 0x06000B70 RID: 2928 RVA: 0x00003CC4 File Offset: 0x00001EC4
	protected override float DeathCoinAttack_AngleSpread
	{
		get
		{
			return 12f;
		}
	}

	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x06000B71 RID: 2929 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_canDashWake
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x06000B72 RID: 2930 RVA: 0x00005FB1 File Offset: 0x000041B1
	protected override float m_dashWakeActivationRange
	{
		get
		{
			return 5.5f;
		}
	}

	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x06000B73 RID: 2931 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float m_dashWakeAttackOdds
	{
		get
		{
			return 1f;
		}
	}
}

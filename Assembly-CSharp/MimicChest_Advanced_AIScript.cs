using System;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class MimicChest_Advanced_AIScript : MimicChest_Basic_AIScript
{
	// Token: 0x1700053E RID: 1342
	// (get) Token: 0x06000B10 RID: 2832 RVA: 0x00006EBF File Offset: 0x000050BF
	protected override Vector2 JumpHeight
	{
		get
		{
			return new Vector2(16.5f, 16.5f);
		}
	}

	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x06000B11 RID: 2833 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected override int DeathCoinAttack_CoinSpreadAmount
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x06000B12 RID: 2834 RVA: 0x00003D9A File Offset: 0x00001F9A
	protected override float DeathCoinAttack_AngleSpread
	{
		get
		{
			return 16f;
		}
	}

	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x06000B13 RID: 2835 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_canDashWake
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x06000B14 RID: 2836 RVA: 0x00005FB1 File Offset: 0x000041B1
	protected override float m_dashWakeActivationRange
	{
		get
		{
			return 5.5f;
		}
	}

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x06000B15 RID: 2837 RVA: 0x0000457A File Offset: 0x0000277A
	protected override float m_dashWakeAttackOdds
	{
		get
		{
			return 0.5f;
		}
	}
}

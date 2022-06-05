using System;
using UnityEngine;

// Token: 0x020000FB RID: 251
public class MimicChest_Expert_AIScript : MimicChest_Basic_AIScript
{
	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x060007DF RID: 2015 RVA: 0x0001B49E File Offset: 0x0001969E
	protected override Vector2 JumpHeight
	{
		get
		{
			return new Vector2(16.5f, 16.5f);
		}
	}

	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x060007E0 RID: 2016 RVA: 0x0001B4AF File Offset: 0x000196AF
	protected override int DeathCoinAttack_CoinSpreadAmount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x060007E1 RID: 2017 RVA: 0x0001B4B2 File Offset: 0x000196B2
	protected override float DeathCoinAttack_AngleSpread
	{
		get
		{
			return 12f;
		}
	}

	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x060007E2 RID: 2018 RVA: 0x0001B4B9 File Offset: 0x000196B9
	protected override bool m_canDashWake
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0001B4BC File Offset: 0x000196BC
	protected override float m_dashWakeActivationRange
	{
		get
		{
			return 5.5f;
		}
	}

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x060007E4 RID: 2020 RVA: 0x0001B4C3 File Offset: 0x000196C3
	protected override float m_dashWakeAttackOdds
	{
		get
		{
			return 1f;
		}
	}
}

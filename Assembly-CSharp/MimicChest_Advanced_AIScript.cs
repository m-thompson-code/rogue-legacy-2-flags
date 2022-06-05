using System;
using UnityEngine;

// Token: 0x020000F9 RID: 249
public class MimicChest_Advanced_AIScript : MimicChest_Basic_AIScript
{
	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x0600079F RID: 1951 RVA: 0x0001AA90 File Offset: 0x00018C90
	protected override Vector2 JumpHeight
	{
		get
		{
			return new Vector2(16.5f, 16.5f);
		}
	}

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x060007A0 RID: 1952 RVA: 0x0001AAA1 File Offset: 0x00018CA1
	protected override int DeathCoinAttack_CoinSpreadAmount
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x060007A1 RID: 1953 RVA: 0x0001AAA4 File Offset: 0x00018CA4
	protected override float DeathCoinAttack_AngleSpread
	{
		get
		{
			return 16f;
		}
	}

	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x060007A2 RID: 1954 RVA: 0x0001AAAB File Offset: 0x00018CAB
	protected override bool m_canDashWake
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x060007A3 RID: 1955 RVA: 0x0001AAAE File Offset: 0x00018CAE
	protected override float m_dashWakeActivationRange
	{
		get
		{
			return 5.5f;
		}
	}

	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x060007A4 RID: 1956 RVA: 0x0001AAB5 File Offset: 0x00018CB5
	protected override float m_dashWakeAttackOdds
	{
		get
		{
			return 0.5f;
		}
	}
}

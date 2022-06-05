using System;

// Token: 0x020002BD RID: 701
public class CloakStrikeNew_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x170009A3 RID: 2467
	// (get) Token: 0x060014A9 RID: 5289 RVA: 0x00004A6C File Offset: 0x00002C6C
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2.25f;
		}
	}

	// Token: 0x170009A4 RID: 2468
	// (get) Token: 0x060014AA RID: 5290 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009A5 RID: 2469
	// (get) Token: 0x060014AB RID: 5291 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float TellAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170009A6 RID: 2470
	// (get) Token: 0x060014AC RID: 5292 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009A7 RID: 2471
	// (get) Token: 0x060014AD RID: 5293 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170009A8 RID: 2472
	// (get) Token: 0x060014AE RID: 5294 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009A9 RID: 2473
	// (get) Token: 0x060014AF RID: 5295 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170009AA RID: 2474
	// (get) Token: 0x060014B0 RID: 5296 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170009AB RID: 2475
	// (get) Token: 0x060014B1 RID: 5297 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170009AC RID: 2476
	// (get) Token: 0x060014B2 RID: 5298 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}
}

using System;

// Token: 0x020002B6 RID: 694
public class ScreenSlice_Ability : BaseAbility_RL, ISpell, IAbility
{
	// Token: 0x1700097B RID: 2427
	// (get) Token: 0x06001462 RID: 5218 RVA: 0x00004536 File Offset: 0x00002736
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700097C RID: 2428
	// (get) Token: 0x06001463 RID: 5219 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700097D RID: 2429
	// (get) Token: 0x06001464 RID: 5220 RVA: 0x00004536 File Offset: 0x00002736
	protected override float TellAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700097E RID: 2430
	// (get) Token: 0x06001465 RID: 5221 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700097F RID: 2431
	// (get) Token: 0x06001466 RID: 5222 RVA: 0x00004FFB File Offset: 0x000031FB
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000980 RID: 2432
	// (get) Token: 0x06001467 RID: 5223 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000981 RID: 2433
	// (get) Token: 0x06001468 RID: 5224 RVA: 0x00004FFB File Offset: 0x000031FB
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000982 RID: 2434
	// (get) Token: 0x06001469 RID: 5225 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000983 RID: 2435
	// (get) Token: 0x0600146A RID: 5226 RVA: 0x00004536 File Offset: 0x00002736
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000984 RID: 2436
	// (get) Token: 0x0600146B RID: 5227 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}
}

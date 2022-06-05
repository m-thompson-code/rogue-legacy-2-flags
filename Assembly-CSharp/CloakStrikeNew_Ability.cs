using System;

// Token: 0x0200017B RID: 379
public class CloakStrikeNew_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x17000733 RID: 1843
	// (get) Token: 0x06000D52 RID: 3410 RVA: 0x00028ABD File Offset: 0x00026CBD
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2.25f;
		}
	}

	// Token: 0x17000734 RID: 1844
	// (get) Token: 0x06000D53 RID: 3411 RVA: 0x00028AC4 File Offset: 0x00026CC4
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000735 RID: 1845
	// (get) Token: 0x06000D54 RID: 3412 RVA: 0x00028ACB File Offset: 0x00026CCB
	protected override float TellAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000736 RID: 1846
	// (get) Token: 0x06000D55 RID: 3413 RVA: 0x00028AD2 File Offset: 0x00026CD2
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000737 RID: 1847
	// (get) Token: 0x06000D56 RID: 3414 RVA: 0x00028AD9 File Offset: 0x00026CD9
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000738 RID: 1848
	// (get) Token: 0x06000D57 RID: 3415 RVA: 0x00028AE0 File Offset: 0x00026CE0
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000739 RID: 1849
	// (get) Token: 0x06000D58 RID: 3416 RVA: 0x00028AE7 File Offset: 0x00026CE7
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700073A RID: 1850
	// (get) Token: 0x06000D59 RID: 3417 RVA: 0x00028AEE File Offset: 0x00026CEE
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700073B RID: 1851
	// (get) Token: 0x06000D5A RID: 3418 RVA: 0x00028AF5 File Offset: 0x00026CF5
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700073C RID: 1852
	// (get) Token: 0x06000D5B RID: 3419 RVA: 0x00028AFC File Offset: 0x00026CFC
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}
}

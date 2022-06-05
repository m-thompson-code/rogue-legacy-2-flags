using System;

// Token: 0x02000177 RID: 375
public class ScreenSlice_Ability : BaseAbility_RL, ISpell, IAbility
{
	// Token: 0x17000711 RID: 1809
	// (get) Token: 0x06000D1D RID: 3357 RVA: 0x00027DF6 File Offset: 0x00025FF6
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000712 RID: 1810
	// (get) Token: 0x06000D1E RID: 3358 RVA: 0x00027DFD File Offset: 0x00025FFD
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000713 RID: 1811
	// (get) Token: 0x06000D1F RID: 3359 RVA: 0x00027E04 File Offset: 0x00026004
	protected override float TellAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000714 RID: 1812
	// (get) Token: 0x06000D20 RID: 3360 RVA: 0x00027E0B File Offset: 0x0002600B
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000715 RID: 1813
	// (get) Token: 0x06000D21 RID: 3361 RVA: 0x00027E12 File Offset: 0x00026012
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000716 RID: 1814
	// (get) Token: 0x06000D22 RID: 3362 RVA: 0x00027E19 File Offset: 0x00026019
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000717 RID: 1815
	// (get) Token: 0x06000D23 RID: 3363 RVA: 0x00027E20 File Offset: 0x00026020
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000718 RID: 1816
	// (get) Token: 0x06000D24 RID: 3364 RVA: 0x00027E27 File Offset: 0x00026027
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000719 RID: 1817
	// (get) Token: 0x06000D25 RID: 3365 RVA: 0x00027E2E File Offset: 0x0002602E
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700071A RID: 1818
	// (get) Token: 0x06000D26 RID: 3366 RVA: 0x00027E35 File Offset: 0x00026035
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}
}

using System;

// Token: 0x02000195 RID: 405
public class Chakram_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x17000856 RID: 2134
	// (get) Token: 0x06000F3B RID: 3899 RVA: 0x0002D54E File Offset: 0x0002B74E
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000857 RID: 2135
	// (get) Token: 0x06000F3C RID: 3900 RVA: 0x0002D555 File Offset: 0x0002B755
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000858 RID: 2136
	// (get) Token: 0x06000F3D RID: 3901 RVA: 0x0002D55C File Offset: 0x0002B75C
	protected override float TellAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000859 RID: 2137
	// (get) Token: 0x06000F3E RID: 3902 RVA: 0x0002D563 File Offset: 0x0002B763
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700085A RID: 2138
	// (get) Token: 0x06000F3F RID: 3903 RVA: 0x0002D56A File Offset: 0x0002B76A
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x1700085B RID: 2139
	// (get) Token: 0x06000F40 RID: 3904 RVA: 0x0002D571 File Offset: 0x0002B771
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700085C RID: 2140
	// (get) Token: 0x06000F41 RID: 3905 RVA: 0x0002D578 File Offset: 0x0002B778
	protected override float AttackAnimSpeed
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x1700085D RID: 2141
	// (get) Token: 0x06000F42 RID: 3906 RVA: 0x0002D57F File Offset: 0x0002B77F
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x1700085E RID: 2142
	// (get) Token: 0x06000F43 RID: 3907 RVA: 0x0002D586 File Offset: 0x0002B786
	protected override float ExitAnimSpeed
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x1700085F RID: 2143
	// (get) Token: 0x06000F44 RID: 3908 RVA: 0x0002D58D File Offset: 0x0002B78D
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}
}

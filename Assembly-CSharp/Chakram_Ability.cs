using System;

// Token: 0x020002EC RID: 748
public class Chakram_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x17000AF0 RID: 2800
	// (get) Token: 0x06001710 RID: 5904 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000AF1 RID: 2801
	// (get) Token: 0x06001711 RID: 5905 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AF2 RID: 2802
	// (get) Token: 0x06001712 RID: 5906 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float TellAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000AF3 RID: 2803
	// (get) Token: 0x06001713 RID: 5907 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AF4 RID: 2804
	// (get) Token: 0x06001714 RID: 5908 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000AF5 RID: 2805
	// (get) Token: 0x06001715 RID: 5909 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000AF6 RID: 2806
	// (get) Token: 0x06001716 RID: 5910 RVA: 0x000052A9 File Offset: 0x000034A9
	protected override float AttackAnimSpeed
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x17000AF7 RID: 2807
	// (get) Token: 0x06001717 RID: 5911 RVA: 0x0000457A File Offset: 0x0000277A
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000AF8 RID: 2808
	// (get) Token: 0x06001718 RID: 5912 RVA: 0x00005FAA File Offset: 0x000041AA
	protected override float ExitAnimSpeed
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x17000AF9 RID: 2809
	// (get) Token: 0x06001719 RID: 5913 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}
}

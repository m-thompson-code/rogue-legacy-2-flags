using System;
using UnityEngine;

// Token: 0x020002A9 RID: 681
public class AimedAbilityFast_RL : AimedAbility_RL, ISpell, IAbility
{
	// Token: 0x17000931 RID: 2353
	// (get) Token: 0x060013DA RID: 5082 RVA: 0x0000676B File Offset: 0x0000496B
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3.25f;
		}
	}

	// Token: 0x17000932 RID: 2354
	// (get) Token: 0x060013DB RID: 5083 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000933 RID: 2355
	// (get) Token: 0x060013DC RID: 5084 RVA: 0x00003C62 File Offset: 0x00001E62
	protected override float TellAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000934 RID: 2356
	// (get) Token: 0x060013DD RID: 5085 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000935 RID: 2357
	// (get) Token: 0x060013DE RID: 5086 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000936 RID: 2358
	// (get) Token: 0x060013DF RID: 5087 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000937 RID: 2359
	// (get) Token: 0x060013E0 RID: 5088 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000938 RID: 2360
	// (get) Token: 0x060013E1 RID: 5089 RVA: 0x0000452F File Offset: 0x0000272F
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000939 RID: 2361
	// (get) Token: 0x060013E2 RID: 5090 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700093A RID: 2362
	// (get) Token: 0x060013E3 RID: 5091 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700093B RID: 2363
	// (get) Token: 0x060013E4 RID: 5092 RVA: 0x00006764 File Offset: 0x00004964
	protected virtual float GravityReduction
	{
		get
		{
			return 0.425f;
		}
	}

	// Token: 0x1700093C RID: 2364
	// (get) Token: 0x060013E5 RID: 5093 RVA: 0x0000A1D5 File Offset: 0x000083D5
	protected virtual Vector2 BowPushbackAmount
	{
		get
		{
			return new Vector2(0f, 11.5f);
		}
	}

	// Token: 0x1700093D RID: 2365
	// (get) Token: 0x060013E6 RID: 5094 RVA: 0x0000A1E6 File Offset: 0x000083E6
	public override Vector2 PushbackAmount
	{
		get
		{
			return this.BowPushbackAmount;
		}
	}

	// Token: 0x060013E7 RID: 5095 RVA: 0x0000A1EE File Offset: 0x000083EE
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		this.m_gravityReductionModWhenAiming = this.GravityReduction;
		base.Initialize(abilityController, castAbilityType);
	}
}

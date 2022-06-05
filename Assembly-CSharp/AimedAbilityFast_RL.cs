using System;
using UnityEngine;

// Token: 0x02000170 RID: 368
public class AimedAbilityFast_RL : AimedAbility_RL, ISpell, IAbility
{
	// Token: 0x170006D1 RID: 1745
	// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x000271C2 File Offset: 0x000253C2
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3.25f;
		}
	}

	// Token: 0x170006D2 RID: 1746
	// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x000271C9 File Offset: 0x000253C9
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006D3 RID: 1747
	// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x000271D0 File Offset: 0x000253D0
	protected override float TellAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170006D4 RID: 1748
	// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x000271D7 File Offset: 0x000253D7
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006D5 RID: 1749
	// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x000271DE File Offset: 0x000253DE
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170006D6 RID: 1750
	// (get) Token: 0x06000CBA RID: 3258 RVA: 0x000271E5 File Offset: 0x000253E5
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006D7 RID: 1751
	// (get) Token: 0x06000CBB RID: 3259 RVA: 0x000271EC File Offset: 0x000253EC
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170006D8 RID: 1752
	// (get) Token: 0x06000CBC RID: 3260 RVA: 0x000271F3 File Offset: 0x000253F3
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x170006D9 RID: 1753
	// (get) Token: 0x06000CBD RID: 3261 RVA: 0x000271FA File Offset: 0x000253FA
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170006DA RID: 1754
	// (get) Token: 0x06000CBE RID: 3262 RVA: 0x00027201 File Offset: 0x00025401
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006DB RID: 1755
	// (get) Token: 0x06000CBF RID: 3263 RVA: 0x00027208 File Offset: 0x00025408
	protected virtual float GravityReduction
	{
		get
		{
			return 0.425f;
		}
	}

	// Token: 0x170006DC RID: 1756
	// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x0002720F File Offset: 0x0002540F
	protected virtual Vector2 BowPushbackAmount
	{
		get
		{
			return new Vector2(0f, 11.5f);
		}
	}

	// Token: 0x170006DD RID: 1757
	// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x00027220 File Offset: 0x00025420
	public override Vector2 PushbackAmount
	{
		get
		{
			return this.BowPushbackAmount;
		}
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x00027228 File Offset: 0x00025428
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		this.m_gravityReductionModWhenAiming = this.GravityReduction;
		base.Initialize(abilityController, castAbilityType);
	}
}

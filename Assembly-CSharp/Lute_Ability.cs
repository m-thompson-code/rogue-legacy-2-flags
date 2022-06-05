using System;
using UnityEngine;

// Token: 0x020001A1 RID: 417
public class Lute_Ability : AimedAbilityFast_RL, IAttack, IAbility
{
	// Token: 0x170008E8 RID: 2280
	// (get) Token: 0x06001014 RID: 4116 RVA: 0x0002ED13 File Offset: 0x0002CF13
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170008E9 RID: 2281
	// (get) Token: 0x06001015 RID: 4117 RVA: 0x0002ED1A File Offset: 0x0002CF1A
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0.01f;
		}
	}

	// Token: 0x170008EA RID: 2282
	// (get) Token: 0x06001016 RID: 4118 RVA: 0x0002ED21 File Offset: 0x0002CF21
	protected override float TellAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170008EB RID: 2283
	// (get) Token: 0x06001017 RID: 4119 RVA: 0x0002ED28 File Offset: 0x0002CF28
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008EC RID: 2284
	// (get) Token: 0x06001018 RID: 4120 RVA: 0x0002ED2F File Offset: 0x0002CF2F
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170008ED RID: 2285
	// (get) Token: 0x06001019 RID: 4121 RVA: 0x0002ED36 File Offset: 0x0002CF36
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008EE RID: 2286
	// (get) Token: 0x0600101A RID: 4122 RVA: 0x0002ED3D File Offset: 0x0002CF3D
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170008EF RID: 2287
	// (get) Token: 0x0600101B RID: 4123 RVA: 0x0002ED44 File Offset: 0x0002CF44
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.175f;
		}
	}

	// Token: 0x170008F0 RID: 2288
	// (get) Token: 0x0600101C RID: 4124 RVA: 0x0002ED4B File Offset: 0x0002CF4B
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170008F1 RID: 2289
	// (get) Token: 0x0600101D RID: 4125 RVA: 0x0002ED52 File Offset: 0x0002CF52
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600101E RID: 4126 RVA: 0x0002ED5C File Offset: 0x0002CF5C
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		if (this.m_endAimIndicatorPivot)
		{
			this.m_endAimIndicatorPivot.transform.GetChild(0).transform.localPosition = new Vector3(this.m_aimLineLength + 0.5f, 0f, 0f);
		}
		base.Initialize(abilityController, castAbilityType);
	}

	// Token: 0x0600101F RID: 4127 RVA: 0x0002EDB4 File Offset: 0x0002CFB4
	protected override void FireProjectile()
	{
		Vector2 vector = CDGHelper.AngleToVector(-this.m_aimAngle);
		if (this.m_abilityController.PlayerController.IsFacingRight)
		{
			vector.x = -vector.x;
		}
		float aimAngle = this.m_aimAngle;
		CDGHelper.RotatedPoint(new Vector2(this.ProjectileOffset.x, 0f), Vector2.zero, aimAngle).y += this.ProjectileOffset.y;
		base.FireProjectile();
		if (this.m_firedProjectile.transform.localScale.x < 0f)
		{
			this.m_firedProjectile.Flip();
		}
	}

	// Token: 0x06001020 RID: 4128 RVA: 0x0002EE59 File Offset: 0x0002D059
	protected override void Update()
	{
		base.Update();
		if (this.m_isAiming && this.m_endAimIndicatorPivot)
		{
			this.m_endAimIndicatorPivot.transform.SetLocalEulerZ(this.m_unmoddedAngle);
		}
	}

	// Token: 0x040011A7 RID: 4519
	[SerializeField]
	private GameObject m_endAimIndicatorPivot;
}

using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x0200017C RID: 380
[Obsolete("Currently Using CloakStrikeNew instead.")]
public class CloakStrike_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x1700073D RID: 1853
	// (get) Token: 0x06000D5D RID: 3421 RVA: 0x00028B0B File Offset: 0x00026D0B
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700073E RID: 1854
	// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00028B12 File Offset: 0x00026D12
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700073F RID: 1855
	// (get) Token: 0x06000D5F RID: 3423 RVA: 0x00028B19 File Offset: 0x00026D19
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000740 RID: 1856
	// (get) Token: 0x06000D60 RID: 3424 RVA: 0x00028B20 File Offset: 0x00026D20
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x17000741 RID: 1857
	// (get) Token: 0x06000D61 RID: 3425 RVA: 0x00028B27 File Offset: 0x00026D27
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000742 RID: 1858
	// (get) Token: 0x06000D62 RID: 3426 RVA: 0x00028B2E File Offset: 0x00026D2E
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000743 RID: 1859
	// (get) Token: 0x06000D63 RID: 3427 RVA: 0x00028B35 File Offset: 0x00026D35
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000744 RID: 1860
	// (get) Token: 0x06000D64 RID: 3428 RVA: 0x00028B3C File Offset: 0x00026D3C
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000D65 RID: 3429 RVA: 0x00028B43 File Offset: 0x00026D43
	public override IEnumerator CastAbility()
	{
		PlayerController playerController = this.m_abilityController.PlayerController;
		playerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
		playerController.ControllerCorgi.State.IsCollidingBelow = false;
		playerController.ControllerCorgi.State.IsFalling = true;
		playerController.CharacterJump.ResetBrakeForce();
		while (!base.IsAnimationComplete && !playerController.IsGrounded)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000D66 RID: 3430 RVA: 0x00028B54 File Offset: 0x00026D54
	protected override void OnEnterTellIntroLogic()
	{
		PlayerController playerController = this.m_abilityController.PlayerController;
		if (playerController.IsFacingRight)
		{
			playerController.SetVelocity(Ability_EV.CLOAKSTRIKE_INITIAL_VELOCITY.x, Ability_EV.CLOAKSTRIKE_INITIAL_VELOCITY.y, false);
		}
		else
		{
			playerController.SetVelocity(-Ability_EV.CLOAKSTRIKE_INITIAL_VELOCITY.x, Ability_EV.CLOAKSTRIKE_INITIAL_VELOCITY.y, false);
		}
		base.OnEnterTellIntroLogic();
	}

	// Token: 0x06000D67 RID: 3431 RVA: 0x00028BB4 File Offset: 0x00026DB4
	protected override void OnEnterAttackLogic()
	{
		PlayerController playerController = this.m_abilityController.PlayerController;
		playerController.ControllerCorgi.GravityActive(false);
		if (playerController.IsFacingRight)
		{
			playerController.SetVelocity(Ability_EV.CLOAKSTRIKE_ATTACK_VELOCITY.x, Ability_EV.CLOAKSTRIKE_ATTACK_VELOCITY.y, false);
		}
		else
		{
			playerController.SetVelocity(-Ability_EV.CLOAKSTRIKE_ATTACK_VELOCITY.x, Ability_EV.CLOAKSTRIKE_ATTACK_VELOCITY.y, false);
		}
		base.OnEnterAttackLogic();
	}

	// Token: 0x06000D68 RID: 3432 RVA: 0x00028C20 File Offset: 0x00026E20
	protected override void FireProjectile()
	{
		base.FireProjectile();
		this.m_firedProjectile.OnCollisionRelay.AddListener(new Action<Projectile_RL, GameObject>(this.Ricochet), false);
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x00028C48 File Offset: 0x00026E48
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(true);
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		this.m_abilityController.Animator.SetTrigger("Cancel_Ability_Anim");
		if (this.m_firedProjectile != null)
		{
			this.m_firedProjectile.OnCollisionRelay.RemoveListener(new Action<Projectile_RL, GameObject>(this.Ricochet));
			this.m_firedProjectile.FlagForDestruction(null);
			this.m_firedProjectile = null;
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x00028CE9 File Offset: 0x00026EE9
	public void Ricochet(Projectile_RL projectile, GameObject colliderObj)
	{
		this.StopAbility(true);
	}
}

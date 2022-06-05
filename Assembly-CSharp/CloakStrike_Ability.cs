using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020002BE RID: 702
[Obsolete("Currently Using CloakStrikeNew instead.")]
public class CloakStrike_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x170009AD RID: 2477
	// (get) Token: 0x060014B4 RID: 5300 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170009AE RID: 2478
	// (get) Token: 0x060014B5 RID: 5301 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009AF RID: 2479
	// (get) Token: 0x060014B6 RID: 5302 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170009B0 RID: 2480
	// (get) Token: 0x060014B7 RID: 5303 RVA: 0x0000456C File Offset: 0x0000276C
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x170009B1 RID: 2481
	// (get) Token: 0x060014B8 RID: 5304 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170009B2 RID: 2482
	// (get) Token: 0x060014B9 RID: 5305 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009B3 RID: 2483
	// (get) Token: 0x060014BA RID: 5306 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170009B4 RID: 2484
	// (get) Token: 0x060014BB RID: 5307 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060014BC RID: 5308 RVA: 0x0000A60A File Offset: 0x0000880A
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

	// Token: 0x060014BD RID: 5309 RVA: 0x00087B80 File Offset: 0x00085D80
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

	// Token: 0x060014BE RID: 5310 RVA: 0x00087BE0 File Offset: 0x00085DE0
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

	// Token: 0x060014BF RID: 5311 RVA: 0x0000A619 File Offset: 0x00008819
	protected override void FireProjectile()
	{
		base.FireProjectile();
		this.m_firedProjectile.OnCollisionRelay.AddListener(new Action<Projectile_RL, GameObject>(this.Ricochet), false);
	}

	// Token: 0x060014C0 RID: 5312 RVA: 0x00087C4C File Offset: 0x00085E4C
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

	// Token: 0x060014C1 RID: 5313 RVA: 0x0000A63F File Offset: 0x0000883F
	public void Ricochet(Projectile_RL projectile, GameObject colliderObj)
	{
		this.StopAbility(true);
	}
}

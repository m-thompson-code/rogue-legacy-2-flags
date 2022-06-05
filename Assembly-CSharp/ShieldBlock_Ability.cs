using System;
using System.Collections;
using FMODUnity;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x02000188 RID: 392
public class ShieldBlock_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x06000E03 RID: 3587 RVA: 0x0002B023 File Offset: 0x00029223
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			"ShieldBlockPerfectTalentProjectile"
		};
	}

	// Token: 0x17000789 RID: 1929
	// (get) Token: 0x06000E04 RID: 3588 RVA: 0x0002B042 File Offset: 0x00029242
	public override string ProjectileName
	{
		get
		{
			if (this.m_isPerfectBlock)
			{
				return "ShieldBlockPerfectTalentProjectile";
			}
			return base.ProjectileName;
		}
	}

	// Token: 0x06000E05 RID: 3589 RVA: 0x0002B058 File Offset: 0x00029258
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerBlocked = new Action<MonoBehaviour, EventArgs>(this.OnPlayerBlocked);
	}

	// Token: 0x06000E06 RID: 3590 RVA: 0x0002B072 File Offset: 0x00029272
	public override void PreCastAbility()
	{
		base.PreCastAbility();
		this.m_raiseShieldEmitter.Play();
	}

	// Token: 0x06000E07 RID: 3591 RVA: 0x0002B085 File Offset: 0x00029285
	protected override void OnEnterExitLogic()
	{
		base.OnEnterExitLogic();
		this.m_lowerShieldEmitter.Play();
	}

	// Token: 0x06000E08 RID: 3592 RVA: 0x0002B098 File Offset: 0x00029298
	public override IEnumerator CastAbility()
	{
		this.m_stopAbilityFlag = false;
		this.m_abilityController.PlayerController.IsBlocking = true;
		this.m_abilityController.PlayerController.BaseStunDefense += 100f;
		this.m_abilityController.PlayerController.BaseKnockbackDefense += 100f;
		this.m_abilityController.PlayerController.BlockStartTime = Time.time;
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerBlocked, this.m_onPlayerBlocked);
		this.m_animator.ResetTrigger("Shield_Counterattack");
		yield return base.CastAbility();
		yield break;
	}

	// Token: 0x06000E09 RID: 3593 RVA: 0x0002B0A8 File Offset: 0x000292A8
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_abilityController.PlayerController.IsBlocking)
		{
			this.m_abilityController.PlayerController.DisableFriction = false;
		}
		this.m_abilityController.PlayerController.IsBlocking = false;
		this.m_abilityController.PlayerController.BaseStunDefense -= 100f;
		this.m_abilityController.PlayerController.BaseKnockbackDefense -= 100f;
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerBlocked, this.m_onPlayerBlocked);
		if (this.m_abilityController.PlayerController.ConditionState == CharacterStates.CharacterConditions.DisableHorizontalMovement)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(true);
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x0002B170 File Offset: 0x00029370
	private void OnPlayerBlocked(MonoBehaviour sender, EventArgs args)
	{
		if (this.m_abilityController.PlayerController.IsDead)
		{
			return;
		}
		PlayerBlockedEventArgs playerBlockedEventArgs = args as PlayerBlockedEventArgs;
		if ((this.m_abilityController.PlayerController.IsFacingRight && playerBlockedEventArgs.DamageObj.gameObject.transform.position.x < this.m_abilityController.PlayerController.transform.position.x) || (!this.m_abilityController.PlayerController.IsFacingRight && playerBlockedEventArgs.DamageObj.gameObject.transform.position.x > this.m_abilityController.PlayerController.transform.position.x))
		{
			this.m_abilityController.PlayerController.CharacterCorgi.Flip(false, false);
		}
		this.m_isPerfectBlock = (Time.time < this.m_abilityController.PlayerController.BlockStartTime + 0.135f);
		if (this.m_isPerfectBlock)
		{
			this.m_perfectBlockEmitter.Play();
		}
		else
		{
			this.m_blockEmitter.Play();
		}
		base.Invoke("FireProjectile", 0.15f);
		this.StartCooldownTimer();
		this.m_animator.SetTrigger("Shield_Counterattack");
		this.m_abilityController.PlayerController.CharacterHitResponse.SetInvincibleTime(1.25f, false, true);
		EffectManager.SetEffectParams("SlowTime_Effect", new object[]
		{
			"m_timeScaleValue",
			0.1f
		});
		EffectManager.PlayEffect(base.gameObject, this.m_abilityController.PlayerController.Animator, "SlowTime_Effect", Vector3.zero, 0.25f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
		this.m_abilityController.PlayerController.DisableFriction = true;
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(true);
		if (this.m_abilityController.PlayerController.IsFacingRight)
		{
			this.m_abilityController.PlayerController.SetVelocity(-Ability_EV.SHIELD_BLOCK_KNOCKBACK_ADD.x, Ability_EV.SHIELD_BLOCK_KNOCKBACK_ADD.y, false);
		}
		else
		{
			this.m_abilityController.PlayerController.SetVelocity(Ability_EV.SHIELD_BLOCK_KNOCKBACK_ADD.x, Ability_EV.SHIELD_BLOCK_KNOCKBACK_ADD.y, false);
		}
		this.m_stopAbilityFlag = true;
	}

	// Token: 0x06000E0B RID: 3595 RVA: 0x0002B3B3 File Offset: 0x000295B3
	protected override void FireProjectile()
	{
		base.FireProjectile();
		this.m_firedProjectile = null;
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x0002B3C2 File Offset: 0x000295C2
	protected override IEnumerator ChangeAnim(float duration)
	{
		if (base.CurrentAbilityAnimState == AbilityAnimState.Tell)
		{
			while (!this.m_stopAbilityFlag && (Rewired_RL.Player.GetButtonDown(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)) || Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType))))
			{
				yield return null;
			}
		}
		yield return base.ChangeAnim(duration);
		yield break;
	}

	// Token: 0x04001106 RID: 4358
	[SerializeField]
	private StudioEventEmitter m_raiseShieldEmitter;

	// Token: 0x04001107 RID: 4359
	[SerializeField]
	private StudioEventEmitter m_lowerShieldEmitter;

	// Token: 0x04001108 RID: 4360
	[SerializeField]
	private StudioEventEmitter m_blockEmitter;

	// Token: 0x04001109 RID: 4361
	[SerializeField]
	private StudioEventEmitter m_perfectBlockEmitter;

	// Token: 0x0400110A RID: 4362
	private bool m_stopAbilityFlag;

	// Token: 0x0400110B RID: 4363
	private bool m_isPerfectBlock;

	// Token: 0x0400110C RID: 4364
	private WaitRL_Yield m_fireProjectileWaitYield;

	// Token: 0x0400110D RID: 4365
	private Action<MonoBehaviour, EventArgs> m_onPlayerBlocked;
}

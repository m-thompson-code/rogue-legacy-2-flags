using System;
using System.Collections;
using FMODUnity;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020002D3 RID: 723
public class ShieldBlock_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x06001590 RID: 5520 RVA: 0x0000AAF8 File Offset: 0x00008CF8
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			"ShieldBlockPerfectTalentProjectile"
		};
	}

	// Token: 0x17000A0B RID: 2571
	// (get) Token: 0x06001591 RID: 5521 RVA: 0x0000AB17 File Offset: 0x00008D17
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

	// Token: 0x06001592 RID: 5522 RVA: 0x0000AB2D File Offset: 0x00008D2D
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerBlocked = new Action<MonoBehaviour, EventArgs>(this.OnPlayerBlocked);
	}

	// Token: 0x06001593 RID: 5523 RVA: 0x0000AB47 File Offset: 0x00008D47
	public override void PreCastAbility()
	{
		base.PreCastAbility();
		this.m_raiseShieldEmitter.Play();
	}

	// Token: 0x06001594 RID: 5524 RVA: 0x0000AB5A File Offset: 0x00008D5A
	protected override void OnEnterExitLogic()
	{
		base.OnEnterExitLogic();
		this.m_lowerShieldEmitter.Play();
	}

	// Token: 0x06001595 RID: 5525 RVA: 0x0000AB6D File Offset: 0x00008D6D
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

	// Token: 0x06001596 RID: 5526 RVA: 0x0008A238 File Offset: 0x00088438
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

	// Token: 0x06001597 RID: 5527 RVA: 0x0008A300 File Offset: 0x00088500
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

	// Token: 0x06001598 RID: 5528 RVA: 0x0000AB7C File Offset: 0x00008D7C
	protected override void FireProjectile()
	{
		base.FireProjectile();
		this.m_firedProjectile = null;
	}

	// Token: 0x06001599 RID: 5529 RVA: 0x0000AB8B File Offset: 0x00008D8B
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

	// Token: 0x04001682 RID: 5762
	[SerializeField]
	private StudioEventEmitter m_raiseShieldEmitter;

	// Token: 0x04001683 RID: 5763
	[SerializeField]
	private StudioEventEmitter m_lowerShieldEmitter;

	// Token: 0x04001684 RID: 5764
	[SerializeField]
	private StudioEventEmitter m_blockEmitter;

	// Token: 0x04001685 RID: 5765
	[SerializeField]
	private StudioEventEmitter m_perfectBlockEmitter;

	// Token: 0x04001686 RID: 5766
	private bool m_stopAbilityFlag;

	// Token: 0x04001687 RID: 5767
	private bool m_isPerfectBlock;

	// Token: 0x04001688 RID: 5768
	private WaitRL_Yield m_fireProjectileWaitYield;

	// Token: 0x04001689 RID: 5769
	private Action<MonoBehaviour, EventArgs> m_onPlayerBlocked;
}

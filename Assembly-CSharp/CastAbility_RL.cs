using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000041 RID: 65
[RequireComponent(typeof(CharacterClass))]
public class CastAbility_RL : CharacterAbility
{
	// Token: 0x17000006 RID: 6
	// (get) Token: 0x060000B5 RID: 181 RVA: 0x0000717F File Offset: 0x0000537F
	// (set) Token: 0x060000B6 RID: 182 RVA: 0x00007187 File Offset: 0x00005387
	public bool OnPauseResetInput { get; private set; }

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x060000B7 RID: 183 RVA: 0x00007190 File Offset: 0x00005390
	// (set) Token: 0x060000B8 RID: 184 RVA: 0x00007198 File Offset: 0x00005398
	public AbilityType WeaponAbilityType { get; private set; }

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x060000B9 RID: 185 RVA: 0x000071A1 File Offset: 0x000053A1
	// (set) Token: 0x060000BA RID: 186 RVA: 0x000071A9 File Offset: 0x000053A9
	public AbilityType SpellAbilityType { get; private set; }

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x060000BB RID: 187 RVA: 0x000071B2 File Offset: 0x000053B2
	// (set) Token: 0x060000BC RID: 188 RVA: 0x000071BA File Offset: 0x000053BA
	public AbilityType TalentAbilityType { get; private set; }

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x060000BD RID: 189 RVA: 0x000071C3 File Offset: 0x000053C3
	public float HorizontalInput
	{
		get
		{
			return this._horizontalInput;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x060000BE RID: 190 RVA: 0x000071CC File Offset: 0x000053CC
	public bool IsAiming
	{
		get
		{
			bool flag = this.m_weaponAbility && this.m_weaponAbility.IsAiming;
			bool flag2 = this.m_spellAbility && this.m_spellAbility.IsAiming;
			bool flag3 = this.m_talentAbility && this.m_talentAbility.IsAiming;
			return flag || flag2 || flag3;
		}
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00007230 File Offset: 0x00005430
	public string GetAbilityInputString(CastAbilityType castAbilityType)
	{
		switch (castAbilityType)
		{
		default:
			return "Attack";
		case CastAbilityType.Spell:
			return "Spell";
		case CastAbilityType.Talent:
			return "Talent";
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x060000C0 RID: 192 RVA: 0x00007255 File Offset: 0x00005455
	// (set) Token: 0x060000C1 RID: 193 RVA: 0x0000725D File Offset: 0x0000545D
	public CastAbilityType LastCastAbilityTypeCasted { get; private set; }

	// Token: 0x060000C2 RID: 194 RVA: 0x00007266 File Offset: 0x00005466
	public void SetLastCastAbilityTypeOverride(CastAbilityType castAbilityTypeOverride)
	{
		this.LastCastAbilityTypeCasted = castAbilityTypeOverride;
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x060000C3 RID: 195 RVA: 0x0000726F File Offset: 0x0000546F
	public global::PlayerController PlayerController
	{
		get
		{
			return this.m_playerController;
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x060000C4 RID: 196 RVA: 0x00007278 File Offset: 0x00005478
	public bool ActiveAbilitiesAreJumpBreakable
	{
		get
		{
			BaseAbility_RL ability = this.GetAbility(CastAbilityType.Weapon, false);
			if (ability && this.AbilityInProgress(CastAbilityType.Weapon) && !ability.JumpInterruptable)
			{
				return false;
			}
			BaseAbility_RL ability2 = this.GetAbility(CastAbilityType.Spell, false);
			if (ability2 && this.AbilityInProgress(CastAbilityType.Spell) && !ability2.JumpInterruptable)
			{
				return false;
			}
			BaseAbility_RL ability3 = this.GetAbility(CastAbilityType.Talent, false);
			return !ability3 || !this.AbilityInProgress(CastAbilityType.Talent) || ability3.JumpInterruptable;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x060000C5 RID: 197 RVA: 0x000072F2 File Offset: 0x000054F2
	public bool AnyAbilityInProgress
	{
		get
		{
			return this.AbilityInProgress(CastAbilityType.Weapon) || this.AbilityInProgress(CastAbilityType.Spell) || this.AbilityInProgress(CastAbilityType.Talent);
		}
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x0000730F File Offset: 0x0000550F
	public bool AbilityInProgress(CastAbilityType castAbilityType)
	{
		switch (castAbilityType)
		{
		case CastAbilityType.Weapon:
			return this.m_weaponAbilityInProgress;
		case CastAbilityType.Spell:
			return this.m_spellAbilityInProgress;
		case CastAbilityType.Talent:
			return this.m_talentAbilityInProgress;
		default:
			return false;
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x060000C7 RID: 199 RVA: 0x0000733B File Offset: 0x0000553B
	public Animator Animator
	{
		get
		{
			return this._animator;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x060000C8 RID: 200 RVA: 0x00007343 File Offset: 0x00005543
	public bool IsInitialized
	{
		get
		{
			return base.AbilityInitialized;
		}
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x0000734C File Offset: 0x0000554C
	public BaseAbility_RL GetAbility(CastAbilityType abilityType, bool ignoreOverrides = false)
	{
		switch (abilityType)
		{
		case CastAbilityType.Weapon:
			if (this.m_isWeaponAbilityOverridden && !ignoreOverrides)
			{
				return this.m_weaponAbilityOverride;
			}
			return this.m_weaponAbility;
		case CastAbilityType.Spell:
			return this.m_spellAbility;
		case CastAbilityType.Talent:
			return this.m_talentAbility;
		default:
			Debug.LogFormat("<color=red>{0}: No Ability found matching Ability Type ({1})</color>", new object[]
			{
				Time.frameCount,
				abilityType
			});
			return null;
		}
	}

	// Token: 0x060000CA RID: 202 RVA: 0x000073BC File Offset: 0x000055BC
	public void SetAbility(CastAbilityType castAbilityType, AbilityType abilityType, bool destroyOldAbility = true)
	{
		BaseAbility_RL abilityInstance = this.CreateAbilityInstance(castAbilityType, abilityType);
		this.SetAbility(castAbilityType, abilityInstance, destroyOldAbility);
	}

	// Token: 0x060000CB RID: 203 RVA: 0x000073DC File Offset: 0x000055DC
	public BaseAbility_RL CreateAbilityInstance(CastAbilityType castAbilityType, AbilityType abilityType)
	{
		BaseAbility_RL ability = AbilityLibrary.GetAbility(abilityType);
		BaseAbility_RL baseAbility_RL = null;
		if (ability)
		{
			baseAbility_RL = UnityEngine.Object.Instantiate<BaseAbility_RL>(ability);
			baseAbility_RL.Initialize(this, castAbilityType);
			baseAbility_RL.transform.position = Vector3.zero;
			baseAbility_RL.transform.SetParent(base.transform, false);
		}
		return baseAbility_RL;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x0000742C File Offset: 0x0000562C
	public void SetAbility(CastAbilityType castAbilityType, BaseAbility_RL abilityInstance, bool destroyOldAbility = true)
	{
		switch (castAbilityType)
		{
		case CastAbilityType.Weapon:
			if (this.m_weaponAbility && destroyOldAbility)
			{
				foreach (BaseEffect baseEffect in this.m_weaponAbility.GetComponentsInChildren<BaseEffect>(true))
				{
					baseEffect.transform.SetParent(null);
					baseEffect.gameObject.SetActive(false);
				}
				this.m_weaponAbility.OnPreDestroy();
				UnityEngine.Object.Destroy(this.m_weaponAbility.gameObject);
			}
			this.m_weaponAbility = abilityInstance;
			if (this.m_weaponAbility)
			{
				this.WeaponAbilityType = abilityInstance.AbilityType;
			}
			else
			{
				this.WeaponAbilityType = AbilityType.None;
			}
			break;
		case CastAbilityType.Spell:
			if (this.m_spellAbility && destroyOldAbility)
			{
				foreach (BaseEffect baseEffect2 in this.m_spellAbility.GetComponentsInChildren<BaseEffect>(true))
				{
					baseEffect2.transform.SetParent(null);
					baseEffect2.gameObject.SetActive(false);
				}
				this.m_spellAbility.OnPreDestroy();
				UnityEngine.Object.Destroy(this.m_spellAbility.gameObject);
			}
			this.m_spellAbility = abilityInstance;
			if (this.m_spellAbility)
			{
				this.SpellAbilityType = abilityInstance.AbilityType;
			}
			else
			{
				this.SpellAbilityType = AbilityType.None;
			}
			break;
		case CastAbilityType.Talent:
			if (this.m_talentAbility && destroyOldAbility)
			{
				foreach (BaseEffect baseEffect3 in this.m_talentAbility.GetComponentsInChildren<BaseEffect>(true))
				{
					baseEffect3.transform.SetParent(null);
					baseEffect3.gameObject.SetActive(false);
				}
				this.m_talentAbility.OnPreDestroy();
				UnityEngine.Object.Destroy(this.m_talentAbility.gameObject);
			}
			this.m_talentAbility = abilityInstance;
			if (this.m_talentAbility)
			{
				this.TalentAbilityType = abilityInstance.AbilityType;
			}
			else
			{
				this.TalentAbilityType = AbilityType.None;
			}
			break;
		}
		this.m_changeAbilityArgs.Initialise(castAbilityType, abilityInstance);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.ChangeAbility, this, this.m_changeAbilityArgs);
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00007618 File Offset: 0x00005818
	public void ReinitializeAbility(CastAbilityType castAbilityType)
	{
		switch (castAbilityType)
		{
		case CastAbilityType.Weapon:
			if (this.m_weaponAbility)
			{
				this.m_weaponAbility.Reinitialize();
				return;
			}
			break;
		case CastAbilityType.Spell:
			if (this.m_spellAbility)
			{
				this.m_spellAbility.Reinitialize();
				return;
			}
			break;
		case CastAbilityType.Talent:
			if (this.m_talentAbility)
			{
				this.m_talentAbility.Reinitialize();
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00007684 File Offset: 0x00005884
	private void Awake()
	{
		this.m_changeAbilityArgs = new ChangeAbilityEventArgs(CastAbilityType.Weapon, null);
		this.m_abilityUsedEventArgs = new AbilityUsedEventArgs(null);
		this.m_damageProjectileHelper = UnityEngine.Object.Instantiate<Projectile_RL>(ProjectileLibrary.GetProjectile("SwordWeaponProjectile"), base.transform);
		this.m_damageProjectileHelper.name = "Damage Projectile Helper";
		this.m_damageProjectileHelper.gameObject.SetActive(false);
		ProjectileManager.ActiveProjectileCount++;
		this.m_onGamePaused = new Action<MonoBehaviour, EventArgs>(this.OnGamePaused);
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00007704 File Offset: 0x00005904
	protected override void HandleInput()
	{
		if (this.m_playerController.ConditionState == CharacterStates.CharacterConditions.ControlledMovement)
		{
			return;
		}
		if (this.m_weaponAbility && this._character.REPlayer.GetButtonDown(this.GetAbilityInputString(CastAbilityType.Weapon)))
		{
			this.StartAbility(CastAbilityType.Weapon, false, true);
		}
		if (this.m_spellAbility && this._character.REPlayer.GetButtonDown(this.GetAbilityInputString(CastAbilityType.Spell)))
		{
			this.StartAbility(CastAbilityType.Spell, false, true);
		}
		if (this.m_talentAbility && this._character.REPlayer.GetButtonDown(this.GetAbilityInputString(CastAbilityType.Talent)))
		{
			this.StartAbility(CastAbilityType.Talent, false, true);
		}
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x000077B0 File Offset: 0x000059B0
	public bool IsAbilityPermitted(CastAbilityType abilityType)
	{
		BaseAbility_RL ability = this.GetAbility(abilityType, false);
		return this.IsAbilityPermitted(ability);
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x000077D0 File Offset: 0x000059D0
	public bool IsAbilityPermitted(BaseAbility_RL ability)
	{
		if (!this.AbilityPermitted)
		{
			return false;
		}
		Vector2 vector = this.m_playerController.Midpoint;
		vector.y += this.m_playerController.CollisionBounds.height / 2f;
		bool flag = false;
		if (Time.time >= this.m_textPopupDelay || this.m_lastCastAbilityPermittedCheck != ability.CastAbilityType)
		{
			flag = true;
			this.m_textPopupDelay = Time.time + 1f;
			this.m_lastCastAbilityPermittedCheck = ability.CastAbilityType;
		}
		if (this.m_playerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_Disarmed))
		{
			if (this.m_inCooldownUnityEvent != null)
			{
				this.m_inCooldownUnityEvent.Invoke();
			}
			if (flag)
			{
				TextPopupManager.DisplayTextAtAbsPos(TextPopupType.OutOfAmmo, LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_ABILITY_DISABLED_1", false, false), vector, null, TextAlignmentOptions.Center);
			}
			return false;
		}
		if (this.m_playerController.CharacterDownStrike.AttackButtonTriggersSpinKick && !this._controller.State.IsGrounded && this.m_playerController.CharacterDownStrike.IsHoldingDownStrikeAngle)
		{
			return false;
		}
		if ((this.m_playerController.ConditionState != CharacterStates.CharacterConditions.Normal && this.m_playerController.ConditionState != CharacterStates.CharacterConditions.DisableHorizontalMovement) || this.m_playerController.MovementState == CharacterStates.MovementStates.DownStriking)
		{
			return false;
		}
		if (!ability)
		{
			return false;
		}
		if (TraitManager.IsTraitActive(TraitType.CantAttack) && !ability.DealsNoDamage)
		{
			return false;
		}
		if (this.m_playerController.MovementState == CharacterStates.MovementStates.Dashing && !ability.CanCastWhileDashing)
		{
			return false;
		}
		if (ability.LockoutTimer > 0f)
		{
			return false;
		}
		BaseAbility_RL ability2 = this.GetAbility(CastAbilityType.Spell, false);
		BaseAbility_RL ability3 = this.GetAbility(CastAbilityType.Weapon, false);
		BaseAbility_RL ability4 = this.GetAbility(CastAbilityType.Talent, false);
		switch (ability.EVAssignedCastAbilityType)
		{
		case CastAbilityType.Weapon:
			if (this.m_weaponAbilityInProgress && ability3 && !ability3.WeaponInterruptable)
			{
				return false;
			}
			if (this.m_spellAbilityInProgress && ability2 && !ability2.WeaponInterruptable)
			{
				return false;
			}
			if (this.m_talentAbilityInProgress && ability4 && !ability4.WeaponInterruptable)
			{
				return false;
			}
			break;
		case CastAbilityType.Spell:
			if (this.m_weaponAbilityInProgress && ability3 && !ability3.SpellInterruptable)
			{
				return false;
			}
			if (this.m_spellAbilityInProgress && ability2 && !ability2.SpellInterruptable)
			{
				return false;
			}
			if (this.m_talentAbilityInProgress && ability4 && !ability4.SpellInterruptable)
			{
				return false;
			}
			break;
		case CastAbilityType.Talent:
			if (this.m_weaponAbilityInProgress && ability3 && !ability3.TalentInterruptable)
			{
				return false;
			}
			if (this.m_spellAbilityInProgress && ability2 && !ability2.TalentInterruptable)
			{
				return false;
			}
			if (this.m_talentAbilityInProgress && ability4 && !ability4.TalentInterruptable)
			{
				return false;
			}
			break;
		}
		if (ability.CooldownTimer > 0f && ability.MaxAmmo <= 0)
		{
			if (this.m_inCooldownUnityEvent != null)
			{
				this.m_inCooldownUnityEvent.Invoke();
			}
			if (ability.AbilityType == AbilityType.CrowsNestTalent && (ability as CrowsNest_Ability).CrowsNestActive)
			{
				flag = false;
			}
			if (flag)
			{
				TextPopupManager.DisplayLocIDText(TextPopupType.SpellOnCooldown, "LOC_ID_STATUS_EFFECT_NO_COOLDOWN_1", StringGenderType.UsePlayerData, vector, 0.5f);
			}
			return false;
		}
		if (ability.ActualCost > this.m_playerController.CurrentManaAsInt)
		{
			IPersistentAbility persistentAbility = ability as IPersistentAbility;
			if (persistentAbility == null || !persistentAbility.IsPersistentActive)
			{
				bool flag2 = false;
				RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.FreeCastSpell);
				if (relic.Level > 0)
				{
					int relicMaxStack = Relic_EV.GetRelicMaxStack(relic.RelicType, relic.Level);
					if (relic.IntValue >= relicMaxStack)
					{
						flag2 = true;
					}
				}
				if (!flag2)
				{
					if (this.m_notEnoughManaUnityEvent != null)
					{
						this.m_notEnoughManaUnityEvent.Invoke();
					}
					if (flag)
					{
						TextPopupManager.DisplayLocIDText(TextPopupType.NoMana, "LOC_ID_STATUS_EFFECT_NO_MANA_1", StringGenderType.UsePlayerData, vector, 0.5f);
					}
					return false;
				}
			}
		}
		if (ability.MaxAmmo > 0 && ability.CurrentAmmo <= 0)
		{
			IPersistentAbility persistentAbility2 = ability as IPersistentAbility;
			if (persistentAbility2 == null || !persistentAbility2.IsPersistentActive)
			{
				if (this.m_inCooldownUnityEvent != null)
				{
					this.m_inCooldownUnityEvent.Invoke();
				}
				if (flag)
				{
					TextPopupManager.DisplayLocIDText(TextPopupType.OutOfAmmo, "LOC_ID_STATUS_EFFECT_NO_AMMO_KICK_1", StringGenderType.UsePlayerData, vector, 0.5f);
				}
				return false;
			}
		}
		return true;
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00007BD0 File Offset: 0x00005DD0
	public void StartQueueCoroutine(CastAbilityType castAbilityType)
	{
		this.StopQueueCoroutine(castAbilityType);
		switch (castAbilityType)
		{
		case CastAbilityType.Weapon:
			this.m_weaponQueueCoroutine = base.StartCoroutine(this.AbilityQueueCoroutine(castAbilityType));
			return;
		case CastAbilityType.Spell:
			this.m_spellQueueCoroutine = base.StartCoroutine(this.AbilityQueueCoroutine(castAbilityType));
			return;
		case CastAbilityType.Talent:
			this.m_talentQueueCoroutine = base.StartCoroutine(this.AbilityQueueCoroutine(castAbilityType));
			return;
		default:
			return;
		}
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00007C32 File Offset: 0x00005E32
	private IEnumerator AbilityQueueCoroutine(CastAbilityType castAbilityType)
	{
		float duration = Time.time + 0.175f;
		while (Time.time < duration)
		{
			if (this.IsAbilityPermitted(castAbilityType))
			{
				this.StartAbility(castAbilityType, false, true);
				yield break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x00007C48 File Offset: 0x00005E48
	public void StopQueueCoroutine(CastAbilityType castAbilityType)
	{
		switch (castAbilityType)
		{
		case CastAbilityType.Weapon:
			if (this.m_weaponQueueCoroutine != null)
			{
				base.StopCoroutine(this.m_weaponQueueCoroutine);
				return;
			}
			break;
		case CastAbilityType.Spell:
			if (this.m_spellQueueCoroutine != null)
			{
				base.StopCoroutine(this.m_spellQueueCoroutine);
				return;
			}
			break;
		case CastAbilityType.Talent:
			if (this.m_talentQueueCoroutine != null)
			{
				base.StopCoroutine(this.m_talentQueueCoroutine);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x00007CA6 File Offset: 0x00005EA6
	public void StopAllQueueCoroutines()
	{
		this.StopQueueCoroutine(CastAbilityType.Weapon);
		this.StopQueueCoroutine(CastAbilityType.Spell);
		this.StopQueueCoroutine(CastAbilityType.Talent);
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00007CBD File Offset: 0x00005EBD
	private void FixedUpdate()
	{
		this.PerformAbilityStuckCheck();
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00007CC8 File Offset: 0x00005EC8
	private void PerformAbilityStuckCheck()
	{
		if (this.m_performingStuckCheck)
		{
			if (Time.time > this.m_stuckCheckDelayTimer)
			{
				if (this.Animator.GetCurrentAnimatorStateInfo(1).IsName("Empty"))
				{
					if (this.m_weaponAbilityInProgress && !this.m_weaponAbility.IgnoreStuckCheck)
					{
						this.StopAbility(CastAbilityType.Weapon, false);
					}
					if (this.m_spellAbilityInProgress && !this.m_spellAbility.IgnoreStuckCheck)
					{
						this.StopAbility(CastAbilityType.Spell, false);
					}
					if (this.m_talentAbilityInProgress && !this.m_talentAbility.IgnoreStuckCheck)
					{
						this.StopAbility(CastAbilityType.Talent, false);
					}
				}
				this.m_performingStuckCheck = false;
				return;
			}
		}
		else if (this.Animator.GetCurrentAnimatorStateInfo(1).IsName("Empty") && ((this.m_weaponAbilityInProgress && this.m_weaponAbility && !this.m_weaponAbility.IgnoreStuckCheck) || (this.m_spellAbilityInProgress && this.m_spellAbility && !this.m_spellAbility.IgnoreStuckCheck) || (this.m_talentAbilityInProgress && this.m_talentAbility && !this.m_talentAbility.IgnoreStuckCheck)))
		{
			this.m_stuckCheckDelayTimer = Time.time + 0.05f;
			this.m_performingStuckCheck = true;
		}
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00007E04 File Offset: 0x00006004
	public void StartAbility(CastAbilityType abilityType, bool castAbilityOverride = false, bool allowQueuing = false)
	{
		if (!this.m_isWeaponAbilityOverridden && abilityType == CastAbilityType.Weapon && this.m_weaponAbility && this.m_weaponAbility.AbilityType == AbilityType.PistolWeapon && (this.m_weaponAbility.CurrentAmmo <= 0 || castAbilityOverride))
		{
			this.m_weaponAbilityOverride = (this.m_weaponAbility as PistolWeapon_Ability).ReloadAbility;
			this.m_isWeaponAbilityOverridden = true;
			if (!this.IsAbilityPermitted(abilityType))
			{
				this.m_weaponAbilityOverride = null;
				this.m_isWeaponAbilityOverridden = false;
			}
		}
		bool isWeaponAbilityOverridden = this.m_isWeaponAbilityOverridden;
		BaseAbility_RL ability = this.GetAbility(abilityType, false);
		if (!this.IsAbilityPermitted(abilityType))
		{
			if (allowQueuing && ability && !ability.DisableAbilityQueuing)
			{
				this.StartQueueCoroutine(abilityType);
			}
			return;
		}
		this.LastCastAbilityTypeCasted = abilityType;
		if (ability.HasAnimation)
		{
			this.StopAllAbilities(false);
		}
		this.m_isWeaponAbilityOverridden = isWeaponAbilityOverridden;
		switch (abilityType)
		{
		case CastAbilityType.Weapon:
			this.m_weaponCoroutine = base.StartCoroutine(this.CastAbility(abilityType));
			return;
		case CastAbilityType.Spell:
			this.m_spellCoroutine = base.StartCoroutine(this.CastAbility(abilityType));
			return;
		case CastAbilityType.Talent:
			this.m_talentCoroutine = base.StartCoroutine(this.CastAbility(abilityType));
			return;
		default:
			return;
		}
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00007F1C File Offset: 0x0000611C
	protected IEnumerator CastAbility(CastAbilityType abilityType)
	{
		BaseAbility_RL ability = this.GetAbility(abilityType, false);
		switch (abilityType)
		{
		case CastAbilityType.Weapon:
			this.m_weaponAbilityInProgress = true;
			break;
		case CastAbilityType.Spell:
			this.m_spellAbilityInProgress = true;
			break;
		case CastAbilityType.Talent:
			this.m_talentAbilityInProgress = true;
			if (this.m_playerController.CharacterClass.ClassType == ClassType.SaberClass && this.m_talentAbility && !(this.m_talentAbility is AimedAbility_RL))
			{
				this.m_playerController.StatusEffectController.StartStatusEffect(StatusEffectType.Player_FreeCrit, 1f, null);
			}
			break;
		}
		this.OnPauseResetInput = false;
		if (SaveManager.ConfigData.ToggleMouseAttackFlip)
		{
			ControllerType lastActiveControllerType = ReInput.controllers.GetLastActiveControllerType();
			if (lastActiveControllerType == ControllerType.Mouse || lastActiveControllerType == ControllerType.Keyboard)
			{
				Vector2 vector = this.m_playerController.Midpoint;
				Vector2 screenPosition = ReInput.controllers.Mouse.screenPosition;
				Vector2 vector2 = CameraController.GameCamera.ScreenToWorldPoint(screenPosition);
				if ((vector2.x < vector.x && this.m_playerController.IsFacingRight) || (vector2.x > vector.x && !this.m_playerController.IsFacingRight))
				{
					this.m_playerController.CharacterCorgi.Flip(false, false);
				}
			}
		}
		this.m_performingStuckCheck = false;
		ability.PreCastAbility();
		yield return ability.CastAbility();
		this.StopAbility(abilityType, false);
		yield break;
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00007F34 File Offset: 0x00006134
	public void BroadcastAbilityCastEvents(CastAbilityType abilityType)
	{
		switch (abilityType)
		{
		case CastAbilityType.Weapon:
			if (this.m_isWeaponAbilityOverridden && this.m_weaponAbilityOverride)
			{
				this.m_abilityUsedEventArgs.Initialise(this.m_weaponAbilityOverride);
			}
			else
			{
				this.m_abilityUsedEventArgs.Initialise(this.m_weaponAbility);
			}
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerWeaponAbilityCast, this, this.m_abilityUsedEventArgs);
			return;
		case CastAbilityType.Spell:
			this.m_abilityUsedEventArgs.Initialise(this.m_spellAbility);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerSpellAbilityCast, this, this.m_abilityUsedEventArgs);
			return;
		case CastAbilityType.Talent:
			this.m_abilityUsedEventArgs.Initialise(this.m_talentAbility);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerTalentAbilityCast, this, this.m_abilityUsedEventArgs);
			return;
		default:
			return;
		}
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00007FDC File Offset: 0x000061DC
	public void StopAbility(CastAbilityType abilityType, bool abilityInterrupted)
	{
		BaseAbility_RL ability = this.GetAbility(abilityType, false);
		switch (abilityType)
		{
		case CastAbilityType.Weapon:
			if (this.m_weaponCoroutine != null)
			{
				base.StopCoroutine(this.m_weaponCoroutine);
			}
			this.m_weaponCoroutine = null;
			this.m_weaponAbilityInProgress = false;
			break;
		case CastAbilityType.Spell:
			if (this.m_spellCoroutine != null)
			{
				base.StopCoroutine(this.m_spellCoroutine);
			}
			this.m_spellCoroutine = null;
			this.m_spellAbilityInProgress = false;
			break;
		case CastAbilityType.Talent:
			if (this.m_playerController && this.m_playerController.CharacterClass.ClassType == ClassType.SaberClass && !abilityInterrupted && this.m_talentAbilityInProgress && this.m_talentAbility && this.m_talentAbility is AimedAbility_RL)
			{
				this.m_playerController.StatusEffectController.StartStatusEffect(StatusEffectType.Player_FreeCrit, 1f, null);
			}
			if (this.m_talentCoroutine != null)
			{
				base.StopCoroutine(this.m_talentCoroutine);
			}
			this.m_talentCoroutine = null;
			this.m_talentAbilityInProgress = false;
			break;
		}
		if (ability && ability.AbilityActive)
		{
			ability.StopAbility(abilityInterrupted);
		}
		if (abilityType == CastAbilityType.Weapon)
		{
			this.m_isWeaponAbilityOverridden = false;
		}
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00008100 File Offset: 0x00006300
	public void StopPersistentAbility(CastAbilityType abilityType)
	{
		BaseAbility_RL ability = this.GetAbility(abilityType, false);
		if (ability)
		{
			IPersistentAbility persistentAbility = ability as IPersistentAbility;
			if (persistentAbility != null && persistentAbility.IsPersistentActive)
			{
				persistentAbility.StopPersistentAbility();
			}
		}
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00008138 File Offset: 0x00006338
	public void StopAllAbilities(bool stopPersistentAbilities)
	{
		if (this.IsInitialized)
		{
			base.StopAllCoroutines();
		}
		this.StopAbility(CastAbilityType.Weapon, true);
		this.StopAbility(CastAbilityType.Spell, true);
		this.StopAbility(CastAbilityType.Talent, true);
		this.StopAllQueueCoroutines();
		if (stopPersistentAbilities)
		{
			this.StopPersistentAbility(CastAbilityType.Weapon);
			this.StopPersistentAbility(CastAbilityType.Spell);
			this.StopPersistentAbility(CastAbilityType.Talent);
		}
	}

	// Token: 0x060000DE RID: 222 RVA: 0x0000818C File Offset: 0x0000638C
	public void ResetAbilityCooldowns(CastAbilityType abilityType, bool ignoreAbilityOverride = false)
	{
		BaseAbility_RL ability = this.GetAbility(abilityType, ignoreAbilityOverride);
		if (ability)
		{
			ability.EndCooldownTimer(false);
		}
	}

	// Token: 0x060000DF RID: 223 RVA: 0x000081B4 File Offset: 0x000063B4
	public void ResetAbilityAmmo(CastAbilityType abilityType, bool ignoreAbilityOverride = false)
	{
		BaseAbility_RL ability = this.GetAbility(abilityType, ignoreAbilityOverride);
		if (ability)
		{
			for (int i = 0; i < ability.MaxAmmo; i++)
			{
				ability.RegenerateAmmo();
			}
		}
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x000081EC File Offset: 0x000063EC
	public void SetAbilityAmmo(CastAbilityType abilityType, int amount, bool ignoreAbilityOverride = false)
	{
		BaseAbility_RL ability = this.GetAbility(abilityType, ignoreAbilityOverride);
		if (ability)
		{
			if (ability.AbilityType == AbilityType.KineticBowWeapon && amount <= 0)
			{
				amount = 1;
			}
			ability.CurrentAmmo = amount;
		}
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00008224 File Offset: 0x00006424
	public void InitializeProjectile(Projectile_RL projectile)
	{
		projectile.CastAbilityType = this.LastCastAbilityTypeCasted;
		if (this.m_spellAbility)
		{
			this.m_spellAbility.InitializeProjectile(projectile);
		}
		if (this.m_weaponAbility)
		{
			this.m_weaponAbility.InitializeProjectile(projectile);
		}
		if (this.m_talentAbility)
		{
			this.m_talentAbility.InitializeProjectile(projectile);
		}
		this.m_playerController.CloakInterrupted = false;
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00008294 File Offset: 0x00006494
	public void InitializeProjectile(Projectile_RL projectile, CastAbilityType castAbilityTypeOverride)
	{
		projectile.CastAbilityType = castAbilityTypeOverride;
		if (this.m_spellAbility)
		{
			this.m_spellAbility.InitializeProjectile(projectile);
		}
		if (this.m_weaponAbility)
		{
			this.m_weaponAbility.InitializeProjectile(projectile);
		}
		if (this.m_talentAbility)
		{
			this.m_talentAbility.InitializeProjectile(projectile);
		}
		this.m_playerController.CloakInterrupted = false;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x000082FF File Offset: 0x000064FF
	protected override void OnEnable()
	{
		base.OnEnable();
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.GamePauseStateChange, this.m_onGamePaused);
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00008314 File Offset: 0x00006514
	protected override void OnDisable()
	{
		base.OnDisable();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.GamePauseStateChange, this.m_onGamePaused);
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00008329 File Offset: 0x00006529
	private void OnGamePaused(object sender, EventArgs args)
	{
		this.OnPauseResetInput = true;
	}

	// Token: 0x04000139 RID: 313
	[Header("Ability Prefabs")]
	[SerializeField]
	[ReadOnly]
	protected BaseAbility_RL m_weaponAbility;

	// Token: 0x0400013A RID: 314
	[SerializeField]
	[ReadOnly]
	protected BaseAbility_RL m_spellAbility;

	// Token: 0x0400013B RID: 315
	[SerializeField]
	[ReadOnly]
	protected BaseAbility_RL m_talentAbility;

	// Token: 0x0400013C RID: 316
	[SerializeField]
	private UnityEvent m_notEnoughManaUnityEvent;

	// Token: 0x0400013D RID: 317
	[SerializeField]
	private UnityEvent m_inCooldownUnityEvent;

	// Token: 0x0400013E RID: 318
	protected bool m_weaponAbilityInProgress;

	// Token: 0x0400013F RID: 319
	protected bool m_spellAbilityInProgress;

	// Token: 0x04000140 RID: 320
	protected bool m_talentAbilityInProgress;

	// Token: 0x04000141 RID: 321
	private bool m_isWeaponAbilityOverridden;

	// Token: 0x04000142 RID: 322
	private BaseAbility_RL m_weaponAbilityOverride;

	// Token: 0x04000143 RID: 323
	protected Coroutine m_weaponCoroutine;

	// Token: 0x04000144 RID: 324
	protected Coroutine m_spellCoroutine;

	// Token: 0x04000145 RID: 325
	protected Coroutine m_talentCoroutine;

	// Token: 0x04000146 RID: 326
	private Coroutine m_weaponQueueCoroutine;

	// Token: 0x04000147 RID: 327
	private Coroutine m_spellQueueCoroutine;

	// Token: 0x04000148 RID: 328
	private Coroutine m_talentQueueCoroutine;

	// Token: 0x04000149 RID: 329
	private AbilityUsedEventArgs m_abilityUsedEventArgs;

	// Token: 0x0400014A RID: 330
	private ChangeAbilityEventArgs m_changeAbilityArgs;

	// Token: 0x0400014B RID: 331
	private Projectile_RL m_damageProjectileHelper;

	// Token: 0x0400014C RID: 332
	private Action<MonoBehaviour, EventArgs> m_onGamePaused;

	// Token: 0x04000152 RID: 338
	private float m_textPopupDelay;

	// Token: 0x04000153 RID: 339
	private CastAbilityType m_lastCastAbilityPermittedCheck;

	// Token: 0x04000154 RID: 340
	private const string EMPTY_STATE_NAME = "Empty";

	// Token: 0x04000155 RID: 341
	private bool m_performingStuckCheck;

	// Token: 0x04000156 RID: 342
	private float m_stuckCheckDelayTimer;
}

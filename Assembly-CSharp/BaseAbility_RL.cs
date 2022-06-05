using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using Rewired;
using RLAudio;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000167 RID: 359
public abstract class BaseAbility_RL : MonoBehaviour, ICooldown, IAbility, IHasProjectileNameArray, IAudioEventEmitter
{
	// Token: 0x17000688 RID: 1672
	// (get) Token: 0x06000BFC RID: 3068 RVA: 0x00024D97 File Offset: 0x00022F97
	public virtual bool IgnoreStuckCheck
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000689 RID: 1673
	// (get) Token: 0x06000BFD RID: 3069 RVA: 0x00024D9A File Offset: 0x00022F9A
	public IRelayLink BeginCastingRelay
	{
		get
		{
			return this.m_beginCastingRelay.link;
		}
	}

	// Token: 0x1700068A RID: 1674
	// (get) Token: 0x06000BFE RID: 3070 RVA: 0x00024DA7 File Offset: 0x00022FA7
	public IRelayLink StopCastingRelay
	{
		get
		{
			return this.m_stopCastingRelay.link;
		}
	}

	// Token: 0x1700068B RID: 1675
	// (get) Token: 0x06000BFF RID: 3071 RVA: 0x00024DB4 File Offset: 0x00022FB4
	public bool DisableAbilityQueuing
	{
		get
		{
			return this.m_disableAbilityQueuing;
		}
	}

	// Token: 0x1700068C RID: 1676
	// (get) Token: 0x06000C00 RID: 3072 RVA: 0x00024DBC File Offset: 0x00022FBC
	public bool CritsWhenDashing
	{
		get
		{
			return this.m_critsWhenDashing;
		}
	}

	// Token: 0x1700068D RID: 1677
	// (get) Token: 0x06000C01 RID: 3073 RVA: 0x00024DC4 File Offset: 0x00022FC4
	// (set) Token: 0x06000C02 RID: 3074 RVA: 0x00024DCC File Offset: 0x00022FCC
	public bool ForceTriggerCrit { get; set; }

	// Token: 0x1700068E RID: 1678
	// (get) Token: 0x06000C03 RID: 3075 RVA: 0x00024DD5 File Offset: 0x00022FD5
	public IRelayLink<object, CooldownEventArgs> OnBeginCooldownRelay
	{
		get
		{
			return this.m_onCooldownRelay.link;
		}
	}

	// Token: 0x1700068F RID: 1679
	// (get) Token: 0x06000C04 RID: 3076 RVA: 0x00024DE2 File Offset: 0x00022FE2
	// (set) Token: 0x06000C05 RID: 3077 RVA: 0x00024DEA File Offset: 0x00022FEA
	public CooldownRegenType CooldownRegenType { get; protected set; }

	// Token: 0x17000690 RID: 1680
	// (get) Token: 0x06000C06 RID: 3078 RVA: 0x00024DF3 File Offset: 0x00022FF3
	public virtual bool IsAiming
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000691 RID: 1681
	// (get) Token: 0x06000C07 RID: 3079 RVA: 0x00024DF6 File Offset: 0x00022FF6
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x17000692 RID: 1682
	// (get) Token: 0x06000C08 RID: 3080 RVA: 0x00024DFE File Offset: 0x00022FFE
	public virtual string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				this.InitializeProjectileNameArray();
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x00024E14 File Offset: 0x00023014
	protected virtual void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName
		};
	}

	// Token: 0x17000693 RID: 1683
	// (get) Token: 0x06000C0A RID: 3082 RVA: 0x00024E2C File Offset: 0x0002302C
	protected virtual bool IsInCritWindow
	{
		get
		{
			return (this.m_critHitTimingWindow.x != 0f || this.m_critHitTimingWindow.y != 0f) && Time.time >= this.m_abilityCastStartTime + this.m_critHitTimingWindow.x && Time.time <= this.m_abilityCastStartTime + this.m_critHitTimingWindow.y;
		}
	}

	// Token: 0x17000694 RID: 1684
	// (get) Token: 0x06000C0B RID: 3083 RVA: 0x00024E96 File Offset: 0x00023096
	public bool DealsNoDamage
	{
		get
		{
			return this.m_dealsNoDamage;
		}
	}

	// Token: 0x17000695 RID: 1685
	// (get) Token: 0x06000C0C RID: 3084 RVA: 0x00024E9E File Offset: 0x0002309E
	public AbilityType AbilityType
	{
		get
		{
			return this.m_abilityType;
		}
	}

	// Token: 0x17000696 RID: 1686
	// (get) Token: 0x06000C0D RID: 3085 RVA: 0x00024EA6 File Offset: 0x000230A6
	protected virtual float AttackBounceHeight
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000697 RID: 1687
	// (get) Token: 0x06000C0E RID: 3086 RVA: 0x00024EAD File Offset: 0x000230AD
	protected virtual float TellIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000698 RID: 1688
	// (get) Token: 0x06000C0F RID: 3087 RVA: 0x00024EB4 File Offset: 0x000230B4
	protected virtual float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000699 RID: 1689
	// (get) Token: 0x06000C10 RID: 3088 RVA: 0x00024EBB File Offset: 0x000230BB
	protected virtual float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700069A RID: 1690
	// (get) Token: 0x06000C11 RID: 3089 RVA: 0x00024EC2 File Offset: 0x000230C2
	protected virtual float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700069B RID: 1691
	// (get) Token: 0x06000C12 RID: 3090 RVA: 0x00024EC9 File Offset: 0x000230C9
	protected virtual float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700069C RID: 1692
	// (get) Token: 0x06000C13 RID: 3091 RVA: 0x00024ED0 File Offset: 0x000230D0
	protected virtual float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700069D RID: 1693
	// (get) Token: 0x06000C14 RID: 3092 RVA: 0x00024ED7 File Offset: 0x000230D7
	protected virtual float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700069E RID: 1694
	// (get) Token: 0x06000C15 RID: 3093 RVA: 0x00024EDE File Offset: 0x000230DE
	protected virtual float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700069F RID: 1695
	// (get) Token: 0x06000C16 RID: 3094 RVA: 0x00024EE5 File Offset: 0x000230E5
	protected virtual float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006A0 RID: 1696
	// (get) Token: 0x06000C17 RID: 3095 RVA: 0x00024EEC File Offset: 0x000230EC
	protected virtual float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006A1 RID: 1697
	// (get) Token: 0x06000C18 RID: 3096 RVA: 0x00024EF3 File Offset: 0x000230F3
	public virtual Vector2 ProjectileOffset
	{
		get
		{
			return this.m_projectileOffset;
		}
	}

	// Token: 0x170006A2 RID: 1698
	// (get) Token: 0x06000C19 RID: 3097 RVA: 0x00024EFB File Offset: 0x000230FB
	public virtual bool HasAttackFlipCheck
	{
		get
		{
			return this.m_hasAttackFlipCheck;
		}
	}

	// Token: 0x170006A3 RID: 1699
	// (get) Token: 0x06000C1A RID: 3098 RVA: 0x00024F03 File Offset: 0x00023103
	public virtual string ProjectileName
	{
		get
		{
			return this.m_projectileName;
		}
	}

	// Token: 0x170006A4 RID: 1700
	// (get) Token: 0x06000C1B RID: 3099 RVA: 0x00024F0B File Offset: 0x0002310B
	protected virtual int CurrentAnimationLayer
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170006A5 RID: 1701
	// (get) Token: 0x06000C1C RID: 3100 RVA: 0x00024F0E File Offset: 0x0002310E
	public bool HasAnimation
	{
		get
		{
			return this.m_hasAnimation;
		}
	}

	// Token: 0x170006A6 RID: 1702
	// (get) Token: 0x06000C1D RID: 3101 RVA: 0x00024F16 File Offset: 0x00023116
	public virtual string AbilityTellIntroName
	{
		get
		{
			return this.m_abilityTellIntroName;
		}
	}

	// Token: 0x170006A7 RID: 1703
	// (get) Token: 0x06000C1E RID: 3102 RVA: 0x00024F1E File Offset: 0x0002311E
	// (set) Token: 0x06000C1F RID: 3103 RVA: 0x00024F26 File Offset: 0x00023126
	public bool IsAnimationComplete
	{
		get
		{
			return this.m_isAnimationComplete;
		}
		protected set
		{
			this.m_isAnimationComplete = value;
		}
	}

	// Token: 0x170006A8 RID: 1704
	// (get) Token: 0x06000C20 RID: 3104 RVA: 0x00024F2F File Offset: 0x0002312F
	// (set) Token: 0x06000C21 RID: 3105 RVA: 0x00024F37 File Offset: 0x00023137
	public bool AbilityActive { get; protected set; }

	// Token: 0x170006A9 RID: 1705
	// (get) Token: 0x06000C22 RID: 3106 RVA: 0x00024F40 File Offset: 0x00023140
	public CastAbilityType EVAssignedCastAbilityType
	{
		get
		{
			return this.AbilityData.Type;
		}
	}

	// Token: 0x170006AA RID: 1706
	// (get) Token: 0x06000C23 RID: 3107 RVA: 0x00024F4D File Offset: 0x0002314D
	// (set) Token: 0x06000C24 RID: 3108 RVA: 0x00024F55 File Offset: 0x00023155
	public CastAbilityType CastAbilityType { get; private set; }

	// Token: 0x170006AB RID: 1707
	// (get) Token: 0x06000C25 RID: 3109 RVA: 0x00024F5E File Offset: 0x0002315E
	public int BaseCost
	{
		get
		{
			return this.AbilityData.BaseCost;
		}
	}

	// Token: 0x170006AC RID: 1708
	// (get) Token: 0x06000C26 RID: 3110 RVA: 0x00024F6C File Offset: 0x0002316C
	public int ActualCost
	{
		get
		{
			int baseCost = this.BaseCost;
			if (TraitManager.IsTraitActive(TraitType.ManaCostAndDamageUp))
			{
				return (int)((float)baseCost * 2f);
			}
			return baseCost;
		}
	}

	// Token: 0x170006AD RID: 1709
	// (get) Token: 0x06000C27 RID: 3111 RVA: 0x00024F97 File Offset: 0x00023197
	public int MaxAmmo
	{
		get
		{
			return this.AbilityData.MaxAmmo;
		}
	}

	// Token: 0x170006AE RID: 1710
	// (get) Token: 0x06000C28 RID: 3112 RVA: 0x00024FA4 File Offset: 0x000231A4
	public float BaseCooldownTime
	{
		get
		{
			return this.AbilityData.CooldownTime;
		}
	}

	// Token: 0x170006AF RID: 1711
	// (get) Token: 0x06000C29 RID: 3113 RVA: 0x00024FB4 File Offset: 0x000231B4
	public float ActualCooldownTime
	{
		get
		{
			float abilityCoolDownMod = this.m_abilityController.PlayerController.AbilityCoolDownMod;
			float num = 0f;
			if (this.CastAbilityType == CastAbilityType.Weapon)
			{
				if (this.AbilityType != AbilityType.PistolWeapon && this.AbilityType != AbilityType.ReloadTalent)
				{
					int level = SaveManager.PlayerSaveData.GetRelic(RelicType.AttackCooldown).Level;
					if (this.CooldownRegenType == CooldownRegenType.SpinKick)
					{
						num += (float)level;
					}
					else
					{
						num += 2f * (float)level;
					}
				}
				if (this.AbilityType == AbilityType.LanceWeapon && (this as Lance_Ability).DashAttacking)
				{
					num += 0f;
				}
			}
			float num2 = (this.BaseCooldownTime + num) * (1f - abilityCoolDownMod);
			if (TraitManager.IsTraitActive(TraitType.LongerCD) && this.AbilityType != AbilityType.ReloadTalent && this.AbilityType != AbilityType.CookingTalent && (this.CastAbilityType == CastAbilityType.Spell || this.CastAbilityType == CastAbilityType.Talent))
			{
				return (float)Mathf.RoundToInt(num2 * 1.25f) + 5f;
			}
			if (TraitManager.IsTraitActive(TraitType.ManaCostAndDamageUp) && this.CastAbilityType == CastAbilityType.Spell)
			{
				return num2 * 1f;
			}
			return num2;
		}
	}

	// Token: 0x170006B0 RID: 1712
	// (get) Token: 0x06000C2A RID: 3114 RVA: 0x000250C2 File Offset: 0x000232C2
	// (set) Token: 0x06000C2B RID: 3115 RVA: 0x000250CA File Offset: 0x000232CA
	public bool DisplayPausedAbilityCooldown { get; set; }

	// Token: 0x170006B1 RID: 1713
	// (get) Token: 0x06000C2C RID: 3116 RVA: 0x000250D3 File Offset: 0x000232D3
	public float LockoutTime
	{
		get
		{
			return this.AbilityData.LockoutTime;
		}
	}

	// Token: 0x170006B2 RID: 1714
	// (get) Token: 0x06000C2D RID: 3117 RVA: 0x000250E0 File Offset: 0x000232E0
	// (set) Token: 0x06000C2E RID: 3118 RVA: 0x000250ED File Offset: 0x000232ED
	public bool DecreaseCooldownWhenHit
	{
		get
		{
			return this.AbilityData.CooldownDecreasePerHit;
		}
		set
		{
			this.AbilityData.CooldownDecreasePerHit = value;
		}
	}

	// Token: 0x170006B3 RID: 1715
	// (get) Token: 0x06000C2F RID: 3119 RVA: 0x000250FB File Offset: 0x000232FB
	// (set) Token: 0x06000C30 RID: 3120 RVA: 0x00025108 File Offset: 0x00023308
	public bool DecreaseCooldownOverTime
	{
		get
		{
			return this.AbilityData.CooldownDecreaseOverTime;
		}
		set
		{
			this.AbilityData.CooldownDecreaseOverTime = value;
		}
	}

	// Token: 0x170006B4 RID: 1716
	// (get) Token: 0x06000C31 RID: 3121 RVA: 0x00025116 File Offset: 0x00023316
	public bool GainManaWhenHit
	{
		get
		{
			return this.AbilityData.ManaGainPerHit;
		}
	}

	// Token: 0x170006B5 RID: 1717
	// (get) Token: 0x06000C32 RID: 3122 RVA: 0x00025123 File Offset: 0x00023323
	public bool CooldownRefreshesAllAmmo
	{
		get
		{
			return this.AbilityData.CooldownRefreshesAllAmmo;
		}
	}

	// Token: 0x170006B6 RID: 1718
	// (get) Token: 0x06000C33 RID: 3123 RVA: 0x00025130 File Offset: 0x00023330
	public virtual bool LockDirectionWhenCasting
	{
		get
		{
			return !TraitManager.IsTraitActive(TraitType.DisableAttackLock) && this.AbilityData.LockDirection;
		}
	}

	// Token: 0x170006B7 RID: 1719
	// (get) Token: 0x06000C34 RID: 3124 RVA: 0x0002514B File Offset: 0x0002334B
	public virtual float AirMovementMod
	{
		get
		{
			return this.AbilityData.AirMovementMod;
		}
	}

	// Token: 0x170006B8 RID: 1720
	// (get) Token: 0x06000C35 RID: 3125 RVA: 0x00025158 File Offset: 0x00023358
	public virtual float MovementMod
	{
		get
		{
			return this.AbilityData.MovementMod;
		}
	}

	// Token: 0x170006B9 RID: 1721
	// (get) Token: 0x06000C36 RID: 3126 RVA: 0x00025165 File Offset: 0x00023365
	public bool CanCastWhileDashing
	{
		get
		{
			return this.AbilityData.CanCastWhileDashing;
		}
	}

	// Token: 0x170006BA RID: 1722
	// (get) Token: 0x06000C37 RID: 3127 RVA: 0x00025172 File Offset: 0x00023372
	public bool WeaponInterruptable
	{
		get
		{
			return this.AbilityData.AttackCancel;
		}
	}

	// Token: 0x170006BB RID: 1723
	// (get) Token: 0x06000C38 RID: 3128 RVA: 0x0002517F File Offset: 0x0002337F
	public bool SpellInterruptable
	{
		get
		{
			return this.AbilityData.SpellCancel;
		}
	}

	// Token: 0x170006BC RID: 1724
	// (get) Token: 0x06000C39 RID: 3129 RVA: 0x0002518C File Offset: 0x0002338C
	public bool TalentInterruptable
	{
		get
		{
			return this.AbilityData.TalentCancel;
		}
	}

	// Token: 0x170006BD RID: 1725
	// (get) Token: 0x06000C3A RID: 3130 RVA: 0x00025199 File Offset: 0x00023399
	public bool DashInterruptable
	{
		get
		{
			return this.AbilityData.DashCancel;
		}
	}

	// Token: 0x170006BE RID: 1726
	// (get) Token: 0x06000C3B RID: 3131 RVA: 0x000251A6 File Offset: 0x000233A6
	public virtual bool JumpInterruptable
	{
		get
		{
			return this.AbilityData.JumpCancel;
		}
	}

	// Token: 0x170006BF RID: 1727
	// (get) Token: 0x06000C3C RID: 3132 RVA: 0x000251B3 File Offset: 0x000233B3
	public float CooldownTimer
	{
		get
		{
			return this.m_cooldownTimer;
		}
	}

	// Token: 0x170006C0 RID: 1728
	// (get) Token: 0x06000C3D RID: 3133 RVA: 0x000251BB File Offset: 0x000233BB
	// (set) Token: 0x06000C3E RID: 3134 RVA: 0x000251C3 File Offset: 0x000233C3
	public float LockoutTimer
	{
		get
		{
			return this.m_lockoutTimer;
		}
		set
		{
			this.m_lockoutTimer = value;
		}
	}

	// Token: 0x170006C1 RID: 1729
	// (get) Token: 0x06000C3F RID: 3135 RVA: 0x000251CC File Offset: 0x000233CC
	// (set) Token: 0x06000C40 RID: 3136 RVA: 0x000251D4 File Offset: 0x000233D4
	public int CurrentAmmo
	{
		get
		{
			return this.m_currentAmmo;
		}
		set
		{
			float num = (float)this.m_currentAmmo;
			this.m_currentAmmo = value;
			this.m_currentAmmo = Mathf.Clamp(this.m_currentAmmo, 0, this.MaxAmmo);
			if (num != (float)this.m_currentAmmo)
			{
				if (this.m_ammoChangeEventArgs == null)
				{
					this.m_ammoChangeEventArgs = new PlayerAmmoChangeEventArgs(this, (float)this.m_currentAmmo, num);
				}
				else
				{
					this.m_ammoChangeEventArgs.Initialize(this, (float)this.m_currentAmmo, num);
				}
				Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerAmmoChange, this, this.m_ammoChangeEventArgs);
				if (this.m_currentAmmo <= 0)
				{
					this.StartCooldownTimer();
				}
			}
		}
	}

	// Token: 0x170006C2 RID: 1730
	// (get) Token: 0x06000C41 RID: 3137 RVA: 0x00025261 File Offset: 0x00023461
	public bool IsOnCooldown
	{
		get
		{
			return this.CooldownTimer > 0f;
		}
	}

	// Token: 0x170006C3 RID: 1731
	// (get) Token: 0x06000C42 RID: 3138 RVA: 0x00025270 File Offset: 0x00023470
	public virtual AbilityData AbilityData
	{
		get
		{
			return this.m_abilityData;
		}
	}

	// Token: 0x170006C4 RID: 1732
	// (get) Token: 0x06000C43 RID: 3139 RVA: 0x00025278 File Offset: 0x00023478
	// (set) Token: 0x06000C44 RID: 3140 RVA: 0x00025280 File Offset: 0x00023480
	public AbilityAnimState CurrentAbilityAnimState { get; protected set; }

	// Token: 0x06000C45 RID: 3141 RVA: 0x0002528C File Offset: 0x0002348C
	protected virtual void Awake()
	{
		this.m_hasAnimation = !string.IsNullOrEmpty(this.m_abilityTellIntroName);
		if (Application.isPlaying)
		{
			if (this.m_abilityData)
			{
				this.m_abilityData = UnityEngine.Object.Instantiate<AbilityData>(this.m_abilityData);
			}
			this.m_cooldownOverEventArgs = new AbilityCooldownOverEventArgs(this);
			this.m_attackFlipRaycastArray = new RaycastHit2D[5];
			this.m_reduceCooldownEvent = new Action<Projectile_RL, GameObject>(this.ReduceCooldownEvent);
			this.m_increaseManaEvent = new Action<Projectile_RL, GameObject>(this.IncreaseManaEvent);
			this.m_onProjectileHitDisableAttackFlip = new Action<Projectile_RL, GameObject>(this.OnProjectileHitDisableAttackFlip);
			this.m_bounce = new Action<Projectile_RL, GameObject>(this.Bounce);
			this.m_exitAnimationState = new Action<AbilityAnimState>(this.ExitAnimationState);
			this.m_enterAnimationState = new Action<AbilityAnimState>(this.EnterAnimationState);
			this.m_onEnemyHitCancelAttackFlip = new Action<object, EventArgs>(this.OnEnemyHitCancelAttackFlip);
		}
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x00025370 File Offset: 0x00023570
	public virtual void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		this.m_abilityController = abilityController;
		this.m_animator = this.m_abilityController.Animator;
		if (!this.m_animator)
		{
			throw new Exception("Animator null on ability: " + base.name);
		}
		this.CastAbilityType = castAbilityType;
		this.CurrentAmmo = this.MaxAmmo;
		this.SetCooldownRegenType();
		if (this.m_critHitTimingWindow != Vector2.zero)
		{
			this.m_critEffectGO = new GameObject("CritEffectGO");
			this.m_critEffectGO.transform.SetParent(this.m_abilityController.PlayerController.Pivot.transform, false);
			this.m_critEffectGO.transform.localScale = new Vector3(1f, 1f, 1f);
			this.m_critEffectGO.transform.localPosition = new Vector3(0f, this.m_critEffectOffset.y, 0f);
		}
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x0002546A File Offset: 0x0002366A
	public virtual void Reinitialize()
	{
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x0002546C File Offset: 0x0002366C
	private void SetCooldownRegenType()
	{
		if (this.DecreaseCooldownOverTime)
		{
			this.CooldownRegenType = CooldownRegenType.Timer_Small;
			return;
		}
		if (this.DecreaseCooldownWhenHit)
		{
			this.CooldownRegenType = CooldownRegenType.EnemyHit_Small;
			return;
		}
		if (base.GetComponent<ReduceCDOnKillAbilityMod>())
		{
			this.CooldownRegenType = CooldownRegenType.EnemyKilled_Small;
			return;
		}
		if (base.GetComponent<ReduceCDOnPlayerHitAbilityMod>())
		{
			this.CooldownRegenType = CooldownRegenType.PlayerHit_Small;
			return;
		}
		if (base.GetComponent<ReduceCDOnItemDropAbilityMod>())
		{
			this.CooldownRegenType = CooldownRegenType.MeatPickedUp_Small;
			return;
		}
		if (base.GetComponent<ReduceCDOnSpinKickAbilityMod>())
		{
			this.CooldownRegenType = CooldownRegenType.SpinKick;
		}
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x000254F4 File Offset: 0x000236F4
	public virtual void PreCastAbility()
	{
		this.AbilityActive = true;
		this.IsAnimationComplete = false;
		this.m_animator.SetFloat("NaturalWalk", 1f);
		this.m_abilityCastStartTime = Time.time;
		bool flag = false;
		if (SaveManager.ConfigData.ToggleMouseAttackFlip)
		{
			ControllerType lastActiveControllerType = ReInput.controllers.GetLastActiveControllerType();
			if (lastActiveControllerType == ControllerType.Mouse || lastActiveControllerType == ControllerType.Keyboard)
			{
				flag = true;
			}
		}
		if (!flag && !this.m_abilityController.PlayerController.CharacterMove.IsFlickDetected && ((this.m_abilityController.HorizontalInput < 0f && this.m_abilityController.PlayerController.IsFacingRight) || (this.m_abilityController.HorizontalInput > 0f && !this.m_abilityController.PlayerController.IsFacingRight)))
		{
			this.m_abilityController.PlayerController.CharacterCorgi.Flip(false, false);
		}
		if (this.HasAnimation)
		{
			AbilityAnimBehaviourManager.OnAnimStateEnterRelay.AddListener(this.m_enterAnimationState, false);
			AbilityAnimBehaviourManager.OnAnimStateExitRelay.AddListener(this.m_exitAnimationState, false);
			this.m_abilityNameCasted = this.AbilityTellIntroName;
			this.m_animator.SetTrigger(this.m_abilityNameCasted);
			if (this.HasAttackFlipCheck)
			{
				this.m_enemyHit = false;
				this.PerformAttackFlipCheck();
				return;
			}
		}
		else
		{
			this.IsAnimationComplete = true;
		}
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x00025630 File Offset: 0x00023830
	public void PerformAttackFlipCheck()
	{
		if (this.m_attackFlipCheckCoroutine != null)
		{
			base.StopCoroutine(this.m_attackFlipCheckCoroutine);
		}
		this.m_attackFlipCheckCoroutine = base.StartCoroutine(this.UpdateAttackFlipCheck());
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x00025658 File Offset: 0x00023858
	private void OnEnemyHitCancelAttackFlip(object sender, EventArgs args)
	{
		CharacterHitEventArgs characterHitEventArgs = args as CharacterHitEventArgs;
		if (characterHitEventArgs != null && characterHitEventArgs.Attacker == this.m_firedProjectile && this.m_attackFlipCheckCoroutine != null)
		{
			base.StopCoroutine(this.m_attackFlipCheckCoroutine);
		}
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x00025694 File Offset: 0x00023894
	protected virtual void EnterAnimationState(AbilityAnimState animState)
	{
		this.CurrentAbilityAnimState = animState;
		float value = 0f;
		switch (animState)
		{
		case AbilityAnimState.TellIntro:
			value = this.TellIntroAnimSpeed;
			this.OnEnterTellIntroLogic();
			break;
		case AbilityAnimState.Tell:
			value = this.TellAnimSpeed;
			this.OnEnterTellLogic();
			break;
		case AbilityAnimState.Attack_Intro:
			value = this.AttackIntroAnimSpeed;
			this.OnEnterAttackIntroLogic();
			break;
		case AbilityAnimState.Attack:
			value = this.AttackAnimSpeed;
			this.OnEnterAttackLogic();
			break;
		case AbilityAnimState.Exit:
			value = this.ExitAnimSpeed;
			this.OnEnterExitLogic();
			break;
		}
		if (!this.m_animator)
		{
			throw new Exception("ability animator should not be null.  AbilityName: " + base.name + " WasDestroyed: " + (!this).ToString());
		}
		this.m_animator.SetFloat("Ability_Anim_Speed", value);
		float duration = this.CalculateTotalAnimationDelay();
		this.m_changeAnimCoroutine = base.StartCoroutine(this.ChangeAnim(duration));
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x00025777 File Offset: 0x00023977
	protected void CancelChangeAnimCoroutine()
	{
		this.m_animator.ResetTrigger("Change_Ability_Anim");
		if (this.m_changeAnimCoroutine != null)
		{
			base.StopCoroutine(this.m_changeAnimCoroutine);
		}
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x000257A0 File Offset: 0x000239A0
	protected virtual float CalculateTotalAnimationDelay()
	{
		float num = 0f;
		float num2 = 0f;
		switch (this.CurrentAbilityAnimState)
		{
		case AbilityAnimState.TellIntro:
			num = this.TellIntroAnimSpeed;
			num2 = this.TellIntroAnimExitDelay;
			break;
		case AbilityAnimState.Tell:
			num = this.TellAnimSpeed;
			num2 = this.TellAnimExitDelay;
			break;
		case AbilityAnimState.Attack_Intro:
			num = this.AttackIntroAnimSpeed;
			num2 = this.AttackIntroAnimExitDelay;
			break;
		case AbilityAnimState.Attack:
			num = this.AttackAnimSpeed;
			num2 = this.AttackAnimExitDelay;
			break;
		case AbilityAnimState.Exit:
			num = this.ExitAnimSpeed;
			num2 = this.ExitAnimExitDelay;
			break;
		}
		float num3 = 0f;
		float num4 = this.m_animator.GetCurrentAnimatorStateInfo(this.CurrentAnimationLayer).length / num;
		return num3 + num4 + num2 - Time.deltaTime;
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x00025856 File Offset: 0x00023A56
	protected virtual void ExitAnimationState(AbilityAnimState animState)
	{
		switch (animState)
		{
		case AbilityAnimState.TellIntro:
			this.OnExitTellIntroLogic();
			return;
		case AbilityAnimState.Tell:
			this.OnExitTellLogic();
			return;
		case AbilityAnimState.Attack_Intro:
			this.OnExitAttackIntroLogic();
			return;
		case AbilityAnimState.Attack:
			this.OnExitAttackLogic();
			return;
		case AbilityAnimState.Exit:
			this.OnExitExitLogic();
			return;
		default:
			return;
		}
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x00025895 File Offset: 0x00023A95
	protected virtual void OnEnterTellIntroLogic()
	{
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x00025897 File Offset: 0x00023A97
	protected virtual void OnEnterTellLogic()
	{
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x00025899 File Offset: 0x00023A99
	protected virtual void OnEnterAttackIntroLogic()
	{
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x0002589B File Offset: 0x00023A9B
	protected virtual void OnEnterAttackLogic()
	{
		this.m_abilityController.BroadcastAbilityCastEvents(this.CastAbilityType);
		this.FireProjectile();
		this.StartCooldownTimer();
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x000258BA File Offset: 0x00023ABA
	protected virtual void OnEnterExitLogic()
	{
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x000258BC File Offset: 0x00023ABC
	protected virtual void OnExitTellIntroLogic()
	{
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x000258BE File Offset: 0x00023ABE
	protected virtual void OnExitTellLogic()
	{
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x000258C0 File Offset: 0x00023AC0
	protected virtual void OnExitAttackIntroLogic()
	{
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x000258C4 File Offset: 0x00023AC4
	protected virtual void OnExitAttackLogic()
	{
		if (!this.m_animator)
		{
			throw new Exception("ability animator should not be null.  AbilityName: " + base.name + " WasDestroyed: " + (!this).ToString());
		}
		this.m_animator.SetFloat("NaturalWalk", 0f);
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x0002591F File Offset: 0x00023B1F
	protected virtual void OnExitExitLogic()
	{
		this.IsAnimationComplete = true;
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x00025928 File Offset: 0x00023B28
	protected virtual IEnumerator ChangeAnim(float duration)
	{
		if (duration <= 0f)
		{
			yield return null;
		}
		while (duration > 0f)
		{
			duration -= Time.deltaTime;
			yield return null;
		}
		this.m_animator.SetTrigger("Change_Ability_Anim");
		this.PerformTurnAnimCheck();
		yield break;
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x00025940 File Offset: 0x00023B40
	protected void PerformTurnAnimCheck()
	{
		if (this.CurrentAbilityAnimState == AbilityAnimState.Exit && this.LockDirectionWhenCasting && this.MovementMod > 0f)
		{
			global::PlayerController playerController = this.m_abilityController.PlayerController;
			Character characterCorgi = playerController.CharacterCorgi;
			if (playerController.IsGrounded)
			{
				float axis = characterCorgi.REPlayer.GetAxis("MoveHorizontal");
				if (axis > 0.1f && !characterCorgi.IsFacingRight)
				{
					characterCorgi.Flip(false, false);
					return;
				}
				if (axis < -0.1f && characterCorgi.IsFacingRight)
				{
					characterCorgi.Flip(false, false);
				}
			}
		}
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x000259C6 File Offset: 0x00023BC6
	public virtual IEnumerator CastAbility()
	{
		this.m_beginCastingRelay.Dispatch();
		if (this.m_hasAnimation)
		{
			while (!this.IsAnimationComplete)
			{
				yield return null;
			}
		}
		else
		{
			this.OnEnterAttackLogic();
		}
		yield break;
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x000259D8 File Offset: 0x00023BD8
	public virtual void StopAbility(bool abilityInterrupted)
	{
		this.m_stopCastingRelay.Dispatch();
		this.m_animator.SetFloat("NaturalWalk", 0f);
		base.StopAllCoroutines();
		this.IsAnimationComplete = false;
		this.m_attackFlipCheckTimer = 0f;
		this.m_animator.SetFloat("Ability_Anim_Speed", 1f);
		this.ForceTriggerCrit = false;
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyHit, this.m_onEnemyHitCancelAttackFlip);
		if (this.m_critDisplayOn)
		{
			this.m_abilityController.PlayerController.BlinkPulseEffect.ResetRendererArrayColor();
			this.m_critDisplayOn = false;
		}
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile = null;
		}
		if (!this.AbilityActive)
		{
			return;
		}
		if (this.HasAnimation)
		{
			AbilityAnimBehaviourManager.OnAnimStateEnterRelay.RemoveListener(this.m_enterAnimationState);
			AbilityAnimBehaviourManager.OnAnimStateExitRelay.RemoveListener(this.m_exitAnimationState);
			if (abilityInterrupted)
			{
				this.m_abilityController.Animator.SetTrigger("Cancel_Ability_Anim");
			}
			this.m_animator.ResetTrigger("Change_Ability_Anim");
			this.m_animator.ResetTrigger(this.m_abilityNameCasted);
			AbilityAnimBehaviourManager.ResetManager();
		}
		this.AbilityActive = false;
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x00025AF8 File Offset: 0x00023CF8
	protected virtual void Update()
	{
		if (this.DecreaseCooldownOverTime && this.CooldownTimer > 0f)
		{
			this.m_cooldownTimer -= Time.deltaTime;
			if (this.CooldownTimer <= 0f)
			{
				this.EndCooldownTimer(true);
			}
		}
		if (this.LockoutTimer > 0f)
		{
			this.m_lockoutTimer -= Time.deltaTime;
		}
		if (this.AbilityActive)
		{
			this.CriticalHitTimingDisplay();
		}
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x00025B6D File Offset: 0x00023D6D
	protected void CriticalHitTimingDisplay()
	{
		if (this.IsInCritWindow && !this.m_critDisplayOn && this.m_critEffectGO)
		{
			base.StartCoroutine(this.CritWindowCoroutine());
		}
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x00025B99 File Offset: 0x00023D99
	private IEnumerator CritWindowCoroutine()
	{
		this.m_critDisplayOn = true;
		GenericEffect genericEffect = EffectManager.PlayEffect(this.m_abilityController.gameObject, null, "WeaponShaPing_Effect", Vector3.zero, 1f, EffectStopType.Gracefully, EffectTriggerDirection.None) as GenericEffect;
		genericEffect.transform.SetParent(this.m_critEffectGO.transform, false);
		genericEffect.transform.localPosition = new Vector3(this.m_critEffectOffset.x, 0f, 0f);
		this.PlayCritWindowAudio();
		float num = this.m_critHitTimingWindow.y - this.m_critHitTimingWindow.x;
		ParticleSystem.MainModule mainPartSys = genericEffect.ParticleSystem.main;
		float simulationSpeed = mainPartSys.duration / num;
		float storedSimSpeed = mainPartSys.simulationSpeed;
		mainPartSys.simulationSpeed = simulationSpeed;
		float delay = Time.time + num;
		while (Time.time < delay)
		{
			yield return null;
		}
		mainPartSys.simulationSpeed = storedSimSpeed;
		this.m_critDisplayOn = false;
		yield break;
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x00025BA8 File Offset: 0x00023DA8
	protected virtual void PlayCritWindowAudio()
	{
	}

	// Token: 0x06000C62 RID: 3170 RVA: 0x00025BAA File Offset: 0x00023DAA
	protected virtual void PlayDashCritAudio()
	{
	}

	// Token: 0x06000C63 RID: 3171 RVA: 0x00025BAC File Offset: 0x00023DAC
	protected virtual void PlayFreeCritReleaseAudio()
	{
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Weapons/sfx_weapon_crit_charged_release", this.m_abilityController.gameObject);
	}

	// Token: 0x06000C64 RID: 3172 RVA: 0x00025BC4 File Offset: 0x00023DC4
	protected virtual void FireProjectile()
	{
		if (!string.IsNullOrEmpty(this.ProjectileName))
		{
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, true, 0f, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			this.ApplyAbilityCosts();
		}
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x00025C28 File Offset: 0x00023E28
	protected virtual void ApplyAbilityCosts()
	{
		if (this.m_abilityController.PlayerController.SpellOrbs > 0)
		{
			this.m_consumedSpellOrb = true;
			this.m_abilityController.PlayerController.SpellOrbs--;
			this.m_abilityController.PlayerController.SetMana(0f, true, true, false);
			return;
		}
		if (this.MaxAmmo > 0)
		{
			int currentAmmo = this.CurrentAmmo;
			this.CurrentAmmo = currentAmmo - 1;
		}
		if (this.ActualCost > 0)
		{
			RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.FreeCastSpell);
			if (relic.Level > 0)
			{
				int relicMaxStack = Relic_EV.GetRelicMaxStack(relic.RelicType, relic.Level);
				if (relic.IntValue >= relicMaxStack)
				{
					relic.SetIntValue(0, false, true);
					return;
				}
				relic.SetIntValue(1, true, true);
			}
			this.m_abilityController.PlayerController.SetMana((float)(-(float)this.ActualCost), true, true, false);
		}
	}

	// Token: 0x06000C66 RID: 3174 RVA: 0x00025D08 File Offset: 0x00023F08
	public virtual void StartCooldownTimer()
	{
		if (this.m_consumedSpellOrb)
		{
			this.m_consumedSpellOrb = false;
			return;
		}
		if (this.MaxAmmo <= 0 || this.CooldownTimer <= 0f)
		{
			this.m_cooldownTimer = this.ActualCooldownTime;
		}
		if (this.MaxAmmo <= 0 || this.CooldownTimer <= 0f || (this.MaxAmmo > 0 && this.CurrentAmmo <= 0))
		{
			if (this.m_cooldownEventArgs == null)
			{
				this.m_cooldownEventArgs = new CooldownEventArgs(this);
			}
			else
			{
				this.m_cooldownEventArgs.Initialize(this);
			}
			this.m_onCooldownRelay.Dispatch(this, this.m_cooldownEventArgs);
		}
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x00025DA4 File Offset: 0x00023FA4
	public void EndCooldownTimer(bool displayCooldownResetText = true)
	{
		if (!this)
		{
			return;
		}
		int currentAmmo = this.CurrentAmmo;
		this.RegenerateAmmo();
		this.m_cooldownTimer = 0f;
		if (this.AbilityType != AbilityType.LightningSpell && displayCooldownResetText && currentAmmo == 0 && !CutsceneManager.IsCutsceneActive)
		{
			AudioManager.PlayOneShot(null, "event:/UI/FrontEnd/ui_fe_deathRecap_discoveries", base.transform.position);
			string @string = LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_COOLDOWN_CHARGED_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			TextPopupManager.DisplayTextDefaultPos(TextPopupType.DownstrikeAmmoGain, @string, this.m_abilityController.PlayerController, true, true);
		}
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.AbilityCooldownOver, this, this.m_cooldownOverEventArgs);
		if (this.CurrentAmmo < this.MaxAmmo && this.MaxAmmo > 0)
		{
			this.StartCooldownTimer();
		}
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x00025E60 File Offset: 0x00024060
	public virtual void RegenerateAmmo()
	{
		if (this.CooldownRefreshesAllAmmo)
		{
			this.CurrentAmmo = this.MaxAmmo;
			return;
		}
		int currentAmmo = this.CurrentAmmo;
		this.CurrentAmmo = currentAmmo + 1;
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x00025E92 File Offset: 0x00024092
	protected void StartLockoutTimer()
	{
		this.m_lockoutTimer = this.LockoutTime;
	}

	// Token: 0x06000C6A RID: 3178 RVA: 0x00025EA0 File Offset: 0x000240A0
	public void InitializeProjectile(Projectile_RL projectile)
	{
		projectile.OnCollisionRelay.AddOnce(this.m_onProjectileHitDisableAttackFlip, false);
		if (this.DecreaseCooldownWhenHit)
		{
			if (projectile.HasCDReductionDelay)
			{
				if (this.m_persistentCollListenerProj)
				{
					this.m_persistentCollListenerProj.OnCollisionRelay.RemoveListener(this.m_reduceCooldownEvent);
				}
				this.m_persistentCollListenerProj = projectile;
				projectile.OnCollisionRelay.AddListener(this.m_reduceCooldownEvent, false);
			}
			else
			{
				projectile.OnCollisionRelay.AddOnce(this.m_reduceCooldownEvent, false);
			}
		}
		if (this.GainManaWhenHit)
		{
			projectile.OnCollisionRelay.AddOnce(this.m_increaseManaEvent, false);
		}
		if (projectile.CastAbilityType == this.CastAbilityType)
		{
			if (this.CastAbilityType == CastAbilityType.Weapon)
			{
				if (this.m_abilityController.PlayerController.CharacterClass.ClassType == ClassType.LadleClass && !(projectile is DownstrikeProjectile_RL))
				{
					float num = 3.05f;
					int level = SaveManager.PlayerSaveData.GetRelic(RelicType.WeaponsBurnAdd).Level;
					if (level > 0)
					{
						num += 2f * (float)level;
					}
					projectile.AttachStatusEffect(StatusEffectType.Enemy_Burn, num);
				}
				if (this.m_abilityController.PlayerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_FreeCrit))
				{
					this.PlayFreeCritReleaseAudio();
					this.m_abilityController.PlayerController.StatusEffectController.StopStatusEffect(StatusEffectType.Player_FreeCrit, false);
					this.ForceTriggerCrit = true;
				}
				if (this.m_abilityController.PlayerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_Combo))
				{
					BaseStatusEffect statusEffect = this.m_abilityController.PlayerController.StatusEffectController.GetStatusEffect(StatusEffectType.Player_Combo);
					if (statusEffect && statusEffect.IsPlaying && statusEffect.TimesStacked >= 15)
					{
						this.ForceTriggerCrit = true;
					}
				}
				if (this.m_abilityController.PlayerController.CloakInterrupted)
				{
					projectile.AttachStatusEffect(StatusEffectType.Enemy_Vulnerable, 2f);
					this.ForceTriggerCrit = true;
				}
			}
			if (this.CritsWhenDashing && this.m_abilityController.PlayerController.MovementState == CharacterStates.MovementStates.Dashing)
			{
				this.PlayDashCritAudio();
				this.ForceTriggerCrit = true;
			}
		}
		if (this.IsInCritWindow)
		{
			this.ForceTriggerCrit = true;
			if (this.m_critEffectGO)
			{
				BaseEffect baseEffect = EffectManager.PlayEffect(this.m_abilityController.gameObject, null, "CriticalSuccess_Effect", Vector3.zero, 1f, EffectStopType.Gracefully, EffectTriggerDirection.None);
				baseEffect.transform.SetParent(this.m_critEffectGO.transform, false);
				baseEffect.transform.localPosition = new Vector3(this.m_critEffectOffset.x, 0f, 0f);
			}
		}
		if (projectile.CastAbilityType == this.CastAbilityType)
		{
			this.m_abilityCastStartTime = 0f;
			if (this.ForceTriggerCrit)
			{
				projectile.ActualCritChance += 100f;
				if (projectile.CastAbilityType == CastAbilityType.Weapon)
				{
					projectile.ChangeSpriteRendererColor(ProjectileLibrary.GetBuffColor(ProjectileBuffType.PlayerDashAttack));
				}
				this.ForceTriggerCrit = false;
			}
		}
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x00026165 File Offset: 0x00024365
	protected void OnProjectileHitDisableAttackFlip(Projectile_RL projectile, GameObject colliderObj)
	{
		this.m_enemyHit = true;
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x00026170 File Offset: 0x00024370
	protected void ReduceCooldownEvent(Projectile_RL projectile, GameObject colliderObj)
	{
		if (!projectile.isActiveAndEnabled)
		{
			return;
		}
		if (colliderObj.CompareTag("Enemy"))
		{
			if (!projectile.HasCDReductionDelay || Time.time > projectile.CDReducDelay)
			{
				EnemyController component = colliderObj.GetComponent<EnemyController>();
				if (component && component.DisableHPMPBonuses)
				{
					return;
				}
				projectile.CDReducDelay = Time.time + 0.5f;
				this.ReduceCooldown(projectile.CooldownReductionPerHit);
				return;
			}
		}
		else
		{
			if (TraitManager.IsTraitActive(TraitType.BreakPropsForMana) && (colliderObj.CompareTag("Breakable") || colliderObj.CompareTag("FlimsyBreakable")))
			{
				this.ReduceCooldown(0f);
			}
			if (!projectile.HasCDReductionDelay)
			{
				projectile.OnCollisionRelay.AddOnce(this.m_reduceCooldownEvent, false);
			}
		}
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x0002622D File Offset: 0x0002442D
	public void ReduceCooldown(float amount)
	{
		if (this.CooldownTimer > 0f)
		{
			this.m_cooldownTimer -= amount;
			if (this.CooldownTimer <= 0f)
			{
				this.EndCooldownTimer(true);
			}
		}
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x00026260 File Offset: 0x00024460
	public void ReduceFlatCooldown(int flatAmount)
	{
		for (int i = 0; i < flatAmount; i++)
		{
			this.EndCooldownTimer(true);
		}
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x00026280 File Offset: 0x00024480
	protected void IncreaseManaEvent(Projectile_RL projectile, GameObject colliderObj)
	{
		bool isActiveAndEnabled = projectile.isActiveAndEnabled;
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x00026289 File Offset: 0x00024489
	protected IEnumerator UpdateAttackFlipCheck()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyHit, this.m_onEnemyHitCancelAttackFlip);
		this.m_attackFlipCheckTimer = 0.285f;
		global::PlayerController playerController = this.m_abilityController.PlayerController;
		while (this.m_attackFlipCheckTimer > 0f && !this.m_enemyHit)
		{
			float num = 0.01f;
			Vector2 vector = playerController.ControllerCorgi.BoundsBottomLeftCorner;
			Vector2 vector2 = playerController.ControllerCorgi.BoundsTopLeftCorner;
			vector.x -= num;
			vector2.x -= num;
			Vector2 vector3 = playerController.ControllerCorgi.BoundsBottomRightCorner;
			Vector2 vector4 = playerController.ControllerCorgi.BoundsTopRightCorner;
			vector3.x += num;
			vector4.x += num;
			while (!this.m_firedProjectile || !this.m_firedProjectile.isActiveAndEnabled)
			{
				yield return null;
			}
			Collider2D collider = this.m_firedProjectile.HitboxController.GetCollider(HitboxType.Weapon);
			float num2 = collider.bounds.center.x - playerController.Midpoint.x;
			Vector3 center = collider.bounds.center;
			Vector2 boxSize = collider.bounds.size;
			num2 += 0.625f;
			boxSize.x += 1.25f;
			boxSize.y += 1.6f;
			bool collidingFront;
			bool collidingBack;
			if (playerController.IsFacingRight)
			{
				collidingFront = this.CheckAttackFlip(center, boxSize, 1);
				center.x -= num2 * 2f;
				collidingBack = this.CheckAttackFlip(center, boxSize, -1);
			}
			else
			{
				collidingBack = this.CheckAttackFlip(center, boxSize, 1);
				center.x -= num2 * 2f;
				collidingFront = this.CheckAttackFlip(center, boxSize, -1);
			}
			this.m_attackFlipCheckTimer -= Time.deltaTime;
			if (playerController.MovementState == CharacterStates.MovementStates.Dashing)
			{
				break;
			}
			if (this.IsAttackFlipValid(playerController, collidingBack, collidingFront))
			{
				playerController.CharacterCorgi.Flip(false, false);
				break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x00026298 File Offset: 0x00024498
	protected bool CheckAttackFlip(Vector2 boxOrigin, Vector2 boxSize, sbyte direction)
	{
		Vector2 direction2 = Vector2.right;
		if (direction < 0)
		{
			direction2 = Vector2.left;
		}
		int num = Physics2D.BoxCastNonAlloc(boxOrigin, boxSize, 0f, direction2, this.m_attackFlipRaycastArray, 0.01f, 2097152);
		for (int i = 0; i < num; i++)
		{
			RaycastHit2D raycastHit2D = this.m_attackFlipRaycastArray[i];
			if (raycastHit2D.collider.CompareTag("Enemy"))
			{
				return true;
			}
			if (raycastHit2D.collider.CompareTag("EnemyProjectile"))
			{
				Projectile_RL component = raycastHit2D.collider.GetComponent<HitboxInfo>().RootGameObj.GetComponent<Projectile_RL>();
				if (component && (component.CollisionFlags & ProjectileCollisionFlag.WeaponStrikeable) != ProjectileCollisionFlag.None)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x00026344 File Offset: 0x00024544
	protected bool IsAttackFlipValid(global::PlayerController playerController, bool collidingBack, bool collidingFront)
	{
		float axis = Rewired_RL.Player.GetAxis("MoveHorizontal");
		return (axis < 0f && playerController.IsFacingRight && collidingBack && !collidingFront) || (axis > 0f && !playerController.IsFacingRight && !collidingBack && collidingFront);
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x00026394 File Offset: 0x00024594
	protected void Bounce(Projectile_RL projectile, GameObject colliderObj)
	{
		CharacterStates.CharacterConditions conditionState = this.m_abilityController.PlayerController.ConditionState;
		if (conditionState == CharacterStates.CharacterConditions.Stunned || conditionState == CharacterStates.CharacterConditions.Dead)
		{
			return;
		}
		GameObject root = colliderObj.GetRoot(false);
		DownstrikeKnockbackOverride component = root.GetComponent<DownstrikeKnockbackOverride>();
		BoundsObj component2 = root.GetComponent<BoundsObj>();
		float num = this.AttackBounceHeight;
		if (component != null)
		{
			num = component.Value;
		}
		if (component2 != null)
		{
			float num2 = num * num / (2f * Mathf.Abs(-50f));
			float num3 = component2.Top - projectile.Owner.transform.position.y;
			num = Mathf.Sqrt(2f * Mathf.Abs(-50f) * (num2 + num3));
		}
		if (num > this.m_highestBounceAmount)
		{
			this.m_highestBounceAmount = num;
			this.m_triggerBounce = true;
		}
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x00026458 File Offset: 0x00024658
	private void TriggerBounce()
	{
		this.m_triggerBounce = false;
		float highestBounceAmount = this.m_highestBounceAmount;
		this.m_highestBounceAmount = float.MinValue;
		CharacterStates.CharacterConditions conditionState = this.m_abilityController.PlayerController.ConditionState;
		if (conditionState == CharacterStates.CharacterConditions.Stunned || conditionState == CharacterStates.CharacterConditions.Dead)
		{
			return;
		}
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(true);
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.OnCollisionRelay.RemoveListener(this.m_bounce);
		}
		this.m_abilityController.PlayerController.SetVelocityY(highestBounceAmount, false);
		if (this.m_abilityController.PlayerController.CharacterJump != null)
		{
			this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
			this.m_abilityController.PlayerController.CharacterJump.StartJumpTime();
		}
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x00026526 File Offset: 0x00024726
	private void LateUpdate()
	{
		if (this.m_triggerBounce)
		{
			this.TriggerBounce();
		}
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x00026538 File Offset: 0x00024738
	protected virtual void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyHit, this.m_onEnemyHitCancelAttackFlip);
		AbilityAnimBehaviourManager.OnAnimStateEnterRelay.RemoveListener(this.m_enterAnimationState);
		AbilityAnimBehaviourManager.OnAnimStateExitRelay.RemoveListener(this.m_exitAnimationState);
		if (this.m_persistentCollListenerProj)
		{
			this.m_persistentCollListenerProj.OnCollisionRelay.RemoveListener(this.m_reduceCooldownEvent);
		}
		this.m_persistentCollListenerProj = null;
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x0002659E File Offset: 0x0002479E
	public virtual void OnPreDestroy()
	{
	}

	// Token: 0x0400104C RID: 4172
	protected const string CHANGE_ABILITY_ANIM_PARAM = "Change_Ability_Anim";

	// Token: 0x0400104D RID: 4173
	[SerializeField]
	private AbilityType m_abilityType = AbilityType.FireballSpell;

	// Token: 0x0400104E RID: 4174
	[Header("Ability Data")]
	[SerializeField]
	protected AbilityData m_abilityData;

	// Token: 0x0400104F RID: 4175
	[SerializeField]
	[Tooltip("This field does not affect the actual damage of the ability. It's just used to know if it can be cast with the Pacifist trait active.")]
	protected bool m_dealsNoDamage;

	// Token: 0x04001050 RID: 4176
	[SerializeField]
	protected string m_abilityTellIntroName;

	// Token: 0x04001051 RID: 4177
	[SerializeField]
	protected Vector2 m_critHitTimingWindow;

	// Token: 0x04001052 RID: 4178
	[SerializeField]
	protected Vector2 m_critEffectOffset;

	// Token: 0x04001053 RID: 4179
	[SerializeField]
	protected bool m_critsWhenDashing;

	// Token: 0x04001054 RID: 4180
	[SerializeField]
	protected bool m_disableAbilityQueuing;

	// Token: 0x04001055 RID: 4181
	[Header("Projectile Data")]
	[SerializeField]
	protected string m_projectileName;

	// Token: 0x04001056 RID: 4182
	[SerializeField]
	protected Vector2 m_projectileOffset;

	// Token: 0x04001057 RID: 4183
	[SerializeField]
	protected bool m_hasAttackFlipCheck;

	// Token: 0x04001058 RID: 4184
	protected bool m_consumedSpellOrb;

	// Token: 0x04001059 RID: 4185
	protected CastAbility_RL m_abilityController;

	// Token: 0x0400105A RID: 4186
	protected float m_cooldownTimer;

	// Token: 0x0400105B RID: 4187
	protected float m_lockoutTimer;

	// Token: 0x0400105C RID: 4188
	protected int m_currentAmmo;

	// Token: 0x0400105D RID: 4189
	protected Projectile_RL m_firedProjectile;

	// Token: 0x0400105E RID: 4190
	protected bool m_hasAnimation;

	// Token: 0x0400105F RID: 4191
	protected Animator m_animator;

	// Token: 0x04001060 RID: 4192
	private CooldownEventArgs m_cooldownEventArgs;

	// Token: 0x04001061 RID: 4193
	private AbilityCooldownOverEventArgs m_cooldownOverEventArgs;

	// Token: 0x04001062 RID: 4194
	private Coroutine m_attackFlipCheckCoroutine;

	// Token: 0x04001063 RID: 4195
	private float m_attackFlipCheckTimer;

	// Token: 0x04001064 RID: 4196
	private bool m_enemyHit;

	// Token: 0x04001065 RID: 4197
	private bool m_isAnimationComplete;

	// Token: 0x04001066 RID: 4198
	protected Coroutine m_changeAnimCoroutine;

	// Token: 0x04001067 RID: 4199
	private PlayerAmmoChangeEventArgs m_ammoChangeEventArgs;

	// Token: 0x04001068 RID: 4200
	private RaycastHit2D[] m_attackFlipRaycastArray;

	// Token: 0x04001069 RID: 4201
	protected float m_highestBounceAmount;

	// Token: 0x0400106A RID: 4202
	protected bool m_triggerBounce;

	// Token: 0x0400106B RID: 4203
	private string m_abilityNameCasted;

	// Token: 0x0400106C RID: 4204
	protected float m_abilityCastStartTime;

	// Token: 0x0400106D RID: 4205
	private Projectile_RL m_persistentCollListenerProj;

	// Token: 0x0400106E RID: 4206
	protected GameObject m_critEffectGO;

	// Token: 0x0400106F RID: 4207
	private Action<Projectile_RL, GameObject> m_reduceCooldownEvent;

	// Token: 0x04001070 RID: 4208
	private Action<Projectile_RL, GameObject> m_increaseManaEvent;

	// Token: 0x04001071 RID: 4209
	private Action<Projectile_RL, GameObject> m_onProjectileHitDisableAttackFlip;

	// Token: 0x04001072 RID: 4210
	protected Action<Projectile_RL, GameObject> m_bounce;

	// Token: 0x04001073 RID: 4211
	private Action<AbilityAnimState> m_exitAnimationState;

	// Token: 0x04001074 RID: 4212
	private Action<AbilityAnimState> m_enterAnimationState;

	// Token: 0x04001075 RID: 4213
	private Action<object, EventArgs> m_onEnemyHitCancelAttackFlip;

	// Token: 0x04001076 RID: 4214
	protected Relay m_beginCastingRelay = new Relay();

	// Token: 0x04001077 RID: 4215
	protected Relay m_stopCastingRelay = new Relay();

	// Token: 0x04001079 RID: 4217
	private Relay<object, CooldownEventArgs> m_onCooldownRelay = new Relay<object, CooldownEventArgs>();

	// Token: 0x0400107B RID: 4219
	[NonSerialized]
	protected string[] m_projectileNameArray;

	// Token: 0x04001080 RID: 4224
	private bool m_critDisplayOn;
}

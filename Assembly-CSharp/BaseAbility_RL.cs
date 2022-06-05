using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using Rewired;
using RLAudio;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000298 RID: 664
public abstract class BaseAbility_RL : MonoBehaviour, ICooldown, IAbility, IHasProjectileNameArray, IAudioEventEmitter
{
	// Token: 0x170008D8 RID: 2264
	// (get) Token: 0x060012F1 RID: 4849 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public virtual bool IgnoreStuckCheck
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170008D9 RID: 2265
	// (get) Token: 0x060012F2 RID: 4850 RVA: 0x00009A22 File Offset: 0x00007C22
	public IRelayLink BeginCastingRelay
	{
		get
		{
			return this.m_beginCastingRelay.link;
		}
	}

	// Token: 0x170008DA RID: 2266
	// (get) Token: 0x060012F3 RID: 4851 RVA: 0x00009A2F File Offset: 0x00007C2F
	public IRelayLink StopCastingRelay
	{
		get
		{
			return this.m_stopCastingRelay.link;
		}
	}

	// Token: 0x170008DB RID: 2267
	// (get) Token: 0x060012F4 RID: 4852 RVA: 0x00009A3C File Offset: 0x00007C3C
	public bool DisableAbilityQueuing
	{
		get
		{
			return this.m_disableAbilityQueuing;
		}
	}

	// Token: 0x170008DC RID: 2268
	// (get) Token: 0x060012F5 RID: 4853 RVA: 0x00009A44 File Offset: 0x00007C44
	public bool CritsWhenDashing
	{
		get
		{
			return this.m_critsWhenDashing;
		}
	}

	// Token: 0x170008DD RID: 2269
	// (get) Token: 0x060012F6 RID: 4854 RVA: 0x00009A4C File Offset: 0x00007C4C
	// (set) Token: 0x060012F7 RID: 4855 RVA: 0x00009A54 File Offset: 0x00007C54
	public bool ForceTriggerCrit { get; set; }

	// Token: 0x170008DE RID: 2270
	// (get) Token: 0x060012F8 RID: 4856 RVA: 0x00009A5D File Offset: 0x00007C5D
	public IRelayLink<object, CooldownEventArgs> OnBeginCooldownRelay
	{
		get
		{
			return this.m_onCooldownRelay.link;
		}
	}

	// Token: 0x170008DF RID: 2271
	// (get) Token: 0x060012F9 RID: 4857 RVA: 0x00009A6A File Offset: 0x00007C6A
	// (set) Token: 0x060012FA RID: 4858 RVA: 0x00009A72 File Offset: 0x00007C72
	public CooldownRegenType CooldownRegenType { get; protected set; }

	// Token: 0x170008E0 RID: 2272
	// (get) Token: 0x060012FB RID: 4859 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public virtual bool IsAiming
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170008E1 RID: 2273
	// (get) Token: 0x060012FC RID: 4860 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x170008E2 RID: 2274
	// (get) Token: 0x060012FD RID: 4861 RVA: 0x00009A83 File Offset: 0x00007C83
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

	// Token: 0x060012FE RID: 4862 RVA: 0x00009A99 File Offset: 0x00007C99
	protected virtual void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName
		};
	}

	// Token: 0x170008E3 RID: 2275
	// (get) Token: 0x060012FF RID: 4863 RVA: 0x00083F20 File Offset: 0x00082120
	protected virtual bool IsInCritWindow
	{
		get
		{
			return (this.m_critHitTimingWindow.x != 0f || this.m_critHitTimingWindow.y != 0f) && Time.time >= this.m_abilityCastStartTime + this.m_critHitTimingWindow.x && Time.time <= this.m_abilityCastStartTime + this.m_critHitTimingWindow.y;
		}
	}

	// Token: 0x170008E4 RID: 2276
	// (get) Token: 0x06001300 RID: 4864 RVA: 0x00009AB0 File Offset: 0x00007CB0
	public bool DealsNoDamage
	{
		get
		{
			return this.m_dealsNoDamage;
		}
	}

	// Token: 0x170008E5 RID: 2277
	// (get) Token: 0x06001301 RID: 4865 RVA: 0x00009AB8 File Offset: 0x00007CB8
	public AbilityType AbilityType
	{
		get
		{
			return this.m_abilityType;
		}
	}

	// Token: 0x170008E6 RID: 2278
	// (get) Token: 0x06001302 RID: 4866 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float AttackBounceHeight
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008E7 RID: 2279
	// (get) Token: 0x06001303 RID: 4867 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float TellIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170008E8 RID: 2280
	// (get) Token: 0x06001304 RID: 4868 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170008E9 RID: 2281
	// (get) Token: 0x06001305 RID: 4869 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170008EA RID: 2282
	// (get) Token: 0x06001306 RID: 4870 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170008EB RID: 2283
	// (get) Token: 0x06001307 RID: 4871 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170008EC RID: 2284
	// (get) Token: 0x06001308 RID: 4872 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008ED RID: 2285
	// (get) Token: 0x06001309 RID: 4873 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008EE RID: 2286
	// (get) Token: 0x0600130A RID: 4874 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008EF RID: 2287
	// (get) Token: 0x0600130B RID: 4875 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008F0 RID: 2288
	// (get) Token: 0x0600130C RID: 4876 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008F1 RID: 2289
	// (get) Token: 0x0600130D RID: 4877 RVA: 0x00009AC0 File Offset: 0x00007CC0
	public virtual Vector2 ProjectileOffset
	{
		get
		{
			return this.m_projectileOffset;
		}
	}

	// Token: 0x170008F2 RID: 2290
	// (get) Token: 0x0600130E RID: 4878 RVA: 0x00009AC8 File Offset: 0x00007CC8
	public virtual bool HasAttackFlipCheck
	{
		get
		{
			return this.m_hasAttackFlipCheck;
		}
	}

	// Token: 0x170008F3 RID: 2291
	// (get) Token: 0x0600130F RID: 4879 RVA: 0x00009AD0 File Offset: 0x00007CD0
	public virtual string ProjectileName
	{
		get
		{
			return this.m_projectileName;
		}
	}

	// Token: 0x170008F4 RID: 2292
	// (get) Token: 0x06001310 RID: 4880 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int CurrentAnimationLayer
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170008F5 RID: 2293
	// (get) Token: 0x06001311 RID: 4881 RVA: 0x00009AD8 File Offset: 0x00007CD8
	public bool HasAnimation
	{
		get
		{
			return this.m_hasAnimation;
		}
	}

	// Token: 0x170008F6 RID: 2294
	// (get) Token: 0x06001312 RID: 4882 RVA: 0x00009AE0 File Offset: 0x00007CE0
	public virtual string AbilityTellIntroName
	{
		get
		{
			return this.m_abilityTellIntroName;
		}
	}

	// Token: 0x170008F7 RID: 2295
	// (get) Token: 0x06001313 RID: 4883 RVA: 0x00009AE8 File Offset: 0x00007CE8
	// (set) Token: 0x06001314 RID: 4884 RVA: 0x00009AF0 File Offset: 0x00007CF0
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

	// Token: 0x170008F8 RID: 2296
	// (get) Token: 0x06001315 RID: 4885 RVA: 0x00009AF9 File Offset: 0x00007CF9
	// (set) Token: 0x06001316 RID: 4886 RVA: 0x00009B01 File Offset: 0x00007D01
	public bool AbilityActive { get; protected set; }

	// Token: 0x170008F9 RID: 2297
	// (get) Token: 0x06001317 RID: 4887 RVA: 0x00009B0A File Offset: 0x00007D0A
	public CastAbilityType EVAssignedCastAbilityType
	{
		get
		{
			return this.AbilityData.Type;
		}
	}

	// Token: 0x170008FA RID: 2298
	// (get) Token: 0x06001318 RID: 4888 RVA: 0x00009B17 File Offset: 0x00007D17
	// (set) Token: 0x06001319 RID: 4889 RVA: 0x00009B1F File Offset: 0x00007D1F
	public CastAbilityType CastAbilityType { get; private set; }

	// Token: 0x170008FB RID: 2299
	// (get) Token: 0x0600131A RID: 4890 RVA: 0x00009B28 File Offset: 0x00007D28
	public int BaseCost
	{
		get
		{
			return this.AbilityData.BaseCost;
		}
	}

	// Token: 0x170008FC RID: 2300
	// (get) Token: 0x0600131B RID: 4891 RVA: 0x00083F8C File Offset: 0x0008218C
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

	// Token: 0x170008FD RID: 2301
	// (get) Token: 0x0600131C RID: 4892 RVA: 0x00009B35 File Offset: 0x00007D35
	public int MaxAmmo
	{
		get
		{
			return this.AbilityData.MaxAmmo;
		}
	}

	// Token: 0x170008FE RID: 2302
	// (get) Token: 0x0600131D RID: 4893 RVA: 0x00009B42 File Offset: 0x00007D42
	public float BaseCooldownTime
	{
		get
		{
			return this.AbilityData.CooldownTime;
		}
	}

	// Token: 0x170008FF RID: 2303
	// (get) Token: 0x0600131E RID: 4894 RVA: 0x00083FB8 File Offset: 0x000821B8
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

	// Token: 0x17000900 RID: 2304
	// (get) Token: 0x0600131F RID: 4895 RVA: 0x00009B4F File Offset: 0x00007D4F
	// (set) Token: 0x06001320 RID: 4896 RVA: 0x00009B57 File Offset: 0x00007D57
	public bool DisplayPausedAbilityCooldown { get; set; }

	// Token: 0x17000901 RID: 2305
	// (get) Token: 0x06001321 RID: 4897 RVA: 0x00009B60 File Offset: 0x00007D60
	public float LockoutTime
	{
		get
		{
			return this.AbilityData.LockoutTime;
		}
	}

	// Token: 0x17000902 RID: 2306
	// (get) Token: 0x06001322 RID: 4898 RVA: 0x00009B6D File Offset: 0x00007D6D
	// (set) Token: 0x06001323 RID: 4899 RVA: 0x00009B7A File Offset: 0x00007D7A
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

	// Token: 0x17000903 RID: 2307
	// (get) Token: 0x06001324 RID: 4900 RVA: 0x00009B88 File Offset: 0x00007D88
	// (set) Token: 0x06001325 RID: 4901 RVA: 0x00009B95 File Offset: 0x00007D95
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

	// Token: 0x17000904 RID: 2308
	// (get) Token: 0x06001326 RID: 4902 RVA: 0x00009BA3 File Offset: 0x00007DA3
	public bool GainManaWhenHit
	{
		get
		{
			return this.AbilityData.ManaGainPerHit;
		}
	}

	// Token: 0x17000905 RID: 2309
	// (get) Token: 0x06001327 RID: 4903 RVA: 0x00009BB0 File Offset: 0x00007DB0
	public bool CooldownRefreshesAllAmmo
	{
		get
		{
			return this.AbilityData.CooldownRefreshesAllAmmo;
		}
	}

	// Token: 0x17000906 RID: 2310
	// (get) Token: 0x06001328 RID: 4904 RVA: 0x00009BBD File Offset: 0x00007DBD
	public virtual bool LockDirectionWhenCasting
	{
		get
		{
			return !TraitManager.IsTraitActive(TraitType.DisableAttackLock) && this.AbilityData.LockDirection;
		}
	}

	// Token: 0x17000907 RID: 2311
	// (get) Token: 0x06001329 RID: 4905 RVA: 0x00009BD8 File Offset: 0x00007DD8
	public virtual float AirMovementMod
	{
		get
		{
			return this.AbilityData.AirMovementMod;
		}
	}

	// Token: 0x17000908 RID: 2312
	// (get) Token: 0x0600132A RID: 4906 RVA: 0x00009BE5 File Offset: 0x00007DE5
	public virtual float MovementMod
	{
		get
		{
			return this.AbilityData.MovementMod;
		}
	}

	// Token: 0x17000909 RID: 2313
	// (get) Token: 0x0600132B RID: 4907 RVA: 0x00009BF2 File Offset: 0x00007DF2
	public bool CanCastWhileDashing
	{
		get
		{
			return this.AbilityData.CanCastWhileDashing;
		}
	}

	// Token: 0x1700090A RID: 2314
	// (get) Token: 0x0600132C RID: 4908 RVA: 0x00009BFF File Offset: 0x00007DFF
	public bool WeaponInterruptable
	{
		get
		{
			return this.AbilityData.AttackCancel;
		}
	}

	// Token: 0x1700090B RID: 2315
	// (get) Token: 0x0600132D RID: 4909 RVA: 0x00009C0C File Offset: 0x00007E0C
	public bool SpellInterruptable
	{
		get
		{
			return this.AbilityData.SpellCancel;
		}
	}

	// Token: 0x1700090C RID: 2316
	// (get) Token: 0x0600132E RID: 4910 RVA: 0x00009C19 File Offset: 0x00007E19
	public bool TalentInterruptable
	{
		get
		{
			return this.AbilityData.TalentCancel;
		}
	}

	// Token: 0x1700090D RID: 2317
	// (get) Token: 0x0600132F RID: 4911 RVA: 0x00009C26 File Offset: 0x00007E26
	public bool DashInterruptable
	{
		get
		{
			return this.AbilityData.DashCancel;
		}
	}

	// Token: 0x1700090E RID: 2318
	// (get) Token: 0x06001330 RID: 4912 RVA: 0x00009C33 File Offset: 0x00007E33
	public virtual bool JumpInterruptable
	{
		get
		{
			return this.AbilityData.JumpCancel;
		}
	}

	// Token: 0x1700090F RID: 2319
	// (get) Token: 0x06001331 RID: 4913 RVA: 0x00009C40 File Offset: 0x00007E40
	public float CooldownTimer
	{
		get
		{
			return this.m_cooldownTimer;
		}
	}

	// Token: 0x17000910 RID: 2320
	// (get) Token: 0x06001332 RID: 4914 RVA: 0x00009C48 File Offset: 0x00007E48
	// (set) Token: 0x06001333 RID: 4915 RVA: 0x00009C50 File Offset: 0x00007E50
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

	// Token: 0x17000911 RID: 2321
	// (get) Token: 0x06001334 RID: 4916 RVA: 0x00009C59 File Offset: 0x00007E59
	// (set) Token: 0x06001335 RID: 4917 RVA: 0x000840C8 File Offset: 0x000822C8
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

	// Token: 0x17000912 RID: 2322
	// (get) Token: 0x06001336 RID: 4918 RVA: 0x00009C61 File Offset: 0x00007E61
	public bool IsOnCooldown
	{
		get
		{
			return this.CooldownTimer > 0f;
		}
	}

	// Token: 0x17000913 RID: 2323
	// (get) Token: 0x06001337 RID: 4919 RVA: 0x00009C70 File Offset: 0x00007E70
	public virtual AbilityData AbilityData
	{
		get
		{
			return this.m_abilityData;
		}
	}

	// Token: 0x17000914 RID: 2324
	// (get) Token: 0x06001338 RID: 4920 RVA: 0x00009C78 File Offset: 0x00007E78
	// (set) Token: 0x06001339 RID: 4921 RVA: 0x00009C80 File Offset: 0x00007E80
	public AbilityAnimState CurrentAbilityAnimState { get; protected set; }

	// Token: 0x0600133A RID: 4922 RVA: 0x00084158 File Offset: 0x00082358
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

	// Token: 0x0600133B RID: 4923 RVA: 0x0008423C File Offset: 0x0008243C
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

	// Token: 0x0600133C RID: 4924 RVA: 0x00002FCA File Offset: 0x000011CA
	public virtual void Reinitialize()
	{
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x00084338 File Offset: 0x00082538
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

	// Token: 0x0600133E RID: 4926 RVA: 0x000843C0 File Offset: 0x000825C0
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

	// Token: 0x0600133F RID: 4927 RVA: 0x00009C89 File Offset: 0x00007E89
	public void PerformAttackFlipCheck()
	{
		if (this.m_attackFlipCheckCoroutine != null)
		{
			base.StopCoroutine(this.m_attackFlipCheckCoroutine);
		}
		this.m_attackFlipCheckCoroutine = base.StartCoroutine(this.UpdateAttackFlipCheck());
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x000844FC File Offset: 0x000826FC
	private void OnEnemyHitCancelAttackFlip(object sender, EventArgs args)
	{
		CharacterHitEventArgs characterHitEventArgs = args as CharacterHitEventArgs;
		if (characterHitEventArgs != null && characterHitEventArgs.Attacker == this.m_firedProjectile && this.m_attackFlipCheckCoroutine != null)
		{
			base.StopCoroutine(this.m_attackFlipCheckCoroutine);
		}
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x00084538 File Offset: 0x00082738
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

	// Token: 0x06001342 RID: 4930 RVA: 0x00009CB1 File Offset: 0x00007EB1
	protected void CancelChangeAnimCoroutine()
	{
		this.m_animator.ResetTrigger("Change_Ability_Anim");
		if (this.m_changeAnimCoroutine != null)
		{
			base.StopCoroutine(this.m_changeAnimCoroutine);
		}
	}

	// Token: 0x06001343 RID: 4931 RVA: 0x0008461C File Offset: 0x0008281C
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

	// Token: 0x06001344 RID: 4932 RVA: 0x00009CD7 File Offset: 0x00007ED7
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

	// Token: 0x06001345 RID: 4933 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void OnEnterTellIntroLogic()
	{
	}

	// Token: 0x06001346 RID: 4934 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void OnEnterTellLogic()
	{
	}

	// Token: 0x06001347 RID: 4935 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void OnEnterAttackIntroLogic()
	{
	}

	// Token: 0x06001348 RID: 4936 RVA: 0x00009D16 File Offset: 0x00007F16
	protected virtual void OnEnterAttackLogic()
	{
		this.m_abilityController.BroadcastAbilityCastEvents(this.CastAbilityType);
		this.FireProjectile();
		this.StartCooldownTimer();
	}

	// Token: 0x06001349 RID: 4937 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void OnEnterExitLogic()
	{
	}

	// Token: 0x0600134A RID: 4938 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void OnExitTellIntroLogic()
	{
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void OnExitTellLogic()
	{
	}

	// Token: 0x0600134C RID: 4940 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void OnExitAttackIntroLogic()
	{
	}

	// Token: 0x0600134D RID: 4941 RVA: 0x000846D4 File Offset: 0x000828D4
	protected virtual void OnExitAttackLogic()
	{
		if (!this.m_animator)
		{
			throw new Exception("ability animator should not be null.  AbilityName: " + base.name + " WasDestroyed: " + (!this).ToString());
		}
		this.m_animator.SetFloat("NaturalWalk", 0f);
	}

	// Token: 0x0600134E RID: 4942 RVA: 0x00009D35 File Offset: 0x00007F35
	protected virtual void OnExitExitLogic()
	{
		this.IsAnimationComplete = true;
	}

	// Token: 0x0600134F RID: 4943 RVA: 0x00009D3E File Offset: 0x00007F3E
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

	// Token: 0x06001350 RID: 4944 RVA: 0x00084730 File Offset: 0x00082930
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

	// Token: 0x06001351 RID: 4945 RVA: 0x00009D54 File Offset: 0x00007F54
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

	// Token: 0x06001352 RID: 4946 RVA: 0x000847B8 File Offset: 0x000829B8
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

	// Token: 0x06001353 RID: 4947 RVA: 0x000848D8 File Offset: 0x00082AD8
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

	// Token: 0x06001354 RID: 4948 RVA: 0x00009D63 File Offset: 0x00007F63
	protected void CriticalHitTimingDisplay()
	{
		if (this.IsInCritWindow && !this.m_critDisplayOn && this.m_critEffectGO)
		{
			base.StartCoroutine(this.CritWindowCoroutine());
		}
	}

	// Token: 0x06001355 RID: 4949 RVA: 0x00009D8F File Offset: 0x00007F8F
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

	// Token: 0x06001356 RID: 4950 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void PlayCritWindowAudio()
	{
	}

	// Token: 0x06001357 RID: 4951 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void PlayDashCritAudio()
	{
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x00009D9E File Offset: 0x00007F9E
	protected virtual void PlayFreeCritReleaseAudio()
	{
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Weapons/sfx_weapon_crit_charged_release", this.m_abilityController.gameObject);
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x00084950 File Offset: 0x00082B50
	protected virtual void FireProjectile()
	{
		if (!string.IsNullOrEmpty(this.ProjectileName))
		{
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, true, 0f, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			this.ApplyAbilityCosts();
		}
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x000849B4 File Offset: 0x00082BB4
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

	// Token: 0x0600135B RID: 4955 RVA: 0x00084A94 File Offset: 0x00082C94
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

	// Token: 0x0600135C RID: 4956 RVA: 0x00084B30 File Offset: 0x00082D30
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

	// Token: 0x0600135D RID: 4957 RVA: 0x00084BEC File Offset: 0x00082DEC
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

	// Token: 0x0600135E RID: 4958 RVA: 0x00009DB6 File Offset: 0x00007FB6
	protected void StartLockoutTimer()
	{
		this.m_lockoutTimer = this.LockoutTime;
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x00084C20 File Offset: 0x00082E20
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

	// Token: 0x06001360 RID: 4960 RVA: 0x00009DC4 File Offset: 0x00007FC4
	protected void OnProjectileHitDisableAttackFlip(Projectile_RL projectile, GameObject colliderObj)
	{
		this.m_enemyHit = true;
	}

	// Token: 0x06001361 RID: 4961 RVA: 0x00084EE8 File Offset: 0x000830E8
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

	// Token: 0x06001362 RID: 4962 RVA: 0x00009DCD File Offset: 0x00007FCD
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

	// Token: 0x06001363 RID: 4963 RVA: 0x00084FA8 File Offset: 0x000831A8
	public void ReduceFlatCooldown(int flatAmount)
	{
		for (int i = 0; i < flatAmount; i++)
		{
			this.EndCooldownTimer(true);
		}
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x00009DFE File Offset: 0x00007FFE
	protected void IncreaseManaEvent(Projectile_RL projectile, GameObject colliderObj)
	{
		bool isActiveAndEnabled = projectile.isActiveAndEnabled;
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x00009E07 File Offset: 0x00008007
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

	// Token: 0x06001366 RID: 4966 RVA: 0x00084FC8 File Offset: 0x000831C8
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

	// Token: 0x06001367 RID: 4967 RVA: 0x00085074 File Offset: 0x00083274
	protected bool IsAttackFlipValid(global::PlayerController playerController, bool collidingBack, bool collidingFront)
	{
		float axis = Rewired_RL.Player.GetAxis("MoveHorizontal");
		return (axis < 0f && playerController.IsFacingRight && collidingBack && !collidingFront) || (axis > 0f && !playerController.IsFacingRight && !collidingBack && collidingFront);
	}

	// Token: 0x06001368 RID: 4968 RVA: 0x000850C4 File Offset: 0x000832C4
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

	// Token: 0x06001369 RID: 4969 RVA: 0x00085188 File Offset: 0x00083388
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

	// Token: 0x0600136A RID: 4970 RVA: 0x00009E16 File Offset: 0x00008016
	private void LateUpdate()
	{
		if (this.m_triggerBounce)
		{
			this.TriggerBounce();
		}
	}

	// Token: 0x0600136B RID: 4971 RVA: 0x00085258 File Offset: 0x00083458
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

	// Token: 0x0600136C RID: 4972 RVA: 0x00002FCA File Offset: 0x000011CA
	public virtual void OnPreDestroy()
	{
	}

	// Token: 0x0400156B RID: 5483
	protected const string CHANGE_ABILITY_ANIM_PARAM = "Change_Ability_Anim";

	// Token: 0x0400156C RID: 5484
	[SerializeField]
	private AbilityType m_abilityType = AbilityType.FireballSpell;

	// Token: 0x0400156D RID: 5485
	[Header("Ability Data")]
	[SerializeField]
	protected AbilityData m_abilityData;

	// Token: 0x0400156E RID: 5486
	[SerializeField]
	[Tooltip("This field does not affect the actual damage of the ability. It's just used to know if it can be cast with the Pacifist trait active.")]
	protected bool m_dealsNoDamage;

	// Token: 0x0400156F RID: 5487
	[SerializeField]
	protected string m_abilityTellIntroName;

	// Token: 0x04001570 RID: 5488
	[SerializeField]
	protected Vector2 m_critHitTimingWindow;

	// Token: 0x04001571 RID: 5489
	[SerializeField]
	protected Vector2 m_critEffectOffset;

	// Token: 0x04001572 RID: 5490
	[SerializeField]
	protected bool m_critsWhenDashing;

	// Token: 0x04001573 RID: 5491
	[SerializeField]
	protected bool m_disableAbilityQueuing;

	// Token: 0x04001574 RID: 5492
	[Header("Projectile Data")]
	[SerializeField]
	protected string m_projectileName;

	// Token: 0x04001575 RID: 5493
	[SerializeField]
	protected Vector2 m_projectileOffset;

	// Token: 0x04001576 RID: 5494
	[SerializeField]
	protected bool m_hasAttackFlipCheck;

	// Token: 0x04001577 RID: 5495
	protected bool m_consumedSpellOrb;

	// Token: 0x04001578 RID: 5496
	protected CastAbility_RL m_abilityController;

	// Token: 0x04001579 RID: 5497
	protected float m_cooldownTimer;

	// Token: 0x0400157A RID: 5498
	protected float m_lockoutTimer;

	// Token: 0x0400157B RID: 5499
	protected int m_currentAmmo;

	// Token: 0x0400157C RID: 5500
	protected Projectile_RL m_firedProjectile;

	// Token: 0x0400157D RID: 5501
	protected bool m_hasAnimation;

	// Token: 0x0400157E RID: 5502
	protected Animator m_animator;

	// Token: 0x0400157F RID: 5503
	private CooldownEventArgs m_cooldownEventArgs;

	// Token: 0x04001580 RID: 5504
	private AbilityCooldownOverEventArgs m_cooldownOverEventArgs;

	// Token: 0x04001581 RID: 5505
	private Coroutine m_attackFlipCheckCoroutine;

	// Token: 0x04001582 RID: 5506
	private float m_attackFlipCheckTimer;

	// Token: 0x04001583 RID: 5507
	private bool m_enemyHit;

	// Token: 0x04001584 RID: 5508
	private bool m_isAnimationComplete;

	// Token: 0x04001585 RID: 5509
	protected Coroutine m_changeAnimCoroutine;

	// Token: 0x04001586 RID: 5510
	private PlayerAmmoChangeEventArgs m_ammoChangeEventArgs;

	// Token: 0x04001587 RID: 5511
	private RaycastHit2D[] m_attackFlipRaycastArray;

	// Token: 0x04001588 RID: 5512
	protected float m_highestBounceAmount;

	// Token: 0x04001589 RID: 5513
	protected bool m_triggerBounce;

	// Token: 0x0400158A RID: 5514
	private string m_abilityNameCasted;

	// Token: 0x0400158B RID: 5515
	protected float m_abilityCastStartTime;

	// Token: 0x0400158C RID: 5516
	private Projectile_RL m_persistentCollListenerProj;

	// Token: 0x0400158D RID: 5517
	protected GameObject m_critEffectGO;

	// Token: 0x0400158E RID: 5518
	private Action<Projectile_RL, GameObject> m_reduceCooldownEvent;

	// Token: 0x0400158F RID: 5519
	private Action<Projectile_RL, GameObject> m_increaseManaEvent;

	// Token: 0x04001590 RID: 5520
	private Action<Projectile_RL, GameObject> m_onProjectileHitDisableAttackFlip;

	// Token: 0x04001591 RID: 5521
	protected Action<Projectile_RL, GameObject> m_bounce;

	// Token: 0x04001592 RID: 5522
	private Action<AbilityAnimState> m_exitAnimationState;

	// Token: 0x04001593 RID: 5523
	private Action<AbilityAnimState> m_enterAnimationState;

	// Token: 0x04001594 RID: 5524
	private Action<object, EventArgs> m_onEnemyHitCancelAttackFlip;

	// Token: 0x04001595 RID: 5525
	protected Relay m_beginCastingRelay = new Relay();

	// Token: 0x04001596 RID: 5526
	protected Relay m_stopCastingRelay = new Relay();

	// Token: 0x04001598 RID: 5528
	private Relay<object, CooldownEventArgs> m_onCooldownRelay = new Relay<object, CooldownEventArgs>();

	// Token: 0x0400159A RID: 5530
	[NonSerialized]
	protected string[] m_projectileNameArray;

	// Token: 0x0400159F RID: 5535
	private bool m_critDisplayOn;
}

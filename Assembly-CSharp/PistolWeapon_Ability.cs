using System;
using FMODUnity;
using RL_Windows;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public class PistolWeapon_Ability : AimedAbility_RL, IAttack, IAbility
{
	// Token: 0x170008FD RID: 2301
	// (get) Token: 0x0600103D RID: 4157 RVA: 0x0002F36E File Offset: 0x0002D56E
	public BaseAbility_RL ReloadAbility
	{
		get
		{
			return this.m_reloadAbility;
		}
	}

	// Token: 0x170008FE RID: 2302
	// (get) Token: 0x0600103E RID: 4158 RVA: 0x0002F376 File Offset: 0x0002D576
	public override Vector2 PushbackAmount
	{
		get
		{
			return this.BowPushbackAmount;
		}
	}

	// Token: 0x170008FF RID: 2303
	// (get) Token: 0x0600103F RID: 4159 RVA: 0x0002F37E File Offset: 0x0002D57E
	protected virtual Vector2 BowPushbackAmount
	{
		get
		{
			if (base.CurrentAmmo > 0)
			{
				return this.m_bowPushbackAmount;
			}
			return this.m_bowPushbackAmountNoAmmo;
		}
	}

	// Token: 0x17000900 RID: 2304
	// (get) Token: 0x06001040 RID: 4160 RVA: 0x0002F396 File Offset: 0x0002D596
	public override string ProjectileName
	{
		get
		{
			if (base.CurrentAmmo > 10)
			{
				return base.ProjectileName;
			}
			if (base.CurrentAmmo > 0)
			{
				return this.m_critHitProjectileName;
			}
			return this.m_emptyClipProjectileName;
		}
	}

	// Token: 0x17000901 RID: 2305
	// (get) Token: 0x06001041 RID: 4161 RVA: 0x0002F3BF File Offset: 0x0002D5BF
	protected override bool CancelTimeSlowOnFire
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001042 RID: 4162 RVA: 0x0002F3C2 File Offset: 0x0002D5C2
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_critHitProjectileName,
			this.m_emptyClipProjectileName
		};
	}

	// Token: 0x17000902 RID: 2306
	// (get) Token: 0x06001043 RID: 4163 RVA: 0x0002F3EB File Offset: 0x0002D5EB
	protected virtual bool CanManuallyReload
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001044 RID: 4164 RVA: 0x0002F3F0 File Offset: 0x0002D5F0
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		if (this.CanManuallyReload)
		{
			this.m_reloadAbility = AbilityLibrary.GetAbility(AbilityType.ReloadTalent);
			this.m_reloadAbility = UnityEngine.Object.Instantiate<BaseAbility_RL>(this.m_reloadAbility);
			this.m_reloadAbility.transform.SetParent(base.transform);
			this.m_reloadAbility.Initialize(abilityController, castAbilityType);
		}
	}

	// Token: 0x17000903 RID: 2307
	// (get) Token: 0x06001045 RID: 4165 RVA: 0x0002F451 File Offset: 0x0002D651
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3.25f;
		}
	}

	// Token: 0x17000904 RID: 2308
	// (get) Token: 0x06001046 RID: 4166 RVA: 0x0002F458 File Offset: 0x0002D658
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000905 RID: 2309
	// (get) Token: 0x06001047 RID: 4167 RVA: 0x0002F45F File Offset: 0x0002D65F
	protected override float TellAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000906 RID: 2310
	// (get) Token: 0x06001048 RID: 4168 RVA: 0x0002F466 File Offset: 0x0002D666
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000907 RID: 2311
	// (get) Token: 0x06001049 RID: 4169 RVA: 0x0002F46D File Offset: 0x0002D66D
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000908 RID: 2312
	// (get) Token: 0x0600104A RID: 4170 RVA: 0x0002F474 File Offset: 0x0002D674
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000909 RID: 2313
	// (get) Token: 0x0600104B RID: 4171 RVA: 0x0002F47B File Offset: 0x0002D67B
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700090A RID: 2314
	// (get) Token: 0x0600104C RID: 4172 RVA: 0x0002F482 File Offset: 0x0002D682
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700090B RID: 2315
	// (get) Token: 0x0600104D RID: 4173 RVA: 0x0002F489 File Offset: 0x0002D689
	protected override float ExitAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x1700090C RID: 2316
	// (get) Token: 0x0600104E RID: 4174 RVA: 0x0002F490 File Offset: 0x0002D690
	protected override float ExitAnimExitDelay
	{
		get
		{
			if (SaveManager.PlayerSaveData.GetRelic(RelicType.AttackCooldown).Level > 0)
			{
				return (float)SaveManager.PlayerSaveData.GetRelic(RelicType.AttackCooldown).Level * 0.125f;
			}
			return 0f;
		}
	}

	// Token: 0x0600104F RID: 4175 RVA: 0x0002F4CC File Offset: 0x0002D6CC
	protected override void UpdateAimLine()
	{
		Vector3 localEulerAngles = this.m_aimSprite.transform.localEulerAngles;
		localEulerAngles.z = this.m_unmoddedAngle;
		this.m_aimSprite.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06001050 RID: 4176 RVA: 0x0002F508 File Offset: 0x0002D708
	protected override void Update()
	{
		if (!base.AbilityActive && Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
		{
			this.m_abilityController.StartAbility(base.CastAbilityType, false, false);
		}
		base.Update();
		if (this.CanManuallyReload && !Interactable.PlayerIsInteracting && !base.AbilityActive && Rewired_RL.Player.GetButtonDown("Interact") && this.m_reloadAbility && this.m_abilityController.IsAbilityPermitted(this.m_reloadAbility))
		{
			this.m_abilityController.StartAbility(CastAbilityType.Weapon, true, false);
		}
		if (this.m_aimSprite)
		{
			if (!this.m_aimSpriteInitialized)
			{
				this.m_aimSprite.transform.localScale = new Vector3(1f / this.m_abilityController.PlayerController.transform.lossyScale.x, 1f / this.m_abilityController.PlayerController.transform.lossyScale.y, 1f);
				float num = this.m_abilityController.PlayerController.transform.lossyScale.x / this.m_abilityController.PlayerController.BaseScaleToOffsetWith;
				Vector2 v = new Vector2(0f, this.m_projectileOffset.y * num);
				this.m_aimSprite.transform.position = this.m_abilityController.transform.localPosition + v;
				this.m_aimSpriteInitialized = true;
			}
			if ((this.m_abilityController.AnyAbilityInProgress && !this.m_abilityController.AbilityInProgress(CastAbilityType.Weapon)) || WindowManager.IsAnyWindowOpen || GameManager.IsGamePaused || (!GameManager.IsGamePaused && !RewiredMapController.IsCurrentMapEnabled))
			{
				if (this.m_aimSprite.gameObject.activeSelf)
				{
					this.m_aimSprite.gameObject.SetActive(false);
				}
			}
			else if (!this.m_aimSprite.gameObject.activeSelf)
			{
				this.m_aimSprite.gameObject.SetActive(true);
			}
			if (!base.AbilityActive)
			{
				this.UpdateArrowAim(true);
				this.UpdateAimLine();
			}
		}
	}

	// Token: 0x06001051 RID: 4177 RVA: 0x0002F729 File Offset: 0x0002D929
	protected void Base_FireProjectile()
	{
		base.FireProjectile();
	}

	// Token: 0x06001052 RID: 4178 RVA: 0x0002F734 File Offset: 0x0002D934
	protected override void FireProjectile()
	{
		if (base.CurrentAmmo > 0)
		{
			this.m_shootAudioEmitter.Play();
		}
		else
		{
			this.m_emptyClipAudioEmitter.Play();
		}
		if (base.CurrentAmmo <= 10)
		{
			this.m_shootAudioEmitter.SetParameter("ammoFull", 0.1f, false);
		}
		base.FireProjectile();
		if (base.CurrentAmmo <= 0)
		{
			this.m_firedProjectile.RemoveAllStatusEffects();
		}
		if (base.CurrentAmmo < 10)
		{
			this.m_firedProjectile.ActualCritChance += 100f;
		}
		int level = SaveManager.PlayerSaveData.GetRelic(RelicType.AttackCooldown).Level;
		if (level > 0)
		{
			this.m_firedProjectile.LifespanTimer += (float)level * 0f;
		}
	}

	// Token: 0x040011B1 RID: 4529
	[SerializeField]
	private GameObject m_aimSprite;

	// Token: 0x040011B2 RID: 4530
	[SerializeField]
	protected StudioEventEmitter m_shootAudioEmitter;

	// Token: 0x040011B3 RID: 4531
	[SerializeField]
	protected StudioEventEmitter m_emptyClipAudioEmitter;

	// Token: 0x040011B4 RID: 4532
	[SerializeField]
	protected string m_emptyClipProjectileName;

	// Token: 0x040011B5 RID: 4533
	[SerializeField]
	private string m_critHitProjectileName;

	// Token: 0x040011B6 RID: 4534
	private BaseAbility_RL m_reloadAbility;

	// Token: 0x040011B7 RID: 4535
	private const float LOW_AMMO_AUDIO_PARAMETER_VALUE = 0.1f;

	// Token: 0x040011B8 RID: 4536
	private bool m_aimSpriteInitialized;

	// Token: 0x040011B9 RID: 4537
	private Vector2 m_bowPushbackAmount = new Vector2(0f, 3.25f);

	// Token: 0x040011BA RID: 4538
	private Vector2 m_bowPushbackAmountNoAmmo = new Vector2(0f, 2.4f);
}

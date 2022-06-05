using System;
using System.Collections;
using Rewired;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class Cannon_Ability : AimedAbilityFast_RL, IAttack, IAbility
{
	// Token: 0x06000F1C RID: 3868 RVA: 0x0002CFD5 File Offset: 0x0002B1D5
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_shootProjectileName
		};
	}

	// Token: 0x17000844 RID: 2116
	// (get) Token: 0x06000F1D RID: 3869 RVA: 0x0002CFF5 File Offset: 0x0002B1F5
	// (set) Token: 0x06000F1E RID: 3870 RVA: 0x0002CFFD File Offset: 0x0002B1FD
	public bool IsShooting { get; private set; }

	// Token: 0x17000845 RID: 2117
	// (get) Token: 0x06000F1F RID: 3871 RVA: 0x0002D006 File Offset: 0x0002B206
	public override Vector2 PushbackAmount
	{
		get
		{
			if (!this.IsShooting)
			{
				return Vector2.zero;
			}
			return base.PushbackAmount;
		}
	}

	// Token: 0x17000846 RID: 2118
	// (get) Token: 0x06000F20 RID: 3872 RVA: 0x0002D01C File Offset: 0x0002B21C
	protected override float GravityReduction
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000847 RID: 2119
	// (get) Token: 0x06000F21 RID: 3873 RVA: 0x0002D023 File Offset: 0x0002B223
	protected override float TellIntroAnimSpeed
	{
		get
		{
			if (this.IsShooting)
			{
				return this.ShootTellIntroAnimSpeed;
			}
			return this.MeleeTellIntroAnimSpeed;
		}
	}

	// Token: 0x17000848 RID: 2120
	// (get) Token: 0x06000F22 RID: 3874 RVA: 0x0002D03A File Offset: 0x0002B23A
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			if (this.IsShooting)
			{
				return this.ShootTellIntroAnimExitDelay;
			}
			return this.MeleeTellIntroAnimExitDelay;
		}
	}

	// Token: 0x17000849 RID: 2121
	// (get) Token: 0x06000F23 RID: 3875 RVA: 0x0002D051 File Offset: 0x0002B251
	protected override float TellAnimSpeed
	{
		get
		{
			if (this.IsShooting)
			{
				return this.ShootTellAnimSpeed;
			}
			return this.MeleeTellAnimSpeed;
		}
	}

	// Token: 0x1700084A RID: 2122
	// (get) Token: 0x06000F24 RID: 3876 RVA: 0x0002D068 File Offset: 0x0002B268
	protected override float TellAnimExitDelay
	{
		get
		{
			if (this.IsShooting)
			{
				return this.ShootTellAnimExitDelay;
			}
			return this.MeleeTellAnimExitDelay;
		}
	}

	// Token: 0x1700084B RID: 2123
	// (get) Token: 0x06000F25 RID: 3877 RVA: 0x0002D07F File Offset: 0x0002B27F
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			if (this.IsShooting)
			{
				return this.ShootAttackIntroAnimSpeed;
			}
			return this.MeleeAttackIntroAnimSpeed;
		}
	}

	// Token: 0x1700084C RID: 2124
	// (get) Token: 0x06000F26 RID: 3878 RVA: 0x0002D096 File Offset: 0x0002B296
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			if (this.IsShooting)
			{
				return this.ShootAttackIntroAnimExitDelay;
			}
			return this.MeleeAttackIntroAnimExitDelay;
		}
	}

	// Token: 0x1700084D RID: 2125
	// (get) Token: 0x06000F27 RID: 3879 RVA: 0x0002D0AD File Offset: 0x0002B2AD
	protected override float AttackAnimSpeed
	{
		get
		{
			if (this.IsShooting)
			{
				return this.ShootAttackAnimSpeed;
			}
			return this.MeleeAttackAnimSpeed;
		}
	}

	// Token: 0x1700084E RID: 2126
	// (get) Token: 0x06000F28 RID: 3880 RVA: 0x0002D0C4 File Offset: 0x0002B2C4
	protected override float AttackAnimExitDelay
	{
		get
		{
			if (this.IsShooting)
			{
				return this.ShootAttackAnimExitDelay;
			}
			return this.MeleeAttackAnimExitDelay;
		}
	}

	// Token: 0x1700084F RID: 2127
	// (get) Token: 0x06000F29 RID: 3881 RVA: 0x0002D0DB File Offset: 0x0002B2DB
	protected override float ExitAnimSpeed
	{
		get
		{
			if (this.IsShooting)
			{
				return this.ShootExitAnimSpeed;
			}
			return this.MeleeExitAnimSpeed;
		}
	}

	// Token: 0x17000850 RID: 2128
	// (get) Token: 0x06000F2A RID: 3882 RVA: 0x0002D0F2 File Offset: 0x0002B2F2
	protected override float ExitAnimExitDelay
	{
		get
		{
			if (this.IsShooting)
			{
				return this.ShootExitAnimExitDelay;
			}
			return this.MeleeExitAnimExitDelay;
		}
	}

	// Token: 0x17000851 RID: 2129
	// (get) Token: 0x06000F2B RID: 3883 RVA: 0x0002D109 File Offset: 0x0002B309
	public override bool HasAttackFlipCheck
	{
		get
		{
			return !this.IsShooting && base.HasAttackFlipCheck;
		}
	}

	// Token: 0x17000852 RID: 2130
	// (get) Token: 0x06000F2C RID: 3884 RVA: 0x0002D11B File Offset: 0x0002B31B
	public override string ProjectileName
	{
		get
		{
			if (this.IsShooting)
			{
				return this.m_shootProjectileName;
			}
			return base.ProjectileName;
		}
	}

	// Token: 0x17000853 RID: 2131
	// (get) Token: 0x06000F2D RID: 3885 RVA: 0x0002D132 File Offset: 0x0002B332
	public override Vector2 ProjectileOffset
	{
		get
		{
			if (this.IsShooting)
			{
				return this.m_shootProjectileOffset;
			}
			return base.ProjectileOffset;
		}
	}

	// Token: 0x17000854 RID: 2132
	// (get) Token: 0x06000F2E RID: 3886 RVA: 0x0002D149 File Offset: 0x0002B349
	public override AbilityData AbilityData
	{
		get
		{
			if (this.IsShooting)
			{
				return this.m_shootAbilityData;
			}
			return base.AbilityData;
		}
	}

	// Token: 0x17000855 RID: 2133
	// (get) Token: 0x06000F2F RID: 3887 RVA: 0x0002D160 File Offset: 0x0002B360
	public override string AbilityTellIntroName
	{
		get
		{
			if (this.IsShooting)
			{
				return this.m_shootTellIntroName;
			}
			return base.AbilityTellIntroName;
		}
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x0002D177 File Offset: 0x0002B377
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		abilityController.Animator.SetBool("FryingPan_UsePirateAudio", true);
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x0002D194 File Offset: 0x0002B394
	protected override void InitializeAimLine()
	{
		Projectile_RL projectile = ProjectileLibrary.GetProjectile(this.m_shootProjectileName);
		if (projectile)
		{
			this.m_projFallMultiplier = projectile.FallMultiplierOverride;
			this.m_projGravityKickInDelay = projectile.GravityKickInDelay;
			this.m_projSpeed = projectile.ProjectileData.Speed;
			this.m_projIsWeighted = projectile.IsWeighted;
		}
		if (this.m_aimLine)
		{
			Color startColor = new Color(1f, 1f, 1f, 0.25f);
			this.m_aimLine.startColor = startColor;
			this.m_aimLine.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000F32 RID: 3890 RVA: 0x0002D22F File Offset: 0x0002B42F
	public override void PreCastAbility()
	{
		this.IsShooting = this.m_shootButtonHeld;
		this.m_shootButtonReleased = false;
		base.PreCastAbility();
		if (!this.IsShooting)
		{
			this.m_aimLine.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x0002D263 File Offset: 0x0002B463
	protected override void OnEnterAttackLogic()
	{
		this.m_abilityController.BroadcastAbilityCastEvents(base.CastAbilityType);
		this.FireProjectile();
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x0002D27C File Offset: 0x0002B47C
	protected override void FireProjectile()
	{
		base.FireProjectile();
		if (this.IsShooting)
		{
			Vector3 localPosition = this.m_firedProjectile.transform.localPosition;
			Vector3 vector = localPosition;
			Vector3 lossyScale = base.transform.lossyScale;
			vector.x += 1.1f * lossyScale.x;
			vector = CDGHelper.RotatedPoint(vector, localPosition, this.m_unmoddedAngle);
			vector.z = localPosition.z;
			EffectManager.PlayEffect(this.m_firedProjectile.gameObject, null, "CannonBallMuzzle_Effect", vector, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None).transform.localScale = lossyScale;
			this.StartCooldownTimer();
			if (this.m_firedProjectile)
			{
				GameObject gameObject = this.m_firedProjectile.gameObject.FindObjectReference("CriticalEffects");
				if (gameObject)
				{
					if (this.m_firedProjectile.ActualCritChance >= 100f)
					{
						gameObject.SetActive(true);
						return;
					}
					gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06000F35 RID: 3893 RVA: 0x0002D375 File Offset: 0x0002B575
	protected override void UpdateArrowAim(bool doNotUpdatePlayerAnims = false)
	{
		if (this.IsShooting)
		{
			base.UpdateArrowAim(doNotUpdatePlayerAnims);
		}
	}

	// Token: 0x06000F36 RID: 3894 RVA: 0x0002D386 File Offset: 0x0002B586
	protected override IEnumerator ChangeAnim(float duration)
	{
		if (!this.m_shootButtonHeld)
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
			base.PerformTurnAnimCheck();
		}
		else
		{
			yield return base.ChangeAnim(duration);
		}
		yield break;
	}

	// Token: 0x06000F37 RID: 3895 RVA: 0x0002D39C File Offset: 0x0002B59C
	public override void StopAbility(bool abilityInterrupted)
	{
		base.StopAbility(abilityInterrupted);
		if (this.IsShooting)
		{
			this.IsShooting = false;
		}
	}

	// Token: 0x06000F38 RID: 3896 RVA: 0x0002D3B4 File Offset: 0x0002B5B4
	protected override void Update()
	{
		if (ReInput.isReady && !GameManager.IsGamePaused && !this.m_shootButtonReleased && !Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
		{
			this.m_shootButtonReleased = true;
			this.m_shootButtonHeld = false;
			this.StartCooldownTimer();
		}
		if (!base.AbilityActive)
		{
			base.Update();
			return;
		}
		if (this.m_shootButtonHeld)
		{
			base.Update();
			return;
		}
		if (!this.m_shootButtonReleased && !this.IsShooting && base.CurrentAbilityAnimState > AbilityAnimState.Attack)
		{
			this.m_shootButtonHeld = true;
			this.m_abilityController.StartAbility(base.CastAbilityType, false, false);
		}
		if (base.DecreaseCooldownOverTime && base.CooldownTimer > 0f)
		{
			this.m_cooldownTimer -= Time.deltaTime;
			if (base.CooldownTimer <= 0f)
			{
				base.EndCooldownTimer(true);
			}
		}
		if (base.LockoutTimer > 0f)
		{
			this.m_lockoutTimer -= Time.deltaTime;
		}
		if (base.AbilityActive)
		{
			base.CriticalHitTimingDisplay();
		}
	}

	// Token: 0x0400114D RID: 4429
	[Header("Shoot Values")]
	[SerializeField]
	private string m_shootTellIntroName;

	// Token: 0x0400114E RID: 4430
	[SerializeField]
	protected AbilityData m_shootAbilityData;

	// Token: 0x0400114F RID: 4431
	[SerializeField]
	protected string m_shootProjectileName;

	// Token: 0x04001150 RID: 4432
	[SerializeField]
	protected Vector2 m_shootProjectileOffset;

	// Token: 0x04001152 RID: 4434
	private float MeleeTellIntroAnimSpeed = 3f;

	// Token: 0x04001153 RID: 4435
	private float MeleeTellIntroAnimExitDelay;

	// Token: 0x04001154 RID: 4436
	private float MeleeTellAnimSpeed = 1.5f;

	// Token: 0x04001155 RID: 4437
	private float MeleeTellAnimExitDelay;

	// Token: 0x04001156 RID: 4438
	private float MeleeAttackIntroAnimSpeed = 1.35f;

	// Token: 0x04001157 RID: 4439
	private float MeleeAttackIntroAnimExitDelay;

	// Token: 0x04001158 RID: 4440
	private float MeleeAttackAnimSpeed = 1.25f;

	// Token: 0x04001159 RID: 4441
	private float MeleeAttackAnimExitDelay;

	// Token: 0x0400115A RID: 4442
	private float MeleeExitAnimSpeed = 1.25f;

	// Token: 0x0400115B RID: 4443
	private float MeleeExitAnimExitDelay;

	// Token: 0x0400115C RID: 4444
	private float ShootTellIntroAnimSpeed = 3f;

	// Token: 0x0400115D RID: 4445
	private float ShootTellIntroAnimExitDelay;

	// Token: 0x0400115E RID: 4446
	private float ShootTellAnimSpeed = 1.5f;

	// Token: 0x0400115F RID: 4447
	private float ShootTellAnimExitDelay;

	// Token: 0x04001160 RID: 4448
	private float ShootAttackIntroAnimSpeed = 1.25f;

	// Token: 0x04001161 RID: 4449
	private float ShootAttackIntroAnimExitDelay;

	// Token: 0x04001162 RID: 4450
	private float ShootAttackAnimSpeed = 1.25f;

	// Token: 0x04001163 RID: 4451
	private float ShootAttackAnimExitDelay;

	// Token: 0x04001164 RID: 4452
	private float ShootExitAnimSpeed = 1.25f;

	// Token: 0x04001165 RID: 4453
	private float ShootExitAnimExitDelay;

	// Token: 0x04001166 RID: 4454
	private bool m_shootButtonHeld;

	// Token: 0x04001167 RID: 4455
	private bool m_shootButtonReleased;
}

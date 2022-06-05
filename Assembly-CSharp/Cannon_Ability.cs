using System;
using System.Collections;
using Rewired;
using UnityEngine;

// Token: 0x020002EA RID: 746
public class Cannon_Ability : AimedAbilityFast_RL, IAttack, IAbility
{
	// Token: 0x060016EB RID: 5867 RVA: 0x0000B916 File Offset: 0x00009B16
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_shootProjectileName
		};
	}

	// Token: 0x17000ADC RID: 2780
	// (get) Token: 0x060016EC RID: 5868 RVA: 0x0000B936 File Offset: 0x00009B36
	// (set) Token: 0x060016ED RID: 5869 RVA: 0x0000B93E File Offset: 0x00009B3E
	public bool IsShooting { get; private set; }

	// Token: 0x17000ADD RID: 2781
	// (get) Token: 0x060016EE RID: 5870 RVA: 0x0000B947 File Offset: 0x00009B47
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

	// Token: 0x17000ADE RID: 2782
	// (get) Token: 0x060016EF RID: 5871 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float GravityReduction
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000ADF RID: 2783
	// (get) Token: 0x060016F0 RID: 5872 RVA: 0x0000B95D File Offset: 0x00009B5D
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

	// Token: 0x17000AE0 RID: 2784
	// (get) Token: 0x060016F1 RID: 5873 RVA: 0x0000B974 File Offset: 0x00009B74
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

	// Token: 0x17000AE1 RID: 2785
	// (get) Token: 0x060016F2 RID: 5874 RVA: 0x0000B98B File Offset: 0x00009B8B
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

	// Token: 0x17000AE2 RID: 2786
	// (get) Token: 0x060016F3 RID: 5875 RVA: 0x0000B9A2 File Offset: 0x00009BA2
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

	// Token: 0x17000AE3 RID: 2787
	// (get) Token: 0x060016F4 RID: 5876 RVA: 0x0000B9B9 File Offset: 0x00009BB9
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

	// Token: 0x17000AE4 RID: 2788
	// (get) Token: 0x060016F5 RID: 5877 RVA: 0x0000B9D0 File Offset: 0x00009BD0
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

	// Token: 0x17000AE5 RID: 2789
	// (get) Token: 0x060016F6 RID: 5878 RVA: 0x0000B9E7 File Offset: 0x00009BE7
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

	// Token: 0x17000AE6 RID: 2790
	// (get) Token: 0x060016F7 RID: 5879 RVA: 0x0000B9FE File Offset: 0x00009BFE
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

	// Token: 0x17000AE7 RID: 2791
	// (get) Token: 0x060016F8 RID: 5880 RVA: 0x0000BA15 File Offset: 0x00009C15
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

	// Token: 0x17000AE8 RID: 2792
	// (get) Token: 0x060016F9 RID: 5881 RVA: 0x0000BA2C File Offset: 0x00009C2C
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

	// Token: 0x17000AE9 RID: 2793
	// (get) Token: 0x060016FA RID: 5882 RVA: 0x0000BA43 File Offset: 0x00009C43
	public override bool HasAttackFlipCheck
	{
		get
		{
			return !this.IsShooting && base.HasAttackFlipCheck;
		}
	}

	// Token: 0x17000AEA RID: 2794
	// (get) Token: 0x060016FB RID: 5883 RVA: 0x0000BA55 File Offset: 0x00009C55
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

	// Token: 0x17000AEB RID: 2795
	// (get) Token: 0x060016FC RID: 5884 RVA: 0x0000BA6C File Offset: 0x00009C6C
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

	// Token: 0x17000AEC RID: 2796
	// (get) Token: 0x060016FD RID: 5885 RVA: 0x0000BA83 File Offset: 0x00009C83
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

	// Token: 0x17000AED RID: 2797
	// (get) Token: 0x060016FE RID: 5886 RVA: 0x0000BA9A File Offset: 0x00009C9A
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

	// Token: 0x060016FF RID: 5887 RVA: 0x0000BAB1 File Offset: 0x00009CB1
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		abilityController.Animator.SetBool("FryingPan_UsePirateAudio", true);
	}

	// Token: 0x06001700 RID: 5888 RVA: 0x0008BA68 File Offset: 0x00089C68
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

	// Token: 0x06001701 RID: 5889 RVA: 0x0000BACC File Offset: 0x00009CCC
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

	// Token: 0x06001702 RID: 5890 RVA: 0x0000A2B3 File Offset: 0x000084B3
	protected override void OnEnterAttackLogic()
	{
		this.m_abilityController.BroadcastAbilityCastEvents(base.CastAbilityType);
		this.FireProjectile();
	}

	// Token: 0x06001703 RID: 5891 RVA: 0x0008BB04 File Offset: 0x00089D04
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

	// Token: 0x06001704 RID: 5892 RVA: 0x0000BB00 File Offset: 0x00009D00
	protected override void UpdateArrowAim(bool doNotUpdatePlayerAnims = false)
	{
		if (this.IsShooting)
		{
			base.UpdateArrowAim(doNotUpdatePlayerAnims);
		}
	}

	// Token: 0x06001705 RID: 5893 RVA: 0x0000BB11 File Offset: 0x00009D11
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

	// Token: 0x06001706 RID: 5894 RVA: 0x0000BB27 File Offset: 0x00009D27
	public override void StopAbility(bool abilityInterrupted)
	{
		base.StopAbility(abilityInterrupted);
		if (this.IsShooting)
		{
			this.IsShooting = false;
		}
	}

	// Token: 0x06001707 RID: 5895 RVA: 0x0008BC00 File Offset: 0x00089E00
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

	// Token: 0x040016F2 RID: 5874
	[Header("Shoot Values")]
	[SerializeField]
	private string m_shootTellIntroName;

	// Token: 0x040016F3 RID: 5875
	[SerializeField]
	protected AbilityData m_shootAbilityData;

	// Token: 0x040016F4 RID: 5876
	[SerializeField]
	protected string m_shootProjectileName;

	// Token: 0x040016F5 RID: 5877
	[SerializeField]
	protected Vector2 m_shootProjectileOffset;

	// Token: 0x040016F7 RID: 5879
	private float MeleeTellIntroAnimSpeed = 3f;

	// Token: 0x040016F8 RID: 5880
	private float MeleeTellIntroAnimExitDelay;

	// Token: 0x040016F9 RID: 5881
	private float MeleeTellAnimSpeed = 1.5f;

	// Token: 0x040016FA RID: 5882
	private float MeleeTellAnimExitDelay;

	// Token: 0x040016FB RID: 5883
	private float MeleeAttackIntroAnimSpeed = 1.35f;

	// Token: 0x040016FC RID: 5884
	private float MeleeAttackIntroAnimExitDelay;

	// Token: 0x040016FD RID: 5885
	private float MeleeAttackAnimSpeed = 1.25f;

	// Token: 0x040016FE RID: 5886
	private float MeleeAttackAnimExitDelay;

	// Token: 0x040016FF RID: 5887
	private float MeleeExitAnimSpeed = 1.25f;

	// Token: 0x04001700 RID: 5888
	private float MeleeExitAnimExitDelay;

	// Token: 0x04001701 RID: 5889
	private float ShootTellIntroAnimSpeed = 3f;

	// Token: 0x04001702 RID: 5890
	private float ShootTellIntroAnimExitDelay;

	// Token: 0x04001703 RID: 5891
	private float ShootTellAnimSpeed = 1.5f;

	// Token: 0x04001704 RID: 5892
	private float ShootTellAnimExitDelay;

	// Token: 0x04001705 RID: 5893
	private float ShootAttackIntroAnimSpeed = 1.25f;

	// Token: 0x04001706 RID: 5894
	private float ShootAttackIntroAnimExitDelay;

	// Token: 0x04001707 RID: 5895
	private float ShootAttackAnimSpeed = 1.25f;

	// Token: 0x04001708 RID: 5896
	private float ShootAttackAnimExitDelay;

	// Token: 0x04001709 RID: 5897
	private float ShootExitAnimSpeed = 1.25f;

	// Token: 0x0400170A RID: 5898
	private float ShootExitAnimExitDelay;

	// Token: 0x0400170B RID: 5899
	private bool m_shootButtonHeld;

	// Token: 0x0400170C RID: 5900
	private bool m_shootButtonReleased;
}

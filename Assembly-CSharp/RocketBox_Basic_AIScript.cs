using System;
using System.Collections;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020001AD RID: 429
public class RocketBox_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000BC2 RID: 3010 RVA: 0x0006B470 File Offset: 0x00069670
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"RocketBoxBounceHomingBoltProjectile",
			"RocketBoxBounceBoltProjectile",
			"RocketBoxWarningProjectile",
			"RocketBoxExplosionProjectile",
			"RocketBoxWarningMinibossProjectile",
			"RocketBoxExplosionMinibossProjectile",
			"RocketBoxBoltMinibossBlueProjectile"
		};
	}

	// Token: 0x1700058E RID: 1422
	// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x00007326 File Offset: 0x00005526
	public IRelayLink ShoutAttackWarningAppearedRelay
	{
		get
		{
			return this.m_shoutAttackWarningAppearedRelay.link;
		}
	}

	// Token: 0x1700058F RID: 1423
	// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x00007333 File Offset: 0x00005533
	public IRelayLink ShoutAttackExplodedRelay
	{
		get
		{
			return this.m_shoutAttackExplodedRelay.link;
		}
	}

	// Token: 0x17000590 RID: 1424
	// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x00007340 File Offset: 0x00005540
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-7f, 7f);
		}
	}

	// Token: 0x17000591 RID: 1425
	// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x00007351 File Offset: 0x00005551
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(-6f, 6f);
		}
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x0006B4C4 File Offset: 0x000696C4
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.AddListener(new Action<object, CharacterHitEventArgs>(this.OnEnemyHit), false);
		this.m_platformCollider = base.EnemyController.HitboxController.GetCollider(HitboxType.Platform);
	}

	// Token: 0x17000592 RID: 1426
	// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_shoot_NumberOfBullets
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000593 RID: 1427
	// (get) Token: 0x06000BC9 RID: 3017 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_shoot_VerticalBullets
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000594 RID: 1428
	// (get) Token: 0x06000BCA RID: 3018 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_shoot_HomingBullets
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x00007362 File Offset: 0x00005562
	public override void OnEnemyActivated()
	{
		this.TriggerRestState(true);
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x0000736B File Offset: 0x0000556B
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.EnemyController.AlwaysFacing = true;
		base.EnemyController.LockFlip = false;
		base.StopProjectile(ref this.m_shoutWarningProjectile);
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x00007397 File Offset: 0x00005597
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Shoot()
	{
		float angle = 0f;
		base.EnemyController.AlwaysFacing = false;
		base.EnemyController.LockFlip = true;
		if (!base.EnemyController.IsFacingRight)
		{
			angle = 180f;
		}
		yield return this.Default_TellIntroAndLoop("SingleShot_Tell_Intro", this.m_shoot_TellIntro_AnimationSpeed, "SingleShot_Tell_Hold", this.m_shoot_TellHold_AnimationSpeed, this.m_shoot_Tell_Delay);
		yield return this.Default_Animation("SingleShot_Attack_Intro", this.m_shoot_AttackIntro_AnimationSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("SingleShot_Attack_Hold", this.m_shoot_AttackHold_AnimationSpeed, 0f, false);
		int num;
		for (int i = 0; i < this.m_shoot_NumberOfBullets; i = num + 1)
		{
			if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
			{
				this.FireProjectile("RocketBoxBoltMinibossBlueProjectile", 1, false, angle, 1f, true, true, true);
			}
			else if (this.m_shoot_HomingBullets)
			{
				this.FireProjectile("RocketBoxBounceHomingBoltProjectile", 0, false, angle, 1f, true, true, true);
			}
			else
			{
				this.FireProjectile("RocketBoxBounceBoltProjectile", 0, false, angle, 1f, true, true, true);
			}
			if (this.m_shoot_VerticalBullets)
			{
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
				{
					this.FireProjectile("RocketBoxBoltMinibossBlueProjectile", 3, false, 90f, 1f, true, true, true);
					this.FireProjectile("RocketBoxBoltMinibossBlueProjectile", 2, false, 180f - angle, 1f, true, true, true);
					this.FireProjectile("RocketBoxBoltMinibossBlueProjectile", 4, false, 270f, 1f, true, true, true);
				}
				else if (this.m_shoot_HomingBullets)
				{
					this.FireProjectile("RocketBoxBounceHomingBoltProjectile", 0, false, 90f, 1f, true, true, true);
					this.FireProjectile("RocketBoxBounceHomingBoltProjectile", 0, false, 180f - angle, 1f, true, true, true);
					this.FireProjectile("RocketBoxBounceHomingBoltProjectile", 0, false, 270f, 1f, true, true, true);
				}
				else
				{
					this.FireProjectile("RocketBoxBounceBoltProjectile", 0, false, 90f, 1f, true, true, true);
					this.FireProjectile("RocketBoxBounceBoltProjectile", 0, false, 180f - angle, 1f, true, true, true);
					this.FireProjectile("RocketBoxBounceBoltProjectile", 0, false, 270f, 1f, true, true, true);
				}
			}
			if (this.m_shoot_NumberOfBullets > 1 && this.m_shoot_ShotDelay > 0f)
			{
				yield return base.Wait(this.m_shoot_ShotDelay, false);
			}
			num = i;
		}
		base.EnemyController.LockFlip = false;
		if (this.m_shoot_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.m_shoot_AttackHold_Delay, false);
		}
		yield return this.Default_Animation("SingleShot_Exit", this.m_shoot_Exit_AnimationSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.AlwaysFacing = true;
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x17000595 RID: 1429
	// (get) Token: 0x06000BCE RID: 3022 RVA: 0x00005FAA File Offset: 0x000041AA
	protected virtual float m_explosion_WarningDuration
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x17000596 RID: 1430
	// (get) Token: 0x06000BCF RID: 3023 RVA: 0x00004536 File Offset: 0x00002736
	protected virtual float m_explosion_AttackCD
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x000073A6 File Offset: 0x000055A6
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator AltShoot()
	{
		base.EnemyController.AlwaysFacing = false;
		base.EnemyController.LockFlip = true;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			this.m_shoutWarningProjectile = this.FireProjectileAbsPos("RocketBoxWarningMinibossProjectile", base.EnemyController.Midpoint, false, 0f, 1f, true, true, true);
		}
		else
		{
			this.m_shoutWarningProjectile = this.FireProjectileAbsPos("RocketBoxWarningProjectile", base.EnemyController.Midpoint, false, 0f, 1f, true, true, true);
		}
		this.m_shoutWarningProjectile.transform.SetParent(base.EnemyController.transform, true);
		this.m_shoutAttackWarningAppearedRelay.Dispatch();
		yield return this.Default_TellIntroAndLoop("Explosion_Tell_Intro", this.m_shoot_TellIntro_AnimationSpeed, "Explosion_Tell_Hold", this.m_shoot_TellHold_AnimationSpeed, this.m_shoot_Tell_Delay);
		this.m_shoutAttackWarningAppearedRelay.Dispatch();
		yield return base.Wait(this.m_explosion_WarningDuration, false);
		base.StopProjectile(ref this.m_shoutWarningProjectile);
		this.m_shoutAttackExplodedRelay.Dispatch();
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			this.FireProjectileAbsPos("RocketBoxExplosionMinibossProjectile", base.EnemyController.Midpoint, false, 0f, 1f, true, true, true);
		}
		else
		{
			this.FireProjectileAbsPos("RocketBoxExplosionProjectile", base.EnemyController.Midpoint, false, 0f, 1f, true, true, true);
		}
		yield return this.Default_Animation("Explosion_Attack_Intro", this.m_shoot_AttackIntro_AnimationSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("Explosion_Attack_Hold", this.m_shoot_AttackHold_AnimationSpeed, 0f, false);
		base.EnemyController.LockFlip = false;
		if (this.m_shoot_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.m_shoot_AttackHold_Delay, false);
		}
		yield return this.Default_Animation("Explosion_Exit", this.m_shoot_Exit_AnimationSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.AlwaysFacing = true;
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_explosion_AttackCD);
		yield break;
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x0006B514 File Offset: 0x00069714
	private void OnEnemyHit(object sender, EventArgs args)
	{
		if (this.m_platformCollider && base.EnemyController.CurrentHealth > 0f && (args as CharacterHitEventArgs).Attacker.ActualKnockbackStrength > base.EnemyController.ActualKnockbackDefense)
		{
			this.m_platformCollider.gameObject.layer = 2;
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.ReenablePlatformCoroutine());
			}
		}
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x000073B5 File Offset: 0x000055B5
	private IEnumerator ReenablePlatformCoroutine()
	{
		float delay = 0.1f + Time.time;
		while (Time.time < delay)
		{
			yield return null;
		}
		while (base.EnemyController.KnockedIntoAir)
		{
			yield return null;
		}
		if (this.m_platformCollider.gameObject.layer != 11)
		{
			this.m_platformCollider.gameObject.layer = 11;
		}
		yield break;
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x0006B588 File Offset: 0x00069788
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_platformCollider)
		{
			this.m_platformCollider.gameObject.layer = 11;
		}
		if (!GameManager.IsApplicationClosing && base.Animator)
		{
			base.Animator.WriteDefaultValues();
		}
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x000073C4 File Offset: 0x000055C4
	private void OnDestroy()
	{
		if (!GameManager.IsApplicationClosing && base.IsInitialized)
		{
			base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(new Action<object, CharacterHitEventArgs>(this.OnEnemyHit));
		}
	}

	// Token: 0x04000E4E RID: 3662
	private Relay m_shoutAttackWarningAppearedRelay = new Relay();

	// Token: 0x04000E4F RID: 3663
	private Relay m_shoutAttackExplodedRelay = new Relay();

	// Token: 0x04000E50 RID: 3664
	private Collider2D m_platformCollider;

	// Token: 0x04000E51 RID: 3665
	private const string WARNING_PROJECTILE_NAME = "RocketBoxWarningProjectile";

	// Token: 0x04000E52 RID: 3666
	private const string EXPLOSION_PROJECTILE_NAME = "RocketBoxExplosionProjectile";

	// Token: 0x04000E53 RID: 3667
	private const string WARNING_MINIBOSS_PROJECTILE_NAME = "RocketBoxWarningMinibossProjectile";

	// Token: 0x04000E54 RID: 3668
	private const string EXPLOSION_MINIBOSS_PROJECTILE_NAME = "RocketBoxExplosionMinibossProjectile";

	// Token: 0x04000E55 RID: 3669
	private const string MINIBOSS_BASIC_PROJECTILE_NAME = "RocketBoxBoltMinibossBlueProjectile";

	// Token: 0x04000E56 RID: 3670
	protected float m_shoot_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000E57 RID: 3671
	protected float m_shoot_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000E58 RID: 3672
	protected float m_shoot_Tell_Delay;

	// Token: 0x04000E59 RID: 3673
	protected float m_shoot_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000E5A RID: 3674
	protected float m_shoot_AttackIntro_Delay;

	// Token: 0x04000E5B RID: 3675
	protected float m_shoot_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000E5C RID: 3676
	protected float m_shoot_AttackHold_Delay;

	// Token: 0x04000E5D RID: 3677
	protected float m_shoot_Exit_AnimationSpeed = 1f;

	// Token: 0x04000E5E RID: 3678
	protected float m_shoot_Exit_Delay;

	// Token: 0x04000E5F RID: 3679
	protected float m_shoot_Exit_ForceIdle;

	// Token: 0x04000E60 RID: 3680
	protected float m_shoot_Exit_AttackCD;

	// Token: 0x04000E61 RID: 3681
	protected float m_shoot_ShotDelay = 0.1f;

	// Token: 0x04000E62 RID: 3682
	private Projectile_RL m_shoutWarningProjectile;
}

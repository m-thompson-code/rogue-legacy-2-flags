using System;
using System.Collections;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x0200010A RID: 266
public class RocketBox_Basic_AIScript : BaseAIScript
{
	// Token: 0x0600081B RID: 2075 RVA: 0x0001BEB0 File Offset: 0x0001A0B0
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

	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x0600081C RID: 2076 RVA: 0x0001BF01 File Offset: 0x0001A101
	public IRelayLink ShoutAttackWarningAppearedRelay
	{
		get
		{
			return this.m_shoutAttackWarningAppearedRelay.link;
		}
	}

	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x0600081D RID: 2077 RVA: 0x0001BF0E File Offset: 0x0001A10E
	public IRelayLink ShoutAttackExplodedRelay
	{
		get
		{
			return this.m_shoutAttackExplodedRelay.link;
		}
	}

	// Token: 0x1700045A RID: 1114
	// (get) Token: 0x0600081E RID: 2078 RVA: 0x0001BF1B File Offset: 0x0001A11B
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-7f, 7f);
		}
	}

	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x0600081F RID: 2079 RVA: 0x0001BF2C File Offset: 0x0001A12C
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(-6f, 6f);
		}
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x0001BF40 File Offset: 0x0001A140
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.AddListener(new Action<object, CharacterHitEventArgs>(this.OnEnemyHit), false);
		this.m_platformCollider = base.EnemyController.HitboxController.GetCollider(HitboxType.Platform);
	}

	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x06000821 RID: 2081 RVA: 0x0001BF8E File Offset: 0x0001A18E
	protected virtual int m_shoot_NumberOfBullets
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x06000822 RID: 2082 RVA: 0x0001BF91 File Offset: 0x0001A191
	protected virtual bool m_shoot_VerticalBullets
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x06000823 RID: 2083 RVA: 0x0001BF94 File Offset: 0x0001A194
	protected virtual bool m_shoot_HomingBullets
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x0001BF97 File Offset: 0x0001A197
	public override void OnEnemyActivated()
	{
		this.TriggerRestState(true);
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x0001BFA0 File Offset: 0x0001A1A0
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.EnemyController.AlwaysFacing = true;
		base.EnemyController.LockFlip = false;
		base.StopProjectile(ref this.m_shoutWarningProjectile);
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x0001BFCC File Offset: 0x0001A1CC
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

	// Token: 0x1700045F RID: 1119
	// (get) Token: 0x06000827 RID: 2087 RVA: 0x0001BFDB File Offset: 0x0001A1DB
	protected virtual float m_explosion_WarningDuration
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x17000460 RID: 1120
	// (get) Token: 0x06000828 RID: 2088 RVA: 0x0001BFE2 File Offset: 0x0001A1E2
	protected virtual float m_explosion_AttackCD
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x0001BFE9 File Offset: 0x0001A1E9
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

	// Token: 0x0600082A RID: 2090 RVA: 0x0001BFF8 File Offset: 0x0001A1F8
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

	// Token: 0x0600082B RID: 2091 RVA: 0x0001C06C File Offset: 0x0001A26C
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

	// Token: 0x0600082C RID: 2092 RVA: 0x0001C07C File Offset: 0x0001A27C
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

	// Token: 0x0600082D RID: 2093 RVA: 0x0001C0CD File Offset: 0x0001A2CD
	private void OnDestroy()
	{
		if (!GameManager.IsApplicationClosing && base.IsInitialized)
		{
			base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(new Action<object, CharacterHitEventArgs>(this.OnEnemyHit));
		}
	}

	// Token: 0x04000B7E RID: 2942
	private Relay m_shoutAttackWarningAppearedRelay = new Relay();

	// Token: 0x04000B7F RID: 2943
	private Relay m_shoutAttackExplodedRelay = new Relay();

	// Token: 0x04000B80 RID: 2944
	private Collider2D m_platformCollider;

	// Token: 0x04000B81 RID: 2945
	private const string WARNING_PROJECTILE_NAME = "RocketBoxWarningProjectile";

	// Token: 0x04000B82 RID: 2946
	private const string EXPLOSION_PROJECTILE_NAME = "RocketBoxExplosionProjectile";

	// Token: 0x04000B83 RID: 2947
	private const string WARNING_MINIBOSS_PROJECTILE_NAME = "RocketBoxWarningMinibossProjectile";

	// Token: 0x04000B84 RID: 2948
	private const string EXPLOSION_MINIBOSS_PROJECTILE_NAME = "RocketBoxExplosionMinibossProjectile";

	// Token: 0x04000B85 RID: 2949
	private const string MINIBOSS_BASIC_PROJECTILE_NAME = "RocketBoxBoltMinibossBlueProjectile";

	// Token: 0x04000B86 RID: 2950
	protected float m_shoot_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000B87 RID: 2951
	protected float m_shoot_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000B88 RID: 2952
	protected float m_shoot_Tell_Delay;

	// Token: 0x04000B89 RID: 2953
	protected float m_shoot_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000B8A RID: 2954
	protected float m_shoot_AttackIntro_Delay;

	// Token: 0x04000B8B RID: 2955
	protected float m_shoot_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000B8C RID: 2956
	protected float m_shoot_AttackHold_Delay;

	// Token: 0x04000B8D RID: 2957
	protected float m_shoot_Exit_AnimationSpeed = 1f;

	// Token: 0x04000B8E RID: 2958
	protected float m_shoot_Exit_Delay;

	// Token: 0x04000B8F RID: 2959
	protected float m_shoot_Exit_ForceIdle;

	// Token: 0x04000B90 RID: 2960
	protected float m_shoot_Exit_AttackCD;

	// Token: 0x04000B91 RID: 2961
	protected float m_shoot_ShotDelay = 0.1f;

	// Token: 0x04000B92 RID: 2962
	private Projectile_RL m_shoutWarningProjectile;
}

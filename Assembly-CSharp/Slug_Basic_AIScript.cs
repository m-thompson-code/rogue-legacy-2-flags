using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x020001E8 RID: 488
public class Slug_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000D4E RID: 3406 RVA: 0x00007ADC File Offset: 0x00005CDC
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"SlugBoltProjectile",
			"SlugGravityBoltProjectile"
		};
	}

	// Token: 0x17000628 RID: 1576
	// (get) Token: 0x06000D4F RID: 3407 RVA: 0x0000746B File Offset: 0x0000566B
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.5f, 0.5f);
		}
	}

	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x06000D50 RID: 3408 RVA: 0x00007AFA File Offset: 0x00005CFA
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.45f);
		}
	}

	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x06000D51 RID: 3409 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_WalkTowards_ForceIdle
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700062B RID: 1579
	// (get) Token: 0x06000D52 RID: 3410 RVA: 0x000047A4 File Offset: 0x000029A4
	protected virtual int m_trail_ProjectileAmount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x06000D53 RID: 3411 RVA: 0x0007038C File Offset: 0x0006E58C
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.m_moveRight = (CDGHelper.RandomPlusMinus() > 0);
		base.EnemyController.OnEnemyDeathRelay.AddListener(new Action<object, EnemyDeathEventArgs>(this.OnSlugDeath), false);
		this.m_slimeTrail = base.EnemyController.gameObject.FindObjectReference("SlimeTrail", false, false);
		this.m_slimeEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_slug_trail_spawn_loop", base.transform);
		AudioManager.PlayAttached(this, this.m_slimeEventInstance, base.gameObject);
	}

	// Token: 0x06000D54 RID: 3412 RVA: 0x00007B0B File Offset: 0x00005D0B
	public override void ResetScript()
	{
		this.m_moveRight = (CDGHelper.RandomPlusMinus() > 0);
		base.ResetScript();
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x00007B21 File Offset: 0x00005D21
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			AudioManager.PlayAttached(this, this.m_slimeEventInstance, base.gameObject);
		}
	}

	// Token: 0x06000D56 RID: 3414 RVA: 0x00070414 File Offset: 0x0006E614
	protected override void OnDisable()
	{
		base.OnDisable();
		this.m_lastProjectileTrailSpawned = null;
		if (this.m_slimeTrail)
		{
			this.m_slimeTrail.transform.SetLocalPositionX(0f);
		}
		this.m_slimeTrailDisplacedLeft = false;
		this.m_slimeTrailDisplacedRight = false;
		AudioManager.Stop(this.m_slimeEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x06000D57 RID: 3415 RVA: 0x0007046C File Offset: 0x0006E66C
	private void OnDestroy()
	{
		if (base.EnemyController)
		{
			base.EnemyController.OnEnemyDeathRelay.RemoveListener(new Action<object, EnemyDeathEventArgs>(this.OnSlugDeath));
		}
		if (this.m_slimeEventInstance.isValid())
		{
			this.m_slimeEventInstance.release();
		}
	}

	// Token: 0x06000D58 RID: 3416 RVA: 0x000704BC File Offset: 0x0006E6BC
	private void OnSlugDeath(object sender, EnemyDeathEventArgs args)
	{
		ProjectileManager.DisableAllProjectiles(true, base.EnemyController.gameObject, "SlugBoltProjectile");
		if (this.m_lastProjectileTrailSpawned)
		{
			base.StopProjectile(ref this.m_lastProjectileTrailSpawned);
			AudioManager.PlayDelayedOneShot(this, "event:/SFX/Enemies/sfx_enemy_slug_trail_despawn", base.EnemyController.Pivot.transform.position, 0.8f);
		}
	}

	// Token: 0x1700062C RID: 1580
	// (get) Token: 0x06000D59 RID: 3417 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_verticalShot_TellIntro_AnimationSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x06000D5A RID: 3418 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_verticalShot_TellHold_AnimationSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x06000D5B RID: 3419 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_verticalShot_TellIntroAndHold_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x06000D5C RID: 3420 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_verticalShot_AttackIntro_AnimationSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000630 RID: 1584
	// (get) Token: 0x06000D5D RID: 3421 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_verticalShot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000631 RID: 1585
	// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_verticalShot_AttackHold_AnimationSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000632 RID: 1586
	// (get) Token: 0x06000D5F RID: 3423 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_verticalShot_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000633 RID: 1587
	// (get) Token: 0x06000D60 RID: 3424 RVA: 0x00006780 File Offset: 0x00004980
	protected virtual float m_verticalShot_Exit_AnimationSpeed
	{
		get
		{
			return 0.65f;
		}
	}

	// Token: 0x17000634 RID: 1588
	// (get) Token: 0x06000D61 RID: 3425 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_verticalShot_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000635 RID: 1589
	// (get) Token: 0x06000D62 RID: 3426 RVA: 0x000050CB File Offset: 0x000032CB
	protected virtual float m_verticalShot_Exit_ForceIdle
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x17000636 RID: 1590
	// (get) Token: 0x06000D63 RID: 3427 RVA: 0x00004536 File Offset: 0x00002736
	protected virtual float m_verticalShot_Exit_AttackCD
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000637 RID: 1591
	// (get) Token: 0x06000D64 RID: 3428 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual int m_verticalShot_TotalShotSpread
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x06000D65 RID: 3429 RVA: 0x00007B3D File Offset: 0x00005D3D
	protected virtual int m_verticalShot_SideBulletAngleOffset
	{
		get
		{
			return 14;
		}
	}

	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x06000D66 RID: 3430 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_verticalShot_SpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700063A RID: 1594
	// (get) Token: 0x06000D67 RID: 3431 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual int m_verticalShot_RepeatAttackPattern
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x06000D68 RID: 3432 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_verticalShot_RepeatAttackPatternDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x00007B41 File Offset: 0x00005D41
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator VerticalShot_Attack()
	{
		base.SetVelocity(0f, 0f, false);
		yield return this.Default_TellIntroAndLoop("Shoot_Tell_Intro", this.m_verticalShot_TellIntro_AnimationSpeed, "Shoot_Tell_Hold", this.m_verticalShot_TellHold_AnimationSpeed, this.m_verticalShot_TellIntroAndHold_Delay);
		yield return this.Default_Animation("Shoot_Attack_Intro", this.m_verticalShot_AttackIntro_AnimationSpeed, this.m_verticalShot_AttackIntro_Delay, true);
		this.SetAnimationSpeedMultiplier(this.m_verticalShot_AttackHold_AnimationSpeed);
		yield return this.ChangeAnimationState("Shoot_Attack_Hold");
		int num;
		for (int i = 0; i < this.m_verticalShot_RepeatAttackPattern; i = num + 1)
		{
			this.FireProjectile("SlugGravityBoltProjectile", 1, true, 90f, this.m_verticalShot_SpeedMod, true, true, true);
			for (int j = 1; j < this.m_verticalShot_TotalShotSpread + 1; j++)
			{
				this.FireProjectile("SlugGravityBoltProjectile", 1, true, (float)(90 + this.m_verticalShot_SideBulletAngleOffset * j), this.m_verticalShot_SpeedMod, true, true, true);
				this.FireProjectile("SlugGravityBoltProjectile", 1, true, (float)(90 - this.m_verticalShot_SideBulletAngleOffset * j), this.m_verticalShot_SpeedMod, true, true, true);
			}
			if (this.m_verticalShot_RepeatAttackPatternDelay > 0f)
			{
				yield return base.Wait(this.m_verticalShot_RepeatAttackPatternDelay, false);
			}
			num = i;
		}
		float num2 = (float)(this.m_verticalShot_RepeatAttackPattern * (this.m_verticalShot_RepeatAttackPattern - 1));
		num2 = Mathf.Clamp(this.m_verticalShot_AttackHold_Delay - num2, 0f, 2.1474836E+09f);
		if (num2 > 0f)
		{
			yield return base.Wait(num2, false);
		}
		yield return this.Default_Animation("Shoot_Exit", this.m_verticalShot_Exit_AnimationSpeed, this.m_verticalShot_Exit_Delay, true);
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_verticalShot_Exit_ForceIdle, this.m_verticalShot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x00007B50 File Offset: 0x00005D50
	public override IEnumerator WalkTowards()
	{
		if (this.m_moveRight)
		{
			base.SetVelocityX(base.EnemyController.ActualSpeed, false);
		}
		else
		{
			base.SetVelocityX(-base.EnemyController.ActualSpeed, false);
		}
		if (base.EnemyController.IsFacingRight != this.m_moveRight)
		{
			base.Flip();
		}
		if (this.m_trail_ProjectileAmount > 0)
		{
			float getWalkDuration = UnityEngine.Random.Range(this.WalkTowardsDuration.x, this.WalkTowardsDuration.y);
			float timeBetweenProjectiles = getWalkDuration / (float)this.m_trail_ProjectileAmount;
			int num;
			for (int i = 0; i < this.m_trail_ProjectileAmount; i = num + 1)
			{
				if (base.EnemyController.IsGrounded)
				{
					this.m_lastProjectileTrailSpawned = this.FireProjectile("SlugBoltProjectile", 0, false, 0f, 0f, true, true, true);
				}
				if (getWalkDuration > 0f)
				{
					yield return base.Wait(timeBetweenProjectiles, false);
				}
				num = i;
			}
		}
		else
		{
			yield return base.Wait(UnityEngine.Random.Range(this.WalkTowardsDuration.x, this.WalkTowardsDuration.y), false);
		}
		this.SetAnimationSpeedMultiplier(1f);
		if (this.m_WalkTowards_ForceIdle > 0f)
		{
			yield return this.Idle(this.m_WalkTowards_ForceIdle);
		}
		yield break;
	}

	// Token: 0x06000D6B RID: 3435 RVA: 0x00070524 File Offset: 0x0006E724
	private void Turn()
	{
		base.SetVelocityX(-base.EnemyController.Velocity.x, false);
		this.m_moveRight = !this.m_moveRight;
		if (base.EnemyController.IsFacingRight != this.m_moveRight)
		{
			base.Flip();
		}
	}

	// Token: 0x06000D6C RID: 3436 RVA: 0x00070574 File Offset: 0x0006E774
	private void LateUpdate()
	{
		if (!base.IsInitialized || base.EnemyController.IsDead)
		{
			return;
		}
		if (this.m_slimeTrail)
		{
			if (!base.EnemyController.IsGrounded && this.m_slimeTrail.isPlaying)
			{
				this.m_slimeTrail.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			}
			else if (base.EnemyController.IsGrounded && !this.m_slimeTrail.isPlaying)
			{
				this.m_slimeTrail.Play(true);
			}
		}
		if ((base.EnemyController.ControllerCorgi.State.IsCollidingLeft && !base.EnemyController.IsFacingRight) || (base.EnemyController.ControllerCorgi.State.IsCollidingRight && base.EnemyController.IsFacingRight) || (base.EnemyController.LeftSidePlatformDropPrevented && !base.EnemyController.IsFacingRight) || (base.EnemyController.RightSidePlatformDropPrevented && base.EnemyController.IsFacingRight) || (base.EnemyController.TouchingLeftRoomEdge && !base.EnemyController.IsFacingRight) || (base.EnemyController.TouchingRightRoomEdge && base.EnemyController.IsFacingRight))
		{
			this.Turn();
		}
		float belowSlopeAngle = base.EnemyController.ControllerCorgi.State.BelowSlopeAngle;
		if ((belowSlopeAngle > 0f && !base.EnemyController.IsFacingRight) || (belowSlopeAngle < 0f && base.EnemyController.IsFacingRight))
		{
			if (!this.m_slimeTrailDisplacedRight)
			{
				if (this.m_slimeTrail)
				{
					this.m_slimeTrail.gameObject.transform.position = base.GetAbsoluteSpawnPositionAtIndex(3, false);
				}
				this.m_slimeTrailDisplacedRight = true;
				return;
			}
		}
		else if ((belowSlopeAngle > 0f && base.EnemyController.IsFacingRight) || (belowSlopeAngle < 0f && !base.EnemyController.IsFacingRight))
		{
			if (!this.m_slimeTrailDisplacedLeft)
			{
				if (this.m_slimeTrail)
				{
					this.m_slimeTrail.gameObject.transform.position = base.GetAbsoluteSpawnPositionAtIndex(2, false);
				}
				this.m_slimeTrailDisplacedLeft = true;
				return;
			}
		}
		else if (this.m_slimeTrailDisplacedLeft || this.m_slimeTrailDisplacedRight)
		{
			if (this.m_slimeTrail)
			{
				this.m_slimeTrail.gameObject.transform.position = base.EnemyController.transform.position;
			}
			this.m_slimeTrailDisplacedLeft = false;
			this.m_slimeTrailDisplacedRight = false;
		}
	}

	// Token: 0x04000FAB RID: 4011
	private const float RAYCAST_LENGTH = 0.25f;

	// Token: 0x04000FAC RID: 4012
	protected const string WALK_PROJECTILE = "SlugBoltProjectile";

	// Token: 0x04000FAD RID: 4013
	private const string TRAIL_DESPAWN_AUDIO_PATH = "event:/SFX/Enemies/sfx_enemy_slug_trail_despawn";

	// Token: 0x04000FAE RID: 4014
	private bool m_moveRight;

	// Token: 0x04000FAF RID: 4015
	private Projectile_RL m_lastProjectileTrailSpawned;

	// Token: 0x04000FB0 RID: 4016
	private ParticleSystem m_slimeTrail;

	// Token: 0x04000FB1 RID: 4017
	private EventInstance m_slimeEventInstance;

	// Token: 0x04000FB2 RID: 4018
	protected const string VERTICAL_SHOT_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x04000FB3 RID: 4019
	protected const string VERTICAL_SHOT_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x04000FB4 RID: 4020
	protected const string VERTICAL_SHOT_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x04000FB5 RID: 4021
	protected const string VERTICAL_SHOT_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x04000FB6 RID: 4022
	protected const string VERTICAL_SHOT_EXIT = "Shoot_Exit";

	// Token: 0x04000FB7 RID: 4023
	protected const string VERTICAL_SHOT_PROJECTILE = "SlugGravityBoltProjectile";

	// Token: 0x04000FB8 RID: 4024
	private bool m_slimeTrailDisplacedRight;

	// Token: 0x04000FB9 RID: 4025
	private bool m_slimeTrailDisplacedLeft;
}

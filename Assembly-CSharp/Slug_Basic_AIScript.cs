using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class Slug_Basic_AIScript : BaseAIScript
{
	// Token: 0x060008BD RID: 2237 RVA: 0x0001D1E0 File Offset: 0x0001B3E0
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"SlugBoltProjectile",
			"SlugGravityBoltProjectile"
		};
	}

	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x060008BE RID: 2238 RVA: 0x0001D1FE File Offset: 0x0001B3FE
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.5f, 0.5f);
		}
	}

	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x060008BF RID: 2239 RVA: 0x0001D20F File Offset: 0x0001B40F
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.45f);
		}
	}

	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x060008C0 RID: 2240 RVA: 0x0001D220 File Offset: 0x0001B420
	protected virtual float m_WalkTowards_ForceIdle
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x060008C1 RID: 2241 RVA: 0x0001D227 File Offset: 0x0001B427
	protected virtual int m_trail_ProjectileAmount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x0001D22C File Offset: 0x0001B42C
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.m_moveRight = (CDGHelper.RandomPlusMinus() > 0);
		base.EnemyController.OnEnemyDeathRelay.AddListener(new Action<object, EnemyDeathEventArgs>(this.OnSlugDeath), false);
		this.m_slimeTrail = base.EnemyController.gameObject.FindObjectReference("SlimeTrail", false, false);
		this.m_slimeEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_slug_trail_spawn_loop", base.transform);
		AudioManager.PlayAttached(this, this.m_slimeEventInstance, base.gameObject);
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x0001D2B1 File Offset: 0x0001B4B1
	public override void ResetScript()
	{
		this.m_moveRight = (CDGHelper.RandomPlusMinus() > 0);
		base.ResetScript();
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x0001D2C7 File Offset: 0x0001B4C7
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			AudioManager.PlayAttached(this, this.m_slimeEventInstance, base.gameObject);
		}
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x0001D2E4 File Offset: 0x0001B4E4
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

	// Token: 0x060008C6 RID: 2246 RVA: 0x0001D33C File Offset: 0x0001B53C
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

	// Token: 0x060008C7 RID: 2247 RVA: 0x0001D38C File Offset: 0x0001B58C
	private void OnSlugDeath(object sender, EnemyDeathEventArgs args)
	{
		ProjectileManager.DisableAllProjectiles(true, base.EnemyController.gameObject, "SlugBoltProjectile");
		if (this.m_lastProjectileTrailSpawned)
		{
			base.StopProjectile(ref this.m_lastProjectileTrailSpawned);
			AudioManager.PlayDelayedOneShot(this, "event:/SFX/Enemies/sfx_enemy_slug_trail_despawn", base.EnemyController.Pivot.transform.position, 0.8f);
		}
	}

	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x060008C8 RID: 2248 RVA: 0x0001D3F2 File Offset: 0x0001B5F2
	protected virtual float m_verticalShot_TellIntro_AnimationSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0001D3F9 File Offset: 0x0001B5F9
	protected virtual float m_verticalShot_TellHold_AnimationSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x060008CA RID: 2250 RVA: 0x0001D400 File Offset: 0x0001B600
	protected virtual float m_verticalShot_TellIntroAndHold_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170004AB RID: 1195
	// (get) Token: 0x060008CB RID: 2251 RVA: 0x0001D407 File Offset: 0x0001B607
	protected virtual float m_verticalShot_AttackIntro_AnimationSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170004AC RID: 1196
	// (get) Token: 0x060008CC RID: 2252 RVA: 0x0001D40E File Offset: 0x0001B60E
	protected virtual float m_verticalShot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x060008CD RID: 2253 RVA: 0x0001D415 File Offset: 0x0001B615
	protected virtual float m_verticalShot_AttackHold_AnimationSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x060008CE RID: 2254 RVA: 0x0001D41C File Offset: 0x0001B61C
	protected virtual float m_verticalShot_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x060008CF RID: 2255 RVA: 0x0001D423 File Offset: 0x0001B623
	protected virtual float m_verticalShot_Exit_AnimationSpeed
	{
		get
		{
			return 0.65f;
		}
	}

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x060008D0 RID: 2256 RVA: 0x0001D42A File Offset: 0x0001B62A
	protected virtual float m_verticalShot_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x060008D1 RID: 2257 RVA: 0x0001D431 File Offset: 0x0001B631
	protected virtual float m_verticalShot_Exit_ForceIdle
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x060008D2 RID: 2258 RVA: 0x0001D438 File Offset: 0x0001B638
	protected virtual float m_verticalShot_Exit_AttackCD
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0001D43F File Offset: 0x0001B63F
	protected virtual int m_verticalShot_TotalShotSpread
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x060008D4 RID: 2260 RVA: 0x0001D442 File Offset: 0x0001B642
	protected virtual int m_verticalShot_SideBulletAngleOffset
	{
		get
		{
			return 14;
		}
	}

	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0001D446 File Offset: 0x0001B646
	protected virtual float m_verticalShot_SpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x060008D6 RID: 2262 RVA: 0x0001D44D File Offset: 0x0001B64D
	protected virtual int m_verticalShot_RepeatAttackPattern
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0001D450 File Offset: 0x0001B650
	protected virtual float m_verticalShot_RepeatAttackPatternDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x0001D457 File Offset: 0x0001B657
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

	// Token: 0x060008D9 RID: 2265 RVA: 0x0001D466 File Offset: 0x0001B666
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

	// Token: 0x060008DA RID: 2266 RVA: 0x0001D478 File Offset: 0x0001B678
	private void Turn()
	{
		base.SetVelocityX(-base.EnemyController.Velocity.x, false);
		this.m_moveRight = !this.m_moveRight;
		if (base.EnemyController.IsFacingRight != this.m_moveRight)
		{
			base.Flip();
		}
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x0001D4C8 File Offset: 0x0001B6C8
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

	// Token: 0x04000C57 RID: 3159
	private const float RAYCAST_LENGTH = 0.25f;

	// Token: 0x04000C58 RID: 3160
	protected const string WALK_PROJECTILE = "SlugBoltProjectile";

	// Token: 0x04000C59 RID: 3161
	private const string TRAIL_DESPAWN_AUDIO_PATH = "event:/SFX/Enemies/sfx_enemy_slug_trail_despawn";

	// Token: 0x04000C5A RID: 3162
	private bool m_moveRight;

	// Token: 0x04000C5B RID: 3163
	private Projectile_RL m_lastProjectileTrailSpawned;

	// Token: 0x04000C5C RID: 3164
	private ParticleSystem m_slimeTrail;

	// Token: 0x04000C5D RID: 3165
	private EventInstance m_slimeEventInstance;

	// Token: 0x04000C5E RID: 3166
	protected const string VERTICAL_SHOT_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x04000C5F RID: 3167
	protected const string VERTICAL_SHOT_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x04000C60 RID: 3168
	protected const string VERTICAL_SHOT_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x04000C61 RID: 3169
	protected const string VERTICAL_SHOT_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x04000C62 RID: 3170
	protected const string VERTICAL_SHOT_EXIT = "Shoot_Exit";

	// Token: 0x04000C63 RID: 3171
	protected const string VERTICAL_SHOT_PROJECTILE = "SlugGravityBoltProjectile";

	// Token: 0x04000C64 RID: 3172
	private bool m_slimeTrailDisplacedRight;

	// Token: 0x04000C65 RID: 3173
	private bool m_slimeTrailDisplacedLeft;
}

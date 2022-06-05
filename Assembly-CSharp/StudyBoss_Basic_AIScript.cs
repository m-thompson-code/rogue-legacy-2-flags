using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000136 RID: 310
public class StudyBoss_Basic_AIScript : BaseAIScript
{
	// Token: 0x0600098F RID: 2447 RVA: 0x0001EDA0 File Offset: 0x0001CFA0
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"StudyBossVerticalBeamWarningProjectile",
			"StudyBossVerticalBeamProjectile",
			"StudyBossRapidFireProjectile",
			"StudyBossRapidFireVoidProjectile",
			"StudyBossCircleProjectile",
			this.BOMB_ATTACK_EXPLOSION_PROJECTILE_NAME,
			this.BOMB_ATTACK_EXPLOSION_ADVANCED_PROJECTILE_NAME,
			this.BOMB_ATTACK_WARNING_PROJECTILE_NAME,
			this.BOMB_ATTACK_WARNING_ADVANCED_PROJECTILE_NAME,
			this.BOMB_ATTACK_FIREBALL_PROJECTILE_NAME,
			"StudyBossShoutExplosionProjectile",
			"StudyBossShoutWarningProjectile",
			"StudyBossShieldProjectile",
			this.CURSE_ATTACK_PROJECTILE_NAME,
			this.CURSE_ATTACK_ADVANCED_PROJECTILE_NAME,
			"StudyBossFlowerProjectile"
		};
	}

	// Token: 0x17000528 RID: 1320
	// (get) Token: 0x06000990 RID: 2448 RVA: 0x0001EE48 File Offset: 0x0001D048
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.4f, 0.8f);
		}
	}

	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x06000991 RID: 2449 RVA: 0x0001EE59 File Offset: 0x0001D059
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.4f, 0.8f);
		}
	}

	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x06000992 RID: 2450 RVA: 0x0001EE6A File Offset: 0x0001D06A
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.4f, 0.8f);
		}
	}

	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x06000993 RID: 2451 RVA: 0x0001EE7B File Offset: 0x0001D07B
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-2f, 2f);
		}
	}

	// Token: 0x1700052C RID: 1324
	// (get) Token: 0x06000994 RID: 2452 RVA: 0x0001EE8C File Offset: 0x0001D08C
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(3f, 3f);
		}
	}

	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x06000995 RID: 2453 RVA: 0x0001EE9D File Offset: 0x0001D09D
	protected virtual bool m_advancedAttack
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700052E RID: 1326
	// (get) Token: 0x06000996 RID: 2454 RVA: 0x0001EEA0 File Offset: 0x0001D0A0
	protected virtual bool is_Variant
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x06000997 RID: 2455 RVA: 0x0001EEA3 File Offset: 0x0001D0A3
	protected virtual string CURSE_ATTACK_PROJECTILE_NAME
	{
		get
		{
			return "StudyBossCurseProjectile";
		}
	}

	// Token: 0x17000530 RID: 1328
	// (get) Token: 0x06000998 RID: 2456 RVA: 0x0001EEAA File Offset: 0x0001D0AA
	protected virtual string CURSE_ATTACK_ADVANCED_PROJECTILE_NAME
	{
		get
		{
			return "StudyBossCurseAdvancedProjectile";
		}
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x0001EEB1 File Offset: 0x0001D0B1
	private void Awake()
	{
		this.m_onBossHit = new Action<object, HealthChangeEventArgs>(this.OnBossHit);
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x0001EEC5 File Offset: 0x0001D0C5
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.LogicController.DisableLogicActivationByDistance = true;
		this.m_verticalBeamWarningProjectiles = new Projectile_RL[this.m_verticalBeamAttackCount];
		this.m_verticalBeamProjectiles = new Projectile_RL[this.m_verticalBeamAttackCount];
		this.InitializeAudio();
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x0001EF02 File Offset: 0x0001D102
	public override void OnEnemyActivated()
	{
		base.OnEnemyActivated();
		base.EnemyController.HealthChangeRelay.AddListener(this.m_onBossHit, false);
	}

	// Token: 0x17000531 RID: 1329
	// (get) Token: 0x0600099C RID: 2460 RVA: 0x0001EF22 File Offset: 0x0001D122
	protected virtual int m_verticalBeamAttackCount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000532 RID: 1330
	// (get) Token: 0x0600099D RID: 2461 RVA: 0x0001EF25 File Offset: 0x0001D125
	protected virtual float m_timeBetweenVerticalBeamAttacks
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000533 RID: 1331
	// (get) Token: 0x0600099E RID: 2462 RVA: 0x0001EF2C File Offset: 0x0001D12C
	protected virtual float m_verticalBeamWarningLifetime
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x17000534 RID: 1332
	// (get) Token: 0x0600099F RID: 2463 RVA: 0x0001EF33 File Offset: 0x0001D133
	protected virtual float m_verticalBeamLifetime
	{
		get
		{
			return 8f;
		}
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x0001EF3A File Offset: 0x0001D13A
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator VerticalBeamAttack()
	{
		this.ToDo("VERTICAL BEAM ATTACK");
		if (this.m_verticalBeam_stopMovingWhileAttacking)
		{
			base.EnemyController.FlyingMovementType = FlyingMovementType.Override;
			this.StopAndFaceTarget();
			base.EnemyController.LockFlip = true;
		}
		yield return this.Default_TellIntroAndLoop("SummonVerticalBeam_Tell_Intro", this.m_verticalBeam_TellIntro_AnimSpeed, "SummonVerticalBeam_Tell_Hold", this.m_verticalBeam_TellHold_AnimSpeed, this.m_verticalBeam_TellIntroAndHold_Delay);
		yield return this.Default_Animation("SummonVerticalBeam_Attack_Intro", this.m_verticalBeam_AttackIntro_AnimSpeed, this.m_verticalBeam_AttackIntro_Delay, true);
		this.SetAnimationSpeedMultiplier(this.m_verticalBeam_AttackHold_AnimSpeed);
		yield return this.ChangeAnimationState("SummonVerticalBeam_Attack_Hold");
		int num;
		for (int i = 0; i < this.m_verticalBeamAttackCount; i = num + 1)
		{
			this.RunPersistentCoroutine(this.FireVerticalBeamWarningAndAttack(i));
			if (this.m_timeBetweenVerticalBeamAttacks > 0f)
			{
				yield return base.Wait(this.m_timeBetweenVerticalBeamAttacks, false);
			}
			num = i;
		}
		yield return base.Wait(this.m_verticalBeam_AttackHold_Delay, false);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("SummonVerticalBeam_Exit", this.m_verticalBeam_Exit_AnimSpeed, this.m_verticalBeam_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_Attack_Cooldown(this.m_verticalBeam_Exit_IdleDuration, this.m_verticalBeam_AttackCD);
		yield break;
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x0001EF49 File Offset: 0x0001D149
	private IEnumerator FireVerticalBeamWarningAndAttack(int index)
	{
		if (this.m_warningWaitTime == null)
		{
			this.m_warningWaitTime = new WaitForSeconds(this.m_verticalBeamWarningLifetime);
			this.m_beamWaitTime = new WaitForSeconds(this.m_verticalBeamLifetime);
		}
		Vector2 projectileOffset = this.GetVerticalBeamProjectileOffset();
		this.BeginVerticalBeamAudio();
		base.StopProjectile(ref this.m_verticalBeamWarningProjectiles[index]);
		this.m_verticalBeamWarningProjectiles[index] = this.FireProjectileAbsPos("StudyBossVerticalBeamWarningProjectile", projectileOffset, false, 270f, 1f, true, true, true);
		yield return this.m_warningWaitTime;
		base.StopProjectile(ref this.m_verticalBeamWarningProjectiles[index]);
		base.StopProjectile(ref this.m_verticalBeamProjectiles[index]);
		this.m_verticalBeamProjectiles[index] = this.FireProjectileAbsPos("StudyBossVerticalBeamProjectile", projectileOffset, false, 270f, 1f, true, true, true);
		yield return this.m_beamWaitTime;
		base.StopProjectile(ref this.m_verticalBeamProjectiles[index]);
		this.EndVerticalBeamAudio();
		yield break;
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x0001EF5F File Offset: 0x0001D15F
	private Vector2 GetVerticalBeamProjectileOffset()
	{
		Vector2 zero = Vector2.zero;
		return this.GetVerticalBeamOrigin();
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x0001EF70 File Offset: 0x0001D170
	private Vector2 GetVerticalBeamOrigin()
	{
		Vector2 vector = PlayerManager.GetPlayerController().Midpoint;
		float y = (base.EnemyController.Room as Room).Bounds.max.y + 1f;
		return new Vector2(vector.x, y);
	}

	// Token: 0x17000535 RID: 1333
	// (get) Token: 0x060009A4 RID: 2468 RVA: 0x0001EFC2 File Offset: 0x0001D1C2
	protected virtual int m_rapidFireShotCount
	{
		get
		{
			return 35;
		}
	}

	// Token: 0x17000536 RID: 1334
	// (get) Token: 0x060009A5 RID: 2469 RVA: 0x0001EFC6 File Offset: 0x0001D1C6
	protected virtual bool m_rapidFire_ShootVoidBulletAtEnd
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x0001EFC9 File Offset: 0x0001D1C9
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator RapidFireAttack()
	{
		this.ToDo("RAPID FIRE ATTACK");
		if (this.m_rapidFire_stopMovingWhileAttacking)
		{
			this.StopAndFaceTarget();
			base.EnemyController.LockFlip = true;
		}
		yield return this.Default_TellIntroAndLoop("EyeballShot_Tell_Intro", this.m_rapidFire_TellIntro_AnimSpeed, "EyeballShot_Tell_Hold", this.m_rapidFire_TellHold_AnimSpeed, this.m_rapidFire_TellIntroAndHold_Delay);
		this.BeginRapidFireAttackAudio();
		yield return this.Default_Animation("EyeballShot_Attack_Intro", this.m_rapidFire_AttackIntro_AnimSpeed, this.m_rapidFire_AttackIntro_Delay, true);
		this.SetAnimationSpeedMultiplier(this.m_rapidFire_AttackHold_AnimSpeed);
		yield return this.ChangeAnimationState("EyeballShot_Attack_Hold");
		float fireInterval = this.m_rapidFire_AttackHold_Delay / (float)this.m_rapidFireShotCount;
		int num;
		for (int i = 0; i < this.m_rapidFireShotCount; i = num + 1)
		{
			float randomRapidFireAngle = this.GetRandomRapidFireAngle(false);
			this.FireProjectile("StudyBossRapidFireProjectile", 2, false, randomRapidFireAngle, this.m_rapidFire_SpeedMod, true, true, true);
			if (fireInterval > 0f)
			{
				yield return base.Wait(fireInterval, false);
			}
			num = i;
		}
		if (this.m_rapidFire_ShootVoidBulletAtEnd)
		{
			float angle = this.GetRandomRapidFireAngle(false);
			if (this.m_rapidFire_VoidShotDelay > 0f)
			{
				yield return base.Wait(this.m_rapidFire_VoidShotDelay, false);
			}
			this.FireProjectile("StudyBossRapidFireVoidProjectile", Vector2.zero, false, angle, this.m_rapidFire_VoidProjectileSpeedMod, true, true, true);
			this.FireProjectile("StudyBossRapidFireVoidProjectile", Vector2.zero, false, angle - 25f, this.m_rapidFire_VoidProjectileSpeedMod, true, true, true);
			this.FireProjectile("StudyBossRapidFireVoidProjectile", Vector2.zero, false, angle - 50f, this.m_rapidFire_VoidProjectileSpeedMod, true, true, true);
			this.FireProjectile("StudyBossRapidFireVoidProjectile", Vector2.zero, false, angle - 75f, this.m_rapidFire_VoidProjectileSpeedMod, true, true, true);
			this.FireProjectile("StudyBossRapidFireVoidProjectile", Vector2.zero, false, angle + 25f, this.m_rapidFire_VoidProjectileSpeedMod, true, true, true);
			this.FireProjectile("StudyBossRapidFireVoidProjectile", Vector2.zero, false, angle + 50f, this.m_rapidFire_VoidProjectileSpeedMod, true, true, true);
			this.FireProjectile("StudyBossRapidFireVoidProjectile", Vector2.zero, false, angle + 75f, this.m_rapidFire_VoidProjectileSpeedMod, true, true, true);
			if (this.m_advancedAttack)
			{
				for (int i = 0; i < 3; i = num + 1)
				{
					if (this.m_rapidFire_VoidShotAdvancedDelay > 0f)
					{
						yield return base.Wait(this.m_rapidFire_VoidShotAdvancedDelay, false);
					}
					this.FireProjectile("StudyBossRapidFireVoidProjectile", Vector2.zero, false, angle, this.m_rapidFire_VoidProjectileSpeedMod, true, true, true);
					this.FireProjectile("StudyBossRapidFireVoidProjectile", Vector2.zero, false, angle - 25f, this.m_rapidFire_VoidProjectileSpeedMod, true, true, true);
					this.FireProjectile("StudyBossRapidFireVoidProjectile", Vector2.zero, false, angle - 50f, this.m_rapidFire_VoidProjectileSpeedMod, true, true, true);
					this.FireProjectile("StudyBossRapidFireVoidProjectile", Vector2.zero, false, angle - 75f, this.m_rapidFire_VoidProjectileSpeedMod, true, true, true);
					this.FireProjectile("StudyBossRapidFireVoidProjectile", Vector2.zero, false, angle + 25f, this.m_rapidFire_VoidProjectileSpeedMod, true, true, true);
					this.FireProjectile("StudyBossRapidFireVoidProjectile", Vector2.zero, false, angle + 50f, this.m_rapidFire_VoidProjectileSpeedMod, true, true, true);
					this.FireProjectile("StudyBossRapidFireVoidProjectile", Vector2.zero, false, angle + 75f, this.m_rapidFire_VoidProjectileSpeedMod, true, true, true);
					num = i;
				}
			}
		}
		base.SetAttackingWithContactDamage(false, 0.1f);
		this.EndRapidFireAttackAudio();
		yield return this.Default_Animation("EyeballShot_Exit", this.m_rapidFire_Exit_AnimSpeed, this.m_rapidFire_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_rapidFire_Exit_IdleDuration, this.m_rapidFire_AttackCD);
		yield break;
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x0001EFD8 File Offset: 0x0001D1D8
	private float GetRandomRapidFireAngle(bool disableRandom = false)
	{
		int num = UnityEngine.Random.Range(0, 2);
		int num2 = 1;
		if (num == 1)
		{
			num2 = -1;
		}
		float num3 = UnityEngine.Random.Range(0f, this.m_rapidFire_MaxRandomAngleDeviation);
		float num4 = CDGHelper.VectorToAngle(PlayerManager.GetPlayerController().Midpoint - base.EnemyController.Midpoint);
		if (!disableRandom)
		{
			num4 += (float)num2 * num3;
		}
		return num4;
	}

	// Token: 0x17000537 RID: 1335
	// (get) Token: 0x060009A8 RID: 2472 RVA: 0x0001F034 File Offset: 0x0001D234
	protected virtual int m_circleAttackProjectileCount
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x17000538 RID: 1336
	// (get) Token: 0x060009A9 RID: 2473 RVA: 0x0001F037 File Offset: 0x0001D237
	protected virtual float m_circleAttackInitialRadius
	{
		get
		{
			return 8f;
		}
	}

	// Token: 0x17000539 RID: 1337
	// (get) Token: 0x060009AA RID: 2474 RVA: 0x0001F03E File Offset: 0x0001D23E
	protected virtual float m_circleAttackInnerRadius
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x1700053A RID: 1338
	// (get) Token: 0x060009AB RID: 2475 RVA: 0x0001F045 File Offset: 0x0001D245
	protected virtual float m_circleAttackTimeBeforeCircleBeginsToClose
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700053B RID: 1339
	// (get) Token: 0x060009AC RID: 2476 RVA: 0x0001F04C File Offset: 0x0001D24C
	protected virtual float m_circleAttackTimeToClose
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700053C RID: 1340
	// (get) Token: 0x060009AD RID: 2477 RVA: 0x0001F053 File Offset: 0x0001D253
	protected virtual float m_circleAttackOrbitSpeed
	{
		get
		{
			return 360f;
		}
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x0001F05A File Offset: 0x0001D25A
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator CircleAttack()
	{
		this.ToDo("CIRCLE ATTACK");
		if (this.m_circleAttack_stopMovingWhileAttacking)
		{
			base.EnemyController.FlyingMovementType = FlyingMovementType.Override;
			this.StopAndFaceTarget();
			base.EnemyController.LockFlip = true;
		}
		yield return this.Default_TellIntroAndLoop("SummonCircularBullets_Tell_Intro", this.m_circleAttack_TellIntro_AnimSpeed, "SummonCircularBullets_Tell_Hold", this.m_circleAttack_TellHold_AnimSpeed, this.m_circleAttack_TellIntroAndHold_Delay);
		yield return this.Default_Animation("SummonCircularBullets_Attack_Intro", this.m_circleAttack_AttackIntro_AnimSpeed, this.m_circleAttack_AttackIntro_Delay, true);
		this.SetAnimationSpeedMultiplier(this.m_circleAttack_AttackHold_AnimSpeed);
		yield return this.ChangeAnimationState("SummonCircularBullets_Attack_Hold");
		this.SpawnOrbitingAttackProjectiles(this.m_circleAttackProjectileCount, new Func<float, IEnumerator>(this.OrbitProjectileAroundPlayer));
		if (this.m_advancedAttack)
		{
			yield return base.Wait(0.1f, false);
			this.SpawnOrbitingAttackProjectiles(this.m_circleAttackProjectileCount, new Func<float, IEnumerator>(this.OrbitProjectileAroundPlayer));
			yield return base.Wait(0.1f, false);
			this.SpawnOrbitingAttackProjectiles(this.m_circleAttackProjectileCount, new Func<float, IEnumerator>(this.OrbitProjectileAroundPlayer));
		}
		if (this.m_circleAttack_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.m_circleAttack_AttackHold_Delay, false);
		}
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("SummonCircularBullets_Exit", this.m_circleAttack_Exit_AnimSpeed, this.m_circleAttack_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_Attack_Cooldown(this.m_circleAttack_Exit_IdleDuration, this.m_circleAttack_AttackCD);
		yield break;
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x0001F06C File Offset: 0x0001D26C
	private void SpawnOrbitingAttackProjectiles(int projectileCount, Func<float, IEnumerator> orbitCoroutine)
	{
		float num = 360f / (float)projectileCount;
		float num2 = 0f;
		for (int i = 0; i < projectileCount; i++)
		{
			float arg = num2 + (float)i * num;
			this.RunPersistentCoroutine(orbitCoroutine(arg));
		}
		this.m_hasVoidCircleClosedIn = false;
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x0001F0B0 File Offset: 0x0001D2B0
	private IEnumerator OrbitProjectileAroundPlayer(float initialAngle)
	{
		float num = 0f;
		Vector3 pivot = PlayerManager.GetPlayerController().Midpoint;
		Vector3 vector = this.GetCircleAttackProjectileOffset(pivot, initialAngle, this.m_circleAttackInitialRadius + num);
		Projectile_RL projectile = this.FireProjectile("StudyBossCircleProjectile", vector, false, 0f, 1f, true, true, true);
		float shrinkSpeed = (this.m_circleAttackInitialRadius + num) / this.m_circleAttackTimeToClose;
		float angle = initialAngle;
		float radius = this.m_circleAttackInitialRadius + num;
		float timeStart = Time.time;
		while (Time.time - timeStart < this.m_circleAttackTimeBeforeCircleBeginsToClose + this.m_circleAttackTimeToClose)
		{
			if (radius > this.m_circleAttackInnerRadius)
			{
				pivot = PlayerManager.GetPlayerController().Midpoint;
			}
			vector = this.GetCircleAttackProjectileOffset(pivot, angle, radius);
			projectile.transform.position = base.EnemyController.Midpoint + vector;
			if (Time.time - timeStart >= this.m_circleAttackTimeBeforeCircleBeginsToClose)
			{
				angle += this.m_circleAttackOrbitSpeed * Time.deltaTime;
				radius -= shrinkSpeed * Time.deltaTime;
				if (!this.m_hasVoidCircleClosedIn)
				{
					this.m_hasVoidCircleClosedIn = true;
					this.PlayCircleClosingAudio();
				}
			}
			yield return null;
		}
		base.StopProjectile(ref projectile);
		yield break;
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x0001F0C8 File Offset: 0x0001D2C8
	private Vector2 GetCircleAttackProjectileOffset(Vector3 pivot, float angle, float currentRadius)
	{
		float f = 0.017453292f * angle;
		float num = Mathf.Cos(f);
		float num2 = Mathf.Sin(f);
		Vector2 down = Vector2.down;
		float x = down.x * num - down.y * num2;
		float y = down.x * num2 + down.y * num;
		Vector2 a = new Vector2(x, y) * currentRadius;
		Vector2 b = pivot - base.EnemyController.Midpoint;
		return a + b;
	}

	// Token: 0x1700053D RID: 1341
	// (get) Token: 0x060009B2 RID: 2482 RVA: 0x0001F141 File Offset: 0x0001D341
	protected virtual string BOMB_ATTACK_EXPLOSION_PROJECTILE_NAME
	{
		get
		{
			return "StudyBossBombExplosionProjectile";
		}
	}

	// Token: 0x1700053E RID: 1342
	// (get) Token: 0x060009B3 RID: 2483 RVA: 0x0001F148 File Offset: 0x0001D348
	protected virtual string BOMB_ATTACK_EXPLOSION_ADVANCED_PROJECTILE_NAME
	{
		get
		{
			return "StudyBossBombExplosionAdvancedProjectile";
		}
	}

	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x060009B4 RID: 2484 RVA: 0x0001F14F File Offset: 0x0001D34F
	protected virtual string BOMB_ATTACK_WARNING_PROJECTILE_NAME
	{
		get
		{
			return "StudyBossBombWarningProjectile";
		}
	}

	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x060009B5 RID: 2485 RVA: 0x0001F156 File Offset: 0x0001D356
	protected virtual string BOMB_ATTACK_WARNING_ADVANCED_PROJECTILE_NAME
	{
		get
		{
			return "StudyBossBombWarningAdvancedProjectile";
		}
	}

	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x060009B6 RID: 2486 RVA: 0x0001F15D File Offset: 0x0001D35D
	protected virtual string BOMB_ATTACK_FIREBALL_PROJECTILE_NAME
	{
		get
		{
			return "StudyBossBombFireballProjectile";
		}
	}

	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x060009B7 RID: 2487 RVA: 0x0001F164 File Offset: 0x0001D364
	protected virtual int m_bombAttack_FireballCountAtEnd
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x060009B8 RID: 2488 RVA: 0x0001F167 File Offset: 0x0001D367
	public IRelayLink BombAttackWarningAppearedRelay
	{
		get
		{
			return this.m_bombAttackWarningAppearedRelay.link;
		}
	}

	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x060009B9 RID: 2489 RVA: 0x0001F174 File Offset: 0x0001D374
	public IRelayLink BombAttackExplodedRelay
	{
		get
		{
			return this.m_bombAttackExplodedRelay.link;
		}
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x0001F181 File Offset: 0x0001D381
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator BombAttack()
	{
		this.ToDo("BOMB ATTACK");
		if (this.m_bombAttack_stopMovingWhileAttacking)
		{
			base.EnemyController.FlyingMovementType = FlyingMovementType.Override;
			base.SetVelocity(0f, 0f, false);
			base.EnemyController.LockFlip = true;
		}
		string projectileName = this.BOMB_ATTACK_WARNING_PROJECTILE_NAME;
		if (this.m_advancedAttack)
		{
			projectileName = this.BOMB_ATTACK_WARNING_ADVANCED_PROJECTILE_NAME;
		}
		Vector2 offset5;
		Vector2 offset4;
		Vector2 offset3;
		Vector2 offset2;
		Vector2 offset = offset2 = (offset3 = (offset4 = (offset5 = PlayerManager.GetPlayerController().Midpoint)));
		float y = this.Die3Roll();
		offset += new Vector2(-this.m_bombAttack_Displace_X * 2f, y);
		this.m_bombWarningProjectile = this.FireProjectileAbsPos(projectileName, offset, false, 0f, 1f, true, true, true);
		y = this.Die3Roll();
		offset2 += new Vector2(0f, y);
		this.m_bombWarningProjectile2 = this.FireProjectileAbsPos(projectileName, offset2, false, 0f, 1f, true, true, true);
		y = this.Die3Roll();
		offset3 += new Vector2(-this.m_bombAttack_Displace_X, y);
		this.m_bombWarningProjectile3 = this.FireProjectileAbsPos(projectileName, offset3, false, 0f, 1f, true, true, true);
		y = this.Die3Roll();
		offset4 += new Vector2(this.m_bombAttack_Displace_X, y);
		this.m_bombWarningProjectile4 = this.FireProjectileAbsPos(projectileName, offset4, false, 0f, 1f, true, true, true);
		y = this.Die3Roll();
		offset5 += new Vector2(this.m_bombAttack_Displace_X * 2f, y);
		this.m_bombWarningProjectile5 = this.FireProjectileAbsPos(projectileName, offset5, false, 0f, 1f, true, true, true);
		this.m_bombAttackWarningAppearedRelay.Dispatch();
		yield return this.Default_TellIntroAndLoop("SummonBomb_Tell_Intro", this.m_bombAttack_TellIntro_AnimSpeed, "SummonBomb_Tell_Hold", this.m_bombAttack_TellHold_AnimSpeed, this.m_bombAttack_TellIntroAndHold_Delay);
		if (this.m_bombAttack_TimeToExplosion > 0f)
		{
			yield return base.Wait(this.m_bombAttack_TimeToExplosion, false);
		}
		base.StopProjectile(ref this.m_bombWarningProjectile);
		base.StopProjectile(ref this.m_bombWarningProjectile2);
		base.StopProjectile(ref this.m_bombWarningProjectile3);
		base.StopProjectile(ref this.m_bombWarningProjectile4);
		base.StopProjectile(ref this.m_bombWarningProjectile5);
		string projectileName2 = this.BOMB_ATTACK_EXPLOSION_PROJECTILE_NAME;
		string projectileName3 = this.CURSE_ATTACK_PROJECTILE_NAME;
		if (this.m_advancedAttack)
		{
			projectileName2 = this.BOMB_ATTACK_EXPLOSION_ADVANCED_PROJECTILE_NAME;
			projectileName3 = this.CURSE_ATTACK_ADVANCED_PROJECTILE_NAME;
		}
		this.m_bombAttackExplodedRelay.Dispatch();
		this.FireProjectileAbsPos(projectileName2, offset, false, 0f, 1f, true, true, true);
		this.FireProjectileAbsPos(projectileName2, offset2, false, 0f, 1f, true, true, true);
		this.FireProjectileAbsPos(projectileName2, offset3, false, 0f, 1f, true, true, true);
		this.FireProjectileAbsPos(projectileName2, offset4, false, 0f, 1f, true, true, true);
		this.FireProjectileAbsPos(projectileName2, offset5, false, 0f, 1f, true, true, true);
		if (this.m_bombAttack_FireballCountAtEnd > 0)
		{
			float num = (float)(360 / this.m_bombAttack_FireballCountAtEnd);
			float num2 = 0f;
			for (int i = 0; i < this.m_bombAttack_FireballCountAtEnd; i++)
			{
				float angle = num2 + (float)i * num;
				this.FireProjectileAbsPos(projectileName3, offset, false, angle, 1f, true, true, true);
				this.FireProjectileAbsPos(projectileName3, offset2, false, angle, 1f, true, true, true);
				this.FireProjectileAbsPos(projectileName3, offset3, false, angle, 1f, true, true, true);
				this.FireProjectileAbsPos(projectileName3, offset4, false, angle, 1f, true, true, true);
				this.FireProjectileAbsPos(projectileName3, offset5, false, angle, 1f, true, true, true);
			}
		}
		yield return this.Default_Animation("SummonBomb_Attack_Intro", this.m_bombAttack_AttackIntro_AnimSpeed, this.m_bombAttack_AttackIntro_Delay, true);
		yield return this.Default_Animation("SummonBomb_Attack_Hold", this.m_bombAttack_AttackHold_AnimSpeed, this.m_bombAttack_AttackHold_Delay, true);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("SummonBomb_Exit", this.m_bombAttack_Exit_AnimSpeed, this.m_bombAttack_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_Attack_Cooldown(this.m_bombAttack_Exit_IdleDuration, this.m_bombAttack_AttackCD);
		yield break;
	}

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x060009BB RID: 2491 RVA: 0x0001F190 File Offset: 0x0001D390
	protected virtual int m_shout_fireballsAtEnd
	{
		get
		{
			return 18;
		}
	}

	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x060009BC RID: 2492 RVA: 0x0001F194 File Offset: 0x0001D394
	public IRelayLink ShoutAttackWarningAppearedRelay
	{
		get
		{
			return this.m_shoutAttackWarningAppearedRelay.link;
		}
	}

	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x060009BD RID: 2493 RVA: 0x0001F1A1 File Offset: 0x0001D3A1
	public IRelayLink ShoutAttackExplodedRelay
	{
		get
		{
			return this.m_shoutAttackExplodedRelay.link;
		}
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x0001F1AE File Offset: 0x0001D3AE
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShoutAttack()
	{
		this.ToDo("SHOUT ATTACK");
		if (this.m_shout_stopMovingWhileAttacking)
		{
			base.EnemyController.FlyingMovementType = FlyingMovementType.Override;
			this.StopAndFaceTarget();
			base.EnemyController.LockFlip = true;
		}
		this.m_shoutWarningProjectile = this.FireProjectile("StudyBossShoutWarningProjectile", 2, false, 0f, 1f, true, true, true);
		yield return this.Default_TellIntroAndLoop("Shout_Tell_Intro", this.m_shout_TellIntro_AnimSpeed, "Shout_Tell_Hold", this.m_shout_TellHold_AnimSpeed, this.m_shout_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Shout_Attack_Intro", this.m_shout_AttackIntro_AnimSpeed, this.m_shout_AttackIntro_Delay, true);
		this.m_shoutWarningProjectile.transform.position = base.EnemyController.Midpoint;
		this.m_shoutWarningProjectile.transform.SetParent(base.EnemyController.transform, true);
		this.m_shoutAttackWarningAppearedRelay.Dispatch();
		base.StopProjectile(ref this.m_shoutWarningProjectile);
		this.m_shoutAttackExplodedRelay.Dispatch();
		this.FireProjectile("StudyBossShoutExplosionProjectile", 2, false, 0f, 1f, true, true, true);
		if (this.m_shout_fireballsAtEnd > 0)
		{
			float angleBetweenProjectiles = (float)(360 / this.m_shout_fireballsAtEnd);
			float initialAngle = 0f;
			for (int i = 0; i < this.m_shout_fireballsAtEnd; i++)
			{
				float angle = initialAngle + (float)i * angleBetweenProjectiles;
				this.FireProjectile(this.BOMB_ATTACK_FIREBALL_PROJECTILE_NAME, 2, false, angle, 1f, true, true, true);
			}
			if (this.m_advancedAttack)
			{
				if (this.m_shoutAttackDelaySecondExplosion > 0f)
				{
					yield return base.Wait(this.m_shoutAttackDelaySecondExplosion, false);
				}
				for (int j = 0; j < this.m_shout_fireballsAtEnd; j++)
				{
					float num = initialAngle + (float)j * angleBetweenProjectiles;
					num += (float)this.m_shoutAttackDelaySecondExplosionAngleAdd;
					this.FireProjectile(this.BOMB_ATTACK_FIREBALL_PROJECTILE_NAME, 2, false, num, 1f, true, true, true);
				}
				if (this.m_shoutAttackDelaySecondExplosion > 0f)
				{
					yield return base.Wait(this.m_shoutAttackDelaySecondExplosion, false);
				}
				for (int k = 0; k < this.m_shout_fireballsAtEnd; k++)
				{
					float angle2 = initialAngle + (float)k * angleBetweenProjectiles;
					this.FireProjectile(this.BOMB_ATTACK_FIREBALL_PROJECTILE_NAME, 2, false, angle2, 1f, true, true, true);
				}
			}
		}
		yield return this.Default_Animation("Shout_Attack_Hold", this.m_shout_AttackHold_AnimSpeed, this.m_shout_AttackHold_Delay, true);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("Shout_Exit", this.m_shout_Exit_AnimSpeed, this.m_shout_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_Attack_Cooldown(this.m_shout_Exit_IdleDuration, this.m_shout_AttackCD);
		yield break;
	}

	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x060009BF RID: 2495 RVA: 0x0001F1BD File Offset: 0x0001D3BD
	protected virtual int m_dashAttackFireballCountAtEnd
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x0001F1C0 File Offset: 0x0001D3C0
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator DashAttack()
	{
		this.ToDo("Dash Attack");
		this.StopAndFaceTarget();
		base.EnemyController.FollowTarget = false;
		yield return this.Default_TellIntroAndLoop("Dash_Tell_Intro", this.m_dashAttack_TellIntro_AnimSpeed, "Dash_Tell_Hold", this.m_dashAttack_TellHold_AnimSpeed, this.m_dashAttack_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		base.EnemyController.Heading = base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Override;
		Vector2 vector = base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint;
		Vector2 dashVelocity = this.m_dashAttackSpeed * vector.normalized;
		this.FaceTarget();
		base.EnemyController.LockFlip = true;
		yield return this.Default_Animation("Dash_Attack_Intro", this.m_dashAttack_AttackIntro_AnimSpeed, this.m_dashAttack_AttackIntro_Delay, true);
		base.EnemyController.SetVelocity(dashVelocity.x, dashVelocity.y, false);
		this.SetAnimationSpeedMultiplier(this.m_dashAttack_AttackHold_AnimSpeed);
		yield return this.ChangeAnimationState("Dash_Attack_Hold");
		if (this.m_dashAttack_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.m_dashAttack_AttackHold_Delay, false);
		}
		if (this.m_dashAttackFireballCountAtEnd > 0)
		{
			float num = (float)(360 / this.m_dashAttackFireballCountAtEnd);
			float num2 = 0f;
			for (int i = 0; i < this.m_dashAttackFireballCountAtEnd; i++)
			{
				float angle = num2 + (float)i * num;
				this.FireProjectile("StudyBossFlowerProjectile", Vector2.zero, false, angle, this.m_dashAttackFireballSpeedMod, true, true, true);
			}
		}
		base.EnemyController.SetVelocity(0f, 0f, false);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("Dash_Exit", this.m_dashAttack_Exit_AnimSpeed, this.m_dashAttack_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.FollowTarget = true;
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_dashAttack_Exit_IdleDuration, this.m_dashAttack_AttackCD);
		yield break;
	}

	// Token: 0x17000549 RID: 1353
	// (get) Token: 0x060009C1 RID: 2497 RVA: 0x0001F1CF File Offset: 0x0001D3CF
	protected virtual int m_shieldAttackProjectileCount
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x0001F1D2 File Offset: 0x0001D3D2
	private void ShieldAttack()
	{
		if (!this.m_isShieldAttackRunning)
		{
			this.ToDo("SHIELD ATTACK");
			this.m_isShieldAttackRunning = true;
			this.SpawnOrbitingAttackProjectiles(this.m_shieldAttackProjectileCount, new Func<float, IEnumerator>(this.OrbitShieldProjectileAroundBoss));
			this.PlayShieldAttackAudio();
		}
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x0001F20C File Offset: 0x0001D40C
	private IEnumerator OrbitShieldProjectileAroundBoss(float initialAngle)
	{
		Vector3 midpoint = base.EnemyController.Midpoint;
		Vector3 pivotDiff = midpoint - base.EnemyController.transform.localPosition;
		Vector3 vector = this.GetCircleAttackProjectileOffset(midpoint, initialAngle, this.m_shieldAttackRadius);
		Projectile_RL projectile = this.FireProjectile("StudyBossShieldProjectile", vector, false, 0f, 1f, true, true, true);
		float angle = initialAngle;
		float timeStart = Time.time;
		bool isRespawnTimerRunning = false;
		while (!base.EnemyController.IsDead)
		{
			if (!projectile.gameObject.activeInHierarchy && !isRespawnTimerRunning)
			{
				isRespawnTimerRunning = true;
				timeStart = Time.time;
			}
			midpoint = base.EnemyController.Midpoint;
			vector = this.GetCircleAttackProjectileOffset(midpoint, angle, this.m_shieldAttackRadius);
			vector += pivotDiff;
			projectile.transform.position = base.EnemyController.Midpoint + vector;
			if (isRespawnTimerRunning)
			{
				isRespawnTimerRunning = false;
				if (Time.time - timeStart >= this.m_shieldAttackTimeBetweenProjectileRespawn)
				{
					projectile = this.FireProjectile("StudyBossShieldProjectile", vector, false, 0f, 1f, true, true, true);
				}
			}
			angle += this.m_shieldAttackOrbitSpeed * Time.deltaTime;
			yield return null;
		}
		base.StopProjectile(ref projectile);
		yield break;
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x0001F222 File Offset: 0x0001D422
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[WanderLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ModeShift()
	{
		this.ToDo("MODE SHIFT");
		base.RemoveStatusEffects(false);
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = true;
		yield return base.DeathAnim();
		base.EnemyController.LockFlip = true;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.SetVelocity(0f, 0f, false);
		base.EnemyController.ModeshiftDamageMod = this.m_modeShift_DamageMod;
		this.m_shoutWarningProjectile = this.FireProjectile("StudyBossShoutWarningProjectile", 2, false, 0f, 1f, true, true, true);
		this.m_shoutAttackWarningAppearedRelay.Dispatch();
		yield return this.Default_Animation("ModeShift_Intro", this.m_modeShift_Intro_AnimSpeed, this.m_modeShift_Intro_Delay, true);
		yield return this.Default_Animation("ModeShift_Scream_Intro", this.m_modeShift_Scream_Intro_AnimSpeed, this.m_modeShift_Scream_Intro_Delay, true);
		base.SetAttackingWithContactDamage(true, 0.1f);
		base.StopProjectile(ref this.m_shoutWarningProjectile);
		this.ShieldAttack();
		this.FireProjectile("StudyBossShoutExplosionProjectile", 2, false, 0f, 1f, true, true, true);
		yield return this.Default_Animation("ModeShift_Scream_Hold", this.m_modeShift_Scream_Hold_AnimSpeed, this.m_modeShift_Scream_Hold_Delay, true);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("ModeShift_Scream_Exit", this.m_modeShift_Exit_AnimSpeed, this.m_modeShift_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ModeshiftDamageMod = 1f;
		base.EnemyController.ResetBaseValues();
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		yield return this.Default_Attack_Cooldown(this.m_modeShift_IdleDuration, this.m_modeShift_AttackCD);
		yield break;
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x0001F231 File Offset: 0x0001D431
	public override IEnumerator SpawnAnim()
	{
		yield return this.Default_Animation("Intro_Idle", this.m_spawn_Idle_AnimSpeed, this.m_spawn_Idle_Delay, true);
		yield return this.Default_Animation("Intro", this.m_spawn_Intro_AnimSpeed, this.m_spawn_Intro_Delay, true);
		MusicManager.PlayMusic(SongID.StudyBossBGM_Boss5_Phase1, false, false);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 1f, false);
		yield break;
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x0001F240 File Offset: 0x0001D440
	public override IEnumerator DeathAnim()
	{
		yield return base.DeathAnim();
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 0.5f);
		yield return this.Default_Animation("Death_Intro", this.m_death_Intro_AnimSpeed, this.m_death_Intro_Delay, true);
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
		base.EnemyController.GetComponent<BossModeShiftController>().DissolveMaterials(this.m_death_Hold_Delay);
		yield return this.Default_Animation("Death_Loop", this.m_death_Hold_AnimSpeed, this.m_death_Hold_Delay, true);
		yield break;
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x0001F250 File Offset: 0x0001D450
	private void OnBossHit(object sender, HealthChangeEventArgs args)
	{
		if (base.EnemyController.IsDead || args.PrevHealthValue <= args.NewHealthValue)
		{
			return;
		}
		if (base.EnemyController.CurrentHealth <= (float)base.EnemyController.ActualMaxHealth * this.m_modeShift_HealthMod)
		{
			string forceExecuteLogicBlockName_OnceOnly = "ModeShift";
			base.LogicController.StopAllLogic(true);
			base.LogicController.ForceExecuteLogicBlockName_OnceOnly = forceExecuteLogicBlockName_OnceOnly;
			if (this.m_modeShiftEventArgs == null)
			{
				this.m_modeShiftEventArgs = new EnemyModeShiftEventArgs(base.EnemyController);
			}
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EnemyModeShift, this, this.m_modeShiftEventArgs);
			base.EnemyController.HealthChangeRelay.RemoveListener(this.m_onBossHit);
		}
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x0001F2F5 File Offset: 0x0001D4F5
	public override void OnLBCompleteOrCancelled()
	{
		this.DisableWarningProjectiles();
		base.EnemyController.LockFlip = false;
		this.EndRapidFireAttackAudio();
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x0001F315 File Offset: 0x0001D515
	protected override void OnDisable()
	{
		base.OnDisable();
		if (base.IsInitialized)
		{
			this.DisableWarningProjectiles();
			this.DisablePersistentLogicProjectiles();
			this.DisableAudio();
		}
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x0001F338 File Offset: 0x0001D538
	private void DisableWarningProjectiles()
	{
		base.StopProjectile(ref this.m_shoutWarningProjectile);
		base.StopProjectile(ref this.m_bombWarningProjectile);
		base.StopProjectile(ref this.m_bombWarningProjectile2);
		base.StopProjectile(ref this.m_bombWarningProjectile3);
		base.StopProjectile(ref this.m_bombWarningProjectile4);
		base.StopProjectile(ref this.m_bombWarningProjectile5);
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x0001F390 File Offset: 0x0001D590
	private void DisablePersistentLogicProjectiles()
	{
		for (int i = 0; i < this.m_verticalBeamAttackCount; i++)
		{
			Projectile_RL projectile_RL = this.m_verticalBeamProjectiles[i];
			base.StopProjectile(ref projectile_RL);
			Projectile_RL projectile_RL2 = this.m_verticalBeamWarningProjectiles[i];
			base.StopProjectile(ref projectile_RL2);
		}
		if (this.m_verticalBeamAudioPlaying)
		{
			this.EndVerticalBeamAudio();
		}
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x0001F3E0 File Offset: 0x0001D5E0
	private float Die3Roll()
	{
		float num = (float)UnityEngine.Random.Range(0, 5);
		if (num == 1f)
		{
			num = -this.m_bombAttack_Displace_Y * 2f;
		}
		else if (num == 2f)
		{
			num = -this.m_bombAttack_Displace_Y;
		}
		else if (num == 3f)
		{
			num = 0f;
		}
		else if (num == 4f)
		{
			num = this.m_bombAttack_Displace_Y;
		}
		else
		{
			num = this.m_bombAttack_Displace_Y * 2f;
		}
		return num;
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x0001F44F File Offset: 0x0001D64F
	public override void ResetScript()
	{
		this.m_isShieldAttackRunning = false;
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		base.ResetScript();
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x0001F470 File Offset: 0x0001D670
	private void InitializeAudio()
	{
		this.m_flyingAudioEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_studyBoss_fly_loop", base.transform);
		this.m_idleAudioEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_studyBoss_idle_loop", base.transform);
		this.m_verticalBeamLoopAudioEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_studyBoss_attack_skyBeam_spawn_start_loop", base.transform);
		this.m_verticalBeamDespawnAudioEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_studyBoss_attack_skyBeam_despawn", base.transform);
		this.m_voidCircleClosingAudioEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_studyBoss_attack_voidCircle_closeIn", base.transform);
		this.m_eyeballShotLoopAudioEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_studyBoss_attack_eye_shoot_start_loop", base.transform);
		this.m_shieldAttackLoopAudioEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_studyBoss_phase2_voidProjectiles_loop", base.transform);
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x0001F517 File Offset: 0x0001D717
	private void DisableAudio()
	{
		AudioManager.Stop(this.m_flyingAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Stop(this.m_idleAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Stop(this.m_verticalBeamLoopAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Stop(this.m_eyeballShotLoopAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Stop(this.m_shieldAttackLoopAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x0001F558 File Offset: 0x0001D758
	private void OnDestroy()
	{
		if (this.m_flyingAudioEventInstance.isValid())
		{
			this.m_flyingAudioEventInstance.release();
		}
		if (this.m_idleAudioEventInstance.isValid())
		{
			this.m_idleAudioEventInstance.release();
		}
		if (this.m_verticalBeamLoopAudioEventInstance.isValid())
		{
			this.m_verticalBeamLoopAudioEventInstance.release();
		}
		if (this.m_verticalBeamDespawnAudioEventInstance.isValid())
		{
			this.m_verticalBeamDespawnAudioEventInstance.release();
		}
		if (this.m_voidCircleClosingAudioEventInstance.isValid())
		{
			this.m_voidCircleClosingAudioEventInstance.release();
		}
		if (this.m_eyeballShotLoopAudioEventInstance.isValid())
		{
			this.m_eyeballShotLoopAudioEventInstance.release();
		}
		if (this.m_shieldAttackLoopAudioEventInstance.isValid())
		{
			this.m_shieldAttackLoopAudioEventInstance.release();
		}
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x0001F614 File Offset: 0x0001D814
	protected override void PlayDeathAnimAudio()
	{
		if (base.EnemyController.CurrentHealth > 0f)
		{
			AudioManager.PlayOneShot(this, "event:/SFX/Enemies/sfx_studyBoss_phase2_hit_start", base.gameObject.transform.position);
			return;
		}
		base.PlayDeathAnimAudio();
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x0001F64C File Offset: 0x0001D84C
	private void Update()
	{
		bool flag = base.EnemyController.Velocity.sqrMagnitude > 0f;
		this.UpdateFlyingAudio(flag);
		this.UpdateIdleAudio(flag);
		this.m_wasFlying = flag;
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x0001F68C File Offset: 0x0001D88C
	private void UpdateFlyingAudio(bool isFlying)
	{
		if (isFlying && !this.m_wasFlying)
		{
			AudioManager.PlayAttached(this, this.m_flyingAudioEventInstance, base.gameObject);
		}
		else if (!isFlying && this.m_wasFlying)
		{
			AudioManager.Stop(this.m_flyingAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (isFlying)
		{
			float magnitude = base.EnemyController.Velocity.magnitude;
			float value = 0f;
			if (magnitude > 0f)
			{
				if (magnitude < 5f)
				{
					value = magnitude / 5f;
				}
				else
				{
					value = 1f;
				}
			}
			if (this.m_flyingAudioEventInstance.isValid() && AudioUtility.GetHasParameter(this.m_flyingAudioEventInstance, "dancingBoss_flySpeed"))
			{
				this.m_flyingAudioEventInstance.setParameterByName("dancingBoss_flySpeed", value, false);
			}
		}
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x0001F740 File Offset: 0x0001D940
	private void UpdateIdleAudio(bool isFlying)
	{
		if (isFlying && !this.m_wasFlying)
		{
			AudioManager.Stop(this.m_idleAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			return;
		}
		if (!isFlying)
		{
			PLAYBACK_STATE playback_STATE;
			this.m_idleAudioEventInstance.getPlaybackState(out playback_STATE);
			if (playback_STATE == PLAYBACK_STATE.STOPPED)
			{
				AudioManager.PlayAttached(this, this.m_idleAudioEventInstance, base.gameObject);
			}
		}
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x0001F78C File Offset: 0x0001D98C
	private void BeginVerticalBeamAudio()
	{
		AudioManager.Play(this, this.m_verticalBeamLoopAudioEventInstance);
		this.m_verticalBeamAudioPlaying = true;
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x0001F7A1 File Offset: 0x0001D9A1
	private void EndVerticalBeamAudio()
	{
		AudioManager.Stop(this.m_verticalBeamLoopAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Play(this, this.m_verticalBeamDespawnAudioEventInstance);
		this.m_verticalBeamAudioPlaying = false;
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x0001F7C2 File Offset: 0x0001D9C2
	private void BeginRapidFireAttackAudio()
	{
		if (!this.m_rapidFireAudioPlaying)
		{
			this.m_rapidFireAudioPlaying = true;
			AudioManager.PlayAttached(this, this.m_eyeballShotLoopAudioEventInstance, base.gameObject);
		}
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x0001F7E5 File Offset: 0x0001D9E5
	private void EndRapidFireAttackAudio()
	{
		if (this.m_rapidFireAudioPlaying)
		{
			this.m_rapidFireAudioPlaying = false;
			AudioManager.Stop(this.m_eyeballShotLoopAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x0001F802 File Offset: 0x0001DA02
	private void PlayCircleClosingAudio()
	{
		AudioManager.PlayAttached(this, this.m_voidCircleClosingAudioEventInstance, base.Target);
	}

	// Token: 0x060009DA RID: 2522 RVA: 0x0001F816 File Offset: 0x0001DA16
	private void PlayShieldAttackAudio()
	{
		AudioManager.PlayAttached(this, this.m_shieldAttackLoopAudioEventInstance, base.gameObject);
	}

	// Token: 0x04000DC5 RID: 3525
	private EnemyModeShiftEventArgs m_modeShiftEventArgs;

	// Token: 0x04000DC6 RID: 3526
	private Action<object, HealthChangeEventArgs> m_onBossHit;

	// Token: 0x04000DC7 RID: 3527
	private const float m_global_AnimationOverride = 1f;

	// Token: 0x04000DC8 RID: 3528
	private bool m_hasVoidCircleClosedIn;

	// Token: 0x04000DC9 RID: 3529
	private const string VERTICAL_BEAM_TELL_INTRO = "SummonVerticalBeam_Tell_Intro";

	// Token: 0x04000DCA RID: 3530
	private const string VERTICAL_BEAM_TELL_HOLD = "SummonVerticalBeam_Tell_Hold";

	// Token: 0x04000DCB RID: 3531
	private const string VERTICAL_BEAM_ATTACK_INTRO = "SummonVerticalBeam_Attack_Intro";

	// Token: 0x04000DCC RID: 3532
	private const string VERTICAL_BEAM_ATTACK_HOLD = "SummonVerticalBeam_Attack_Hold";

	// Token: 0x04000DCD RID: 3533
	private const string VERTICAL_BEAM_EXIT = "SummonVerticalBeam_Exit";

	// Token: 0x04000DCE RID: 3534
	private const string VERTICAL_BEAM_PROJECTILE_NAME = "StudyBossVerticalBeamProjectile";

	// Token: 0x04000DCF RID: 3535
	private const float VERTICAL_BEAM_OFFSET = 1f;

	// Token: 0x04000DD0 RID: 3536
	private const string VERTICAL_BEAM_WARNING_PROJECTILE_NAME = "StudyBossVerticalBeamWarningProjectile";

	// Token: 0x04000DD1 RID: 3537
	private float m_verticalBeam_TellIntro_AnimSpeed = 1f;

	// Token: 0x04000DD2 RID: 3538
	private float m_verticalBeam_TellHold_AnimSpeed = 1f;

	// Token: 0x04000DD3 RID: 3539
	private float m_verticalBeam_TellIntroAndHold_Delay = 1.75f;

	// Token: 0x04000DD4 RID: 3540
	private float m_verticalBeam_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04000DD5 RID: 3541
	private float m_verticalBeam_AttackIntro_Delay;

	// Token: 0x04000DD6 RID: 3542
	private float m_verticalBeam_AttackHold_AnimSpeed = 1f;

	// Token: 0x04000DD7 RID: 3543
	private float m_verticalBeam_AttackHold_Delay = 1f;

	// Token: 0x04000DD8 RID: 3544
	private float m_verticalBeam_Exit_AnimSpeed = 1f;

	// Token: 0x04000DD9 RID: 3545
	private float m_verticalBeam_Exit_Delay = 0.45f;

	// Token: 0x04000DDA RID: 3546
	private float m_verticalBeam_Exit_IdleDuration = 0.15f;

	// Token: 0x04000DDB RID: 3547
	private float m_verticalBeam_AttackCD = 18f;

	// Token: 0x04000DDC RID: 3548
	private bool m_verticalBeam_stopMovingWhileAttacking = true;

	// Token: 0x04000DDD RID: 3549
	private Projectile_RL[] m_verticalBeamWarningProjectiles;

	// Token: 0x04000DDE RID: 3550
	private Projectile_RL[] m_verticalBeamProjectiles;

	// Token: 0x04000DDF RID: 3551
	private bool m_verticalBeamAudioPlaying;

	// Token: 0x04000DE0 RID: 3552
	private WaitForSeconds m_warningWaitTime;

	// Token: 0x04000DE1 RID: 3553
	private WaitForSeconds m_beamWaitTime;

	// Token: 0x04000DE2 RID: 3554
	private const string RAPID_FIRE_TELL_INTRO = "EyeballShot_Tell_Intro";

	// Token: 0x04000DE3 RID: 3555
	private const string RAPID_FIRE_TELL_HOLD = "EyeballShot_Tell_Hold";

	// Token: 0x04000DE4 RID: 3556
	private const string RAPID_FIRE_ATTACK_INTRO = "EyeballShot_Attack_Intro";

	// Token: 0x04000DE5 RID: 3557
	private const string RAPID_FIRE_ATTACK_HOLD = "EyeballShot_Attack_Hold";

	// Token: 0x04000DE6 RID: 3558
	private const string RAPID_FIRE_EXIT = "EyeballShot_Exit";

	// Token: 0x04000DE7 RID: 3559
	private const string RAPID_FIRE_PROJECTILE_NAME = "StudyBossRapidFireProjectile";

	// Token: 0x04000DE8 RID: 3560
	private const string RAPID_FIRE_VOID_PROJECTILE_NAME = "StudyBossRapidFireVoidProjectile";

	// Token: 0x04000DE9 RID: 3561
	private float m_rapidFire_TellIntro_AnimSpeed = 1f;

	// Token: 0x04000DEA RID: 3562
	private float m_rapidFire_TellHold_AnimSpeed = 1f;

	// Token: 0x04000DEB RID: 3563
	private float m_rapidFire_TellIntroAndHold_Delay = 1.5f;

	// Token: 0x04000DEC RID: 3564
	private float m_rapidFire_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04000DED RID: 3565
	private float m_rapidFire_AttackIntro_Delay;

	// Token: 0x04000DEE RID: 3566
	private float m_rapidFire_AttackHold_AnimSpeed = 1f;

	// Token: 0x04000DEF RID: 3567
	private float m_rapidFire_AttackHold_Delay = 3.25f;

	// Token: 0x04000DF0 RID: 3568
	private float m_rapidFire_Exit_AnimSpeed = 1f;

	// Token: 0x04000DF1 RID: 3569
	private float m_rapidFire_Exit_Delay = 0.45f;

	// Token: 0x04000DF2 RID: 3570
	private float m_rapidFire_Exit_IdleDuration = 0.15f;

	// Token: 0x04000DF3 RID: 3571
	private float m_rapidFire_AttackCD = 15f;

	// Token: 0x04000DF4 RID: 3572
	private float m_rapidFire_MaxRandomAngleDeviation = 60f;

	// Token: 0x04000DF5 RID: 3573
	private float m_rapidFire_SpeedMod = 1f;

	// Token: 0x04000DF6 RID: 3574
	private float m_rapidFire_VoidProjectileSpeedMod = 1f;

	// Token: 0x04000DF7 RID: 3575
	private float m_rapidFire_VoidShotDelay = 0.15f;

	// Token: 0x04000DF8 RID: 3576
	private float m_rapidFire_VoidShotAdvancedDelay = 0.25f;

	// Token: 0x04000DF9 RID: 3577
	private bool m_rapidFire_stopMovingWhileAttacking = true;

	// Token: 0x04000DFA RID: 3578
	private const string CIRCLE_ATTACK_TELL_INTRO = "SummonCircularBullets_Tell_Intro";

	// Token: 0x04000DFB RID: 3579
	private const string CIRCLE_ATTACK_TELL_HOLD = "SummonCircularBullets_Tell_Hold";

	// Token: 0x04000DFC RID: 3580
	private const string CIRCLE_ATTACK_ATTACK_INTRO = "SummonCircularBullets_Attack_Intro";

	// Token: 0x04000DFD RID: 3581
	private const string CIRCLE_ATTACK_ATTACK_HOLD = "SummonCircularBullets_Attack_Hold";

	// Token: 0x04000DFE RID: 3582
	private const string CIRCLE_ATTACK_EXIT = "SummonCircularBullets_Exit";

	// Token: 0x04000DFF RID: 3583
	private const string CIRCLE_ATTACK_PROJECTILE_NAME = "StudyBossCircleProjectile";

	// Token: 0x04000E00 RID: 3584
	private float m_circleAttack_TellIntro_AnimSpeed = 1f;

	// Token: 0x04000E01 RID: 3585
	private float m_circleAttack_TellHold_AnimSpeed = 1f;

	// Token: 0x04000E02 RID: 3586
	private float m_circleAttack_TellIntroAndHold_Delay = 1.5f;

	// Token: 0x04000E03 RID: 3587
	private float m_circleAttack_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04000E04 RID: 3588
	private float m_circleAttack_AttackIntro_Delay;

	// Token: 0x04000E05 RID: 3589
	private float m_circleAttack_AttackHold_AnimSpeed = 1f;

	// Token: 0x04000E06 RID: 3590
	private float m_circleAttack_AttackHold_Delay = 1.25f;

	// Token: 0x04000E07 RID: 3591
	private float m_circleAttack_Exit_AnimSpeed = 1f;

	// Token: 0x04000E08 RID: 3592
	private float m_circleAttack_Exit_Delay = 0.45f;

	// Token: 0x04000E09 RID: 3593
	private float m_circleAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x04000E0A RID: 3594
	private float m_circleAttack_AttackCD = 12f;

	// Token: 0x04000E0B RID: 3595
	private bool m_circleAttack_stopMovingWhileAttacking;

	// Token: 0x04000E0C RID: 3596
	private const string BOMB_ATTACK_TELL_INTRO = "SummonBomb_Tell_Intro";

	// Token: 0x04000E0D RID: 3597
	private const string BOMB_ATTACK_TELL_HOLD = "SummonBomb_Tell_Hold";

	// Token: 0x04000E0E RID: 3598
	private const string BOMB_ATTACK_ATTACK_INTRO = "SummonBomb_Attack_Intro";

	// Token: 0x04000E0F RID: 3599
	private const string BOMB_ATTACK_ATTACK_HOLD = "SummonBomb_Attack_Hold";

	// Token: 0x04000E10 RID: 3600
	private const string BOMB_ATTACK_EXIT = "SummonBomb_Exit";

	// Token: 0x04000E11 RID: 3601
	private float m_bombAttack_TellIntro_AnimSpeed = 1f;

	// Token: 0x04000E12 RID: 3602
	private float m_bombAttack_TellHold_AnimSpeed = 1f;

	// Token: 0x04000E13 RID: 3603
	private float m_bombAttack_TellIntroAndHold_Delay = 2.25f;

	// Token: 0x04000E14 RID: 3604
	private float m_bombAttack_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04000E15 RID: 3605
	private float m_bombAttack_AttackIntro_Delay;

	// Token: 0x04000E16 RID: 3606
	private float m_bombAttack_AttackHold_AnimSpeed = 1f;

	// Token: 0x04000E17 RID: 3607
	private float m_bombAttack_AttackHold_Delay;

	// Token: 0x04000E18 RID: 3608
	private float m_bombAttack_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000E19 RID: 3609
	private float m_bombAttack_Exit_Delay = 0.45f;

	// Token: 0x04000E1A RID: 3610
	private float m_bombAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x04000E1B RID: 3611
	private float m_bombAttack_AttackCD = 12f;

	// Token: 0x04000E1C RID: 3612
	private float m_bombAttack_TimeToExplosion;

	// Token: 0x04000E1D RID: 3613
	private bool m_bombAttack_stopMovingWhileAttacking = true;

	// Token: 0x04000E1E RID: 3614
	private float m_bombAttack_Displace_X = 10f;

	// Token: 0x04000E1F RID: 3615
	private float m_bombAttack_Displace_Y = 3.75f;

	// Token: 0x04000E20 RID: 3616
	private Projectile_RL m_bombWarningProjectile;

	// Token: 0x04000E21 RID: 3617
	private Projectile_RL m_bombWarningProjectile2;

	// Token: 0x04000E22 RID: 3618
	private Projectile_RL m_bombWarningProjectile3;

	// Token: 0x04000E23 RID: 3619
	private Projectile_RL m_bombWarningProjectile4;

	// Token: 0x04000E24 RID: 3620
	private Projectile_RL m_bombWarningProjectile5;

	// Token: 0x04000E25 RID: 3621
	private Relay m_bombAttackWarningAppearedRelay = new Relay();

	// Token: 0x04000E26 RID: 3622
	private Relay m_bombAttackExplodedRelay = new Relay();

	// Token: 0x04000E27 RID: 3623
	private const string SHOUT_TELL_INTRO = "Shout_Tell_Intro";

	// Token: 0x04000E28 RID: 3624
	private const string SHOUT_TELL_HOLD = "Shout_Tell_Hold";

	// Token: 0x04000E29 RID: 3625
	private const string SHOUT_ATTACK_INTRO = "Shout_Attack_Intro";

	// Token: 0x04000E2A RID: 3626
	private const string SHOUT_ATTACK_HOLD = "Shout_Attack_Hold";

	// Token: 0x04000E2B RID: 3627
	private const string SHOUT_EXIT = "Shout_Exit";

	// Token: 0x04000E2C RID: 3628
	private const string SHOUT_ATTACK_EXPLOSION_PROJECTILE_NAME = "StudyBossShoutExplosionProjectile";

	// Token: 0x04000E2D RID: 3629
	private const string SHOUT_ATTACK_WARNING_PROJECTILE_NAME = "StudyBossShoutWarningProjectile";

	// Token: 0x04000E2E RID: 3630
	private float m_shout_TellIntro_AnimSpeed = 1f;

	// Token: 0x04000E2F RID: 3631
	private float m_shout_TellHold_AnimSpeed = 1f;

	// Token: 0x04000E30 RID: 3632
	private float m_shout_TellIntroAndHold_Delay = 1.5f;

	// Token: 0x04000E31 RID: 3633
	private float m_shout_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04000E32 RID: 3634
	private float m_shout_AttackIntro_Delay;

	// Token: 0x04000E33 RID: 3635
	private float m_shout_AttackHold_AnimSpeed = 1f;

	// Token: 0x04000E34 RID: 3636
	private float m_shout_AttackHold_Delay;

	// Token: 0x04000E35 RID: 3637
	private float m_shout_Exit_AnimSpeed = 0.65f;

	// Token: 0x04000E36 RID: 3638
	private float m_shout_Exit_Delay;

	// Token: 0x04000E37 RID: 3639
	private float m_shout_Exit_IdleDuration = 0.35f;

	// Token: 0x04000E38 RID: 3640
	private float m_shout_AttackCD = 12f;

	// Token: 0x04000E39 RID: 3641
	private float m_shoutAttackTimeToExplosion;

	// Token: 0x04000E3A RID: 3642
	private bool m_shout_stopMovingWhileAttacking = true;

	// Token: 0x04000E3B RID: 3643
	private float m_shoutAttackDelaySecondExplosion = 0.65f;

	// Token: 0x04000E3C RID: 3644
	private int m_shoutAttackDelaySecondExplosionAngleAdd = 10;

	// Token: 0x04000E3D RID: 3645
	private Projectile_RL m_shoutWarningProjectile;

	// Token: 0x04000E3E RID: 3646
	private Relay m_shoutAttackWarningAppearedRelay = new Relay();

	// Token: 0x04000E3F RID: 3647
	private Relay m_shoutAttackExplodedRelay = new Relay();

	// Token: 0x04000E40 RID: 3648
	private const string DASH_ATTACK_TELL_INTRO = "Dash_Tell_Intro";

	// Token: 0x04000E41 RID: 3649
	private const string DASH_ATTACK_TELL_HOLD = "Dash_Tell_Hold";

	// Token: 0x04000E42 RID: 3650
	private const string DASH_ATTACK_ATTACK_INTRO = "Dash_Attack_Intro";

	// Token: 0x04000E43 RID: 3651
	private const string DASH_ATTACK_ATTACK_HOLD = "Dash_Attack_Hold";

	// Token: 0x04000E44 RID: 3652
	private const string DASH_ATTACK_EXIT = "Dash_Exit";

	// Token: 0x04000E45 RID: 3653
	private const string CURSE_ATTACK_RED_PROJECTILE_NAME = "StudyBossFlowerProjectile";

	// Token: 0x04000E46 RID: 3654
	private float m_dashAttack_TellIntro_AnimSpeed = 1f;

	// Token: 0x04000E47 RID: 3655
	private float m_dashAttack_TellHold_AnimSpeed = 1f;

	// Token: 0x04000E48 RID: 3656
	private float m_dashAttack_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x04000E49 RID: 3657
	private float m_dashAttack_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04000E4A RID: 3658
	private float m_dashAttack_AttackIntro_Delay;

	// Token: 0x04000E4B RID: 3659
	private float m_dashAttack_AttackHold_AnimSpeed = 1f;

	// Token: 0x04000E4C RID: 3660
	private float m_dashAttack_AttackHold_Delay = 1f;

	// Token: 0x04000E4D RID: 3661
	private float m_dashAttack_Exit_AnimSpeed = 1f;

	// Token: 0x04000E4E RID: 3662
	private float m_dashAttack_Exit_Delay = 0.45f;

	// Token: 0x04000E4F RID: 3663
	private float m_dashAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x04000E50 RID: 3664
	private float m_dashAttack_AttackCD = 12f;

	// Token: 0x04000E51 RID: 3665
	private float m_dashAttackSpeed = 16.5f;

	// Token: 0x04000E52 RID: 3666
	private float m_dashAttackFireballSpeedMod = 1f;

	// Token: 0x04000E53 RID: 3667
	private const string SHIELD_ATTACK_PROJECTILE_NAME = "StudyBossShieldProjectile";

	// Token: 0x04000E54 RID: 3668
	private float m_shieldAttackRadius = 13.5f;

	// Token: 0x04000E55 RID: 3669
	private float m_shieldAttackOrbitSpeed = 25f;

	// Token: 0x04000E56 RID: 3670
	private float m_shieldAttackTimeBetweenProjectileRespawn = 5f;

	// Token: 0x04000E57 RID: 3671
	private bool m_isShieldAttackRunning;

	// Token: 0x04000E58 RID: 3672
	private const string MODE_SHIFT_INTRO = "ModeShift_Intro";

	// Token: 0x04000E59 RID: 3673
	private const string MODE_SHIFT_SCREAM_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x04000E5A RID: 3674
	private const string MODE_SHIFT_SCREAM_HOLD = "ModeShift_Scream_Hold";

	// Token: 0x04000E5B RID: 3675
	private const string MODE_SHIFT_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x04000E5C RID: 3676
	private float m_modeShift_Intro_AnimSpeed = 1f;

	// Token: 0x04000E5D RID: 3677
	private float m_modeShift_Intro_Delay = 1.5f;

	// Token: 0x04000E5E RID: 3678
	private float m_modeShift_Scream_Intro_AnimSpeed = 1f;

	// Token: 0x04000E5F RID: 3679
	private float m_modeShift_Scream_Intro_Delay;

	// Token: 0x04000E60 RID: 3680
	private float m_modeShift_Scream_Hold_AnimSpeed = 1f;

	// Token: 0x04000E61 RID: 3681
	private float m_modeShift_Scream_Hold_Delay = 1.25f;

	// Token: 0x04000E62 RID: 3682
	private float m_modeShift_Exit_AnimSpeed = 1f;

	// Token: 0x04000E63 RID: 3683
	private float m_modeShift_Exit_Delay = 0.5f;

	// Token: 0x04000E64 RID: 3684
	private float m_modeShift_IdleDuration = 1f;

	// Token: 0x04000E65 RID: 3685
	private float m_modeShift_AttackCD = 99999f;

	// Token: 0x04000E66 RID: 3686
	private float m_modeShift_HealthMod = 0.5f;

	// Token: 0x04000E67 RID: 3687
	private float m_modeShift_DamageMod = 0.1f;

	// Token: 0x04000E68 RID: 3688
	protected const string SPAWN_IDLE = "Intro_Idle";

	// Token: 0x04000E69 RID: 3689
	protected const string SPAWN_INTRO = "Intro";

	// Token: 0x04000E6A RID: 3690
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x04000E6B RID: 3691
	protected float m_spawn_Idle_Delay = 2f;

	// Token: 0x04000E6C RID: 3692
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x04000E6D RID: 3693
	protected float m_spawn_Intro_Delay;

	// Token: 0x04000E6E RID: 3694
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x04000E6F RID: 3695
	protected const string DEATH_HOLD = "Death_Loop";

	// Token: 0x04000E70 RID: 3696
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x04000E71 RID: 3697
	protected float m_death_Intro_Delay;

	// Token: 0x04000E72 RID: 3698
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x04000E73 RID: 3699
	protected float m_death_Hold_Delay = 4.5f;

	// Token: 0x04000E74 RID: 3700
	private const string PHASE2_HIT_AUDIO_PATH = "event:/SFX/Enemies/sfx_studyBoss_phase2_hit_start";

	// Token: 0x04000E75 RID: 3701
	private const string FLYING_AUDIO_PATH = "event:/SFX/Enemies/sfx_studyBoss_fly_loop";

	// Token: 0x04000E76 RID: 3702
	private const string FLYING_SPEED_PARAMETER = "dancingBoss_flySpeed";

	// Token: 0x04000E77 RID: 3703
	private const string IDLE_AUDIO_PATH = "event:/SFX/Enemies/sfx_studyBoss_idle_loop";

	// Token: 0x04000E78 RID: 3704
	private const string VERTICAL_BEAM_LOOP_AUDIO_PATH = "event:/SFX/Enemies/sfx_studyBoss_attack_skyBeam_spawn_start_loop";

	// Token: 0x04000E79 RID: 3705
	private const string VERTICAL_BEAM_DESPAWN_AUDIO_PATH = "event:/SFX/Enemies/sfx_studyBoss_attack_skyBeam_despawn";

	// Token: 0x04000E7A RID: 3706
	private const string VOID_CIRCLE_CLOSING_AUDIO_PATH = "event:/SFX/Enemies/sfx_studyBoss_attack_voidCircle_closeIn";

	// Token: 0x04000E7B RID: 3707
	private const string EYEBALL_SHOT_LOOP_AUDIO_PATH = "event:/SFX/Enemies/sfx_studyBoss_attack_eye_shoot_start_loop";

	// Token: 0x04000E7C RID: 3708
	private const string SHIELD_ATTACK_LOOP_AUDIO_PATH = "event:/SFX/Enemies/sfx_studyBoss_phase2_voidProjectiles_loop";

	// Token: 0x04000E7D RID: 3709
	private const float MAX_FLYING_SPEED = 5f;

	// Token: 0x04000E7E RID: 3710
	private bool m_wasFlying;

	// Token: 0x04000E7F RID: 3711
	private EventInstance m_flyingAudioEventInstance;

	// Token: 0x04000E80 RID: 3712
	private EventInstance m_idleAudioEventInstance;

	// Token: 0x04000E81 RID: 3713
	private EventInstance m_verticalBeamLoopAudioEventInstance;

	// Token: 0x04000E82 RID: 3714
	private EventInstance m_verticalBeamDespawnAudioEventInstance;

	// Token: 0x04000E83 RID: 3715
	private EventInstance m_voidCircleClosingAudioEventInstance;

	// Token: 0x04000E84 RID: 3716
	private EventInstance m_eyeballShotLoopAudioEventInstance;

	// Token: 0x04000E85 RID: 3717
	private EventInstance m_shieldAttackLoopAudioEventInstance;

	// Token: 0x04000E86 RID: 3718
	private bool m_rapidFireAudioPlaying;
}

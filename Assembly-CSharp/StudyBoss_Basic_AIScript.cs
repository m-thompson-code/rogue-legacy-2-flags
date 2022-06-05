using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000216 RID: 534
public class StudyBoss_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000EA4 RID: 3748 RVA: 0x0007534C File Offset: 0x0007354C
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

	// Token: 0x170006D8 RID: 1752
	// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x000080F2 File Offset: 0x000062F2
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.4f, 0.8f);
		}
	}

	// Token: 0x170006D9 RID: 1753
	// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x000080F2 File Offset: 0x000062F2
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.4f, 0.8f);
		}
	}

	// Token: 0x170006DA RID: 1754
	// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x000080F2 File Offset: 0x000062F2
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.4f, 0.8f);
		}
	}

	// Token: 0x170006DB RID: 1755
	// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x00008103 File Offset: 0x00006303
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-2f, 2f);
		}
	}

	// Token: 0x170006DC RID: 1756
	// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x00008114 File Offset: 0x00006314
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(3f, 3f);
		}
	}

	// Token: 0x170006DD RID: 1757
	// (get) Token: 0x06000EAA RID: 3754 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_advancedAttack
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170006DE RID: 1758
	// (get) Token: 0x06000EAB RID: 3755 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool is_Variant
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170006DF RID: 1759
	// (get) Token: 0x06000EAC RID: 3756 RVA: 0x00008125 File Offset: 0x00006325
	protected virtual string CURSE_ATTACK_PROJECTILE_NAME
	{
		get
		{
			return "StudyBossCurseProjectile";
		}
	}

	// Token: 0x170006E0 RID: 1760
	// (get) Token: 0x06000EAD RID: 3757 RVA: 0x0000812C File Offset: 0x0000632C
	protected virtual string CURSE_ATTACK_ADVANCED_PROJECTILE_NAME
	{
		get
		{
			return "StudyBossCurseAdvancedProjectile";
		}
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x00008133 File Offset: 0x00006333
	private void Awake()
	{
		this.m_onBossHit = new Action<object, HealthChangeEventArgs>(this.OnBossHit);
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x00008147 File Offset: 0x00006347
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.LogicController.DisableLogicActivationByDistance = true;
		this.m_verticalBeamWarningProjectiles = new Projectile_RL[this.m_verticalBeamAttackCount];
		this.m_verticalBeamProjectiles = new Projectile_RL[this.m_verticalBeamAttackCount];
		this.InitializeAudio();
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x00008184 File Offset: 0x00006384
	public override void OnEnemyActivated()
	{
		base.OnEnemyActivated();
		base.EnemyController.HealthChangeRelay.AddListener(this.m_onBossHit, false);
	}

	// Token: 0x170006E1 RID: 1761
	// (get) Token: 0x06000EB1 RID: 3761 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_verticalBeamAttackCount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170006E2 RID: 1762
	// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_timeBetweenVerticalBeamAttacks
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170006E3 RID: 1763
	// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected virtual float m_verticalBeamWarningLifetime
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x170006E4 RID: 1764
	// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x000081A4 File Offset: 0x000063A4
	protected virtual float m_verticalBeamLifetime
	{
		get
		{
			return 8f;
		}
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x000081AB File Offset: 0x000063AB
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

	// Token: 0x06000EB6 RID: 3766 RVA: 0x000081BA File Offset: 0x000063BA
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

	// Token: 0x06000EB7 RID: 3767 RVA: 0x000081D0 File Offset: 0x000063D0
	private Vector2 GetVerticalBeamProjectileOffset()
	{
		Vector2 zero = Vector2.zero;
		return this.GetVerticalBeamOrigin();
	}

	// Token: 0x06000EB8 RID: 3768 RVA: 0x000753F4 File Offset: 0x000735F4
	private Vector2 GetVerticalBeamOrigin()
	{
		Vector2 vector = PlayerManager.GetPlayerController().Midpoint;
		float y = (base.EnemyController.Room as Room).Bounds.max.y + 1f;
		return new Vector2(vector.x, y);
	}

	// Token: 0x170006E5 RID: 1765
	// (get) Token: 0x06000EB9 RID: 3769 RVA: 0x000081DE File Offset: 0x000063DE
	protected virtual int m_rapidFireShotCount
	{
		get
		{
			return 35;
		}
	}

	// Token: 0x170006E6 RID: 1766
	// (get) Token: 0x06000EBA RID: 3770 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_rapidFire_ShootVoidBulletAtEnd
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x000081E2 File Offset: 0x000063E2
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

	// Token: 0x06000EBC RID: 3772 RVA: 0x00075448 File Offset: 0x00073648
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

	// Token: 0x170006E7 RID: 1767
	// (get) Token: 0x06000EBD RID: 3773 RVA: 0x00003E42 File Offset: 0x00002042
	protected virtual int m_circleAttackProjectileCount
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x170006E8 RID: 1768
	// (get) Token: 0x06000EBE RID: 3774 RVA: 0x000081A4 File Offset: 0x000063A4
	protected virtual float m_circleAttackInitialRadius
	{
		get
		{
			return 8f;
		}
	}

	// Token: 0x170006E9 RID: 1769
	// (get) Token: 0x06000EBF RID: 3775 RVA: 0x00005319 File Offset: 0x00003519
	protected virtual float m_circleAttackInnerRadius
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x170006EA RID: 1770
	// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x00004536 File Offset: 0x00002736
	protected virtual float m_circleAttackTimeBeforeCircleBeginsToClose
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170006EB RID: 1771
	// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_circleAttackTimeToClose
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170006EC RID: 1772
	// (get) Token: 0x06000EC2 RID: 3778 RVA: 0x000081F1 File Offset: 0x000063F1
	protected virtual float m_circleAttackOrbitSpeed
	{
		get
		{
			return 360f;
		}
	}

	// Token: 0x06000EC3 RID: 3779 RVA: 0x000081F8 File Offset: 0x000063F8
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

	// Token: 0x06000EC4 RID: 3780 RVA: 0x000754A4 File Offset: 0x000736A4
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

	// Token: 0x06000EC5 RID: 3781 RVA: 0x00008207 File Offset: 0x00006407
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

	// Token: 0x06000EC6 RID: 3782 RVA: 0x000754E8 File Offset: 0x000736E8
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

	// Token: 0x170006ED RID: 1773
	// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x0000821D File Offset: 0x0000641D
	protected virtual string BOMB_ATTACK_EXPLOSION_PROJECTILE_NAME
	{
		get
		{
			return "StudyBossBombExplosionProjectile";
		}
	}

	// Token: 0x170006EE RID: 1774
	// (get) Token: 0x06000EC8 RID: 3784 RVA: 0x00008224 File Offset: 0x00006424
	protected virtual string BOMB_ATTACK_EXPLOSION_ADVANCED_PROJECTILE_NAME
	{
		get
		{
			return "StudyBossBombExplosionAdvancedProjectile";
		}
	}

	// Token: 0x170006EF RID: 1775
	// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x0000822B File Offset: 0x0000642B
	protected virtual string BOMB_ATTACK_WARNING_PROJECTILE_NAME
	{
		get
		{
			return "StudyBossBombWarningProjectile";
		}
	}

	// Token: 0x170006F0 RID: 1776
	// (get) Token: 0x06000ECA RID: 3786 RVA: 0x00008232 File Offset: 0x00006432
	protected virtual string BOMB_ATTACK_WARNING_ADVANCED_PROJECTILE_NAME
	{
		get
		{
			return "StudyBossBombWarningAdvancedProjectile";
		}
	}

	// Token: 0x170006F1 RID: 1777
	// (get) Token: 0x06000ECB RID: 3787 RVA: 0x00008239 File Offset: 0x00006439
	protected virtual string BOMB_ATTACK_FIREBALL_PROJECTILE_NAME
	{
		get
		{
			return "StudyBossBombFireballProjectile";
		}
	}

	// Token: 0x170006F2 RID: 1778
	// (get) Token: 0x06000ECC RID: 3788 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_bombAttack_FireballCountAtEnd
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170006F3 RID: 1779
	// (get) Token: 0x06000ECD RID: 3789 RVA: 0x00008240 File Offset: 0x00006440
	public IRelayLink BombAttackWarningAppearedRelay
	{
		get
		{
			return this.m_bombAttackWarningAppearedRelay.link;
		}
	}

	// Token: 0x170006F4 RID: 1780
	// (get) Token: 0x06000ECE RID: 3790 RVA: 0x0000824D File Offset: 0x0000644D
	public IRelayLink BombAttackExplodedRelay
	{
		get
		{
			return this.m_bombAttackExplodedRelay.link;
		}
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x0000825A File Offset: 0x0000645A
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

	// Token: 0x170006F5 RID: 1781
	// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x00008269 File Offset: 0x00006469
	protected virtual int m_shout_fireballsAtEnd
	{
		get
		{
			return 18;
		}
	}

	// Token: 0x170006F6 RID: 1782
	// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x0000826D File Offset: 0x0000646D
	public IRelayLink ShoutAttackWarningAppearedRelay
	{
		get
		{
			return this.m_shoutAttackWarningAppearedRelay.link;
		}
	}

	// Token: 0x170006F7 RID: 1783
	// (get) Token: 0x06000ED2 RID: 3794 RVA: 0x0000827A File Offset: 0x0000647A
	public IRelayLink ShoutAttackExplodedRelay
	{
		get
		{
			return this.m_shoutAttackExplodedRelay.link;
		}
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x00008287 File Offset: 0x00006487
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

	// Token: 0x170006F8 RID: 1784
	// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual int m_dashAttackFireballCountAtEnd
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x06000ED5 RID: 3797 RVA: 0x00008296 File Offset: 0x00006496
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

	// Token: 0x170006F9 RID: 1785
	// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x000047A7 File Offset: 0x000029A7
	protected virtual int m_shieldAttackProjectileCount
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x000082A5 File Offset: 0x000064A5
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

	// Token: 0x06000ED8 RID: 3800 RVA: 0x000082DF File Offset: 0x000064DF
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

	// Token: 0x06000ED9 RID: 3801 RVA: 0x000082F5 File Offset: 0x000064F5
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

	// Token: 0x06000EDA RID: 3802 RVA: 0x00008304 File Offset: 0x00006504
	public override IEnumerator SpawnAnim()
	{
		yield return this.Default_Animation("Intro_Idle", this.m_spawn_Idle_AnimSpeed, this.m_spawn_Idle_Delay, true);
		yield return this.Default_Animation("Intro", this.m_spawn_Intro_AnimSpeed, this.m_spawn_Intro_Delay, true);
		MusicManager.PlayMusic(SongID.StudyBossBGM_Boss5_Phase1, false, false);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 1f, false);
		yield break;
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x00008313 File Offset: 0x00006513
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

	// Token: 0x06000EDC RID: 3804 RVA: 0x00075564 File Offset: 0x00073764
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

	// Token: 0x06000EDD RID: 3805 RVA: 0x00008322 File Offset: 0x00006522
	public override void OnLBCompleteOrCancelled()
	{
		this.DisableWarningProjectiles();
		base.EnemyController.LockFlip = false;
		this.EndRapidFireAttackAudio();
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x00008342 File Offset: 0x00006542
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

	// Token: 0x06000EDF RID: 3807 RVA: 0x0007560C File Offset: 0x0007380C
	private void DisableWarningProjectiles()
	{
		base.StopProjectile(ref this.m_shoutWarningProjectile);
		base.StopProjectile(ref this.m_bombWarningProjectile);
		base.StopProjectile(ref this.m_bombWarningProjectile2);
		base.StopProjectile(ref this.m_bombWarningProjectile3);
		base.StopProjectile(ref this.m_bombWarningProjectile4);
		base.StopProjectile(ref this.m_bombWarningProjectile5);
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x00075664 File Offset: 0x00073864
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

	// Token: 0x06000EE1 RID: 3809 RVA: 0x000756B4 File Offset: 0x000738B4
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

	// Token: 0x06000EE2 RID: 3810 RVA: 0x00008364 File Offset: 0x00006564
	public override void ResetScript()
	{
		this.m_isShieldAttackRunning = false;
		base.EnemyController.StatusEffectController.ImmuneToAllStatusEffects = false;
		base.ResetScript();
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x00075724 File Offset: 0x00073924
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

	// Token: 0x06000EE4 RID: 3812 RVA: 0x00008384 File Offset: 0x00006584
	private void DisableAudio()
	{
		AudioManager.Stop(this.m_flyingAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Stop(this.m_idleAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Stop(this.m_verticalBeamLoopAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Stop(this.m_eyeballShotLoopAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Stop(this.m_shieldAttackLoopAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x06000EE5 RID: 3813 RVA: 0x000757CC File Offset: 0x000739CC
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

	// Token: 0x06000EE6 RID: 3814 RVA: 0x000083C2 File Offset: 0x000065C2
	protected override void PlayDeathAnimAudio()
	{
		if (base.EnemyController.CurrentHealth > 0f)
		{
			AudioManager.PlayOneShot(this, "event:/SFX/Enemies/sfx_studyBoss_phase2_hit_start", base.gameObject.transform.position);
			return;
		}
		base.PlayDeathAnimAudio();
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x00075888 File Offset: 0x00073A88
	private void Update()
	{
		bool flag = base.EnemyController.Velocity.sqrMagnitude > 0f;
		this.UpdateFlyingAudio(flag);
		this.UpdateIdleAudio(flag);
		this.m_wasFlying = flag;
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x000758C8 File Offset: 0x00073AC8
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

	// Token: 0x06000EE9 RID: 3817 RVA: 0x0007597C File Offset: 0x00073B7C
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

	// Token: 0x06000EEA RID: 3818 RVA: 0x000083F8 File Offset: 0x000065F8
	private void BeginVerticalBeamAudio()
	{
		AudioManager.Play(this, this.m_verticalBeamLoopAudioEventInstance);
		this.m_verticalBeamAudioPlaying = true;
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x0000840D File Offset: 0x0000660D
	private void EndVerticalBeamAudio()
	{
		AudioManager.Stop(this.m_verticalBeamLoopAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Play(this, this.m_verticalBeamDespawnAudioEventInstance);
		this.m_verticalBeamAudioPlaying = false;
	}

	// Token: 0x06000EEC RID: 3820 RVA: 0x0000842E File Offset: 0x0000662E
	private void BeginRapidFireAttackAudio()
	{
		if (!this.m_rapidFireAudioPlaying)
		{
			this.m_rapidFireAudioPlaying = true;
			AudioManager.PlayAttached(this, this.m_eyeballShotLoopAudioEventInstance, base.gameObject);
		}
	}

	// Token: 0x06000EED RID: 3821 RVA: 0x00008451 File Offset: 0x00006651
	private void EndRapidFireAttackAudio()
	{
		if (this.m_rapidFireAudioPlaying)
		{
			this.m_rapidFireAudioPlaying = false;
			AudioManager.Stop(this.m_eyeballShotLoopAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06000EEE RID: 3822 RVA: 0x0000846E File Offset: 0x0000666E
	private void PlayCircleClosingAudio()
	{
		AudioManager.PlayAttached(this, this.m_voidCircleClosingAudioEventInstance, base.Target);
	}

	// Token: 0x06000EEF RID: 3823 RVA: 0x00008482 File Offset: 0x00006682
	private void PlayShieldAttackAudio()
	{
		AudioManager.PlayAttached(this, this.m_shieldAttackLoopAudioEventInstance, base.gameObject);
	}

	// Token: 0x04001179 RID: 4473
	private EnemyModeShiftEventArgs m_modeShiftEventArgs;

	// Token: 0x0400117A RID: 4474
	private Action<object, HealthChangeEventArgs> m_onBossHit;

	// Token: 0x0400117B RID: 4475
	private const float m_global_AnimationOverride = 1f;

	// Token: 0x0400117C RID: 4476
	private bool m_hasVoidCircleClosedIn;

	// Token: 0x0400117D RID: 4477
	private const string VERTICAL_BEAM_TELL_INTRO = "SummonVerticalBeam_Tell_Intro";

	// Token: 0x0400117E RID: 4478
	private const string VERTICAL_BEAM_TELL_HOLD = "SummonVerticalBeam_Tell_Hold";

	// Token: 0x0400117F RID: 4479
	private const string VERTICAL_BEAM_ATTACK_INTRO = "SummonVerticalBeam_Attack_Intro";

	// Token: 0x04001180 RID: 4480
	private const string VERTICAL_BEAM_ATTACK_HOLD = "SummonVerticalBeam_Attack_Hold";

	// Token: 0x04001181 RID: 4481
	private const string VERTICAL_BEAM_EXIT = "SummonVerticalBeam_Exit";

	// Token: 0x04001182 RID: 4482
	private const string VERTICAL_BEAM_PROJECTILE_NAME = "StudyBossVerticalBeamProjectile";

	// Token: 0x04001183 RID: 4483
	private const float VERTICAL_BEAM_OFFSET = 1f;

	// Token: 0x04001184 RID: 4484
	private const string VERTICAL_BEAM_WARNING_PROJECTILE_NAME = "StudyBossVerticalBeamWarningProjectile";

	// Token: 0x04001185 RID: 4485
	private float m_verticalBeam_TellIntro_AnimSpeed = 1f;

	// Token: 0x04001186 RID: 4486
	private float m_verticalBeam_TellHold_AnimSpeed = 1f;

	// Token: 0x04001187 RID: 4487
	private float m_verticalBeam_TellIntroAndHold_Delay = 1.75f;

	// Token: 0x04001188 RID: 4488
	private float m_verticalBeam_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04001189 RID: 4489
	private float m_verticalBeam_AttackIntro_Delay;

	// Token: 0x0400118A RID: 4490
	private float m_verticalBeam_AttackHold_AnimSpeed = 1f;

	// Token: 0x0400118B RID: 4491
	private float m_verticalBeam_AttackHold_Delay = 1f;

	// Token: 0x0400118C RID: 4492
	private float m_verticalBeam_Exit_AnimSpeed = 1f;

	// Token: 0x0400118D RID: 4493
	private float m_verticalBeam_Exit_Delay = 0.45f;

	// Token: 0x0400118E RID: 4494
	private float m_verticalBeam_Exit_IdleDuration = 0.15f;

	// Token: 0x0400118F RID: 4495
	private float m_verticalBeam_AttackCD = 18f;

	// Token: 0x04001190 RID: 4496
	private bool m_verticalBeam_stopMovingWhileAttacking = true;

	// Token: 0x04001191 RID: 4497
	private Projectile_RL[] m_verticalBeamWarningProjectiles;

	// Token: 0x04001192 RID: 4498
	private Projectile_RL[] m_verticalBeamProjectiles;

	// Token: 0x04001193 RID: 4499
	private bool m_verticalBeamAudioPlaying;

	// Token: 0x04001194 RID: 4500
	private WaitForSeconds m_warningWaitTime;

	// Token: 0x04001195 RID: 4501
	private WaitForSeconds m_beamWaitTime;

	// Token: 0x04001196 RID: 4502
	private const string RAPID_FIRE_TELL_INTRO = "EyeballShot_Tell_Intro";

	// Token: 0x04001197 RID: 4503
	private const string RAPID_FIRE_TELL_HOLD = "EyeballShot_Tell_Hold";

	// Token: 0x04001198 RID: 4504
	private const string RAPID_FIRE_ATTACK_INTRO = "EyeballShot_Attack_Intro";

	// Token: 0x04001199 RID: 4505
	private const string RAPID_FIRE_ATTACK_HOLD = "EyeballShot_Attack_Hold";

	// Token: 0x0400119A RID: 4506
	private const string RAPID_FIRE_EXIT = "EyeballShot_Exit";

	// Token: 0x0400119B RID: 4507
	private const string RAPID_FIRE_PROJECTILE_NAME = "StudyBossRapidFireProjectile";

	// Token: 0x0400119C RID: 4508
	private const string RAPID_FIRE_VOID_PROJECTILE_NAME = "StudyBossRapidFireVoidProjectile";

	// Token: 0x0400119D RID: 4509
	private float m_rapidFire_TellIntro_AnimSpeed = 1f;

	// Token: 0x0400119E RID: 4510
	private float m_rapidFire_TellHold_AnimSpeed = 1f;

	// Token: 0x0400119F RID: 4511
	private float m_rapidFire_TellIntroAndHold_Delay = 1.5f;

	// Token: 0x040011A0 RID: 4512
	private float m_rapidFire_AttackIntro_AnimSpeed = 1f;

	// Token: 0x040011A1 RID: 4513
	private float m_rapidFire_AttackIntro_Delay;

	// Token: 0x040011A2 RID: 4514
	private float m_rapidFire_AttackHold_AnimSpeed = 1f;

	// Token: 0x040011A3 RID: 4515
	private float m_rapidFire_AttackHold_Delay = 3.25f;

	// Token: 0x040011A4 RID: 4516
	private float m_rapidFire_Exit_AnimSpeed = 1f;

	// Token: 0x040011A5 RID: 4517
	private float m_rapidFire_Exit_Delay = 0.45f;

	// Token: 0x040011A6 RID: 4518
	private float m_rapidFire_Exit_IdleDuration = 0.15f;

	// Token: 0x040011A7 RID: 4519
	private float m_rapidFire_AttackCD = 15f;

	// Token: 0x040011A8 RID: 4520
	private float m_rapidFire_MaxRandomAngleDeviation = 60f;

	// Token: 0x040011A9 RID: 4521
	private float m_rapidFire_SpeedMod = 1f;

	// Token: 0x040011AA RID: 4522
	private float m_rapidFire_VoidProjectileSpeedMod = 1f;

	// Token: 0x040011AB RID: 4523
	private float m_rapidFire_VoidShotDelay = 0.15f;

	// Token: 0x040011AC RID: 4524
	private float m_rapidFire_VoidShotAdvancedDelay = 0.25f;

	// Token: 0x040011AD RID: 4525
	private bool m_rapidFire_stopMovingWhileAttacking = true;

	// Token: 0x040011AE RID: 4526
	private const string CIRCLE_ATTACK_TELL_INTRO = "SummonCircularBullets_Tell_Intro";

	// Token: 0x040011AF RID: 4527
	private const string CIRCLE_ATTACK_TELL_HOLD = "SummonCircularBullets_Tell_Hold";

	// Token: 0x040011B0 RID: 4528
	private const string CIRCLE_ATTACK_ATTACK_INTRO = "SummonCircularBullets_Attack_Intro";

	// Token: 0x040011B1 RID: 4529
	private const string CIRCLE_ATTACK_ATTACK_HOLD = "SummonCircularBullets_Attack_Hold";

	// Token: 0x040011B2 RID: 4530
	private const string CIRCLE_ATTACK_EXIT = "SummonCircularBullets_Exit";

	// Token: 0x040011B3 RID: 4531
	private const string CIRCLE_ATTACK_PROJECTILE_NAME = "StudyBossCircleProjectile";

	// Token: 0x040011B4 RID: 4532
	private float m_circleAttack_TellIntro_AnimSpeed = 1f;

	// Token: 0x040011B5 RID: 4533
	private float m_circleAttack_TellHold_AnimSpeed = 1f;

	// Token: 0x040011B6 RID: 4534
	private float m_circleAttack_TellIntroAndHold_Delay = 1.5f;

	// Token: 0x040011B7 RID: 4535
	private float m_circleAttack_AttackIntro_AnimSpeed = 1f;

	// Token: 0x040011B8 RID: 4536
	private float m_circleAttack_AttackIntro_Delay;

	// Token: 0x040011B9 RID: 4537
	private float m_circleAttack_AttackHold_AnimSpeed = 1f;

	// Token: 0x040011BA RID: 4538
	private float m_circleAttack_AttackHold_Delay = 1.25f;

	// Token: 0x040011BB RID: 4539
	private float m_circleAttack_Exit_AnimSpeed = 1f;

	// Token: 0x040011BC RID: 4540
	private float m_circleAttack_Exit_Delay = 0.45f;

	// Token: 0x040011BD RID: 4541
	private float m_circleAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x040011BE RID: 4542
	private float m_circleAttack_AttackCD = 12f;

	// Token: 0x040011BF RID: 4543
	private bool m_circleAttack_stopMovingWhileAttacking;

	// Token: 0x040011C0 RID: 4544
	private const string BOMB_ATTACK_TELL_INTRO = "SummonBomb_Tell_Intro";

	// Token: 0x040011C1 RID: 4545
	private const string BOMB_ATTACK_TELL_HOLD = "SummonBomb_Tell_Hold";

	// Token: 0x040011C2 RID: 4546
	private const string BOMB_ATTACK_ATTACK_INTRO = "SummonBomb_Attack_Intro";

	// Token: 0x040011C3 RID: 4547
	private const string BOMB_ATTACK_ATTACK_HOLD = "SummonBomb_Attack_Hold";

	// Token: 0x040011C4 RID: 4548
	private const string BOMB_ATTACK_EXIT = "SummonBomb_Exit";

	// Token: 0x040011C5 RID: 4549
	private float m_bombAttack_TellIntro_AnimSpeed = 1f;

	// Token: 0x040011C6 RID: 4550
	private float m_bombAttack_TellHold_AnimSpeed = 1f;

	// Token: 0x040011C7 RID: 4551
	private float m_bombAttack_TellIntroAndHold_Delay = 2.25f;

	// Token: 0x040011C8 RID: 4552
	private float m_bombAttack_AttackIntro_AnimSpeed = 1f;

	// Token: 0x040011C9 RID: 4553
	private float m_bombAttack_AttackIntro_Delay;

	// Token: 0x040011CA RID: 4554
	private float m_bombAttack_AttackHold_AnimSpeed = 1f;

	// Token: 0x040011CB RID: 4555
	private float m_bombAttack_AttackHold_Delay;

	// Token: 0x040011CC RID: 4556
	private float m_bombAttack_Exit_AnimSpeed = 0.65f;

	// Token: 0x040011CD RID: 4557
	private float m_bombAttack_Exit_Delay = 0.45f;

	// Token: 0x040011CE RID: 4558
	private float m_bombAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x040011CF RID: 4559
	private float m_bombAttack_AttackCD = 12f;

	// Token: 0x040011D0 RID: 4560
	private float m_bombAttack_TimeToExplosion;

	// Token: 0x040011D1 RID: 4561
	private bool m_bombAttack_stopMovingWhileAttacking = true;

	// Token: 0x040011D2 RID: 4562
	private float m_bombAttack_Displace_X = 10f;

	// Token: 0x040011D3 RID: 4563
	private float m_bombAttack_Displace_Y = 3.75f;

	// Token: 0x040011D4 RID: 4564
	private Projectile_RL m_bombWarningProjectile;

	// Token: 0x040011D5 RID: 4565
	private Projectile_RL m_bombWarningProjectile2;

	// Token: 0x040011D6 RID: 4566
	private Projectile_RL m_bombWarningProjectile3;

	// Token: 0x040011D7 RID: 4567
	private Projectile_RL m_bombWarningProjectile4;

	// Token: 0x040011D8 RID: 4568
	private Projectile_RL m_bombWarningProjectile5;

	// Token: 0x040011D9 RID: 4569
	private Relay m_bombAttackWarningAppearedRelay = new Relay();

	// Token: 0x040011DA RID: 4570
	private Relay m_bombAttackExplodedRelay = new Relay();

	// Token: 0x040011DB RID: 4571
	private const string SHOUT_TELL_INTRO = "Shout_Tell_Intro";

	// Token: 0x040011DC RID: 4572
	private const string SHOUT_TELL_HOLD = "Shout_Tell_Hold";

	// Token: 0x040011DD RID: 4573
	private const string SHOUT_ATTACK_INTRO = "Shout_Attack_Intro";

	// Token: 0x040011DE RID: 4574
	private const string SHOUT_ATTACK_HOLD = "Shout_Attack_Hold";

	// Token: 0x040011DF RID: 4575
	private const string SHOUT_EXIT = "Shout_Exit";

	// Token: 0x040011E0 RID: 4576
	private const string SHOUT_ATTACK_EXPLOSION_PROJECTILE_NAME = "StudyBossShoutExplosionProjectile";

	// Token: 0x040011E1 RID: 4577
	private const string SHOUT_ATTACK_WARNING_PROJECTILE_NAME = "StudyBossShoutWarningProjectile";

	// Token: 0x040011E2 RID: 4578
	private float m_shout_TellIntro_AnimSpeed = 1f;

	// Token: 0x040011E3 RID: 4579
	private float m_shout_TellHold_AnimSpeed = 1f;

	// Token: 0x040011E4 RID: 4580
	private float m_shout_TellIntroAndHold_Delay = 1.5f;

	// Token: 0x040011E5 RID: 4581
	private float m_shout_AttackIntro_AnimSpeed = 1f;

	// Token: 0x040011E6 RID: 4582
	private float m_shout_AttackIntro_Delay;

	// Token: 0x040011E7 RID: 4583
	private float m_shout_AttackHold_AnimSpeed = 1f;

	// Token: 0x040011E8 RID: 4584
	private float m_shout_AttackHold_Delay;

	// Token: 0x040011E9 RID: 4585
	private float m_shout_Exit_AnimSpeed = 0.65f;

	// Token: 0x040011EA RID: 4586
	private float m_shout_Exit_Delay;

	// Token: 0x040011EB RID: 4587
	private float m_shout_Exit_IdleDuration = 0.35f;

	// Token: 0x040011EC RID: 4588
	private float m_shout_AttackCD = 12f;

	// Token: 0x040011ED RID: 4589
	private float m_shoutAttackTimeToExplosion;

	// Token: 0x040011EE RID: 4590
	private bool m_shout_stopMovingWhileAttacking = true;

	// Token: 0x040011EF RID: 4591
	private float m_shoutAttackDelaySecondExplosion = 0.65f;

	// Token: 0x040011F0 RID: 4592
	private int m_shoutAttackDelaySecondExplosionAngleAdd = 10;

	// Token: 0x040011F1 RID: 4593
	private Projectile_RL m_shoutWarningProjectile;

	// Token: 0x040011F2 RID: 4594
	private Relay m_shoutAttackWarningAppearedRelay = new Relay();

	// Token: 0x040011F3 RID: 4595
	private Relay m_shoutAttackExplodedRelay = new Relay();

	// Token: 0x040011F4 RID: 4596
	private const string DASH_ATTACK_TELL_INTRO = "Dash_Tell_Intro";

	// Token: 0x040011F5 RID: 4597
	private const string DASH_ATTACK_TELL_HOLD = "Dash_Tell_Hold";

	// Token: 0x040011F6 RID: 4598
	private const string DASH_ATTACK_ATTACK_INTRO = "Dash_Attack_Intro";

	// Token: 0x040011F7 RID: 4599
	private const string DASH_ATTACK_ATTACK_HOLD = "Dash_Attack_Hold";

	// Token: 0x040011F8 RID: 4600
	private const string DASH_ATTACK_EXIT = "Dash_Exit";

	// Token: 0x040011F9 RID: 4601
	private const string CURSE_ATTACK_RED_PROJECTILE_NAME = "StudyBossFlowerProjectile";

	// Token: 0x040011FA RID: 4602
	private float m_dashAttack_TellIntro_AnimSpeed = 1f;

	// Token: 0x040011FB RID: 4603
	private float m_dashAttack_TellHold_AnimSpeed = 1f;

	// Token: 0x040011FC RID: 4604
	private float m_dashAttack_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x040011FD RID: 4605
	private float m_dashAttack_AttackIntro_AnimSpeed = 1f;

	// Token: 0x040011FE RID: 4606
	private float m_dashAttack_AttackIntro_Delay;

	// Token: 0x040011FF RID: 4607
	private float m_dashAttack_AttackHold_AnimSpeed = 1f;

	// Token: 0x04001200 RID: 4608
	private float m_dashAttack_AttackHold_Delay = 1f;

	// Token: 0x04001201 RID: 4609
	private float m_dashAttack_Exit_AnimSpeed = 1f;

	// Token: 0x04001202 RID: 4610
	private float m_dashAttack_Exit_Delay = 0.45f;

	// Token: 0x04001203 RID: 4611
	private float m_dashAttack_Exit_IdleDuration = 0.15f;

	// Token: 0x04001204 RID: 4612
	private float m_dashAttack_AttackCD = 12f;

	// Token: 0x04001205 RID: 4613
	private float m_dashAttackSpeed = 16.5f;

	// Token: 0x04001206 RID: 4614
	private float m_dashAttackFireballSpeedMod = 1f;

	// Token: 0x04001207 RID: 4615
	private const string SHIELD_ATTACK_PROJECTILE_NAME = "StudyBossShieldProjectile";

	// Token: 0x04001208 RID: 4616
	private float m_shieldAttackRadius = 13.5f;

	// Token: 0x04001209 RID: 4617
	private float m_shieldAttackOrbitSpeed = 25f;

	// Token: 0x0400120A RID: 4618
	private float m_shieldAttackTimeBetweenProjectileRespawn = 5f;

	// Token: 0x0400120B RID: 4619
	private bool m_isShieldAttackRunning;

	// Token: 0x0400120C RID: 4620
	private const string MODE_SHIFT_INTRO = "ModeShift_Intro";

	// Token: 0x0400120D RID: 4621
	private const string MODE_SHIFT_SCREAM_INTRO = "ModeShift_Scream_Intro";

	// Token: 0x0400120E RID: 4622
	private const string MODE_SHIFT_SCREAM_HOLD = "ModeShift_Scream_Hold";

	// Token: 0x0400120F RID: 4623
	private const string MODE_SHIFT_EXIT = "ModeShift_Scream_Exit";

	// Token: 0x04001210 RID: 4624
	private float m_modeShift_Intro_AnimSpeed = 1f;

	// Token: 0x04001211 RID: 4625
	private float m_modeShift_Intro_Delay = 1.5f;

	// Token: 0x04001212 RID: 4626
	private float m_modeShift_Scream_Intro_AnimSpeed = 1f;

	// Token: 0x04001213 RID: 4627
	private float m_modeShift_Scream_Intro_Delay;

	// Token: 0x04001214 RID: 4628
	private float m_modeShift_Scream_Hold_AnimSpeed = 1f;

	// Token: 0x04001215 RID: 4629
	private float m_modeShift_Scream_Hold_Delay = 1.25f;

	// Token: 0x04001216 RID: 4630
	private float m_modeShift_Exit_AnimSpeed = 1f;

	// Token: 0x04001217 RID: 4631
	private float m_modeShift_Exit_Delay = 0.5f;

	// Token: 0x04001218 RID: 4632
	private float m_modeShift_IdleDuration = 1f;

	// Token: 0x04001219 RID: 4633
	private float m_modeShift_AttackCD = 99999f;

	// Token: 0x0400121A RID: 4634
	private float m_modeShift_HealthMod = 0.5f;

	// Token: 0x0400121B RID: 4635
	private float m_modeShift_DamageMod = 0.1f;

	// Token: 0x0400121C RID: 4636
	protected const string SPAWN_IDLE = "Intro_Idle";

	// Token: 0x0400121D RID: 4637
	protected const string SPAWN_INTRO = "Intro";

	// Token: 0x0400121E RID: 4638
	protected float m_spawn_Idle_AnimSpeed = 1f;

	// Token: 0x0400121F RID: 4639
	protected float m_spawn_Idle_Delay = 2f;

	// Token: 0x04001220 RID: 4640
	protected float m_spawn_Intro_AnimSpeed = 1f;

	// Token: 0x04001221 RID: 4641
	protected float m_spawn_Intro_Delay;

	// Token: 0x04001222 RID: 4642
	protected const string DEATH_INTRO = "Death_Intro";

	// Token: 0x04001223 RID: 4643
	protected const string DEATH_HOLD = "Death_Loop";

	// Token: 0x04001224 RID: 4644
	protected float m_death_Intro_AnimSpeed = 1f;

	// Token: 0x04001225 RID: 4645
	protected float m_death_Intro_Delay;

	// Token: 0x04001226 RID: 4646
	protected float m_death_Hold_AnimSpeed = 1f;

	// Token: 0x04001227 RID: 4647
	protected float m_death_Hold_Delay = 4.5f;

	// Token: 0x04001228 RID: 4648
	private const string PHASE2_HIT_AUDIO_PATH = "event:/SFX/Enemies/sfx_studyBoss_phase2_hit_start";

	// Token: 0x04001229 RID: 4649
	private const string FLYING_AUDIO_PATH = "event:/SFX/Enemies/sfx_studyBoss_fly_loop";

	// Token: 0x0400122A RID: 4650
	private const string FLYING_SPEED_PARAMETER = "dancingBoss_flySpeed";

	// Token: 0x0400122B RID: 4651
	private const string IDLE_AUDIO_PATH = "event:/SFX/Enemies/sfx_studyBoss_idle_loop";

	// Token: 0x0400122C RID: 4652
	private const string VERTICAL_BEAM_LOOP_AUDIO_PATH = "event:/SFX/Enemies/sfx_studyBoss_attack_skyBeam_spawn_start_loop";

	// Token: 0x0400122D RID: 4653
	private const string VERTICAL_BEAM_DESPAWN_AUDIO_PATH = "event:/SFX/Enemies/sfx_studyBoss_attack_skyBeam_despawn";

	// Token: 0x0400122E RID: 4654
	private const string VOID_CIRCLE_CLOSING_AUDIO_PATH = "event:/SFX/Enemies/sfx_studyBoss_attack_voidCircle_closeIn";

	// Token: 0x0400122F RID: 4655
	private const string EYEBALL_SHOT_LOOP_AUDIO_PATH = "event:/SFX/Enemies/sfx_studyBoss_attack_eye_shoot_start_loop";

	// Token: 0x04001230 RID: 4656
	private const string SHIELD_ATTACK_LOOP_AUDIO_PATH = "event:/SFX/Enemies/sfx_studyBoss_phase2_voidProjectiles_loop";

	// Token: 0x04001231 RID: 4657
	private const float MAX_FLYING_SPEED = 5f;

	// Token: 0x04001232 RID: 4658
	private bool m_wasFlying;

	// Token: 0x04001233 RID: 4659
	private EventInstance m_flyingAudioEventInstance;

	// Token: 0x04001234 RID: 4660
	private EventInstance m_idleAudioEventInstance;

	// Token: 0x04001235 RID: 4661
	private EventInstance m_verticalBeamLoopAudioEventInstance;

	// Token: 0x04001236 RID: 4662
	private EventInstance m_verticalBeamDespawnAudioEventInstance;

	// Token: 0x04001237 RID: 4663
	private EventInstance m_voidCircleClosingAudioEventInstance;

	// Token: 0x04001238 RID: 4664
	private EventInstance m_eyeballShotLoopAudioEventInstance;

	// Token: 0x04001239 RID: 4665
	private EventInstance m_shieldAttackLoopAudioEventInstance;

	// Token: 0x0400123A RID: 4666
	private bool m_rapidFireAudioPlaying;
}

using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200014F RID: 335
public class FlyingAxe_Basic_AIScript : BaseAIScript, IBodyOnEnterHitResponse, IHitResponse, IBodyOnStayHitResponse
{
	// Token: 0x060008C4 RID: 2244 RVA: 0x00006236 File Offset: 0x00004436
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlyingAxeBounceBoltProjectile",
			"FlyingAxeFlameThrowerProjectile",
			"WallTurretFlameThrowerWarningProjectile_Template",
			"FlyingAxeFireballProjectile"
		};
	}

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x060008C5 RID: 2245 RVA: 0x00006264 File Offset: 0x00004464
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.3f, 0.7f);
		}
	}

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x060008C6 RID: 2246 RVA: 0x00006275 File Offset: 0x00004475
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.8f, 1.1f);
		}
	}

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x060008C7 RID: 2247 RVA: 0x00006275 File Offset: 0x00004475
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.8f, 1.1f);
		}
	}

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x060008C8 RID: 2248 RVA: 0x00005FB1 File Offset: 0x000041B1
	protected virtual float m_sideSpin_Attack_Duration
	{
		get
		{
			return 5.5f;
		}
	}

	// Token: 0x170003DF RID: 991
	// (get) Token: 0x060008C9 RID: 2249 RVA: 0x00006286 File Offset: 0x00004486
	protected virtual float m_sideSpin_Attack_TurnSpeed
	{
		get
		{
			return 85f;
		}
	}

	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x060008CA RID: 2250 RVA: 0x0000606E File Offset: 0x0000426E
	protected virtual float m_sideSpin_Attack_MovementSpeed
	{
		get
		{
			return 9f;
		}
	}

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x060008CB RID: 2251 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_sideSpin_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x060008CC RID: 2252 RVA: 0x0000628D File Offset: 0x0000448D
	protected virtual Vector2 m_sideSpin_InternalKnockBackMod
	{
		get
		{
			return new Vector2(1.75f, 1.75f);
		}
	}

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x060008CD RID: 2253 RVA: 0x00004536 File Offset: 0x00002736
	protected virtual float m_sideSpin_FlightRecoveryMod
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x0000629E File Offset: 0x0000449E
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Side_Spin_Attack()
	{
		base.EnemyController.FollowTarget = true;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.FollowOffset = Vector3.zero;
		yield return this.Default_TellIntroAndLoop("SideSpin_Tell_Intro", this.m_sideSpin_TellIntro_AnimationSpeed, "SideSpin_Tell_Hold", this.m_sideSpin_TellHold_AnimationSpeed, this.m_sideSpin_TellIntroAndHold_Duration);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("SideSpin_Attack_Intro", this.m_sideSpin_AttackIntro_AnimationSpeed, this.m_sideSpin_AttackIntro_Delay, true);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_sideSpin_Attack_MovementSpeed;
		base.EnemyController.BaseTurnSpeed = this.m_sideSpin_Attack_TurnSpeed;
		base.EnemyController.InternalKnockbackMod = this.m_sideSpin_InternalKnockBackMod;
		base.EnemyController.FlightKnockbackDecelerationMod = this.m_sideSpin_FlightRecoveryMod;
		yield return this.Default_Animation("SideSpin_Attack_Hold", 1f, 0f, true);
		if (this.m_sideSpin_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_sideSpin_Attack_Duration, false);
		}
		base.EnemyController.ResetBaseValues();
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("SideSpin_Exit", this.m_sideSpin_ExitIntro_AnimationSpeed, 0.15f, true);
		base.EnemyController.ResetTurnTrigger();
		yield return this.Default_Attack_Cooldown(0.15f, this.m_sideSpin_Exit_AttackCD);
		yield break;
	}

	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x060008CF RID: 2255 RVA: 0x00005319 File Offset: 0x00003519
	protected virtual float m_vertSpin_Attack_ChaseDuration
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x060008D0 RID: 2256 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_vertSpin_Attack_TurnSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x060008D1 RID: 2257 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_vertSpin_Attack_MovementSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x060008D2 RID: 2258 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_vertSpin_Attack_FireBulletsWhileSpinning
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x060008D3 RID: 2259 RVA: 0x00004762 File Offset: 0x00002962
	protected virtual int m_vertSpin_Attack_BulletShotPerLoop
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x060008D4 RID: 2260 RVA: 0x000046FA File Offset: 0x000028FA
	protected virtual int m_vertSpin_Attack_TotalLoops
	{
		get
		{
			return 10;
		}
	}

	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x060008D5 RID: 2261 RVA: 0x00004A26 File Offset: 0x00002C26
	protected virtual Vector2 m_vertSpin_Attack_FireballSpeedMod
	{
		get
		{
			return new Vector2(0.5f, 1f);
		}
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x000062AD File Offset: 0x000044AD
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Vertical_Spin_Attack()
	{
		base.EnemyController.AlwaysFacing = false;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.FollowOffset = Vector3.zero;
		this.m_flameTellProjectile1 = this.FireProjectile("WallTurretFlameThrowerWarningProjectile_Template", new Vector2(1f, 0f), false, 0f, 1f, true, true, true);
		this.m_flameTellProjectile2 = this.FireProjectile("WallTurretFlameThrowerWarningProjectile_Template", new Vector2(0f, 1f), false, 90f, 1f, true, true, true);
		this.m_flameTellProjectile3 = this.FireProjectile("WallTurretFlameThrowerWarningProjectile_Template", new Vector2(-1f, 0f), false, 180f, 1f, true, true, true);
		this.m_flameTellProjectile4 = this.FireProjectile("WallTurretFlameThrowerWarningProjectile_Template", new Vector2(0f, -1f), false, 270f, 1f, true, true, true);
		yield return this.Default_TellIntroAndLoop("VerticalSpin_Tell_Intro", this.m_vertSpin_TellIntro_AnimationSpeed, "VerticalSpin_Tell_Hold", this.m_vertSpin_TellHold_AnimationSpeed, this.m_vertSpin_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		base.StopProjectile(ref this.m_flameTellProjectile1);
		base.StopProjectile(ref this.m_flameTellProjectile2);
		base.StopProjectile(ref this.m_flameTellProjectile3);
		base.StopProjectile(ref this.m_flameTellProjectile4);
		yield return this.Default_Animation("VerticalSpin_Attack_Intro", this.m_vertSpin_AttackIntro_AnimationSpeed, this.m_vertSpin_AttackIntro_Delay, true);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_vertSpin_Attack_MovementSpeed;
		base.EnemyController.BaseTurnSpeed = this.m_vertSpin_Attack_TurnSpeed;
		yield return this.Default_Animation("VerticalSpin_Attack_Hold", 1f, 0f, false);
		this.m_flameThrowerProjectile1 = this.FireProjectile("FlyingAxeFlameThrowerProjectile", new Vector2(1f, 0f), false, 0f, 1f, true, true, true);
		this.m_flameThrowerProjectile2 = this.FireProjectile("FlyingAxeFlameThrowerProjectile", new Vector2(0f, 1f), false, 90f, 1f, true, true, true);
		this.m_flameThrowerProjectile3 = this.FireProjectile("FlyingAxeFlameThrowerProjectile", new Vector2(-1f, 0f), false, 180f, 1f, true, true, true);
		this.m_flameThrowerProjectile4 = this.FireProjectile("FlyingAxeFlameThrowerProjectile", new Vector2(0f, -1f), false, 270f, 1f, true, true, true);
		if (this.m_vertSpin_Attack_ChaseDuration > 0f)
		{
			if (this.m_vertSpin_Attack_FireBulletsWhileSpinning)
			{
				int angleDivider = 360 / this.m_vertSpin_Attack_BulletShotPerLoop;
				int num2;
				for (int i = 0; i < this.m_vertSpin_Attack_TotalLoops; i = num2 + 1)
				{
					for (int j = 0; j < this.m_vertSpin_Attack_BulletShotPerLoop; j++)
					{
						int num = UnityEngine.Random.Range(angleDivider * j, angleDivider * (j + 1));
						float speedMod = UnityEngine.Random.Range(this.m_vertSpin_Attack_FireballSpeedMod.x, this.m_vertSpin_Attack_FireballSpeedMod.y);
						this.FireProjectile("FlyingAxeFireballProjectile", new Vector2(0f, 0f), false, (float)num, speedMod, true, true, true);
					}
					yield return base.Wait(this.m_vertSpin_Attack_ChaseDuration / (float)this.m_vertSpin_Attack_TotalLoops, false);
					num2 = i;
				}
			}
			else
			{
				yield return base.Wait(this.m_vertSpin_Attack_ChaseDuration, false);
			}
		}
		base.EnemyController.ResetBaseValues();
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.StopProjectile(ref this.m_flameThrowerProjectile1);
		base.StopProjectile(ref this.m_flameThrowerProjectile2);
		base.StopProjectile(ref this.m_flameThrowerProjectile3);
		base.StopProjectile(ref this.m_flameThrowerProjectile4);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("VerticalSpin_Exit", this.m_vertSpin_ExitIntro_AnimationSpeed, 0.15f, true);
		base.EnemyController.ResetTurnTrigger();
		yield return this.Default_Attack_Cooldown(0.25f, this.m_vertSpin_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x00002FCA File Offset: 0x000011CA
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x000062BC File Offset: 0x000044BC
	public void BodyOnStayHitResponse(IHitboxController otherHBController)
	{
		this.BodyOnEnterHitResponse(otherHBController);
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x000062C5 File Offset: 0x000044C5
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.DisableDistanceThresholdCheck = true;
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x0006420C File Offset: 0x0006240C
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.StopProjectile(ref this.m_flameTellProjectile1);
		base.StopProjectile(ref this.m_flameTellProjectile2);
		base.StopProjectile(ref this.m_flameTellProjectile3);
		base.StopProjectile(ref this.m_flameTellProjectile4);
		base.StopProjectile(ref this.m_flameThrowerProjectile1);
		base.StopProjectile(ref this.m_flameThrowerProjectile2);
		base.StopProjectile(ref this.m_flameThrowerProjectile3);
		base.StopProjectile(ref this.m_flameThrowerProjectile4);
	}

	// Token: 0x04000C89 RID: 3209
	protected float m_sideSpin_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000C8A RID: 3210
	protected float m_sideSpin_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000C8B RID: 3211
	protected float m_sideSpin_TellIntroAndHold_Duration = 1f;

	// Token: 0x04000C8C RID: 3212
	protected float m_sideSpin_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000C8D RID: 3213
	protected float m_sideSpin_AttackIntro_Delay;

	// Token: 0x04000C8E RID: 3214
	protected const float m_sideSpin_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000C8F RID: 3215
	protected const float m_sideSpin_AttackHold_Delay = 0f;

	// Token: 0x04000C90 RID: 3216
	protected float m_sideSpin_ExitIntro_AnimationSpeed = 1f;

	// Token: 0x04000C91 RID: 3217
	protected const float m_sideSpin_ExitIntro_EndDelay = 0.15f;

	// Token: 0x04000C92 RID: 3218
	protected const float m_sideSpin_Exit_ForceIdle = 0.15f;

	// Token: 0x04000C93 RID: 3219
	protected float m_vertSpin_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000C94 RID: 3220
	protected float m_vertSpin_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000C95 RID: 3221
	protected float m_vertSpin_TellIntroAndHold_Delay = 1.2f;

	// Token: 0x04000C96 RID: 3222
	protected float m_vertSpin_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000C97 RID: 3223
	protected float m_vertSpin_AttackIntro_Delay;

	// Token: 0x04000C98 RID: 3224
	protected const float m_vertSpin_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000C99 RID: 3225
	protected const float m_vertSpin_AttackHold_Delay = 0f;

	// Token: 0x04000C9A RID: 3226
	protected float m_vertSpin_ExitIntro_AnimationSpeed = 1f;

	// Token: 0x04000C9B RID: 3227
	protected const float m_vertSpin_ExitIntro_EndDelay = 0.15f;

	// Token: 0x04000C9C RID: 3228
	protected const float m_vertSpin_Exit_ForceIdle = 0.25f;

	// Token: 0x04000C9D RID: 3229
	protected float m_vertSpin_Exit_AttackCD = 5f;

	// Token: 0x04000C9E RID: 3230
	private Projectile_RL m_flameThrowerProjectile1;

	// Token: 0x04000C9F RID: 3231
	private Projectile_RL m_flameThrowerProjectile2;

	// Token: 0x04000CA0 RID: 3232
	private Projectile_RL m_flameThrowerProjectile3;

	// Token: 0x04000CA1 RID: 3233
	private Projectile_RL m_flameThrowerProjectile4;

	// Token: 0x04000CA2 RID: 3234
	private const string FLAME_TELL_PROJECTILE = "WallTurretFlameThrowerWarningProjectile_Template";

	// Token: 0x04000CA3 RID: 3235
	private const string FIREBALL_PROJECTILE = "FlyingAxeFireballProjectile";

	// Token: 0x04000CA4 RID: 3236
	protected Projectile_RL m_flameTellProjectile1;

	// Token: 0x04000CA5 RID: 3237
	protected Projectile_RL m_flameTellProjectile2;

	// Token: 0x04000CA6 RID: 3238
	protected Projectile_RL m_flameTellProjectile3;

	// Token: 0x04000CA7 RID: 3239
	protected Projectile_RL m_flameTellProjectile4;
}

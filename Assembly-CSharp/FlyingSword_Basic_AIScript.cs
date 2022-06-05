using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000179 RID: 377
public class FlyingSword_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000A4F RID: 2639 RVA: 0x00006997 File Offset: 0x00004B97
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlyingSwordProjectile",
			"FlyingSwordMinibossProjectile",
			"FlyingSwordDaggerProjectile"
		};
	}

	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x06000A50 RID: 2640 RVA: 0x000069BD File Offset: 0x00004BBD
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.45f, 0.75f);
		}
	}

	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x06000A51 RID: 2641 RVA: 0x00006275 File Offset: 0x00004475
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.8f, 1.1f);
		}
	}

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x06000A52 RID: 2642 RVA: 0x00006275 File Offset: 0x00004475
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.8f, 1.1f);
		}
	}

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x06000A53 RID: 2643 RVA: 0x000069CE File Offset: 0x00004BCE
	protected virtual float m_thrust_Attack_InitialTargetTurnRate
	{
		get
		{
			return 900f;
		}
	}

	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x06000A54 RID: 2644 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_thrust_Attack_TurnRate
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x06000A55 RID: 2645 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float m_thrust_Exit_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x06000A56 RID: 2646 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_thrustFast_DashAttackAmount
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x06000A57 RID: 2647 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_thrustFast_AttackTell_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x06000A58 RID: 2648 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_thrustFast_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x06000A59 RID: 2649 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_thrustFastAttackExitEndDuration_Fast
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06000A5A RID: 2650 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_thrust_SpawnProjectilesAtEnd
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x06000A5B RID: 2651 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_thrust_SpawnMinibossProjectilesAtEnd
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x000069D5 File Offset: 0x00004BD5
	public IEnumerator Custom_Tell_Animation(float tellIntroDuration, float tellHoldDuration, float reverseDuration)
	{
		base.EnemyController.FollowOffset = Vector3.zero;
		base.EnemyController.BaseTurnSpeed = this.m_thrust_Attack_InitialTargetTurnRate;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_Animation("Thrust_Tell_Intro", this.m_thrust_TellIntro_AnimationSpeed, 0f, false);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return base.Wait(0.25f, false);
		base.EnemyController.PivotFollowsOrientation = true;
		yield return base.WaitUntilAnimComplete(this.DefaultAnimationLayer);
		if (tellIntroDuration > 0f)
		{
			yield return base.Wait(tellIntroDuration, false);
		}
		yield return this.Default_Animation("Thrust_Tell_Hold", this.m_thrust_TellHold_AnimationSpeed, tellHoldDuration, false);
		yield return this.Default_Animation("Thrust_Attack_Intro", this.m_thrust_AttackIntro_AnimationSpeed, this.m_thrust_AttackIntro_Delay, true);
		yield return this.Default_Animation("Thrust_Attack_Hold_BackUp", this.m_thrust_AttackHold_AnimationSpeed, 0f, false);
		base.EnemyController.BaseSpeed *= -this.m_thrust_Attack_ReverseSpeed;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Away;
		if (reverseDuration > 0f)
		{
			yield return base.Wait(reverseDuration, false);
		}
		base.EnemyController.BaseSpeed = base.EnemyController.EnemyData.Speed;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield break;
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x000069F9 File Offset: 0x00004BF9
	public IEnumerator Custom_Thrust_Attack()
	{
		base.EnemyController.BaseTurnSpeed = this.m_thrust_Attack_TurnRate;
		base.EnemyController.FollowTarget = true;
		base.EnemyController.LockFlip = true;
		base.EnemyController.PivotFollowsOrientation = true;
		this.SetAnimationSpeedMultiplier(this.m_thrust_AttackHold_AnimationSpeed);
		yield return this.Default_Animation("Thrust_Attack_Hold", this.m_thrust_AttackHold_AnimationSpeed, 0f, false);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_thrust_Attack_ForwardSpeed;
		if (this.m_thrust_Attack_ForwardDuration > 0f)
		{
			yield return base.Wait(this.m_thrust_Attack_ForwardDuration, false);
		}
		base.EnemyController.BaseSpeed = base.EnemyController.EnemyData.Speed;
		base.EnemyController.BaseTurnSpeed = 0f;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		if (this.m_thrust_SpawnProjectilesAtEnd)
		{
			this.FireProjectile("FlyingSwordProjectile", new Vector2(0f, 0f), true, 45f, 1f, true, true, true);
			this.FireProjectile("FlyingSwordProjectile", new Vector2(0f, 0f), true, 135f, 1f, true, true, true);
			this.FireProjectile("FlyingSwordProjectile", new Vector2(0f, 0f), true, 225f, 1f, true, true, true);
			this.FireProjectile("FlyingSwordProjectile", new Vector2(0f, 0f), true, 315f, 1f, true, true, true);
		}
		else if (this.m_thrust_SpawnMinibossProjectilesAtEnd)
		{
			this.FireProjectile("FlyingSwordMinibossProjectile", new Vector2(0f, 0f), true, 45f, 1f, true, true, true);
			this.FireProjectile("FlyingSwordMinibossProjectile", new Vector2(0f, 0f), true, 135f, 1f, true, true, true);
			this.FireProjectile("FlyingSwordMinibossProjectile", new Vector2(0f, 0f), true, 225f, 1f, true, true, true);
			this.FireProjectile("FlyingSwordMinibossProjectile", new Vector2(0f, 0f), true, 315f, 1f, true, true, true);
		}
		if (this.m_thrust_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.m_thrust_AttackHold_Delay, false);
		}
		yield break;
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x00006A08 File Offset: 0x00004C08
	public IEnumerator Custom_Fast_Exit_Animation()
	{
		yield return this.Default_Animation("Thrust_Exit_Intro", this.m_thrustFast_ExitIntro_AnimationSpeed, this.m_thrustFastAttackExitEndDuration_Fast, true);
		base.EnemyController.FollowTarget = true;
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetPivotRotation();
		base.EnemyController.ResetBaseValues();
		yield break;
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x00006A17 File Offset: 0x00004C17
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Thrust_Attack()
	{
		base.EnemyController.DisableOffscreenWarnings = false;
		ProjectileManager.AttachOffscreenIcon(base.EnemyController, true);
		yield return this.Custom_Tell_Animation(this.m_thrust_TellIntro_Delay, this.m_thrust_TellHold_Delay, this.m_thrust_TellHold_ReverseDuration);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Custom_Thrust_Attack();
		int i = 0;
		while ((float)i < this.m_thrustFast_DashAttackAmount)
		{
			yield return this.Custom_Fast_Exit_Animation();
			yield return this.Custom_Tell_Animation(this.m_thrustFast_AttackTell_Delay, this.m_thrustFast_AttackHold_Delay, this.m_thrust_TellHold_ReverseDuration);
			yield return this.Custom_Thrust_Attack();
			int num = i;
			i = num + 1;
		}
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("Thrust_Exit_Intro", this.m_thrust_ExitIntro_AnimationSpeed, this.m_thrust_ExitIntro_Delay, true);
		base.EnemyController.PivotFollowsOrientation = false;
		base.EnemyController.ResetPivotRotation();
		yield return this.Default_Animation("Thrust_Exit", this.m_thrust_Exit_AnimationSpeed, this.m_thrust_Exit_Delay, true);
		base.EnemyController.DisableOffscreenWarnings = true;
		base.EnemyController.FollowTarget = true;
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_thrust_Exit_ForceIdle, this.m_thrust_Exit_AttackCD);
		yield break;
	}

	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x06000A60 RID: 2656 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_vertSpin_Attack_TurnSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x06000A61 RID: 2657 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_vertSpin_Attack_MovementSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004E4 RID: 1252
	// (get) Token: 0x06000A62 RID: 2658 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected virtual int m_vertSpin_Attack_TotalLoops
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170004E5 RID: 1253
	// (get) Token: 0x06000A63 RID: 2659 RVA: 0x00006A26 File Offset: 0x00004C26
	protected virtual float m_vertSpin_Attack_DelayPerLoop
	{
		get
		{
			return 0.225f;
		}
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x00006A2D File Offset: 0x00004C2D
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
		yield return this.Default_TellIntroAndLoop("VerticalSpin_Tell_Intro", this.m_vertSpin_TellIntro_AnimationSpeed, "VerticalSpin_Tell_Hold", this.m_vertSpin_TellHold_AnimationSpeed, this.m_vertSpin_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("VerticalSpin_Attack_Intro", this.m_vertSpin_AttackIntro_AnimationSpeed, this.m_vertSpin_AttackIntro_Delay, true);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_vertSpin_Attack_MovementSpeed;
		base.EnemyController.BaseTurnSpeed = this.m_vertSpin_Attack_TurnSpeed;
		yield return this.Default_Animation("VerticalSpin_Attack_Hold", 1f, 0.5f, true);
		int num;
		for (int i = 0; i < this.m_vertSpin_Attack_TotalLoops; i = num + 1)
		{
			float angle = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
			this.FireProjectile("FlyingSwordDaggerProjectile", new Vector2(0f, 0f), false, angle, 1f, true, true, true);
			yield return base.Wait(this.m_vertSpin_Attack_DelayPerLoop, false);
			num = i;
		}
		base.EnemyController.ResetBaseValues();
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("VerticalSpin_Exit", this.m_vertSpin_ExitIntro_AnimationSpeed, 0.15f, true);
		base.EnemyController.ResetTurnTrigger();
		yield return this.Default_Attack_Cooldown(0.25f, this.m_vertSpin_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x00006A3C File Offset: 0x00004C3C
	public override IEnumerator WalkAway()
	{
		base.EnemyController.ResetPivotRotation();
		return base.WalkAway();
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x00006A4F File Offset: 0x00004C4F
	public override IEnumerator WalkTowards()
	{
		base.EnemyController.ResetPivotRotation();
		return base.WalkTowards();
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x00006A62 File Offset: 0x00004C62
	public override IEnumerator Idle()
	{
		base.EnemyController.ResetPivotRotation();
		base.EnemyController.FollowTarget = true;
		base.EnemyController.LockFlip = false;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		this.SetAnimationSpeedMultiplier(this.IdleAnimSpeedMod);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		float num = UnityEngine.Random.Range(this.IdleDuration.x, this.IdleDuration.y);
		if (num > 0f)
		{
			yield return base.Wait(num, false);
		}
		this.SetAnimationSpeedMultiplier(this.IdleAnimSpeedMod);
		yield return null;
		yield break;
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x00006A71 File Offset: 0x00004C71
	public override void OnLBCompleteOrCancelled()
	{
		base.EnemyController.PivotFollowsOrientation = false;
		base.EnemyController.LockFlip = false;
		base.EnemyController.DisableOffscreenWarnings = true;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x04000D78 RID: 3448
	private const string GENERIC_PROJECTILE_NAME = "FlyingSwordProjectile";

	// Token: 0x04000D79 RID: 3449
	private const string MINIBOSS_PROJECTILE_NAME = "FlyingSwordMinibossProjectile";

	// Token: 0x04000D7A RID: 3450
	private const string DAGGER_PROJECTILE_NAME = "FlyingSwordDaggerProjectile";

	// Token: 0x04000D7B RID: 3451
	protected float m_thrust_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000D7C RID: 3452
	protected float m_thrust_TellIntro_Delay;

	// Token: 0x04000D7D RID: 3453
	protected float m_thrust_TellHold_AnimationSpeed = 1.5f;

	// Token: 0x04000D7E RID: 3454
	protected float m_thrust_TellHold_Delay = 0.55f;

	// Token: 0x04000D7F RID: 3455
	protected float m_thrust_TellHold_ReverseDuration = 0.325f;

	// Token: 0x04000D80 RID: 3456
	protected float m_thrust_AttackIntro_AnimationSpeed = 2.75f;

	// Token: 0x04000D81 RID: 3457
	protected float m_thrust_AttackIntro_Delay;

	// Token: 0x04000D82 RID: 3458
	protected float m_thrust_AttackHold_AnimationSpeed = 2.75f;

	// Token: 0x04000D83 RID: 3459
	protected float m_thrust_AttackHold_Delay;

	// Token: 0x04000D84 RID: 3460
	protected float m_thrust_ExitIntro_AnimationSpeed = 1.2f;

	// Token: 0x04000D85 RID: 3461
	protected float m_thrust_ExitIntro_Delay = 0.175f;

	// Token: 0x04000D86 RID: 3462
	protected float m_thrust_Exit_AnimationSpeed = 1f;

	// Token: 0x04000D87 RID: 3463
	protected float m_thrust_Exit_Delay;

	// Token: 0x04000D88 RID: 3464
	protected float m_thrust_Attack_ReverseSpeed = -3f;

	// Token: 0x04000D89 RID: 3465
	protected float m_thrust_Attack_ForwardDuration = 0.625f;

	// Token: 0x04000D8A RID: 3466
	protected float m_thrust_Attack_ForwardSpeed = 28f;

	// Token: 0x04000D8B RID: 3467
	protected float m_thrust_Exit_AttackCD = 1f;

	// Token: 0x04000D8C RID: 3468
	protected float m_thrustFast_ExitIntro_AnimationSpeed = 1.25f;

	// Token: 0x04000D8D RID: 3469
	protected float m_vertSpin_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000D8E RID: 3470
	protected float m_vertSpin_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000D8F RID: 3471
	protected float m_vertSpin_TellIntroAndHold_Delay = 0.6f;

	// Token: 0x04000D90 RID: 3472
	protected float m_vertSpin_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000D91 RID: 3473
	protected float m_vertSpin_AttackIntro_Delay;

	// Token: 0x04000D92 RID: 3474
	protected const float m_vertSpin_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000D93 RID: 3475
	protected const float m_vertSpin_AttackHold_Delay = 0.5f;

	// Token: 0x04000D94 RID: 3476
	protected float m_vertSpin_ExitIntro_AnimationSpeed = 1f;

	// Token: 0x04000D95 RID: 3477
	protected const float m_vertSpin_ExitIntro_EndDelay = 0.15f;

	// Token: 0x04000D96 RID: 3478
	protected const float m_vertSpin_Exit_ForceIdle = 0.25f;

	// Token: 0x04000D97 RID: 3479
	protected float m_vertSpin_Exit_AttackCD = 5f;
}

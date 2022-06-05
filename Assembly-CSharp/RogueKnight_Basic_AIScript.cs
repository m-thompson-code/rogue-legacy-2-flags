using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001B4 RID: 436
public class RogueKnight_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000BF4 RID: 3060 RVA: 0x00007444 File Offset: 0x00005644
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"RogueKnightBounceBoltProjectile"
		};
	}

	// Token: 0x170005A6 RID: 1446
	// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x00003F6C File Offset: 0x0000216C
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.35f, 0.75f);
		}
	}

	// Token: 0x170005A7 RID: 1447
	// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x0000745A File Offset: 0x0000565A
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.5f, 0.85f);
		}
	}

	// Token: 0x170005A8 RID: 1448
	// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x0000746B File Offset: 0x0000566B
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.5f, 0.5f);
		}
	}

	// Token: 0x170005A9 RID: 1449
	// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected virtual float m_dash_TellIntro_AnimationSpeed
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x170005AA RID: 1450
	// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_TellHold_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005AB RID: 1451
	// (get) Token: 0x06000BFA RID: 3066 RVA: 0x0000566E File Offset: 0x0000386E
	protected virtual float m_dash_TellIntroAndHold_Delay
	{
		get
		{
			return 0.8f;
		}
	}

	// Token: 0x170005AC RID: 1452
	// (get) Token: 0x06000BFB RID: 3067 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float m_dash_RepeatTellIntroAndHold_Delay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170005AD RID: 1453
	// (get) Token: 0x06000BFC RID: 3068 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_AttackIntro_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005AE RID: 1454
	// (get) Token: 0x06000BFD RID: 3069 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005AF RID: 1455
	// (get) Token: 0x06000BFE RID: 3070 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_AttackHold_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005B0 RID: 1456
	// (get) Token: 0x06000BFF RID: 3071 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005B1 RID: 1457
	// (get) Token: 0x06000C00 RID: 3072 RVA: 0x00006780 File Offset: 0x00004980
	protected virtual float m_dash_Exit_AnimationSpeed
	{
		get
		{
			return 0.65f;
		}
	}

	// Token: 0x170005B2 RID: 1458
	// (get) Token: 0x06000C01 RID: 3073 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005B3 RID: 1459
	// (get) Token: 0x06000C02 RID: 3074 RVA: 0x00004520 File Offset: 0x00002720
	protected virtual float m_dash_Exit_ForceIdle
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x170005B4 RID: 1460
	// (get) Token: 0x06000C03 RID: 3075 RVA: 0x00005319 File Offset: 0x00003519
	protected virtual float m_dash_AttackCD
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x170005B5 RID: 1461
	// (get) Token: 0x06000C04 RID: 3076 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_raiseKnockbackDefenseWhileAttacking
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005B6 RID: 1462
	// (get) Token: 0x06000C05 RID: 3077 RVA: 0x00004762 File Offset: 0x00002962
	protected virtual int m_knockbackDefenseBoostOverride
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x170005B7 RID: 1463
	// (get) Token: 0x06000C06 RID: 3078 RVA: 0x0000747C File Offset: 0x0000567C
	protected virtual float m_dash_Attack_Duration
	{
		get
		{
			return 0.325f;
		}
	}

	// Token: 0x170005B8 RID: 1464
	// (get) Token: 0x06000C07 RID: 3079 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float m_dash_Attack_LockDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170005B9 RID: 1465
	// (get) Token: 0x06000C08 RID: 3080 RVA: 0x0000521E File Offset: 0x0000341E
	protected virtual float m_dash_Attack_Speed
	{
		get
		{
			return 22f;
		}
	}

	// Token: 0x170005BA RID: 1466
	// (get) Token: 0x06000C09 RID: 3081 RVA: 0x0000747C File Offset: 0x0000567C
	protected virtual float m_dash_RepeatAttack_Duration
	{
		get
		{
			return 0.325f;
		}
	}

	// Token: 0x170005BB RID: 1467
	// (get) Token: 0x06000C0A RID: 3082 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float m_dash_RepeatAttack_LockDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170005BC RID: 1468
	// (get) Token: 0x06000C0B RID: 3083 RVA: 0x0000521E File Offset: 0x0000341E
	protected virtual float m_dash_RepeatAttack_Speed
	{
		get
		{
			return 22f;
		}
	}

	// Token: 0x170005BD RID: 1469
	// (get) Token: 0x06000C0C RID: 3084 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float m_dash_EndGravity_Delay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170005BE RID: 1470
	// (get) Token: 0x06000C0D RID: 3085 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_dash_NumDashes
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x06000C0E RID: 3086 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual int m_dash_OddstoThrowVersusDashing
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x170005C0 RID: 1472
	// (get) Token: 0x06000C0F RID: 3087 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_throw_Exit_ForceIdle
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170005C1 RID: 1473
	// (get) Token: 0x06000C10 RID: 3088 RVA: 0x00007483 File Offset: 0x00005683
	protected virtual float m_throw_AttackCD
	{
		get
		{
			return 3.5f;
		}
	}

	// Token: 0x170005C2 RID: 1474
	// (get) Token: 0x06000C11 RID: 3089 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_throw_Attack_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005C3 RID: 1475
	// (get) Token: 0x06000C12 RID: 3090 RVA: 0x000047A4 File Offset: 0x000029A4
	protected virtual int m_throw_Attack_ProjectileAmount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x170005C4 RID: 1476
	// (get) Token: 0x06000C13 RID: 3091 RVA: 0x0000452F File Offset: 0x0000272F
	protected virtual float m_throw_Attack_ProjectileDelay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x170005C5 RID: 1477
	// (get) Token: 0x06000C14 RID: 3092 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_throw_RepeatAttack_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005C6 RID: 1478
	// (get) Token: 0x06000C15 RID: 3093 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_throw_RepeatAttack_ProjectileAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170005C7 RID: 1479
	// (get) Token: 0x06000C16 RID: 3094 RVA: 0x0000452F File Offset: 0x0000272F
	protected virtual float m_throw_RepeatAttack_ProjectileDelay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x0006BDF4 File Offset: 0x00069FF4
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		Vector3 midpoint = base.EnemyController.Midpoint;
		midpoint.z = -3f;
		this.m_aimIndicator.transform.position = midpoint;
		this.m_aimIndicator.layer = 29;
		this.m_aimIndicator.SetActive(false);
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x0000748A File Offset: 0x0000568A
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator DashAttack()
	{
		base.EnemyController.ControllerCorgi.GravityActive(false);
		base.EnemyController.LockFlip = false;
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		this.m_aimIndicator.SetActive(true);
		float desiredRotation = CDGHelper.AngleBetweenPts(base.EnemyController.Midpoint, base.EnemyController.TargetController.Midpoint);
		Vector3 localEulerAngles = this.m_aimIndicator.transform.localEulerAngles;
		localEulerAngles.z = desiredRotation;
		this.m_aimIndicator.transform.localEulerAngles = localEulerAngles;
		yield return this.Default_TellIntroAndLoop("Dash_Tell_Intro", this.m_dash_TellIntro_AnimationSpeed, "Dash_Tell_Hold", this.m_dash_TellHold_AnimationSpeed, this.m_dash_TellIntroAndHold_Delay - this.m_dash_Attack_LockDelay);
		localEulerAngles = this.m_aimIndicator.transform.localEulerAngles;
		localEulerAngles.z = desiredRotation;
		this.m_aimIndicator.transform.localEulerAngles = localEulerAngles;
		this.m_aimIndicator.SetActive(false);
		if (this.m_dash_Attack_LockDelay > 0f)
		{
			yield return base.Wait(this.m_dash_Attack_LockDelay, false);
		}
		yield return this.Default_Animation("Dash_Attack_Intro", this.m_dash_AttackIntro_AnimationSpeed, this.m_dash_AttackIntro_Delay, true);
		yield return this.Default_Animation("Dash_Attack_Hold", this.m_dash_AttackHold_AnimationSpeed, this.m_dash_AttackHold_Delay, false);
		Vector2 vector = CDGHelper.AngleToVector(desiredRotation);
		if (this.m_raiseKnockbackDefenseWhileAttacking)
		{
			base.EnemyController.BaseKnockbackDefense = (float)this.m_knockbackDefenseBoostOverride;
		}
		base.EnemyController.ControllerCorgi.DisableOneWayCollision = true;
		base.EnemyController.DisableFriction = true;
		base.EnemyController.FallLedge = true;
		base.EnemyController.SetVelocity(vector.x * this.m_dash_Attack_Speed, vector.y * this.m_dash_Attack_Speed, false);
		if (this.m_dash_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_dash_Attack_Duration, false);
		}
		base.EnemyController.SetVelocity(0f, 0f, false);
		base.EnemyController.ControllerCorgi.DisableOneWayCollision = false;
		base.EnemyController.DisableFriction = false;
		base.EnemyController.FallLedge = false;
		if (UnityEngine.Random.Range(0, this.m_dash_OddstoThrowVersusDashing) == 0)
		{
			if (this.m_dash_NumDashes >= 2)
			{
				base.EnemyController.LockFlip = false;
				int num;
				for (int i = 1; i < this.m_dash_NumDashes; i = num + 1)
				{
					yield return this.DashRepeat();
					num = i;
				}
			}
			if (this.m_dash_EndGravity_Delay > 0f)
			{
				yield return base.Wait(this.m_dash_EndGravity_Delay, false);
			}
			base.EnemyController.LockFlip = false;
			base.EnemyController.ControllerCorgi.GravityActive(true);
			yield return this.Default_Animation("Dash_Exit", this.m_dash_Exit_AnimationSpeed, this.m_dash_Exit_Delay, true);
		}
		else
		{
			yield return this.ThrowRepeat();
			if (this.m_dash_EndGravity_Delay > 0f)
			{
				yield return base.Wait(this.m_dash_EndGravity_Delay, false);
			}
			base.EnemyController.LockFlip = false;
			base.EnemyController.ControllerCorgi.GravityActive(true);
			yield return this.Default_Animation("Throw_Exit", this.m_throw_Exit_AnimationSpeed, this.m_throw_Exit_Delay, true);
		}
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_dash_Exit_ForceIdle, this.m_dash_AttackCD);
		yield break;
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x00007499 File Offset: 0x00005699
	protected IEnumerator DashRepeat()
	{
		base.EnemyController.LockFlip = false;
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		this.m_aimIndicator.SetActive(true);
		float desiredRotation = CDGHelper.AngleBetweenPts(this.m_aimIndicator.transform.position, base.EnemyController.TargetController.Midpoint);
		Vector3 localEulerAngles = this.m_aimIndicator.transform.localEulerAngles;
		localEulerAngles.z = desiredRotation;
		this.m_aimIndicator.transform.localEulerAngles = localEulerAngles;
		yield return this.Default_TellIntroAndLoop("Dash_Tell_Intro", this.m_dash_TellIntro_AnimationSpeed, "Dash_Tell_Hold", this.m_dash_TellHold_AnimationSpeed, this.m_dash_RepeatTellIntroAndHold_Delay - this.m_dash_RepeatAttack_LockDelay);
		localEulerAngles = this.m_aimIndicator.transform.localEulerAngles;
		localEulerAngles.z = desiredRotation;
		this.m_aimIndicator.transform.localEulerAngles = localEulerAngles;
		this.m_aimIndicator.SetActive(false);
		if (this.m_dash_RepeatAttack_LockDelay > 0f)
		{
			yield return base.Wait(this.m_dash_RepeatAttack_LockDelay, false);
		}
		yield return this.Default_Animation("Dash_Attack_Intro", this.m_dash_AttackIntro_AnimationSpeed, this.m_dash_AttackIntro_Delay, true);
		yield return this.Default_Animation("Dash_Attack_Hold", this.m_dash_AttackHold_AnimationSpeed, this.m_dash_AttackHold_Delay, false);
		if (this.m_raiseKnockbackDefenseWhileAttacking)
		{
			base.EnemyController.BaseKnockbackDefense = (float)this.m_knockbackDefenseBoostOverride;
		}
		Vector2 vector = CDGHelper.AngleToVector(desiredRotation);
		base.EnemyController.SetVelocity(vector.x * this.m_dash_RepeatAttack_Speed, vector.y * this.m_dash_RepeatAttack_Speed, false);
		if (this.m_dash_RepeatAttack_Duration > 0f)
		{
			yield return base.Wait(this.m_dash_RepeatAttack_Duration, false);
		}
		base.EnemyController.SetVelocity(0f, 0f, false);
		base.EnemyController.LockFlip = false;
		yield break;
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x000074A8 File Offset: 0x000056A8
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ThrowAttack()
	{
		this.StopAndFaceTarget();
		if (!this.m_throw_Attack_TargetPlayer)
		{
			base.EnemyController.LockFlip = true;
		}
		yield return this.Default_TellIntroAndLoop("Throw_Tell_Intro", this.m_throw_TellIntro_AnimationSpeed, "Throw_Tell_Hold", this.m_throw_TellHold_AnimationSpeed, this.m_throw_TellIntroAndHold_Delay);
		yield return this.Default_Animation("Throw_Attack_Intro", this.m_throw_AttackIntro_AnimationSpeed, this.m_throw_AttackIntro_Delay, true);
		yield return this.Default_Animation("Throw_Attack_Hold", this.m_throw_AttackHold_AnimationSpeed, 0f, false);
		int num2;
		for (int i = 0; i < this.m_throw_Attack_ProjectileAmount; i = num2 + 1)
		{
			if (this.m_throw_Attack_TargetPlayer)
			{
				float num = Mathf.Atan2(base.EnemyController.TargetController.Midpoint.y - base.EnemyController.Midpoint.y, base.EnemyController.TargetController.Midpoint.x - base.EnemyController.Midpoint.x);
				num = CDGHelper.WrapAngleDegrees(CDGHelper.ToDegrees(num), false);
				this.FireProjectile("RogueKnightBounceBoltProjectile", 1, false, num, 1f, true, true, true);
			}
			else
			{
				this.FireProjectile("RogueKnightBounceBoltProjectile", 1, true, 0f, 1f, true, true, true);
			}
			if (this.m_throw_Attack_ProjectileAmount > 1 && this.m_throw_Attack_ProjectileDelay > 0f)
			{
				yield return base.Wait(this.m_throw_Attack_ProjectileDelay, false);
			}
			num2 = i;
		}
		yield return base.Wait(this.m_throw_AttackHold_Delay, false);
		yield return this.Default_Animation("Throw_Exit", this.m_throw_Exit_AnimationSpeed, this.m_throw_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_throw_Exit_ForceIdle, this.m_throw_AttackCD);
		yield break;
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x000074B7 File Offset: 0x000056B7
	protected IEnumerator ThrowRepeat()
	{
		this.StopAndFaceTarget();
		if (!this.m_throw_Attack_TargetPlayer)
		{
			base.EnemyController.LockFlip = true;
		}
		yield return this.Default_TellIntroAndLoop("Throw_Tell_Intro", this.m_throw_TellIntro_AnimationSpeed, "Throw_Tell_Hold", this.m_throw_TellHold_AnimationSpeed, this.m_throw_RepeatTellIntroAndHold_Delay);
		yield return this.Default_Animation("Throw_Attack_Intro", this.m_throw_AttackIntro_AnimationSpeed, this.m_throw_AttackIntro_Delay, true);
		yield return this.Default_Animation("Throw_Attack_Hold", this.m_throw_AttackHold_AnimationSpeed, 0f, false);
		int num2;
		for (int i = 0; i < this.m_throw_RepeatAttack_ProjectileAmount; i = num2 + 1)
		{
			if (this.m_throw_RepeatAttack_TargetPlayer)
			{
				float num = Mathf.Atan2(base.EnemyController.TargetController.Midpoint.y - base.EnemyController.Midpoint.y, base.EnemyController.TargetController.Midpoint.x - base.EnemyController.Midpoint.x);
				num = CDGHelper.WrapAngleDegrees(CDGHelper.ToDegrees(num), false);
				this.FireProjectile("RogueKnightBounceBoltProjectile", 1, false, num, 1f, true, true, true);
			}
			else
			{
				this.FireProjectile("RogueKnightBounceBoltProjectile", 1, true, 0f, 1f, true, true, true);
			}
			if (this.m_throw_RepeatAttack_ProjectileAmount > 1 && this.m_throw_Attack_ProjectileDelay > 0f)
			{
				yield return base.Wait(this.m_throw_RepeatAttack_ProjectileDelay, false);
			}
			num2 = i;
		}
		yield break;
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x0006BE4C File Offset: 0x0006A04C
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.EnemyController.ControllerCorgi.DisableOneWayCollision = false;
		base.EnemyController.DisableFriction = false;
		this.m_aimIndicator.SetActive(false);
		base.EnemyController.ControllerCorgi.GravityActive(true);
		base.EnemyController.LockFlip = false;
	}

	// Token: 0x04000E6F RID: 3695
	protected float m_throw_TellIntro_AnimationSpeed = 0.75f;

	// Token: 0x04000E70 RID: 3696
	protected float m_throw_TellHold_AnimationSpeed = 0.65f;

	// Token: 0x04000E71 RID: 3697
	protected float m_throw_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x04000E72 RID: 3698
	protected float m_throw_RepeatTellIntroAndHold_Delay = 0.5f;

	// Token: 0x04000E73 RID: 3699
	protected float m_throw_AttackIntro_AnimationSpeed = 2f;

	// Token: 0x04000E74 RID: 3700
	protected float m_throw_AttackIntro_Delay;

	// Token: 0x04000E75 RID: 3701
	protected float m_throw_AttackHold_AnimationSpeed = 2f;

	// Token: 0x04000E76 RID: 3702
	protected float m_throw_AttackHold_Delay = 0.25f;

	// Token: 0x04000E77 RID: 3703
	protected float m_throw_Exit_AnimationSpeed = 0.45f;

	// Token: 0x04000E78 RID: 3704
	protected float m_throw_Exit_Delay = 0.15f;

	// Token: 0x04000E79 RID: 3705
	[SerializeField]
	private GameObject m_aimIndicator;
}

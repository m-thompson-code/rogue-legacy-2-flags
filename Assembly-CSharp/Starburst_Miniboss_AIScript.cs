using System;
using System.Collections;

// Token: 0x02000130 RID: 304
public class Starburst_Miniboss_AIScript : Starburst_Basic_AIScript
{
	// Token: 0x17000510 RID: 1296
	// (get) Token: 0x0600096E RID: 2414 RVA: 0x0001ECBA File Offset: 0x0001CEBA
	protected override int NumberOfShots
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x17000511 RID: 1297
	// (get) Token: 0x0600096F RID: 2415 RVA: 0x0001ECBD File Offset: 0x0001CEBD
	protected override bool AlternateShots
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000512 RID: 1298
	// (get) Token: 0x06000970 RID: 2416 RVA: 0x0001ECC0 File Offset: 0x0001CEC0
	protected virtual int NumberOfCurse
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x0001ECC3 File Offset: 0x0001CEC3
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public IEnumerator ShootBoss()
	{
		yield return this.Default_TellIntroAndLoop("Spin_Tell_Intro", this.m_shoot_TellIntro_AnimationSpeed, "Spin_Tell_Hold", this.m_shoot_TellHold_AnimationSpeed, this.m_shoot_TellHold_Delay);
		yield return this.Default_Animation("Spin_Attack_Intro", this.m_shoot_AttackIntro_AnimationSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("Spin_Attack_Hold", this.m_shoot_AttackHold_AnimationSpeed, this.m_shoot_AttackHold_Delay, false);
		float num = 360f / (float)this.NumberOfCurse;
		float num2 = 0f;
		if (this.AlternateShots && this.AlternateCount == 1)
		{
			num2 = num + num / 2f;
			this.AlternateCount = 0;
		}
		else if (this.AlternateShots && this.AlternateCount == 0)
		{
			this.AlternateCount = 1;
		}
		for (int i = 0; i < this.NumberOfCurse; i++)
		{
			if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
			{
				this.FireProjectile("StarburstSlashBoltMinibossProjectile", 0, false, num * (float)i + num2, 1f, true, true, true);
			}
			else if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
			{
				this.FireProjectile("StarburstVoidProjectile", 0, false, num * (float)i + num2, 1f, true, true, true);
			}
			else
			{
				this.FireProjectile("StarburstBoltProjectile", 0, false, num * (float)i + num2, 1f, true, true, true);
			}
		}
		if (this.m_shoot_AttackHold_Delay > 0f)
		{
			base.Wait(this.m_shoot_AttackHold_Delay, false);
		}
		yield return this.Default_Animation("Spin_Exit", this.m_shoot_Exit_AnimationSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x17000513 RID: 1299
	// (get) Token: 0x06000972 RID: 2418 RVA: 0x0001ECD2 File Offset: 0x0001CED2
	protected virtual float m_dash_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000514 RID: 1300
	// (get) Token: 0x06000973 RID: 2419 RVA: 0x0001ECD9 File Offset: 0x0001CED9
	protected virtual float m_dash_TellIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000515 RID: 1301
	// (get) Token: 0x06000974 RID: 2420 RVA: 0x0001ECE0 File Offset: 0x0001CEE0
	protected virtual float m_dash_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000516 RID: 1302
	// (get) Token: 0x06000975 RID: 2421 RVA: 0x0001ECE7 File Offset: 0x0001CEE7
	protected virtual float m_dash_TellHold_Delay
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000517 RID: 1303
	// (get) Token: 0x06000976 RID: 2422 RVA: 0x0001ECEE File Offset: 0x0001CEEE
	protected virtual float m_dash_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x06000977 RID: 2423 RVA: 0x0001ECF5 File Offset: 0x0001CEF5
	protected virtual float m_dash_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x06000978 RID: 2424 RVA: 0x0001ECFC File Offset: 0x0001CEFC
	protected virtual float m_dash_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x06000979 RID: 2425 RVA: 0x0001ED03 File Offset: 0x0001CF03
	protected virtual float m_dash_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x0600097A RID: 2426 RVA: 0x0001ED0A File Offset: 0x0001CF0A
	protected virtual float m_dash_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x0600097B RID: 2427 RVA: 0x0001ED11 File Offset: 0x0001CF11
	protected virtual float m_dash_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x0600097C RID: 2428 RVA: 0x0001ED18 File Offset: 0x0001CF18
	protected virtual float m_dash_Exit_ForceIdle
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x0600097D RID: 2429 RVA: 0x0001ED1F File Offset: 0x0001CF1F
	protected virtual float m_dash_Exit_AttackCD
	{
		get
		{
			return 5.5f;
		}
	}

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x0600097E RID: 2430 RVA: 0x0001ED26 File Offset: 0x0001CF26
	protected virtual float m_dash_Attack_ForwardSpeedMod
	{
		get
		{
			return 4.5f;
		}
	}

	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x0600097F RID: 2431 RVA: 0x0001ED2D File Offset: 0x0001CF2D
	protected virtual float m_dash_Attack_Duration
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x06000980 RID: 2432 RVA: 0x0001ED34 File Offset: 0x0001CF34
	protected virtual bool m_dropsFireballsDuringDashAttack
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x06000981 RID: 2433 RVA: 0x0001ED37 File Offset: 0x0001CF37
	protected virtual float m_fireballDropDuringDashInterval
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x0001ED3E File Offset: 0x0001CF3E
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public IEnumerator DashAttack()
	{
		this.StopAndFaceTarget();
		yield return this.Default_Animation("Spin_Tell_Intro", this.m_dash_TellIntro_AnimSpeed, this.m_dash_TellIntro_Delay, false);
		yield return this.Default_Animation("Spin_Tell_Hold", this.m_dash_TellHold_AnimSpeed, this.m_dash_TellHold_Delay, false);
		base.EnemyController.Heading = base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint;
		base.EnemyController.LockFlip = true;
		base.EnemyController.FollowTarget = false;
		yield return this.Default_Animation("Spin_Attack_Intro", this.m_dash_AttackIntro_AnimSpeed, this.m_dash_AttackIntro_Delay, true);
		yield return this.Default_Animation("Spin_Attack_Hold", this.m_dash_AttackHold_AnimSpeed, this.m_dash_AttackHold_Delay, false);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = base.EnemyController.BaseSpeed * this.m_dash_Attack_ForwardSpeedMod;
		if (this.m_dash_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_dash_Attack_Duration, false);
		}
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		this.FireProjectile("StarburstSlashBoltMinibossProjectile", 0, false, 0f, 1f, true, true, true);
		this.FireProjectile("StarburstSlashBoltMinibossProjectile", 0, false, 180f, 1f, true, true, true);
		yield return this.Default_Animation("Spin_Exit", this.m_dash_Exit_AnimSpeed, this.m_dash_Exit_Delay, true);
		yield return this.Default_Attack_Cooldown(this.m_dash_Exit_ForceIdle, this.m_dash_Exit_AttackCD);
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		base.EnemyController.LockFlip = false;
		yield break;
	}

	// Token: 0x04000DC0 RID: 3520
	protected const string DASH_TELL_INTRO = "Spin_Tell_Intro";

	// Token: 0x04000DC1 RID: 3521
	protected const string DASH_TELL_HOLD = "Spin_Tell_Hold";

	// Token: 0x04000DC2 RID: 3522
	protected const string DASH_ATTACK_INTRO = "Spin_Attack_Intro";

	// Token: 0x04000DC3 RID: 3523
	protected const string DASH_ATTACK_HOLD = "Spin_Attack_Hold";

	// Token: 0x04000DC4 RID: 3524
	protected const string DASH_EXIT = "Spin_Exit";
}
